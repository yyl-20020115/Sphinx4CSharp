using System;

using System.Runtime.Serialization;
using System.Security.Permissions;
using edu.cmu.sphinx.jsgf.rule;
using IKVM.Attributes;
using IKVM.Runtime;
using java.io;
using java.lang;
using java.net;
using java.util;

namespace edu.cmu.sphinx.jsgf.parser
{
	[Implements(new string[]
	{
		"edu.cmu.sphinx.jsgf.parser.JSGFParserConstants"
	})]
	public class JSGFParser : java.lang.Object, JSGFParserConstants
	{
		
		public static void __<clinit>()
		{
		}

		[Throws(new string[]
		{
			"edu.cmu.sphinx.jsgf.JSGFGrammarParseException",
			"java.io.IOException"
		})]
		[LineNumberTable(new byte[]
		{
			160,
			109,
			118,
			103,
			107,
			127,
			20,
			143,
			114,
			135
		})]
		
		public static JSGFRuleGrammar newGrammarFromJSGF(URL url, JSGFRuleGrammarFactory factory)
		{
			BufferedInputStream.__<clinit>();
			BufferedInputStream bufferedInputStream = new BufferedInputStream(url.openStream(), 256);
			JSGFEncoding jsgfencoding = JSGFParser.getJSGFEncoding(bufferedInputStream);
			InputStreamReader i;
			if (jsgfencoding != null && jsgfencoding.encoding != null)
			{
				java.lang.System.@out.println(new StringBuilder().append("Grammar Character Encoding \"").append(jsgfencoding.encoding).append("\"").toString());
				i = new InputStreamReader(bufferedInputStream, jsgfencoding.encoding);
			}
			else
			{
				if (jsgfencoding == null)
				{
					java.lang.System.@out.println("WARNING: Grammar missing self identifying header");
				}
				i = new InputStreamReader(bufferedInputStream);
			}
			return JSGFParser.newGrammarFromJSGF(i, factory);
		}

		[LineNumberTable(new byte[]
		{
			162,
			241,
			104
		})]
		
		public JSGFParser(InputStream stream) : this(stream, null)
		{
		}

		[LineNumberTable(new byte[]
		{
			82,
			98,
			103,
			104,
			171,
			108,
			102,
			135,
			148,
			98,
			106,
			106,
			104,
			108,
			102,
			135,
			118,
			140,
			146,
			98,
			106,
			104,
			108,
			102,
			135,
			108,
			118,
			140,
			146,
			98,
			106,
			101,
			108,
			102,
			132,
			105,
			112,
			255,
			2,
			74,
			226,
			58,
			129,
			145,
			34,
			129,
			130,
			131,
			145,
			34,
			129,
			226,
			69,
			106,
			110,
			99,
			99,
			99,
			99,
			114,
			142,
			145,
			34,
			129,
			130,
			114,
			114,
			114
		})]
		
		private static JSGFEncoding getJSGFEncoding(BufferedInputStream bufferedInputStream)
		{
			int i = 0;
			byte[] array = new byte[2];
			byte[] array2 = new byte[80];
			bufferedInputStream.mark(256);
			JSGFEncoding result;
			try
			{
				if (bufferedInputStream.read(array, 0, 2) == 2)
				{
					if (array[0] == 35 && array[1] == 74)
					{
						i = 0;
						byte[] array3 = array2;
						int num = i;
						i++;
						array3[num] = array[0];
						byte[] array4 = array2;
						int num2 = i;
						i++;
						array4[num2] = array[1];
						while (i < 80)
						{
							if (bufferedInputStream.read(array, 0, 1) != 1)
							{
								bufferedInputStream.reset();
								return null;
							}
							if (array[0] == 10)
							{
								break;
							}
							if (array[0] == 13)
							{
								break;
							}
							byte[] array5 = array2;
							int num3 = i;
							i++;
							array5[num3] = array[0];
						}
					}
					else if (array[0] == 35 && array[1] == 0)
					{
						i = 0;
						byte[] array6 = array2;
						int num4 = i;
						i++;
						array6[num4] = array[0];
						while (i < 80)
						{
							if (bufferedInputStream.read(array, 0, 2) != 2)
							{
								bufferedInputStream.reset();
								return null;
							}
							if (array[1] != 0)
							{
								return null;
							}
							if (array[0] == 10)
							{
								break;
							}
							if (array[0] == 13)
							{
								break;
							}
							byte[] array7 = array2;
							int num5 = i;
							i++;
							array7[num5] = array[0];
						}
					}
					else if (array[0] == 0 && array[1] == 35)
					{
						i = 0;
						byte[] array8 = array2;
						int num6 = i;
						i++;
						array8[num6] = array[1];
						while (i < 80)
						{
							if (bufferedInputStream.read(array, 0, 2) != 2)
							{
								bufferedInputStream.reset();
								return null;
							}
							if (array[0] != 0)
							{
								return null;
							}
							if (array[1] == 10)
							{
								break;
							}
							if (array[1] == 13)
							{
								break;
							}
							byte[] array9 = array2;
							int num7 = i;
							i++;
							array9[num7] = array[1];
						}
					}
					goto IL_173;
				}
				bufferedInputStream.reset();
				result = null;
			}
			catch (IOException ex)
			{
				goto IL_17F;
			}
			return result;
			IL_173:
			if (i == 0)
			{
				try
				{
					bufferedInputStream.reset();
				}
				catch (IOException ex2)
				{
				}
				return null;
			}
			string text = java.lang.String.newhelper(array2, 0, i);
			StringTokenizer stringTokenizer = new StringTokenizer(text, " \t\n\r\f;");
			string text2 = null;
			string text3 = null;
			string text4 = null;
			string text5 = null;
			if (stringTokenizer.hasMoreTokens())
			{
				text2 = stringTokenizer.nextToken();
			}
			if (!java.lang.String.instancehelper_equals(text2, "#JSGF"))
			{
				try
				{
					bufferedInputStream.reset();
				}
				catch (IOException ex3)
				{
				}
				return null;
			}
			if (stringTokenizer.hasMoreTokens())
			{
				text3 = stringTokenizer.nextToken();
			}
			if (stringTokenizer.hasMoreTokens())
			{
				text4 = stringTokenizer.nextToken();
			}
			if (stringTokenizer.hasMoreTokens())
			{
				text5 = stringTokenizer.nextToken();
			}
			return new JSGFEncoding(text3, text4, text5);
			IL_17F:
			try
			{
				bufferedInputStream.reset();
			}
			catch (IOException ex4)
			{
			}
			return null;
		}

		[LineNumberTable(new byte[]
		{
			163,
			10,
			232,
			21,
			237,
			77,
			108,
			103,
			231,
			160,
			92,
			236,
			111,
			139,
			103,
			237,
			159,
			143,
			110,
			118,
			107,
			103,
			103,
			118,
			127,
			0
		})]
		
		public JSGFParser(Reader stream)
		{
			this.jj_la1 = new int[26];
			this.jj_2_rtns = new JSGFParser.JJCalls[1];
			this.jj_rescan = false;
			this.jj_gc = 0;
			this.jj_ls = new JSGFParser.LookaheadSuccess(null);
			this.jj_expentries = new ArrayList();
			this.jj_kind = -1;
			this.jj_lasttokens = new int[100];
			this.jj_input_stream = new JavaCharStream(stream, 1, 1);
			JSGFParserTokenManager.__<clinit>();
			this.token_source = new JSGFParserTokenManager(this.jj_input_stream);
			this.token = new Token();
			this.jj_ntk = -1;
			this.jj_gen = 0;
			for (int i = 0; i < 26; i++)
			{
				this.jj_la1[i] = -1;
			}
			for (int i = 0; i < this.jj_2_rtns.Length; i++)
			{
				this.jj_2_rtns[i] = new JSGFParser.JJCalls();
			}
		}

