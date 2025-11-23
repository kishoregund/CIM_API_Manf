using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using System.Data;
using System.Text;
using WebApi.DynamicQuery;

namespace WebApi.Controllers
{
    [ApiController]
    [Route("api/dynamic-query")]
    public class DynamicQueryController : ControllerBase
    {
        private readonly string _connString;
        public DynamicQueryController(IConfiguration cfg)
        {
            _connString = cfg.GetConnectionString("DefaultConnection")!;
        }

        [HttpPost("execute")]
        public async Task<IActionResult> Execute([FromBody] DynamicQueryRequest req)
        {
            // validate screens
            var sMetas = new List<ScreenMeta>();
            foreach (var sk in req.Screens.Distinct())
            {
                if (!QueryCatalog.Screens.TryGetValue(sk, out var sm)) return BadRequest($"Unknown screen: {sk}");
                sMetas.Add(sm);
            }
            if (!sMetas.Any()) return BadRequest("No screens selected.");

            // SELECT
            var selectParts = new List<string>();
            foreach (var c in req.Columns)
            {
                if (!QueryCatalog.Screens.TryGetValue(c.ScreenKey, out var sm) ||
                    !sm.Columns.TryGetValue(c.ColumnKey, out var cm))
                {
                    return BadRequest($"Unknown column: {c.ScreenKey}.{c.ColumnKey}");
                }
                var alias = $"{c.ScreenKey}_{c.ColumnKey}";
                selectParts.Add($"[{sm.Key}].{cm.Name} AS [{alias}]");
            }
            if (!selectParts.Any()) return BadRequest("No columns selected.");

            // FROM + JOINS
            var from = new StringBuilder();
            var main = sMetas[0];
            from.Append($"{main.Table} AS [{main.Key}]");

            foreach (var s in sMetas.Skip(1))
            {
                var join = req.Joins?.FirstOrDefault(j => j.LeftScreen == s.Key || j.RightScreen == s.Key);
                if (join is null) return BadRequest($"Missing join for screen: {s.Key}");

                if (!QueryCatalog.Screens.TryGetValue(join.LeftScreen, out var left) ||
                    !QueryCatalog.Screens.TryGetValue(join.RightScreen, out var right) ||
                    !left.Columns.TryGetValue(join.LeftColumn, out var lcol) ||
                    !right.Columns.TryGetValue(join.RightColumn, out var rcol))
                {
                    return BadRequest("Invalid join specification.");
                }

                var jt = join.Type.ToUpperInvariant() switch { "LEFT" => "LEFT JOIN", "RIGHT" => "RIGHT JOIN", _ => "INNER JOIN" };
                from.Append($" {jt} {right.Table} AS [{right.Key}] ON [{left.Key}].{lcol.Name} = [{right.Key}].{rcol.Name}");
            }

            // WHERE
            var whereParts = new List<string>();
            var parameters = new List<SqlParameter>();
            int p = 0;

            foreach (var f in req.Filters)
            {
                if (!QueryCatalog.Screens.TryGetValue(f.ScreenKey, out var sm) ||
                    !sm.Columns.TryGetValue(f.ColumnKey, out var cm))
                {
                    return BadRequest($"Invalid filter target: {f.ScreenKey}.{f.ColumnKey}");
                }
                var col = $"[{sm.Key}].{cm.Name}";
                var op = f.Operator.ToUpperInvariant();

                switch (op)
                {
                    case "IS NULL":
                    case "IS NOT NULL":
                        whereParts.Add($"{col} {op}");
                        break;
                    case "BETWEEN":
                        {
                            var vals = (f.Value as IEnumerable<object>)?.Cast<object>().ToArray() ?? Array.Empty<object>();
                            var p1 = new SqlParameter($"@p{p++}", vals.ElementAtOrDefault(0) ?? DBNull.Value);
                            var p2 = new SqlParameter($"@p{p++}", vals.ElementAtOrDefault(1) ?? DBNull.Value);
                            parameters.Add(p1); parameters.Add(p2);
                            whereParts.Add($"{col} BETWEEN {p1.ParameterName} AND {p2.ParameterName}");
                            break;
                        }
                    case "IN":
                        {
                            var list = (f.Value as IEnumerable<object>)?.ToList() ?? new List<object>();
                            if (!list.Any()) { whereParts.Add("1=0"); break; } // empty IN → match none
                            var names = new List<string>();
                            foreach (var v in list) { var sp = new SqlParameter($"@p{p++}", v ?? DBNull.Value); parameters.Add(sp); names.Add(sp.ParameterName); }
                            whereParts.Add($"{col} IN ({string.Join(",", names)})");
                            break;
                        }
                    case "LIKE":
                        {
                            var sp = new SqlParameter($"@p{p++}", f.Value?.ToString() ?? "");
                            parameters.Add(sp);
                            whereParts.Add($"{col} LIKE {sp.ParameterName}");
                            break;
                        }
                    default:
                        {
                            var sqlOp = op == "!=" ? "<>" : op;
                            var sp = new SqlParameter($"@p{p++}", f.Value ?? DBNull.Value);
                            parameters.Add(sp);
                            whereParts.Add($"{col} {sqlOp} {sp.ParameterName}");
                            break;
                        }
                }
            }

            // ORDER BY
            var orderBy = new List<string>();
            if (req.OrderBy != null)
            {
                foreach (var o in req.OrderBy)
                {
                    if (QueryCatalog.Screens.TryGetValue(o.ScreenKey, out var sm) &&
                        sm.Columns.TryGetValue(o.ColumnKey, out var cm))
                    {
                        var dir = o.Direction?.ToUpperInvariant() == "DESC" ? "DESC" : "ASC";
                        orderBy.Add($"[{sm.Key}].{cm.Name} {dir}");
                    }
                }
            }

            // LIMIT
            var top = (req.Limit is > 0) ? $"TOP {req.Limit} " : "";

            var sql = new StringBuilder();
            sql.Append("SELECT ").Append(top).Append(string.Join(", ", selectParts))
               .Append(" FROM ").Append(from.ToString());
            if (whereParts.Any()) sql.Append(" WHERE ").Append(string.Join(" AND ", whereParts));
            if (orderBy.Any()) sql.Append(" ORDER BY ").Append(string.Join(", ", orderBy));

            using var conn = new SqlConnection(_connString);
            await conn.OpenAsync();
            using var cmd = new SqlCommand(sql.ToString(), conn);
            cmd.Parameters.AddRange(parameters.ToArray());
            using var reader = await cmd.ExecuteReaderAsync(CommandBehavior.CloseConnection);

            var rows = new List<Dictionary<string, object?>>();
            while (await reader.ReadAsync())
            {
                var row = new Dictionary<string, object?>();
                for (int i = 0; i < reader.FieldCount; i++)
                {
                    row[reader.GetName(i)] = reader.IsDBNull(i) ? null : reader.GetValue(i);
                }
                rows.Add(row);
            }

            return Ok(rows);
        }
    }

}