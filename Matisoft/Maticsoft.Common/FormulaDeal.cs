using System;
namespace Maticsoft.Common
{
	public class FormulaDeal
	{
		static FormulaDeal()
		{
		}
		private double CalculateExpress(string strExpression)
		{
			while (strExpression.IndexOf("+") != -1 || strExpression.IndexOf("-") != -1 || strExpression.IndexOf("*") != -1 || strExpression.IndexOf("/") != -1)
			{
				if (strExpression.IndexOf("*") != -1)
				{
					string text = strExpression.Substring(strExpression.IndexOf("*") + 1, strExpression.Length - strExpression.IndexOf("*") - 1);
					string text2 = strExpression.Substring(0, strExpression.IndexOf("*"));
					string text3 = text2.Substring(this.GetPrivorPos(text2) + 1, text2.Length - this.GetPrivorPos(text2) - 1);
					string text4 = text.Substring(0, this.GetNextPos(text));
					double num = Convert.ToDouble(this.GetExpType(text3)) * Convert.ToDouble(this.GetExpType(text4));
					strExpression = strExpression.Replace(text3 + "*" + text4, num.ToString());
				}
				else
				{
					if (strExpression.IndexOf("/") != -1)
					{
						string text = strExpression.Substring(strExpression.IndexOf("/") + 1, strExpression.Length - strExpression.IndexOf("/") - 1);
						string text2 = strExpression.Substring(0, strExpression.IndexOf("/"));
						string text3 = text2.Substring(this.GetPrivorPos(text2) + 1, text2.Length - this.GetPrivorPos(text2) - 1);
						string text4 = text.Substring(0, this.GetNextPos(text));
						double num = Convert.ToDouble(this.GetExpType(text3)) / Convert.ToDouble(this.GetExpType(text4));
						strExpression = strExpression.Replace(text3 + "/" + text4, num.ToString());
					}
					else
					{
						if (strExpression.IndexOf("+") != -1)
						{
							string text = strExpression.Substring(strExpression.IndexOf("+") + 1, strExpression.Length - strExpression.IndexOf("+") - 1);
							string text2 = strExpression.Substring(0, strExpression.IndexOf("+"));
							string text3 = text2.Substring(this.GetPrivorPos(text2) + 1, text2.Length - this.GetPrivorPos(text2) - 1);
							string text4 = text.Substring(0, this.GetNextPos(text));
							double num = Convert.ToDouble(this.GetExpType(text3)) + Convert.ToDouble(this.GetExpType(text4));
							strExpression = strExpression.Replace(text3 + "+" + text4, num.ToString());
						}
						else
						{
							if (strExpression.IndexOf("-") != -1)
							{
								string text = strExpression.Substring(strExpression.IndexOf("-") + 1, strExpression.Length - strExpression.IndexOf("-") - 1);
								string text2 = strExpression.Substring(0, strExpression.IndexOf("-"));
								string text3 = text2.Substring(this.GetPrivorPos(text2) + 1, text2.Length - this.GetPrivorPos(text2) - 1);
								string text4 = text.Substring(0, this.GetNextPos(text));
								double num = Convert.ToDouble(this.GetExpType(text3)) - Convert.ToDouble(this.GetExpType(text4));
								strExpression = strExpression.Replace(text3 + "-" + text4, num.ToString());
							}
						}
					}
				}
			}
			return Convert.ToDouble(strExpression);
		}
		private double CalculateExExpress(string strExpression, EnumFormula ExpressType)
		{
			double num = 0.0;
			switch (ExpressType)
			{
			case EnumFormula.Sin:
				num = Math.Sin(Convert.ToDouble(strExpression));
				break;
			case EnumFormula.Cos:
				num = Math.Cos(Convert.ToDouble(strExpression));
				break;
			case EnumFormula.Tan:
				num = Math.Tan(Convert.ToDouble(strExpression));
				break;
			case EnumFormula.ATan:
				num = Math.Atan(Convert.ToDouble(strExpression));
				break;
			case EnumFormula.Sqrt:
				num = Math.Sqrt(Convert.ToDouble(strExpression));
				break;
			case EnumFormula.Pow:
				num = Math.Pow(Convert.ToDouble(strExpression), 2.0);
				break;
			}
			if (num == 0.0)
			{
				return Convert.ToDouble(strExpression);
			}
			return num;
		}
		private int GetNextPos(string strExpression)
		{
			int[] array = new int[]
			{
				strExpression.IndexOf("+"),
				strExpression.IndexOf("-"),
				strExpression.IndexOf("*"),
				strExpression.IndexOf("/")
			};
			int num = strExpression.Length;
			for (int i = 1; i <= array.Length; i++)
			{
				if (num > array[i - 1] && array[i - 1] != -1)
				{
					num = array[i - 1];
				}
			}
			return num;
		}
		private int GetPrivorPos(string strExpression)
		{
			int[] array = new int[]
			{
				strExpression.LastIndexOf("+"),
				strExpression.LastIndexOf("-"),
				strExpression.LastIndexOf("*"),
				strExpression.LastIndexOf("/")
			};
			int num = -1;
			for (int i = 1; i <= array.Length; i++)
			{
				if (num < array[i - 1] && array[i - 1] != -1)
				{
					num = array[i - 1];
				}
			}
			return num;
		}
		public string SpiltExpression(string strExpression)
		{
			while (strExpression.IndexOf("(") != -1)
			{
				string text = strExpression.Substring(strExpression.LastIndexOf("(") + 1, strExpression.Length - strExpression.LastIndexOf("(") - 1);
				string text2 = text.Substring(0, text.IndexOf(")"));
				strExpression = strExpression.Replace("(" + text2 + ")", this.CalculateExpress(text2).ToString());
			}
			if (strExpression.IndexOf("+") != -1 || strExpression.IndexOf("-") != -1 || strExpression.IndexOf("*") != -1 || strExpression.IndexOf("/") != -1)
			{
				strExpression = this.CalculateExpress(strExpression).ToString();
			}
			return strExpression;
		}
		private string GetExpType(string strExpression)
		{
			strExpression = strExpression.ToUpper();
			if (strExpression.IndexOf("SIN") != -1)
			{
				return this.CalculateExExpress(strExpression.Substring(strExpression.IndexOf("N") + 1, strExpression.Length - 1 - strExpression.IndexOf("N")), EnumFormula.Sin).ToString();
			}
			if (strExpression.IndexOf("COS") != -1)
			{
				return this.CalculateExExpress(strExpression.Substring(strExpression.IndexOf("S") + 1, strExpression.Length - 1 - strExpression.IndexOf("S")), EnumFormula.Cos).ToString();
			}
			if (strExpression.IndexOf("TAN") != -1)
			{
				return this.CalculateExExpress(strExpression.Substring(strExpression.IndexOf("N") + 1, strExpression.Length - 1 - strExpression.IndexOf("N")), EnumFormula.Tan).ToString();
			}
			if (strExpression.IndexOf("ATAN") != -1)
			{
				return this.CalculateExExpress(strExpression.Substring(strExpression.IndexOf("N") + 1, strExpression.Length - 1 - strExpression.IndexOf("N")), EnumFormula.ATan).ToString();
			}
			if (strExpression.IndexOf("SQRT") != -1)
			{
				return this.CalculateExExpress(strExpression.Substring(strExpression.IndexOf("T") + 1, strExpression.Length - 1 - strExpression.IndexOf("T")), EnumFormula.Sqrt).ToString();
			}
			if (strExpression.IndexOf("POW") != -1)
			{
				return this.CalculateExExpress(strExpression.Substring(strExpression.IndexOf("W") + 1, strExpression.Length - 1 - strExpression.IndexOf("W")), EnumFormula.Pow).ToString();
			}
			return strExpression;
		}
	}
}
