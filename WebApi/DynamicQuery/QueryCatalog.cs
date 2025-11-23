namespace WebApi.DynamicQuery
{
    public class ScreenMeta
    {
        public string Key { get; set; } = "";
        public string Table { get; set; } = "";
        public Dictionary<string, ColumnMeta> Columns { get; set; } = new();
    }

    public class ColumnMeta
    {
        public string Key { get; set; } = "";
        public string Name { get; set; } = ""; // [Column]
        public string Type { get; set; } = ""; // string | number | date | boolean
    }

    public static class QueryCatalog
    {
        public static readonly Dictionary<string, ScreenMeta> Screens = new()
        {
            ["users"] = new ScreenMeta
            {
                Key = "users",
                Table = "[dbo].[Users]",
                Columns = new Dictionary<string, ColumnMeta>
                {
                    ["id"] = new ColumnMeta { Key = "id", Name = "[Id]", Type = "number" },
                    ["name"] = new ColumnMeta { Key = "name", Name = "[Name]", Type = "string" },
                    ["email"] = new ColumnMeta { Key = "email", Name = "[Email]", Type = "string" },
                    ["createdAt"] = new ColumnMeta { Key = "createdAt", Name = "[CreatedAt]", Type = "date" },
                    ["isActive"] = new ColumnMeta { Key = "isActive", Name = "[IsActive]", Type = "boolean" }
                }
            },
            ["orders"] = new ScreenMeta
            {
                Key = "orders",
                Table = "[dbo].[Orders]",
                Columns = new Dictionary<string, ColumnMeta>
                {
                    ["orderId"] = new ColumnMeta { Key = "orderId", Name = "[OrderId]", Type = "number" },
                    ["userId"] = new ColumnMeta { Key = "userId", Name = "[UserId]", Type = "number" },
                    ["amount"] = new ColumnMeta { Key = "amount", Name = "[Amount]", Type = "number" },
                    ["status"] = new ColumnMeta { Key = "status", Name = "[Status]", Type = "string" },
                    ["orderedAt"] = new ColumnMeta { Key = "orderedAt", Name = "[OrderedAt]", Type = "date" }
                }
            }
        };
    }

}
