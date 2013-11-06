using System;
using System.Linq;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Net;
using System.Web;
using FantasyScrape.DomainObjects;
using FantasyScrape.Persistence;
using FantasyScrape.Utilities;
using System.Text;
using System.Diagnostics;
using System.Xml;

namespace FantasyScrape.DataRetrieval {

	public class LiveDataRetriever : IDataRetriever {
		static string ConsumerKey = "dj0yJmk9eGhEN3V4Y1YzTlF6JmQ9WVdrOU5YSlhha1J5TjJVbWNHbzlOak15T1RBMk9EWXkmcz1jb25zdW1lcnNlY3JldCZ4PTBk";
		static string ConsumerSecret = "e56da845a8fe81155d5149f361833fe64ebf5f33";

		Dictionary<string, string> responseDictionary = new Dictionary<string, string>();
		
		public WeekResult GetWeekResult(string week, Year year, IRepository repository) {
			Console.WriteLine("Getting Team Data For Week " + week + "...");

			Login();

			var result = new WeekResult { Week = week, Year = year };

			var teams = GetTeams(int.Parse(week), year);
			if (teams.Count == 0) {
				Console.WriteLine(week + " did not do something right");
				Console.ReadLine();
				throw new Exception();
			}
			foreach (var team in teams) {
				Console.WriteLine("Loading Team: " + team.TeamName);
				team.Roster = GetPlayerStats(team.TeamKey, int.Parse(result.Week), responseDictionary);
			}
			result.Teams = teams;

			return result;
		}

		Dictionary<string, string> Login() {
			responseDictionary = ReadResponseDictionary();
			var response = "";
			var testUrl = "http://query.yahooapis.com/v1/yql?&q=select * from fantasysports.draftresults where league_key='257.l.841121'";
			try {
				response = RunQuery(responseDictionary, testUrl);
			}
			catch {
				responseDictionary = Authenticate();
				response = RunQuery(responseDictionary, testUrl);

				WriteResponseDictionary(responseDictionary);
			}
			return responseDictionary;
		}

		string RunQuery(Dictionary<string, string> responseDictionary, string apiUrl) {
			Logger.Log(apiUrl);
			var response = "";
			var apiReq = System.Net.HttpWebRequest.Create(apiUrl);
			var header = string.Format("OAuth realm=\"yahooapis.com\",oauth_consumer_key=\"{0}\",oauth_nonce=\"{1}\",oauth_signature_method=\"PLAINTEXT\",oauth_timestamp=\"{2}\",oauth_token=\"{3}\",oauth_version=\"1.0\",oauth_signature=\"{4}%26{5}\"",
				ConsumerKey, GenerateNonce(), GenerateTimeStamp(), responseDictionary["oauth_token"], ConsumerSecret, responseDictionary["oauth_token_secret"]);
			apiReq.Headers.Add("Authorization", header);

			using (var sr = new StreamReader(apiReq.GetResponse().GetResponseStream())) {
				response = sr.ReadToEnd();
			}
			return response;
		}

