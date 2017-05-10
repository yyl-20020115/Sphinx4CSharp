using System;

using IKVM.Attributes;
using java.io;
using java.lang;

namespace edu.cmu.sphinx.jsgf.parser
{
	[Implements(new string[]
	{
		"edu.cmu.sphinx.jsgf.parser.JSGFParserConstants"
	})]
	public class JSGFParserTokenManager : java.lang.Object, JSGFParserConstants
	{
		
		public static void __<clinit>()
		{
		}

		[LineNumberTable(new byte[]
		{
			163,
			92,
			232,
			156,
			75,
			235,
			163,
			173,
			109,
			109,
			107,
			236,
			160,
			81,
			103,
			231,
			159,
			182,
			103
		})]
		
		public JSGFParserTokenManager(JavaCharStream stream)
		{
			this.debugStream = java.lang.System.@out;
			this.jjrounds = new int[54];
			this.jjstateSet = new int[108];
			this.jjimage = new StringBuilder();
			this.image = this.jjimage;
			this.curLexState = 0;
			this.defaultLexState = 0;
			this.input_stream = stream;
		}

		[LineNumberTable(new byte[]
		{
			163,
			105,
			114,
			108,
			103,
			102
		})]
		
		public virtual void ReInit(JavaCharStream stream)
		{
			int num = 0;
			int num2 = num;
			this.jjnewStateCnt = num;
			this.jjmatchedPos = num2;
			this.curLexState = this.defaultLexState;
			this.input_stream = stream;
			this.ReInitRounds();
		}

		[LineNumberTable(new byte[]
		{
			163,
			177,
			130,
			226,
			71,
			252,
			72,
			226,
			58,
			129,
			103,
			103,
			103,
			130,
			108,
			108,
			199,
			191,
			1,
			108,
			127,
			5,
			156,
			104,
			107,
			103,
			103,
			133,
			104,
			103,
			98,
			103,
			159,
			2,
			104,
			204,
			107,
			103,
			103,
			146,
			202,
			107,
			103,
			103,
			146,
			200,
			144,
			107,
			117,
			159,
			3,
			103,
			103,
			111,
			114,
			130,
			159,
			6,
			159,
			3,
			103,
			99,
			164,
			103,
			143,
			169,
			103,
			114,
			183,
			102,
			111,
			114,
			98,
			139,
			124,
			133,
			129,
			109,
			109,
			99,
			99,
			255,
			4,
			74,
			226,
			55,
			97,
			99,
			120,
			116,
			102,
			165,
			134,
			100,
			108,
			152
		})]
		
		public virtual Token getNextToken()
		{
			Token token = null;
			int num = 0;
			Token token2;
			for (;;)
			{
				try
				{
					this.curChar = this.input_stream.BeginToken();
				}
				catch (IOException ex)
				{
					break;
				}
				this.image = this.jjimage;
				this.image.setLength(0);
				this.jjimageLen = 0;
				for (;;)
				{
					switch (this.curLexState)
					{
					case 0:
						try
						{
							this.input_stream.backup(0);
							while (this.curChar <= ' ' && (4294981120L & 1L << (int)this.curChar) != 0L)
							{
								this.curChar = this.input_stream.BeginToken();
							}
						}
						catch (IOException ex2)
						{
							goto IL_C2;
						}
						this.jjmatchedKind = int.MaxValue;
						this.jjmatchedPos = 0;
						num = this.jjMoveStringLiteralDfa0_0();
						break;
					case 1:
						this.jjmatchedKind = 9;
						this.jjmatchedPos = -1;
						num = this.jjMoveStringLiteralDfa0_1();
						if (this.jjmatchedPos < 0 || (this.jjmatchedPos == 0 && this.jjmatchedKind > 12))
						{
							this.jjmatchedKind = 12;
							this.jjmatchedPos = 0;
						}
						break;
					case 2:
						this.jjmatchedKind = int.MaxValue;
						this.jjmatchedPos = 0;
						num = this.jjMoveStringLiteralDfa0_2();
						if (this.jjmatchedPos == 0 && this.jjmatchedKind > 12)
						{
							this.jjmatchedKind = 12;
						}
						break;
					case 3:
						this.jjmatchedKind = int.MaxValue;
						this.jjmatchedPos = 0;
						num = this.jjMoveStringLiteralDfa0_3();
						if (this.jjmatchedPos == 0 && this.jjmatchedKind > 12)
						{
							this.jjmatchedKind = 12;
						}
						break;
					}
					if (this.jjmatchedKind == 2147483647)
					{
						goto IL_31D;
					}
					if (this.jjmatchedPos + 1 < num)
					{
						this.input_stream.backup(num - this.jjmatchedPos - 1);
					}
					if ((JSGFParserTokenManager.jjtoToken[this.jjmatchedKind >> 6] & 1L << this.jjmatchedKind) != 0L)
					{
						goto Block_12;
					}
					if ((JSGFParserTokenManager.jjtoSkip[this.jjmatchedKind >> 6] & 1L << this.jjmatchedKind) != 0L)
					{
						goto Block_14;
					}
					this.MoreLexicalActions();
					if (JSGFParserTokenManager.__jjnewLexState[this.jjmatchedKind] != -1)
					{
						this.curLexState = JSGFParserTokenManager.__jjnewLexState[this.jjmatchedKind];
					}
					num = 0;
					this.jjmatchedKind = int.MaxValue;
					try
					{
						this.curChar = this.input_stream.readChar();
					}
					catch (IOException ex3)
					{
						goto IL_314;
					}
				}
				IL_C2:
				continue;
				Block_14:
				if ((JSGFParserTokenManager.jjtoSpecial[this.jjmatchedKind >> 6] & 1L << this.jjmatchedKind) != 0L)
				{
					token2 = this.jjFillToken();
					if (token == null)
					{
						token = token2;
					}
					else
					{
						token2.specialToken = token;
						Token token3 = token;
						Token token4 = token2;
						Token token5 = token3;
						Token token6 = token4;
						token5.next = token4;
						token = token6;
					}
					this.SkipLexicalActions(token2);
				}
				else
				{
					this.SkipLexicalActions(null);
				}
				if (JSGFParserTokenManager.__jjnewLexState[this.jjmatchedKind] != -1)
				{
					this.curLexState = JSGFParserTokenManager.__jjnewLexState[this.jjmatchedKind];
				}
			}
			this.jjmatchedKind = 0;
			token2 = this.jjFillToken();
			token2.specialToken = token;
			return token2;
			Block_12:
			token2 = this.jjFillToken();
			token2.specialToken = token;
			if (JSGFParserTokenManager.__jjnewLexState[this.jjmatchedKind] != -1)
			{
				this.curLexState = JSGFParserTokenManager.__jjnewLexState[this.jjmatchedKind];
			}
			return token2;
			IL_314:
			IL_31D:
			int num2 = this.input_stream.getEndLine();
			int num3 = this.input_stream.getEndColumn();
			string text = null;
			int num4 = 0;
			try
			{
				this.input_stream.readChar();
				this.input_stream.backup(1);
			}
			catch (IOException ex4)
			{
				goto IL_35D;
			}
			goto IL_3A3;
			IL_35D:
			num4 = 1;
			text = ((num > 1) ? this.input_stream.GetImage() : "");
			if (this.curChar == '\n' || this.curChar == '\r')
			{
				num2++;
				num3 = 0;
			}
			else
			{
				num3++;
			}
			IL_3A3:
			if (num4 == 0)
			{
				this.input_stream.backup(1);
				text = ((num > 1) ? this.input_stream.GetImage() : "");
			}
			bool eofseen = num4 != 0;
			int lexState = this.curLexState;
			int errorLine = num2;
			int errorColumn = num3;
			string errorAfter = text;
			char c = this.curChar;
			int reason = 0;
			
			throw new TokenMgrError(eofseen, lexState, errorLine, errorColumn, errorAfter, c, reason);
		}

		
		private int jjStopStringLiteralDfa_0(int num, long num2)
		{
			switch (num)
			{
			case 0:
				if ((num2 & 17179869504L) != 0L)
				{
					return 2;
				}
				if ((num2 & (long)((ulong)134275072)) != 0L)
				{
					this.jjmatchedKind = 23;
					return 37;
				}
				if ((num2 & (long)((ulong)536870912)) != 0L)
				{
					return 5;
				}
				return -1;
			case 1:
				if ((num2 & (long)((ulong)256)) != 0L)
				{
					return 0;
				}
				if ((num2 & (long)((ulong)134275072)) != 0L)
				{
					this.jjmatchedKind = 23;
					this.jjmatchedPos = 1;
					return 37;
				}
				return -1;
			case 2:
				if ((num2 & (long)((ulong)134217728)) != 0L)
				{
					if (this.jjmatchedPos < 1)
					{
						this.jjmatchedKind = 23;
						this.jjmatchedPos = 1;
					}
					return -1;
				}
				if ((num2 & (long)((ulong)57344)) != 0L)
				{
					this.jjmatchedKind = 23;
					this.jjmatchedPos = 2;
					return 37;
				}
				return -1;
			case 3:
				if ((num2 & (long)((ulong)134217728)) != 0L)
				{
					if (this.jjmatchedPos < 1)
					{
						this.jjmatchedKind = 23;
						this.jjmatchedPos = 1;
					}
					return -1;
				}
				if ((num2 & (long)((ulong)57344)) != 0L)
				{
					this.jjmatchedKind = 23;
					this.jjmatchedPos = 3;
					return 37;
				}
				return -1;
			case 4:
				if ((num2 & (long)((ulong)57344)) != 0L)
				{
					this.jjmatchedKind = 23;
					this.jjmatchedPos = 4;
					return 37;
				}
				return -1;
			case 5:
				if ((num2 & (long)((ulong)49152)) != 0L)
				{
					return 37;
				}
				if ((num2 & (long)((ulong)8192)) != 0L)
				{
					this.jjmatchedKind = 23;
					this.jjmatchedPos = 5;
					return 37;
				}
				return -1;
			default:
				return -1;
			}
		}

