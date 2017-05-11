using edu.cmu.sphinx.linguist.acoustic;

namespace edu.cmu.sphinx.trainer
{
	public class UtteranceHMMGraph : Graph, UtteranceGraph
	{
		public UtteranceHMMGraph(string context, Utterance utterance, AcousticModel acousticModel, UnitManager unitManager)
		{
			utterance.startTranscriptIterator();
			while (utterance.hasMoreTranscripts())
			{
				Transcript transcript = utterance.nextTranscript();
				TranscriptHMMGraph transcriptGraph = new TranscriptHMMGraph(context, transcript, acousticModel, unitManager);
				this.add(transcriptGraph);
			}
		}
		public virtual void add(Graph transcriptGraph)
		{
			this.copyGraph(transcriptGraph);
		}
	}
}
