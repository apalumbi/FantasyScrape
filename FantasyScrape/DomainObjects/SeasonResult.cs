using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace FantasyScrape.DomainObjects {
	[XmlRoot(ElementName = "SeasonResult", IsNullable = false)]
	public class SeasonResult {
		public string PlayerKey { get; set; }
		public string Position { get; set; }
		public string NFLTeam { get; set; }
		public string Points { get; set; }
		public string Name { get; set; }

		public virtual decimal PointsValue { get { return decimal.Parse(Points); } }

	}
}