		[Throws(new string[]
		{
			"edu.cmu.sphinx.jsgf.parser.ParseException"
		})]
		[LineNumberTable(new byte[]
		{
			160,
			152,
			98,
			159,
			3,
			102,
			130,
			174,
			168,
			191,
			3,
			130,
			110,
			130,
			201,
			223,
			11,
			130,
			110,
			130,
			137,
			104
		})]
		
		public JSGFRuleGrammar GrammarUnit(JSGFRuleGrammarFactory factory)
		{
			if (((this.jj_ntk != -1) ? this.jj_ntk : this.jj_ntk()) == 23)
			{
				this.IdentHeader();
			}
			else
			{
				this.jj_la1[0] = this.jj_gen;
			}
			JSGFRuleGrammar jsgfruleGrammar = this.GrammarDeclaration(factory);
			while (((this.jj_ntk != -1) ? this.jj_ntk : this.jj_ntk()) == 14)
			{
				this.ImportDeclaration(jsgfruleGrammar);
			}
			this.jj_la1[1] = this.jj_gen;
			for (;;)
			{
				int num = (this.jj_ntk != -1) ? this.jj_ntk : this.jj_ntk();
				if (num != 15)
				{
					if (num != 28)
					{
						break;
					}
				}
				this.RuleDeclaration(jsgfruleGrammar);
			}
			this.jj_la1[2] = this.jj_gen;
			this.jj_consume_token(0);
			return jsgfruleGrammar;
		}

		[LineNumberTable(new byte[]
		{
			162,
			254,
			104
		})]
		
		public virtual void ReInit(InputStream stream)
		{
			this.ReInit(stream, null);
		}

		[LineNumberTable(new byte[]
		{
			163,
			21,
			110,
			113,
			107,
			103,
			103,
			118,
			127,
			0
		})]
		
		public virtual void ReInit(Reader stream)
		{
			this.jj_input_stream.ReInit(stream, 1, 1);
			this.token_source.ReInit(this.jj_input_stream);
			this.token = new Token();
			this.jj_ntk = -1;
			this.jj_gen = 0;
			for (int i = 0; i < 26; i++)
			{
				this.jj_la1[i] = -1;
			}
			for (int i = 0; i < this.jj_2_rtns.Length; i++)
			{
				this.jj_2_rtns[i] = new JSGFParser.JJCalls();
			}
		}

		[Throws(new string[]
		{
			"edu.cmu.sphinx.jsgf.JSGFGrammarParseException"
		})]
		[LineNumberTable(new byte[]
		{
			65,
			98,
			103,
			141,
			171,
			108,
			119,
			97,
			104,
			127,
			1
		})]
		
		public static JSGFRuleGrammar newGrammarFromJSGF(Reader i, JSGFRuleGrammarFactory factory)
		{
			if (JSGFParser.parser == null)
			{
				JSGFParser.parser = new JSGFParser(i);
			}
			else
			{
				JSGFParser.parser.ReInit(i);
			}
			JSGFRuleGrammar result;
			ParseException ex2;
			try
			{
				JSGFRuleGrammar jsgfruleGrammar = JSGFParser.parser.GrammarUnit(factory);
				result = jsgfruleGrammar;
			}
			catch (ParseException ex)
			{
				ex2 = ByteCodeHelper.MapException<ParseException>(ex, 1);
				goto IL_40;
			}
			return result;
			IL_40:
			ParseException ex3 = ex2;
			Token currentToken = ex3.currentToken;
			JSGFGrammarParseException ex4 = new JSGFGrammarParseException(currentToken.beginLine, currentToken.beginColumn, "Grammar Error", Throwable.instancehelper_getMessage(ex3));
			throw Throwable.__<unmap>(ex4);
		}

		[Throws(new string[]
		{
			"edu.cmu.sphinx.jsgf.parser.ParseException"
		})]
		[LineNumberTable(new byte[]
		{
			161,
			88,
			166,
			102,
			255,
			109,
			75,
			103,
			168,
			191,
			3,
			130,
			110,
			133,
			105,
			103,
			202,
			103,
			103,
			104,
			173,
			105,
			103,
			103,
			104,
			109,
			191,
			3,
			130,
			111,
			226,
			69,
			111,
			104,
			139,
			104,
			105,
			136
		})]
		
		public JSGFRuleAlternatives alternatives()
		{
			ArrayList arrayList = new ArrayList();
			ArrayList arrayList2 = new ArrayList();
			switch ((this.jj_ntk != -1) ? this.jj_ntk : this.jj_ntk())
			{
			case 13:
			case 14:
			case 15:
			case 16:
			case 18:
			case 21:
			case 23:
			case 28:
			case 36:
			case 38:
			{
				JSGFRuleSequence jsgfruleSequence = this.sequence();
				arrayList.add(jsgfruleSequence);
				while (((this.jj_ntk != -1) ? this.jj_ntk : this.jj_ntk()) == 33)
				{
					this.jj_consume_token(33);
					jsgfruleSequence = this.sequence();
					arrayList.add(jsgfruleSequence);
				}
				this.jj_la1[8] = this.jj_gen;
				goto IL_19E;
			}
			case 34:
			{
				float num = this.weight();
				JSGFRuleSequence jsgfruleSequence = this.sequence();
				arrayList.add(jsgfruleSequence);
				arrayList2.add(Float.valueOf(num));
				do
				{
					this.jj_consume_token(33);
					num = this.weight();
					jsgfruleSequence = this.sequence();
					arrayList.add(jsgfruleSequence);
					arrayList2.add(Float.valueOf(num));
				}
				while (((this.jj_ntk != -1) ? this.jj_ntk : this.jj_ntk()) == 33);
				this.jj_la1[9] = this.jj_gen;
				goto IL_19E;
			}
			}
			this.jj_la1[10] = this.jj_gen;
			this.jj_consume_token(-1);
			
			throw new ParseException();
			IL_19E:
			JSGFRuleAlternatives jsgfruleAlternatives = new JSGFRuleAlternatives(arrayList);
			if (arrayList2.size() > 0)
			{
				jsgfruleAlternatives.setWeights(arrayList2);
			}
			return jsgfruleAlternatives;
		}

		[LineNumberTable(new byte[]
		{
			163,
			115,
			120,
			159,
			13
		})]
		
		private int jj_ntk()
		{
			Token token = this.token.next;
			bool flag = token != null;
			this.jj_nt = token;
			int kind;
			if (!flag)
			{
				Token token2 = this.token;
				token = this.token_source.getNextToken();
				Token token3 = token2;
				Token token4 = token;
				token3.next = token;
				kind = token4.kind;
				int result = kind;
				this.jj_ntk = kind;
				return result;
			}
			kind = this.jj_nt.kind;
			int result2 = kind;
			this.jj_ntk = kind;
			return result2;
		}

		[Throws(new string[]
		{
			"edu.cmu.sphinx.jsgf.parser.ParseException"
		})]
		[LineNumberTable(new byte[]
		{
			160,
			212,
			105,
			105,
			159,
			3,
			105,
			159,
			3,
			105,
			130,
			174,
			130,
			174,
			105
		})]
		
