using System;

using IKVM.Attributes;
using java.lang;

namespace edu.cmu.sphinx.alignment.tokenizer
{
	public class NumberExpander : java.lang.Object
	{
		
		public static void __<clinit>()
		{
		}

		[LineNumberTable(new byte[]
		{
			161,
			27,
			168,
			139,
			104,
			104,
			114,
			109,
			141,
			236,
			56,
			230,
			75
		})]
		
		public static void expandLetters(string letters, WordRelation wordRelation)
		{
			letters = java.lang.String.instancehelper_toLowerCase(letters);
			for (int i = 0; i < java.lang.String.instancehelper_length(letters); i++)
			{
				int num = (int)java.lang.String.instancehelper_charAt(letters, i);
				if (Character.isDigit((char)num))
				{
					wordRelation.addWord(NumberExpander.digit2num[num - 48]);
				}
				else if (java.lang.String.instancehelper_equals(letters, "a"))
				{
					wordRelation.addWord("_a");
				}
				else
				{
					wordRelation.addWord(java.lang.String.valueOf((char)num));
				}
			}
		}

		[LineNumberTable(new byte[]
		{
			160,
			242,
			167,
			139,
			107,
			115,
			139,
			107,
			115,
			112,
			138,
			110,
			107,
			113,
			141,
			105,
			105,
			135,
			107,
			106,
			105,
			135,
			130,
			135
		})]
		
		public static void expandReal(string numberString, WordRelation wordRelation)
		{
			int num = java.lang.String.instancehelper_length(numberString);
			int num2;
			if (java.lang.String.instancehelper_charAt(numberString, 0) == '-')
			{
				wordRelation.addWord("minus");
				NumberExpander.expandReal(java.lang.String.instancehelper_substring(numberString, 1, num), wordRelation);
			}
			else if (java.lang.String.instancehelper_charAt(numberString, 0) == '+')
			{
				wordRelation.addWord("plus");
				NumberExpander.expandReal(java.lang.String.instancehelper_substring(numberString, 1, num), wordRelation);
			}
			else if ((num2 = java.lang.String.instancehelper_indexOf(numberString, 101)) != -1 || (num2 = java.lang.String.instancehelper_indexOf(numberString, 69)) != -1)
			{
				NumberExpander.expandReal(java.lang.String.instancehelper_substring(numberString, 0, num2), wordRelation);
				wordRelation.addWord("e");
				NumberExpander.expandReal(java.lang.String.instancehelper_substring(numberString, num2 + 1), wordRelation);
			}
			else if ((num2 = java.lang.String.instancehelper_indexOf(numberString, 46)) != -1)
			{
				string text = java.lang.String.instancehelper_substring(numberString, 0, num2);
				if (java.lang.String.instancehelper_length(text) > 0)
				{
					NumberExpander.expandReal(text, wordRelation);
				}
				wordRelation.addWord("point");
				string text2 = java.lang.String.instancehelper_substring(numberString, num2 + 1);
				if (java.lang.String.instancehelper_length(text2) > 0)
				{
					NumberExpander.expandDigits(text2, wordRelation);
				}
			}
			else
			{
				NumberExpander.expandNumber(numberString, wordRelation);
			}
		}

		[LineNumberTable(new byte[]
		{
			160,
			110,
			103,
			102,
			104,
			104,
			152,
			235,
			59,
			230,
			72
		})]
		
		public static void expandDigits(string numberString, WordRelation wordRelation)
		{
			int num = java.lang.String.instancehelper_length(numberString);
			for (int i = 0; i < num; i++)
			{
				int num2 = (int)java.lang.String.instancehelper_charAt(numberString, i);
				if (Character.isDigit((char)num2))
				{
					wordRelation.addWord(NumberExpander.digit2num[(int)(java.lang.String.instancehelper_charAt(numberString, i) - '0')]);
				}
				else
				{
					wordRelation.addWord("umpty");
				}
			}
		}

		[LineNumberTable(new byte[]
		{
			14,
			135,
			136,
			100,
			105,
			100,
			105,
			100,
			105,
			100,
			105,
			101,
			105,
			101,
			137,
			135
		})]
		
