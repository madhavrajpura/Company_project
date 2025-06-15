namespace DataAccessLayer.ViewModels;

public class MainViewModel
{
    public List<CategoryViewModel> categoryListVM { get; set; }
    public CategoryViewModel categoryVM { get; set; }
    public ProductViewModel productVM { get; set; }
    public PaginationViewModel<ProductViewModel> productListVM { get; set; }
}
