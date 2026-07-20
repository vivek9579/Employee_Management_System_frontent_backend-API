namespace Employee_View.Models
{
    public class EmployeeQueryDTO
    {
        public string? Search { get; set; }
        public string? Sorting { get; set; }
        public bool Asc { get; set; } = true;
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 2;
        public int TotalRecords { get; set; }
        public int TotalPages =>
            (int)Math.Ceiling((double)TotalRecords / PageSize);
        public List<EmployeeDTO> Employees { get; set; } = new();
    }
}