using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace WebApiTestTaskFromVK.Models
{
	public class User
	{
		public int Id { get; set; }
		[Required]
		[StringLength(15, MinimumLength = 4, ErrorMessage = "The length of the login must be from 4 to 15 characters.")]
		public string? Login { get; set; }
		[Required]
		[MinLength(4, ErrorMessage = "The password must contain at least 4 characters.")]
		public string? Password { get; set; }
		public DateTime? CreatedDate { get; set; }
		public int UserGroupId { get; set; }

		public int UserStateId { get; set; }
	}
}
