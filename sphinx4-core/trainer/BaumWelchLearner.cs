using System;

using edu.cmu.sphinx.frontend;
using edu.cmu.sphinx.frontend.util;
using edu.cmu.sphinx.linguist.acoustic;
using edu.cmu.sphinx.linguist.acoustic.tiedstate;
using edu.cmu.sphinx.linguist.acoustic.tiedstate.trainer;
using edu.cmu.sphinx.util;
using edu.cmu.sphinx.util.props;
using IKVM.Attributes;
using ikvm.@internal;
using IKVM.Runtime;
using java.io;
using java.lang;
using java.util;
using java.util.logging;

namespace edu.cmu.sphinx.trainer
{
	[Implements(new string[]
	{
		"edu.cmu.sphinx.trainer.Learner"
	})]
	public class BaumWelchLearner : java.lang.Object, Learner, Configurable
	{
		
		public static void __<clinit>()
		{
		}

		[Throws(new string[]
		{
			"java.io.IOException"
		})]
		[LineNumberTable(new byte[]
		{
			38,
			103,
			103,
			109
		})]
		
		public virtual void setUtterance(Utterance utterance)
		{
			string text = java.lang.Object.instancehelper_toString(utterance);
			FileInputStream @is = new FileInputStream(text);
			this.dataSource.setInputStream(@is, false);
		}

		public virtual void setGraph(UtteranceGraph graph)
		{
			this.graph = graph;
		}

		[LineNumberTable(new byte[]
		{
			51,
			145,
			104,
			167,
			109,
			113,
			104,
			196,
			109,
			164,
			109,
			255,
			7,
			70,
			226,
			60,
			97,
			127,
			5,
			102,
			130
		})]
		
		private bool getFeature()
		{
			bool result;
			DataProcessingException ex2;
			try
			{
				this.curFeature = this.frontEnd.getData();
				if (this.curFeature == null)
				{
					result = false;
				}
				else
				{
					if (this.curFeature is DataStartSignal)
					{
						this.curFeature = this.frontEnd.getData();
						if (this.curFeature == null)
						{
							return false;
						}
					}
					if (this.curFeature is DataEndSignal)
					{
						return false;
					}
					if (this.curFeature is Signal)
					{
						string text = "Can't score non-content feature";
						
						throw new Error(text);
					}
					return true;
				}
			}
			catch (DataProcessingException ex)
			{
				ex2 = ByteCodeHelper.MapException<DataProcessingException>(ex, 1);
				goto IL_8B;
			}
			return result;
			IL_8B:
			DataProcessingException ex3 = ex2;
			java.lang.System.@out.println(new StringBuilder().append("DataProcessingException ").append(ex3).toString());
			Throwable.instancehelper_printStackTrace(ex3);
			return false;
		}

		[LineNumberTable(new byte[]
		{
			160,
			212,
			112,
			111,
			151,
			155,
			240,
			58,
			233,
			74,
			103,
			246,
			69,
			115,
			173,
			109,
			133,
			109,
			110,
			105,
			165,
			109,
			102,
			139,
			109,
			111,
			174,
			196,
			114,
			41,
			139,
			105,
			137,
			132,
			103,
			5,
			233,
			71,
			199,
			125,
			230,
			71,
			165,
			191,
			0,
			240,
			7,
			233,
			125,
			115,
			109,
			99,
			99,
			109,
			109,
			110,
			105,
			133,
			110,
			109,
			109,
			165,
			109,
			102,
			171,
			109,
			111,
			113,
			98,
			204,
			114,
			41,
			139,
			105,
			169,
			132,
			103,
			5,
			199,
			98,
			199,
			127,
			3,
			230,
			71,
			229,
			71,
			240,
			4,
			233,
			126
		})]
		