		public static void expandNumber(string numberString, WordRelation wordRelation)
		{
			int num = java.lang.String.instancehelper_length(numberString);
			if (num != 0)
			{
				if (num == 1)
				{
					NumberExpander.expandDigits(numberString, wordRelation);
				}
				else if (num == 2)
				{
					NumberExpander.expand2DigitNumber(numberString, wordRelation);
				}
				else if (num == 3)
				{
					NumberExpander.expand3DigitNumber(numberString, wordRelation);
				}
				else if (num < 7)
				{
					NumberExpander.expandBelow7DigitNumber(numberString, wordRelation);
				}
				else if (num < 10)
				{
					NumberExpander.expandBelow10DigitNumber(numberString, wordRelation);
				}
				else if (num < 13)
				{
					NumberExpander.expandBelow13DigitNumber(numberString, wordRelation);
				}
				else
				{
					NumberExpander.expandDigits(numberString, wordRelation);
				}
			}
		}

		[LineNumberTable(new byte[]
		{
			160,
			207,
			135,
			113,
			105,
			107,
			140,
			110,
			144,
			111,
			107,
			115,
			147,
			105,
			111,
			113,
			103,
			110,
			98,
			110,
			142
		})]
		
		public static void expandID(string numberString, WordRelation wordRelation)
		{
			int num = java.lang.String.instancehelper_length(numberString);
			if (num == 4 && java.lang.String.instancehelper_charAt(numberString, 2) == '0' && java.lang.String.instancehelper_charAt(numberString, 3) == '0')
			{
				if (java.lang.String.instancehelper_charAt(numberString, 1) == '0')
				{
					NumberExpander.expandNumber(numberString, wordRelation);
				}
				else
				{
					NumberExpander.expandNumber(java.lang.String.instancehelper_substring(numberString, 0, 2), wordRelation);
					wordRelation.addWord("hundred");
				}
			}
			else if (num == 2 && java.lang.String.instancehelper_charAt(numberString, 0) == '0')
			{
				wordRelation.addWord("oh");
				NumberExpander.expandDigits(java.lang.String.instancehelper_substring(numberString, 1, 2), wordRelation);
			}
			else if ((num == 4 && java.lang.String.instancehelper_charAt(numberString, 1) == '0') || num < 3)
			{
				NumberExpander.expandNumber(numberString, wordRelation);
			}
			else
			{
				int num2 = num;
				int num3 = 2;
				if (((num3 != -1) ? (num2 % num3) : 0) == 1)
				{
					string word = NumberExpander.digit2num[(int)(java.lang.String.instancehelper_charAt(numberString, 0) - '0')];
					wordRelation.addWord(word);
					NumberExpander.expandID(java.lang.String.instancehelper_substring(numberString, 1, num), wordRelation);
				}
				else
				{
					NumberExpander.expandNumber(java.lang.String.instancehelper_substring(numberString, 0, 2), wordRelation);
					NumberExpander.expandID(java.lang.String.instancehelper_substring(numberString, 2, num), wordRelation);
				}
			}
		}

		[LineNumberTable(new byte[]
		{
			160,
			130,
			191,
			25,
			135,
			134,
			104,
			110,
			147,
			100,
			147,
			100,
			179,
			110,
			105,
			110,
			105,
			110,
			231,
			69,
			100,
			168
		})]
		
		public static void expandOrdinal(string rawNumberString, WordRelation wordRelation)
		{
			object obj = ",";
			object obj2 = "";
			object _<ref> = obj;
			CharSequence charSequence;
			charSequence.__<ref> = _<ref>;
			CharSequence charSequence2 = charSequence;
			_<ref> = obj2;
			charSequence.__<ref> = _<ref>;
			NumberExpander.expandNumber(java.lang.String.instancehelper_replace(rawNumberString, charSequence2, charSequence), wordRelation);
			Item tail = wordRelation.getTail();
			if (tail != null)
			{
				FeatureSet features = tail.getFeatures();
				string @string = features.getString("name");
				string text = NumberExpander.findMatchInArray(@string, NumberExpander.digit2num, NumberExpander.ord2num);
				if (text == null)
				{
					text = NumberExpander.findMatchInArray(@string, NumberExpander.digit2teen, NumberExpander.ord2teen);
				}
				if (text == null)
				{
					text = NumberExpander.findMatchInArray(@string, NumberExpander.digit2enty, NumberExpander.ord2enty);
				}
				if (java.lang.String.instancehelper_equals(@string, "hundred"))
				{
					text = "hundredth";
				}
				else if (java.lang.String.instancehelper_equals(@string, "thousand"))
				{
					text = "thousandth";
				}
				else if (java.lang.String.instancehelper_equals(@string, "billion"))
				{
					text = "billionth";
				}
				if (text != null)
				{
					wordRelation.setLastWord(text);
				}
			}
		}

