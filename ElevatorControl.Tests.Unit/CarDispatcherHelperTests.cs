using ElevatorControl.Application.Helpers;
using ElevatorControl.Domain.Entities;
using ElevatorControl.Domain.Enums;
using System.Collections.Generic;
using Xunit;

namespace ElevatorControl.Tests.Unit
{
	public class CarDispatcherHelperTests
	{
		[Fact]
		public void SelectMostAppropriateCar_ShouldPickClosestCar()
		{
			// Arrange
			var cars = new List<Car>
			{
				new Car { Id = 1, Floor = 0, Direction = Direction.Idle },
				new Car { Id = 2, Floor = 5, Direction = Direction.Idle }
			};

			// Act
			var result = CarDispatcherHelper.SelectMostAppropriateCar(cars, 2, Direction.Up);

			// Assert
			Assert.Equal(1, result);
		}

		[Theory]
		[InlineData(2, Direction.Up, 0, Direction.Up, 2)]
		[InlineData(2, Direction.Up, 0, Direction.Down, 4)] 
		[InlineData(5, Direction.Down, 2, Direction.Idle, 3)]
		public void Score_ShouldCalculateCorrectly(int reqFloor, Direction reqDir, int carFloor, Direction carDir, int expected)
		{
			// Act
			var score = CarDispatcherHelper.Score(reqFloor, reqDir, carFloor, carDir);

			// Assert
			Assert.Equal(expected, score);
		}

		[Fact]
		public void OptimizePlan_ShouldOrderAscending_WhenGoingUp()
		{
			// Arrange
			var plan = new List<int> { 5, 2, 4, 2 };

			// Act
			var result = CarDispatcherHelper.OptimizePlan(plan, 0, Direction.Up);

			// Assert
			Assert.Equal(new List<int> { 2, 4, 5 }, result);
		}

		[Fact]
		public void OptimizePlan_ShouldOrderDescending_WhenGoingDown()
		{
			// Arrange
			var plan = new List<int> { 1, 4, 3, 4 };

			// Act
			var result = CarDispatcherHelper.OptimizePlan(plan, 0, Direction.Down);

			// Assert
			Assert.Equal(new List<int> { 4, 3, 1 }, result);
		}

		[Fact]
		public void OptimizePlan_ShouldOrderByClosestFloor_WhenIdle()
		{
			// Arrange
			var plan = new List<int> { 8, 3, 5 };
			int carFloor = 4;

			// Act
			var result = CarDispatcherHelper.OptimizePlan(plan, carFloor, Direction.Idle);

			// Assert
			Assert.Equal(new List<int> { 3, 5, 8 }, result);
		}
	}
}