		private void forwardPass(TrainerScore[] array)
		{
			for (int i = 0; i < this.graph.size(); i++)
			{
				this.outputProbs[i] = this.calculateScores(i);
				array[i] = new TrainerScore(this.curFeature, this.outputProbs[i], (HMMState)this.graph.getNode(i).getObject(), this.componentScores);
				array[i].setAlpha(this.probCurrentFrame[i]);
			}
			float[] array2 = this.probCurrentFrame;
			this.probCurrentFrame = new float[this.graph.size()];
			int j;
			for (j = 0; j < this.graph.size(); j++)
			{
				Node node = this.graph.getNode(j);
				if (node.isType("STATE"))
				{
					SenoneHMMState senoneHMMState = (SenoneHMMState)node.getObject();
					SenoneHMM senoneHMM = (SenoneHMM)senoneHMMState.getHMM();
					if (senoneHMMState.isEmitting())
					{
						this.probCurrentFrame[j] = float.MinValue;
						node.startIncomingEdgeIterator();
						while (node.hasMoreIncomingEdges())
						{
							Node source = node.nextIncomingEdge().getSource();
							int num = this.graph.indexOf(source);
							HMMState hmmstate = (HMMState)source.getObject();
							float num2;
							if (hmmstate != null)
							{
								if (!BaumWelchLearner.assertionsDisabled && hmmstate.isEmitting() && hmmstate.getHMM() != senoneHMM)
								{
									
									throw new AssertionError();
								}
								if (!hmmstate.isEmitting())
								{
									num2 = 0f;
								}
								else
								{
									num2 = senoneHMM.getTransitionProbability(hmmstate.getState(), senoneHMMState.getState());
								}
							}
							else
							{
								num2 = 0f;
							}
							this.probCurrentFrame[j] = this.logMath.addAsLinear(this.probCurrentFrame[j], array2[num] + num2);
						}
						float[] array3 = this.probCurrentFrame;
						int num3 = j;
						float[] array4 = array3;
						array4[num3] += this.outputProbs[j];
						array[j].setAlpha(this.probCurrentFrame[j]);
					}
				}
			}
			j = 0;
			while (j < this.graph.size())
			{
				Node node = this.graph.getNode(j);
				HMMState hmmstate2 = null;
				SenoneHMM senoneHMM = null;
				if (node.isType("STATE"))
				{
					hmmstate2 = (HMMState)node.getObject();
					senoneHMM = (SenoneHMM)hmmstate2.getHMM();
					if (!hmmstate2.isEmitting())
					{
						goto IL_26F;
					}
				}
				else
				{
					if (!this.graph.isInitialNode(node))
					{
						goto IL_26F;
					}
					array[j].setAlpha(float.MinValue);
					this.probCurrentFrame[j] = float.MinValue;
				}
				IL_35D:
				j++;
				continue;
				IL_26F:
				this.probCurrentFrame[j] = float.MinValue;
				node.startIncomingEdgeIterator();
				while (node.hasMoreIncomingEdges())
				{
					Node source2 = node.nextIncomingEdge().getSource();
					int num4 = this.graph.indexOf(source2);
					float num5;
					if (source2.isType("STATE"))
					{
						HMMState hmmstate3 = (HMMState)source2.getObject();
						if (!BaumWelchLearner.assertionsDisabled && hmmstate3.isEmitting() && hmmstate3.getHMM() != senoneHMM)
						{
							
							throw new AssertionError();
						}
						if (!hmmstate3.isEmitting())
						{
							num5 = 0f;
						}
						else
						{
							num5 = senoneHMM.getTransitionProbability(hmmstate3.getState(), hmmstate2.getState());
						}
					}
					else
					{
						num5 = 0f;
					}
					this.probCurrentFrame[j] = this.logMath.addAsLinear(this.probCurrentFrame[j], this.probCurrentFrame[num4] + num5);
				}
				array[j].setAlpha(this.probCurrentFrame[j]);
				goto IL_35D;
			}
		}

		[LineNumberTable(new byte[]
		{
			121,
			102,
			108,
			103,
			108,
			236,
			70,
			140,
			108,
			110,
			104,
			46,
			200,
			142,
			102,
			107,
			104,
			105,
			111,
			142,
			208,
			110,
			105,
			142,
			146,
			197,
			103,
			104,
			103,
			104,
			144,
			159,
			10,
			110,
			46,
			168,
			109,
			143,
			110,
			103,
			108,
			105,
			105,
			111,
			142,
			110,
			210,
			110,
			105,
			174,
			133
		})]
		
