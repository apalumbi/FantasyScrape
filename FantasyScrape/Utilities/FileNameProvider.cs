using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;

namespace FantasyScrape.Utilities {
	public static class FileNameProvider {
		static string Extension = ".dat";
		public static string GetWeekFileName(string weekKey) {
			return ConfigurationManager.AppSettings["ResultsDirectory"] + "\\" + weekKey.Trim() + Extension;
		}

		public static string GetDraftFileName() {
			return ConfigurationManager.AppSettings["ResultsDirectory"] + "\\draft" + Extension;
		}
	}
}
