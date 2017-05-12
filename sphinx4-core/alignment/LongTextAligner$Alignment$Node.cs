using java.lang;
using java.util;

namespace edu.cmu.sphinx.alignment
{
	public sealed class LongTextAligner_Alignment_Node : java.lang.Object
	{	
		private LongTextAligner_Alignment_Node(LongTextAligner.Alignment alignment, int num, int num2)
		{
			this_1 = alignment;
			this.databaseIndex = num2;
			this.queryIndex = num;
		}

		public int getQueryIndex()
		{
			return ((Integer)LongTextAligner.Alignment.access_100(this.this_1).get(this.queryIndex - 1)).intValue();
		}
		
		public int getDatabaseIndex()
		{
			return ((Integer)LongTextAligner.Alignment.access_000(this.this_1).get(this.databaseIndex - 1)).intValue();
		}

		public bool isBoundary()
		{
			return this.queryIndex == 0 || this.databaseIndex == 0;
		}

		public bool hasMatch()
		{
			return java.lang.String.instancehelper_equals(this.getQueryWord(), this.getDatabaseWord());
		}

		public string getQueryWord()
		{
			if (this.queryIndex > 0)
			{
				return (string)LongTextAligner.Alignment.access_200(this.this_1).get(this.getQueryIndex());
			}
			return null;
		}
	
		public string getDatabaseWord()
		{
			if (this.databaseIndex > 0)
			{
				return (string)LongTextAligner.access_300(this.this_1.this_0).get(this.getDatabaseIndex());
			}
			return null;
		}
	
		public int getValue()
		{
			if (this.isBoundary())
			{
				return java.lang.Math.max(this.queryIndex, this.databaseIndex);
			}
			return (!this.hasMatch()) ? 1 : 0;
		}
	
		public bool isTarget()
		{
			return this.queryIndex == LongTextAligner.Alignment.access_100(this.this_1).size() && this.databaseIndex == LongTextAligner.Alignment.access_000(this.this_1).size();
		}
	
		public List adjacent()
		{
			ArrayList arrayList = new ArrayList(3);
			if (this.queryIndex < LongTextAligner.Alignment.access_100(this.this_1).size() && this.databaseIndex < LongTextAligner.Alignment.access_000(this.this_1).size())
			{
				arrayList.add(new LongTextAligner_Alignment_Node(this.this_1, this.queryIndex + 1, this.databaseIndex + 1));
			}
			if (this.databaseIndex < LongTextAligner.Alignment.access_000(this.this_1).size())
			{
				arrayList.add(new LongTextAligner_Alignment_Node(this.this_1, this.queryIndex, this.databaseIndex + 1));
			}
			if (this.queryIndex < LongTextAligner.Alignment.access_100(this.this_1).size())
			{
				arrayList.add(new LongTextAligner_Alignment_Node(this.this_1, this.queryIndex + 1, this.databaseIndex));
			}
			return arrayList;
		}
		public override bool equals(object @object)
		{
			if (!(@object is LongTextAligner_Alignment_Node))
			{
				return false;
			}
			LongTextAligner_Alignment_Node longTextAligner_Alignment_Node = (LongTextAligner_Alignment_Node)@object;
			return this.queryIndex == longTextAligner_Alignment_Node.queryIndex && this.databaseIndex == longTextAligner_Alignment_Node.databaseIndex;
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
		
		internal LongTextAligner_Alignment_Node(LongTextAligner.Alignment alignment, int num, int num2, LongTextAligner_1 longTextAligner_) : this(alignment, num, num2)
		{
		}
		
		internal static int access_600(LongTextAligner_Alignment_Node longTextAligner_Alignment_Node)
		{
			return longTextAligner_Alignment_Node.queryIndex;
		}
		
		internal static int access_700(LongTextAligner_Alignment_Node longTextAligner_Alignment_Node)
		{
			return longTextAligner_Alignment_Node.databaseIndex;
		}
		
		private int databaseIndex;

		private int queryIndex;
		
		internal LongTextAligner.Alignment this_1;
	}
}
