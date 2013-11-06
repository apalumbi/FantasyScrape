using System;
using System.Xml.Serialization;

namespace FantasyScrape.DomainObjects {

	[XmlRoot(ElementName = "Player", IsNullable = false)]
	public class Player : IComparable {
		public string TeamKey { get; set; }
		public string PlayerKey { get; set; }
		public string Position { get; set; }
		public string NFLTeam { get; set; }
		public string Points { get; set; }
		public string Name { get; set; }
		public bool IsStarter { get; set; }
		
		public virtual decimal PointsValue { get { return decimal.Parse(Points); } }
		
		public override string ToString() {
			return Name + " (" + Position + " for " + NFLTeam + ") - " + Points + " - " + (IsStarter ? "Active" : "Bench");
		}

		public virtual int CompareTo(object obj) {
			var that = obj as Player;
			if (that == null) {
				throw new ArgumentException("not a player");
			}
			if (IsStarter == that.IsStarter) {
				return PointsValue.CompareTo(that.PointsValue) * -1;
			}

			if (IsStarter && !that.IsStarter) {
				return -1;
			}
			return 1;
		}
	}
}