		private TrainerScore[][] prepareScore()
		{
			ArrayList arrayList = new ArrayList();
			int num = this.graph.size();
			TrainerScore[] array = new TrainerScore[num];
			this.betas = new float[num];
			this.outputProbs = new float[num];
			this.probCurrentFrame = new float[num];
			Node initialNode = this.graph.getInitialNode();
			int num2 = this.graph.indexOf(initialNode);
			for (int i = 0; i < num; i++)
			{
				this.probCurrentFrame[i] = float.MinValue;
			}
			this.probCurrentFrame[num2] = 0f;
			initialNode.startOutgoingEdgeIterator();
			while (initialNode.hasMoreOutgoingEdges())
			{
				Edge edge = initialNode.nextOutgoingEdge();
				Node destination = edge.getDestination();
				int num3 = this.graph.indexOf(destination);
				if (!destination.isType("STATE"))
				{
					this.probCurrentFrame[num3] = 0f;
				}
				else
				{
					HMMState hmmstate = (HMMState)destination.getObject();
					if (!hmmstate.isEmitting())
					{
						this.probCurrentFrame[num3] = 0f;
					}
					if (!BaumWelchLearner.assertionsDisabled)
					{
						
						throw new AssertionError();
					}
				}
			}
			this.lastFeatureIndex = 0;
			while (this.getFeature())
			{
				this.forwardPass(array);
				arrayList.add(array);
				this.lastFeatureIndex++;
			}
			BaumWelchLearner.logger.info(new StringBuilder().append("Feature frames read: ").append(this.lastFeatureIndex).toString());
			for (int i = 0; i < this.probCurrentFrame.Length; i++)
			{
				this.probCurrentFrame[i] = float.MinValue;
			}
			Node finalNode = this.graph.getFinalNode();
			int num4 = this.graph.indexOf(finalNode);
			this.probCurrentFrame[num4] = 0f;
			finalNode.startIncomingEdgeIterator();
			while (finalNode.hasMoreIncomingEdges())
			{
				Edge edge2 = finalNode.nextIncomingEdge();
				Node source = edge2.getSource();
				int num5 = this.graph.indexOf(source);
				if (!source.isType("STATE"))
				{
					this.probCurrentFrame[num5] = 0f;
					if (!BaumWelchLearner.assertionsDisabled)
					{
						
						throw new AssertionError();
					}
				}
				else
				{
					HMMState hmmstate2 = (HMMState)source.getObject();
					if (!hmmstate2.isEmitting())
					{
						this.probCurrentFrame[num5] = 0f;
					}
				}
			}
			return (TrainerScore[][])arrayList.toArray(new TrainerScore[arrayList.size()][]);
		}

		[LineNumberTable(new byte[]
		{
			161,
			105,
			112,
			112,
			16,
			198,
			103,
			182,
			115,
			173,
			109,
			133,
			109,
			110,
			105,
			197,
			109,
			102,
			171,
			109,
			111,
			110,
			164,
			114,
			41,
			139,
			107,
			137,
			100,
			103,
			37,
			233,
			70,
			199,
			127,
			9,
			198,
			133,
			240,
			16,
			233,
			120,
			117,
			109,
			99,
			109,
			109,
			105,
			133,
			110,
			109,
			109,
			165,
			109,
			102,
			171,
			109,
			111,
			110,
			174,
			191,
			2,
			231,
			74,
			98,
			199,
			127,
			3,
			166,
			133,
			240,
			15,
			233,
			115
		})]
		
