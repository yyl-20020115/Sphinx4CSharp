using java.lang;
using java.util;
using java.util.logging;

namespace edu.cmu.sphinx.trainer
{
	public class SimpleUtterance : java.lang.Object, Utterance
	{
		public SimpleUtterance(string utteranceID)
		{
			SimpleUtterance.logger.info(new StringBuilder().append("Utterance ID: ").append(utteranceID).toString());
			this.utteranceID = utteranceID;
			this.transcriptSet = new LinkedList();
		}

		public SimpleUtterance()
		{
			this.transcriptSet = new LinkedList();
		}

		public virtual void add(string transcript, linguist.dictionary.Dictionary dictionary, bool isExact, string wordSeparator)
		{
			SimpleUtterance.logger.info(new StringBuilder().append("Transcript: ").append(transcript).toString());
			this.transcriptSet.add(new SimpleTranscript(transcript, dictionary, isExact, wordSeparator));
		}

		public virtual void startTranscriptIterator()
		{
			this.transcriptIterator = this.transcriptSet.iterator();
		}
	
		public virtual bool hasMoreTranscripts()
		{
			return this.transcriptIterator.hasNext();
		}
		
		public virtual Transcript nextTranscript()
		{
			return (Transcript)this.transcriptIterator.next();
		}

		public override string toString()
		{
			return this.utteranceID;
		}

		private string utteranceID;
		
		private Collection transcriptSet;
		
		private Iterator transcriptIterator;

		private static Logger logger = Logger.getLogger("edu.cmu.sphinx.trainer.SimpleUtterance");
	}
}
