using System;

using IKVM.Attributes;
using IKVM.Runtime;
using java.io;
using java.lang;
using java.util;

namespace edu.cmu.sphinx.linguist.acoustic.tiedstate.HTK
{
	public class NamesConversion : java.lang.Object
	{		
		internal virtual void split3ph(string text)
		{
			int num = java.lang.String.instancehelper_indexOf(text, 45);
			if (num >= 0)
			{
				this.left = java.lang.String.instancehelper_substring(text, 0, num);
			}
			else
			{
				this.left = null;
				num = -1;
			}
			string text2 = java.lang.String.instancehelper_substring(text, num + 1);
			num = java.lang.String.instancehelper_indexOf(text2, 43);
			if (num >= 0)
			{
				this.right = java.lang.String.instancehelper_substring(text2, num + 1);
			}
			else
			{
				this.right = null;
				num = java.lang.String.instancehelper_length(text2);
			}
			this.@base = java.lang.String.instancehelper_substring(text2, 0, num);
		}
		
		internal virtual void addInConv(string text, HashMap hashMap)
		{
			if (!hashMap.containsKey(text))
			{
				string text2 = java.lang.String.instancehelper_toUpperCase(text);
				while (hashMap.containsValue(text2))
				{
					text2 = new StringBuilder().append(text2).append("_X").toString();
				}
				hashMap.put(text, text2);
			}
		}
		
		internal virtual string conv1ph(string text)
		{
			return (string)this.phoneConv.get(text);
		}
		
		internal virtual string conv3ph()
		{
			string text;
			if (this.left != null)
			{
				text = new StringBuilder().append(this.conv1ph(this.left)).append('-').toString();
			}
			else
			{
				text = "";
			}
			text = new StringBuilder().append(text).append(this.conv1ph(this.@base)).toString();
			if (this.right != null)
			{
				text = new StringBuilder().append(text).append('+').append(this.conv1ph(this.right)).toString();
			}
			if (java.lang.String.instancehelper_equals(text, "null"))
			{
				java.lang.System.err.println(new StringBuilder().append("detson error ").append(this.left).append(' ').append(this.@base).append(' ').append(this.right).toString());
				java.lang.System.exit(1);
			}
			return text;
		}
		
		public NamesConversion()
		{
			this.phoneConv = new HashMap();
			this.wordConv = new HashMap();
		}
		
		internal virtual void buildPhoneConversion(string text)
		{
			IOException ex2;
			try
			{
				BufferedReader bufferedReader = new BufferedReader(new FileReader(text));
				for (;;)
				{
					string text2 = bufferedReader.readLine();
					if (text2 == null)
					{
						break;
					}
					int num = java.lang.String.instancehelper_indexOf(text2, "~h");
					if (num >= 0)
					{
						num = java.lang.String.instancehelper_indexOf(text2, 34);
						int num2 = java.lang.String.instancehelper_lastIndexOf(text2, 34);
						string text3 = java.lang.String.instancehelper_substring(text2, num + 1, num2);
						this.split3ph(text3);
						if (this.left != null)
						{
							this.addInConv(this.left, this.phoneConv);
						}
						if (this.@base != null)
						{
							this.addInConv(this.@base, this.phoneConv);
						}
						if (this.right != null)
						{
							this.addInConv(this.right, this.phoneConv);
						}
					}
				}
				bufferedReader.close();
			}
			catch (IOException ex)
			{
				ex2 = ByteCodeHelper.MapException<IOException>(ex, 1);
				goto IL_BD;
			}
			return;
			IL_BD:
			IOException ex3 = ex2;
			Throwable.instancehelper_printStackTrace(ex3);
		}

		[LineNumberTable(new byte[]
		{
			24,
			172,
			103,
			101,
			103,
			104,
			103,
			141,
			98,
			186,
			2,
			98,
			135
		})]
		
		internal virtual void buildWordConversion(string text)
		{
			IOException ex2;
			try
			{
				BufferedReader bufferedReader = new BufferedReader(new FileReader(text));
				for (;;)
				{
					string text2 = bufferedReader.readLine();
					if (text2 == null)
					{
						break;
					}
					StringTokenizer stringTokenizer = new StringTokenizer(text2);
					if (stringTokenizer.hasMoreTokens())
					{
						string text3 = stringTokenizer.nextToken();
						this.addInConv(text3, this.wordConv);
					}
				}
				bufferedReader.close();
			}
			catch (IOException ex)
			{
				ex2 = ByteCodeHelper.MapException<IOException>(ex, 1);
				goto IL_53;
			}
			return;
			IL_53:
			IOException ex3 = ex2;
			Throwable.instancehelper_printStackTrace(ex3);
		}

