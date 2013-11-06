using System.Collections.Generic;
using FantasyScrape.DomainObjects;
using NUnit.Framework;

namespace FantasyScrapeTest.DomainObjects {
	[TestFixture]
	public class TeamTest {
		[Test]
		public void AverageStarterPlayerScore() {
			var five = new Player { IsStarter = true, Points = "5" };
			var ten = new Player { IsStarter = true, Points = "10" };
			var fifteen = new Player { IsStarter = true, Points = "15" };
			var benched = new Player { IsStarter = false, Points = "33" };
			var roster = new List<Player> { five, ten, fifteen, benched };
			var team = new Team {Roster = roster};
			Assert.AreEqual("10.00", team.AverageStarterPlayerScore);
		}

		[Test]
		public void AverageBenchedPlayerScore() {
			var five = new Player { IsStarter = false, Points = "5" };
			var ten = new Player { IsStarter = false, Points = "10" };
			var fifteen = new Player { IsStarter = false, Points = "15" };
			var starter = new Player { IsStarter = true, Points = "33" };
			var roster = new List<Player> { five, ten, fifteen, starter };
			var team = new Team { Roster = roster };
			Assert.AreEqual("10.00", team.AverageBenchedPlayerScore);
		}
	}
}
