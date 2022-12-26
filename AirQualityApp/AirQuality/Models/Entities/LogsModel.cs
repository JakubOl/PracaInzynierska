using System.ComponentModel.DataAnnotations;

namespace AirQuality.Models.Entities
{
	public class LogsModel
	{
		[Key]
		public int Id { get; set; }
		public string Message { get; set; }
	}
}
