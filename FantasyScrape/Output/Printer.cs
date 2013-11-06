using System;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FantasyScrape.DomainObjects;

namespace FantasyScrape.Output {
	public class Printer {
		private readonly List<WeekResult> results;
		private string PlayoffTeam1 = "Wrongway";
		private string PlayoffTeam2 = "JoeyBangs";
		private string PlayoffTeam3 = "The E Team";
		private string PlayoffTeam4 = "Maclin On Your Girl";

		public Printer(List<WeekResult> results) {
			this.results = results;
		}

		public string Standings {
			get {
				var output = new StringBuilder();
				PrintStandings(output, PrintStandingsStartForConsole, PrintStandingsRowForConsole, () => "");
				return output.ToString();
			}
		}

		void PrintStandings(StringBuilder output, Func<string> printStart, Func<int, dynamic, string> printRow, Func<string> printEnd) {
			var totalWinsLookup = new Dictionary<string, int>();
			var totalPointsLookup = new Dictionary<string, decimal>();
			var lastResult = new StringBuilder();
			foreach (var result in results) {
				lastResult = new StringBuilder();
				CreateHeader(output, result.Week, "Standings");
				var start = printStart(); 
				output.AppendLine(start);
				lastResult.AppendLine(start);

				int wins = result.Teams.Count() - 1;
				var weekWinsAndPoints = from t in result.Teams
																orderby decimal.Parse(t.TotalPoints) descending
																select new { t.TeamName, Points = decimal.Parse(t.TotalPoints), Wins = wins-- };
				var enumerated = weekWinsAndPoints.ToList();

				foreach (var team in enumerated) {
					if (totalWinsLookup.ContainsKey(team.TeamName)) {
						totalWinsLookup[team.TeamName] += team.Wins;
					}
					else {
						totalWinsLookup.Add(team.TeamName, team.Wins);
					}

					if (totalPointsLookup.ContainsKey(team.TeamName)) {
						totalPointsLookup[team.TeamName] += team.Points;
					}
					else {
						totalPointsLookup.Add(team.TeamName, team.Points);
					}
				}

				IEnumerable<dynamic> totals = from t in enumerated
																			select new { t, TotalPoints = totalPointsLookup[t.TeamName], TotalWins = totalWinsLookup[t.TeamName] };

				int position = 1;
				foreach (var total in totals.OrderByDescending(w => w.TotalWins).ThenByDescending(w => w.TotalPoints)) {
					var row = printRow(position, total);
					output.AppendLine(row);
					lastResult.AppendLine(row);
					position++;
				}
			}
			Clipboard.SetText(lastResult.ToString());
		}

		string PrintStandingsStartForConsole() {
			return String.Format("{0}\t{1}\t{2}\t{3}\t{4}\t{5}", "Rank", "Team", "Total Points", "Total Wins", "Week Points", "Week Wins");
		}

		string PrintStandingsRowForConsole(int position, dynamic total) {
			return String.Format("{0}\t{1}\t{2}\t{3}\t{4}\t{5}", position, total.t.TeamName, total.TotalPoints, total.TotalWins, total.t.Points, total.t.Wins);
		}

		public string StandingsInHTML {
			get {
				var output = new StringBuilder();
				PrintStandings(output, PrintStandingsStartForHTML, PrintStandingsRowForHTML, PrintStandingsEndForHTML);
				return output.ToString();
			}
		}

		string PrintStandingsStartForHTML() {
			return String.Format("<table><tbody><tr><th width=\"16%\">Rank</th>{0}<th width=\"16%\">Team</th>{0}<th width=\"16%\">Total Points</th>{0}<th width=\"16%\">Total Wins</th>{0}<th width=\"16%\">Week Points</th>{0}<th width=\"16%\">Week Wins</th></tr>", 
													Environment.NewLine);
		}

		string PrintStandingsRowForHTML(int position, dynamic total) {
			return String.Format("<tr><td width=\"16%\">{0}</td>{6}<td dir=\"ltr\" width=\"16%\">{1}</td>{6}<td width=\"16%\">{2}</td>{6}<td width=\"16%\">{3}</td>{6}<td width=\"16%\">{4}</td>{6}<td width=\"16%\">{5}</td>{6}</tr>",
													position, total.t.TeamName, total.TotalPoints, total.TotalWins, total.t.Points, total.t.Wins, Environment.NewLine);
		}

