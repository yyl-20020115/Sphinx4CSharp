using java.lang;
using java.util;

namespace edu.cmu.sphinx.util
{
	public class StatisticsVariable : Object
	{
		public static StatisticsVariable getStatisticsVariable(string statName)
		{
			StatisticsVariable statisticsVariable = (StatisticsVariable)StatisticsVariable.pool.get(statName);
			if (statisticsVariable == null)
			{
				statisticsVariable = new StatisticsVariable(statName);
				StatisticsVariable.pool.put(statName, statisticsVariable);
			}
			return statisticsVariable;
		}

		public static StatisticsVariable getStatisticsVariable(string instanceName, string statName)
		{
			return StatisticsVariable.getStatisticsVariable(new StringBuilder().append(instanceName).append('.').append(statName).toString());
		}

		private StatisticsVariable(string text)
		{
			this.name = text;
			this.value = (double)0f;
		}

		public virtual void dump()
		{
			if (this.isEnabled())
			{
				java.lang.System.@out.println(new StringBuilder().append(this.name).append(' ').append(this.value).toString());
			}
		}

		public virtual void reset()
		{
			this.setValue((double)0f);
		}

		public virtual void setValue(double value)
		{
			this.value = value;
		}

		public virtual bool isEnabled()
		{
			return this.enabled;
		}

		public static void dumpAll()
		{
			java.lang.System.@out.println(" ========= statistics  =======");
			Iterator iterator = StatisticsVariable.pool.values().iterator();
			while (iterator.hasNext())
			{
				StatisticsVariable statisticsVariable = (StatisticsVariable)iterator.next();
				statisticsVariable.dump();
			}
		}

		public static void resetAll()
		{
			Iterator iterator = StatisticsVariable.pool.values().iterator();
			while (iterator.hasNext())
			{
				StatisticsVariable statisticsVariable = (StatisticsVariable)iterator.next();
				statisticsVariable.reset();
			}
		}

		public virtual string getName()
		{
			return this.name;
		}

		public virtual double getValue()
		{
			return this.value;
		}

		public virtual void setEnabled(bool enabled)
		{
			this.enabled = enabled;
		}

		public static void main(string[] args)
		{
			StatisticsVariable statisticsVariable = StatisticsVariable.getStatisticsVariable("main", "loops");
			StatisticsVariable statisticsVariable2 = StatisticsVariable.getStatisticsVariable("main", "sum");
			StatisticsVariable statisticsVariable3 = StatisticsVariable.getStatisticsVariable("body", "foot");
			StatisticsVariable statisticsVariable4 = StatisticsVariable.getStatisticsVariable("body", "leg");
			StatisticsVariable statisticsVariable5 = StatisticsVariable.getStatisticsVariable("body", "finger");
			statisticsVariable3.setValue(2.0);
			statisticsVariable4.setValue(2.0);
			statisticsVariable5.setValue(10.0);
			StatisticsVariable.dumpAll();
			StatisticsVariable.dumpAll();
			for (int i = 0; i < 1000; i++)
			{
				StatisticsVariable statisticsVariable6 = statisticsVariable;
				statisticsVariable6.value += (double)1f;
				StatisticsVariable statisticsVariable7 = statisticsVariable2;
				statisticsVariable7.value += (double)i;
			}
			StatisticsVariable.dumpAll();
			StatisticsVariable statisticsVariable8 = StatisticsVariable.getStatisticsVariable("main", "loops");
			StatisticsVariable statisticsVariable9 = StatisticsVariable.getStatisticsVariable("main", "sum");
			for (int j = 0; j < 1000; j++)
			{
				StatisticsVariable statisticsVariable10 = statisticsVariable8;
				statisticsVariable10.value += (double)1f;
				StatisticsVariable statisticsVariable11 = statisticsVariable9;
				statisticsVariable11.value += (double)j;
			}
			StatisticsVariable.dumpAll();
			StatisticsVariable.resetAll();
			StatisticsVariable.dumpAll();
		}

		private static Map pool = new HashMap();

		public double value;

		private string name;

		private bool enabled;
	}
}
