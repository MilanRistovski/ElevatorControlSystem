using ElevatorControl.Application.Abstractions;
using ElevatorControl.Application.Helpers;
using ElevatorControl.Application.Options;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElevatorControl.Application.Services
{
	public class CarMover
	{
		private readonly IElevatorState _state;
		private readonly IEventLog _log;
		private readonly ElevatorOptions _opts;

		private readonly ConcurrentDictionary<int, Task> _running = new();

		public CarMover(IElevatorState state, IEventLog log, IOptions<ElevatorOptions> options)
		{
			_state = state;
			_log = log;
			_opts = options.Value;
		}

		public void EnsureRunning(int carId, CancellationToken cancellationToken = default)
		{
			_running.GetOrAdd(carId, _ => Task.Run(() => RunCarAsync(carId, cancellationToken)));
		}

		public async Task RunCarAsync(int carId, CancellationToken cancellationToken)
		{
			while(!cancellationToken.IsCancellationRequested) {
				var car = _state.GetCars().FirstOrDefault(c => c.Id == carId);
				if(car == null) {
					_running.TryRemove(carId, out _);
					return;
				}

				var plan = _state.GetPlannedStops(carId);
				if(plan.Count == 0) {
					CarMoverHelper.SetIdle(_state, carId, car.Floor);
					_running.TryRemove(carId, out _);
					return;
				}

				var target = plan[0];
				if(target == car.Floor) {
					await CarMoverHelper.HandleArrivalAsync(_state, _log, _opts, carId, car.Floor, plan);
					continue;
				}

				await CarMoverHelper.MoveCarAsync(_state, _log, _opts, carId, car.Floor, target);
			}
		}
	}
}
