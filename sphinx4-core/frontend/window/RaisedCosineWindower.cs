using edu.cmu.sphinx.frontend.endpoint;
using edu.cmu.sphinx.frontend.util;
using edu.cmu.sphinx.util.props;
using ikvm.@internal;
using IKVM.Runtime;
using java.lang;
using java.util;

namespace edu.cmu.sphinx.frontend.window
{
	public class RaisedCosineWindower : BaseDataProcessor
	{		
		private void createWindow(int num)
		{
			if (this.cosineWindow != null && num == this.sampleRate)
			{
				return;
			}
			this.sampleRate = num;
			int samplesPerWindow = DataUtil.getSamplesPerWindow(num, this.windowSizeInMs);
			this.cosineWindow = new double[samplesPerWindow];
			this.windowShift = DataUtil.getSamplesPerShift(num, this.windowShiftInMs);
			if (this.cosineWindow.Length > 1)
			{
				double num2 = (double)1f - this.alpha;
				for (int i = 0; i < this.cosineWindow.Length; i++)
				{
					this.cosineWindow[i] = num2 - this.alpha * Math.cos(6.2831853071795862 * (double)i / ((double)this.cosineWindow.Length - (double)1f));
				}
			}
			this.overflowBuffer = new DoubleBuffer(samplesPerWindow);
		}
		
		private void process(DoubleData doubleData)
		{
			double[] values = doubleData.getValues();
			int i = this.overflowBuffer.getOccupancy() + values.Length;
			LinkedList linkedList = new LinkedList();
			linkedList.add(doubleData);
			Data data = null;
			while (i < this.cosineWindow.Length)
			{
				Data data2 = this.getPredecessor().getData();
				if (data2 is DoubleData)
				{
					linkedList.add((DoubleData)data2);
					i += ((DoubleData)data2).getValues().Length;
				}
				else
				{
					if (data2 is DataEndSignal || data2 is SpeechEndSignal)
					{
						data = data2;
						break;
					}
					this.outputQueue.add(data2);
				}
			}
			double[] array = values;
			int num;
			if (i != values.Length)
			{
				array = new double[i];
				ByteCodeHelper.arraycopy_primitive_8(this.overflowBuffer.getBuffer(), 0, array, 0, this.overflowBuffer.getOccupancy());
				num = this.overflowBuffer.getOccupancy();
				Iterator iterator = linkedList.iterator();
				while (iterator.hasNext())
				{
					DoubleData doubleData2 = (DoubleData)iterator.next();
					double[] values2 = doubleData2.getValues();
					ByteCodeHelper.arraycopy_primitive_8(values2, 0, array, num, values2.Length);
					num += values2.Length;
				}
			}
			num = this.applyRaisedCosineWindow(array, i);
			this.overflowBuffer.reset();
			if (i - num > 0)
			{
				this.overflowBuffer.append(array, num, i - num);
			}
			if (data != null)
			{
				this.processUtteranceEnd();
				this.outputQueue.add(data);
			}
		}
		
		private void processUtteranceEnd()
		{
			if (this.overflowBuffer.getOccupancy() > 0)
			{
				this.overflowBuffer.padWindow(this.cosineWindow.Length);
				this.applyRaisedCosineWindow(this.overflowBuffer.getBuffer(), this.cosineWindow.Length);
				this.overflowBuffer.reset();
			}
		}
		
		private int applyRaisedCosineWindow(double[] array, int num)
		{
			int num2;
			if (num < this.cosineWindow.Length)
			{
				double[] array2 = new double[this.cosineWindow.Length];
				ByteCodeHelper.arraycopy_primitive_8(array, 0, array2, 0, num);
				array = array2;
				num2 = 1;
			}
			else
			{
				num2 = RaisedCosineWindower.getWindowCount(num, this.cosineWindow.Length, this.windowShift);
			}
			int num3 = num2;
			int num4 = this.cosineWindow.Length;
			int[] array3 = new int[2];
			int num5 = num4;
			array3[1] = num5;
			num5 = num3;
			array3[0] = num5;
			double[][] array4 = (double[][])ByteCodeHelper.multianewarray(typeof(double[][]).TypeHandle, array3);
			int num6 = 0;
			for (int i = 0; i < num2; i++)
			{
				double[] array5 = array4[i];
				int j = 0;
				int num7 = num6;
				while (j < array5.Length)
				{
					array5[j] = array[num7] * this.cosineWindow[j];
					num7++;
					j++;
				}
				this.outputQueue.add(new DoubleData(array5, this.sampleRate, this.currentFirstSampleNumber));
				this.currentFirstSampleNumber += (long)this.windowShift;
				num6 += this.windowShift;
			}
			return num6;
		}

