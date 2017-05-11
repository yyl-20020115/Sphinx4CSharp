using edu.cmu.sphinx.frontend.endpoint;
using edu.cmu.sphinx.util.props;
using java.lang;
using java.util;

namespace edu.cmu.sphinx.frontend.feature
{
	public class BatchVarNorm : BaseDataProcessor
	{		
		private void reset()
		{
			this.variances = null;
			this.cepstraList.clear();
			this.numberDataCepstra = 0;
		}

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
		
		public BatchVarNorm()
		{
			this.initLogger();
		}
		
		public override void newProperties(PropertySheet ps)
		{
			base.newProperties(ps);
		}
		
		public override void initialize()
		{
			base.initialize();
			this.variances = null;
			this.cepstraList = new LinkedList();
		}
		
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