		public void IdentHeader()
		{
			this.jj_consume_token(23);
			this.jj_consume_token(27);
			if (((this.jj_ntk != -1) ? this.jj_ntk : this.jj_ntk()) == 23)
			{
				this.jj_consume_token(23);
				if (((this.jj_ntk != -1) ? this.jj_ntk : this.jj_ntk()) == 23)
				{
					this.jj_consume_token(23);
				}
				else
				{
					this.jj_la1[3] = this.jj_gen;
				}
			}
			else
			{
				this.jj_la1[4] = this.jj_gen;
			}
			this.jj_consume_token(26);
		}

		[Throws(new string[]
		{
			"edu.cmu.sphinx.jsgf.parser.ParseException"
		})]
		[LineNumberTable(new byte[]
		{
			160,
			195,
			98,
			98,
			105,
			103,
			105,
			104,
			110,
			127,
			5,
			98,
			177
		})]
		
		public JSGFRuleGrammar GrammarDeclaration(JSGFRuleGrammarFactory factory)
		{
			Token token = this.jj_consume_token(13);
			string name = this.Name();
			this.jj_consume_token(26);
			JSGFRuleGrammar jsgfruleGrammar = factory.newGrammar(name);
			if (jsgfruleGrammar != null && token != null && token.specialToken != null && token.specialToken.image != null && java.lang.String.instancehelper_startsWith(token.specialToken.image, "/**"))
			{
				JSGFRuleGrammar jsgfruleGrammar2 = jsgfruleGrammar;
				jsgfruleGrammar2.addGrammarDocComment(token.specialToken.image);
			}
			return jsgfruleGrammar;
		}

		[Throws(new string[]
		{
			"edu.cmu.sphinx.jsgf.parser.ParseException"
		})]
		[LineNumberTable(new byte[]
		{
			160,
			234,
			130,
			98,
			105,
			105,
			103,
			159,
			3,
			105,
			105,
			98,
			130,
			174,
			105,
			137,
			126,
			103,
			99,
			103,
			115,
			127,
			5,
			99,
			211
		})]
		
		public void ImportDeclaration(JSGFRuleGrammar grammar)
		{
			int num = 0;
			Token token = this.jj_consume_token(14);
			this.jj_consume_token(28);
			string text = this.Name();
			if (((this.jj_ntk != -1) ? this.jj_ntk : this.jj_ntk()) == 29)
			{
				this.jj_consume_token(29);
				this.jj_consume_token(30);
				num = 1;
			}
			else
			{
				this.jj_la1[5] = this.jj_gen;
			}
			this.jj_consume_token(31);
			this.jj_consume_token(26);
			if (num != 0)
			{
				text = new StringBuilder().append(text).append(".*").toString();
			}
			JSGFRuleName jsgfruleName = new JSGFRuleName(text);
			if (grammar != null)
			{
				grammar.addImport(jsgfruleName);
				if (grammar is JSGFRuleGrammar && token != null && token.specialToken != null && token.specialToken.image != null && java.lang.String.instancehelper_startsWith(token.specialToken.image, "/**"))
				{
					grammar.addImportDocComment(jsgfruleName, token.specialToken.image);
				}
			}
		}

		[Throws(new string[]
		{
			"edu.cmu.sphinx.jsgf.parser.ParseException"
		})]
		[LineNumberTable(new byte[]
		{
			161,
			50,
			162,
			98,
			98,
			159,
			3,
			105,
			98,
			130,
			174,
			105,
			103,
			105,
			105,
			104,
			137,
			102,
			106,
			99,
			127,
			8,
			127,
			6,
			114,
			105,
			244,
			69,
			2,
			97,
			159,
			5
		})]
		
		public void RuleDeclaration(JSGFRuleGrammar grammar)
		{
			int isPublic = 0;
			Token token = null;
			if (((this.jj_ntk != -1) ? this.jj_ntk : this.jj_ntk()) == 15)
			{
				token = this.jj_consume_token(15);
				isPublic = 1;
			}
			else
			{
				this.jj_la1[7] = this.jj_gen;
			}
			Token token2 = this.jj_consume_token(28);
			string text = this.ruleDef();
			this.jj_consume_token(31);
			this.jj_consume_token(32);
			JSGFRuleAlternatives rule = this.alternatives();
			this.jj_consume_token(26);
			try
			{
				if (grammar != null)
				{
					grammar.setRule(text, rule, isPublic != 0);
					string text2 = null;
					if (token != null && token.specialToken != null && token.specialToken.image != null)
					{
						text2 = token.specialToken.image;
					}
					else if (token2 != null && token2.specialToken != null && token2.specialToken.image != null)
					{
						text2 = token2.specialToken.image;
					}
					if (text2 != null && java.lang.String.instancehelper_startsWith(text2, "/**"))
					{
						JSGFParser.extractKeywords(grammar, text, text2);
						grammar.addRuleDocComment(text, text2);
					}
				}
			}
			catch (IllegalArgumentException ex)
			{
				goto IL_101;
			}
			return;
			IL_101:
			java.lang.System.@out.println(new StringBuilder().append("ERROR SETTING JSGFRule ").append(text).toString());
		}

		[Throws(new string[]
		{
			"edu.cmu.sphinx.jsgf.parser.ParseException"
		})]
		[LineNumberTable(new byte[]
		{
			163,
			50,
			127,
			3,
			127,
			2,
			103,
			113,
			110,
			121,
			103,
			110,
			107,
			100,
			119,
			235,
			60,
			232,
			72,
			135,
			103,
			103
		})]
		
		private Token jj_consume_token(int num)
		{
			Token token;
			if ((token = this.token).next != null)
			{
				this.token = this.token.next;
			}
			else
			{
				Token token2 = this.token;
				Token nextToken = this.token_source.getNextToken();
				Token token3 = token2;
				Token token4 = nextToken;
				token3.next = nextToken;
				this.token = token4;
			}
			this.jj_ntk = -1;
			if (this.token.kind == num)
			{
				this.jj_gen++;
				int num2 = this.jj_gc + 1;
				int num3 = num2;
				this.jj_gc = num2;
				if (num3 > 100)
				{
					this.jj_gc = 0;
					for (int i = 0; i < this.jj_2_rtns.Length; i++)
					{
						for (JSGFParser.JJCalls jjcalls = this.jj_2_rtns[i]; jjcalls != null; jjcalls = jjcalls.next)
						{
							if (jjcalls.gen < this.jj_gen)
							{
								jjcalls.first = null;
							}
						}
					}
				}
				return this.token;
			}
			this.token = token;
			this.jj_kind = num;
			throw Throwable.__<unmap>(this.generateParseException());
		}

		[Throws(new string[]
		{
			"edu.cmu.sphinx.jsgf.parser.ParseException"
		})]
		[LineNumberTable(new byte[]
		{
			161,
			13,
			102,
			159,
			27,
			105,
			130,
			105,
			130,
			105,
			130,
			105,
			130,
			110,
			104,
			139,
			173,
			233,
			69,
			105,
			105,
			105,
			143
		})]
		
