using ElevatorControl.Application.Abstractions;
using ElevatorControl.Domain.Entities;
using ElevatorControl.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElevatorControl.Infrastructure.State
{
	public class ElevatorState : IElevatorState
	{
		private readonly object _lock = new();
		private readonly CarManager _carManager;
		private readonly int _floorCount;

		public ElevatorState(int floorCount = 10, int carCount = 4, int initialFloor = 1)
		{
			_floorCount = floorCount;
			_carManager = new CarManager(carCount, initialFloor);
		}

		public int FloorCount => _floorCount;

		public IEnumerable<Car> GetCars()
		{
			lock(_lock) {
				return _carManager.AllCars
					.OrderBy(c => c.Id)
					.ToList();
			}
		}

		public List<int> GetPlannedStops(int carId)
		{
			lock(_lock) {
				var car = _carManager.GetCar(carId);
				return car != null ? new List<int>(car.Plan) : new List<int>();
			}
		}

		public void SetPlannedStops(int carId, List<int> stops)
		{
			lock(_lock) {
				_carManager.SetPlannedStops(carId, stops);
			}
		}

		public void SetCarState(int carId, int currentFloor, Direction direction, bool doorsOpen)
		{
			lock(_lock) {
				_carManager.UpdateCar(carId, currentFloor, direction, doorsOpen);
			}
		}
	}
}
