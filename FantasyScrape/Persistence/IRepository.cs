using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FantasyScrape.DomainObjects;

namespace FantasyScrape.Persistence {
	public interface IRepository {
		void Save(Year year, WeekResult weekResult);
		bool HasWeekData(string weekKey);
		WeekResult FindByWeek(string weekKey);

		IEnumerable<string> CurrentWeeks { get; }
	}
}
