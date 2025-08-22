using ElevatorControl.Application.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElevatorControl.Infrastructure.Logging
{
	public class InMemoryLog : IEventLog
	{
		private readonly object _lock = new();
		private readonly Queue<string> _lines = new();
		private readonly int _max = 200;

		public void Add(string line)
		{
			lock (_lock)
			{
				_lines.Enqueue(line);
				while (_lines.Count > _max)
					_lines.Dequeue();
			}
		}

		public IList<string> GetAll()
		{
			lock (_lock) { return new List<string>(_lines); }
		}
	}
}
