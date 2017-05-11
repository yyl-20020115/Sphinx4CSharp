using edu.cmu.sphinx.frontend.endpoint;
using edu.cmu.sphinx.util.props;
using java.util;

namespace edu.cmu.sphinx.frontend.feature
{
	public class BatchAGC : BaseDataProcessor
	{		
		private int readUtterance()
		{
			int num = 0;
			Data data;
			for (;;)
			{
				data = this.getPredecessor().getData();
				if (data == null)
				{
					break;
				}
				if (data is DataEndSignal || data is SpeechEndSignal)
				{
					goto IL_28;
				}
				if (data is DoubleData)
				{
					this.cepstraList.add(data);
					double num2 = ((DoubleData)data).getValues()[0];
					if (this.agc < num2)
					{
						this.agc = num2;
					}
				}
				else
				{
					this.cepstraList.add(data);
				}
				num++;
			}
			return num;
			IL_28:
			this.cepstraList.add(data);
			return num;
		}
		
		private void normalizeList()
		{
			Iterator iterator = this.cepstraList.iterator();
			while (iterator.hasNext())
			{
				Data data = (Data)iterator.next();
				if (data is DoubleData)
				{
					double[] values = ((DoubleData)data).getValues();
					int num = 0;
					double[] array = values;
					array[num] -= this.agc;
				}
			}
		}
		
		public BatchAGC()
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
				this.agc = (double)0f;
				this.cepstraList.clear();
				if (this.readUtterance() > 0)
				{
					this.normalizeList();
					result = (Data)this.cepstraList.remove(0);
				}
			}
			return result;
		}
		
		private List cepstraList;

		private double agc;
	}
}
