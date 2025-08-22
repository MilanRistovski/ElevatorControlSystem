using ElevatorControl.Domain.Entities;
using ElevatorControl.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElevatorControl.Application.Abstractions
{
	public interface IElevatorState
	{
		int FloorCount { get; }
		IEnumerable<Car> GetCars();
		List<int> GetPlannedStops(int carId);
		void SetPlannedStops(int carId, List<int> stops);
		void SetCarState(int carId, int currentFloor, Direction direction, bool doorsOpen);
	}
}
