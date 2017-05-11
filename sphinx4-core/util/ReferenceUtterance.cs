using System;

using IKVM.Attributes;
using java.lang;
using java.util;

namespace edu.cmu.sphinx.util
{
	.
	
	internal sealed class ReferenceUtterance : java.lang.Object
	{
		internal float getEndTime()
		{
			return this.endTime;
		}

		internal bool isSilenceGap()
		{
			return this.isSilenceGap;
		}

		internal float getStartTime()
		{
			return this.startTime;
		}

		[LineNumberTable(new byte[]
		{
			160,
			80,
			104,
			103,
			103,
			103,
			103,
			109,
			135,
			113,
			145,
			104,
			103,
			113,
			108,
			46,
			200,
			140
		})]
		
		internal ReferenceUtterance(string text)
		{
			StringTokenizer stringTokenizer = new StringTokenizer(text);
			stringTokenizer.nextToken();
			stringTokenizer.nextToken();
			string text2 = stringTokenizer.nextToken();
			if (java.lang.String.instancehelper_equals(text2, "inter_segment_gap"))
			{
				this.isSilenceGap = true;
			}
			this.startTime = Float.parseFloat(stringTokenizer.nextToken());
			this.endTime = Float.parseFloat(stringTokenizer.nextToken());
			if (stringTokenizer.hasMoreTokens())
			{
				stringTokenizer.nextToken();
				this.words = new string[stringTokenizer.countTokens()];
				for (int i = 0; i < this.words.Length; i++)
				{
					this.words[i] = stringTokenizer.nextToken();
				}
			}
			else
			{
				this.words = new string[0];
			}
		}

		internal string[] getWords()
		{
			return this.words;
		}

		private bool isSilenceGap;

		
		private float startTime;

		
		private float endTime;

		
		private string[] words;
	}
}
