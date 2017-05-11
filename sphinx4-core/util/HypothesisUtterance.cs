using System;

using IKVM.Attributes;
using IKVM.Runtime;
using java.lang;
using java.util;

namespace edu.cmu.sphinx.util
{
	.
	
	internal sealed class HypothesisUtterance : java.lang.Object
	{
		
		[LineNumberTable(new byte[]
		{
			161,
			12,
			102,
			109
		})]
		
		internal List getWords()
		{
			LinkedList linkedList = new LinkedList();
			linkedList.addAll(this.words);
			return linkedList;
		}

		[LineNumberTable(new byte[]
		{
			160,
			226,
			104,
			107,
			108,
			107,
			135,
			108,
			109,
			139,
			223,
			3,
			226,
			61,
			98,
			127,
			5,
			135,
			101,
			109,
			115,
			109,
			108,
			115,
			141
		})]
		
		internal HypothesisUtterance(string text)
		{
			this.words = new LinkedList();
			StringTokenizer stringTokenizer = new StringTokenizer(text, " \t\n\r\f(),");
			while (stringTokenizer.hasMoreTokens())
			{
				string text2 = stringTokenizer.nextToken();
				NumberFormatException ex2;
				try
				{
					float num = Float.parseFloat(stringTokenizer.nextToken());
					float num2 = Float.parseFloat(stringTokenizer.nextToken());
					HypothesisWord hypothesisWord = new HypothesisWord(text2, num, num2);
					this.words.add(hypothesisWord);
				}
				catch (NumberFormatException ex)
				{
					ex2 = ByteCodeHelper.MapException<NumberFormatException>(ex, 1);
					goto IL_73;
				}
				continue;
				IL_73:
				NumberFormatException ex3 = ex2;
				java.lang.System.@out.println(new StringBuilder().append("NumberFormatException at line: ").append(text).toString());
				Throwable.instancehelper_printStackTrace(ex3);
			}
			if (!this.words.isEmpty())
			{
				HypothesisWord hypothesisWord2 = (HypothesisWord)this.words.get(0);
				this.startTime = hypothesisWord2.getStartTime();
				HypothesisWord hypothesisWord3 = (HypothesisWord)this.words.get(this.words.size() - 1);
				this.endTime = hypothesisWord3.getEndTime();
			}
		}

		
		
		internal int getWordCount()
		{
			return this.words.size();
		}

		internal float getStartTime()
		{
			return this.startTime;
		}

		internal float getEndTime()
		{
			return this.endTime;
		}

		
		
		private List words;

		private float startTime;

		private float endTime;
	}
}
