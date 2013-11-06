using System;
using System.Collections.Generic;
using System.IO;
using FantasyScrape.DomainObjects;
using FantasyScrape.Utilities;
using FantasyScrape.Persistence;

namespace FantasyScrape.DataRetrieval {
	public class StoredDataRetriever : IDataRetriever{
		
		public WeekResult GetWeekResult(string week, Year year, IRepository repository) {
			Console.WriteLine("Loading Week " + week + " results from disk...");
			return repository.FindByWeek(year.WeekKey(week));
		}

		public List<SeasonResult> GetSeasonResult(Year year) {
			throw new NotSupportedException("not yet");
		}
	}
}
