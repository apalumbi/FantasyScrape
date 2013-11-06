using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FantasyScrape.DomainObjects;
using FantasyScrape.Persistence;
using FantasyScrape.Utilities;
using System.Configuration;

namespace FantasyScrape.DataRetrieval {
	public class AllTimeDataManager {

		public List<WeekResult> GetData() {
			var repository = new ResultSerializer();
			var storedDataRetriever = new StoredDataRetriever();
			var weeks = ConfigurationManager.AppSettings["weeks"].Split(',');
			var results = new List<WeekResult>();

			foreach (var year in Keys.AllYears) {
				foreach (var week in repository.AllWeeks) {
					if (repository.HasWeekData(year.WeekKey(week))) {
						results.Add(storedDataRetriever.GetWeekResult(week, year, repository));
					}
				}
			}
			
			return results;
		}
	}
}
