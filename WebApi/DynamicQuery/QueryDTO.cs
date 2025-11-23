namespace WebApi.DynamicQuery
{
    public class DynamicQueryRequest
    {
        public List<string> Screens { get; set; } = new();
        public List<SelectedColumn> Columns { get; set; } = new();
        public List<QueryFilter> Filters { get; set; } = new();
        public List<JoinSpec>? Joins { get; set; }
        public List<OrderBySpec>? OrderBy { get; set; }
        public int? Limit { get; set; }
    }

    public class SelectedColumn
    {
        public string ScreenKey { get; set; } = "";
        public string ColumnKey { get; set; } = "";
    }

    public class QueryFilter
    {
        public string ScreenKey { get; set; } = "";
        public string ColumnKey { get; set; } = "";
        public string Operator { get; set; } = "=";
        public object? Value { get; set; }
    }

    public class JoinSpec
    {
        public string LeftScreen { get; set; } = "";
        public string LeftColumn { get; set; } = "";
        public string RightScreen { get; set; } = "";
        public string RightColumn { get; set; } = "";
        public string Type { get; set; } = "INNER";
    }

    public class OrderBySpec
    {
        public string ScreenKey { get; set; } = "";
        public string ColumnKey { get; set; } = "";
        public string Direction { get; set; } = "ASC";
    }

}