		[LineNumberTable(new byte[]
		{
			160,
			230,
			98,
			104,
			98,
			105,
			166,
			124,
			102,
			141,
			174,
			191,
			160,
			143,
			144,
			101,
			99,
			138,
			106,
			106,
			106,
			105,
			106,
			125,
			144,
			101,
			99,
			142,
			109,
			175,
			109,
			191,
			3,
			122,
			167,
			109,
			191,
			3,
			109,
			172,
			112,
			101,
			101,
			99,
			106,
			133,
			115,
			172,
			112,
			101,
			101,
			99,
			105,
			133,
			109,
			175,
			115,
			173,
			117,
			168,
			115,
			173,
			115,
			175,
			115,
			173,
			115,
			191,
			4,
			115,
			173,
			109,
			173,
			115,
			173,
			115,
			173,
			117,
			168,
			115,
			175,
			115,
			173,
			115,
			191,
			4,
			115,
			173,
			106,
			133,
			115,
			175,
			115,
			175,
			115,
			175,
			115,
			191,
			4,
			115,
			173,
			112,
			101,
			101,
			99,
			104,
			133,
			112,
			101,
			101,
			99,
			105,
			133,
			112,
			101,
			101,
			99,
			104,
			133,
			115,
			175,
			106,
			101,
			101,
			99,
			106,
			133,
			112,
			101,
			101,
			99,
			106,
			133,
			115,
			173,
			112,
			101,
			101,
			99,
			106,
			133,
			115,
			175,
			115,
			173,
			112,
			101,
			101,
			99,
			106,
			130,
			112,
			172,
			112,
			170,
			112,
			204,
			103,
			101,
			144,
			177,
			191,
			160,
			133,
			144,
			101,
			99,
			141,
			109,
			175,
			103,
			167,
			115,
			175,
			123,
			168,
			112,
			173,
			109,
			175,
			115,
			173,
			112,
			173,
			109,
			175,
			115,
			173,
			109,
			175,
			115,
			175,
			109,
			175,
			115,
			175,
			117,
			168,
			112,
			101,
			101,
			99,
			104,
			130,
			112,
			172,
			112,
			172,
			112,
			204,
			103,
			165,
			106,
			102,
			109,
			112,
			177,
			223,
			45,
			113,
			101,
			101,
			99,
			104,
			133,
			123,
			167,
			116,
			191,
			1,
			113,
			170,
			113,
			204,
			135,
			136,
			103,
			103,
			134,
			101,
			123,
			98,
			124
		})]
		
