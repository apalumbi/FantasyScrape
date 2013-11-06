using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;

namespace FantasyScrape.Utilities {
	public static class Logger {
		public static void Log(string textToLog) {
			var configOption = ConfigurationManager.AppSettings["logRequests"];
			if (configOption == "True") {
				Console.WriteLine("LOG: " + textToLog);
			}
		}
	}
}
