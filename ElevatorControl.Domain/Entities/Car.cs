using ElevatorControl.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElevatorControl.Domain.Entities
{
	public class Car
	{
		public int Id { get; set; }
		public int Floor { get; set; }
		public Direction Direction { get; set; }
		public bool DoorsOpen { get; set; }
		public List<int> Plan { get; set; } = new();
	}
}
