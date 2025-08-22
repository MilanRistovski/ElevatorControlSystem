using ElevatorControl.Application.Abstractions;
using ElevatorControl.Application.Options;
using ElevatorControl.Application.Services;
using ElevatorControl.Domain.Entities;
using ElevatorControl.Domain.Enums;
using Microsoft.Extensions.Options;
using Moq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace ElevatorControl.Tests.Unit
{
	public class CarMoverTests
	{
		private readonly Mock<IElevatorState> _stateMock;
		private readonly Mock<IEventLog> _logMock;
		private readonly CarMover _sut;
		private readonly ElevatorOptions _options;

		public CarMoverTests()
		{
			_stateMock = new Mock<IElevatorState>();
			_logMock = new Mock<IEventLog>();
			_options = new ElevatorOptions { MoveSecondsPerFloor = 0, DoorSeconds = 0 };
			_sut = new CarMover(_stateMock.Object, _logMock.Object, Options.Create(_options));
		}

		private Car SetupCar(int id, int floor, Direction direction, List<int> plan)
		{
			var car = new Car { Id = id, Floor = floor, Direction = direction, DoorsOpen = false };
			_stateMock.Setup(s => s.GetCars()).Returns(new[] { car });
			_stateMock.Setup(s => s.GetPlannedStops(id)).Returns(plan);
			return car;
		}

		[Fact]
		public async Task AssertRunning_ShouldMoveCarUp_WhenTargetAbove()
		{
			// Arrange
			var car = SetupCar(1, 0, Direction.Idle, new List<int> { 2 });
			int? lastFloor = null;
			Direction? lastDirection = null;
			bool? lastDoorsOpen = null;

			_stateMock.Setup(s => s.SetCarState(1, It.IsAny<int>(), It.IsAny<Direction>(), It.IsAny<bool>()))
				.Callback<int, int, Direction, bool>((carId, floor, dir, doors) => {
					lastFloor = floor;
					lastDirection = dir;
					lastDoorsOpen = doors;
					car.Floor = floor;
					car.Direction = dir;
					car.DoorsOpen = doors;
				});

			// Act
			_sut.EnsureRunning(1);
			await Task.Delay(50);

			// Assert
			Assert.Equal(2, lastFloor);
			Assert.Equal(Direction.Idle, lastDirection);
			Assert.False(lastDoorsOpen.Value);
		}

		[Fact]
		public async Task AssertRunning_ShouldMoveCarDown_WhenTargetBelow()
		{
			// Arrange
			var car = SetupCar(1, 8, Direction.Idle, new List<int> { 4 });
			int? lastFloor = null;
			Direction? lastDirection = null;

			_stateMock.Setup(s => s.SetCarState(1, It.IsAny<int>(), It.IsAny<Direction>(), It.IsAny<bool>()))
				.Callback<int, int, Direction, bool>((id, floor, dir, doors) => {
					lastFloor = floor;
					lastDirection = dir;
					car.Floor = floor;
					car.Direction = dir;
				});

			// Act
			_sut.EnsureRunning(1);
			await Task.Delay(50);

			// Assert
			Assert.Equal(4, lastFloor);
			Assert.Equal(Direction.Idle, lastDirection);
		}
	}
}
