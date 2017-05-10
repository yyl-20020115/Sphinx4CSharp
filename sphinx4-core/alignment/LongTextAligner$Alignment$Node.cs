using System;

using IKVM.Attributes;
using java.lang;
using java.util;

namespace edu.cmu.sphinx.alignment
{
	[NonNestedOuterClass("edu.cmu.sphinx.alignment.LongTextAligner$Alignment")]
	
	[SourceFile("LongTextAligner.java")]
	public sealed class LongTextAligner$Alignment$Node : java.lang.Object
	{
		[LineNumberTable(new byte[]
		{
			159,
			179,
			111,
			103,
			103
		})]
		
		private LongTextAligner$Alignment$Node(LongTextAligner.Alignment alignment, int num, int num2)
		{
			this.databaseIndex = num2;
			this.queryIndex = num;
		}

		
		
		public int getQueryIndex()
		{
			return ((Integer)LongTextAligner.Alignment.access$100(this.this$1).get(this.queryIndex - 1)).intValue();
		}

		
		
		public int getDatabaseIndex()
		{
			return ((Integer)LongTextAligner.Alignment.access$000(this.this$1).get(this.databaseIndex - 1)).intValue();
		}

		public bool isBoundary()
		{
			return this.queryIndex == 0 || this.databaseIndex == 0;
		}

		
		
		public bool hasMatch()
		{
			return java.lang.String.instancehelper_equals(this.getQueryWord(), this.getDatabaseWord());
		}

		[LineNumberTable(new byte[]
		{
			1,
			105,
			124
		})]
		
		public string getQueryWord()
		{
			if (this.queryIndex > 0)
			{
				return (string)LongTextAligner.Alignment.access$200(this.this$1).get(this.getQueryIndex());
			}
			return null;
		}

		[LineNumberTable(new byte[]
		{
			7,
			105,
			127,
			2
		})]
		
		public string getDatabaseWord()
		{
			if (this.databaseIndex > 0)
			{
				return (string)LongTextAligner.access$300(this.this$1.this$0).get(this.getDatabaseIndex());
			}
			return null;
		}

		[LineNumberTable(new byte[]
		{
			13,
			104,
			114
		})]
		
		public int getValue()
		{
			if (this.isBoundary())
			{
				return java.lang.Math.max(this.queryIndex, this.databaseIndex);
			}
			return (!this.hasMatch()) ? 1 : 0;
		}

		[LineNumberTable(new byte[]
		{
			27,
			127,
			5,
			48
		})]
		
		public bool isTarget()
		{
			return this.queryIndex == LongTextAligner.Alignment.access$100(this.this$1).size() && this.databaseIndex == LongTextAligner.Alignment.access$000(this.this$1).size();
		}

		
		[LineNumberTable(new byte[]
		{
			32,
			103,
			127,
			5,
			108,
			159,
			3,
			120,
			159,
			1,
			120,
			191,
			1
		})]
		
		public List adjacent()
		{
			ArrayList arrayList = new ArrayList(3);
			if (this.queryIndex < LongTextAligner.Alignment.access$100(this.this$1).size() && this.databaseIndex < LongTextAligner.Alignment.access$000(this.this$1).size())
			{
				arrayList.add(new LongTextAligner$Alignment$Node(this.this$1, this.queryIndex + 1, this.databaseIndex + 1));
			}
			if (this.databaseIndex < LongTextAligner.Alignment.access$000(this.this$1).size())
			{
				arrayList.add(new LongTextAligner$Alignment$Node(this.this$1, this.queryIndex, this.databaseIndex + 1));
			}
			if (this.queryIndex < LongTextAligner.Alignment.access$100(this.this$1).size())
			{
				arrayList.add(new LongTextAligner$Alignment$Node(this.this$1, this.queryIndex + 1, this.databaseIndex));
			}
			return arrayList;
		}

		[LineNumberTable(new byte[]
		{
			49,
			104,
			130,
			103
		})]
		public override bool equals(object @object)
		{
			if (!(@object is LongTextAligner$Alignment$Node))
			{
				return false;
			}
			LongTextAligner$Alignment$Node longTextAligner$Alignment$Node = (LongTextAligner$Alignment$Node)@object;
			return this.queryIndex == longTextAligner$Alignment$Node.queryIndex && this.databaseIndex == longTextAligner$Alignment$Node.databaseIndex;
		}

		public override int hashCode()
		{
			return 31 * (31 * this.queryIndex + this.databaseIndex);
		}

		
		
		public override string toString()
		{
			return java.lang.String.format("[%d %d]", new object[]
			{
				Integer.valueOf(this.queryIndex),
				Integer.valueOf(this.databaseIndex)
			});
		}

		
		
		
		internal LongTextAligner$Alignment$Node(LongTextAligner.Alignment alignment, int num, int num2, LongTextAligner$1 longTextAligner$) : this(alignment, num, num2)
		{
		}

		
		
		internal static int access$600(LongTextAligner$Alignment$Node longTextAligner$Alignment$Node)
		{
			return longTextAligner$Alignment$Node.queryIndex;
		}

		
		
		internal static int access$700(LongTextAligner$Alignment$Node longTextAligner$Alignment$Node)
		{
			return longTextAligner$Alignment$Node.databaseIndex;
		}

		
		private int databaseIndex;

		
		private int queryIndex;

		
		internal LongTextAligner.Alignment this$1 = alignment;
	}
}
