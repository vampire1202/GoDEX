using dotnetCHARTING;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
namespace Maticsoft.Common
{
	public class ChartHelper
	{
		public event SetXColumnHandler OnSetXColumn;
		public string SetXColumn(string ora_str)
		{
			if (this.OnSetXColumn != null)
			{
				return this.OnSetXColumn(ora_str);
			}
			return ora_str;
		}
		public void Create(Chart chart, string title, DataTable table, string xColumn, string yColumn, string style, bool user3D)
		{
			chart.set_Palette(new Color[]
			{
				Color.FromArgb(49, 255, 49),
				Color.FromArgb(255, 255, 0),
				Color.FromArgb(255, 99, 49),
				Color.FromArgb(0, 156, 255),
				Color.FromArgb(255, 125, 49),
				Color.FromArgb(125, 255, 49),
				Color.FromArgb(0, 255, 49)
			});
			chart.set_Use3D(user3D);
			SeriesCollection seriesCollection = this.getRandomData(table, xColumn, yColumn);
			if (string.IsNullOrEmpty(style) || style == "线形")
			{
				chart.set_Type(0);
				seriesCollection = this.getRandomData2(table, xColumn, yColumn);
			}
			else
			{
				if (style == "柱形")
				{
					chart.set_Type(0);
				}
				else
				{
					if (style == "金字塔")
					{
						chart.set_Type(15);
						chart.get_DefaultSeries().set_Type(3);
					}
					else
					{
						if (style == "圆锥")
						{
							chart.set_Type(15);
							chart.get_DefaultSeries().set_Type(2);
						}
					}
				}
			}
			chart.set_Title(title);
			if (string.IsNullOrEmpty(style) || style == "线形")
			{
				chart.get_DefaultSeries().set_Type(3);
			}
			chart.get_DefaultElement().set_ShowValue(true);
			chart.set_PieLabelMode(1);
			chart.set_ShadingEffectMode(3);
			chart.get_NoDataLabel().set_Text("没有数据显示");
			chart.get_SeriesCollection().Add(seriesCollection);
		}
		public void Create(Chart chart, string title, List<DataTable> tables, List<DateTime> dates, string xColumn, string yColumn, string style, bool user3D, string targetUrl)
		{
			chart.set_Palette(new Color[]
			{
				Color.FromArgb(49, 255, 49),
				Color.FromArgb(255, 255, 0),
				Color.FromArgb(255, 99, 49),
				Color.FromArgb(0, 156, 255),
				Color.FromArgb(255, 125, 49),
				Color.FromArgb(125, 255, 49),
				Color.FromArgb(0, 255, 49)
			});
			chart.set_Use3D(user3D);
			chart.set_Type(0);
			chart.set_Title(title);
			chart.get_DefaultSeries().set_Type(3);
			SeriesCollection seriesCollection = new SeriesCollection();
			for (int i = 0; i < dates.Count; i++)
			{
				string text = dates[i].ToString("yyyy-MM-dd");
				Series series = new Series(text);
				foreach (DataRow dataRow in tables[i].Rows)
				{
					Element element = new Element(dataRow[xColumn].ToString());
					element.set_URLTarget("_self");
					element.get_LegendEntry().set_URL(targetUrl + text);
					element.get_LegendEntry().set_URLTarget("_self");
					element.set_URL(targetUrl + text);
					element.set_YValue(Convert.ToDouble(dataRow[yColumn]));
					series.get_Elements().Add(element);
					seriesCollection.Add(series);
				}
			}
			chart.get_DefaultElement().set_ShowValue(true);
			if (string.IsNullOrEmpty(style) || style == "线形")
			{
				chart.get_DefaultSeries().set_Type(3);
			}
			chart.set_PieLabelMode(1);
			chart.set_ShadingEffectMode(3);
			chart.get_NoDataLabel().set_Text("没有数据显示");
			chart.get_SeriesCollection().Add(seriesCollection);
		}
		private SeriesCollection getRandomData(DataTable table, string x, string y)
		{
			SeriesCollection seriesCollection = new SeriesCollection();
			foreach (DataRow dataRow in table.Rows)
			{
				Series series = new Series(dataRow[x].ToString());
				Element element = new Element(dataRow[x].ToString());
				element.set_YValue(Convert.ToDouble(dataRow[y]));
				series.get_Elements().Add(element);
				seriesCollection.Add(series);
			}
			return seriesCollection;
		}
		private SeriesCollection getRandomData2(DataTable table, string x, string y)
		{
			SeriesCollection seriesCollection = new SeriesCollection();
			Series series = new Series();
			foreach (DataRow dataRow in table.Rows)
			{
				Element element = new Element(dataRow[x].ToString());
				element.set_YValue(Convert.ToDouble(dataRow[y]));
				series.get_Elements().Add(element);
			}
			seriesCollection.Add(series);
			return seriesCollection;
		}
		public void Pie(Chart chart, int width, int height, string title, DataTable table, string xColumn, string yColumn)
		{
			SeriesCollection seriesCollection = new SeriesCollection();
			Series series = new Series("");
			DataView dataView = new DataView(table);
			dataView.Sort = yColumn + "  desc";
			int num = 0;
			DataTable dataTable = dataView.ToTable();
			Element element = new Element("其他");
			bool flag = false;
			double num2 = 0.0;
			foreach (DataRow dataRow in dataTable.Rows)
			{
				if (num > 9)
				{
					num2 += Convert.ToDouble(dataRow[yColumn].ToString());
					element.set_LabelTemplate("%PercentOfTotal");
					flag = true;
				}
				else
				{
					string text = dataRow[xColumn].ToString();
					text = this.SetXColumn(text);
					Element element2 = new Element(text);
					element2.set_LabelTemplate("%PercentOfTotal");
					element2.set_YValue(Convert.ToDouble(dataRow[yColumn].ToString()));
					series.get_Elements().Add(element2);
					num++;
				}
			}
			if (flag)
			{
				series.get_Elements().Add(element);
			}
			chart.get_TitleBox().set_Position(5);
			element.set_YValue(num2);
			seriesCollection.Add(series);
			chart.set_TempDirectory("temp");
			chart.set_Use3D(false);
			chart.get_DefaultAxis().set_FormatString("N");
			chart.get_DefaultAxis().set_CultureName("zh-CN");
			chart.set_Palette(new Color[]
			{
				Color.FromArgb(49, 255, 49),
				Color.FromArgb(255, 255, 0),
				Color.FromArgb(255, 99, 49),
				Color.FromArgb(0, 156, 255),
				Color.FromArgb(255, 156, 255),
				Color.FromArgb(0, 156, 0),
				Color.FromArgb(0, 156, 99),
				Color.FromArgb(0, 99, 255),
				Color.FromArgb(99, 156, 255),
				Color.FromArgb(0, 0, 99),
				Color.FromArgb(0, 156, 126)
			});
			chart.get_DefaultElement().get_SmartLabel().set_AutoWrap(true);
			chart.set_Type(4);
			chart.set_Size(width + "x" + height);
			chart.get_DefaultElement().get_SmartLabel().set_Text("");
			chart.set_Title(title);
			chart.get_DefaultElement().set_ShowValue(true);
			chart.set_PieLabelMode(1);
			chart.set_ShadingEffectMode(3);
			chart.get_DefaultElement().get_SmartLabel().set_AutoWrap(true);
			chart.get_NoDataLabel().set_Text("没有数据显示");
			chart.get_SeriesCollection().Add(seriesCollection);
		}
		public void Create(Chart chart, string title, DataTable table, string xColumn, string yColumn, string style, int displayNum)
		{
			SeriesCollection seriesCollection = new SeriesCollection();
			DataView dataView = new DataView(table);
			dataView.Sort = yColumn + "  desc";
			int num = 0;
			DataTable dataTable = dataView.ToTable();
			Element element = new Element("其他");
			bool flag = false;
			double num2 = 0.0;
			Color item = Color.FromArgb(49, 255, 49);
			Random random = new Random(255);
			Color item2 = Color.FromArgb(255, 49, 255);
			List<Color> list = new List<Color>();
			list.Add(item);
			list.Add(item2);
			for (int i = 0; i < displayNum; i++)
			{
				Color item3 = Color.FromArgb(((int)item.A + random.Next(10000)) % 255, ((int)item.B + random.Next(456)) % 255, ((int)item.G + random.Next(1027)) % 100);
				list.Add(item3);
			}
			foreach (DataRow dataRow in dataTable.Rows)
			{
				Series series = new Series("");
				if (num > displayNum - 2)
				{
					num2 += Convert.ToDouble(dataRow[yColumn].ToString());
					element.set_LabelTemplate("%PercentOfTotal");
					flag = true;
				}
				else
				{
					string text = dataRow[xColumn].ToString();
					text = this.SetXColumn(text);
					series.set_Name(text);
					Element element2 = new Element(text);
					element2.set_LabelTemplate("%PercentOfTotal");
					element2.get_SmartLabel().set_Text(text);
					element2.set_YValue(Convert.ToDouble(dataRow[yColumn].ToString()));
					series.get_Elements().Add(element2);
					num++;
					seriesCollection.Add(series);
				}
			}
			if (flag)
			{
				Series series2 = new Series("其他");
				series2.get_Elements().Add(element);
				seriesCollection.Add(series2);
			}
			element.set_YValue(num2);
			element.get_SmartLabel().set_Text("其他");
			chart.set_TempDirectory("temp");
			chart.set_Use3D(false);
			chart.get_DefaultAxis().set_FormatString("N");
			chart.get_DefaultAxis().set_CultureName("zh-CN");
			chart.set_Palette(list.ToArray());
			chart.get_DefaultElement().get_SmartLabel().set_AutoWrap(true);
			if (string.IsNullOrEmpty(style) || style == "线形")
			{
				chart.set_Type(0);
				chart.get_DefaultSeries().set_Type(3);
			}
			else
			{
				if (style == "柱形")
				{
					chart.set_Type(0);
				}
				else
				{
					if (style == "横柱形")
					{
						chart.set_Type(2);
					}
					else
					{
						if (style == "图片柱形")
						{
							chart.set_Type(0);
							chart.get_DefaultSeries().set_ImageBarTemplate("ethernetcable");
						}
						else
						{
							if (style == "雷达")
							{
								chart.set_Type(7);
							}
							else
							{
								if (style == "圆锥")
								{
									chart.set_Type(15);
									chart.get_DefaultSeries().set_Type(2);
								}
							}
						}
					}
				}
			}
			chart.get_DefaultElement().get_SmartLabel().set_Text("");
			chart.set_Title(title);
			chart.get_DefaultElement().set_ShowValue(true);
			chart.set_PieLabelMode(1);
			chart.set_ShadingEffectMode(3);
			chart.get_DefaultElement().get_SmartLabel().set_AutoWrap(true);
			chart.get_NoDataLabel().set_Text("没有数据显示");
			chart.get_SeriesCollection().Add(seriesCollection);
		}
		public void Pie2(Chart chart, string title, DataTable table, string xColumn, string yColumn, string style, int displayNum)
		{
			SeriesCollection seriesCollection = new SeriesCollection();
			Series series = new Series("");
			DataView dataView = new DataView(table);
			dataView.Sort = yColumn + "  desc";
			int num = 0;
			DataTable dataTable = dataView.ToTable();
			Element element = new Element("其他");
			bool flag = false;
			double num2 = 0.0;
			Color item = Color.FromArgb(49, 255, 49);
			Random random = new Random(255);
			Color item2 = Color.FromArgb(255, 49, 255);
			List<Color> list = new List<Color>();
			list.Add(item);
			list.Add(item2);
			for (int i = 0; i < displayNum; i++)
			{
				Color item3 = Color.FromArgb(((int)item.A + random.Next(10000)) % 255, ((int)item.B + random.Next(456)) % 255, ((int)item.G + random.Next(1027)) % 100);
				list.Add(item3);
			}
			foreach (DataRow dataRow in dataTable.Rows)
			{
				if (num > displayNum - 2)
				{
					num2 += Convert.ToDouble(dataRow[yColumn].ToString());
					element.set_LabelTemplate("%PercentOfTotal");
					flag = true;
				}
				else
				{
					string text = dataRow[xColumn].ToString();
					text = this.SetXColumn(text);
					Element element2 = new Element(text);
					element2.set_LabelTemplate("%PercentOfTotal");
					element2.get_SmartLabel().set_Text(text);
					element2.set_YValue(Convert.ToDouble(dataRow[yColumn].ToString()));
					series.get_Elements().Add(element2);
					num++;
				}
			}
			if (flag)
			{
				series.get_Elements().Add(element);
			}
			element.set_YValue(num2);
			element.get_SmartLabel().set_Text("其他");
			seriesCollection.Add(series);
			chart.set_TempDirectory("temp");
			chart.set_Use3D(false);
			chart.get_DefaultAxis().set_FormatString("N");
			chart.get_DefaultAxis().set_CultureName("zh-CN");
			chart.set_Palette(list.ToArray());
			chart.get_DefaultElement().get_SmartLabel().set_AutoWrap(true);
			if (style == "饼形")
			{
				chart.set_Type(4);
			}
			else
			{
				if (style == "柱形")
				{
					chart.set_Type(0);
				}
				else
				{
					if (style == "横柱形")
					{
						chart.set_Type(2);
					}
					else
					{
						if (style == "图片柱形")
						{
							chart.set_Type(0);
							chart.get_DefaultSeries().set_ImageBarTemplate("ethernetcable");
						}
						else
						{
							if (style == "雷达")
							{
								chart.set_Type(7);
							}
							else
							{
								if (style == "圆锥")
								{
									chart.set_Type(15);
									chart.get_DefaultSeries().set_Type(2);
								}
							}
						}
					}
				}
			}
			chart.get_DefaultElement().get_SmartLabel().set_Text("");
			chart.set_Title(title);
			chart.get_DefaultElement().set_ShowValue(true);
			chart.set_PieLabelMode(1);
			chart.set_ShadingEffectMode(3);
			chart.get_DefaultElement().get_SmartLabel().set_AutoWrap(true);
			chart.get_NoDataLabel().set_Text("没有数据显示");
			chart.get_SeriesCollection().Add(seriesCollection);
		}
		public void Pie2(Chart chart, string title, DataTable table, string xColumn, string yColumn, string style, int displayNum, string targetUrl)
		{
			this.Pie2(chart, title, table, xColumn, yColumn, style, displayNum, targetUrl, "Jpg", "", false);
		}
		public void Pie2(Chart chart, string title, DataTable table, string xColumn, string yColumn, string style, int displayNum, string targetUrl, string format, string legendBoxPos, bool user3d)
		{
			SeriesCollection seriesCollection = new SeriesCollection();
			Series series = new Series("");
			DataView dataView = new DataView(table);
			dataView.Sort = yColumn + "  desc";
			int num = 0;
			DataTable dataTable = dataView.ToTable();
			Element element = new Element("其他");
			bool flag = false;
			double num2 = 0.0;
			Color item = Color.FromArgb(49, 255, 49);
			Random random = new Random(255);
			Color item2 = Color.FromArgb(255, 49, 255);
			List<Color> list = new List<Color>();
			list.Add(item);
			list.Add(item2);
			for (int i = 0; i < displayNum; i++)
			{
				Color item3 = Color.FromArgb(((int)item.A + random.Next(50000)) % 255, ((int)item.B + random.Next(456)) % 255, ((int)item.G + random.Next(1207)) % 100);
				list.Add(item3);
			}
			if (legendBoxPos.ToLower() == "title")
			{
				chart.get_TitleBox().set_Position(5);
			}
			foreach (DataRow dataRow in dataTable.Rows)
			{
				if (num > displayNum)
				{
					num2 += Convert.ToDouble(dataRow[yColumn].ToString());
					element.set_LabelTemplate("%Name: %PercentOfTotal");
					flag = true;
				}
				else
				{
					string text = dataRow[xColumn].ToString();
					text = this.SetXColumn(text);
					Element element2 = new Element(text);
					element2.set_ToolTip(text);
					element2.set_LabelTemplate("%PercentOfTotal");
					element2.get_LegendEntry().set_HeaderMode(1);
					element2.get_LegendEntry().set_SortOrder(0);
					if (!string.IsNullOrEmpty(targetUrl))
					{
						element2.get_LegendEntry().set_URL(targetUrl + text);
						element2.get_LegendEntry().set_URLTarget("_self");
						element2.set_URL(targetUrl + text);
						element2.set_URLTarget("_self");
					}
					element2.set_YValue(Convert.ToDouble(dataRow[yColumn].ToString()));
					series.get_Elements().Add(element2);
					num++;
				}
			}
			if (flag)
			{
				series.get_Elements().Add(element);
			}
			element.set_YValue(num2);
			element.get_SmartLabel().set_Text("其他");
			seriesCollection.Add(series);
			chart.set_TempDirectory("temp");
			chart.set_Use3D(user3d);
			chart.get_DefaultAxis().set_FormatString("N");
			chart.get_DefaultAxis().set_CultureName("zh-CN");
			chart.set_Palette(list.ToArray());
			chart.get_DefaultElement().get_SmartLabel().set_AutoWrap(true);
			if (style == "饼形")
			{
				chart.set_Type(4);
			}
			else
			{
				if (style == "柱形")
				{
					chart.set_Type(0);
				}
				else
				{
					if (style == "横柱形")
					{
						chart.set_Type(2);
					}
					else
					{
						if (style == "图片柱形")
						{
							chart.set_Type(0);
							chart.get_DefaultSeries().set_ImageBarTemplate("ethernetcable");
						}
						else
						{
							if (style == "雷达")
							{
								chart.set_Type(7);
							}
							else
							{
								if (style == "圆锥")
								{
									chart.set_Type(15);
									chart.get_DefaultSeries().set_Type(2);
								}
							}
						}
					}
				}
			}
			chart.set_Title(title);
			chart.set_PieLabelMode(2);
			chart.get_DefaultElement().set_ShowValue(true);
			chart.set_ShadingEffectMode(3);
			chart.get_LegendBox().get_DefaultEntry().set_PaddingTop(5);
			if (format != null)
			{
				if (!(format == "Jpg"))
				{
					if (!(format == "Png"))
					{
						if (format == "Swf")
						{
							chart.set_ImageFormat(6);
						}
					}
					else
					{
						chart.set_ImageFormat(0);
					}
				}
				else
				{
					chart.set_ImageFormat(2);
				}
			}
			chart.get_DefaultElement().get_SmartLabel().set_AutoWrap(true);
			chart.get_NoDataLabel().set_Text("没有数据显示");
			chart.get_SeriesCollection().Add(seriesCollection);
		}
		public static void ComboHorizontal(Chart chart, int width, int height, string title, DataTable table, string xColumn, string yColumn)
		{
			SeriesCollection seriesCollection = new SeriesCollection();
			Series series = new Series();
			foreach (DataRow dataRow in table.Rows)
			{
				string name = dataRow[xColumn].ToString();
				Element element = new Element();
				element.set_Name(name);
				element.set_LabelTemplate("%PercentOfTotal");
				element.set_YValue(Convert.ToDouble(dataRow[yColumn].ToString()));
				series.get_Elements().Add(element);
			}
			seriesCollection.Add(series);
			chart.set_TempDirectory("temp");
			chart.set_Use3D(false);
			chart.get_DefaultAxis().set_Interval(10.0);
			chart.get_DefaultAxis().set_CultureName("zh-CN");
			chart.set_Palette(new Color[]
			{
				Color.FromArgb(49, 255, 49),
				Color.FromArgb(255, 255, 0),
				Color.FromArgb(255, 99, 49),
				Color.FromArgb(0, 156, 255)
			});
			chart.get_DefaultElement().get_SmartLabel().set_AutoWrap(true);
			chart.set_Type(2);
			chart.set_Size(width + "x" + height);
			chart.get_DefaultElement().get_SmartLabel().set_Text("");
			chart.set_Title(title);
			chart.get_DefaultElement().set_ShowValue(true);
			chart.set_PieLabelMode(1);
			chart.set_ShadingEffectMode(3);
			chart.get_NoDataLabel().set_Text("没有数据显示");
			chart.get_SeriesCollection().Add(seriesCollection);
		}
	}
}