		private void backwardPass(TrainerScore[] array)
		{
			for (int i = 0; i < this.graph.size(); i++)
			{
				this.outputProbs[i] = array[i].getScore();
				array[i].setBeta(this.probCurrentFrame[i]);
			}
			float[] array2 = this.probCurrentFrame;
			this.probCurrentFrame = new float[this.graph.size()];
			int j;
			for (j = 0; j < this.graph.size(); j++)
			{
				Node node = this.graph.getNode(j);
				if (node.isType("STATE"))
				{
					HMMState hmmstate = (HMMState)node.getObject();
					SenoneHMM senoneHMM = (SenoneHMM)hmmstate.getHMM();
					if (hmmstate.isEmitting())
					{
						this.probCurrentFrame[j] = float.MinValue;
						node.startOutgoingEdgeIterator();
						while (node.hasMoreOutgoingEdges())
						{
							Node destination = node.nextOutgoingEdge().getDestination();
							int num = this.graph.indexOf(destination);
							HMMState hmmstate2 = (HMMState)destination.getObject();
							float num2;
							if (hmmstate2 != null)
							{
								if (!BaumWelchLearner.assertionsDisabled && hmmstate2.isEmitting() && hmmstate2.getHMM() != senoneHMM)
								{
									
									throw new AssertionError();
								}
								if (hmmstate2.getHMM() != senoneHMM)
								{
									num2 = 0f;
								}
								else
								{
									num2 = senoneHMM.getTransitionProbability(hmmstate.getState(), hmmstate2.getState());
								}
							}
							else
							{
								num2 = 0f;
							}
							this.probCurrentFrame[j] = this.logMath.addAsLinear(this.probCurrentFrame[j], array2[num] + num2 + this.outputProbs[num]);
						}
						array[j].setBeta(this.probCurrentFrame[j]);
					}
				}
			}
			j = this.graph.size() - 1;
			while (j >= 0)
			{
				Node node = this.graph.getNode(j);
				HMMState hmmstate = null;
				if (node.isType("STATE"))
				{
					hmmstate = (HMMState)node.getObject();
					if (!hmmstate.isEmitting())
					{
						goto IL_21A;
					}
				}
				else
				{
					if (!this.graph.isFinalNode(node))
					{
						goto IL_21A;
					}
					array[j].setBeta(float.MinValue);
					this.probCurrentFrame[j] = float.MinValue;
				}
				IL_2DE:
				j += -1;
				continue;
				IL_21A:
				this.probCurrentFrame[j] = float.MinValue;
				node.startOutgoingEdgeIterator();
				while (node.hasMoreOutgoingEdges())
				{
					Node destination2 = node.nextOutgoingEdge().getDestination();
					int num3 = this.graph.indexOf(destination2);
					float num4;
					if (destination2.isType("STATE"))
					{
						HMMState hmmstate3 = (HMMState)destination2.getObject();
						if (!BaumWelchLearner.assertionsDisabled && !hmmstate3.isEmitting() && hmmstate3 != hmmstate)
						{
							
							throw new AssertionError();
						}
						num4 = 0f;
					}
					else
					{
						num4 = 0f;
					}
					this.probCurrentFrame[j] = this.logMath.addAsLinear(this.probCurrentFrame[j], this.probCurrentFrame[num3] + num4);
				}
				array[j].setBeta(this.probCurrentFrame[j]);
				goto IL_2DE;
			}
		}

		[LineNumberTable(new byte[]
		{
			160,
			187,
			119,
			139,
			146,
			173,
			156,
			103,
			134
		})]
		
		private float calculateScores(int i)
		{
			SenoneHMMState senoneHMMState = (SenoneHMMState)this.graph.getNode(i).getObject();
			float result;
			if (senoneHMMState != null && senoneHMMState.isEmitting())
			{
				this.componentScores = senoneHMMState.calculateComponentScore(this.curFeature);
				result = senoneHMMState.getScore(this.curFeature);
				if (!BaumWelchLearner.assertionsDisabled && this.componentScores.Length != 1)
				{
					
					throw new AssertionError();
				}
			}
			else
			{
				this.componentScores = null;
				result = 0f;
			}
			return result;
		}

		
		
		public BaumWelchLearner()
		{
		}

		[Throws(new string[]
		{
			"edu.cmu.sphinx.util.props.PropertyException"
		})]
		[LineNumberTable(new byte[]
		{
			17,
			107,
			150,
			118,
			113
		})]
		
