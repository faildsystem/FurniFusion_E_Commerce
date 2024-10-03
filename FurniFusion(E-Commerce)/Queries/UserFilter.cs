namespace FurniFusion.Queries
{
    public class UserFilter
    {
        public string? Role { get; set; }
        public string? email { get; set; }
        public string? userName { get; set; }
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 5;
    }
}