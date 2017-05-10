using System;

using edu.cmu.sphinx.util.props;
using IKVM.Attributes;
using java.lang;

namespace edu.cmu.sphinx.frontend.feature
{
	public class FrameDropper : BaseDataProcessor
	{
		[LineNumberTable(new byte[]
		{
			159,
			125,
			130,
			103,
			100,
			208,
			103
		})]
		
		protected internal virtual void initVars(int dropEveryNthFrame, bool replaceNthWithPrevious)
		{
			this.dropEveryNthFrame = dropEveryNthFrame;
			if (dropEveryNthFrame <= 1)
			{
				string text = "dropEveryNthFramemust be greater than one";
				
				throw new IllegalArgumentException(text);
			}
			this.replaceNthWithPrevious = replaceNthWithPrevious;
		}

		[Throws(new string[]
		{
			"edu.cmu.sphinx.frontend.DataProcessingException"
		})]
		[LineNumberTable(new byte[]
		{
			99,
			108,
			99,
			142
		})]
		
		private Data readData()
		{
			Data data = this.getPredecessor().getData();
			if (data != null)
			{
				this.id++;
			}
			return data;
		}

		[LineNumberTable(new byte[]
		{
			159,
			130,
			130,
			104,
			102,
			104
		})]
		
		public FrameDropper(int dropEveryNthFrame, bool replaceNthWithPrevious)
		{
			this.initLogger();
			this.initVars(dropEveryNthFrame, replaceNthWithPrevious);
		}

		[LineNumberTable(new byte[]
		{
			5,
			102
		})]
		
		public FrameDropper()
		{
		}

		[Throws(new string[]
		{
			"edu.cmu.sphinx.util.props.PropertyException"
		})]
		[LineNumberTable(new byte[]
		{
			15,
			103,
			127,
			2
		})]
		
		public override void newProperties(PropertySheet ps)
		{
			base.newProperties(ps);
			this.initVars(ps.getInt("dropEveryNthFrame"), ps.getBoolean("replaceNthWithPrevious").booleanValue());
		}

		[LineNumberTable(new byte[]
		{
			32,
			102,
			103
		})]
		
		public override void initialize()
		{
			base.initialize();
			this.id = -1;
		}

		[Throws(new string[]
		{
			"edu.cmu.sphinx.frontend.DataProcessingException"
		})]
		[LineNumberTable(new byte[]
		{
			46,
			103,
			107,
			112,
			159,
			4,
			139,
			109,
			140,
			97,
			102,
			102,
			107,
			98,
			140,
			97,
			102,
			102,
			107,
			162,
			199,
			99,
			104,
			135,
			104,
			156,
			169,
			199
		})]
		
		public override Data getData()
		{
			object obj = this.readData();
			if ((Data)obj != null)
			{
				if (!(((Data)obj) is Signal))
				{
					int num = this.id;
					int num2 = this.dropEveryNthFrame;
					if (((num2 != -1) ? (num % num2) : 0) == this.dropEveryNthFrame - 1)
					{
						if (this.replaceNthWithPrevious)
						{
							if (((Data)obj) is FloatData)
							{
								FloatData floatData = (FloatData)this.lastFeature;
								obj = new FloatData(floatData.getValues(), floatData.getSampleRate(), floatData.getFirstSampleNumber());
							}
							else
							{
								DoubleData doubleData = (DoubleData)this.lastFeature;
								obj = new DoubleData(doubleData.getValues(), doubleData.getSampleRate(), doubleData.getFirstSampleNumber());
							}
						}
						else
						{
							obj = this.readData();
						}
					}
				}
				if (obj != null)
				{
					if (obj is DataEndSignal)
					{
						this.id = -1;
					}
					if (obj is FloatData)
					{
						object obj2 = obj;
						Data data;
						if (obj2 != null)
						{
							if ((data = (obj2 as Data)) == null)
							{
								throw new IncompatibleClassChangeError();
							}
						}
						else
						{
							data = null;
						}
						this.lastFeature = data;
					}
					else
					{
						this.lastFeature = null;
					}
				}
				else
				{
					this.lastFeature = null;
				}
			}
			object obj3 = obj;
			Data result;
			if (obj3 != null)
			{
				if ((result = (obj3 as Data)) == null)
				{
					throw new IncompatibleClassChangeError();
				}
			}
			else
			{
				result = null;
			}
			return result;
		}

		[S4Integer(new object[]
		{
			64,
			"Ledu/cmu/sphinx/util/props/S4Integer;",
			"defaultValue",
			-1
		})]
		public const string PROP_DROP_EVERY_NTH_FRAME = "dropEveryNthFrame";

		[S4Boolean(new object[]
		{
			64,
			"Ledu/cmu/sphinx/util/props/S4Boolean;",
			"defaultValue",
			false
		})]
		public const string PROP_REPLACE_NTH_WITH_PREVIOUS = "replaceNthWithPrevious";

		private Data lastFeature;

		private bool replaceNthWithPrevious;

		private int dropEveryNthFrame;

		private int id;
	}
}