		public string Name()
		{
			StringBuilder stringBuilder = new StringBuilder();
			int num = (this.jj_ntk != -1) ? this.jj_ntk : this.jj_ntk();
			Token token;
			if (num == 13)
			{
				token = this.jj_consume_token(13);
			}
			else if (num == 14)
			{
				token = this.jj_consume_token(14);
			}
			else if (num == 15)
			{
				token = this.jj_consume_token(15);
			}
			else
			{
				if (num != 23)
				{
					this.jj_la1[6] = this.jj_gen;
					this.jj_consume_token(-1);
					
					throw new ParseException();
				}
				token = this.jj_consume_token(23);
			}
			stringBuilder.append(token.image);
			while (this.jj_2_1(2))
			{
				this.jj_consume_token(29);
				Token token2 = this.jj_consume_token(23);
				stringBuilder.append('.');
				stringBuilder.append(token2.image);
			}
			return stringBuilder.toString();
		}

		[LineNumberTable(new byte[]
		{
			162,
			202,
			126,
			147,
			78,
			8,
			100,
			103,
			75,
			34
		})]
		
		private bool jj_2_1(int num)
		{
			this.jj_la = num;
			Token token = this.token;
			Token token2 = token;
			this.jj_scanpos = token;
			this.jj_lastpos = token2;
			int result;
			try
			{
				try
				{
					result = (this.jj_3_1() ? 0 : 1);
				}
				catch (JSGFParser.LookaheadSuccess lookaheadSuccess)
				{
					goto IL_3C;
				}
			}
			catch
			{
				this.jj_save(0, num);
				throw;
			}
			this.jj_save(0, num);
			return result != 0;
			IL_3C:
			int result2;
			try
			{
				result2 = 1;
			}
			finally
			{
				this.jj_save(0, num);
			}
			return result2 != 0;
		}

		[Throws(new string[]
		{
			"edu.cmu.sphinx.jsgf.parser.ParseException"
		})]
		[LineNumberTable(new byte[]
		{
			161,
			158,
			159,
			46,
			105,
			130,
			105,
			130,
			105,
			130,
			105,
			130,
			105,
			130,
			111,
			104,
			139
		})]
		
		public string ruleDef()
		{
			Token token;
			switch ((this.jj_ntk != -1) ? this.jj_ntk : this.jj_ntk())
			{
			case 13:
				token = this.jj_consume_token(13);
				goto IL_A6;
			case 14:
				token = this.jj_consume_token(14);
				goto IL_A6;
			case 15:
				token = this.jj_consume_token(15);
				goto IL_A6;
			case 16:
				token = this.jj_consume_token(16);
				goto IL_A6;
			case 23:
				token = this.jj_consume_token(23);
				goto IL_A6;
			}
			this.jj_la1[11] = this.jj_gen;
			this.jj_consume_token(-1);
			
			throw new ParseException();
			IL_A6:
			return token.image;
		}

		[LineNumberTable(new byte[]
		{
			160,
			137,
			98,
			117,
			120,
			100,
			103,
			109,
			133,
			116,
			100,
			101
		})]
		
		internal static void extractKeywords(JSGFRuleGrammar jsgfruleGrammar, string ruleName, string text)
		{
			int num = 0;
			while ((num = java.lang.String.instancehelper_indexOf(text, "@example ", num) + 9) > 9)
			{
				int num2 = java.lang.Math.max(java.lang.String.instancehelper_indexOf(text, 13, num), java.lang.String.instancehelper_indexOf(text, 10, num));
				if (num2 < 0)
				{
					num2 = java.lang.String.instancehelper_length(text);
					if (java.lang.String.instancehelper_endsWith(text, "*/"))
					{
						num2 += -2;
					}
				}
				jsgfruleGrammar.addSampleSentence(ruleName, java.lang.String.instancehelper_trim(java.lang.String.instancehelper_substring(text, num, num2)));
				num = num2 + 1;
			}
		}

		[Throws(new string[]
		{
			"edu.cmu.sphinx.jsgf.parser.ParseException"
		})]
		[LineNumberTable(new byte[]
		{
			161,
			186,
			166,
			103,
			104,
			255,
			106,
			76,
			133,
			111,
			162
		})]
		
		public JSGFRuleSequence sequence()
		{
			ArrayList arrayList = new ArrayList();
			for (;;)
			{
				JSGFRule jsgfrule = this.item();
				arrayList.add(jsgfrule);
				switch ((this.jj_ntk != -1) ? this.jj_ntk : this.jj_ntk())
				{
				case 13:
				case 14:
				case 15:
				case 16:
				case 18:
				case 21:
				case 23:
				case 28:
				case 36:
				case 38:
					continue;
				}
				break;
			}
			this.jj_la1[12] = this.jj_gen;
			return new JSGFRuleSequence(arrayList);
		}

		[Throws(new string[]
		{
			"edu.cmu.sphinx.jsgf.parser.ParseException"
		})]
		[LineNumberTable(new byte[]
		{
			161,
			216,
			105,
			159,
			11,
			105,
			130,
			105,
			130,
			111,
			104,
			139,
			105
		})]
		
		public float weight()
		{
			this.jj_consume_token(34);
			int num = (this.jj_ntk != -1) ? this.jj_ntk : this.jj_ntk();
			Token token;
			if (num == 16)
			{
				token = this.jj_consume_token(16);
			}
			else
			{
				if (num != 18)
				{
					this.jj_la1[13] = this.jj_gen;
					this.jj_consume_token(-1);
					
					throw new ParseException();
				}
				token = this.jj_consume_token(18);
			}
			this.jj_consume_token(34);
			return Float.valueOf(token.image).floatValue();
		}

		[Throws(new string[]
		{
			"edu.cmu.sphinx.jsgf.parser.ParseException"
		})]
		[LineNumberTable(new byte[]
		{
			161,
			237,
			98,
			98,
			255,
			109,
			73,
			255,
			66,
			72,
			103,
			130,
			103,
			130,
			111,
			104,
			139,
			191,
			14,
			159,
			11,
			105,
			98,
			130,
			105,
			98,
			130,
			111,
			104,
			203,
			175,
			159,
			3,
			103,
			133,
			175,
			133,
			105,
			103,
			105,
			191,
			14,
			159,
			11,
			105,
			98,
			130,
			105,
			98,
			130,
			111,
			104,
			203,
			175,
			159,
			3,
			103,
			133,
			175,
			133,
			105,
			103,
			105,
			98,
			159,
			3,
			103,
			130,
			175,
			130,
			111,
			104,
			139,
			108,
			102,
			124,
			108,
			115,
			141,
			105,
			130
		})]
		
