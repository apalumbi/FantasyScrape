using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FantasyScrape.DomainObjects;
using FantasyScrape.Persistence;
using FantasyScrape.Utilities;

namespace FantasyScrape.DataRetrieval {
	public interface IDataRetriever {
		WeekResult GetWeekResult(string week, Year year, IRepository repository);
		List<SeasonResult> GetSeasonResult(Year year);
	}
}
