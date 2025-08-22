using System;
using System.Linq;
using ElevatorControl.Application.Abstractions;
using ElevatorControl.Application.Options;
using ElevatorControl.Application.Services;
using ElevatorControl.Application.Services.cs;
using ElevatorControl.Domain.Enums;
using ElevatorControl.Web.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace ElevatorControl.Web.Controllers
{
	[Route("[controller]")]
	public class ElevatorController : Controller
	{
		private readonly IElevatorState _state;
		private readonly IElevatorDispatcher _scheduler;
		private readonly CarMover _mover;
		private readonly IEventLog _log;
		private readonly int _floors;

		public ElevatorController(
			IElevatorState state,
			IElevatorDispatcher scheduler,
			CarMover mover,
			IEventLog log,
			IOptions<ElevatorOptions> opts)
		{
			_state = state;
			_scheduler = scheduler;
			_mover = mover;
			_log = log;
			_floors = opts.Value.FloorCount;
		}

		[HttpGet("/")]
		public IActionResult Index()
		{
			var vm = new ElevatorViewModel
			{
				Floors = _floors,
				Cars = _state.GetCars().ToList(),
				Logs = _log.GetAll()
			};

			return View(vm);
		}

		// Hall call from a floor (Up/Down)
		[HttpPost("Hall")]
		public IActionResult Hall(int floor, Direction direction)
		{
			_scheduler.AssignHallCall(floor, direction);
			_log.Add($"{DateTime.Now:T}  Hall call: {direction} at floor {floor}");
			var cars = _state.GetCars();
			foreach (var car in cars)
			{
				if (_state.GetPlannedStops(car.Id).Count > 0)
				{
					_mover.EnsureRunning(car.Id);
				}
			}

			return RedirectToAction(nameof(Index));
		}

		// Car request: send a specific car to a floor
		[HttpPost("CarGo")]
		public IActionResult CarGo(int carId, int floor)
		{
			var plan = _state.GetPlannedStops(carId);

			if (!plan.Contains(floor))
			{
				plan.Add(floor);
				_state.SetPlannedStops(carId, plan);
			}

			_log.Add($"{DateTime.Now:T}  Car {carId} requested floor {floor}");
			_mover.EnsureRunning(carId);

			return RedirectToAction(nameof(Index));
		}
	}
}
