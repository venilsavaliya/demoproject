using DemoWeb.DAL.ViewModels;

namespace DemoWeb.BLL.Interface;

public interface IuserService
{
    public Task<string> GetUserNameById(int id);

    public Task<List<UserDetailViewModel>> GetUsersList();

    public UserListPaginationViewModel GetPaginatedUserList(string sortColumn, string sortOrder, int pageNumber = 1, int pageSize = 2, string searchKeyword = "");

    public Task<UserDetailViewModel> GetUserDetailById(int Id);

    public Task<Response> AddUser(UserDetailViewModel user);

    public  Task<Response> EditUser(UserDetailViewModel user);

    public  Task<Response> DeleteUser(int Id);
}   
