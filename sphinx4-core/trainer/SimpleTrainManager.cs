using System;

using edu.cmu.sphinx.linguist.acoustic;
using edu.cmu.sphinx.linguist.acoustic.tiedstate.trainer;
using edu.cmu.sphinx.util;
using edu.cmu.sphinx.util.props;
using IKVM.Attributes;
using ikvm.@internal;
using java.lang;
using java.util;

namespace edu.cmu.sphinx.trainer
{
	[Implements(new string[]
	{
		"edu.cmu.sphinx.trainer.TrainManager"
	})]
	public class SimpleTrainManager : java.lang.Object, TrainManager, Configurable
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
			86,
			139,
			127,
			1,
			102,
			98,
			139
		})]
		
		private void loadModels(string text)
		{
			this.dumpMemoryInfo("TrainManager start");
			Iterator iterator = this.acousticModels.iterator();
			while (iterator.hasNext())
			{
				TrainerAcousticModel trainerAcousticModel = (TrainerAcousticModel)iterator.next();
				trainerAcousticModel.load();
			}
			this.dumpMemoryInfo("acoustic model");
		}

		[Throws(new string[]
		{
			"java.io.IOException"
		})]
		[LineNumberTable(new byte[]
		{
			66,
			110,
			153,
			127,
			1,
			104,
			135,
			140,
			130
		})]
		
		public virtual void saveModels(string context)
		{
			if (1 == this.acousticModels.size())
			{
				((TrainerAcousticModel)this.acousticModels.get(0)).save(null);
			}
			else
			{
				Iterator iterator = this.acousticModels.iterator();
				while (iterator.hasNext())
				{
					AcousticModel acousticModel = (AcousticModel)iterator.next();
					if (acousticModel is TrainerAcousticModel)
					{
						TrainerAcousticModel trainerAcousticModel = (TrainerAcousticModel)acousticModel;
						trainerAcousticModel.save(acousticModel.getName());
					}
				}
			}
		}

		[LineNumberTable(new byte[]
		{
			160,
			140,
			104,
			134
		})]
		
		private void dumpMemoryInfo(string msg)
		{
			if (this.dumpMemoryInfo)
			{
				Utilities.dumpMemoryInfo(msg);
			}
		}

		
		
		public SimpleTrainManager()
		{
		}

		[Throws(new string[]
		{
			"edu.cmu.sphinx.util.props.PropertyException"
		})]
		[LineNumberTable(new byte[]
		{
			17,
			118,
			118,
			118,
			118,
			113,
			113,
			118,
			118
		})]
		
		public virtual void newProperties(PropertySheet ps)
		{
			this.dumpMemoryInfo = ps.getBoolean("dumpMemoryInfo").booleanValue();
			this.learner = (Learner)ps.getComponent("learner");
			this.controlFile = (ControlFile)ps.getComponent("control");
			this.initLearner = (Learner)ps.getComponent("initLearner");
			this.minimumImprovement = ps.getFloat("minimumImprovement");
			this.maxIteration = ps.getInt("maximumIteration");
			this.acousticModels = ps.getComponentList("models", ClassLiteral<TrainerAcousticModel>.Value);
			this.unitManager = (UnitManager)ps.getComponent("unitManager");
		}

		[LineNumberTable(new byte[]
		{
			30,
			122,
			107,
			109,
			108,
			107,
			102,
			104,
			146,
			98
		})]
		
		public virtual void train()
		{
			if (!SimpleTrainManager.assertionsDisabled && this.controlFile == null)
			{
				
				throw new AssertionError();
			}
			this.controlFile.startUtteranceIterator();
			while (this.controlFile.hasMoreUtterances())
			{
				Utterance utterance = this.controlFile.nextUtterance();
				java.lang.System.@out.println(utterance);
				utterance.startTranscriptIterator();
				while (utterance.hasMoreTranscripts())
				{
					java.lang.System.@out.println(utterance.nextTranscript());
				}
			}
		}

		[Throws(new string[]
		{
			"java.io.IOException"
		})]
		[LineNumberTable(new byte[]
		{
			54,
			103,
			103
		})]
		
		public virtual void copyModels(string context)
		{
			this.loadModels(context);
			this.saveModels(context);
		}

		[Throws(new string[]
		{
			"java.io.IOException"
		})]
		[LineNumberTable(new byte[]
		{
			104,
			139,
			159,
			4,
			107,
			109,
			108,
			108,
			111,
			119,
			138,
			162,
			103,
			101,
			107
		})]
		
		public virtual void initializeModels(string context)
		{
			this.dumpMemoryInfo("TrainManager start");
			Iterator iterator = this.acousticModels.iterator();
			while (iterator.hasNext())
			{
				TrainerAcousticModel trainerAcousticModel = (TrainerAcousticModel)iterator.next();
				this.controlFile.startUtteranceIterator();
				while (this.controlFile.hasMoreUtterances())
				{
					Utterance utterance = this.controlFile.nextUtterance();
					this.initLearner.setUtterance(utterance);
					TrainerScore[] score;
					while ((score = this.initLearner.getScore()) != null)
					{
						if (!SimpleTrainManager.assertionsDisabled && score.Length != 1)
						{
							
							throw new AssertionError();
						}
						trainerAcousticModel.accumulate(0, score);
					}
				}
				trainerAcousticModel.normalize();
			}
			this.dumpMemoryInfo("acoustic model");
		}

		[Throws(new string[]
		{
			"java.io.IOException"
		})]
		[LineNumberTable(new byte[]
		{
			160,
			76,
			104,
			199,
			139,
			159,
			4,
			102,
			102,
			99,
			185,
			127,
			6,
			102,
			107,
			112,
			109,
			150,
			109,
			109,
			99,
			112,
			106,
			101,
			142,
			234,
			60,
			232,
			71,
			134,
			102,
			101,
			104,
			127,
			6,
			103,
			104,
			104,
			178,
			101,
			134,
			223,
			22,
			227,
			26,
			235,
			104,
			101
		})]
		
		public virtual void trainContextIndependentModels(string context)
		{
			if (this.learner == null)
			{
				this.loadModels(context);
			}
			this.dumpMemoryInfo("TrainManager start");
			Iterator iterator = this.acousticModels.iterator();
			while (iterator.hasNext())
			{
				TrainerAcousticModel trainerAcousticModel = (TrainerAcousticModel)iterator.next();
				float num = float.MaxValue;
				float num2 = 100f;
				int num3 = 0;
				while (num3 < this.maxIteration && num2 > this.minimumImprovement)
				{
					java.lang.System.@out.println(new StringBuilder().append("Iteration: ").append(num3).toString());
					trainerAcousticModel.resetBuffers();
					this.controlFile.startUtteranceIterator();
					while (this.controlFile.hasMoreUtterances())
					{
						Utterance utterance = this.controlFile.nextUtterance();
						UtteranceHMMGraph.__<clinit>();
						UtteranceHMMGraph graph = new UtteranceHMMGraph(context, utterance, trainerAcousticModel, this.unitManager);
						this.learner.setUtterance(utterance);
						this.learner.setGraph(graph);
						TrainerScore[] nextTrainerScore = null;
						TrainerScore[] score;
						while ((score = this.learner.getScore()) != null)
						{
							for (int i = 0; i < score.Length; i++)
							{
								if (i > 0)
								{
									trainerAcousticModel.accumulate(i, score, nextTrainerScore);
								}
								else
								{
									trainerAcousticModel.accumulate(i, score);
								}
							}
							nextTrainerScore = score;
						}
						trainerAcousticModel.updateLogLikelihood();
					}
					float num4 = trainerAcousticModel.normalize();
					java.lang.System.@out.println(new StringBuilder().append("Loglikelihood: ").append(num4).toString());
					this.saveModels(context);
					if (num3 > 0)
					{
						if (num != 0f)
						{
							num2 = (num4 - num) / num * 100f;
						}
						else if (num == num4)
						{
							num2 = 0f;
						}
						java.lang.System.@out.println(new StringBuilder().append("Finished iteration: ").append(num3).append(" - Improvement: ").append(num2).toString());
					}
					num = num4;
					num3++;
				}
			}
		}

		
		static SimpleTrainManager()
		{
		}

		[S4Component(new object[]
		{
			64,
			"Ledu/cmu/sphinx/util/props/S4Component;",
			"type",
			new object[]
			{
				99,
				"Ledu/cmu/sphinx/trainer/ControlFile, sphinx4-core-5prealpha-SNAPSHOT, Version=0/0/0/0, Culture=neutral, PublicKeyToken=null;"
			}
		})]
		public const string CONTROL_FILE = "control";

		private ControlFile controlFile;

		private bool dumpMemoryInfo;

		[S4Component(new object[]
		{
			64,
			"Ledu/cmu/sphinx/util/props/S4Component;",
			"type",
			new object[]
			{
				99,
				"Ledu/cmu/sphinx/trainer/Learner, sphinx4-core-5prealpha-SNAPSHOT, Version=0/0/0/0, Culture=neutral, PublicKeyToken=null;"
			}
		})]
		public const string LEARNER = "learner";

		private Learner learner;

		[S4Component(new object[]
		{
			64,
			"Ledu/cmu/sphinx/util/props/S4Component;",
			"type",
			new object[]
			{
				99,
				"Ledu/cmu/sphinx/trainer/Learner, sphinx4-core-5prealpha-SNAPSHOT, Version=0/0/0/0, Culture=neutral, PublicKeyToken=null;"
			}
		})]
		public const string INIT_LEARNER = "initLearner";

		private Learner initLearner;

		[S4Component(new object[]
		{
			64,
			"Ledu/cmu/sphinx/util/props/S4Component;",
			"type",
			new object[]
			{
				99,
				"Ledu/cmu/sphinx/linguist/acoustic/UnitManager, sphinx4-core-5prealpha-SNAPSHOT, Version=0/0/0/0, Culture=neutral, PublicKeyToken=null;"
			}
		})]
		public const string UNIT_MANAGER = "unitManager";

		private UnitManager unitManager;

		[S4ComponentList(new object[]
		{
			64,
			"Ledu/cmu/sphinx/util/props/S4ComponentList;",
			"type",
			new object[]
			{
				99,
				"Ledu/cmu/sphinx/linguist/acoustic/tiedstate/trainer/TrainerAcousticModel, sphinx4-core-5prealpha-SNAPSHOT, Version=0/0/0/0, Culture=neutral, PublicKeyToken=null;"
			}
		})]
		public const string AM_COLLECTION = "models";

		
		private List acousticModels;

		[S4Boolean(new object[]
		{
			64,
			"Ledu/cmu/sphinx/util/props/S4Boolean;",
			"defaultValue",
			false
		})]
		public const string DUMP_MEMORY_INFO = "dumpMemoryInfo";

		private int maxIteration;

		private float minimumImprovement;

		
		internal static bool assertionsDisabled = !ClassLiteral<SimpleTrainManager>.Value.desiredAssertionStatus();
	}
}
