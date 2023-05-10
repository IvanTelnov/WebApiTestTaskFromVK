using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApiTestTaskFromVK.Models;
using WebApiTestTaskFromVK.Models.ViewModels;

namespace WebApiTestTaskFromVK.Controllers
{
    [Route("api/Main")]
    [ApiController]
    public class MainController : ControllerBase
    {
        private readonly AppDbContext _context;

        public MainController(AppDbContext context)
        {
            _context = context;
        }

		[HttpGet("GetUsers")]
		public async Task<ActionResult<IEnumerable<FullInfoViewModel>>> GetUsers(int? countOfUsers) //Получение всех или определенного количества пользователей, начиная с первого
        {
            if (_context.Users == null)
            {
                return NotFound();
            }

            List<User> users = await _context.Users.ToListAsync();
            List<FullInfoViewModel> fullInfos = new List<FullInfoViewModel>();
            UserGroup? userGroup;
            UserState? userState;
            foreach (var user in users)
            {
                if (user.UserStateId != 2 && user != null)
                {
                    userGroup = await _context.UserGroups.FindAsync(user?.UserGroupId);
                    userState = await _context.UserStates.FindAsync(user?.UserStateId);

                    fullInfos.Add(new FullInfoViewModel
                    {
                        User = user,
                        UserGroup = userGroup,
                        UserState = userState,
                    });
                    countOfUsers--;
                }
                if (countOfUsers == 0)
                    break;
            }
            return fullInfos;
        }

        [HttpGet("GetUserById/{id}")]
		public async Task<ActionResult<FullInfoViewModel>> GetUser(int id) // Получение пользователя по его ID
        {
            if (_context.Users == null)
            {
                return NotFound();
            }
            var user = await _context.Users.FindAsync(id);
            if (user?.UserStateId == 2 || user == null)
            {
                return Ok("This user does not exist.");
            }

            var userGroup = await _context.UserGroups.FindAsync(user?.UserGroupId);
            var userState = await _context.UserStates.FindAsync(user?.UserStateId);

            return new FullInfoViewModel { User = user, UserGroup = userGroup, UserState = userState };
        }

        [HttpGet("GetMultipleUsersById")]
		public async Task<ActionResult<IEnumerable<FullInfoViewModel>>> GetMultipleUsersById([FromQuery] int[] usersId) //Получения нескольких определенных пользователей по их ID 
		{                                                                                                               
			if (_context.Users == null || usersId == null)
            {
                return NotFound();
            }
            var users = new List<FullInfoViewModel>();
            foreach(int id in usersId)
            {
                var user = await _context.Users.FindAsync(id);
                if(user?.UserStateId != 2 && user != null)
                {
                    users.Add(new FullInfoViewModel
                    {
                        User = user,
                        UserGroup = await _context.UserGroups.FindAsync(user.UserGroupId),
                        UserState = await _context.UserStates.FindAsync(user.UserStateId)
                    });
                }
            }
            return users;
        }


        [HttpPost("AddUser")]
        public async Task<IActionResult> AddUser(AddUserModel user)
        {
            if (_context.Users == null)
                return NotFound();

            if (_context.Users.Any(x => x.Login == user.Login))
                return Ok(new JsonResult("A user with this username already exists."));

            _context.Users.Add(new User { 
                Login = user.Login, 
                Password = user.Password, 
                CreatedDate = DateTime.UtcNow, 
                UserGroupId = 2, 
                UserStateId = 1}
            );
            await _context.SaveChangesAsync();

            return Ok(new JsonResult("The user has been successfully created."));
        }

        [HttpDelete("DeleteUser/{id}")]
		public async Task<IActionResult> DeleteUser(int id)
        {
            if (_context.Users == null)
            {
                return NotFound();
            }
			if (id == 1)
				return Ok(new JsonResult("You can't delete an admin."));

			var user = await _context.Users.FindAsync(id);
            if (user?.UserStateId == 2 || user == null)
            {
                return Ok(new JsonResult("This user does not exist."));
            }

            user.UserStateId = 2;
            _context.Users.Update(user);
            await _context.SaveChangesAsync();

            return Ok(new JsonResult("The user has been deleted."));
        }

    }
}
