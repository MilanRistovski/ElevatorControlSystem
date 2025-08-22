using ElevatorControl.Domain.Entities;
using ElevatorControl.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElevatorControl.Application.Helpers
{
	public static class CarDispatcherHelper
	{
		public static int SelectMostAppropriateCar(IEnumerable<Car> cars, int floor, Direction direction)
		{
			return cars
				.Select(c => new { c.Id, Cost = Score(floor, direction, c.Floor, c.Direction) })
				.OrderBy(x => x.Cost)
				.First().Id;
		}

		public static int Score(int requestFloor, Direction requestDir, int carFloor, Direction carDir)
		{
			return Math.Abs(carFloor - requestFloor) +
				   (carDir != Direction.Idle && carDir != requestDir ? 2 : 0);
		}

		public static List<int> OptimizePlan(List<int> currentPlan, int carFloor, Direction carDirection)
		{
			if(carDirection == Direction.Up)
				return currentPlan.Distinct().OrderBy(x => x).ToList();
			else if(carDirection == Direction.Down)
				return currentPlan.Distinct().OrderByDescending(x => x).ToList();
			else
				return currentPlan.Distinct().OrderBy(x => Math.Abs(x - carFloor)).ToList();
		}
	}
}
