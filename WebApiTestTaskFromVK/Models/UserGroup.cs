using System.Text.Json.Serialization;

namespace WebApiTestTaskFromVK.Models
{
	public class UserGroup
	{
	
		public int Id { get; set; }
		public string? Code { get; set; }
		public string? Description { get; set; }
	}
}