		string PrintPlayoffStandingsStartForHTML() {
			return String.Format("<table><tbody><tr><th width=\"16%\">Rank</th>{0}<th width=\"16%\">Team</th>{0}<th width=\"16%\">Total Points</th>{0}<th width=\"16%\">Week Points</th></tr>",
													Environment.NewLine);
		}

		string PrintPlayoffStandingsRowForHTML(int position, dynamic total) {
			return String.Format("<tr><td width=\"16%\">{0}</td>{4}<td dir=\"ltr\" width=\"16%\">{1}</td>{4}<td width=\"16%\">{2}</td>{4}<td width=\"16%\">{3}</td>{4}</tr>",
													position, total.t.TeamName, total.TotalPoints, total.t.Points, Environment.NewLine);
		}

		string PrintStandingsEndForHTML() {
			return "</tbody></table>";
		}

		public string Averages {
			get {
				var output = new StringBuilder();
				CreateHeader(output, results.Last().Week, "Averages");
				var averages = from t in results.Last().Teams
											 select new { t.TeamName, t.AverageStarterPlayerScore, t.AverageBenchedPlayerScore };
				averages.ToList().ForEach(a => output.AppendLine(a.TeamName + ": Starter Average: " + a.AverageStarterPlayerScore + " Bench Average: " + a.AverageBenchedPlayerScore));
				return output.ToString();
			}
		}
				
		void CreateHeader(StringBuilder output, string week, string text) {
			output.AppendLine();
			output.AppendLine("Week " + week + " " + text);
			output.AppendLine("================");
		}

		public string PlayoffsStandingsInHTML {
			get {
				var output = new StringBuilder();
				PrintPlayoffStandings(output, PrintPlayoffStandingsStartForHTML, PrintPlayoffStandingsRowForHTML, PrintStandingsEndForHTML);
				return output.ToString();
			}
		}

		void PrintPlayoffStandings(StringBuilder output, Func<string> printStart, Func<int, dynamic, string> printRow, Func<string> printEnd) {
			var totalWinsLookup = new Dictionary<string, int>();
			var totalPointsLookup = new Dictionary<string, decimal>();
			foreach (var result in results.Where(w => int.Parse(w.Week) > 14)) {
				CreateHeader(output, result.Week, "Standings");
				output.AppendLine(printStart());

				int wins = 4 - 1;
				var weekWinsAndPoints = from t in result.Teams
																where t.TeamName == PlayoffTeam1 || t.TeamName == PlayoffTeam2 || t.TeamName == PlayoffTeam3 || t.TeamName == PlayoffTeam4
																orderby decimal.Parse(t.TotalPoints) descending
																select new { t.TeamName, Points = decimal.Parse(t.TotalPoints), Wins = wins-- };
				var enumerated = weekWinsAndPoints.ToList();

				foreach (var team in enumerated) {
					if (totalWinsLookup.ContainsKey(team.TeamName)) {
						totalWinsLookup[team.TeamName] += team.Wins;
					}
					else {
						totalWinsLookup.Add(team.TeamName, team.Wins);
					}

					if (totalPointsLookup.ContainsKey(team.TeamName)) {
						totalPointsLookup[team.TeamName] += team.Points;
					}
					else {
						totalPointsLookup.Add(team.TeamName, team.Points);
					}
				}

				IEnumerable<dynamic> totals = from t in enumerated
																			select new { t, TotalPoints = totalPointsLookup[t.TeamName], TotalWins = totalWinsLookup[t.TeamName] };

				int position = 1;
				foreach (var total in totals.OrderByDescending(w => w.TotalPoints)) {
					output.AppendLine(printRow(position, total));
					position++;
				}
			}
		}

		public string PlayoffStandings {
			get {
				var output = new StringBuilder();
				PrintPlayoffStandings(output, PrintStandingsStartForConsole, PrintStandingsRowForConsole, () => "");
				return output.ToString();
			}
		}
	}
}
