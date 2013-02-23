using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BloombergScraping
{
	using System.IO;
	using System.Text.RegularExpressions;
	using System.Threading;

	using Newtonsoft.Json;

	class Program
	{
		static void Main(string[] args)
		{
			BloombergFunc();
		}

		private static void BloombergFunc()
		{
			Console.WriteLine("Getting all the companies...");
			var res = Bloomberg.GetAllCompanies();

			//this is going to go once every three hours
			while (true)
			{
				var fileLocation = Directory.GetCurrentDirectory();
				var newDirectory = Path.Combine(fileLocation, DateTime.Now.ToFileTimeUtc().ToString());
				Directory.CreateDirectory(newDirectory);
				ProcessAllCompanies(res, newDirectory);
				Console.WriteLine("Complete! Now going to sleep for three hours...");
				//sleep for three hours...
				Thread.Sleep(new TimeSpan(0, 3, 0, 0, 0));
			}
		}

		static void ProcessAllCompanies(IEnumerable<string> urls, string newDirectory)
		{
			foreach (var item in urls)
			{
				Console.WriteLine("Processing " + item);
				var companyInfo = Bloomberg.GetCompanyInfo(item);

				if (companyInfo == null)
				{
					continue;
				}

				var serializedItem = JsonConvert.SerializeObject(companyInfo, Newtonsoft.Json.Formatting.Indented);
				using (var newFile = File.CreateText(Path.Combine(newDirectory, companyInfo.CompanyHandle.CleanseFileName() + ".json")))
				{
					newFile.Write(serializedItem);
					newFile.Close();
				}
			}
		}
	}

	public static class StringUtil
	{
		public static string CleanseFileName(this string fileName)
		{
			if (string.IsNullOrWhiteSpace(fileName))
			{
				return string.Empty;
			}

			return Regex.Replace(fileName, "[^0-9A-Za-z]", "_");
		}
	}
}
