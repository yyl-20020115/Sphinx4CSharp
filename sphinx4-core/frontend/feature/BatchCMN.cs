using edu.cmu.sphinx.frontend.endpoint;
using edu.cmu.sphinx.util.props;
using java.lang;
using java.text;
using java.util;

namespace edu.cmu.sphinx.frontend.feature
{
	public class BatchCMN : BaseDataProcessor
	{		
		private void reset()
		{
			this.sums = null;
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
						values = ((DoubleData)data).getValues();
						if (this.sums == null)
						{
							this.sums = new double[values.Length];
						}
						else if (this.sums.Length != values.Length)
						{
							break;
						}
						if (values[0] >= (double)0f)
						{
							for (int i = 0; i < values.Length; i++)
							{
								double[] array = this.sums;
								int num = i;
								double[] array2 = array;
								array2[num] += values[i];
							}
							this.numberDataCepstra++;
						}
						this.cepstraList.add(data);
					}
					else
					{
						if (data is DataEndSignal || data is SpeechEndSignal)
						{
							goto IL_E4;
						}
						this.cepstraList.add(data);
					}
				}
				if (data == null)
				{
					goto IL_106;
				}
			}
			string text = new StringBuilder().append("Inconsistent cepstrum lengths: sums: ").append(this.sums.Length).append(", cepstrum: ").append(values.Length).toString();
			
			throw new Error(text);
			IL_E4:
			this.cepstraList.add(data);
			IL_106:
			return this.numberDataCepstra;
		}
		
		private void normalizeList()
		{
			StringBuilder stringBuilder = new StringBuilder();
			for (int i = 0; i < this.sums.Length; i++)
			{
				double[] array = this.sums;
				int num = i;
				double[] array2 = array;
				array2[num] /= (double)this.numberDataCepstra;
				stringBuilder.append(this.formatter.format(this.sums[i]));
				stringBuilder.append(' ');
			}
			this.logger.info(stringBuilder.toString());
			Iterator iterator = this.cepstraList.iterator();
			while (iterator.hasNext())
			{
				Data data = (Data)iterator.next();
				if (data is DoubleData)
				{
					double[] values = ((DoubleData)data).getValues();
					for (int j = 0; j < values.Length; j++)
					{
						double[] array3 = values;
						int num = j;
						double[] array2 = array3;
						array2[num] -= this.sums[j];
					}
				}
			}
		}
		
		public BatchCMN()
		{
			string text = "0.00;-0.00";
			this.formatter = new DecimalFormat(text, new DecimalFormatSymbols(Locale.US));
			this.initLogger();
		}
		
		public override void newProperties(PropertySheet ps)
		{
			base.newProperties(ps);
		}

		public override void initialize()
		{
			base.initialize();
			this.sums = null;
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

		private double[] sums;
		
		private List cepstraList;

		private int numberDataCepstra;

		private DecimalFormat formatter;
	}
}
