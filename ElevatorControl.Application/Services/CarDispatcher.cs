using ElevatorControl.Application.Abstractions;
using ElevatorControl.Application.Helpers;
using ElevatorControl.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElevatorControl.Application.Services.cs
{
	public class CarDispatcher : IElevatorDispatcher
	{
		private readonly IElevatorState _state;

		public CarDispatcher(IElevatorState state) => _state = state;


		public void AssignHallCall(int floor, Direction direction)
		{
			var cars = _state.GetCars();
			if(!cars.Any())
				return;

			var bestCarId = CarDispatcherHelper.SelectMostAppropriateCar(cars, floor, direction);

			var plan = _state.GetPlannedStops(bestCarId);
			if(!plan.Contains(floor))
				plan.Add(floor);

			var carNow = cars.First(c => c.Id == bestCarId);
			plan = CarDispatcherHelper.OptimizePlan(plan, carNow.Floor, carNow.Direction);

			_state.SetPlannedStops(bestCarId, plan);
		}
	}
}
