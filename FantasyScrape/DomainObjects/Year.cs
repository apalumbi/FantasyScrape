using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace FantasyScrape.DomainObjects {
	[XmlRoot(ElementName = "Year", IsNullable = false)]
	public class Year {
		public Year() { }
		public string LeagueKey { get; set; }
		public string theYear { get; set; }

		public Year(string leagueKey, string year) {
			this.LeagueKey = leagueKey;
			this.theYear = year;
		}

		public string WeekKey(string week) {
			return theYear + "_" + week + "_" + LeagueKey;
		}
	}
}
