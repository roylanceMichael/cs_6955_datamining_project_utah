using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BloombergScraping
{
	using System.Net;
	using System.Text.RegularExpressions;

	using HtmlAgilityPack;

	public static class Bloomberg
	{
		const string BloombergUrl = "http://www.bloomberg.com";
		static readonly List<string> CompanyUrls = new List<string>
				                           {
																		 BloombergUrl + "/markets/companies/a-z/0-9/",
					                           BloombergUrl + "/markets/companies/a-z/a/",
																		 BloombergUrl + "/markets/companies/a-z/b/",
																		 BloombergUrl + "/markets/companies/a-z/c/",
																		 BloombergUrl + "/markets/companies/a-z/d/",
																		 BloombergUrl + "/markets/companies/a-z/e/",
																		 BloombergUrl + "/markets/companies/a-z/f/",
																		 BloombergUrl + "/markets/companies/a-z/g/",
																		 BloombergUrl + "/markets/companies/a-z/h/",
																		 BloombergUrl + "/markets/companies/a-z/i/",
																		 BloombergUrl + "/markets/companies/a-z/j/",
																		 BloombergUrl + "/markets/companies/a-z/k/",
																		 BloombergUrl + "/markets/companies/a-z/l/",
																		 BloombergUrl + "/markets/companies/a-z/m/",
																		 BloombergUrl + "/markets/companies/a-z/n/",
																		 BloombergUrl + "/markets/companies/a-z/o/",
																		 BloombergUrl + "/markets/companies/a-z/p/",
																		 BloombergUrl + "/markets/companies/a-z/q/",
																		 BloombergUrl + "/markets/companies/a-z/r/",
																		 BloombergUrl + "/markets/companies/a-z/s/",
																		 BloombergUrl + "/markets/companies/a-z/t/",
																		 BloombergUrl + "/markets/companies/a-z/u/",
																		 BloombergUrl + "/markets/companies/a-z/v/",
																		 BloombergUrl + "/markets/companies/a-z/w/",
																		 BloombergUrl + "/markets/companies/a-z/x/",
																		 BloombergUrl + "/markets/companies/a-z/y/",
																		 BloombergUrl + "/markets/companies/a-z/z/"
				                           };

		public static List<string> GetAllCompanies()
		{
			using (var webClient = new WebClient())
			{
				var individualLinks = new List<string>();
				foreach (var webAddress in CompanyUrls)
				{
					var webPage = webClient.DownloadString(webAddress);
					var doc = new HtmlDocument();
					doc.LoadHtml(webPage);
					var res = doc.DocumentNode.SelectSingleNode("//table[@class='ticker_data']//tr");
					var nextSibling = res;

					if (res == null)
					{
						continue;
					}

					while (nextSibling != null)
					{
						var firstTd = nextSibling.SelectSingleNode("td[@class='name']/a");
						if (firstTd != null && firstTd.Attributes.Any() && firstTd.Attributes.First().Name == "href")
						{
							individualLinks.Add(BloombergUrl + firstTd.Attributes.First().Value);
						}
						nextSibling = nextSibling.NextSibling;
					}
				}

				return individualLinks;
			}
		}

		public static CompanyInfo GetCompanyInfo(string url)
		{
			using (var webClient = new WebClient())
			{
				string webPage;
				try
				{
					webPage = webClient.DownloadString(url);
				}
				catch (Exception)
				{
					return null;
				}

				var doc = new HtmlDocument();
				doc.LoadHtml(webPage);
				var res = doc.DocumentNode.SelectSingleNode("//table[@class='snapshot_table']");
				if (res != null)
				{
					var rawText = Regex.Split(res.InnerText.Trim(), "\\s+");
					if (rawText.Length > 19)
					{
						var ci = new CompanyInfo
						{
							Url = url,
							Open = rawText[1],
							TodayRangeLower = rawText[4],
							TodayRangeUpper = rawText[6],
							PrevClose = rawText[8],
							Volume = rawText[11],
							YearRangeLower = rawText[14],
							YearRangeUpper = rawText[16],
							YearReturn = rawText[19]
						};
						return ci;
					}
				}
			}
			return new CompanyInfo();
		}
	}
}
