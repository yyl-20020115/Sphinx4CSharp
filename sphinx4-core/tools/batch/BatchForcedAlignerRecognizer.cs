using edu.cmu.sphinx.decoder.search;
using edu.cmu.sphinx.linguist.language.grammar;
using edu.cmu.sphinx.recognizer;
using edu.cmu.sphinx.result;
using edu.cmu.sphinx.util.props;
using java.io;
using java.lang;
using java.util;

namespace edu.cmu.sphinx.tools.batch
{
	public class BatchForcedAlignerRecognizer : BatchNISTRecognizer
	{		
		internal virtual void dumpTokenTimes(Token token)
		{
			if (token != null)
			{
				this.dumpTokenTimes(token.getPredecessor());
				java.lang.System.@out.println(new StringBuilder().append(token.getWord()).append(" ").append(token.getCollectTime()).toString());
			}
		}
		
		protected internal override void setInputStream(BatchNISTRecognizer.CTLUtterance utt)
		{
			base.setInputStream(utt);
			this.bfaGrammar.setUtterance(utt.getName());
		}

		protected internal override void handleResult(DataOutputStream @out, BatchNISTRecognizer.CTLUtterance utt, Result result)
		{
			java.lang.System.@out.println(new StringBuilder().append(utt).append(" --> ").append(result).toString());
			Token bestToken = result.getBestToken();
			this.dumpTokenTimes(bestToken);
		}
				
		public BatchForcedAlignerRecognizer(BatchForcedAlignerGrammar bfaGrammar, Recognizer recognizer, List inputDataProcessors, string ctlFile, string dataDir, string refFile, string ctmFile, int bitsPerSample, int samplesPerSecond, int framesPerSecond, int channelCount) : base(recognizer, inputDataProcessors, ctlFile, dataDir, refFile, ctmFile, bitsPerSample, samplesPerSecond, framesPerSecond, channelCount)
		{
			this.bfaGrammar = bfaGrammar;
		}

		public BatchForcedAlignerRecognizer()
		{
		}
	
		public override void newProperties(PropertySheet ps)
		{
			base.newProperties(ps);
			this.bfaGrammar = (BatchForcedAlignerGrammar)ps.getComponent("forcedAlignerGrammar");
		}
	
		public new static void main(string[] argv)
		{
			if (argv.Length != 1)
			{
				java.lang.System.@out.println("Usage: BatchForcedAlignerRecognizer propertiesFile");
				java.lang.System.exit(1);
			}
			BatchNISTRecognizer.main(argv);
		}

		internal string segFile;

		internal ForcedAlignerGrammar forcedAlignerGrammar;

		public BatchForcedAlignerGrammar bfaGrammar;
	}
}
