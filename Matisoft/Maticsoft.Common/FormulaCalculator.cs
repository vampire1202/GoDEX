using System;
using System.Collections.Generic;
using System.Collections.Specialized;
namespace Maticsoft.Common
{
	public class FormulaCalculator
	{
		protected const int MAX_LEVEL = 99;
		private static object[][] Level = new object[][]
		{
			new object[]
			{
				",",
				0
			},
			new object[]
			{
				"=",
				1
			},
			new object[]
			{
				">=",
				1
			},
			new object[]
			{
				"<=",
				1
			},
			new object[]
			{
				"<>",
				1
			},
			new object[]
			{
				">",
				1
			},
			new object[]
			{
				"<",
				1
			},
			new object[]
			{
				"+",
				2
			},
			new object[]
			{
				"-",
				2
			},
			new object[]
			{
				"*",
				3
			},
			new object[]
			{
				"/",
				3
			},
			new object[]
			{
				"%",
				3
			},
			new object[]
			{
				"NEG",
				4
			},
			new object[]
			{
				"^",
				5
			},
			new object[]
			{
				"(",
				99
			},
			new object[]
			{
				"ROUND(",
				99
			},
			new object[]
			{
				"TRUNC(",
				99
			},
			new object[]
			{
				"MAX(",
				99
			},
			new object[]
			{
				"MIN(",
				99
			},
			new object[]
			{
				"ABS(",
				99
			},
			new object[]
			{
				"SUM(",
				99
			},
			new object[]
			{
				"AVERAGE(",
				99
			},
			new object[]
			{
				"SQRT(",
				99
			},
			new object[]
			{
				"EXP(",
				99
			},
			new object[]
			{
				"LOG(",
				99
			},
			new object[]
			{
				"LOG10(",
				99
			},
			new object[]
			{
				"SIN(",
				99
			},
			new object[]
			{
				"COS(",
				99
			},
			new object[]
			{
				"TAN(",
				99
			},
			new object[]
			{
				"IF(",
				99
			},
			new object[]
			{
				"NOT(",
				99
			},
			new object[]
			{
				"AND(",
				99
			},
			new object[]
			{
				"OR(",
				99
			}
		};
		private string _opt;
		private string _expression;
		private string _leftValue;
		private NameValueCollection _data;
		private static int GetOperatorLevel(string o)
		{
			for (int i = 0; i < FormulaCalculator.Level.Length; i++)
			{
				if ((string)FormulaCalculator.Level[i][0] == o)
				{
					return (int)FormulaCalculator.Level[i][1];
				}
			}
			return -1;
		}
		private static string GetOperator(string v)
		{
			for (int i = 0; i < FormulaCalculator.Level.Length; i++)
			{
				if (v.StartsWith((string)FormulaCalculator.Level[i][0]))
				{
					return (string)FormulaCalculator.Level[i][0];
				}
			}
			return null;
		}
		public static decimal[] CalculateExpression(string expression, NameValueCollection dataProvider)
		{
			FormulaCalculator formulaCalculator = new FormulaCalculator(expression, dataProvider);
			return formulaCalculator.Calculate();
		}
		public FormulaCalculator(string expression, NameValueCollection dataProvider)
		{
			this._data = dataProvider;
			this._expression = expression.ToUpper();
			if (this.GetIndex(this._expression) != -1)
			{
				throw new Exception("缺少\"(\"");
			}
			this.Initialize();
		}
		private void Initialize()
		{
			string expression;
			this.GetNext(this._expression, out this._leftValue, out this._opt, out expression);
			this._expression = expression;
		}
		private void GetNext(string expression, out string left, out string opt, out string right)
		{
			right = expression;
			left = string.Empty;
			opt = null;
			while (right != string.Empty)
			{
				opt = FormulaCalculator.GetOperator(right);
				if (opt != null)
				{
					right = right.Substring(opt.Length, right.Length - opt.Length);
					break;
				}
				left += right[0];
				right = right.Substring(1, right.Length - 1);
			}
			right = right.Trim();
		}
		public decimal[] Calculate()
		{
			if (this._opt == null)
			{
				decimal num = 0m;
				try
				{
					num = decimal.Parse(this._leftValue);
				}
				catch
				{
					try
					{
						num = decimal.Parse(this._data[this._leftValue]);
					}
					catch
					{
						throw new Exception("错误的格式:" + this._leftValue);
					}
				}
				return new decimal[]
				{
					num
				};
			}
			if (FormulaCalculator.GetOperatorLevel(this._opt) != 99)
			{
				if (this._opt != "-" && this._leftValue == string.Empty)
				{
					throw new Exception("\"" + this._opt + "\"运算符的左边需要值或表达式");
				}
				if (this._opt == "-" && this._leftValue == string.Empty)
				{
					this._opt = "NEG";
				}
				if (this._expression == string.Empty)
				{
					throw new Exception("\"" + this._opt + "\"运算符的右边需要值或表达式");
				}
				return this.CalculateTwoParms();
			}
			else
			{
				if (this._leftValue != string.Empty)
				{
					throw new Exception("\"" + this._opt + "\"运算符的左边不需要值或表达式");
				}
				return this.CalculateFunction();
			}
		}
		private decimal[] CalculateFunction()
		{
			int index = this.GetIndex(this._expression);
			if (index == -1)
			{
				throw new Exception("缺少\")\"");
			}
			string expression = this._expression.Substring(0, index);
			if (index == this._expression.Length - 1)
			{
				//return this.Calc(this._opt, expression);
			}
			this._expression = this._expression.Substring(index + 1, this._expression.Length - index - 1);
			string text;
			string text2;
			string expression2;
			this.GetNext(this._expression, out text, out text2, out expression2);
			//decimal[] array = this.Calc(this._opt, expression);
			//this._leftValue = array[array.Length - 1].ToString();
			if (text2 == null)
			{
				throw new Exception("\")\"运算符的右边需要运算符");
			}
			this._opt = text2;
			this._expression = expression2;
			return this.Calculate();
		}
		private int GetIndex(string expression)
		{
			int num = 0;
			for (int i = 0; i < expression.Length; i++)
			{
				if (expression[i] == ')')
				{
					if (num == 0)
					{
						return i;
					}
					num--;
				}
				if (expression[i] == '(')
				{
					num++;
				}
			}
			return -1;
		}
		private decimal[] CalculateTwoParms()
		{
			string text;
			string text2;
			string text3;
			this.GetNext(this._expression, out text, out text2, out text3);
			decimal[] array;
			if (text2 == null || FormulaCalculator.GetOperatorLevel(this._opt) >= FormulaCalculator.GetOperatorLevel(text2))
			{
				array = this.Calc(this._opt, this._leftValue, text);
			}
			else
			{
				string text4 = text;
				while (FormulaCalculator.GetOperatorLevel(this._opt) < FormulaCalculator.GetOperatorLevel(text2) && text3 != string.Empty)
				{
					text4 += text2;
					if (FormulaCalculator.GetOperatorLevel(text2) == 99)
					{
						int index = this.GetIndex(text3);
						text4 += text3.Substring(0, index + 1);
						text3 = text3.Substring(index + 1);
					}
					this.GetNext(text3, out text, out text2, out text3);
					text4 += text;
				}
				FormulaCalculator formulaCalculator = new FormulaCalculator(text4, this._data);
				decimal[] array2 = formulaCalculator.Calculate();
				array = this.Calc(this._opt, this._leftValue, array2[array2.Length - 1].ToString());
			}
			this._leftValue = array[array.Length - 1].ToString();
			this._opt = text2;
			this._expression = text3;
			decimal[] array3 = this.Calculate();
			decimal[] array4 = new decimal[array.Length - 1 + array3.Length];
			for (int i = 0; i < array.Length - 1; i++)
			{
				array4[i] = array[i];
			}
			for (int j = 0; j < array3.Length; j++)
			{
				array4[array.Length - 1 + j] = array3[j];
			}
			return array4;
        }
	
