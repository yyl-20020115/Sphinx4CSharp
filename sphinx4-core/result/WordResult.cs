using System;

using edu.cmu.sphinx.linguist.dictionary;
using edu.cmu.sphinx.util;
using IKVM.Attributes;
using java.lang;
using java.util;

namespace edu.cmu.sphinx.result
{
	public class WordResult : java.lang.Object
	{
		public virtual TimeFrame getTimeFrame()
		{
			return this.timeFrame;
		}

		public virtual Word getWord()
		{
			return this.word;
		}

		[LineNumberTable(new byte[]
		{
			7,
			109,
			113,
			11,
			165
		})]
		
		public WordResult(Node node)
		{
			Word w = node.getWord();
			TimeFrame.__<clinit>();
			this..ctor(w, new TimeFrame(node.getBeginTime(), node.getEndTime()), node.getViterbiScore(), node.getPosterior());
		}

		[LineNumberTable(new byte[]
		{
			159,
			183,
			104,
			103,
			103,
			105,
			106
		})]
		
		public WordResult(Word w, TimeFrame timeFrame, double score, double posterior)
		{
			this.word = w;
			this.timeFrame = timeFrame;
			this.score = score;
			this.confidence = posterior;
		}

		
		
		public virtual double getConfidence()
		{
			return java.lang.Math.min(this.confidence, (double)0f);
		}

		public virtual double getScore()
		{
			return this.score;
		}

		
		
		public virtual Pronunciation getPronunciation()
		{
			return this.word.getMostLikelyPronunciation();
		}

		
		
		public virtual bool isFiller()
		{
			return this.word.isFiller() || java.lang.String.instancehelper_equals(this.word.toString(), "<skip>");
		}

		
		
		public override string toString()
		{
			return java.lang.String.format(Locale.US, "{%s, %.3f, [%s]}", new object[]
			{
				this.word,
				Double.valueOf(LogMath.getLogMath().logToLinear((float)this.getConfidence())),
				this.timeFrame
			});
		}

		
		private Word word;

		
		private TimeFrame timeFrame;

		
		private double score;

		
		private double confidence;
	}
}
