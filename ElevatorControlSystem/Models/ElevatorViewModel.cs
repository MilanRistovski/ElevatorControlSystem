using ElevatorControl.Domain.Entities;

namespace ElevatorControl.Web.Models
{
	public class ElevatorViewModel
	{
		public int Floors { get; set; }
		public List<Car> Cars { get; set; } = new();
		public IList<string> Logs { get; set; } = new List<string>();
	}
}
