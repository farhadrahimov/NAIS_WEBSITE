namespace NAIS_Website.Models
{
    public class CatalogPagination
    {
        public List<CatalogViewModel> Catalogs { get; set; }
        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }

        public bool HasPreviousPage => CurrentPage > 1;
        public bool HasNextPage => CurrentPage < TotalPages;
    }
}