		Dictionary<string, string> Authenticate() {
			var url = String.Format("https://api.login.yahoo.com/oauth/v2/get_request_token?oauth_nonce={0}&oauth_timestamp={1}&oauth_consumer_key={2}&oauth_signature_method=plaintext&oauth_signature={3}%26&oauth_version=1.0&xoauth_lang_pref=en-us&oauth_callback=oob",
															GenerateNonce(), GenerateTimeStamp(), ConsumerKey, ConsumerSecret);

			string token_response = "";
			var req = System.Net.HttpWebRequest.Create(url);
			using (var sr = new StreamReader(req.GetResponse().GetResponseStream())) {
				token_response = sr.ReadToEnd();
			}

			var token_responses = token_response.Split('&');

			var tokenDictionary = new Dictionary<string, string>();
			foreach (var t in token_responses) {
				var items = t.Split('=');
				tokenDictionary.Add(items[0], items[1]);
			}

			url = String.Format("https://api.login.yahoo.com/oauth/v2/request_auth?oauth_token={0}&oauth_nonce={1}&oauth_timestamp={2}&oauth_consumer_key={3}&oauth_signature_method=plaintext&oauth_signature={4}%26&oauth_version=1.0&xoauth_lang_pref=en-us&oauth_callback=oob",
								tokenDictionary["oauth_token"], GenerateNonce(), GenerateTimeStamp(), ConsumerKey, ConsumerSecret);
						
			var chrome = "\"C:\\Users\\Anthony Palumbi\\AppData\\Local\\Google\\Chrome\\Application\\chrome.exe\"";
			Process.Start(chrome, "-url " + url);
			

			Console.WriteLine("Enter Auth:");
			var auth_verifier = Console.ReadLine();
			url = String.Format("https://api.login.yahoo.com/oauth/v2/get_token?oauth_consumer_key={0}&oauth_signature_method=PLAINTEXT&oauth_version=1.0&oauth_verifier={1}&oauth_token={2}&oauth_nonce={3}&oauth_timestamp={4}&oauth_signature={5}%26{6}",
				ConsumerKey, auth_verifier, tokenDictionary["oauth_token"], GenerateNonce(), GenerateTimeStamp(), ConsumerSecret, tokenDictionary["oauth_token_secret"]);

			var authResponse = "";
			req = System.Net.HttpWebRequest.Create(url);
			using (var sr = new StreamReader(req.GetResponse().GetResponseStream())) {
				authResponse = sr.ReadToEnd();
			}

			var authResponses = authResponse.Split('&');
			var responseDictionary = new Dictionary<string, string>();
			foreach (var t in authResponses) {
				var items = t.Split('=');
				responseDictionary.Add(items[0], items[1]);
			}
			return responseDictionary;
		}

		string GenerateTimeStamp() {
			TimeSpan ts = DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, 0);
			return Convert.ToInt64(ts.TotalSeconds).ToString();
		}

		Random random = new Random();
		string GenerateNonce() {
			return random.Next(123400, 9999999).ToString();
		}
		
		void WriteResponseDictionary(Dictionary<string, string> responseDictionary) {
			var responseString = new StringBuilder();

			foreach (var p in responseDictionary) {
				responseString.AppendLine(p.Key + "," + p.Value);
			}

			File.WriteAllText("AuthResponse.txt", responseString.ToString());
		}

		Dictionary<string, string> ReadResponseDictionary() {
			if (!File.Exists("AuthResponse.txt")) {
				return new Dictionary<string, string>();
			}
			var lines = File.ReadAllLines("AuthResponse.txt");

			var dictionary = new Dictionary<string, string>();
			foreach (var line in lines) {
				var split = line.Split(',');
				dictionary.Add(split[0], split[1]);
			}
			return dictionary;
		}

		List<Team> GetTeams(int week, Year year) {
			var apiUrl = string.Format("http://query.yahooapis.com/v1/yql?&q=select * from fantasysports.teams.stats where stats_type = 'week' and stats_week = {0} and team_key in  (select team_key from fantasysports.teams where league_key= '{1}')", week, year.LeagueKey);
			var response = RunQuery(responseDictionary, apiUrl);
			return APIResponseTranslator.BuildTeams(response);
		}

		List<Player> GetPlayerStats(string teamKey, int week, Dictionary<string, string> responseDictionary) {
			var apiUrl = String.Format("http://query.yahooapis.com/v1/yql?&q=select * from fantasysports.teams.roster.stats where week = {0} and team_key = '{1}'", week, teamKey);
			var response = RunQuery(responseDictionary, apiUrl);
			return APIResponseTranslator.BuildPlayers(response);
		}

		public List<SeasonResult> GetSeasonResult(Year year) {

			Login();

			var apiUrl = string.Format("http://query.yahooapis.com/v1/yql?&q=select * from fantasysports.players.stats where league_key='{0}'", year.LeagueKey);
			var response = RunQuery(responseDictionary, apiUrl);
			return APIResponseTranslator.BuildSeasonResults(response);
		}
	}
}
