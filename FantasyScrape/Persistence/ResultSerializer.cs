using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using FantasyScrape.DomainObjects;
using FantasyScrape.Utilities;

namespace FantasyScrape.Persistence {
	public class ResultSerializer : IRepository {
		public void Save(Year year, WeekResult weekResult) {
			using (var sw = new StreamWriter(FileNameProvider.GetWeekFileName(year.WeekKey(weekResult.Week))))
				sw.Write(Serialize(weekResult));
		}

		static string Serialize<T>(T result) {
			var xs = new XmlSerializer(typeof(T));
			var stream = new MemoryStream();
			var xmlTextWriter = new XmlTextWriter(stream, Encoding.UTF8);
			xs.Serialize(xmlTextWriter, result);
			stream = (MemoryStream)xmlTextWriter.BaseStream;

			return new UTF8Encoding().GetString(stream.ToArray());
		}

		public static T Deserialize<T>(string serializedData) {
			var xs = new XmlSerializer(typeof(T));
			var memoryStream = new MemoryStream(new UTF8Encoding().GetBytes(serializedData));
			new XmlTextWriter(memoryStream, Encoding.UTF8);

			return (T)xs.Deserialize(memoryStream);
		}

		public bool HasWeekData(string weekKey) {
			return File.Exists(FileNameProvider.GetWeekFileName(weekKey));
		}

		public WeekResult FindByWeek(string weekKey) {
			return ResultSerializer.Deserialize<WeekResult>(File.ReadAllText(FileNameProvider.GetWeekFileName(weekKey)));
		}

		IEnumerable<string> currentWeeks;
		public IEnumerable<string> CurrentWeeks {
			get {
				if (currentWeeks == null) {
					currentWeeks = ConfigurationManager.AppSettings["weeks"].Split(',');
				}
				return currentWeeks;
			}
		}

		public IEnumerable<string> AllWeeks {
			get { return new List<string> { "1", "2", "3", "4", "5", "6", "7", "8", "9", "10", "11", "12", "13", "14", "15", "16" }; }
		}
	}
}