using ElevatorControl.Application.Abstractions;
using ElevatorControl.Application.Options;
using ElevatorControl.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElevatorControl.Application.Helpers
{
	public static class CarMoverHelper
	{
		public static void SetIdle(IElevatorState state, int carId, int floor)
		{
			state.SetCarState(carId, floor, Direction.Idle, false);
		}

		public static async Task HandleArrivalAsync(
			IElevatorState state,
			IEventLog log,
			ElevatorOptions opts,
			int carId,
			int floor,
			List<int> plan)
		{
			plan.RemoveAt(0);
			state.SetPlannedStops(carId, plan);

			state.SetCarState(carId, floor, Direction.Idle, true);
			log.Add($"{DateTime.Now:T}  Car {carId} opening doors at floor {floor}");

			await Task.Delay(TimeSpan.FromSeconds(opts.DoorSeconds));

			state.SetCarState(carId, floor, Direction.Idle, false);
			log.Add($"{DateTime.Now:T}  Car {carId} closing doors at floor {floor}");
		}

		public static async Task MoveCarAsync(
			IElevatorState state,
			IEventLog log,
			ElevatorOptions opts,
			int carId,
			int currentFloor,
			int targetFloor)
		{
			var dir = targetFloor > currentFloor ? Direction.Up : Direction.Down;
			state.SetCarState(carId, currentFloor, dir, false);

			log.Add($"{DateTime.Now:T}  Car {carId} moving {dir} from floor {currentFloor} to {targetFloor}");

			var step = dir == Direction.Up ? 1 : -1;
			var floor = currentFloor;

			while(floor != targetFloor) {
				await Task.Delay(TimeSpan.FromSeconds(opts.MoveSecondsPerFloor));
				floor += step;
				state.SetCarState(carId, floor, dir, false);
				log.Add($"{DateTime.Now:T}  Car {carId} passed floor {floor} going {dir}");
			}

			state.SetCarState(carId, floor, Direction.Idle, true);
			log.Add($"{DateTime.Now:T}  Car {carId} arrived at floor {floor} (doors opening)");

			// Drop current stop if still present
			var plan = state.GetPlannedStops(carId);
			if(plan.Count > 0 && plan[0] == floor) {
				plan.RemoveAt(0);
				state.SetPlannedStops(carId, plan);
			}

			await Task.Delay(TimeSpan.FromSeconds(opts.DoorSeconds));
			state.SetCarState(carId, floor, Direction.Idle, false);
			log.Add($"{DateTime.Now:T}  Car {carId} doors closed at floor {floor}");
		}
	}
}