		public JSGFRule item()
		{
			ArrayList arrayList = null;
			int num = -1;
			JSGFRule jsgfrule;
			switch ((this.jj_ntk != -1) ? this.jj_ntk : this.jj_ntk())
			{
			case 13:
			case 14:
			case 15:
			case 16:
			case 18:
			case 21:
			case 23:
			case 28:
			{
				switch ((this.jj_ntk != -1) ? this.jj_ntk : this.jj_ntk())
				{
				case 13:
				case 14:
				case 15:
				case 16:
				case 18:
				case 21:
				case 23:
					jsgfrule = this.terminal();
					goto IL_125;
				case 28:
					jsgfrule = this.ruleRef();
					goto IL_125;
				}
				this.jj_la1[14] = this.jj_gen;
				this.jj_consume_token(-1);
				
				throw new ParseException();
				IL_125:
				int num2 = (this.jj_ntk != -1) ? this.jj_ntk : this.jj_ntk();
				if (num2 != 30)
				{
					if (num2 != 35)
					{
						this.jj_la1[16] = this.jj_gen;
						goto IL_1C7;
					}
				}
				int num3 = (this.jj_ntk != -1) ? this.jj_ntk : this.jj_ntk();
				if (num3 == 30)
				{
					this.jj_consume_token(30);
					num = 4;
				}
				else
				{
					if (num3 != 35)
					{
						this.jj_la1[15] = this.jj_gen;
						this.jj_consume_token(-1);
						
						throw new ParseException();
					}
					this.jj_consume_token(35);
					num = 3;
				}
				IL_1C7:
				if (((this.jj_ntk != -1) ? this.jj_ntk : this.jj_ntk()) == 22)
				{
					arrayList = this.tags();
					goto IL_37F;
				}
				this.jj_la1[17] = this.jj_gen;
				goto IL_37F;
			}
			case 36:
			{
				this.jj_consume_token(36);
				jsgfrule = this.alternatives();
				this.jj_consume_token(37);
				int num4 = (this.jj_ntk != -1) ? this.jj_ntk : this.jj_ntk();
				if (num4 != 30)
				{
					if (num4 != 35)
					{
						this.jj_la1[19] = this.jj_gen;
						goto IL_2C4;
					}
				}
				int num5 = (this.jj_ntk != -1) ? this.jj_ntk : this.jj_ntk();
				if (num5 == 30)
				{
					this.jj_consume_token(30);
					num = 4;
				}
				else
				{
					if (num5 != 35)
					{
						this.jj_la1[18] = this.jj_gen;
						this.jj_consume_token(-1);
						
						throw new ParseException();
					}
					this.jj_consume_token(35);
					num = 3;
				}
				IL_2C4:
				if (((this.jj_ntk != -1) ? this.jj_ntk : this.jj_ntk()) == 22)
				{
					arrayList = this.tags();
					goto IL_37F;
				}
				this.jj_la1[20] = this.jj_gen;
				goto IL_37F;
			}
			case 38:
				this.jj_consume_token(38);
				jsgfrule = this.alternatives();
				this.jj_consume_token(39);
				num = 2;
				if (((this.jj_ntk != -1) ? this.jj_ntk : this.jj_ntk()) == 22)
				{
					arrayList = this.tags();
					goto IL_37F;
				}
				this.jj_la1[21] = this.jj_gen;
				goto IL_37F;
			}
			this.jj_la1[22] = this.jj_gen;
			this.jj_consume_token(-1);
			
			throw new ParseException();
			IL_37F:
			if (num != -1)
			{
				jsgfrule = new JSGFRuleCount(jsgfrule, num);
			}
			if (arrayList != null)
			{
				Iterator iterator = arrayList.iterator();
				while (iterator.hasNext())
				{
					string text = (string)iterator.next();
					if (java.lang.String.instancehelper_charAt(text, 0) == '{')
					{
						text = java.lang.String.instancehelper_substring(text, 1, java.lang.String.instancehelper_length(text) - 1);
						text = java.lang.String.instancehelper_replace(text, '\\', ' ');
					}
					jsgfrule = new JSGFRuleTag(jsgfrule, text);
				}
			}
			return jsgfrule;
		}

		[Throws(new string[]
		{
			"edu.cmu.sphinx.jsgf.parser.ParseException"
		})]
		[LineNumberTable(new byte[]
		{
			162,
			133,
			159,
			46,
			105,
			133,
			105,
			130,
			105,
			130,
			105,
			130,
			105,
			130,
			105,
			130,
			105,
			130,
			111,
			104,
			139,
			103,
			127,
			11,
			103
		})]
		
		public JSGFRule terminal()
		{
			Token token;
			switch ((this.jj_ntk != -1) ? this.jj_ntk : this.jj_ntk())
			{
			case 13:
				token = this.jj_consume_token(13);
				goto IL_BF;
			case 14:
				token = this.jj_consume_token(14);
				goto IL_BF;
			case 15:
				token = this.jj_consume_token(15);
				goto IL_BF;
			case 16:
				token = this.jj_consume_token(16);
				goto IL_BF;
			case 18:
				token = this.jj_consume_token(18);
				goto IL_BF;
			case 21:
				token = this.jj_consume_token(21);
				goto IL_BF;
			case 23:
				token = this.jj_consume_token(23);
				goto IL_BF;
			}
			this.jj_la1[24] = this.jj_gen;
			this.jj_consume_token(-1);
			
			throw new ParseException();
			IL_BF:
			string text = token.image;
			if (java.lang.String.instancehelper_startsWith(text, "\"") && java.lang.String.instancehelper_endsWith(text, "\""))
			{
				text = java.lang.String.instancehelper_substring(text, 1, java.lang.String.instancehelper_length(text) - 1);
			}
			return new JSGFRuleToken(text);
		}

		[Throws(new string[]
		{
			"edu.cmu.sphinx.jsgf.parser.ParseException"
		})]
		[LineNumberTable(new byte[]
		{
			162,
			170,
			105,
			103,
			105,
			103
		})]
		
		public JSGFRuleName ruleRef()
		{
			this.jj_consume_token(28);
			string name = this.Name();
			this.jj_consume_token(31);
			return new JSGFRuleName(name);
		}

		[Throws(new string[]
		{
			"edu.cmu.sphinx.jsgf.parser.ParseException"
		})]
		
		[LineNumberTable(new byte[]
		{
			162,
			112,
			166,
			105,
			109,
			191,
			3,
			130,
			111,
			162
		})]
		
		public ArrayList tags()
		{
			ArrayList arrayList = new ArrayList();
			do
			{
				Token token = this.jj_consume_token(22);
				arrayList.add(token.image);
			}
			while (((this.jj_ntk != -1) ? this.jj_ntk : this.jj_ntk()) == 22);
			this.jj_la1[23] = this.jj_gen;
			return arrayList;
		}

		[LineNumberTable(new byte[]
		{
			162,
			209,
			108,
			108
		})]
		
		private bool jj_3_1()
		{
			return this.jj_scan_token(29) || this.jj_scan_token(23);
		}

		[LineNumberTable(new byte[]
		{
			163,
			214,
			105,
			110,
			123,
			137,
			127,
			9
		})]
		
		private void jj_save(int num, int num2)
		{
			JSGFParser.JJCalls jjcalls = this.jj_2_rtns[num];
			while (jjcalls.gen > this.jj_gen)
			{
				if (jjcalls.next == null)
				{
					JSGFParser.JJCalls jjcalls2 = jjcalls;
					JSGFParser.JJCalls jjcalls3 = new JSGFParser.JJCalls();
					JSGFParser.JJCalls jjcalls4 = jjcalls2;
					JSGFParser.JJCalls jjcalls5 = jjcalls3;
					jjcalls4.next = jjcalls3;
					jjcalls = jjcalls5;
					break;
				}
				jjcalls = jjcalls.next;
			}
			jjcalls.gen = this.jj_gen + num2 - this.jj_la;
			jjcalls.first = this.token;
			jjcalls.arg = num2;
		}

		[LineNumberTable(new byte[]
		{
			163,
			76,
			113,
			110,
			109,
			159,
			15,
			190,
			145,
			104,
			105,
			121,
			139,
			112,
			127,
			3
		})]
		
