using System;

using edu.cmu.sphinx.frontend.endpoint;
using edu.cmu.sphinx.util.props;
using IKVM.Attributes;
using IKVM.Runtime;
using java.lang;
using java.text;
using java.util;

namespace edu.cmu.sphinx.frontend.feature
{
	public class LiveCMN : BaseDataProcessor
	{
		[LineNumberTable(new byte[]
		{
			63,
			130,
			127,
			4,
			104,
			130,
			172,
			100,
			99,
			108,
			199,
			106,
			104,
			60,
			168,
			142,
			165,
			100,
			129,
			108,
			104,
			59,
			168
		})]
		
		private void initMeansSums()
		{
			int num = -1;
			Iterator iterator = this.initialList.iterator();
			while (iterator.hasNext())
			{
				Data data = (Data)iterator.next();
				if (data is DoubleData)
				{
					double[] values = ((DoubleData)data).getValues();
					if (num < 0)
					{
						num = values.Length;
						this.sum = new double[num];
						this.numberFrame = 0;
					}
					if (values[0] >= (double)0f)
					{
						for (int i = 0; i < num; i++)
						{
							double[] array = this.sum;
							int num2 = i;
							double[] array2 = array;
							array2[num2] += values[i];
						}
						this.numberFrame++;
					}
				}
			}
			if (num < 0)
			{
				return;
			}
			this.currentMean = new double[num];
			for (int j = 0; j < num; j++)
			{
				this.currentMean[j] = this.sum[j] / (double)this.numberFrame;
			}
		}

		[LineNumberTable(new byte[]
		{
			160,
			79,
			104,
			129,
			140,
			107,
			255,
			36,
			69,
			106,
			103,
			52,
			166,
			206,
			103,
			52,
			198,
			142,
			135,
			110,
			124,
			10,
			200,
			146,
			134
		})]
		
		private void normalize(Data data)
		{
			if (!(data is DoubleData))
			{
				return;
			}
			double[] values = ((DoubleData)data).getValues();
			if (values.Length != this.sum.Length)
			{
				string text = new StringBuilder().append("Data length (").append(values.Length).append(") not equal sum array length (").append(this.sum.Length).append(')').toString();
				
				throw new Error(text);
			}
			if (values[0] >= (double)0f)
			{
				for (int i = 0; i < values.Length; i++)
				{
					double[] array = this.sum;
					int num = i;
					double[] array2 = array;
					array2[num] += values[i];
				}
				this.numberFrame++;
			}
			for (int i = 0; i < values.Length; i++)
			{
				double[] array3 = values;
				int num = i;
				double[] array2 = array3;
				array2[num] -= this.currentMean[i];
			}
			if (this.numberFrame > this.cmnShiftWindow)
			{
				StringBuilder stringBuilder = new StringBuilder();
				for (int j = 0; j < this.currentMean.Length; j++)
				{
					stringBuilder.append(this.formatter.format(this.currentMean[j]));
					stringBuilder.append(' ');
				}
				this.logger.info(stringBuilder.toString());
				this.updateMeanSumBuffers();
			}
		}

		[LineNumberTable(new byte[]
		{
			160,
			124,
			143,
			154,
			172,
			110,
			116,
			140
		})]
		
		private void updateMeanSumBuffers()
		{
			double num = (double)1f / (double)this.numberFrame;
			ByteCodeHelper.arraycopy_primitive_8(this.sum, 0, this.currentMean, 0, this.sum.Length);
			LiveCMN.multiplyArray(this.currentMean, num);
			if (this.numberFrame >= this.cmnShiftWindow)
			{
				LiveCMN.multiplyArray(this.sum, num * (double)this.cmnWindow);
				this.numberFrame = this.cmnWindow;
			}
		}

		[LineNumberTable(new byte[]
		{
			160,
			146,
			103,
			46,
			166
		})]
		private static void multiplyArray(double[] array, double num)
		{
			for (int i = 0; i < array.Length; i++)
			{
				int num2 = i;
				array[num2] *= num;
			}
		}

