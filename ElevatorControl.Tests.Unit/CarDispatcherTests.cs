using ElevatorControl.Application.Abstractions;
using ElevatorControl.Application.Services;
using ElevatorControl.Application.Services.cs;
using ElevatorControl.Domain.Entities;
using ElevatorControl.Domain.Enums;
using Moq;
using System.Collections.Generic;
using Xunit;

namespace ElevatorControl.Tests.Unit
{
	public class CarDispatcherTests
	{
		private readonly Mock<IElevatorState> _stateMock;
		private readonly CarDispatcher _sut;

		public CarDispatcherTests()
		{
			_stateMock = new Mock<IElevatorState>();
			_sut = new CarDispatcher(_stateMock.Object);
		}

		[Fact]
		public void Assert_AssignHallCall_ShouldAssignToClosestIdleCar()
		{
			// Arrange
			var cars = new List<Car>
			{
				new Car { Id = 1, Floor = 0, Direction = Direction.Idle },
				new Car { Id = 2, Floor = 5, Direction = Direction.Idle }
			};
			_stateMock.Setup(s => s.GetCars()).Returns(cars);
			_stateMock.Setup(s => s.GetPlannedStops(It.IsAny<int>())).Returns(new List<int>());

			List<int>? capturedPlan = null;
			_stateMock
				.Setup(s => s.SetPlannedStops(1, It.IsAny<List<int>>()))
				.Callback<int, List<int>>((id, plan) => capturedPlan = plan);

			// Act
			_sut.AssignHallCall(2, Direction.Up);

			// Assert
			Assert.NotNull(capturedPlan);
			Assert.Contains(2, capturedPlan);
		}

		[Fact]
		public void Assert_AssignHallCall_ShouldNotDuplicateStop()
		{
			// Arrange
			var cars = new List<Car> { new Car { Id = 1, Floor = 0, Direction = Direction.Idle } };
			_stateMock.Setup(s => s.GetCars()).Returns(cars);
			_stateMock.Setup(s => s.GetPlannedStops(1)).Returns(new List<int> { 3 });

			List<int>? capturedPlan = null;
			_stateMock
				.Setup(s => s.SetPlannedStops(1, It.IsAny<List<int>>()))
				.Callback<int, List<int>>((id, plan) => capturedPlan = plan);

			// Act
			_sut.AssignHallCall(3, Direction.Up);

			// Assert
			Assert.NotNull(capturedPlan);
			Assert.Single(capturedPlan);
			Assert.Equal(3, capturedPlan[0]);
		}

		[Fact]
		public void Assert_AssignHallCall_ShouldOptimizePlan_Upwards()
		{
			// Arrange
			var cars = new List<Car> { new Car { Id = 1, Floor = 2, Direction = Direction.Up } };
			_stateMock.Setup(s => s.GetCars()).Returns(cars);
			_stateMock.Setup(s => s.GetPlannedStops(1)).Returns(new List<int> { 5, 3 });

			List<int>? capturedPlan = null;
			_stateMock
				.Setup(s => s.SetPlannedStops(1, It.IsAny<List<int>>()))
				.Callback<int, List<int>>((id, plan) => capturedPlan = plan);

			// Act
			_sut.AssignHallCall(4, Direction.Up);

			// Assert
			Assert.NotNull(capturedPlan);
			Assert.Equal(new List<int> { 3, 4, 5 }, capturedPlan);
		}

		[Fact]
		public void Assert_AssignHallCall_ShouldOptimizePlan_Downwards()
		{
			// Arrange
			var cars = new List<Car> { new Car { Id = 1, Floor = 5, Direction = Direction.Down } };
			_stateMock.Setup(s => s.GetCars()).Returns(cars);
			_stateMock.Setup(s => s.GetPlannedStops(1)).Returns(new List<int> { 1, 3 });

			List<int>? capturedPlan = null;
			_stateMock
				.Setup(s => s.SetPlannedStops(1, It.IsAny<List<int>>()))
				.Callback<int, List<int>>((id, plan) => capturedPlan = plan);

			// Act
			_sut.AssignHallCall(4, Direction.Down);

			// Assert
			Assert.NotNull(capturedPlan);
			Assert.Equal(new List<int> { 4, 3, 1 }, capturedPlan);
		}
	}
}