		private bool jj_scan_token(int num)
		{
			if (this.jj_scanpos == this.jj_lastpos)
			{
				this.jj_la--;
				if (this.jj_scanpos.next == null)
				{
					Token token = this.jj_scanpos;
					Token token2 = this.token_source.getNextToken();
					Token token3 = token;
					Token token4 = token2;
					token3.next = token2;
					token2 = token4;
					Token token5 = token2;
					this.jj_scanpos = token2;
					this.jj_lastpos = token5;
				}
				else
				{
					Token token2 = this.jj_scanpos.next;
					Token token6 = token2;
					this.jj_scanpos = token2;
					this.jj_lastpos = token6;
				}
			}
			else
			{
				this.jj_scanpos = this.jj_scanpos.next;
			}
			if (this.jj_rescan)
			{
				int num2 = 0;
				Token next = this.token;
				while (next != null && next != this.jj_scanpos)
				{
					num2++;
					next = next.next;
				}
				if (next != null)
				{
					this.jj_add_error_token(num, num2);
				}
			}
			if (this.jj_scanpos.kind != num)
			{
				return true;
			}
			if (this.jj_la == 0 && this.jj_scanpos == this.jj_lastpos)
			{
				throw Throwable.__<unmap>(this.jj_ls);
			}
			return false;
		}

		[LineNumberTable(new byte[]
		{
			162,
			243,
			232,
			44,
			237,
			77,
			108,
			103,
			231,
			160,
			92,
			236,
			111,
			139,
			103,
			237,
			159,
			120,
			127,
			17,
			118,
			107,
			103,
			103,
			118,
			127,
			0
		})]
		
		public JSGFParser(InputStream stream, string encoding)
		{
			this.jj_la1 = new int[26];
			this.jj_2_rtns = new JSGFParser.JJCalls[1];
			this.jj_rescan = false;
			this.jj_gc = 0;
			this.jj_ls = new JSGFParser.LookaheadSuccess(null);
			this.jj_expentries = new ArrayList();
			this.jj_kind = -1;
			this.jj_lasttokens = new int[100];
			UnsupportedEncodingException ex2;
			try
			{
				this.jj_input_stream = new JavaCharStream(stream, encoding, 1, 1);
			}
			catch (UnsupportedEncodingException ex)
			{
				ex2 = ByteCodeHelper.MapException<UnsupportedEncodingException>(ex, 1);
				goto IL_78;
			}
			JSGFParserTokenManager.__<clinit>();
			this.token_source = new JSGFParserTokenManager(this.jj_input_stream);
			this.token = new Token();
			this.jj_ntk = -1;
			this.jj_gen = 0;
			for (int i = 0; i < 26; i++)
			{
				this.jj_la1[i] = -1;
			}
			for (int i = 0; i < this.jj_2_rtns.Length; i++)
			{
				this.jj_2_rtns[i] = new JSGFParser.JJCalls();
			}
			return;
			IL_78:
			UnsupportedEncodingException ex3 = ex2;
			Exception ex4 = ex3;
			
			throw new RuntimeException(ex4);
		}

		[LineNumberTable(new byte[]
		{
			163,
			1,
			127,
			17,
			113,
			107,
			103,
			103,
			118,
			127,
			0
		})]
		
		public virtual void ReInit(InputStream stream, string encoding)
		{
			UnsupportedEncodingException ex2;
			try
			{
				this.jj_input_stream.ReInit(stream, encoding, 1, 1);
			}
			catch (UnsupportedEncodingException ex)
			{
				ex2 = ByteCodeHelper.MapException<UnsupportedEncodingException>(ex, 1);
				goto IL_1E;
			}
			this.token_source.ReInit(this.jj_input_stream);
			this.token = new Token();
			this.jj_ntk = -1;
			this.jj_gen = 0;
			for (int i = 0; i < 26; i++)
			{
				this.jj_la1[i] = -1;
			}
			for (int i = 0; i < this.jj_2_rtns.Length; i++)
			{
				this.jj_2_rtns[i] = new JSGFParser.JJCalls();
			}
			return;
			IL_1E:
			UnsupportedEncodingException ex3 = ex2;
			Exception ex4 = ex3;
			
			throw new RuntimeException(ex4);
		}

		[LineNumberTable(new byte[]
		{
			163,
			153,
			107,
			104,
			105,
			105,
			135,
			106,
			112,
			103,
			112,
			132,
			112,
			231,
			59,
			6,
			233,
			76,
			103,
			101,
			108,
			105,
			242,
			60,
			230,
			71,
			103,
			102,
			104,
			113,
			112,
			52,
			166
		})]
		
		public virtual ParseException generateParseException()
		{
			this.jj_expentries.clear();
			bool[] array = new bool[40];
			if (this.jj_kind >= 0)
			{
				array[this.jj_kind] = true;
				this.jj_kind = -1;
			}
			for (int i = 0; i < 26; i++)
			{
				if (this.jj_la1[i] == this.jj_gen)
				{
					for (int j = 0; j < 32; j++)
					{
						if ((JSGFParser.jj_la1_0[i] & 1 << j) != 0)
						{
							array[j] = true;
						}
						if ((JSGFParser.jj_la1_1[i] & 1 << j) != 0)
						{
							array[32 + j] = true;
						}
					}
				}
			}
			for (int i = 0; i < 40; i++)
			{
				if (array[i])
				{
					this.jj_expentry = new int[1];
					this.jj_expentry[0] = i;
					this.jj_expentries.add(this.jj_expentry);
				}
			}
			this.jj_endpos = 0;
			this.jj_rescan_token();
			this.jj_add_error_token(0, 0);
			int[][] array2 = new int[this.jj_expentries.size()][];
			for (int j = 0; j < this.jj_expentries.size(); j++)
			{
				array2[j] = (int[])this.jj_expentries.get(j);
			}
			return new ParseException(this.token, array2, JSGFParserConstants.tokenImage);
		}

		[LineNumberTable(new byte[]
		{
			163,
			128,
			102,
			107,
			127,
			1,
			107,
			113,
			107,
			48,
			166,
			119,
			114,
			108,
			110,
			112,
			2,
			232,
			69,
			114,
			130,
			101,
			153
		})]
		
		private void jj_add_error_token(int num, int num2)
		{
			if (num2 >= 100)
			{
				return;
			}
			if (num2 == this.jj_endpos + 1)
			{
				int[] array = this.jj_lasttokens;
				int num3 = this.jj_endpos;
				int num4 = num3;
				this.jj_endpos = num3 + 1;
				array[num4] = num;
			}
			else if (this.jj_endpos != 0)
			{
				this.jj_expentry = new int[this.jj_endpos];
				for (int i = 0; i < this.jj_endpos; i++)
				{
					this.jj_expentry[i] = this.jj_lasttokens[i];
				}
				Iterator iterator = this.jj_expentries.iterator();
				IL_7A:
				while (iterator.hasNext())
				{
					int[] array2 = (int[])((int[])iterator.next());
					if (array2.Length == this.jj_expentry.Length)
					{
						for (int j = 0; j < this.jj_expentry.Length; j++)
						{
							if (array2[j] != this.jj_expentry[j])
							{
								goto IL_7A;
							}
						}
						this.jj_expentries.add(this.jj_expentry);
						break;
					}
				}
				if (num2 != 0)
				{
					int[] array3 = this.jj_lasttokens;
					this.jj_endpos = num2;
					array3[num2 - 1] = num;
				}
			}
		}

