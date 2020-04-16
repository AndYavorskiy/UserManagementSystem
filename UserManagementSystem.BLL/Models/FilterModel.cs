namespace UserManagementSystem.BLL.Models
{
    public class FilterModel
    {
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
        public string FilterText { get; set; }
        public bool IncludeInactive { get; set; }
    }
}
