using System;

using edu.cmu.sphinx.util.props;
using IKVM.Attributes;
using ikvm.@internal;
using IKVM.Runtime;
using java.io;
using java.lang;
using java.util;

namespace edu.cmu.sphinx.trainer
{
	public class Trainer : java.lang.Object, Configurable
	{
		private void addStage(Stage stage)
		{
			this.StageList.add(stage);
			this.StageNames.add(stage.toString());
		}
	
		private void processStages(string str)
		{
			if (!this.StageNames.contains(this.initialStage) || !this.StageNames.contains(this.finalStage))
			{
				return;
			}
			Iterator iterator = this.StageList.iterator();
			while (iterator.hasNext())
			{
				Stage stage = (Stage)iterator.next();
				if (!this.isStageActive && java.lang.String.instancehelper_equals(this.initialStage, stage.toString()))
				{
					this.isStageActive = true;
				}
				if (this.isStageActive)
				{
					IOException ex2;
					try
					{
						if (stage.equals(Stage.___00_INITIALIZATION))
						{
							java.lang.System.@out.println("00 - Initializing");
							this.trainManager.initializeModels(str);
							java.lang.System.@out.println("Saving");
							this.trainManager.saveModels(str);
						}
						else if (stage.equals(Stage.___10_CI_TRAIN))
						{
							java.lang.System.@out.println("01 - CI train");
							this.trainManager.trainContextIndependentModels(str);
							java.lang.System.@out.println("Saving");
							this.trainManager.saveModels(str);
						}
						else if (stage.equals(Stage.___20_UNTIED_CD_TRAIN))
						{
							java.lang.System.@out.println("02 - Untied CD train");
						}
						else if (stage.equals(Stage.___30_STATE_PRUNING))
						{
							java.lang.System.@out.println("03 - State pruning");
						}
						else if (stage.equals(Stage.___40_TIED_CD_TRAIN))
						{
							java.lang.System.@out.println("04 - Tied CD train");
						}
						else if (stage.equals(Stage.___90_CP_MODEL))
						{
							java.lang.System.@out.println("Copying");
							this.trainManager.copyModels(str);
						}
						else if (!Trainer.assertionsDisabled)
						{
							object obj = "stage not implemented";
							
							throw new AssertionError(obj);
						}
					}
					catch (IOException ex)
					{
						ex2 = ByteCodeHelper.MapException<IOException>(ex, 1);
						goto IL_1B2;
					}
					goto IL_1B0;
					IL_1B2:
					IOException ex3 = ex2;
					Throwable.instancehelper_printStackTrace(ex3);
					string text = new StringBuilder().append("IOE: Can't finish trainer ").append(ex3).toString();					Exception ex4 = ex3;
					
					throw new Error(text, ex3);
					IL_1B0:
					if (java.lang.String.instancehelper_equals(this.finalStage, stage.toString()))
					{
						this.isStageActive = false;
					}
				}
			}
		}

		[LineNumberTable(new byte[]
		{
			159,
			174,
			232,
			80,
			107
		})]
		
		public Trainer()
		{
			this.StageList = new LinkedList();
			this.StageNames = new HashSet();
		}

		[Throws(new string[]
		{
			"edu.cmu.sphinx.util.props.PropertyException"
		})]
		[LineNumberTable(new byte[]
		{
			4,
			150,
			113,
			145,
			107,
			107,
			107,
			107,
			107,
			107
		})]
		
		public virtual void newProperties(PropertySheet ps)
		{
			this.trainManager = (TrainManager)ps.getComponent("trainManager");
			this.initialStage = ps.getString("initialStage");
			this.finalStage = ps.getString("finalStage");
			this.addStage(Stage.___00_INITIALIZATION);
			this.addStage(Stage.___10_CI_TRAIN);
			this.addStage(Stage.___20_UNTIED_CD_TRAIN);
			this.addStage(Stage.___30_STATE_PRUNING);
			this.addStage(Stage.___40_TIED_CD_TRAIN);
			this.addStage(Stage.___90_CP_MODEL);
		}

		[LineNumberTable(new byte[]
		{
			97,
			101,
			106,
			101,
			134,
			134,
			101,
			132,
			103,
			109,
			135
		})]
		
		public static void main(string[] argv)
		{
			if (argv.Length > 1)
			{
				java.lang.System.@out.println("Usage: Trainer [config]");
				java.lang.System.exit(1);
			}
			string text = "trainer";
			if (argv.Length == 1)
			{
				string configFileName = argv[0];
				ConfigurationManager configurationManager = new ConfigurationManager(configFileName);
				Trainer trainer = (Trainer)configurationManager.lookup(text);
				trainer.processStages(text);
			}
		}

		
		static Trainer()
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
				"Ledu/cmu/sphinx/trainer/TrainManager, sphinx4-core-5prealpha-SNAPSHOT, Version=0/0/0/0, Culture=neutral, PublicKeyToken=null;"
			}
		})]
		public const string TRAIN_MANAGER = "trainManager";

		[S4String(new object[]
		{
			64,
			"Ledu/cmu/sphinx/util/props/S4String;",
			"defaultValue",
			"_00_INITIALIZATION"
		})]
		public const string PROP_INITIAL_STAGE = "initialStage";

		[S4String(new object[]
		{
			64,
			"Ledu/cmu/sphinx/util/props/S4String;",
			"defaultValue",
			"_40_TIED_CD_TRAIN"
		})]
		public const string PROP_FINAL_STAGE = "finalStage";

		private string initialStage;

		private string finalStage;

		private bool isStageActive;

		
		private List StageList;

		
		private Set StageNames;

		private TrainManager trainManager;

		
		internal static bool assertionsDisabled = !ClassLiteral<Trainer>.Value.desiredAssertionStatus();
	}
}