		[LineNumberTable(new byte[]
		{
			33,
			232,
			38,
			255,
			5,
			88,
			171,
			102,
			103,
			103,
			104
		})]
		
		public LiveCMN(double initialMean, int cmnWindow, int cmnShiftWindow, int initialCmnWindow)
		{
			DecimalFormat.__<clinit>();
			string text = "0.00;-0.00";
			DecimalFormatSymbols.__<clinit>();
			this.formatter = new DecimalFormat(text, new DecimalFormatSymbols(Locale.US));
			this.initialList = new LinkedList();
			this.initLogger();
			this.cmnWindow = cmnWindow;
			this.cmnShiftWindow = cmnShiftWindow;
			this.initialCmnWindow = initialCmnWindow;
		}

		[LineNumberTable(new byte[]
		{
			40,
			232,
			31,
			255,
			5,
			88,
			235,
			75
		})]
		
		public LiveCMN()
		{
			DecimalFormat.__<clinit>();
			string text = "0.00;-0.00";
			DecimalFormatSymbols.__<clinit>();
			this.formatter = new DecimalFormat(text, new DecimalFormatSymbols(Locale.US));
			this.initialList = new LinkedList();
		}

		[Throws(new string[]
		{
			"edu.cmu.sphinx.util.props.PropertyException"
		})]
		[LineNumberTable(new byte[]
		{
			46,
			103,
			113,
			113,
			113
		})]
		
		public override void newProperties(PropertySheet ps)
		{
			base.newProperties(ps);
			this.cmnWindow = ps.getInt("cmnWindow");
			this.cmnShiftWindow = ps.getInt("shiftWindow");
			this.initialCmnWindow = ps.getInt("initialCmnWindow");
		}

		[LineNumberTable(new byte[]
		{
			55,
			102
		})]
		
		public override void initialize()
		{
			base.initialize();
		}

		[Throws(new string[]
		{
			"edu.cmu.sphinx.frontend.DataProcessingException"
		})]
		[LineNumberTable(new byte[]
		{
			112,
			104,
			115,
			108,
			109,
			144,
			130,
			102,
			116,
			141,
			180,
			172,
			103
		})]
		
		public override Data getData()
		{
			Data data2;
			if (this.sum == null)
			{
				while (this.initialList.size() < this.initialCmnWindow)
				{
					Data data = this.getPredecessor().getData();
					this.initialList.add(data);
					if (data is SpeechEndSignal || data is DataEndSignal)
					{
						break;
					}
				}
				this.initMeansSums();
				data2 = (Data)this.initialList.remove(0);
			}
			else if (!this.initialList.isEmpty())
			{
				data2 = (Data)this.initialList.remove(0);
			}
			else
			{
				data2 = this.getPredecessor().getData();
			}
			this.normalize(data2);
			return data2;
		}

		private DecimalFormat formatter;

		[S4Integer(new object[]
		{
			64,
			"Ledu/cmu/sphinx/util/props/S4Integer;",
			"defaultValue",
			200
		})]
		public const string PROP_INITIAL_CMN_WINDOW = "initialCmnWindow";

		private int initialCmnWindow;

		[S4Integer(new object[]
		{
			64,
			"Ledu/cmu/sphinx/util/props/S4Integer;",
			"defaultValue",
			300
		})]
		public const string PROP_CMN_WINDOW = "cmnWindow";

		private int cmnWindow;

		[S4Integer(new object[]
		{
			64,
			"Ledu/cmu/sphinx/util/props/S4Integer;",
			"defaultValue",
			400
		})]
		public const string PROP_CMN_SHIFT_WINDOW = "shiftWindow";

		private int cmnShiftWindow;

		private double[] currentMean;

		private double[] sum;

		private int numberFrame;

		
		internal List initialList;
	}
}
