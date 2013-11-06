using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using Rhino.Mocks;
using FantasyScrape.Persistence;
using FantasyScrape.DataRetrieval;
using FantasyScrape.DomainObjects;
using FantasyScrape.Utilities;

namespace FantasyScrapeTest.DataRetrieval {
	[TestFixture]
	public class DataRetrievalManagerTest {

		[Test]
		public void GetWeekResults_StoredData() {
			var mock = new MockRepository();
			var repository = mock.StrictMock<IRepository>();
			var liveRetriever = mock.StrictMock<IDataRetriever>();
			var storedRetriever = mock.StrictMock<IDataRetriever>();

			var year = Keys.CurrentYear;
			using (mock.Record()) {
				Expect.Call(repository.HasWeekData(year.WeekKey("1"))).Return(true);
				Expect.Call(storedRetriever.GetWeekResult("1", year, repository)).Return(new WeekResult());
				Expect.Call(repository.CurrentWeeks).Return(new List<string> { "1" });
			}

			using (mock.Playback()) {
				new DataRetrievalManager(storedRetriever, liveRetriever, repository).GetWeekResults(year);
			}
		}

		[Test]
		public void GetWeekResults_LiveData() {
			var mock = new MockRepository();
			var repository = mock.StrictMock<IRepository>();
			var liveRetriever = mock.StrictMock<IDataRetriever>();
			var storedRetriever = mock.StrictMock<IDataRetriever>();

			var year = Keys.CurrentYear;
			using (mock.Record()) {
				Expect.Call(repository.HasWeekData(year.WeekKey("1"))).Return(false);
				var result = new WeekResult();
				Expect.Call(liveRetriever.GetWeekResult("1", year, repository)).Return(result);
				repository.Save(year, result);
				Expect.Call(repository.CurrentWeeks).Return(new List<string> { "1" });
			}

			using (mock.Playback()) {
				new DataRetrievalManager(storedRetriever, liveRetriever, repository).GetWeekResults(year);
			}
		}
	}
}
