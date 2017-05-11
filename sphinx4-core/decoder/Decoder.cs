using edu.cmu.sphinx.decoder.search;
using edu.cmu.sphinx.result;
using edu.cmu.sphinx.util.props;
using java.util;

namespace edu.cmu.sphinx.decoder
{
	public class Decoder : AbstractDecoder
	{	
		public Decoder()
		{
		}

		public override void newProperties(PropertySheet ps)
		{
			base.newProperties(ps);
			this.featureBlockSize = ps.getInt("featureBlockSize");
		}
			
		public Decoder(SearchManager searchManager, bool fireNonFinalResults, bool autoAllocate, List resultListeners, int featureBlockSize) : base(searchManager, fireNonFinalResults, autoAllocate, resultListeners)
		{
			this.featureBlockSize = featureBlockSize;
		}
		
		public override Result decode(string referenceText)
		{
			this.searchManager.startRecognition();
			Result result;
			do
			{
				result = this.searchManager.recognize(this.featureBlockSize);
				if (result != null)
				{
					result.setReferenceText(referenceText);
					this.fireResultListeners(result);
				}
			}
			while (result != null && !result.isFinal());
			this.searchManager.stopRecognition();
			return result;
		}

		[S4Integer(new object[]
		{
			64,
			"Ledu/cmu/sphinx/util/props/S4Integer;",
			"defaultValue",
			2147483647
		})]
		public const string PROP_FEATURE_BLOCK_SIZE = "featureBlockSize";

		private int featureBlockSize;
	}
}