		private int jjMoveNfa_0(int num, int num2)
		{
			int num3 = 0;
			this.jjnewStateCnt = 54;
			int num4 = 1;
			this.jjstateSet[0] = num;
			int num5 = int.MaxValue;
			for (;;)
			{
				int num6 = this.jjround + 1;
				int num7 = num6;
				this.jjround = num6;
				if (num7 == 2147483647)
				{
					this.ReInitRounds();
				}
				if (this.curChar < '@')
				{
					long num8 = 1L << (int)this.curChar;
					do
					{
						int[] array = this.jjstateSet;
						num4 += -1;
						switch (array[num4])
						{
						case 0:
							if (this.curChar == '*')
							{
								int[] array2 = this.jjstateSet;
								num6 = this.jjnewStateCnt;
								int num9 = num6;
								this.jjnewStateCnt = num6 + 1;
								array2[num9] = 1;
							}
							break;
						case 1:
							if ((-140737488355329L & num8) != 0L && num5 > 7)
							{
								num5 = 7;
							}
							break;
						case 2:
							if (this.curChar == '*')
							{
								int[] array3 = this.jjstateSet;
								num6 = this.jjnewStateCnt;
								int num10 = num6;
								this.jjnewStateCnt = num6 + 1;
								array3[num10] = 0;
							}
							break;
						case 3:
							if ((576233127626670080L & num8) != 0L)
							{
								if (num5 > 23)
								{
									num5 = 23;
								}
								this.jjCheckNAdd(37);
							}
							else if (this.curChar == '"')
							{
								this.jjCheckNAddStates(0, 2);
							}
							else if (this.curChar == '.')
							{
								this.jjCheckNAdd(5);
							}
							else if (this.curChar == '/')
							{
								int[] array4 = this.jjstateSet;
								num6 = this.jjnewStateCnt;
								int num11 = num6;
								this.jjnewStateCnt = num6 + 1;
								array4[num11] = 2;
							}
							if ((287948901175001088L & num8) != 0L)
							{
								if (num5 > 16)
								{
									num5 = 16;
								}
								this.jjCheckNAddStates(3, 10);
							}
							else if (this.curChar == '\'')
							{
								this.jjAddStates(11, 12);
							}
							break;
						case 4:
							if (this.curChar == '.')
							{
								this.jjCheckNAdd(5);
							}
							break;
						case 5:
							if ((287948901175001088L & num8) != 0L)
							{
								if (num5 > 18)
								{
									num5 = 18;
								}
								this.jjCheckNAddStates(13, 15);
							}
							break;
						case 7:
							if ((43980465111040L & num8) != 0L)
							{
								this.jjCheckNAdd(8);
							}
							break;
						case 8:
							if ((287948901175001088L & num8) != 0L)
							{
								if (num5 > 18)
								{
									num5 = 18;
								}
								this.jjCheckNAddTwoStates(8, 9);
							}
							break;
						case 10:
							if (this.curChar == '\'')
							{
								this.jjAddStates(11, 12);
							}
							break;
						case 11:
							if ((-549755823105L & num8) != 0L)
							{
								this.jjCheckNAdd(12);
							}
							break;
						case 12:
							if (this.curChar == '\'' && num5 > 20)
							{
								num5 = 20;
							}
							break;
						case 14:
							if ((566935683072L & num8) != 0L)
							{
								this.jjCheckNAdd(12);
							}
							break;
						case 15:
							if ((71776119061217280L & num8) != 0L)
							{
								this.jjCheckNAddTwoStates(16, 12);
							}
							break;
						case 16:
							if ((71776119061217280L & num8) != 0L)
							{
								this.jjCheckNAdd(12);
							}
							break;
						case 17:
							if ((4222124650659840L & num8) != 0L)
							{
								int[] array5 = this.jjstateSet;
								num6 = this.jjnewStateCnt;
								int num12 = num6;
								this.jjnewStateCnt = num6 + 1;
								array5[num12] = 18;
							}
							break;
						case 18:
							if ((71776119061217280L & num8) != 0L)
							{
								this.jjCheckNAdd(16);
							}
							break;
						case 19:
							if (this.curChar == '"')
							{
								this.jjCheckNAddStates(0, 2);
							}
							break;
						case 20:
							if ((-17179878401L & num8) != 0L)
							{
								this.jjCheckNAddStates(0, 2);
							}
							break;
						case 22:
							if ((566935683072L & num8) != 0L)
							{
								this.jjCheckNAddStates(0, 2);
							}
							break;
						case 23:
							if (this.curChar == '"' && num5 > 21)
							{
								num5 = 21;
							}
							break;
						case 24:
							if ((71776119061217280L & num8) != 0L)
							{
								this.jjCheckNAddStates(16, 19);
							}
							break;
						case 25:
							if ((71776119061217280L & num8) != 0L)
							{
								this.jjCheckNAddStates(0, 2);
							}
							break;
						case 26:
							if ((4222124650659840L & num8) != 0L)
							{
								int[] array6 = this.jjstateSet;
								num6 = this.jjnewStateCnt;
								int num13 = num6;
								this.jjnewStateCnt = num6 + 1;
								array6[num13] = 27;
							}
							break;
						case 27:
							if ((71776119061217280L & num8) != 0L)
							{
								this.jjCheckNAdd(25);
							}
							break;
						case 29:
							this.jjCheckNAddStates(20, 22);
							break;
						case 31:
							if ((566935683072L & num8) != 0L)
							{
								this.jjCheckNAddStates(20, 22);
							}
							break;
						case 33:
							if ((71776119061217280L & num8) != 0L)
							{
								this.jjCheckNAddStates(23, 26);
							}
							break;
						case 34:
							if ((71776119061217280L & num8) != 0L)
							{
								this.jjCheckNAddStates(20, 22);
							}
							break;
						case 35:
							if ((4222124650659840L & num8) != 0L)
							{
								int[] array7 = this.jjstateSet;
								num6 = this.jjnewStateCnt;
								int num14 = num6;
								this.jjnewStateCnt = num6 + 1;
								array7[num14] = 36;
							}
							break;
						case 36:
							if ((71776119061217280L & num8) != 0L)
							{
								this.jjCheckNAdd(34);
							}
							break;
						case 37:
							if ((576233127626670080L & num8) != 0L)
							{
								if (num5 > 23)
								{
									num5 = 23;
								}
								this.jjCheckNAdd(37);
							}
							break;
						case 38:
							if ((287948901175001088L & num8) != 0L)
							{
								if (num5 > 16)
								{
									num5 = 16;
								}
								this.jjCheckNAddStates(3, 10);
							}
							break;
						case 39:
							if ((287948901175001088L & num8) != 0L)
							{
								if (num5 > 16)
								{
									num5 = 16;
								}
								this.jjCheckNAdd(39);
							}
							break;
						case 40:
							if ((287948901175001088L & num8) != 0L)
							{
								this.jjCheckNAddTwoStates(40, 41);
							}
							break;
						case 41:
							if (this.curChar == '.')
							{
								if (num5 > 18)
								{
									num5 = 18;
								}
								this.jjCheckNAddStates(27, 29);
							}
							break;
						case 42:
							if ((287948901175001088L & num8) != 0L)
							{
								if (num5 > 18)
								{
									num5 = 18;
								}
								this.jjCheckNAddStates(27, 29);
							}
							break;
						case 44:
							if ((43980465111040L & num8) != 0L)
							{
								this.jjCheckNAdd(45);
							}
							break;
						case 45:
							if ((287948901175001088L & num8) != 0L)
							{
								if (num5 > 18)
								{
									num5 = 18;
								}
								this.jjCheckNAddTwoStates(45, 9);
							}
							break;
						case 46:
							if ((287948901175001088L & num8) != 0L)
							{
								this.jjCheckNAddTwoStates(46, 47);
							}
							break;
						case 48:
							if ((43980465111040L & num8) != 0L)
							{
								this.jjCheckNAdd(49);
							}
							break;
						case 49:
							if ((287948901175001088L & num8) != 0L)
							{
								if (num5 > 18)
								{
									num5 = 18;
								}
								this.jjCheckNAddTwoStates(49, 9);
							}
							break;
						case 50:
							if ((287948901175001088L & num8) != 0L)
							{
								this.jjCheckNAddStates(30, 32);
							}
							break;
						case 52:
							if ((43980465111040L & num8) != 0L)
							{
								this.jjCheckNAdd(53);
							}
							break;
						case 53:
							if ((287948901175001088L & num8) != 0L)
							{
								this.jjCheckNAddTwoStates(53, 9);
							}
							break;
						}
					}
					while (num4 != num3);
				}
				else if (this.curChar < '\u0080')
				{
					long num8 = 1L << (int)this.curChar;
					do
					{
						int[] array8 = this.jjstateSet;
						num4 += -1;
						switch (array8[num4])
						{
						case 1:
							if (num5 > 7)
							{
								num5 = 7;
							}
							break;
						case 3:
							if ((5188146765764755455L & num8) != 0L)
							{
								if (num5 > 23)
								{
									num5 = 23;
								}
								this.jjCheckNAdd(37);
							}
							else if (this.curChar == '{')
							{
								this.jjCheckNAddStates(20, 22);
							}
							break;
						case 6:
							if ((137438953504L & num8) != 0L)
							{
								this.jjAddStates(33, 34);
							}
							break;
						case 9:
							if ((343597383760L & num8) != 0L && num5 > 18)
							{
								num5 = 18;
							}
							break;
						case 11:
							if ((-268435457L & num8) != 0L)
							{
								this.jjCheckNAdd(12);
							}
							break;
						case 13:
							if (this.curChar == '\\')
							{
								this.jjAddStates(35, 37);
							}
							break;
						case 14:
							if ((5700160604602368L & num8) != 0L)
							{
								this.jjCheckNAdd(12);
							}
							break;
						case 20:
							if ((-268435457L & num8) != 0L)
							{
								this.jjCheckNAddStates(0, 2);
							}
							break;
						case 21:
							if (this.curChar == '\\')
							{
								this.jjAddStates(38, 40);
							}
							break;
						case 22:
							if ((5700160604602368L & num8) != 0L)
							{
								this.jjCheckNAddStates(0, 2);
							}
							break;
						case 28:
							if (this.curChar == '{')
							{
								this.jjCheckNAddStates(20, 22);
							}
							break;
						case 29:
							if ((-2305843009213693953L & num8) != 0L)
							{
								this.jjCheckNAddStates(20, 22);
							}
							break;
						case 30:
							if (this.curChar == '\\')
							{
								this.jjAddStates(41, 43);
							}
							break;
						case 31:
							if ((2311543169818296320L & num8) != 0L)
							{
								this.jjCheckNAddStates(20, 22);
							}
							break;
						case 32:
							if (this.curChar == '}' && num5 > 22)
							{
								num5 = 22;
							}
							break;
						case 37:
							if ((5188146765764755455L & num8) != 0L)
							{
								if (num5 > 23)
								{
									num5 = 23;
								}
								this.jjCheckNAdd(37);
							}
							break;
						case 43:
							if ((137438953504L & num8) != 0L)
							{
								this.jjAddStates(44, 45);
							}
							break;
						case 47:
							if ((137438953504L & num8) != 0L)
							{
								this.jjAddStates(46, 47);
							}
							break;
						case 51:
							if ((137438953504L & num8) != 0L)
							{
								this.jjAddStates(48, 49);
							}
							break;
						}
					}
					while (num4 != num3);
				}
				else
				{
					int num15 = (int)(this.curChar >> 8);
					int num16 = num15 >> 6;
					long num17 = 1L << num15;
					int num18 = (int)((this.curChar & 'ÿ') >> 6);
					long num19 = 1L << (int)this.curChar;
					do
					{
						int[] array9 = this.jjstateSet;
						num4 += -1;
						object obj = array9[num4];
						if (obj == 1)
						{
							if (JSGFParserTokenManager.jjCanMove_0(num15, num16, num18, num17, num19) && num5 > 7)
							{
								num5 = 7;
							}
						}
						else
						{
							if (obj != 3)
							{
								if (obj == 11)
								{
									if (JSGFParserTokenManager.jjCanMove_0(num15, num16, num18, num17, num19))
									{
										int[] array10 = this.jjstateSet;
										num6 = this.jjnewStateCnt;
										int num20 = num6;
										this.jjnewStateCnt = num6 + 1;
										array10[num20] = 12;
										goto IL_CA7;
									}
									goto IL_CA7;
								}
								else if (obj == 20)
								{
									if (JSGFParserTokenManager.jjCanMove_0(num15, num16, num18, num17, num19))
									{
										this.jjAddStates(0, 2);
										goto IL_CA7;
									}
									goto IL_CA7;
								}
								else if (obj == 29)
								{
									if (JSGFParserTokenManager.jjCanMove_0(num15, num16, num18, num17, num19))
									{
										this.jjAddStates(20, 22);
										goto IL_CA7;
									}
									goto IL_CA7;
								}
								else if (obj != 37)
								{
									goto IL_CA7;
								}
							}
							if (JSGFParserTokenManager.jjCanMove_1(num15, num16, num18, num17, num19))
							{
								if (num5 > 23)
								{
									num5 = 23;
								}
								this.jjCheckNAdd(37);
							}
						}
						IL_CA7:;
					}
					while (num4 != num3);
				}
				if (num5 != 2147483647)
				{
					this.jjmatchedKind = num5;
					this.jjmatchedPos = num2;
					num5 = int.MaxValue;
				}
				num2++;
				int num21 = num4 = this.jjnewStateCnt;
				int num22 = 54;
				num6 = num3;
				int num23 = num6;
				this.jjnewStateCnt = num6;
				if (num21 == (num3 = num22 - num23))
				{
					break;
				}
				try
				{
					this.curChar = this.input_stream.readChar();
				}
				catch (IOException ex)
				{
					return num2;
				}
			}
			return num2;
		}

