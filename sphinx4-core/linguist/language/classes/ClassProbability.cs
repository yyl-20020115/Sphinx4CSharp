using System;

using IKVM.Attributes;
using java.lang;

namespace edu.cmu.sphinx.linguist.language.classes
{
	
	internal sealed class ClassProbability : java.lang.Object
	{
		public string getClassName()
		{
			return this.className;
		}

		public float getLogProbability()
		{
			return this.logProbability;
		}

		[LineNumberTable(new byte[]
		{
			159,
			162,
			104,
			103,
			104
		})]
		
		public ClassProbability(string text, float num)
		{
			this.className = text;
			this.logProbability = num;
		}

		
		private string className;

		
		private float logProbability;
	}
}
