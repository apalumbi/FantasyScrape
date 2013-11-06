using System.Collections.Generic;
using FantasyScrape.DomainObjects;
using NUnit.Framework;

namespace FantasyScrapeTest.DomainObjects {
	[TestFixture]
	public class PlayerTest {
		[Test]
		public void Sort() {
			var highPoints = new Player {Points = "19.88", IsStarter = true };
			var lowPoints = new Player { Points = "9.88", IsStarter = true };
			var noPoints = new Player { Points = "0.00", IsStarter = true  };
			var benched1 = new Player { Points = "33.00", IsStarter = false };
			var benched2 = new Player { Points = "33.00", IsStarter = false };

			var players = new List<Player> { noPoints, benched1, lowPoints, highPoints, benched2 };

			players.Sort();

			Assert.AreEqual(highPoints.Points, players[0].Points);
			Assert.AreEqual(lowPoints.Points, players[1].Points);
			Assert.AreEqual(noPoints.Points, players[2].Points);
			Assert.AreEqual(benched1.Points, players[3].Points);
			Assert.AreEqual(benched2.Points, players[4].Points);
		}
	}
}
