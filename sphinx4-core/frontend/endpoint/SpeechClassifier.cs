using edu.cmu.sphinx.util;
using edu.cmu.sphinx.util.props;
using ikvm.@internal;
using java.lang;
using java.util.logging;

namespace edu.cmu.sphinx.frontend.endpoint
{
	public class SpeechClassifier : AbstractVoiceActivityDetector
	{

		public override void initialize()
		{
			base.initialize();
			this.reset();
		}

		protected internal virtual void reset()
		{
			this.level = (double)0f;
			this.background = 300.0;
			this.resetStats();
		}

		private void resetStats()
		{
			this.backgroundFrames = 1L;
			this.speechFrames = 1L;
			this.totalSpeechLevel = (double)0f;
			this.totalBackgroundLevel = (double)0f;
		}

		public static double logRootMeanSquare(double[] samples)
		{
			if (!SpeechClassifier.assertionsDisabled && samples.Length <= 0)
			{
				
				throw new AssertionError();
			}
			double num = (double)0f;
			int num2 = samples.Length;
			for (int i = 0; i < num2; i++)
			{
				double num3 = samples[i];
				num += num3 * num3;
			}
			double num4 = java.lang.Math.sqrt(num / (double)samples.Length);
			num4 = java.lang.Math.max(num4, (double)1f);
			return (double)(LogMath.log10((float)num4) * 20f);
		}

		private void collectStats(bool flag)
		{
			if (flag)
			{
				this.totalSpeechLevel += this.level;
				this.speechFrames += 1L;
			}
			else
			{
				this.totalBackgroundLevel += this.background;
				this.backgroundFrames += 1L;
			}
		}

	
		protected internal virtual SpeechClassifiedData classify(DoubleData audio)
		{
			double num = SpeechClassifier.logRootMeanSquare(audio.getValues());
			this._isSpeech = false;
			if (num >= this.minSignal)
			{
				this.level = (this.level * (double)1f + num) / 2.0;
				if (num < this.background)
				{
					this.background = num;
				}
				else
				{
					this.background += (num - this.background) * this.adjustment;
				}
				if (this.level < this.background)
				{
					this.level = this.background;
				}
				this._isSpeech = (this.level - this.background > this.threshold);
			}
			SpeechClassifiedData speechClassifiedData = new SpeechClassifiedData(audio, this._isSpeech);
			if (this.logger.isLoggable(Level.FINEST))
			{
				string text = "";
				if (speechClassifiedData.isSpeech())
				{
					text = "*";
				}
				this.logger.finest(new StringBuilder().append("Bkg: ").append(this.background).append(", level: ").append(this.level).append(", current: ").append(num).append(' ').append(text).toString());
			}
			this.collectStats(this._isSpeech);
			return speechClassifiedData;
		}

		public virtual double getSNR()
		{
			double num = this.totalSpeechLevel / (double)this.speechFrames - this.totalBackgroundLevel / (double)this.backgroundFrames;
			this.logger.fine(new StringBuilder().append("Background ").append(this.totalBackgroundLevel / (double)this.backgroundFrames).toString());
			this.logger.fine(new StringBuilder().append("Speech ").append(this.totalSpeechLevel / (double)this.speechFrames).toString());
			this.logger.fine(new StringBuilder().append("SNR is ").append(num).toString());
			return num;
		}
		
		public SpeechClassifier(int frameLengthMs, double adjustment, double threshold, double minSignal)
		{
			this.__averageNumber = (double)1f;
			this.initLogger();
			this.frameLengthSec = (float)frameLengthMs / 1000f;
			this.adjustment = adjustment;
			this.threshold = threshold;
			this.minSignal = minSignal;
			this.initialize();
		}
	
		public SpeechClassifier()
		{
			this.__averageNumber = (double)1f;
		}
		
		public override void newProperties(PropertySheet ps)
		{
			base.newProperties(ps);
			int @int = ps.getInt("frameLengthInMs");
			this.frameLengthSec = (float)@int / 1000f;
			this.adjustment = ps.getDouble("adjustment");
			this.threshold = ps.getDouble("threshold");
			this.minSignal = ps.getDouble("minSignal");
			this.logger = ps.getLogger();
			this.initialize();
		}
	
		public override Data getData()
		{
			object obj = this.getPredecessor().getData();
			if (((Data)obj) is DataStartSignal)
			{
				this.reset();
			}
			if (((Data)obj) is DoubleData)
			{
				DoubleData audio = (DoubleData)((Data)obj);
				obj = this.classify(audio);
			}
			object obj2 = obj;
			Data result;
			if (obj2 != null)
			{
				if ((result = (obj2 as Data)) == null)
				{
					throw new IncompatibleClassChangeError();
				}
			}
			else
			{
				result = null;
			}
			return result;
		}

		public override bool isSpeech()
		{
			return this._isSpeech;
		}		
		
		public virtual bool getNoisy()
		{
			return this.getSNR() < 20.0;
		}

		
		protected internal double averageNumber
		{
			
			get
			{
				return this.__averageNumber;
			}
			
			private set
			{
				this.__averageNumber = value;
			}
		}

		[S4Integer(new object[]
		{
			64,
			"Ledu/cmu/sphinx/util/props/S4Integer;",
			"defaultValue",
			10
		})]
		public const string PROP_FRAME_LENGTH_MS = "frameLengthInMs";

		[S4Double(new object[]
		{
			64,
			"Ledu/cmu/sphinx/util/props/S4Double;",
			"defaultValue",
			0.0
		})]
		public const string PROP_MIN_SIGNAL = "minSignal";

		[S4Double(new object[]
		{
			64,
			"Ledu/cmu/sphinx/util/props/S4Double;",
			"defaultValue",
			10.0
		})]
		public const string PROP_THRESHOLD = "threshold";

		[S4Double(new object[]
		{
			64,
			"Ledu/cmu/sphinx/util/props/S4Double;",
			"defaultValue",
			0.003
		})]
		public const string PROP_ADJUSTMENT = "adjustment";

		internal double __averageNumber;

		protected internal double adjustment;

		protected internal double level;

		protected internal double background;

		protected internal double minSignal;

		protected internal double threshold;

		protected internal float frameLengthSec;

		protected internal new bool _isSpeech;

		protected internal long speechFrames;

		protected internal long backgroundFrames;

		protected internal double totalBackgroundLevel;

		protected internal double totalSpeechLevel;

		
		internal static bool assertionsDisabled = !ClassLiteral<SpeechClassifier>.Value.desiredAssertionStatus();
	}
}
