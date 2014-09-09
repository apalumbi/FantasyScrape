using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FantasyScrape.DomainObjects;

namespace FantasyScrape.Output {
	public class AllTimePrinter {
		private readonly List<WeekResult> results;

		public AllTimePrinter(List<WeekResult> results) {
			this.results = results;
		}

		public string UniqueTeamNames {
			get {
				var teams = new List<Team>();
				foreach (var result in results) {
					foreach (var team in result.Teams) {
						if (!teams.Exists(t => t.TeamName.ToUpper() == team.TeamName.ToUpper())) {
							teams.Add(team);
						}
					}
				}


				StringBuilder output = new StringBuilder();
				foreach (var team in teams.OrderBy(t => t.TeamName)) {
					output.AppendLine(team.TeamName);
				}

				output.AppendLine("Total number of unique names: " + teams.Count());

				return output.ToString();
			}
		}

		public string Grouped {
			get {
				var teams = new List<Team>();
				foreach (var result in results) {
					foreach (var team in result.Teams) {
						teams.Add(team);
					}
				}


				StringBuilder output = new StringBuilder();
				foreach (var team in teams.GroupBy(t => t.TeamKey, n => n.TeamName)) {
					output.AppendLine(team.Key);
					foreach (var g in team.Distinct()) {
						output.AppendLine("   " + g);
					}
				}


				return output.ToString();
			}
		}

		public string BenchMajors {
			get {


				var data = new Dictionary<string, string>();

				foreach (var result in results) {
					foreach (var team in result.Teams) {
						var numberOfBenchPlayers = 0;
						foreach (var player in team.Roster.Where(p => !p.IsStarter)) {
							if (player.PointsValue >= 20) {
								numberOfBenchPlayers++;
								if (numberOfBenchPlayers > 1) {
									if (data.ContainsKey(team.TeamName)) {
										data[team.TeamName] = team.TeamName + " had " + numberOfBenchPlayers + " players on week " + result.Week + " in " + result.Year.theYear;
									}
									else {
										data.Add(team.TeamName, team.TeamName + " had " + numberOfBenchPlayers + " players on week " + result.Week + " in " + result.Year.theYear);
									}
								}
							}
						}
					}
				}

				var output = new StringBuilder();
				foreach (var d in data.OrderBy(t => t.Key)) {
					output.AppendLine(d.Value);
				}
				return output.ToString();
			}
		}

		public string Week14Leaders {
			get {
				var leaders = new Dictionary<string, Team>();
				foreach (var item in results) {
					if (item.Week == "14") {
						foreach (var team in item.Teams) {
							if (!leaders.ContainsKey(item.Year.theYear)) {
								leaders.Add(item.Year.theYear, team);
								continue;
							}

							var currentLeader = leaders[item.Year.theYear];
							if (decimal.Parse(currentLeader.TotalPoints) < decimal.Parse(team.TotalPoints)) {
								leaders[item.Year.theYear] = team;
							}
						}
					}
				}

				var result = new StringBuilder();

				foreach (var item in leaders) {
					result.AppendLine(item.Key + " " + item.Value.TotalPoints);
				}

				return result.ToString();
			}

		}
	}
}
