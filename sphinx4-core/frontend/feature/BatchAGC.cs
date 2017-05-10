using System;

using edu.cmu.sphinx.frontend.endpoint;
using edu.cmu.sphinx.util.props;
using IKVM.Attributes;
using java.util;

namespace edu.cmu.sphinx.frontend.feature
{
	public class BatchAGC : BaseDataProcessor
	{
		[Throws(new string[]
		{
			"edu.cmu.sphinx.frontend.DataProcessingException"
		})]
		[LineNumberTable(new byte[]
		{
			33,
			98,
			162,
			108,
			99,
			101,
			112,
			109,
			98,
			104,
			109,
			110,
			105,
			104,
			98,
			141,
			169
		})]
		
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

		[LineNumberTable(new byte[]
		{
			59,
			127,
			1,
			104,
			156,
			98
		})]
		
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

		[LineNumberTable(new byte[]
		{
			159,
			171,
			104,
			102
		})]
		
		public BatchAGC()
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
			180,
			103
		})]
		
		public override void newProperties(PropertySheet ps)
		{
			base.newProperties(ps);
		}

		[LineNumberTable(new byte[]
		{
			159,
			186,
			102,
			107
		})]
		
		public override void initialize()
		{
			base.initialize();
			this.cepstraList = new LinkedList();
		}

		[Throws(new string[]
		{
			"edu.cmu.sphinx.frontend.DataProcessingException"
		})]
		[LineNumberTable(new byte[]
		{
			7,
			130,
			109,
			148,
			108,
			139,
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