		public virtual void newProperties(PropertySheet ps)
		{
			this.logMath = LogMath.getLogMath();
			this.dataSource = (StreamCepstrumSource)ps.getComponent("source");
			this.frontEnd = (FrontEnd)ps.getComponent("frontend");
			this.frontEnd.setDataSource(this.dataSource);
		}

		protected internal virtual FrontEnd getFrontEnd()
		{
			return this.frontEnd;
		}

		public virtual void start()
		{
		}

		public virtual void stop()
		{
		}

		[Throws(new string[]
		{
			"java.io.IOException"
		})]
		[LineNumberTable(new byte[]
		{
			99,
			103,
			103
		})]
		
		public virtual void initializeComputation(Utterance utterance, UtteranceGraph graph)
		{
			this.setUtterance(utterance);
			this.setGraph(graph);
		}

		[LineNumberTable(new byte[]
		{
			160,
			143,
			136,
			108,
			140,
			110,
			108,
			102,
			110,
			125,
			103,
			108,
			104,
			21,
			198,
			112,
			102,
			137,
			116,
			103,
			223,
			26,
			194,
			108
		})]
		
		public virtual TrainerScore[] getScore()
		{
			if (this.scoreArray == null)
			{
				this.scoreArray = this.prepareScore();
				this.currentFeatureIndex = this.lastFeatureIndex;
			}
			this.currentFeatureIndex--;
			if (this.currentFeatureIndex < 0)
			{
				this.scoreArray = (TrainerScore[][])null;
				return null;
			}
			float num = float.MinValue;
			TrainerScore[] array = this.scoreArray[this.currentFeatureIndex];
			if (!BaumWelchLearner.assertionsDisabled && array.Length != this.betas.Length)
			{
				
				throw new AssertionError();
			}
			this.backwardPass(array);
			for (int i = 0; i < this.betas.Length; i++)
			{
				array[i].setGamma();
				num = this.logMath.addAsLinear(num, array[i].getGamma());
			}
			if (this.currentFeatureIndex == this.lastFeatureIndex - 1)
			{
				TrainerScore.setLogLikelihood(num);
				this.totalLogScore = num;
			}
			else if (java.lang.Math.abs(this.totalLogScore - num) > java.lang.Math.abs(this.totalLogScore))
			{
				java.lang.System.@out.println(new StringBuilder().append("WARNING: log probabilities differ: ").append(this.totalLogScore).append(" and ").append(num).toString());
			}
			return array;
		}

		[LineNumberTable(new byte[]
		{
			159,
			177,
			245,
			80,
			101,
			42
		})]
		static BaumWelchLearner()
		{
			BaumWelchLearner.logger = Logger.getLogger("edu.cmu.sphinx.trainer.BaumWelch");
		}

		[S4Component(new object[]
		{
			64,
			"Ledu/cmu/sphinx/util/props/S4Component;",
			"type",
			new object[]
			{
				99,
				"Ledu/cmu/sphinx/frontend/FrontEnd, sphinx4-core-5prealpha-SNAPSHOT, Version=0/0/0/0, Culture=neutral, PublicKeyToken=null;"
			}
		})]
		public const string FRONT_END = "frontend";

		private FrontEnd frontEnd;

		[S4Component(new object[]
		{
			64,
			"Ledu/cmu/sphinx/util/props/S4Component;",
			"type",
			new object[]
			{
				99,
				"Ledu/cmu/sphinx/frontend/util/StreamCepstrumSource, sphinx4-core-5prealpha-SNAPSHOT, Version=0/0/0/0, Culture=neutral, PublicKeyToken=null;"
			}
		})]
		public const string DATA_SOURCE = "source";

		private StreamCepstrumSource dataSource;

		private LogMath logMath;

		private static Logger logger;

		private Data curFeature;

		private UtteranceGraph graph;

		private TrainerScore[][] scoreArray;

		private int lastFeatureIndex;

		private int currentFeatureIndex;

		private float[] betas;

		private float[] outputProbs;

		private float[] componentScores;

		private float[] probCurrentFrame;

		private float totalLogScore;

		
		internal static bool assertionsDisabled = !ClassLiteral<BaumWelchLearner>.Value.desiredAssertionStatus();
	}
}
