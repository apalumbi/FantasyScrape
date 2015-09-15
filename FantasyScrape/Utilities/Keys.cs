using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using FantasyScrape.DomainObjects;

namespace FantasyScrape.Utilities {
	public static class Keys {
		public static readonly Year year2001 = new Year("57.l.174634", "2001");
		public static readonly Year year2002 = new Year("49.l.23936", "2002");
		public static readonly Year year2003 = new Year("79.l.268140", "2003");
		public static readonly Year year2004 = new Year("102.l.1497", "2004");
		public static readonly Year year2005 = new Year("125.l.4490", "2005");
		public static readonly Year year2006 = new Year("154.l.9279", "2006");
		public static readonly Year year2007 = new Year("176.l.19530", "2007");
		public static readonly Year year2008 = new Year("200.l.20684", "2008");
		public static readonly Year year2009 = new Year("223.l.24153", "2009");
		public static readonly Year year2010 = new Year("242.l.735867", "2010");
		public static readonly Year year2011 = new Year("257.l.841121", "2011");
		public static readonly Year year2012 = new Year("273.l.658205", "2012");
		public static readonly Year year2013 = new Year("314.l.920757", "2013");
		public static readonly Year year2014 = new Year("331.l.1068435", "2014");
        public static readonly Year year2015 = new Year("348.l.109874", "2015");
		//select * from fantasysports.games where game_key="331"  -- YQL to get game key

		public static List<Year> AllYears { get { return new List<Year> { year2001, year2002, year2003, year2004, year2005, year2006, year2007, year2008, year2009, year2010, year2011, year2012, year2013, year2014, year2015 }; } }

		public static Year CurrentYear { get { return year2015; } }
	}
}