		private int jjStopAtPos(int num, int num2)
		{
			this.jjmatchedKind = num2;
			this.jjmatchedPos = num;
			return num + 1;
		}

		[LineNumberTable(new byte[]
		{
			160,
			198,
			103,
			103,
			124,
			103
		})]
		
		private int jjStartNfaWithStates_0(int num, int num2, int num3)
		{
			this.jjmatchedKind = num2;
			this.jjmatchedPos = num;
			try
			{
				this.curChar = this.input_stream.readChar();
			}
			catch (IOException ex)
			{
				goto IL_27;
			}
			return this.jjMoveNfa_0(num3, num + 1);
			IL_27:
			return num + 1;
		}

		[LineNumberTable(new byte[]
		{
			114,
			220,
			226,
			61,
			97,
			105,
			130,
			191,
			26,
			108,
			170,
			105,
			169,
			142,
			142,
			142,
			206
		})]
		
		private int jjMoveStringLiteralDfa1_0(long num)
		{
			try
			{
				this.curChar = this.input_stream.readChar();
			}
			catch (IOException ex)
			{
				goto IL_19;
			}
			char c = this.curChar;
			if (c == '*')
			{
				if ((num & (long)((ulong)256)) != 0L)
				{
					return this.jjStartNfaWithStates_0(1, 8, 0);
				}
			}
			else if (c == '/')
			{
				if ((num & (long)((ulong)64)) != 0L)
				{
					return this.jjStopAtPos(1, 6);
				}
			}
			else
			{
				if (c == '1')
				{
					return this.jjMoveStringLiteralDfa2_0(num, (long)((ulong)134217728));
				}
				if (c == 'm')
				{
					return this.jjMoveStringLiteralDfa2_0(num, (long)((ulong)16384));
				}
				if (c == 'r')
				{
					return this.jjMoveStringLiteralDfa2_0(num, (long)((ulong)8192));
				}
				if (c == 'u')
				{
					return this.jjMoveStringLiteralDfa2_0(num, (long)((ulong)32768));
				}
			}
			return this.jjStartNfa_0(0, num);
			IL_19:
			this.jjStopStringLiteralDfa_0(0, num);
			return 1;
		}

