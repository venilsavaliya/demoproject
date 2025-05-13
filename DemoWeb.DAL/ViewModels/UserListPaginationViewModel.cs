using DAL.ViewModels;

namespace DemoWeb.DAL.ViewModels;

public class UserListPaginationViewModel
{
    public List<UserDetailViewModel>? Items { get; set; } = new List<UserDetailViewModel>();

    public Pagination? Page { get; set; }

    public string? SortColumn { get; set; }

    public string? SortOrder { get; set; }
}