		[LineNumberTable(new byte[]
		{
			75,
			108,
			191,
			6,
			103,
			104,
			108,
			100,
			105,
			106,
			109,
			104,
			104,
			127,
			9,
			98,
			103,
			101,
			102,
			186,
			2,
			98,
			135
		})]
		
		internal virtual void convertMMF(string text)
		{
			IOException ex2;
			try
			{
				BufferedReader bufferedReader = new BufferedReader(new FileReader(text));
				PrintWriter printWriter = new PrintWriter(new FileWriter(new StringBuilder().append(text).append(".conv").toString()));
				for (;;)
				{
					string text2 = bufferedReader.readLine();
					if (text2 == null)
					{
						break;
					}
					int num = java.lang.String.instancehelper_indexOf(text2, "~h");
					if (num >= 0)
					{
						num = java.lang.String.instancehelper_indexOf(text2, 34);
						int num2 = java.lang.String.instancehelper_lastIndexOf(text2, 34);
						string text3 = java.lang.String.instancehelper_substring(text2, num + 1, num2);
						this.split3ph(text3);
						string text4 = this.conv3ph();
						printWriter.println(new StringBuilder().append("~h \"").append(text4).append('"').toString());
					}
					else
					{
						printWriter.println(text2);
					}
				}
				printWriter.close();
				bufferedReader.close();
			}
			catch (IOException ex)
			{
				ex2 = ByteCodeHelper.MapException<IOException>(ex, 1);
				goto IL_D2;
			}
			return;
			IL_D2:
			IOException ex3 = ex2;
			Throwable.instancehelper_printStackTrace(ex3);
		}

		[LineNumberTable(new byte[]
		{
			160,
			158,
			108,
			191,
			6,
			103,
			104,
			103,
			109,
			104,
			116,
			104,
			126,
			107,
			136,
			140,
			112,
			138,
			136,
			104,
			104,
			126,
			101,
			102,
			101,
			102,
			186,
			2,
			98,
			135
		})]
		
		internal virtual void convertLexicon(string text)
		{
			IOException ex2;
			try
			{
				BufferedReader bufferedReader = new BufferedReader(new FileReader(text));
				PrintWriter printWriter = new PrintWriter(new FileWriter(new StringBuilder().append(text).append(".conv").toString()));
				for (;;)
				{
					string text2 = bufferedReader.readLine();
					if (text2 == null)
					{
						break;
					}
					StringTokenizer stringTokenizer = new StringTokenizer(text2);
					if (stringTokenizer != null)
					{
						if (stringTokenizer.hasMoreTokens())
						{
							string text3 = stringTokenizer.nextToken();
							string text4 = (string)this.wordConv.get(text3);
							if (text4 != null)
							{
								text3 = text4;
							}
							printWriter.print(new StringBuilder().append(text3).append(' ').toString());
							while (stringTokenizer.hasMoreTokens())
							{
								string text5 = stringTokenizer.nextToken();
								if (java.lang.String.instancehelper_charAt(text5, 0) == '[')
								{
									while (!java.lang.String.instancehelper_endsWith(text5, "]"))
									{
										text5 = stringTokenizer.nextToken();
									}
									text5 = stringTokenizer.nextToken();
								}
								this.split3ph(text5);
								string text6 = this.conv3ph();
								printWriter.print(new StringBuilder().append(text6).append(' ').toString());
							}
							printWriter.println();
						}
					}
				}
				printWriter.close();
				bufferedReader.close();
			}
			catch (IOException ex)
			{
				ex2 = ByteCodeHelper.MapException<IOException>(ex, 1);
				goto IL_131;
			}
			return;
			IL_131:
			IOException ex3 = ex2;
			Throwable.instancehelper_printStackTrace(ex3);
		}

		[LineNumberTable(new byte[]
		{
			101,
			108,
			223,
			6,
			103,
			116,
			103,
			108,
			101,
			162,
			103,
			116,
			103,
			108,
			101,
			130,
			98,
			102,
			103,
			116,
			109,
			100,
			140,
			109,
			114,
			104,
			117,
			127,
			4,
			108,
			105,
			116,
			164,
			127,
			16,
			110,
			148,
			126,
			105,
			159,
			6,
			134,
			133,
			102,
			103,
			116,
			109,
			100,
			140,
			109,
			114,
			104,
			117,
			127,
			4,
			108,
			105,
			116,
			104,
			126,
			108,
			105,
			116,
			104,
			126,
			105,
			191,
			6,
			134,
			133,
			102,
			103,
			116,
			109,
			114,
			104,
			117,
			127,
			4,
			108,
			105,
			116,
			104,
			126,
			108,
			105,
			116,
			104,
			126,
			108,
			105,
			116,
			104,
			126,
			105,
			223,
			6,
			134,
			101,
			102,
			191,
			0,
			2,
			98,
			135
		})]
		
