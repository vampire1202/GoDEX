using System;
using System.IO;
using System.Text.RegularExpressions;
namespace Maticsoft.Common.Mime
{
	public static class QuotedPrintableEncoding
	{
		private const string Equal = "=";
		private const string HexPattern = "(\\=([0-9A-F][0-9A-F]))";
		public static string Decode(string contents)
		{
			if (contents == null)
			{
				throw new ArgumentNullException("contents");
			}
			string result;
			using (StringWriter stringWriter = new StringWriter())
			{
				using (StringReader stringReader = new StringReader(contents))
				{
					string text;
					while ((text = stringReader.ReadLine()) != null)
					{
						text.TrimEnd(new char[0]);
						if (text.EndsWith("="))
						{
							stringWriter.Write(QuotedPrintableEncoding.DecodeLine(text));
						}
						else
						{
							stringWriter.WriteLine(QuotedPrintableEncoding.DecodeLine(text));
						}
					}
				}
				stringWriter.Flush();
				result = stringWriter.ToString();
			}
			return result;
		}
		private static string DecodeLine(string line)
		{
			if (line == null)
			{
				throw new ArgumentNullException("line");
			}
			Regex regex = new Regex("(\\=([0-9A-F][0-9A-F]))", RegexOptions.IgnoreCase);
			return regex.Replace(line, new MatchEvaluator(QuotedPrintableEncoding.HexMatchEvaluator));
		}
		private static string HexMatchEvaluator(Match m)
		{
			int value = Convert.ToInt32(m.Groups[2].Value, 16);
			return Convert.ToChar(value).ToString();
		}
	}
}
