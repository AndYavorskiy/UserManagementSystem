namespace AuthorizationService.Models
{
    public class DataPagedModel<T>
    {
        public T[] Items { get; set; }
        public int TotalCount { get; set; }
    }
}
