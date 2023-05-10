using WebApiTestTaskFromVK.Models;

namespace WebApiTestTaskFromVK.Models.ViewModels
{
    public class FullInfoViewModel
    {
        public User? User { get; set; }
        public UserGroup? UserGroup { get; set; }
        public UserState? UserState { get; set; }
    }
}
