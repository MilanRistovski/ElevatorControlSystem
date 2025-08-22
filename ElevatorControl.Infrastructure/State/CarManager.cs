using ElevatorControl.Domain.Entities;
using ElevatorControl.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElevatorControl.Infrastructure.State
{
	public class CarManager
	{
		private readonly Dictionary<int, Car> _cars = new();

		public CarManager(int carCount, int initialFloor)
		{
			for(int i = 1; i <= carCount; i++) {
				_cars[i] = new Car {
					Id = i,
					Floor = initialFloor,
					Direction = Direction.Idle,
					DoorsOpen = false
				};
			}
		}

		public IEnumerable<Car> AllCars => _cars.Values;

		public Car? GetCar(int id) => _cars.TryGetValue(id, out var c) ? c : null;

		public void SetPlannedStops(int carId, List<int> stops)
		{
			if(_cars.TryGetValue(carId, out var c))
				c.Plan = stops ?? new List<int>();
		}

		public void UpdateCar(int carId, int floor, Direction direction, bool doorsOpen)
		{
			if(_cars.TryGetValue(carId, out var c)) {
				c.Floor = floor;
				c.Direction = direction;
				c.DoorsOpen = doorsOpen;
			}
		}
	}

}