		[LineNumberTable(new byte[]
		{
			160,
			80,
			106,
			105,
			220,
			226,
			61,
			97,
			105,
			130,
			191,
			10,
			142,
			142,
			142,
			206
		})]
		
		private int jjMoveStringLiteralDfa2_0(long num, long num2)
		{
			if ((num2 &= num) == 0L)
			{
				return this.jjStartNfa_0(0, num);
			}
			try
			{
				this.curChar = this.input_stream.readChar();
			}
			catch (IOException ex)
			{
				goto IL_2C;
			}
			char c = this.curChar;
			if (c == '.')
			{
				return this.jjMoveStringLiteralDfa3_0(num2, (long)((ulong)134217728));
			}
			if (c == 'a')
			{
				return this.jjMoveStringLiteralDfa3_0(num2, (long)((ulong)8192));
			}
			if (c == 'b')
			{
				return this.jjMoveStringLiteralDfa3_0(num2, (long)((ulong)32768));
			}
			if (c == 'p')
			{
				return this.jjMoveStringLiteralDfa3_0(num2, (long)((ulong)16384));
			}
			return this.jjStartNfa_0(1, num2);
			IL_2C:
			this.jjStopStringLiteralDfa_0(1, num2);
			return 2;
		}

		
		
		
		private int jjStartNfa_0(int num, long num2)
		{
			return this.jjMoveNfa_0(this.jjStopStringLiteralDfa_0(num, num2), num + 1);
		}

		[LineNumberTable(new byte[]
		{
			160,
			104,
			106,
			105,
			220,
			226,
			61,
			97,
			105,
			130,
			191,
			10,
			108,
			170,
			142,
			142,
			206
		})]
		
		private int jjMoveStringLiteralDfa3_0(long num, long num2)
		{
			if ((num2 &= num) == 0L)
			{
				return this.jjStartNfa_0(1, num);
			}
			try
			{
				this.curChar = this.input_stream.readChar();
			}
			catch (IOException ex)
			{
				goto IL_2C;
			}
			char c = this.curChar;
			if (c == '0')
			{
				if ((num2 & (long)((ulong)134217728)) != 0L)
				{
					return this.jjStopAtPos(3, 27);
				}
			}
			else
			{
				if (c == 'l')
				{
					return this.jjMoveStringLiteralDfa4_0(num2, (long)((ulong)32768));
				}
				if (c == 'm')
				{
					return this.jjMoveStringLiteralDfa4_0(num2, (long)((ulong)8192));
				}
				if (c == 'o')
				{
					return this.jjMoveStringLiteralDfa4_0(num2, (long)((ulong)16384));
				}
			}
			return this.jjStartNfa_0(2, num2);
			IL_2C:
			this.jjStopStringLiteralDfa_0(2, num2);
			return 3;
		}

		[LineNumberTable(new byte[]
		{
			160,
			130,
			106,
			105,
			220,
			226,
			61,
			97,
			105,
			130,
			191,
			2,
			142,
			142,
			206
		})]
		
		private int jjMoveStringLiteralDfa4_0(long num, long num2)
		{
			if ((num2 &= num) == 0L)
			{
				return this.jjStartNfa_0(2, num);
			}
			try
			{
				this.curChar = this.input_stream.readChar();
			}
			catch (IOException ex)
			{
				goto IL_2C;
			}
			char c = this.curChar;
			if (c == 'i')
			{
				return this.jjMoveStringLiteralDfa5_0(num2, (long)((ulong)32768));
			}
			if (c == 'm')
			{
				return this.jjMoveStringLiteralDfa5_0(num2, (long)((ulong)8192));
			}
			if (c == 'r')
			{
				return this.jjMoveStringLiteralDfa5_0(num2, (long)((ulong)16384));
			}
			return this.jjStartNfa_0(3, num2);
			IL_2C:
			this.jjStopStringLiteralDfa_0(3, num2);
			return 4;
		}

		[LineNumberTable(new byte[]
		{
			160,
			152,
			106,
			105,
			220,
			226,
			61,
			97,
			105,
			130,
			191,
			2,
			142,
			108,
			172,
			108,
			236,
			69
		})]
		
		private int jjMoveStringLiteralDfa5_0(long num, long num2)
		{
			if ((num2 &= num) == 0L)
			{
				return this.jjStartNfa_0(3, num);
			}
			try
			{
				this.curChar = this.input_stream.readChar();
			}
			catch (IOException ex)
			{
				goto IL_2C;
			}
			char c = this.curChar;
			if (c == 'a')
			{
				return this.jjMoveStringLiteralDfa6_0(num2, (long)((ulong)8192));
			}
			if (c == 'c')
			{
				if ((num2 & (long)((ulong)32768)) != 0L)
				{
					return this.jjStartNfaWithStates_0(5, 15, 37);
				}
			}
			else if (c == 't')
			{
				if ((num2 & (long)((ulong)16384)) != 0L)
				{
					return this.jjStartNfaWithStates_0(5, 14, 37);
				}
			}
			return this.jjStartNfa_0(4, num2);
			IL_2C:
			this.jjStopStringLiteralDfa_0(4, num2);
			return 5;
		}

		[LineNumberTable(new byte[]
		{
			160,
			178,
			106,
			105,
			220,
			226,
			61,
			97,
			105,
			130,
			177,
			108,
			236,
			69
		})]
		
