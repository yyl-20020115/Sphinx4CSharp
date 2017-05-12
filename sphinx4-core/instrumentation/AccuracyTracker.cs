using edu.cmu.sphinx.decoder;
using edu.cmu.sphinx.recognizer;
using edu.cmu.sphinx.result;
using edu.cmu.sphinx.util;
using edu.cmu.sphinx.util.props;
using java.lang;
using java.util;

namespace edu.cmu.sphinx.instrumentation
{
	public abstract class AccuracyTracker : ConfigurableAdapter, ResultListener, EventListener, Configurable, Resetable, StateListener, Monitor
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
		public AccuracyTracker(Recognizer recognizer, bool showSummary, bool _showDetails, bool showResults, bool showAlignedResults, bool showRawResults)
		{
			this.aligner = new NISTAlign(false, false);
			this.initRecognizer(recognizer);
			this.initLogger();
			this.showSummary = showSummary;
			this._showDetails = _showDetails;
			this.showResults = showResults;
			this.showAlignedResults = showAlignedResults;
			this.showRaw = showRawResults;
			this.aligner.setShowResults(showResults);
			this.aligner.setShowAlignedResults(showAlignedResults);
		}
	
		public AccuracyTracker()
		{
			this.aligner = new NISTAlign(false, false);
		}

		public override void newProperties(PropertySheet ps)
		{
			base.newProperties(ps);
			this.initRecognizer((Recognizer)ps.getComponent("recognizer"));
			this.showSummary = ps.getBoolean("showSummary").booleanValue();
			this._showDetails = ps.getBoolean("_showDetails").booleanValue();
			this.showResults = ps.getBoolean("showResults").booleanValue();
			this.showAlignedResults = ps.getBoolean("showAlignedResults").booleanValue();
			this.showRaw = ps.getBoolean("showRawResults").booleanValue();
			this.aligner.setShowResults(this.showResults);
			this.aligner.setShowAlignedResults(this.showAlignedResults);
		}

		public virtual void reset()
		{
			this.aligner.resetTotals();
		}

		public override string getName()
		{
			return this.name;
		}

		public virtual NISTAlign getAligner()
		{
			return this.aligner;
		}
		
		protected internal virtual void showDetails(string rawText)
		{
			if (this._showDetails)
			{
				this.aligner.printSentenceSummary();
				if (this.showRaw)
				{
					this.logger.info(new StringBuilder().append("RAW     ").append(rawText).toString());
				}
				this.aligner.printTotalSummary();
			}
		}

		public abstract void newResult(Result r);
		
		public virtual void statusChanged(Recognizer.State status)
		{
			if (status == Recognizer.State.__DEALLOCATED && this.showSummary)
			{
				this.logger.info("\n# --------------- Summary statistics ---------");
				this.aligner.printTotalSummary();
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

		[S4Boolean(new object[]
		{
			64,
			"Ledu/cmu/sphinx/util/props/S4Boolean;",
			"defaultValue",
			true
		})]
		public const string PROP_SHOW_RESULTS = "showResults";

		[S4Boolean(new object[]
		{
			64,
			"Ledu/cmu/sphinx/util/props/S4Boolean;",
			"defaultValue",
			true
		})]
		public const string PROP_SHOW_ALIGNED_RESULTS = "showAlignedResults";

		[S4Boolean(new object[]
		{
			64,
			"Ledu/cmu/sphinx/util/props/S4Boolean;",
			"defaultValue",
			true
		})]
		public const string PROP_SHOW_RAW_RESULTS = "showRawResults";

		private string name = string.Empty;

		private Recognizer recognizer;

		private bool showSummary;

		private bool _showDetails;

		private bool showResults;

		private bool showAlignedResults;

		private bool showRaw;

		private NISTAlign aligner;
	}
}
