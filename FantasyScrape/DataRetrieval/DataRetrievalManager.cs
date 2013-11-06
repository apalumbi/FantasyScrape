using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using FantasyScrape.DomainObjects;
using FantasyScrape.Utilities;
using FantasyScrape.Persistence;

namespace FantasyScrape.DataRetrieval {
	public class DataRetrievalManager {
		readonly IDataRetriever liveDataRetriever;
		readonly IDataRetriever storedDataRetriever;
		readonly IRepository repository;
		
		public DataRetrievalManager() : this(new StoredDataRetriever(), new LiveDataRetriever(), new ResultSerializer()) { }

		public DataRetrievalManager(IDataRetriever storedDataRetriever, IDataRetriever liveDataRetriever, IRepository repository) {
			this.liveDataRetriever = liveDataRetriever;
			this.storedDataRetriever = storedDataRetriever;
			this.repository = repository;
		}

		public List<WeekResult> GetWeekResults(Year year) {
			var results = new List<WeekResult>();
			foreach (var week in repository.CurrentWeeks) {
				if (repository.HasWeekData(year.WeekKey(week))) {
					results.Add(storedDataRetriever.GetWeekResult(week, year, repository));
				}
				else {
					var liveResult = liveDataRetriever.GetWeekResult(week, year, repository);
					repository.Save(year, liveResult);
					results.Add(liveResult);
				}
			}

			return results;
		}
	}
}