		private int jjMoveStringLiteralDfa6_0(long num, long num2)
		{
			if ((num2 &= num) == 0L)
			{
				return this.jjStartNfa_0(4, num);
			}
			try
			{
				this.curChar = this.input_stream.readChar();
			}
			catch (IOException ex)
			{
				goto IL_2C;
			}
			if (this.curChar == 'r')
			{
				if ((num2 & (long)((ulong)8192)) != 0L)
				{
					return this.jjStartNfaWithStates_0(6, 13, 37);
				}
			}
			return this.jjStartNfa_0(5, num2);
			IL_2C:
			this.jjStopStringLiteralDfa_0(5, num2);
			return 6;
		}

		[LineNumberTable(new byte[]
		{
			163,
			113,
			107,
			107,
			111
		})]
		private void ReInitRounds()
		{
			this.jjround = -2147483647;
			int num = 54;
			for (;;)
			{
				int num2 = num;
				num += -1;
				if (num2 <= 0)
				{
					break;
				}
				this.jjrounds[num] = int.MinValue;
			}
		}

		[LineNumberTable(new byte[]
		{
			164,
			78,
			144,
			123,
			142
		})]
		private void jjCheckNAdd(int num)
		{
			if (this.jjrounds[num] != this.jjround)
			{
				int[] array = this.jjstateSet;
				int num2 = this.jjnewStateCnt;
				int num3 = num2;
				this.jjnewStateCnt = num2 + 1;
				array[num3] = num;
				this.jjrounds[num] = this.jjround;
			}
		}

		[LineNumberTable(new byte[]
		{
			164,
			99,
			109,
			105
		})]
		
		private void jjCheckNAddStates(int num, int num2)
		{
			int num3;
			do
			{
				this.jjCheckNAdd(JSGFParserTokenManager.jjnextStates[num]);
				num3 = num;
				num++;
			}
			while (num3 != num2);
		}

		[LineNumberTable(new byte[]
		{
			164,
			87,
			127,
			2,
			105
		})]
		private void jjAddStates(int num, int num2)
		{
			int num5;
			do
			{
				int[] array = this.jjstateSet;
				int num3 = this.jjnewStateCnt;
				int num4 = num3;
				this.jjnewStateCnt = num3 + 1;
				array[num4] = JSGFParserTokenManager.jjnextStates[num];
				num5 = num;
				num++;
			}
			while (num5 != num2);
		}

		[LineNumberTable(new byte[]
		{
			164,
			92,
			103,
			103
		})]
		
		private void jjCheckNAddTwoStates(int num, int num2)
		{
			this.jjCheckNAdd(num);
			this.jjCheckNAdd(num2);
		}

		
		[LineNumberTable(new byte[]
		{
			163,
			35,
			191,
			12,
			146,
			146,
			146,
			146,
			146,
			109,
			98
		})]
		private static bool jjCanMove_1(int num, int num2, int num3, long num4, long num5)
		{
			if (num == 0)
			{
				return (JSGFParserTokenManager.jjbitVec4[num3] & num5) != 0L;
			}
			if (num == 48)
			{
				return (JSGFParserTokenManager.jjbitVec5[num3] & num5) != 0L;
			}
			if (num == 49)
			{
				return (JSGFParserTokenManager.jjbitVec6[num3] & num5) != 0L;
			}
			if (num == 51)
			{
				return (JSGFParserTokenManager.jjbitVec7[num3] & num5) != 0L;
			}
			if (num == 61)
			{
				return (JSGFParserTokenManager.jjbitVec8[num3] & num5) != 0L;
			}
			return (JSGFParserTokenManager.jjbitVec3[num2] & num4) != 0L;
		}

		
		[LineNumberTable(new byte[]
		{
			163,
			23,
			171,
			146,
			109,
			98
		})]
		private static bool jjCanMove_0(int num, int num2, int num3, long num4, long num5)
		{
			if (num == 0)
			{
				return (JSGFParserTokenManager.jjbitVec2[num3] & num5) != 0L;
			}
			return (JSGFParserTokenManager.jjbitVec0[num2] & num4) != 0L;
		}

		[LineNumberTable(new byte[]
		{
			162,
			118,
			188,
			2,
			97,
			130,
			177,
			108,
			170,
			130
		})]
		
		private int jjMoveStringLiteralDfa1_3(long num)
		{
			try
			{
				this.curChar = this.input_stream.readChar();
			}
			catch (IOException ex)
			{
				return 1;
			}
			if (this.curChar != '/')
			{
				return 2;
			}
			if ((num & (long)((ulong)2048)) != 0L)
			{
				return this.jjStopAtPos(1, 11);
			}
			return 2;
		}

		[LineNumberTable(new byte[]
		{
			162,
			139,
			98,
			103,
			98,
			105,
			166,
			124,
			102,
			141,
			174,
			191,
			11,
			141,
			101,
			99,
			140,
			141,
			101,
			131,
			109,
			191,
			3,
			109,
			101,
			99,
			106,
			133,
			114,
			165,
			111,
			165,
			106,
			223,
			0,
			103,
			101,
			141,
			176,
			221,
			99,
			106,
			162,
			100,
			165,
			106,
			102,
			109,
			112,
			177,
			221,
			113,
			98,
			101,
			99,
			106,
			162,
			135,
			136,
			103,
			103,
			134,
			101,
			122,
			98,
			124
		})]
		