		private static int getWindowCount(int num, int num2, int num3)
		{
			if (num < num2)
			{
				return 0;
			}
			int num4 = 1;
			int num5 = num2;
			while (num5 + num3 <= num)
			{
				num4++;
				num5 += num3;
			}
			return num4;
		}
		
		public RaisedCosineWindower(double alpha, float windowSizeInMs, float windowShiftInMs)
		{
			this.initLogger();
			this.alpha = alpha;
			this.windowSizeInMs = windowSizeInMs;
			this.windowShiftInMs = windowShiftInMs;
		}
		
		public RaisedCosineWindower()
		{
		}
		
		public override void newProperties(PropertySheet ps)
		{
			base.newProperties(ps);
			this.alpha = ps.getDouble("alpha");
			this.windowSizeInMs = ps.getFloat("windowSizeInMs");
			this.windowShiftInMs = ps.getFloat("windowShiftInMs");
		}
		
		public override void initialize()
		{
			base.initialize();
			this.outputQueue = new LinkedList();
		}
		
		public override Data getData()
		{
			Data data;
			if (this.outputQueue.isEmpty())
			{
				data = this.getPredecessor().getData();
				if (data != null)
				{
					if (data is DoubleData)
					{
						DoubleData doubleData = (DoubleData)data;
						if (this.currentFirstSampleNumber == -1L)
						{
							this.currentFirstSampleNumber = doubleData.getFirstSampleNumber();
						}
						this.createWindow(doubleData.getSampleRate());
						this.process(doubleData);
					}
					else
					{
						if (data is DataStartSignal)
						{
							DataStartSignal dataStartSignal = (DataStartSignal)data;
							this.createWindow(dataStartSignal.getSampleRate());
							Map props = dataStartSignal.getProps();
							props.put("windowSize", Integer.valueOf(this.windowShift));
							props.put("windowShift", Integer.valueOf(this.cosineWindow.Length));
							this.currentFirstSampleNumber = -1L;
						}
						else if (data is SpeechStartSignal)
						{
							this.currentFirstSampleNumber = -1L;
						}
						else if (data is DataEndSignal || data is SpeechEndSignal)
						{
							this.processUtteranceEnd();
						}
						this.outputQueue.add(data);
					}
				}
			}
			if (this.outputQueue.isEmpty())
			{
				return null;
			}
			data = (Data)this.outputQueue.remove(0);
			if (data is DoubleData && !RaisedCosineWindower.assertionsDisabled && ((DoubleData)data).getValues().Length != this.cosineWindow.Length)
			{
				
				throw new AssertionError();
			}
			return data;
		}
		
		public virtual float getWindowShiftInMs()
		{
			if (this.windowShiftInMs == 0f)
			{
				string text = new StringBuilder().append(this).append(" was not initialized yet!").toString();
				
				throw new RuntimeException(text);
			}
			return this.windowShiftInMs;
		}
		public virtual int getSampleRate()
		{
			return this.sampleRate;
		}
		
		public virtual long roundToFrames(long samples)
		{
			int samplesPerWindow = DataUtil.getSamplesPerWindow(this.sampleRate, this.windowSizeInMs);
			int samplesPerShift = DataUtil.getSamplesPerShift(this.sampleRate, this.windowShiftInMs);
			long num = (long)samplesPerShift;
			long num2 = (num != -1L) ? (samples / num) : (-samples);
			int num3 = (int)num2;
			for (;;)
			{
				long num4 = samples - (long)(samplesPerShift * num3);
				if (num4 > (long)samplesPerWindow)
				{
					break;
				}
				num3 --;
			}
			return (long)(samplesPerShift * (num3 + 1) + samplesPerWindow);
		}

		[S4Double(new object[]
		{
			64,
			"Ledu/cmu/sphinx/util/props/S4Double;",
			"defaultValue",
			25.625
		})]
		public const string PROP_WINDOW_SIZE_MS = "windowSizeInMs";

		private float windowSizeInMs;

		[S4Double(new object[]
		{
			64,
			"Ledu/cmu/sphinx/util/props/S4Double;",
			"defaultValue",
			10.0
		})]
		public const string PROP_WINDOW_SHIFT_MS = "windowShiftInMs";

		private float windowShiftInMs;

		[S4Double(new object[]
		{
			64,
			"Ledu/cmu/sphinx/util/props/S4Double;",
			"defaultValue",
			0.46
		})]
		public const string PROP_ALPHA = "alpha";

		private double alpha;

		public const string WINDOW_SHIFT_SAMPLES = "windowSize";

		public const string WINDOW_SIZE_SAMPLES = "windowShift";

		private double[] cosineWindow;

		private int windowShift;

		
		private List outputQueue;

		private DoubleBuffer overflowBuffer;

		private long currentFirstSampleNumber;

		private int sampleRate;

		internal static bool assertionsDisabled = !ClassLiteral<RaisedCosineWindower>.Value.desiredAssertionStatus();
	}
}
