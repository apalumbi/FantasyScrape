using System;
using FantasyScrape.DataRetrieval;
using FantasyScrape.Output;
using NHibernate.Cfg;
using FantasyScrape.DomainObjects;
using FantasyScrape.Persistence;
using FantasyScrape.Utilities;
using System.Collections.Generic;

namespace FantasyScrape {
	class Program {
		[STAThread]
		static void Main(string[] args) {
			DoWeeklyStuff();

			//DoAllTimeStuff();

			//DoDraftStuff();

			Console.WriteLine("Done!");
			Console.ReadLine();
		}

		private static void DoDraftStuff() {
			var dataRetriever = new LiveDataRetriever();
			var year = Keys.CurrentYear;

			var results = dataRetriever.GetSeasonResult(year);

			Console.WriteLine(results.ToString());

		}

		private static void DoWeeklyStuff() {
			var dataRetriever = new DataRetrievalManager();
			var year = Keys.CurrentYear;

			var results = dataRetriever.GetWeekResults(year);

			var printer = new Printer(results);

			Console.WriteLine(printer.StandingsInHTML);
		}

		private static void DoAllTimeStuff() {
			var allTimeDataManager = new AllTimeDataManager();

			var results = allTimeDataManager.GetData();

			var printer = new AllTimePrinter(results);

			Console.WriteLine(printer.UniqueTeamNames);
		}
	}
}