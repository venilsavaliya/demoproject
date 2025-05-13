using BLL.Helper;
using DemoWeb.BLL.Interface;
using DemoWeb.DAL.Models;
using DemoWeb.DAL.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace DemoWeb.BLL.Services;

public class UserService : IuserService
{
    private readonly ApplicationDbContext _context;
    public UserService(ApplicationDbContext context)
    {
        _context = context;
    }
    public async Task<string> GetUserNameById(int Id)
    {
        User? user = await _context.Users.FirstOrDefaultAsync(i => i.Id == Id);

        if (user != null)
        {
            return user.Name;
        }

        return "Unknown";
    }

    public async Task<List<UserDetailViewModel>> GetUsersList()
    {
        return await _context.Users.Where(i => i.Isdeleted != true).Select(i => new UserDetailViewModel
        {
            Id = i.Id,
            Name = i.Name,
            Surname = i.Surname
        }).ToListAsync();
    }

    public async Task<UserDetailViewModel> GetUserDetailById(int Id)
    {
        if (Id == 0)
        {
            return new UserDetailViewModel();
        }

        User? user = await _context.Users.FirstOrDefaultAsync(i => i.Id == Id);

        if (user != null)
        {
            return new UserDetailViewModel
            {
                Id = user.Id,
                Name = user.Name,
                Surname = user.Surname
            };
        }
        else
        {
            return new UserDetailViewModel();
        }
    }

    public async Task<Response> AddUser(UserDetailViewModel user)
    {
        User newUser = new User
        {
            Name = user.Name,
            Surname = user.Surname
        };

        await _context.Users.AddAsync(newUser);

        await _context.SaveChangesAsync();

        return new Response
        {
            Success = true,
            Message = "User Added Successfully"
        };
    }

    public async Task<Response> EditUser(UserDetailViewModel user)
    {
        User? existingUser = await _context.Users.FirstOrDefaultAsync(i => i.Id == user.Id);

        if (existingUser != null)
        {
            existingUser.Name = user.Name;
            existingUser.Surname = user.Surname;

            await _context.SaveChangesAsync();

            return new Response
            {
                Success = true,
                Message = "User Edited Successfully"
            };
        }
        else
        {
            return new Response
            {
                Success = false,
                Message = "Error in Editing User!"
            };
        }


    }

    public async Task<Response> DeleteUser(int Id)
    {
        User? existingUser = await _context.Users.FirstOrDefaultAsync(i => i.Id == Id);

        if (existingUser == null)
        {
            return new Response
            {
                Message = "User not Found!",
                Success = false
            };
        }

        existingUser.Isdeleted = true;

        await _context.SaveChangesAsync();

        return new Response
        {
            Success = true,
            Message = "User Deleted Successfully!"
        };
    }

    public UserListPaginationViewModel GetPaginatedUserList(string sortColumn, string sortOrder, int pageNumber, int pageSize, string searchKeyword)
    {
        searchKeyword = searchKeyword.ToLower();

        UserListPaginationViewModel model = new() { Page = new() };

        var query = from u in _context.Users
                    where u.Isdeleted == false
                    select new UserDetailViewModel
                    {
                        Id = u.Id,
                        Name = u.Name,
                        Surname = u.Surname
                    };

        if (!string.IsNullOrEmpty(searchKeyword))
        {
            query = query.Where(u =>
                                 u.Name.ToLower().Contains(searchKeyword) ||
                                 u.Surname.ToLower().Contains(searchKeyword));
        }

        // Sorting logic
        if (!string.IsNullOrEmpty(sortColumn) && !string.IsNullOrEmpty(sortOrder))
        {
            switch (sortColumn)
            {
                case "Name":
                    query = sortOrder == "asc" ? query.OrderBy(u => u.Name) : query.OrderByDescending(u => u.Name);
                    break;
                case "Id":
                    query = sortOrder == "asc" ? query.OrderBy(u => u.Id) : query.OrderByDescending(u => u.Id);
                    break;
                default:
                    query = query.OrderBy(u => u.Id); // **Ensure a default sorting**
                    break;
            }
        }
        else
        {
            query = query.OrderBy(u => u.Id); // **Apply a default ordering if no sort is provided**
        }

        // Pagination
        int totalCount = query.Count();

        var usersList = query.Skip((pageNumber - 1) * pageSize)
                             .Take(pageSize)
                             .ToList();

        model.Items = usersList;
        model.SortColumn = sortColumn;
        model.SortOrder = sortOrder;
        model.Page.SetPagination(totalCount, pageSize, pageNumber);

        return model;

    }
}
