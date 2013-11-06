using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace FantasyScrape.DomainObjects {
	public static class APIResponseTranslator {

		public static List<Team> BuildTeams(string response) {
			var doc = new XmlDocument();

			doc.LoadXml(response);

			var teams = new List<Team>();
			foreach (var nodeList in doc.DocumentElement.SelectNodes("/query/results").Cast<XmlElement>()) {
				foreach (XmlElement node in nodeList) {
					teams.Add(new Team {
						TeamName = node.GetElementsByTagName("name")[0].InnerText,
						TotalPoints = node.GetElementsByTagName("team_points")[0]["total"].InnerText,
						TeamKey = node.GetElementsByTagName("team_key")[0].InnerText,
					});
				}
			}

			return teams;
		}

		public static List<Player> BuildPlayers(string response) {
			var doc = new XmlDocument();

			doc.LoadXml(response);

			var players = new List<Player>();
			foreach (var nodeList in doc.DocumentElement.SelectNodes("/query/results").Cast<XmlElement>()) {
				foreach (XmlElement node in nodeList) {
					foreach (XmlNode player in node.GetElementsByTagName("players").Cast<XmlElement>().First().GetElementsByTagName("player")) {
						players.Add(new Player {
							PlayerKey = player["player_key"].InnerText,
							Name = player["name"]["full"].InnerText,
							Points = player["player_points"]["total"].InnerText,
							IsStarter = player["selected_position"]["position"].InnerText != "BN",
							Position = player["display_position"].InnerText,
							NFLTeam = player["editorial_team_full_name"].InnerText,
						});
					}
				}
			}
			return players;
		}

		public static List<SeasonResult> BuildSeasonResults(string response) {
			var doc = new XmlDocument();

			doc.LoadXml(response);

			var results = new List<SeasonResult>();
			foreach (var nodeList in doc.DocumentElement.SelectNodes("/query/results").Cast<XmlElement>()) {
				foreach (XmlElement player in nodeList) {
					results.Add(new SeasonResult {
						PlayerKey = player["player_key"].InnerText,
						Name = player["name"]["full"].InnerText,
						Points = player["player_points"]["total"].InnerText,
						Position = player["display_position"].InnerText,
						NFLTeam = player["editorial_team_full_name"].InnerText,
					});
				}
			}
			return results;
		}
	}
}