		[LineNumberTable(new byte[]
		{
			160,
			166,
			105,
			110,
			143,
			150
		})]
		
		public static void expandNumess(string rawString, WordRelation wordRelation)
		{
			if (java.lang.String.instancehelper_length(rawString) == 4)
			{
				NumberExpander.expand2DigitNumber(java.lang.String.instancehelper_substring(rawString, 0, 2), wordRelation);
				NumberExpander.expandNumess(java.lang.String.instancehelper_substring(rawString, 2), wordRelation);
			}
			else
			{
				wordRelation.addWord(NumberExpander.digit2Numness[(int)(java.lang.String.instancehelper_charAt(rawString, 0) - '0')]);
			}
		}

		[LineNumberTable(new byte[]
		{
			161,
			51,
			130,
			110,
			104,
			101,
			106,
			101,
			105,
			104,
			107,
			106,
			101,
			100,
			102,
			101,
			101,
			134,
			132,
			98,
			228,
			45,
			233,
			87
		})]
		
		public static int expandRoman(string roman)
		{
			int num = 0;
			for (int i = 0; i < java.lang.String.instancehelper_length(roman); i++)
			{
				int num2 = (int)java.lang.String.instancehelper_charAt(roman, i);
				if (num2 == 88)
				{
					num += 10;
				}
				else if (num2 == 86)
				{
					num += 5;
				}
				else if (num2 == 73)
				{
					if (i + 1 < java.lang.String.instancehelper_length(roman))
					{
						int num3 = (int)java.lang.String.instancehelper_charAt(roman, i + 1);
						if (num3 == 86)
						{
							num += 4;
							i++;
						}
						else if (num3 == 88)
						{
							num += 9;
							i++;
						}
						else
						{
							num++;
						}
					}
					else
					{
						num++;
					}
				}
			}
			return num;
		}

		[LineNumberTable(new byte[]
		{
			43,
			139,
			208,
			113,
			103,
			101,
			139,
			113,
			103,
			141,
			113,
			103,
			130,
			113,
			103,
			179
		})]
		
		private static void expand2DigitNumber(string text, WordRelation wordRelation)
		{
			if (java.lang.String.instancehelper_charAt(text, 0) == '0')
			{
				if (java.lang.String.instancehelper_charAt(text, 1) != '0')
				{
					string word = NumberExpander.digit2num[(int)(java.lang.String.instancehelper_charAt(text, 1) - '0')];
					wordRelation.addWord(word);
				}
			}
			else if (java.lang.String.instancehelper_charAt(text, 1) == '0')
			{
				string word = NumberExpander.digit2enty[(int)(java.lang.String.instancehelper_charAt(text, 0) - '0')];
				wordRelation.addWord(word);
			}
			else if (java.lang.String.instancehelper_charAt(text, 0) == '1')
			{
				string word = NumberExpander.digit2teen[(int)(java.lang.String.instancehelper_charAt(text, 1) - '0')];
				wordRelation.addWord(word);
			}
			else
			{
				string word = NumberExpander.digit2enty[(int)(java.lang.String.instancehelper_charAt(text, 0) - '0')];
				wordRelation.addWord(word);
				NumberExpander.expandDigits(java.lang.String.instancehelper_substring(text, 1, java.lang.String.instancehelper_length(text)), wordRelation);
			}
		}

		[LineNumberTable(new byte[]
		{
			77,
			107,
			138,
			113,
			103,
			107,
			136
		})]
		
		private static void expand3DigitNumber(string text, WordRelation wordRelation)
		{
			if (java.lang.String.instancehelper_charAt(text, 0) == '0')
			{
				NumberExpander.expandNumberAt(text, 1, wordRelation);
			}
			else
			{
				string word = NumberExpander.digit2num[(int)(java.lang.String.instancehelper_charAt(text, 0) - '0')];
				wordRelation.addWord(word);
				wordRelation.addWord("hundred");
				NumberExpander.expandNumberAt(text, 1, wordRelation);
			}
		}