		[LineNumberTable(new byte[]
		{
			163,
			195,
			103,
			137,
			137,
			110,
			127,
			4,
			107,
			167,
			103,
			110,
			227,
			52,
			233,
			78,
			103
		})]
		
		private void jj_rescan_token()
		{
			this.jj_rescan = true;
			int i = 0;
			while (i < 1)
			{
				try
				{
					JSGFParser.JJCalls jjcalls = this.jj_2_rtns[i];
					do
					{
						if (jjcalls.gen > this.jj_gen)
						{
							this.jj_la = jjcalls.arg;
							Token first = jjcalls.first;
							Token token = first;
							this.jj_scanpos = first;
							this.jj_lastpos = token;
							if (i == 0)
							{
								this.jj_3_1();
							}
						}
						jjcalls = jjcalls.next;
					}
					while (jjcalls != null);
				}
				catch (JSGFParser.LookaheadSuccess lookaheadSuccess)
				{
				}
				IL_74:
				i++;
				continue;
				goto IL_74;
			}
			this.jj_rescan = false;
		}

		[LineNumberTable(new byte[]
		{
			162,
			231,
			127,
			160,
			134
		})]
		private static void jj_la1_init_0()
		{
			JSGFParser.jj_la1_0 = new int[]
			{
				8388608,
				16384,
				268468224,
				8388608,
				8388608,
				536870912,
				8445952,
				32768,
				0,
				0,
				279306240,
				8511488,
				279306240,
				327680,
				279306240,
				1073741824,
				1073741824,
				4194304,
				1073741824,
				1073741824,
				4194304,
				4194304,
				279306240,
				4194304,
				10870784,
				536870912
			};
		}

		[LineNumberTable(new byte[]
		{
			162,
			234,
			127,
			105
		})]
		private static void jj_la1_init_1()
		{
			JSGFParser.jj_la1_1 = new int[]
			{
				0,
				0,
				0,
				0,
				0,
				0,
				0,
				0,
				2,
				2,
				84,
				0,
				80,
				0,
				0,
				8,
				8,
				0,
				8,
				8,
				0,
				0,
				80,
				0,
				0,
				0
			};
		}

		[LineNumberTable(new byte[]
		{
			159,
			191,
			100,
			111,
			148,
			104,
			159,
			17,
			98,
			107,
			159,
			9,
			223,
			21,
			255,
			0,
			61,
			98,
			127,
			6,
			149,
			119,
			137,
			109,
			127,
			21,
			146,
			115,
			137,
			223,
			2,
			226,
			61,
			97,
			127,
			17,
			161,
			111,
			111,
			111,
			111,
			161,
			122,
			191,
			4,
			2,
			98,
			159,
			11
		})]
		
		public static void main(string[] args)
		{
			if (args.Length == 0)
			{
				java.lang.System.@out.println("JSGF Parser Version 1.0:  Reading from standard input . . .");
				JSGFParser.parser = new JSGFParser(System.@in);
			}
			else
			{
				if (args.Length > 0)
				{
					java.lang.System.@out.println(new StringBuilder().append("JSGF Parser Version 1.0:  Reading from file ").append(args[0]).append(" . . .").toString());
					MalformedURLException ex2;
					try
					{
						File file = new File(".");
						string text = new StringBuilder().append(file.getAbsolutePath()).append("/").append(args[0]).toString();
						try
						{
							URL.__<clinit>();
							URL url = new URL(new StringBuilder().append("file:").append(text).toString());
						}
						catch (MalformedURLException ex)
						{
							ex2 = ByteCodeHelper.MapException<MalformedURLException>(ex, 1);
							goto IL_D8;
						}
						goto IL_DB;
					}
					catch (Exception ex3)
					{
						if (ByteCodeHelper.MapException<Exception>(ex3, 2) == null)
						{
							throw;
						}
						goto IL_DD;
					}
					IL_D8:
					MalformedURLException ex4 = ex2;
					try
					{
						MalformedURLException ex5 = ex4;
						java.lang.System.@out.println(new StringBuilder().append("Could not get URL for current directory ").append(ex5).toString());
					}
					catch (Exception ex6)
					{
						if (ByteCodeHelper.MapException<Exception>(ex6, 2) == null)
						{
							throw;
						}
						goto IL_11D;
					}
					return;
					IL_11D:
					goto IL_1D5;
					IL_DB:
					try
					{
						BufferedInputStream.__<clinit>();
						URL url;
						BufferedInputStream bufferedInputStream = new BufferedInputStream(url.openStream(), 256);
						JSGFEncoding jsgfencoding = JSGFParser.getJSGFEncoding(bufferedInputStream);
						InputStreamReader stream;
						if (jsgfencoding != null && jsgfencoding.encoding != null)
						{
							java.lang.System.@out.println(new StringBuilder().append("Grammar Character Encoding \"").append(jsgfencoding.encoding).append("\"").toString());
							stream = new InputStreamReader(bufferedInputStream, jsgfencoding.encoding);
						}
						else
						{
							if (jsgfencoding == null)
							{
								java.lang.System.@out.println("WARNING: Grammar missing self identifying header");
							}
							stream = new InputStreamReader(bufferedInputStream);
						}
						JSGFParser.parser = new JSGFParser(stream);
					}
					catch (Exception ex7)
					{
						if (ByteCodeHelper.MapException<Exception>(ex7, 2) == null)
						{
							throw;
						}
						goto IL_1D0;
					}
					goto IL_244;
					IL_1D0:
					IL_DD:
					IL_1D5:
					java.lang.System.@out.println(new StringBuilder().append("JSGF Parser Version 1.0:  File ").append(args[0]).append(" not found.").toString());
					return;
				}
				java.lang.System.@out.println("JSGF Parser Version 1.0:  Usage is one of:");
				java.lang.System.@out.println("         java JSGFParser < inputfile");
				java.lang.System.@out.println("OR");
				java.lang.System.@out.println("         java JSGFParser inputfile");
				return;
			}
			ParseException ex9;
			try
			{
				IL_244:
				JSGFParser jsgfparser = JSGFParser.parser;
				JSGFRuleGrammarFactory.__<clinit>();
				jsgfparser.GrammarUnit(new JSGFRuleGrammarFactory(new JSGFRuleGrammarManager()));
				java.lang.System.@out.println("JSGF Parser Version 1.0:  JSGF Grammar parsed successfully.");
			}
			catch (ParseException ex8)
			{
				ex9 = ByteCodeHelper.MapException<ParseException>(ex8, 1);
				goto IL_27D;
			}
			return;
			IL_27D:
			ParseException ex10 = ex9;
			java.lang.System.@out.println(new StringBuilder().append("JSGF Parser Version 1.0:  Encountered errors during parse.").append(Throwable.instancehelper_getMessage(ex10)).toString());
		}

		[Throws(new string[]
		{
			"edu.cmu.sphinx.jsgf.JSGFGrammarParseException"
		})]
		[LineNumberTable(new byte[]
		{
			47,
			98,
			103,
			141,
			171,
			108,
			119,
			97,
			104,
			127,
			1
		})]
		
