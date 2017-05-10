using System;

using edu.cmu.sphinx.frontend.endpoint;
using edu.cmu.sphinx.util.props;
using IKVM.Attributes;
using java.lang;
using java.util;

namespace edu.cmu.sphinx.frontend.feature
{
	public class BatchVarNorm : BaseDataProcessor
	{
		[LineNumberTable(new byte[]
		{
			11,
			103,
			107,
			103
		})]
		
		private void reset()
		{
			this.variances = null;
			this.cepstraList.clear();
			this.numberDataCepstra = 0;
		}

		[Throws(new string[]
		{
			"edu.cmu.sphinx.frontend.DataProcessingException"
		})]
		[LineNumberTable(new byte[]
		{
			52,
			162,
			108,
			102,
			107,
			110,
			108,
			104,
			143,
			107,
			255,
			29,
			71,
			103,
			59,
			166,
			141,
			114,
			109,
			130,
			173,
			134
		})]
		
		private int readUtterance()
		{
			Data data;
			double[] values;
			for (;;)
			{
				data = this.getPredecessor().getData();
				if (data != null)
				{
					if (data is DoubleData)
					{
						this.numberDataCepstra++;
						values = ((DoubleData)data).getValues();
						if (this.variances == null)
						{
							this.variances = new double[values.Length];
						}
						else if (this.variances.Length != values.Length)
						{
							break;
						}
						for (int i = 0; i < values.Length; i++)
						{
							double[] array = this.variances;
							int num = i;
							double[] array2 = array;
							array2[num] += values[i] * values[i];
						}
						this.cepstraList.add(data);
					}
					else
					{
						if (data is DataEndSignal || data is SpeechEndSignal)
						{
							goto IL_DE;
						}
						this.cepstraList.add(data);
					}
				}
				if (data == null)
				{
					goto IL_100;
				}
			}
			string text = new StringBuilder().append("Inconsistent cepstrum lengths: sums: ").append(this.variances.Length).append(", cepstrum: ").append(values.Length).toString();
			
			throw new Error(text);
			IL_DE:
			this.cepstraList.add(data);
			IL_100:
			return this.numberDataCepstra;
		}

		[LineNumberTable(new byte[]
		{
			93,
			108,
			62,
			198,
			127,
			4,
			104,
			108,
			105,
			60,
			200,
			101
		})]
		
		private void normalizeList()
		{
			for (int i = 0; i < this.variances.Length; i++)
			{
				this.variances[i] = java.lang.Math.sqrt((double)this.numberDataCepstra / this.variances[i]);
			}
			Iterator iterator = this.cepstraList.iterator();
			while (iterator.hasNext())
			{
				Data data = (Data)iterator.next();
				if (data is DoubleData)
				{
					double[] values = ((DoubleData)data).getValues();
					for (int j = 0; j < values.Length; j++)
					{
						double[] array = values;
						int num = j;
						double[] array2 = array;
						array2[num] *= this.variances[j];
					}
				}
			}
		}

		[LineNumberTable(new byte[]
		{
			159,
			179,
			104,
			102
		})]
		
		public BatchVarNorm()
		{
			this.initLogger();
		}

		[Throws(new string[]
		{
			"edu.cmu.sphinx.util.props.PropertyException"
		})]
		[LineNumberTable(new byte[]
		{
			159,
			188,
			103
		})]
		
		public override void newProperties(PropertySheet ps)
		{
			base.newProperties(ps);
		}

		[LineNumberTable(new byte[]
		{
			3,
			102,
			103,
			107
		})]
		
		public override void initialize()
		{
			base.initialize();
			this.variances = null;
			this.cepstraList = new LinkedList();
		}

		[Throws(new string[]
		{
			"edu.cmu.sphinx.frontend.DataProcessingException"
		})]
		[LineNumberTable(new byte[]
		{
			26,
			130,
			109,
			148,
			166,
			105,
			102,
			210
		})]
		
		public override Data getData()
		{
			Data result = null;
			if (!this.cepstraList.isEmpty())
			{
				result = (Data)this.cepstraList.remove(0);
			}
			else
			{
				this.reset();
				if (this.readUtterance() > 0)
				{
					this.normalizeList();
					result = (Data)this.cepstraList.remove(0);
				}
			}
			return result;
		}

		private double[] variances;

		
		private List cepstraList;

		private int numberDataCepstra;
	}
}
