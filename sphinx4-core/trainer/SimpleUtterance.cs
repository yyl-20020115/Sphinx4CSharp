using System;

using edu.cmu.sphinx.linguist.dictionary;
using IKVM.Attributes;
using java.lang;
using java.util;
using java.util.logging;

namespace edu.cmu.sphinx.trainer
{
	[Implements(new string[]
	{
		"edu.cmu.sphinx.trainer.Utterance"
	})]
	public class SimpleUtterance : java.lang.Object, Utterance
	{
		
		public static void __<clinit>()
		{
		}

		[LineNumberTable(new byte[]
		{
			159,
			189,
			104,
			127,
			5,
			103,
			107
		})]
		
		public SimpleUtterance(string utteranceID)
		{
			SimpleUtterance.logger.info(new StringBuilder().append("Utterance ID: ").append(utteranceID).toString());
			this.utteranceID = utteranceID;
			this.transcriptSet = new LinkedList();
		}

		[LineNumberTable(new byte[]
		{
			159,
			179,
			104,
			107
		})]
		
		public SimpleUtterance()
		{
			this.transcriptSet = new LinkedList();
		}

		[LineNumberTable(new byte[]
		{
			159,
			126,
			66,
			127,
			5,
			150
		})]
		
		public virtual void add(string transcript, Dictionary dictionary, bool isExact, string wordSeparator)
		{
			SimpleUtterance.logger.info(new StringBuilder().append("Transcript: ").append(transcript).toString());
			this.transcriptSet.add(new SimpleTranscript(transcript, dictionary, isExact, wordSeparator));
		}

		[LineNumberTable(new byte[]
		{
			24,
			113
		})]
		
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

		[LineNumberTable(new byte[]
		{
			159,
			174,
			101,
			42
		})]
		static SimpleUtterance()
		{
		}

		private string utteranceID;

		
		private Collection transcriptSet;

		
		private Iterator transcriptIterator;

		private static Logger logger = Logger.getLogger("edu.cmu.sphinx.trainer.SimpleUtterance");
	}
}