		internal virtual void convertWordGrammar(string text)
		{
			IOException ex2;
			try
			{
				BufferedReader bufferedReader = new BufferedReader(new FileReader(text));
				PrintWriter printWriter = new PrintWriter(new FileWriter(new StringBuilder().append(text).append(".conv").toString()));
				for (;;)
				{
					string text2 = bufferedReader.readLine();
					if (text2 == null)
					{
						break;
					}
					printWriter.println(text2);
					if (java.lang.String.instancehelper_indexOf(text2, "\\data\\") == 0)
					{
						goto Block_3;
					}
				}
				printWriter.close();
				bufferedReader.close();
				return;
				Block_3:
				for (;;)
				{
					string text2 = bufferedReader.readLine();
					if (text2 == null)
					{
						break;
					}
					printWriter.println(text2);
					if (java.lang.String.instancehelper_indexOf(text2, "\\1-grams:") == 0)
					{
						goto Block_5;
					}
				}
				printWriter.close();
				bufferedReader.close();
				return;
				Block_5:
				int num = 0;
				while (num == 0)
				{
					string text2 = bufferedReader.readLine();
					if (text2 == null)
					{
						printWriter.close();
						bufferedReader.close();
						return;
					}
					if (java.lang.String.instancehelper_indexOf(text2, "\\2-grams:") == 0)
					{
						printWriter.println(text2);
						break;
					}
					if (java.lang.String.instancehelper_indexOf(text2, "\\end\\") == 0)
					{
						num = 1;
						printWriter.println(text2);
						break;
					}
					StringTokenizer stringTokenizer = new StringTokenizer(text2);
					if ((stringTokenizer & stringTokenizer.hasMoreTokens()) != null)
					{
						printWriter.print(new StringBuilder().append(stringTokenizer.nextToken()).append(' ').toString());
						if (stringTokenizer.hasMoreTokens())
						{
							string text3 = stringTokenizer.nextToken();
							string text4 = (string)this.wordConv.get(text3);
							if (text4 == null)
							{
								java.lang.System.err.println(new StringBuilder().append("WARNING word ").append(text3).append(" not in lexicon !").toString());
								this.addInConv(text3, this.wordConv);
								text4 = (string)this.wordConv.get(text3);
							}
							printWriter.print(new StringBuilder().append(text4).append(' ').toString());
							while (stringTokenizer.hasMoreTokens())
							{
								printWriter.print(new StringBuilder().append(stringTokenizer.nextToken()).append(' ').toString());
							}
						}
						printWriter.println();
					}
				}
				while (num == 0)
				{
					string text2 = bufferedReader.readLine();
					if (text2 == null)
					{
						printWriter.close();
						bufferedReader.close();
						return;
					}
					if (java.lang.String.instancehelper_indexOf(text2, "\\3-grams:") == 0)
					{
						printWriter.println(text2);
						break;
					}
					if (java.lang.String.instancehelper_indexOf(text2, "\\end\\") == 0)
					{
						num = 1;
						printWriter.println(text2);
						break;
					}
					StringTokenizer stringTokenizer = new StringTokenizer(text2);
					if ((stringTokenizer & stringTokenizer.hasMoreTokens()) != null)
					{
						printWriter.print(new StringBuilder().append(stringTokenizer.nextToken()).append(' ').toString());
						if (stringTokenizer.hasMoreTokens())
						{
							string text3 = stringTokenizer.nextToken();
							string text4 = (string)this.wordConv.get(text3);
							if (text4 == null)
							{
								text4 = text3;
							}
							printWriter.print(new StringBuilder().append(text4).append(' ').toString());
							if (stringTokenizer.hasMoreTokens())
							{
								text3 = stringTokenizer.nextToken();
								text4 = (string)this.wordConv.get(text3);
								if (text4 == null)
								{
									text4 = text3;
								}
								printWriter.print(new StringBuilder().append(text4).append(' ').toString());
								while (stringTokenizer.hasMoreTokens())
								{
									printWriter.print(new StringBuilder().append(stringTokenizer.nextToken()).append(' ').toString());
								}
							}
						}
						printWriter.println();
					}
				}
				while (num == 0)
				{
					string text2 = bufferedReader.readLine();
					if (text2 == null)
					{
						printWriter.close();
						bufferedReader.close();
						return;
					}
					if (java.lang.String.instancehelper_indexOf(text2, "\\end\\") == 0)
					{
						printWriter.println(text2);
						break;
					}
					StringTokenizer stringTokenizer = new StringTokenizer(text2);
					if ((stringTokenizer & stringTokenizer.hasMoreTokens()) != null)
					{
						printWriter.print(new StringBuilder().append(stringTokenizer.nextToken()).append(' ').toString());
						if (stringTokenizer.hasMoreTokens())
						{
							string text3 = stringTokenizer.nextToken();
							string text4 = (string)this.wordConv.get(text3);
							if (text4 == null)
							{
								text4 = text3;
							}
							printWriter.print(new StringBuilder().append(text4).append(' ').toString());
							if (stringTokenizer.hasMoreTokens())
							{
								text3 = stringTokenizer.nextToken();
								text4 = (string)this.wordConv.get(text3);
								if (text4 == null)
								{
									text4 = text3;
								}
								printWriter.print(new StringBuilder().append(text4).append(' ').toString());
								if (stringTokenizer.hasMoreTokens())
								{
									text3 = stringTokenizer.nextToken();
									text4 = (string)this.wordConv.get(text3);
									if (text4 == null)
									{
										text4 = text3;
									}
									printWriter.print(new StringBuilder().append(text4).append(' ').toString());
									while (stringTokenizer.hasMoreTokens())
									{
										printWriter.print(new StringBuilder().append(stringTokenizer.nextToken()).append(' ').toString());
									}
								}
							}
						}
						printWriter.println();
					}
				}
				printWriter.close();
				bufferedReader.close();
				goto IL_548;
			}
			catch (IOException ex)
			{
				ex2 = ByteCodeHelper.MapException<IOException>(ex, 1);
				goto IL_54A;
			}
			return;
			IL_548:
			return;
			IL_54A:
			IOException ex3 = ex2;
			Throwable.instancehelper_printStackTrace(ex3);
		}