		public static JSGFRuleGrammar newGrammarFromJSGF(InputStream i, JSGFRuleGrammarFactory factory)
		{
			if (JSGFParser.parser == null)
			{
				JSGFParser.parser = new JSGFParser(i);
			}
			else
			{
				JSGFParser.parser.ReInit(i);
			}
			JSGFRuleGrammar result;
			ParseException ex2;
			try
			{
				JSGFRuleGrammar jsgfruleGrammar = JSGFParser.parser.GrammarUnit(factory);
				result = jsgfruleGrammar;
			}
			catch (ParseException ex)
			{
				ex2 = ByteCodeHelper.MapException<ParseException>(ex, 1);
				goto IL_40;
			}
			return result;
			IL_40:
			ParseException ex3 = ex2;
			Token currentToken = ex3.currentToken;
			JSGFGrammarParseException ex4 = new JSGFGrammarParseException(currentToken.beginLine, currentToken.beginColumn, "Grammar Error", Throwable.instancehelper_getMessage(ex3));
			throw Throwable.__<unmap>(ex4);
		}

		[LineNumberTable(new byte[]
		{
			160,
			122,
			130,
			103,
			116,
			107,
			246,
			69,
			2,
			97,
			143
		})]
		
		public static JSGFRule ruleForJSGF(string text)
		{
			JSGFRuleAlternatives result = null;
			try
			{
				StringReader stream = new StringReader(text);
				if (JSGFParser.parser == null)
				{
					JSGFParser.parser = new JSGFParser(stream);
				}
				else
				{
					JSGFParser.parser.ReInit(stream);
				}
				result = JSGFParser.parser.alternatives();
			}
			catch (ParseException ex)
			{
				goto IL_3B;
			}
			return result;
			IL_3B:
			java.lang.System.@out.println("JSGF Parser Version 1.0:  Encountered errors during parse.");
			return result;
		}

		[Throws(new string[]
		{
			"edu.cmu.sphinx.jsgf.parser.ParseException"
		})]
		[LineNumberTable(new byte[]
		{
			162,
			181,
			98,
			105,
			103,
			159,
			3,
			105,
			105,
			98,
			130,
			175,
			105,
			126,
			103
		})]
		
		public JSGFRuleName importRef()
		{
			int num = 0;
			this.jj_consume_token(28);
			string text = this.Name();
			if (((this.jj_ntk != -1) ? this.jj_ntk : this.jj_ntk()) == 29)
			{
				this.jj_consume_token(29);
				this.jj_consume_token(30);
				num = 1;
			}
			else
			{
				this.jj_la1[25] = this.jj_gen;
			}
			this.jj_consume_token(31);
			if (num != 0)
			{
				text = new StringBuilder().append(text).append(".*").toString();
			}
			return new JSGFRuleName(text);
		}

		[LineNumberTable(new byte[]
		{
			163,
			30,
			232,
			1,
			237,
			77,
			108,
			103,
			231,
			160,
			92,
			236,
			111,
			139,
			103,
			237,
			159,
			163,
			103,
			107,
			103,
			103,
			118,
			127,
			0
		})]
		
		public JSGFParser(JSGFParserTokenManager tm)
		{
			this.jj_la1 = new int[26];
			this.jj_2_rtns = new JSGFParser.JJCalls[1];
			this.jj_rescan = false;
			this.jj_gc = 0;
			this.jj_ls = new JSGFParser.LookaheadSuccess(null);
			this.jj_expentries = new ArrayList();
			this.jj_kind = -1;
			this.jj_lasttokens = new int[100];
			this.token_source = tm;
			this.token = new Token();
			this.jj_ntk = -1;
			this.jj_gen = 0;
			for (int i = 0; i < 26; i++)
			{
				this.jj_la1[i] = -1;
			}
			for (int i = 0; i < this.jj_2_rtns.Length; i++)
			{
				this.jj_2_rtns[i] = new JSGFParser.JJCalls();
			}
		}

		[LineNumberTable(new byte[]
		{
			163,
			40,
			103,
			107,
			103,
			103,
			118,
			127,
			0
		})]
		
		public virtual void ReInit(JSGFParserTokenManager tm)
		{
			this.token_source = tm;
			this.token = new Token();
			this.jj_ntk = -1;
			this.jj_gen = 0;
			for (int i = 0; i < 26; i++)
			{
				this.jj_la1[i] = -1;
			}
			for (int i = 0; i < this.jj_2_rtns.Length; i++)
			{
				this.jj_2_rtns[i] = new JSGFParser.JJCalls();
			}
		}

		[LineNumberTable(new byte[]
		{
			163,
			98,
			127,
			1,
			127,
			2,
			103,
			110
		})]
		
		public Token getNextToken()
		{
			if (this.token.next != null)
			{
				this.token = this.token.next;
			}
			else
			{
				Token token = this.token;
				Token nextToken = this.token_source.getNextToken();
				Token token2 = token;
				Token token3 = nextToken;
				token2.next = nextToken;
				this.token = token3;
			}
			this.jj_ntk = -1;
			this.jj_gen++;
			return this.token;
		}

		[LineNumberTable(new byte[]
		{
			163,
			106,
			103,
			102,
			113,
			23,
			198
		})]
		
		public Token getToken(int index)
		{
			Token token = this.token;
			for (int i = 0; i < index; i++)
			{
				if (token.next != null)
				{
					token = token.next;
				}
				else
				{
					Token token2 = token;
					Token nextToken = this.token_source.getNextToken();
					Token token3 = token2;
					Token token4 = nextToken;
					token3.next = nextToken;
					token = token4;
				}
			}
			return token;
		}

		public void enable_tracing()
		{
		}

		public void disable_tracing()
		{
		}

		[LineNumberTable(new byte[]
		{
			159,
			186,
			230,
			163,
			41,
			101,
			101
		})]
		static JSGFParser()
		{
			JSGFParser.jj_la1_init_0();
			JSGFParser.jj_la1_init_1();
		}

		internal const string version = "1.0";

		internal static JSGFParser parser = null;

		public JSGFParserTokenManager token_source;

		internal JavaCharStream jj_input_stream;

		public Token token;

		public Token jj_nt;

		private int jj_ntk;

		private Token jj_scanpos;

		private Token jj_lastpos;

		private int jj_la;

		private int jj_gen;

		
		private int[] jj_la1;

		private static int[] jj_la1_0;

		private static int[] jj_la1_1;

		
		private JSGFParser.JJCalls[] jj_2_rtns;

		private bool jj_rescan;

		private int jj_gc;

		
		private JSGFParser.LookaheadSuccess jj_ls;

		
		private List jj_expentries;

		private int[] jj_expentry;

		private int jj_kind;

		private int[] jj_lasttokens;

		private int jj_endpos;

		
		[SourceFile("JSGFParser.java")]
		internal sealed class JJCalls : java.lang.Object
		{
			
			
			internal JJCalls()
			{
			}

			internal int gen;

			internal Token first;

			internal int arg;

			internal JSGFParser.JJCalls next;
		}

		
		[SourceFile("JSGFParser.java")]
		[Serializable]
		internal sealed class LookaheadSuccess : Error
		{
			
			
			
			internal LookaheadSuccess(JSGFParser$1 jsgfparser$) : this()
			{
			}

			
			
			private LookaheadSuccess()
			{
			}

			
			[PermissionSet(SecurityAction.Demand, XML = "<PermissionSet class=\"System.Security.PermissionSet\"\nversion=\"1\">\n<IPermission class=\"System.Security.Permissions.SecurityPermission, mscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089\"\nversion=\"1\"\nFlags=\"SerializationFormatter\"/>\n</PermissionSet>\n")]
			protected LookaheadSuccess(SerializationInfo serializationInfo, StreamingContext streamingContext) : base(serializationInfo, streamingContext)
			{
			}
		}
	}
}
