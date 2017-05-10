using System;

using IKVM.Attributes;
using java.lang;

namespace edu.cmu.sphinx.util
{
	[SourceFile("GapInsertionDetector.java")]
	
	internal sealed class HypothesisWord : java.lang.Object
	{
		internal float getStartTime()
		{
			return this.startTime;
		}

		internal float getEndTime()
		{
			return this.endTime;
		}

		internal string getText()
		{
			return this.text;
		}

		[LineNumberTable(new byte[]
		{
			161,
			55,
			104,
			103,
			104,
			104
		})]
		
		internal HypothesisWord(string text, float num, float num2)
		{
			this.text = text;
			this.startTime = num;
			this.endTime = num2;
		}

		
		private string text;

		
		private float startTime;

		
		private float endTime;
	}
}
