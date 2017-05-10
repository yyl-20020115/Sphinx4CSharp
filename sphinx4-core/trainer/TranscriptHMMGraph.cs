using System;

using edu.cmu.sphinx.linguist.acoustic;
using IKVM.Attributes;

namespace edu.cmu.sphinx.trainer
{
	public class TranscriptHMMGraph : Graph, TranscriptGraph
	{
		
		public new static void __<clinit>()
		{
		}

		public TranscriptHMMGraph(string context, Transcript transcript, AcousticModel acousticModel, UnitManager unitManager)
		{
			BuildTranscriptHMM buildTranscriptHMM = new BuildTranscriptHMM(context, transcript, acousticModel, unitManager);
			this.copyGraph(buildTranscriptHMM.getGraph());
		}
	}
}
