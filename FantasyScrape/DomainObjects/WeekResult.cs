using System.Collections.Generic;
using System.Xml.Serialization;
using System;
using FantasyScrape.Utilities;

namespace FantasyScrape.DomainObjects {
	[XmlRoot(ElementName = "WeekResult", IsNullable = false)]
	public class WeekResult {
		public WeekResult() { }
		public Year Year { get; set; }
		public string Week { get; set; }
		public List<Team> Teams { get; set; }

		public override string ToString() {
			return "Week " + Week + " of " + Year;
		}
	}
}
