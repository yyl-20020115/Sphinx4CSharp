using System;

using edu.cmu.sphinx.decoder;
using edu.cmu.sphinx.recognizer;
using edu.cmu.sphinx.result;
using edu.cmu.sphinx.util.props;
using IKVM.Attributes;
using java.lang;
using java.util;

namespace edu.cmu.sphinx.instrumentation
{
	public class RejectionTracker : java.lang.Object, ResultListener, EventListener, Configurable, Resetable, Monitor, StateListener
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
		
		private void printStats()
		{
			if (this.showSummary)
			{
				float num = (float)(this.numCorrectOutOfGrammarUtterances + this.numCorrectInGrammarUtterances) / (float)this.numUtterances * 100f;
				java.lang.System.@out.println(new StringBuilder().append("   Rejection Accuracy: ").append(num).append('%').toString());
			}
			if (this.showDetails)
			{
				java.lang.System.@out.println(new StringBuilder().append("   Correct OOG: ").append(this.numCorrectOutOfGrammarUtterances).append("   False OOG: ").append(this.numFalseOutOfGrammarUtterances).append("   Correct IG: ").append(this.numCorrectInGrammarUtterances).append("   False IG: ").append(this.numFalseInGrammarUtterances).append("   Actual number: ").append(this.numOutOfGrammarUtterances).toString());
			}
		}
		
		public RejectionTracker(Recognizer recognizer, bool showSummary, bool showDetails)
		{
			this.initRecognizer(recognizer);
			this.showSummary = showSummary;
			this.showDetails = showDetails;
		}
	
		public RejectionTracker()
		{
		}

		public virtual void newProperties(PropertySheet ps)
		{
			this.initRecognizer((Recognizer)ps.getComponent("recognizer"));
			this.showSummary = ps.getBoolean("showSummary").booleanValue();
			this.showDetails = ps.getBoolean("showDetails").booleanValue();
		}

		public virtual void reset()
		{
			this.numUtterances = 0;
			this.numOutOfGrammarUtterances = 0;
			this.numCorrectOutOfGrammarUtterances = 0;
			this.numFalseOutOfGrammarUtterances = 0;
			this.numCorrectInGrammarUtterances = 0;
			this.numFalseInGrammarUtterances = 0;
		}

		public virtual string getName()
		{
			return this.name;
		}
		
		public virtual void newResult(Result result)
		{
			string referenceText = result.getReferenceText();
			if (result.isFinal() && referenceText != null)
			{
				this.numUtterances++;
				string bestResultNoFiller = result.getBestResultNoFiller();
				if (java.lang.String.instancehelper_equals(referenceText, "<unk>"))
				{
					this.numOutOfGrammarUtterances++;
					if (java.lang.String.instancehelper_equals(bestResultNoFiller, "<unk>"))
					{
						this.numCorrectOutOfGrammarUtterances++;
					}
					else
					{
						this.numFalseInGrammarUtterances++;
					}
				}
				else if (java.lang.String.instancehelper_equals(bestResultNoFiller, "<unk>"))
				{
					this.numFalseOutOfGrammarUtterances++;
				}
				else
				{
					this.numCorrectInGrammarUtterances++;
				}
				this.printStats();
			}
		}
	
		public virtual void statusChanged(Recognizer.State status)
		{
			if (status == Recognizer.State.__DEALLOCATED)
			{
				this.printStats();
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

		private string name;

		private Recognizer recognizer;

		private bool showSummary;

		private bool showDetails;

		private int numUtterances;

		private int numOutOfGrammarUtterances;

		private int numCorrectOutOfGrammarUtterances;

		private int numFalseOutOfGrammarUtterances;

		private int numCorrectInGrammarUtterances;

		private int numFalseInGrammarUtterances;
	}
}
