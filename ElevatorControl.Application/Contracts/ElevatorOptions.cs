using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElevatorControl.Application.Options
{
	public class ElevatorOptions
	{
		//hardcoded because of testability 
		public int FloorCount { get; set; } = 10;
		public int CarCount { get; set; } = 4;
		public int MoveSecondsPerFloor { get; set; } = 10;
		public int DoorSeconds { get; set; } = 10;
		public int InitialFloor { get; set; } = 1;
	}
}