		[LineNumberTable(new byte[]
		{
			97,
			109
		})]
		
		private static void expandBelow7DigitNumber(string text, WordRelation wordRelation)
		{
			NumberExpander.expandLargeNumber(text, "thousand", 3, wordRelation);
		}

		[LineNumberTable(new byte[]
		{
			109,
			109
		})]
		
		private static void expandBelow10DigitNumber(string text, WordRelation wordRelation)
		{
			NumberExpander.expandLargeNumber(text, "million", 6, wordRelation);
		}

		[LineNumberTable(new byte[]
		{
			121,
			110
		})]
		
		private static void expandBelow13DigitNumber(string text, WordRelation wordRelation)
		{
			NumberExpander.expandLargeNumber(text, "billion", 9, wordRelation);
		}

		[LineNumberTable(new byte[]
		{
			160,
			97,
			99,
			43,
			165
		})]
		
		private static void expandNumberAt(string text, int num, WordRelation wordRelation)
		{
			NumberExpander.expandNumber(java.lang.String.instancehelper_substring(text, num, java.lang.String.instancehelper_length(text)), wordRelation);
		}

		[LineNumberTable(new byte[]
		{
			160,
			72,
			167,
			100,
			169,
			103,
			103,
			105,
			135,
			104
		})]
		
		private static void expandLargeNumber(string text, string word, int num, WordRelation wordRelation)
		{
			int num2 = java.lang.String.instancehelper_length(text);
			int num3 = num2 - num;
			string numberString = java.lang.String.instancehelper_substring(text, 0, num3);
			Item tail = wordRelation.getTail();
			NumberExpander.expandNumber(numberString, wordRelation);
			if (wordRelation.getTail() != tail)
			{
				wordRelation.addWord(word);
			}
			NumberExpander.expandNumberAt(text, num3, wordRelation);
		}

		[LineNumberTable(new byte[]
		{
			160,
			187,
			103,
			107,
			101,
			132,
			226,
			59,
			230,
			73
		})]
		
		private static string findMatchInArray(string text, string[] array, string[] array2)
		{
			int i = 0;
			while (i < array.Length)
			{
				if (java.lang.String.instancehelper_equals(text, array[i]))
				{
					if (i < array2.Length)
					{
						return array2[i];
					}
					return null;
				}
				else
				{
					i++;
				}
			}
			return null;
		}

		
		
		private NumberExpander()
		{
		}

		[LineNumberTable(new byte[]
		{
			159,
			165,
			191,
			62,
			223,
			62,
			223,
			62,
			191,
			62,
			223,
			62,
			223,
			62
		})]
		static NumberExpander()
		{
		}

		
		private static string[] digit2num = new string[]
		{
			"zero",
			"one",
			"two",
			"three",
			"four",
			"five",
			"six",
			"seven",
			"eight",
			"nine"
		};

		
		private static string[] digit2teen = new string[]
		{
			"ten",
			"eleven",
			"twelve",
			"thirteen",
			"fourteen",
			"fifteen",
			"sixteen",
			"seventeen",
			"eighteen",
			"nineteen"
		};

		
		private static string[] digit2enty = new string[]
		{
			"zero",
			"ten",
			"twenty",
			"thirty",
			"forty",
			"fifty",
			"sixty",
			"seventy",
			"eighty",
			"ninety"
		};

		
		private static string[] ord2num = new string[]
		{
			"zeroth",
			"first",
			"second",
			"third",
			"fourth",
			"fifth",
			"sixth",
			"seventh",
			"eighth",
			"ninth"
		};

		
		private static string[] ord2teen = new string[]
		{
			"tenth",
			"eleventh",
			"twelfth",
			"thirteenth",
			"fourteenth",
			"fifteenth",
			"sixteenth",
			"seventeenth",
			"eighteenth",
			"nineteenth"
		};

		
		private static string[] ord2enty = new string[]
		{
			"zeroth",
			"tenth",
			"twentieth",
			"thirtieth",
			"fortieth",
			"fiftieth",
			"sixtieth",
			"seventieth",
			"eightieth",
			"ninetieth"
		};

		private static string[] digit2Numness = new string[]
		{
			"",
			"tens",
			"twenties",
			"thirties",
			"fourties",
			"fifties",
			"sixties",
			"seventies",
			"eighties",
			"nineties"
		};
	}
}
