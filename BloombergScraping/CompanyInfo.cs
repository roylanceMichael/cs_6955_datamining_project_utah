using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BloombergScraping
{
	public class CompanyInfo
	{
		public string Url { get; set; }
		public string Open { get; set; }
		public string TodayRangeUpper { get; set; }
		public string TodayRangeLower { get; set; }
		public string Volume { get; set; }
		public string PrevClose { get; set; }
		public string YearRangeUpper { get; set; }
		public string YearRangeLower { get; set; }
		public string YearReturn { get; set; }

		public string CompanyHandle
		{
			get
			{
				if (Url == null)
				{
					return string.Empty;
				}

				var split = this.Url.Split('/');
				return split[split.Length - 1];
			}
		}

		public override string ToString()
		{
			return "Open: " + this.Open + " Today's Range: " + TodayRangeLower + "-" + TodayRangeUpper + " Volumn: "
						 + this.Volume + " PrevClose: " + this.PrevClose + " Year's Range: " + this.YearRangeLower + "-"
						 + this.YearRangeUpper + " YearReturn: " + this.YearReturn;
		}
	}
}
