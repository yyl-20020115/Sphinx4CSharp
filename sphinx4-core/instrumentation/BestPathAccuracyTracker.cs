using edu.cmu.sphinx.decoder.search;
using edu.cmu.sphinx.recognizer;
using edu.cmu.sphinx.result;
using edu.cmu.sphinx.util.props;

namespace edu.cmu.sphinx.instrumentation
{
	public class BestPathAccuracyTracker : AccuracyTracker
	{
		private void showFullPath(Result result)
		{
			if (this._showFullPath)
			{
				java.lang.System.@out.println();
				Token bestToken = result.getBestToken();
				if (bestToken != null)
				{
					bestToken.dumpTokenPath();
				}
				else
				{
					java.lang.System.@out.println("Null result");
				}
				java.lang.System.@out.println();
			}
		}
	
		public BestPathAccuracyTracker(Recognizer recognizer, bool showSummary, bool showDetails, bool showResults, bool showAlignedResults, bool showRawResults, bool _showFullPath) : base(recognizer, showSummary, showDetails, showResults, showAlignedResults, showRawResults)
		{
			this._showFullPath = _showFullPath;
		}
		
		public BestPathAccuracyTracker()
		{
		}
	
		public override void newProperties(PropertySheet ps)
		{
			base.newProperties(ps);
			this._showFullPath = ps.getBoolean("_showFullPath").booleanValue();
		}
	
		public override void newResult(Result result)
		{
			string referenceText = result.getReferenceText();
			if (result.isFinal() && referenceText != null)
			{
				string bestResultNoFiller = result.getBestResultNoFiller();
				this.getAligner().align(referenceText, bestResultNoFiller);
				this.showFullPath(result);
				this.showDetails(result.toString());
			}
		}

		[S4Boolean(new object[]
		{
			64,
			"Ledu/cmu/sphinx/util/props/S4Boolean;",
			"defaultValue",
			false
		})]
		public const string PROP_SHOW_FULL_PATH = "showFullPath";

		private bool _showFullPath;
	}
}
