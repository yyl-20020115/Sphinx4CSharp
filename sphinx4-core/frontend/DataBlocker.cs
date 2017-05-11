using edu.cmu.sphinx.util.props;
using ikvm.@internal;
using IKVM.Runtime;
using java.lang;
using java.util;

namespace edu.cmu.sphinx.frontend
{
	public class DataBlocker : BaseDataProcessor
	{
		public DataBlocker()
		{
			this.blockSizeSamples = int.MaxValue;
			this.sampleRate = -1;
			this.inBuffer = new LinkedList();
		}
	
		public DataBlocker(double blockSizeMs)
		{
			this.blockSizeSamples = int.MaxValue;
			this.sampleRate = -1;
			this.inBuffer = new LinkedList();
			this.initLogger();
			this.blockSizeMs = blockSizeMs;
		}
		
		public override void newProperties(PropertySheet propertySheet)
		{
			base.newProperties(propertySheet);
			this.blockSizeMs = propertySheet.getDouble("blockSizeMs");
		}
		public virtual double getBlockSizeMs()
		{
			return this.blockSizeMs;
		}
		
		public override Data getData()
		{
			while (this.curInBufferSize < this.blockSizeSamples || this.curInBufferSize == 0)
			{
				Data data = this.getPredecessor().getData();
				if (data is DataStartSignal)
				{
					this.sampleRate = ((DataStartSignal)data).getSampleRate();
					this.blockSizeSamples = (int)java.lang.Math.round((double)this.sampleRate * this.blockSizeMs / 1000.0);
					this.curInBufferSize = 0;
					this.curFirstSamplePos = 0;
					this.inBuffer.clear();
				}
				if (!(data is DoubleData))
				{
					return data;
				}
				DoubleData doubleData = (DoubleData)data;
				this.inBuffer.add(doubleData);
				this.curInBufferSize += doubleData.getValues().Length;
			}
			double[] array = new double[this.blockSizeSamples];
			int num = 0;
			long firstSampleNumber = ((DoubleData)this.inBuffer.get(0)).getFirstSampleNumber() + (long)this.curFirstSamplePos;
			while (!this.inBuffer.isEmpty())
			{
				DoubleData doubleData2 = (DoubleData)this.inBuffer.remove(0);
				double[] values = doubleData2.getValues();
				int num2 = java.lang.Math.min(this.blockSizeSamples - num, values.Length - this.curFirstSamplePos);
				ByteCodeHelper.arraycopy_primitive_8(values, this.curFirstSamplePos, array, num, num2);
				if (num2 < values.Length - this.curFirstSamplePos)
				{
					if (!DataBlocker.assertionsDisabled && !this.inBuffer.isEmpty())
					{
						
						throw new AssertionError();
					}
					this.curFirstSamplePos += num2;
					this.inBuffer.add(0, doubleData2);
					break;
				}
				else
				{
					num += num2;
					this.curFirstSamplePos = 0;
				}
			}
			this.curInBufferSize = ((!this.inBuffer.isEmpty()) ? (((DoubleData)this.inBuffer.get(0)).getValues().Length - this.curFirstSamplePos) : 0);
			return new DoubleData(array, this.sampleRate, firstSampleNumber);
		}

		[S4Double(new object[]
		{
			64,
			"Ledu/cmu/sphinx/util/props/S4Double;",
			"defaultValue",
			10.0
		})]
		public const string PROP_BLOCK_SIZE_MS = "blockSizeMs";

		private double blockSizeMs;

		private int blockSizeSamples;

		private int curFirstSamplePos;

		private int sampleRate;

		private LinkedList inBuffer;

		private int curInBufferSize;

		internal static bool assertionsDisabled = !ClassLiteral<DataBlocker>.Value.desiredAssertionStatus();
	}
}
