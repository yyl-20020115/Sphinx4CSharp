using edu.cmu.sphinx.util.props;
using java.lang;

namespace edu.cmu.sphinx.frontend.feature
{
	public class FrameDropper : BaseDataProcessor
	{		
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
	
		private Data readData()
		{
			Data data = this.getPredecessor().getData();
			if (data != null)
			{
				this.id++;
			}
			return data;
		}
	
		public FrameDropper(int dropEveryNthFrame, bool replaceNthWithPrevious)
		{
			this.initLogger();
			this.initVars(dropEveryNthFrame, replaceNthWithPrevious);
		}
	
		public FrameDropper()
		{
		}
		
		public override void newProperties(PropertySheet ps)
		{
			base.newProperties(ps);
			this.initVars(ps.getInt("dropEveryNthFrame"), ps.getBoolean("replaceNthWithPrevious").booleanValue());
		}

		public override void initialize()
		{
			base.initialize();
			this.id = -1;
		}
		
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
