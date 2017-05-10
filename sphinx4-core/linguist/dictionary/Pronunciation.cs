using System;

using edu.cmu.sphinx.linguist.acoustic;
using IKVM.Attributes;
using java.lang;
using java.util;

namespace edu.cmu.sphinx.linguist.dictionary
{
	public class Pronunciation : java.lang.Object
	{
		
		public static void __<clinit>()
		{
		}

		public virtual Word getWord()
		{
			return this.word;
		}

		public virtual Unit[] getUnits()
		{
			return this.units;
		}

		[LineNumberTable(new byte[]
		{
			159,
			182,
			104,
			103,
			103,
			104
		})]
		
		public Pronunciation(Unit[] units, string tag, float probability)
		{
			this.units = units;
			this.tag = tag;
			this.probability = probability;
		}

		[LineNumberTable(new byte[]
		{
			28,
			104,
			137,
			144
		})]
		
		public virtual void setWord(Word word)
		{
			if (this.word == null)
			{
				this.word = word;
				return;
			}
			string text = "Word of Pronunciation cannot be set twice.";
			
			throw new Error(text);
		}

		
		[LineNumberTable(new byte[]
		{
			18,
			109
		})]
		
		protected internal Pronunciation(List units) : this(units, null, 1f)
		{
		}

		
		[LineNumberTable(new byte[]
		{
			5,
			104,
			119,
			103,
			103,
			104
		})]
		
		protected internal Pronunciation(List units, string tag, float probability)
		{
			Unit[] array = (Unit[])units.toArray(new Unit[units.size()]);
			this.units = array;
			this.tag = tag;
			this.probability = probability;
		}

		[LineNumberTable(new byte[]
		{
			95,
			120,
			117,
			48,
			166,
			105
		})]
		
		public override string toString()
		{
			StringBuilder stringBuilder = new StringBuilder().append(this.word).append('(');
			Unit[] array = this.units;
			int num = array.Length;
			for (int i = 0; i < num; i++)
			{
				Unit unit = array[i];
				stringBuilder.append(unit).append(' ');
			}
			stringBuilder.append(')');
			return stringBuilder.toString();
		}

		public virtual string getTag()
		{
			return this.tag;
		}

		public virtual float getProbability()
		{
			return this.probability;
		}

		[LineNumberTable(new byte[]
		{
			84,
			112
		})]
		
		public virtual void dump()
		{
			java.lang.System.@out.println(this.toString());
		}

		[LineNumberTable(new byte[]
		{
			110,
			120,
			117,
			48,
			166,
			159,
			23
		})]
		
		public virtual string toDetailedString()
		{
			StringBuilder stringBuilder = new StringBuilder().append(this.word).append(' ');
			Unit[] array = this.units;
			int num = array.Length;
			for (int i = 0; i < num; i++)
			{
				Unit unit = array[i];
				stringBuilder.append(unit).append(' ');
			}
			stringBuilder.append("\n   class: ").append(" tag: ").append(this.tag).append(" prob: ").append(this.probability);
			return stringBuilder.toString();
		}

		
		static Pronunciation()
		{
		}

		
		public static Pronunciation UNKNOWN
		{
			
			get
			{
				return Pronunciation.__UNKNOWN;
			}
		}

		internal static Pronunciation __UNKNOWN = new Pronunciation(Unit.__EMPTY_ARRAY, null, 1f);

		private Word word;

		
		private Unit[] units;

		
		private string tag;

		
		private float probability;
	}
}
