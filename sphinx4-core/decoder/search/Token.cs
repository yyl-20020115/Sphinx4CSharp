using System;

using edu.cmu.sphinx.decoder.scorer;
using edu.cmu.sphinx.frontend;
using edu.cmu.sphinx.linguist;
using edu.cmu.sphinx.linguist.acoustic;
using edu.cmu.sphinx.linguist.dictionary;
using IKVM.Attributes;
using java.lang;
using java.text;
using java.util;

namespace edu.cmu.sphinx.decoder.search
{
	[Implements(new string[]
	{
		"edu.cmu.sphinx.decoder.scorer.Scoreable"
	})]
	public class Token : java.lang.Object, Scoreable, Data
	{
		
		public static void __<clinit>()
		{
		}

		public virtual Data getData()
		{
			return this.data;
		}

		public virtual SearchState getSearchState()
		{
			return this.searchState;
		}

		public virtual Token getPredecessor()
		{
			return this.predecessor;
		}

		
		
		public virtual float[] calculateComponentScore(Data feature)
		{
			return ((ScoreProvider)this.searchState).getComponentScore(feature);
		}

		public virtual float getScore()
		{
			return this.logTotalScore;
		}

		
		public virtual bool isWord()
		{
			return this.searchState is WordSearchState;
		}

		public virtual float getAcousticScore()
		{
			return this.logAcousticScore;
		}

		public virtual float getLanguageScore()
		{
			return this.logLanguageScore;
		}

		public virtual float getInsertionScore()
		{
			return this.logInsertionScore;
		}

		[LineNumberTable(new byte[]
		{
			56,
			116,
			104
		})]
		
		public Token(Token predecessor, float logTotalScore, float logAcousticScore, float logInsertionScore, float logLanguageScore) : this(predecessor, null, logTotalScore, logInsertionScore, logLanguageScore, 0L)
		{
			this.logAcousticScore = logAcousticScore;
		}

		
		
		public virtual bool isFinal()
		{
			return this.searchState.isFinal();
		}

		
		
		public virtual bool isEmitting()
		{
			return this.searchState.isEmitting();
		}

		[LineNumberTable(new byte[]
		{
			38,
			120
		})]
		
		public Token(SearchState state, long collectTime) : this(null, state, 0f, 0f, 0f, collectTime)
		{
		}

		public virtual long getCollectTime()
		{
			return this.collectTime;
		}

		[LineNumberTable(new byte[]
		{
			20,
			104,
			103,
			103,
			104,
			105,
			105,
			104,
			108
		})]
		
		public Token(Token predecessor, SearchState state, float logTotalScore, float logInsertionScore, float logLanguageScore, long collectTime)
		{
			this.predecessor = predecessor;
			this.searchState = state;
			this.logTotalScore = logTotalScore;
			this.logInsertionScore = logInsertionScore;
			this.logLanguageScore = logLanguageScore;
			this.collectTime = collectTime;
			Token.curCount++;
		}

		public virtual void update(Token predecessor, SearchState nextState, float logEntryScore, float insertionProbability, float languageProbability, long collectTime)
		{
			this.predecessor = predecessor;
			this.searchState = nextState;
			this.logTotalScore = logEntryScore;
			this.logInsertionScore = insertionProbability;
			this.logLanguageScore = languageProbability;
			this.collectTime = collectTime;
		}

		[LineNumberTable(new byte[]
		{
			85,
			103,
			104,
			145
		})]
		
		public virtual void setData(Data data)
		{
			this.data = data;
			if (data is FloatData)
			{
				this.collectTime = ((FloatData)data).getCollectTime();
			}
		}

		[LineNumberTable(new byte[]
		{
			159,
			65,
			130,
			98,
			134,
			99,
			104,
			137,
			109,
			109,
			100,
			108,
			255,
			5,
			60,
			230,
			71,
			106
		})]
		
		public virtual void dumpTokenPath(bool includeHMMStates)
		{
			Token token = this;
			ArrayList arrayList = new ArrayList();
			while (token != null)
			{
				arrayList.add(token);
				token = token.getPredecessor();
			}
			for (int i = arrayList.size() - 1; i >= 0; i += -1)
			{
				token = (Token)arrayList.get(i);
				if (includeHMMStates || !(token.getSearchState() is HMMSearchState))
				{
					java.lang.System.@out.println(new StringBuilder().append("  ").append(token).toString());
				}
			}
			java.lang.System.@out.println();
		}

		[LineNumberTable(new byte[]
		{
			159,
			58,
			68,
			102,
			130,
			102,
			107,
			97,
			108,
			105,
			206,
			111,
			102,
			106,
			105,
			108,
			115,
			18,
			200,
			138,
			111,
			170,
			140
		})]
		
