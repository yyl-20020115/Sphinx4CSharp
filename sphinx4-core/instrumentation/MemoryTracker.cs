using edu.cmu.sphinx.decoder;
using edu.cmu.sphinx.recognizer;
using edu.cmu.sphinx.result;
using edu.cmu.sphinx.util.props;
using java.lang;
using java.text;
using java.util;

namespace edu.cmu.sphinx.instrumentation
{
	public class MemoryTracker : ConfigurableAdapter, ResultListener, EventListener, Configurable, StateListener, Monitor
	{
		private void initRecognizer(Recognizer recognizer)
		{
			if (this.recognizer == null)
			{
				this.recognizer = recognizer;
				this.recognizer.addResultListener(this);
				this.recognizer.addStateListener(this);
			}
			else if (this.recognizer != recognizer)
			{
				this.recognizer.removeResultListener(this);
				this.recognizer.removeStateListener(this);
				this.recognizer = recognizer;
				this.recognizer.addResultListener(this);
				this.recognizer.addStateListener(this);
			}
		}
	
		private void calculateMemoryUsage(bool flag)
		{
			float num = (float)Runtime.getRuntime().totalMemory() / 1048576f;
			float num2 = (float)Runtime.getRuntime().freeMemory() / 1048576f;
			float num3 = num - num2;
			if (num3 > this.maxMemoryUsed)
			{
				this.maxMemoryUsed = num3;
			}
			this.numMemoryStats++;
			this.avgMemoryUsed = (this.avgMemoryUsed * (float)(this.numMemoryStats - 1) + num3) / (float)this.numMemoryStats;
			if (flag)
			{
				this.logger.info(new StringBuilder().append("   Mem  Total: ").append(MemoryTracker.memFormat.format((double)num)).append("  Free: ").append(MemoryTracker.memFormat.format((double)num2)).toString());
				this.logger.info(new StringBuilder().append("   Used: This: ").append(MemoryTracker.memFormat.format((double)num3)).append("  Avg: ").append(MemoryTracker.memFormat.format((double)this.avgMemoryUsed)).append("  Max: ").append(MemoryTracker.memFormat.format((double)this.maxMemoryUsed)).toString());
			}
		}
	
		public MemoryTracker(Recognizer recognizer, bool showSummary, bool showDetails)
		{
			this.initRecognizer(recognizer);
			this.initLogger();
			this.showSummary = showSummary;
			this.showDetails = showDetails;
		}

		public MemoryTracker()
		{
		}
	
		public override void newProperties(PropertySheet ps)
		{
			base.newProperties(ps);
			Recognizer recognizer = (Recognizer)ps.getComponent("recognizer");
			this.initRecognizer(recognizer);
			this.showSummary = ps.getBoolean("showSummary").booleanValue();
			this.showDetails = ps.getBoolean("showDetails").booleanValue();
		}

		public override string getName()
		{
			return this.name;
		}

		public virtual void newResult(Result result)
		{
			if (result.isFinal())
			{
				this.calculateMemoryUsage(this.showDetails);
			}
		}
	
		public virtual void statusChanged(Recognizer.State status)
		{
			if (status == Recognizer.State.__DEALLOCATED)
			{
				this.calculateMemoryUsage(this.showSummary);
			}
		}

		[S4Component(new object[]
		{
			64,
			"Ledu/cmu/sphinx/util/props/S4Component;",
			"type",
			new object[]
			{
				99,
				"Ledu/cmu/sphinx/recognizer/Recognizer, sphinx4-core-5prealpha-SNAPSHOT, Version=0/0/0/0, Culture=neutral, PublicKeyToken=null;"
			}
		})]
		public const string PROP_RECOGNIZER = "recognizer";

		[S4Boolean(new object[]
		{
			64,
			"Ledu/cmu/sphinx/util/props/S4Boolean;",
			"defaultValue",
			true
		})]
		public const string PROP_SHOW_SUMMARY = "showSummary";

		[S4Boolean(new object[]
		{
			64,
			"Ledu/cmu/sphinx/util/props/S4Boolean;",
			"defaultValue",
			true
		})]
		public const string PROP_SHOW_DETAILS = "showDetails";

		private static DecimalFormat memFormat = new DecimalFormat("0.00 Mb");

		private string name;

		private Recognizer recognizer;

		private bool showSummary;

		private bool showDetails;

		private float maxMemoryUsed;

		private int numMemoryStats;

		private float avgMemoryUsed;
	}
}