		private bool GetBoolean(decimal d)
		{
			return (int)d == 1;
		}
		private decimal[] Calc(string opt, string leftEx, string rightEx)
		{
			decimal num = 0m;
			decimal num2 = 0m;
			decimal num3 = 0m;
			try
			{
				num2 = decimal.Parse(leftEx);
			}
			catch
			{
				if (opt != "NEG")
				{
					try
					{
						num2 = decimal.Parse(this._data[leftEx]);
					}
					catch
					{
						throw new Exception("错误的格式:" + leftEx);
					}
				}
			}
			try
			{
				num3 = decimal.Parse(rightEx);
			}
			catch
			{
				try
				{
					num3 = decimal.Parse(this._data[rightEx]);
				}
				catch
				{
					throw new Exception("错误的格式:" + leftEx);
				}
			}
			string opt2;
			switch (opt2 = this._opt)
			{
			case "NEG":
				num = decimal.Negate(num3);
				break;
			case "+":
				num = num2 + num3;
				break;
			case "-":
				num = num2 - num3;
				break;
			case "*":
				num = num2 * num3;
				break;
			case "/":
				num = num2 / num3;
				break;
			case "%":
				num = decimal.Remainder(num2, num3);
				break;
			case "^":
				num = (decimal)Math.Pow((double)num2, (double)num3);
				break;
			case ",":
				return new decimal[]
				{
					num2,
					num3
				};
			case "=":
				num = ((num2 == num3) ? 1 : 0);
				break;
			case "<>":
				num = ((num2 != num3) ? 1 : 0);
				break;
			case "<":
				num = ((num2 < num3) ? 1 : 0);
				break;
			case ">":
				num = ((num2 > num3) ? 1 : 0);
				break;
			case ">=":
				num = ((num2 >= num3) ? 1 : 0);
				break;
			case "<=":
				num = ((num2 <= num3) ? 1 : 0);
				break;
			}
			return new decimal[]
			{
				num
			};
		}
	}
}