		private int jjMoveNfa_1(int num, int num2)
		{
			int num3 = 0;
			this.jjnewStateCnt = 4;
			int num4 = 1;
			this.jjstateSet[0] = num;
			int num5 = int.MaxValue;
			for (;;)
			{
				int num6 = this.jjround + 1;
				int num7 = num6;
				this.jjround = num6;
				if (num7 == 2147483647)
				{
					this.ReInitRounds();
				}
				if (this.curChar < '@')
				{
					long num8 = 1L << (int)this.curChar;
					do
					{
						int[] array = this.jjstateSet;
						num4 += -1;
						switch (array[num4])
						{
						case 0:
							if ((-9217L & num8) != 0L)
							{
								num5 = 9;
								this.jjCheckNAddStates(50, 52);
							}
							break;
						case 1:
							if (((ulong)9216 & (ulong)num8) != 0UL && num5 > 9)
							{
								num5 = 9;
							}
							break;
						case 2:
							if (this.curChar == '\n' && num5 > 9)
							{
								num5 = 9;
							}
							break;
						case 3:
							if (this.curChar == '\r')
							{
								int[] array2 = this.jjstateSet;
								num6 = this.jjnewStateCnt;
								int num9 = num6;
								this.jjnewStateCnt = num6 + 1;
								array2[num9] = 2;
							}
							break;
						case 4:
							if ((-9217L & num8) != 0L)
							{
								if (num5 > 9)
								{
									num5 = 9;
								}
								this.jjCheckNAddStates(50, 52);
							}
							else if (((ulong)9216 & (ulong)num8) != 0UL && num5 > 9)
							{
								num5 = 9;
							}
							if (this.curChar == '\r')
							{
								int[] array3 = this.jjstateSet;
								num6 = this.jjnewStateCnt;
								int num10 = num6;
								this.jjnewStateCnt = num6 + 1;
								array3[num10] = 2;
							}
							break;
						}
					}
					while (num4 != num3);
				}
				else if (this.curChar < '\u0080')
				{
					long num11 = 1L << (int)this.curChar;
					for (;;)
					{
						int[] array4 = this.jjstateSet;
						num4 += -1;
						object obj = array4[num4];
						if (obj == 0)
						{
							goto IL_1A4;
						}
						if (obj == 4)
						{
							goto IL_1A4;
						}
						IL_1B3:
						if (num4 == num3)
						{
							break;
						}
						continue;
						IL_1A4:
						num5 = 9;
						this.jjCheckNAddStates(50, 52);
						goto IL_1B3;
					}
				}
				else
				{
					int num12 = (int)(this.curChar >> 8);
					int num13 = num12 >> 6;
					long num14 = 1L << num12;
					int num15 = (int)((this.curChar & 'ÿ') >> 6);
					long num16 = 1L << (int)this.curChar;
					for (;;)
					{
						int[] array5 = this.jjstateSet;
						num4 += -1;
						object obj2 = array5[num4];
						if (obj2 == 0)
						{
							goto IL_217;
						}
						if (obj2 == 4)
						{
							goto IL_217;
						}
						IL_23E:
						if (num4 == num3)
						{
							break;
						}
						continue;
						IL_217:
						if (!JSGFParserTokenManager.jjCanMove_0(num12, num13, num15, num14, num16))
						{
							goto IL_23E;
						}
						if (num5 > 9)
						{
							num5 = 9;
						}
						this.jjCheckNAddStates(50, 52);
						goto IL_23E;
					}
				}
				if (num5 != 2147483647)
				{
					this.jjmatchedKind = num5;
					this.jjmatchedPos = num2;
					num5 = int.MaxValue;
				}
				num2++;
				int num17 = num4 = this.jjnewStateCnt;
				int num18 = 4;
				num6 = num3;
				int num19 = num6;
				this.jjnewStateCnt = num6;
				if (num17 == (num3 = num18 - num19))
				{
					break;
				}
				try
				{
					this.curChar = this.input_stream.readChar();
				}
				catch (IOException ex)
				{
					return num2;
				}
			}
			return num2;
		}

		[LineNumberTable(new byte[]
		{
			163,
			0,
			188,
			2,
			97,
			130,
			177,
			108,
			170,
			130
		})]
		
		private int jjMoveStringLiteralDfa1_2(long num)
		{
			try
			{
				this.curChar = this.input_stream.readChar();
			}
			catch (IOException ex)
			{
				return 1;
			}
			if (this.curChar != '/')
			{
				return 2;
			}
			if ((num & (long)((ulong)1024)) != 0L)
			{
				return this.jjStopAtPos(1, 10);
			}
			return 2;
		}

		[LineNumberTable(new byte[]
		{
			163,
			126,
			104,
			159,
			17,
			103
		})]
		
		public virtual void SwitchTo(int lexState)
		{
			if (lexState >= 4 || lexState < 0)
			{
				string message = new StringBuilder().append("Error: Ignoring invalid lexical state : ").append(lexState).append(". State unchanged.").toString();
				int reason = 2;
				
				throw new TokenMgrError(message, reason);
			}
			this.curLexState = lexState;
		}

		[LineNumberTable(new byte[]
		{
			163,
			140,
			137,
			104,
			136,
			108,
			110,
			209,
			110,
			116,
			108,
			109,
			108,
			140,
			142,
			104,
			104,
			105,
			136
		})]
		
		protected internal virtual Token jjFillToken()
		{
			string text;
			int beginLine;
			int endLine;
			int beginColumn;
			int endColumn;
			if (this.jjmatchedPos < 0)
			{
				if (this.image == null)
				{
					text = "";
				}
				else
				{
					text = this.image.toString();
				}
				endLine = (beginLine = this.input_stream.getBeginLine());
				endColumn = (beginColumn = this.input_stream.getBeginColumn());
			}
			else
			{
				string text2 = JSGFParserTokenManager.__jjstrLiteralImages[this.jjmatchedKind];
				text = ((text2 != null) ? text2 : this.input_stream.GetImage());
				beginLine = this.input_stream.getBeginLine();
				beginColumn = this.input_stream.getBeginColumn();
				endLine = this.input_stream.getEndLine();
				endColumn = this.input_stream.getEndColumn();
			}
			Token token = Token.newToken(this.jjmatchedKind, text);
			token.beginLine = beginLine;
			token.endLine = endLine;
			token.beginColumn = beginColumn;
			token.endColumn = endColumn;
			return token;
		}

		[LineNumberTable(new byte[]
		{
			71,
			191,
			160,
			104,
			138,
			138,
			138,
			138,
			139,
			104,
			141,
			138,
			138,
			138,
			138,
			141,
			138,
			138,
			141,
			141,
			141,
			138
		})]
		
		private int jjMoveStringLiteralDfa0_0()
		{
			char c = this.curChar;
			if (c == '(')
			{
				return this.jjStopAtPos(0, 36);
			}
			if (c == ')')
			{
				return this.jjStopAtPos(0, 37);
			}
			if (c == '*')
			{
				return this.jjStopAtPos(0, 30);
			}
			if (c == '+')
			{
				return this.jjStopAtPos(0, 35);
			}
			if (c == '.')
			{
				return this.jjStartNfaWithStates_0(0, 29, 5);
			}
			if (c == '/')
			{
				this.jjmatchedKind = 34;
				return this.jjMoveStringLiteralDfa1_0((long)((ulong)320));
			}
			if (c == ';')
			{
				return this.jjStopAtPos(0, 26);
			}
			if (c == '<')
			{
				return this.jjStopAtPos(0, 28);
			}
			if (c == '=')
			{
				return this.jjStopAtPos(0, 32);
			}
			if (c == '>')
			{
				return this.jjStopAtPos(0, 31);
			}
			if (c == 'V')
			{
				return this.jjMoveStringLiteralDfa1_0((long)((ulong)134217728));
			}
			if (c == '[')
			{
				return this.jjStopAtPos(0, 38);
			}
			if (c == ']')
			{
				return this.jjStopAtPos(0, 39);
			}
			if (c == 'g')
			{
				return this.jjMoveStringLiteralDfa1_0((long)((ulong)8192));
			}
			if (c == 'i')
			{
				return this.jjMoveStringLiteralDfa1_0((long)((ulong)16384));
			}
			if (c == 'p')
			{
				return this.jjMoveStringLiteralDfa1_0((long)((ulong)32768));
			}
			if (c == '|')
			{
				return this.jjStopAtPos(0, 33);
			}
			return this.jjMoveNfa_0(3, 0);
		}

		
		
