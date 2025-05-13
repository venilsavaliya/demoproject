using System.Threading.Tasks;
using DemoWeb.BLL;
using DemoWeb.BLL.Interface;
using DemoWeb.DAL.ViewModels;
using Microsoft.AspNetCore.Mvc;
namespace DemoWeb.Controllers;

public class UserController : Controller
{
    private readonly IuserService _userService;
    public UserController(IuserService userService)
    {
        _userService = userService;
    }

    public IActionResult Index()
    {
        return View();
    }

    public async Task<string> GetUserNameById(int Id)
    {
        return await _userService.GetUserNameById(Id);
    }

    public async Task<IActionResult> GetUserDetailPV()
    {   
        List<UserDetailViewModel> data = await _userService.GetUsersList();

        return PartialView("_UserDetail",data);
    }
    public async Task<IActionResult> GetAddEditUserForm(int Id)
    {   
        UserDetailViewModel data = await _userService.GetUserDetailById(Id);

        return PartialView("_AddEditUser",data);
    }

    [HttpPost]
    public async Task<IActionResult> AddEditUser(UserDetailViewModel user)
    {   
        Response response;

        if(user.Id != 0)
        {
            response = await _userService.EditUser(user);
        }
        else
        {
            response = await _userService.AddUser(user);
        }

        return Json(response);
    }


    public async Task<IActionResult> DeleteUser(int Id)
    {   
        Response response = await _userService.DeleteUser(Id);

        return Json(response);
    }
    
}
