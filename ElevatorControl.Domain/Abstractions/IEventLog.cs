using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElevatorControl.Application.Abstractions
{
	public interface IEventLog
	{
		void Add(string line);
		IList<string> GetAll();
	}
}
