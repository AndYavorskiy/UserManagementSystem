namespace UserManagementSystem.BLL.Models
{

    public class FilterModel : PagedDataRequestModel
    {
        public string FilterText { get; set; }
        public bool IncludeInactive { get; set; }
    }
}
