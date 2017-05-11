using System;

using edu.cmu.sphinx.linguist.acoustic;
using IKVM.Attributes;

namespace edu.cmu.sphinx.linguist.lextree
{
	.
	internal abstract class UnitNode : Node
	{
		
		public new static void __<clinit>()
		{
		}

		internal abstract Unit getBaseUnit();

		internal abstract object getKey();

		[LineNumberTable(new byte[]
		{
			161,
			36,
			104
		})]
		
		internal UnitNode(float num) : base(num)
		{
		}

		internal abstract HMMPosition getPosition();

		internal virtual int getType()
		{
			return this.type;
		}

		internal virtual void setType(int num)
		{
			this.type = num;
		}

		
		static UnitNode()
		{
			Node.__<clinit>();
		}

		public const int SIMPLE_UNIT = 1;

		public const int WORD_BEGINNING_UNIT = 2;

		public const int SILENCE_UNIT = 3;

		public const int FILLER_UNIT = 4;

		private int type;
	}
}