		public virtual string getWordPath(bool wantFiller, bool wantPronunciations)
		{
			StringBuilder stringBuilder = new StringBuilder();
			for (Token token = this; token != null; token = token.getPredecessor())
			{
				if (token.isWord())
				{
					WordSearchState wordSearchState = (WordSearchState)token.getSearchState();
					Pronunciation pronunciation = wordSearchState.getPronunciation();
					Word word = wordSearchState.getPronunciation().getWord();
					if (wantFiller || !word.isFiller())
					{
						if (wantPronunciations)
						{
							stringBuilder.insert(0, ']');
							Unit[] units = pronunciation.getUnits();
							for (int i = units.Length - 1; i >= 0; i += -1)
							{
								if (i < units.Length - 1)
								{
									stringBuilder.insert(0, ',');
								}
								stringBuilder.insert(0, units[i].getName());
							}
							stringBuilder.insert(0, '[');
						}
						stringBuilder.insert(0, word.getSpelling());
						stringBuilder.insert(0, ' ');
					}
				}
			}
			return java.lang.String.instancehelper_trim(stringBuilder.toString());
		}

		[LineNumberTable(new byte[]
		{
			121,
			151,
			148,
			135
		})]
		
		public virtual float calculateScore(Data feature)
		{
			this.logAcousticScore = ((ScoreProvider)this.searchState).getScore(feature);
			this.logTotalScore += this.logAcousticScore;
			this.setData(feature);
			return this.logTotalScore;
		}

		[LineNumberTable(new byte[]
		{
			160,
			78,
			112,
			112
		})]
		public virtual float normalizeScore(float maxLogScore)
		{
			this.logTotalScore -= maxLogScore;
			this.logAcousticScore -= maxLogScore;
			return this.logTotalScore;
		}

		public virtual void setScore(float logScore)
		{
			this.logTotalScore = logScore;
		}

		[LineNumberTable(new byte[]
		{
			160,
			175,
			107,
			124,
			125,
			125,
			120,
			239,
			59
		})]
		
		public override string toString()
		{
			return new StringBuilder().append(Token.numFmt.format(this.getCollectTime())).append(' ').append(Token.scoreFmt.format((double)this.getScore())).append(' ').append(Token.scoreFmt.format((double)this.getAcousticScore())).append(' ').append(Token.scoreFmt.format((double)this.getLanguageScore())).append(' ').append(this.getSearchState()).toString();
		}

		[LineNumberTable(new byte[]
		{
			160,
			186,
			103
		})]
		
		public virtual void dumpTokenPath()
		{
			this.dumpTokenPath(true);
		}

		
		
		public virtual string getWordPathNoFiller()
		{
			return this.getWordPath(false, false);
		}

		
		
		public virtual string getWordPath()
		{
			return this.getWordPath(true, false);
		}

		[LineNumberTable(new byte[]
		{
			161,
			24,
			102,
			130,
			102,
			103,
			104,
			103,
			109,
			127,
			6,
			106,
			104,
			105,
			159,
			6,
			103,
			101
		})]
		
		public virtual string getWordUnitPath()
		{
			StringBuilder stringBuilder = new StringBuilder();
			for (Token token = this; token != null; token = token.getPredecessor())
			{
				SearchState searchState = token.getSearchState();
				if (searchState is WordSearchState)
				{
					WordSearchState wordSearchState = (WordSearchState)searchState;
					Word word = wordSearchState.getPronunciation().getWord();
					stringBuilder.insert(0, new StringBuilder().append(' ').append(word.getSpelling()).toString());
				}
				else if (searchState is UnitSearchState)
				{
					UnitSearchState unitSearchState = (UnitSearchState)searchState;
					Unit unit = unitSearchState.getUnit();
					stringBuilder.insert(0, new StringBuilder().append(' ').append(unit.getName()).toString());
				}
			}
			return java.lang.String.instancehelper_trim(stringBuilder.toString());
		}

		[LineNumberTable(new byte[]
		{
			161,
			51,
			104,
			108,
			140
		})]
		
		public virtual Word getWord()
		{
			if (this.isWord())
			{
				WordSearchState wordSearchState = (WordSearchState)this.searchState;
				return wordSearchState.getPronunciation().getWord();
			}
			return null;
		}

		[LineNumberTable(new byte[]
		{
			161,
			62,
			159,
			35,
			106
		})]
		
		public static void showCount()
		{
			java.lang.System.@out.println(new StringBuilder().append("Cur count: ").append(Token.curCount).append(" new ").append(Token.curCount - Token.lastCount).toString());
			Token.lastCount = Token.curCount;
		}

		public virtual bool validate()
		{
			return true;
		}

		protected internal static DecimalFormat getScoreFormat()
		{
			return Token.scoreFmt;
		}

		protected internal static DecimalFormat getNumberFormat()
		{
			return Token.numFmt;
		}

		[LineNumberTable(new byte[]
		{
			159,
			182,
			111
		})]
		static Token()
		{
		}

		private static int curCount;

		private static int lastCount;

		
		private static DecimalFormat scoreFmt = new DecimalFormat("0.0000000E00");

		
		private static DecimalFormat numFmt = new DecimalFormat("0000");

		private Token predecessor;

		private float logLanguageScore;

		private float logTotalScore;

		private float logInsertionScore;

		private float logAcousticScore;

		private SearchState searchState;

		private long collectTime;

		private Data data;
	}
}
