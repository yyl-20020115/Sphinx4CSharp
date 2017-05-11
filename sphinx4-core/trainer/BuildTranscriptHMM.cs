using edu.cmu.sphinx.linguist.acoustic;
using edu.cmu.sphinx.linguist.acoustic.tiedstate;
using edu.cmu.sphinx.linguist.dictionary;
using ikvm.@internal;
using java.lang;

namespace edu.cmu.sphinx.trainer
{
	public class BuildTranscriptHMM : java.lang.Object
	{
		private Graph buildWordGraph(Transcript transcript)
		{
			Dictionary dictionary = transcript.getDictionary();
			if (!BuildTranscriptHMM.assertionsDisabled && !java.lang.String.instancehelper_endsWith(java.lang.Object.instancehelper_getClass(dictionary).getName(), "TrainerDictionary"))
			{
				
				throw new AssertionError();
			}
			this.dictionary = (TrainerDictionary)dictionary;
			transcript.startWordIterator();
			Graph graph = new Graph();
			Node node = new Node(NodeType.__UTTERANCE_BEGIN);
			graph.addNode(node);
			graph.setInitialNode(node);
			if (transcript.isExact())
			{
				Node node2 = node;
				transcript.startWordIterator();
				Node node3;
				while (transcript.hasMoreWords())
				{
					node3 = new Node(NodeType.__WORD, transcript.nextWord());
					graph.linkNodes(node2, node3);
					node2 = node3;
				}
				node3 = new Node(NodeType.__UTTERANCE_END);
				graph.linkNodes(node2, node3);
				graph.setFinalNode(node3);
			}
			else
			{
				Node node2 = new Node(NodeType.__SILENCE_WITH_LOOPBACK);
				graph.linkNodes(node, node2);
				transcript.startWordIterator();
				while (transcript.hasMoreWords())
				{
					string text = transcript.nextWord();
					Pronunciation[] pronunciations = this.dictionary.getWord(text).getPronunciations();
					int num = pronunciations.Length;
					Node[] array = new Node[num];
					Node node4 = new Node(NodeType.__DUMMY);
					graph.linkNodes(node2, node4);
					Node node5 = new Node(NodeType.__DUMMY);
					for (int i = 0; i < num; i++)
					{
						string text2 = pronunciations[i].getWord().getSpelling();
						if (i > 0)
						{
							text2 = new StringBuilder().append(text2).append("(").append(i).append(')').toString();
						}
						array[i] = new Node(NodeType.__WORD, text2);
						graph.linkNodes(node4, array[i]);
						graph.linkNodes(array[i], node5);
					}
					node2 = new Node(NodeType.__SILENCE_WITH_LOOPBACK);
					graph.linkNodes(node5, node2);
				}
				Node node3 = new Node(NodeType.__UTTERANCE_END);
				graph.linkNodes(node2, node3);
				graph.setFinalNode(node3);
			}
			return graph;
		}
		
		private Graph buildPhonemeGraph(Graph graph)
		{
			Graph graph2 = new Graph();
			graph2.copyGraph(graph);
			Node[] array = graph2.nodeToArray();
			int num = array.Length;
			for (int i = 0; i < num; i++)
			{
				Node node = array[i];
				if (node.getType().equals(NodeType.__WORD))
				{
					string id = node.getID();
					Graph graph3 = this.dictionary.getWordGraph(id, false);
					graph2.insertGraph(graph3, node);
				}
			}
			return graph2;
		}
		
		public virtual Graph buildContextDependentPhonemeGraph(Graph phonemeGraph)
		{
			Graph graph = new Graph();
			graph.copyGraph(phonemeGraph);
			return graph;
		}
		
		public virtual Graph buildHMMGraph(Graph cdGraph)
		{
			Graph graph = new Graph();
			graph.copyGraph(cdGraph);
			Node[] array = graph.nodeToArray();
			int num = array.Length;
			int i = 0;
			while (i < num)
			{
				Node node = array[i];
				Unit unit;
				if (node.getType().equals(NodeType.__PHONE))
				{
					unit = this.unitManager.getUnit(node.getID());
					goto IL_75;
				}
				if (node.getType().equals(NodeType.__SILENCE_WITH_LOOPBACK))
				{
					unit = this.unitManager.getUnit("SIL");
					goto IL_75;
				}
				IL_AB:
				i++;
				continue;
				IL_75:
				HMM hmm = this.acousticModel.lookupNearestHMM(unit, HMMPosition.__UNDEFINED, false);
				Graph graph2 = this.buildModelGraph((SenoneHMM)hmm);
				graph2.validate();
				graph.insertGraph(graph2, node);
				goto IL_AB;
			}
			return graph;
		}		
		private Graph buildModelGraph(SenoneHMM senoneHMM)
		{
			Graph graph = new Graph();
			Node node = null;
			float[][] transitionMatrix = senoneHMM.getTransitionMatrix();
			Node node2 = new Node(NodeType.__DUMMY);
			graph.addNode(node2);
			graph.setInitialNode(node2);
			for (int i = 0; i < senoneHMM.getOrder() + 1; i++)
			{
				node = new Node(NodeType.__STATE, senoneHMM.getUnit().getName());
				node.setObject(senoneHMM.getState(i));
				graph.addNode(node);
				if (i == 0)
				{
					graph.linkNodes(node2, node);
				}
				for (int j = 0; j <= i; j++)
				{
					if (transitionMatrix[j][i] != -3.40282347E+38f)
					{
						graph.linkNodes(graph.getNode(j + 1), node);
					}
				}
				node2 = node;
			}
			graph.setFinalNode(node);
			return graph;
		}
		
		public BuildTranscriptHMM(string context, Transcript transcript, AcousticModel acousticModel, UnitManager unitManager)
		{
			this.acousticModel = acousticModel;
			this.unitManager = unitManager;
			this.wordGraph = this.buildWordGraph(transcript);
			if (!BuildTranscriptHMM.assertionsDisabled && !this.wordGraph.validate())
			{
				object obj = "Word graph not validated";
				
				throw new AssertionError(obj);
			}
			this.phonemeGraph = this.buildPhonemeGraph(this.wordGraph);
			if (!BuildTranscriptHMM.assertionsDisabled && !this.phonemeGraph.validate())
			{
				object obj2 = "Phone graph not validated";
				
				throw new AssertionError(obj2);
			}
			this.contextDependentPhoneGraph = this.buildContextDependentPhonemeGraph(this.phonemeGraph);
			if (!BuildTranscriptHMM.assertionsDisabled && !this.contextDependentPhoneGraph.validate())
			{
				object obj3 = "Context dependent graph not validated";
				
				throw new AssertionError(obj3);
			}
			this.hmmGraph = this.buildHMMGraph(this.contextDependentPhoneGraph);
			if (!BuildTranscriptHMM.assertionsDisabled && !this.hmmGraph.validate())
			{
				object obj4 = "HMM graph not validated";
				
				throw new AssertionError(obj4);
			}
		}

		public virtual Graph getGraph()
		{
			return this.hmmGraph;
		}

		private Graph wordGraph;

		private Graph phonemeGraph;

		private Graph contextDependentPhoneGraph;

		private Graph hmmGraph;

		private TrainerDictionary dictionary;

		private AcousticModel acousticModel;

		private UnitManager unitManager;
		
		internal static bool assertionsDisabled = !ClassLiteral<BuildTranscriptHMM>.Value.desiredAssertionStatus();
	}
}
