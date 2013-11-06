using System.Collections.Generic;
using System.Linq;
using System.Text;
using System;
using System.Xml.Serialization;

namespace FantasyScrape.DomainObjects {
	[XmlRoot(ElementName = "Team", IsNullable = false)]
	public class Team {
		public string WeekYear { get; set; }
		public string TeamKey { get; set; }
		public string TeamName { get; set; }
		public string TotalPoints { get; set; }

		public string TotalPointsNoNegD {
			get {
				var value = decimal.Parse(TotalPoints);
				foreach (var p in Roster) {
					if (p.IsStarter && (p.Position == "DEF" || p.Position == "K")) {
						value += -1 * p.PointsValue;
					}
				}
				return value.ToString();
			}
		}
		
		public List<Player> Roster { get; set; }

		public virtual string AverageStarterPlayerScore {
			get {
				return Roster.Where(p => p.IsStarter)
										 .Average(p => p.PointsValue)
										 .ToString("###.#0");
			}
		}

		public virtual string AverageBenchedPlayerScore {
			get {
				return Roster.Where(p => !p.IsStarter)
										 .Average(p => p.PointsValue)
										 .ToString("###.#0");
			}
		}
		
		public override string ToString() {
			var result = new StringBuilder();
			result.AppendLine("Team: " + TeamName + " - " + TotalPoints);
			Roster.ToList().Sort();
			Roster.ToList<Player>().ForEach(p => result.AppendLine("   " + p.ToString()));
			return result.ToString();
		}
	}
}