		private int jjMoveStringLiteralDfa0_1()
		{
			return this.jjMoveNfa_1(4, 0);
		}

		[LineNumberTable(new byte[]
		{
			162,
			246,
			177,
			141
		})]
		
		private int jjMoveStringLiteralDfa0_2()
		{
			if (this.curChar == '*')
			{
				return this.jjMoveStringLiteralDfa1_2((long)((ulong)1024));
			}
			return 1;
		}

		[LineNumberTable(new byte[]
		{
			162,
			108,
			177,
			141
		})]
		
		private int jjMoveStringLiteralDfa0_3()
		{
			if (this.curChar == '*')
			{
				return this.jjMoveStringLiteralDfa1_3((long)((ulong)2048));
			}
			return 1;
		}

		internal virtual void SkipLexicalActions(Token token)
		{
			int num = this.jjmatchedKind;
		}

		[LineNumberTable(new byte[]
		{
			164,
			64,
			127,
			1,
			176,
			125,
			103,
			108,
			194
		})]
		
		internal virtual void MoreLexicalActions()
		{
			int num = this.jjimageLen;
			int num2 = this.jjmatchedPos + 1;
			int num3 = num2;
			this.lengthOfMatch = num2;
			this.jjimageLen = num + num3;
			if (this.jjmatchedKind == 7)
			{
				this.image.append(this.input_stream.GetSuffix(this.jjimageLen));
				this.jjimageLen = 0;
				this.input_stream.backup(1);
			}
		}

		public virtual void setDebugStream(PrintStream ds)
		{
			this.debugStream = ds;
		}

		[LineNumberTable(new byte[]
		{
			163,
			99,
			105,
			103
		})]
		
		public JSGFParserTokenManager(JavaCharStream stream, int lexState) : this(stream)
		{
			this.SwitchTo(lexState);
		}

		[LineNumberTable(new byte[]
		{
			163,
			120,
			103,
			103
		})]
		
		public virtual void ReInit(JavaCharStream stream, int lexState)
		{
			this.ReInit(stream);
			this.SwitchTo(lexState);
		}

		[LineNumberTable(new byte[]
		{
			160,
			204,
			191,
			1,
			191,
			0,
			191,
			22,
			191,
			7,
			191,
			0,
			191,
			4,
			191,
			0,
			255,
			7,
			162,
			46,
			255,
			160,
			219,
			103,
			255,
			160,
			180,
			70,
			255,
			12,
			71,
			223,
			160,
			108,
			183,
			180,
			180
		})]
		static JSGFParserTokenManager()
		{
		}

		
		public static string[] jjstrLiteralImages
		{
			
			get
			{
				return JSGFParserTokenManager.__jjstrLiteralImages;
			}
		}

		
		public static string[] lexStateNames
		{
			
			get
			{
				return JSGFParserTokenManager.__lexStateNames;
			}
		}

		
		public static int[] jjnewLexState
		{
			
			get
			{
				return JSGFParserTokenManager.__jjnewLexState;
			}
		}

		public PrintStream debugStream;

		
		internal static long[] jjbitVec0 = new long[]
		{
			-2L,
			-1L,
			-1L,
			-1L
		};

		
		internal static long[] jjbitVec2 = new long[]
		{
			0L,
			0L,
			-1L,
			-1L
		};

		
		internal static long[] jjbitVec3 = new long[]
		{
			2301339413881290750L,
			-16384L,
			(long)((ulong)-1),
			432345564227567616L
		};

		
		internal static long[] jjbitVec4 = new long[]
		{
			0L,
			0L,
			0L,
			-36028797027352577L
		};

		
		internal static long[] jjbitVec5 = new long[]
		{
			0L,
			-1L,
			-1L,
			-1L
		};

		
		internal static long[] jjbitVec6 = new long[]
		{
			-1L,
			-1L,
			(long)((ulong)65535),
			0L
		};

		
		internal static long[] jjbitVec7 = new long[]
		{
			-1L,
			-1L,
			0L,
			0L
		};

		
		internal static long[] jjbitVec8 = new long[]
		{
			70368744177663L,
			0L,
			0L,
			0L
		};

		
		internal static int[] jjnextStates = new int[]
		{
			20,
			21,
			23,
			39,
			40,
			41,
			46,
			47,
			50,
			51,
			9,
			11,
			13,
			5,
			6,
			9,
			20,
			21,
			25,
			23,
			29,
			30,
			32,
			29,
			30,
			34,
			32,
			42,
			43,
			9,
			50,
			51,
			9,
			7,
			8,
			14,
			15,
			17,
			22,
			24,
			26,
			31,
			33,
			35,
			44,
			45,
			48,
			49,
			52,
			53,
			0,
			1,
			3
		};

		internal static string[] __jjstrLiteralImages = new string[]
		{
			"",
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			"grammar",
			"import",
			"public",
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			";",
			"V1.0",
			"<",
			".",
			"*",
			">",
			"=",
			"|",
			"/",
			"+",
			"(",
			")",
			"[",
			"]"
		};

		internal static string[] __lexStateNames = new string[]
		{
			"DEFAULT",
			"IN_SINGLE_LINE_COMMENT",
			"IN_FORMAL_COMMENT",
			"IN_MULTI_LINE_COMMENT"
		};

		internal static int[] __jjnewLexState = new int[]
		{
			-1,
			-1,
			-1,
			-1,
			-1,
			-1,
			1,
			2,
			3,
			0,
			0,
			0,
			-1,
			-1,
			-1,
			-1,
			-1,
			-1,
			-1,
			-1,
			-1,
			-1,
			-1,
			-1,
			-1,
			-1,
			-1,
			-1,
			-1,
			-1,
			-1,
			-1,
			-1,
			-1,
			-1,
			-1,
			-1,
			-1,
			-1,
			-1
		};

		
		internal static long[] jjtoToken = new long[]
		{
			1099460632577L
		};

		
		internal static long[] jjtoSkip = new long[]
		{
			(long)((ulong)3646)
		};

		
		internal static long[] jjtoSpecial = new long[]
		{
			(long)((ulong)3584)
		};

		
		internal static long[] jjtoMore = new long[]
		{
			(long)((ulong)4544)
		};

		protected internal JavaCharStream input_stream;

		
		private int[] jjrounds;

		
		private int[] jjstateSet;

		
		private StringBuilder jjimage;

		private StringBuilder image;

		private int jjimageLen;

		private int lengthOfMatch;

		protected internal char curChar;

		internal int curLexState;

		internal int defaultLexState;

		internal int jjnewStateCnt;

		internal int jjround;

		internal int jjmatchedPos;

		internal int jjmatchedKind;
	}
}