		[LineNumberTable(new byte[]
		{
			160,
			195,
			98,
			98,
			98,
			130,
			108,
			112,
			112,
			112,
			109,
			112,
			109,
			112,
			235,
			56,
			235,
			76,
			134,
			103,
			104,
			104,
			127,
			15,
			104,
			99,
			127,
			15,
			136,
			99,
			127,
			15,
			136,
			99,
			127,
			15,
			168
		})]
		
		public static void main(string[] args)
		{
			string text = null;
			string text2 = null;
			string text3 = null;
			string text4 = null;
			for (int i = 0; i < args.Length; i++)
			{
				if (java.lang.String.instancehelper_equals(args[i], "-lex"))
				{
					i++;
					text2 = args[i];
				}
				else if (java.lang.String.instancehelper_equals(args[i], "-gram"))
				{
					i++;
					text4 = args[i];
				}
				else if (java.lang.String.instancehelper_equals(args[i], "-mmf"))
				{
					i++;
					text = args[i];
				}
				else if (java.lang.String.instancehelper_equals(args[i], "-filler"))
				{
					i++;
					text3 = args[i];
				}
			}
			if (text != null)
			{
				NamesConversion namesConversion = new NamesConversion();
				namesConversion.buildPhoneConversion(text);
				namesConversion.buildWordConversion(text2);
				java.lang.System.@out.println(new StringBuilder().append("converting phones in MMF to ").append(text).append(".conv").toString());
				namesConversion.convertMMF(text);
				if (text2 != null)
				{
					java.lang.System.@out.println(new StringBuilder().append("converting phones and words in lexicon to ").append(text2).append(".conv").toString());
					namesConversion.convertLexicon(text2);
				}
				if (text3 != null)
				{
					java.lang.System.@out.println(new StringBuilder().append("converting phones in filler to ").append(text3).append(".conv").toString());
					namesConversion.convertLexicon(text3);
				}
				if (text4 != null)
				{
					java.lang.System.@out.println(new StringBuilder().append("converting words in gram to ").append(text4).append(".conv").toString());
					namesConversion.convertWordGrammar(text4);
				}
			}
		}

		
		
		internal HashMap phoneConv;

		
		
		internal HashMap wordConv;

		internal string left;

		internal string @base;

		internal string right;
	}
}
