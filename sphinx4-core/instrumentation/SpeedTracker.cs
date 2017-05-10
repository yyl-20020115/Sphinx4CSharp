using System;

using edu.cmu.sphinx.decoder;
using edu.cmu.sphinx.frontend;
using edu.cmu.sphinx.frontend.endpoint;
using edu.cmu.sphinx.recognizer;
using edu.cmu.sphinx.result;
using edu.cmu.sphinx.util;
using edu.cmu.sphinx.util.props;
using IKVM.Attributes;
using java.lang;
using java.text;
using java.util;

namespace edu.cmu.sphinx.instrumentation
{
	public class SpeedTracker : ConfigurableAdapter, ResultListener, EventListener, Configurable, Resetable, StateListener, SignalListener, Monitor
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
		
		private void initFrontEnd(FrontEnd frontEnd)
		{
			if (this.frontEnd == null)
			{
				this.frontEnd = frontEnd;
				this.frontEnd.addSignalListener(this);
			}
			else if (this.frontEnd != frontEnd)
			{
				this.frontEnd.removeSignalListener(this);
				this.frontEnd = frontEnd;
				this.frontEnd.addSignalListener(this);
			}
		}

		
		
		private long getTime()
		{
			return java.lang.System.currentTimeMillis();
		}		
		protected internal virtual void showAudioUsage()
		{
			this.logger.info(new StringBuilder().append("   This  Time Audio: ").append(SpeedTracker.timeFormat.format((double)this.audioTime)).append("s  Proc: ").append(SpeedTracker.timeFormat.format((double)this.processingTime)).append("s  Speed: ").append(SpeedTracker.timeFormat.format((double)this.getSpeed())).append(" X real time").toString());
			this.showAudioSummary();
		}

		public virtual float getSpeed()
		{
			if (this.processingTime == 0f || this.audioTime == 0f)
			{
				return 0f;
			}
			return this.processingTime / this.audioTime;
		}
		
		protected internal virtual void showAudioSummary()
		{
			this.logger.info(new StringBuilder().append("   Total Time Audio: ").append(SpeedTracker.timeFormat.format((double)this.totalAudioTime)).append("s  Proc: ").append(SpeedTracker.timeFormat.format((double)this.totalProcessingTime)).append("s ").append(SpeedTracker.timeFormat.format((double)this.getCumulativeSpeed())).append(" X real time").toString());
			if (this.showResponseTime)
			{
				float num = (float)this.totalResponseTime / (float)(this.numUtteranceStart * 1000);
				this.logger.info(new StringBuilder().append("   Response Time:  Avg: ").append(num).append('s').append("  Max: ").append((float)this.maxResponseTime / 1000f).append("s  Min: ").append((float)this.minResponseTime / 1000f).append('s').toString());
			}
		}

		public virtual float getCumulativeSpeed()
		{
			if (this.totalProcessingTime == 0f || this.totalAudioTime == 0f)
			{
				return 0f;
			}
			return this.totalProcessingTime / this.totalAudioTime;
		}
		
		public SpeedTracker(Recognizer recognizer, FrontEnd frontEnd, bool showSummary, bool showDetails, bool showResponseTime, bool showTimers)
		{
			this.maxResponseTime = long.MinValue;
			this.minResponseTime = long.MaxValue;
			this.initLogger();
			this.initRecognizer(recognizer);
			this.initFrontEnd(frontEnd);
			this.showSummary = showSummary;
			this.showDetails = showDetails;
			this.showResponseTime = showResponseTime;
			this.showTimers = showTimers;
		}
		
		public SpeedTracker()
		{
			this.maxResponseTime = long.MinValue;
			this.minResponseTime = long.MaxValue;
		}
		
		public override void newProperties(PropertySheet ps)
		{
			base.newProperties(ps);
			this.initRecognizer((Recognizer)ps.getComponent("recognizer"));
			this.initFrontEnd((FrontEnd)ps.getComponent("frontend"));
			this.showSummary = ps.getBoolean("showSummary").booleanValue();
			this.showDetails = ps.getBoolean("showDetails").booleanValue();
			this.showResponseTime = ps.getBoolean("showResponseTime").booleanValue();
			this.showTimers = ps.getBoolean("showTimers").booleanValue();
		}

		public override string getName()
		{
			return this.name;
		}
		
		public virtual void newResult(Result result)
		{
			if (result.isFinal())
			{
				this.processingTime = (float)(this.getTime() - this.startTime) / 1000f;
				this.totalAudioTime += this.audioTime;
				this.totalProcessingTime += this.processingTime;
				if (this.showDetails)
				{
					this.showAudioUsage();
				}
			}
		}

		public virtual void reset()
		{
			this.totalProcessingTime = 0f;
			this.totalAudioTime = 0f;
			this.numUtteranceStart = 0;
		}
		
		public virtual void signalOccurred(Signal signal)
		{
			if (signal is SpeechStartSignal || signal is DataStartSignal)
			{
				this.startTime = this.getTime();
				this.audioStartTime = signal.getTime();
				long num = this.startTime - this.audioStartTime;
				this.totalResponseTime += num;
				if (num > this.maxResponseTime)
				{
					this.maxResponseTime = num;
				}
				if (num < this.minResponseTime)
				{
					this.minResponseTime = num;
				}
				this.numUtteranceStart++;
			}
			else if (signal is SpeechEndSignal)
			{
				this.audioTime = (float)(signal.getTime() - this.audioStartTime) / 1000f;
			}
			else if (signal is DataEndSignal)
			{
				this.audioTime = (float)((DataEndSignal)signal).getDuration() / 1000f;
			}
		}		
		public virtual void statusChanged(Recognizer.State status)
		{
			if (status == Recognizer.State.__ALLOCATED && this.showTimers)
			{
				TimerPool.dumpAll(this.logger);
			}
			if (status == Recognizer.State.__DEALLOCATING && this.showTimers)
			{
				TimerPool.dumpAll(this.logger);
			}
			if (status == Recognizer.State.__DEALLOCATED && this.showSummary)
			{
				this.showAudioSummary();
			}
		}

		
		static SpeedTracker()
		{
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

		[S4Component(new object[]
		{
			64,
			"Ledu/cmu/sphinx/util/props/S4Component;",
			"type",
			new object[]
			{
				99,
				"Ledu/cmu/sphinx/frontend/FrontEnd, sphinx4-core-5prealpha-SNAPSHOT, Version=0/0/0/0, Culture=neutral, PublicKeyToken=null;"
			}
		})]
		public const string PROP_FRONTEND = "frontend";

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

		[S4Boolean(new object[]
		{
			64,
			"Ledu/cmu/sphinx/util/props/S4Boolean;",
			"defaultValue",
			false
		})]
		public const string PROP_SHOW_RESPONSE_TIME = "showResponseTime";

		[S4Boolean(new object[]
		{
			64,
			"Ledu/cmu/sphinx/util/props/S4Boolean;",
			"defaultValue",
			false
		})]
		public const string PROP_SHOW_TIMERS = "showTimers";

		
		private static DecimalFormat timeFormat = new DecimalFormat("0.00");

		private string name;

		private Recognizer recognizer;

		private FrontEnd frontEnd;

		private bool showSummary;

		private bool showDetails;

		private bool showTimers;

		private long startTime;

		private long audioStartTime;

		private float audioTime;

		private float processingTime;

		private float totalAudioTime;

		private float totalProcessingTime;

		private bool showResponseTime;

		private int numUtteranceStart;

		private long maxResponseTime;

		private long minResponseTime;

		private long totalResponseTime;
	}
}
