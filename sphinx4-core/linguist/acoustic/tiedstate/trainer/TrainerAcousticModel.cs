using System;

using edu.cmu.sphinx.util.props;
using IKVM.Attributes;
using IKVM.Runtime;
using java.io;
using java.lang;

namespace edu.cmu.sphinx.linguist.acoustic.tiedstate.trainer
{
	public class TrainerAcousticModel : TiedStateAcousticModel
	{
		
		public new static void __<clinit>()
		{
		}

		[Throws(new string[]
		{
			"java.io.IOException"
		})]
		[LineNumberTable(new byte[]
		{
			159,
			128,
			130,
			139,
			104,
			108,
			136,
			102
		})]
		
		public TrainerAcousticModel(Loader loader, UnitManager unitManager, bool useComposites, Saver saver, string saveFormat) : base(loader, unitManager, useComposites)
		{
			this.saver = saver;
			this.hmmPoolManager = new HMMPoolManager(loader);
			this.saveFormat = saveFormat;
			this.logInfo();
		}

		[LineNumberTable(new byte[]
		{
			17,
			102
		})]
		
		public TrainerAcousticModel()
		{
		}

		[Throws(new string[]
		{
			"edu.cmu.sphinx.util.props.PropertyException"
		})]
		[LineNumberTable(new byte[]
		{
			22,
			135,
			150,
			191,
			9,
			2,
			97,
			140,
			145,
			102
		})]
		
		public override void newProperties(PropertySheet ps)
		{
			base.newProperties(ps);
			this.saver = (Saver)ps.getComponent("saver");
			IOException ex2;
			try
			{
				HMMPoolManager.__<clinit>();
				this.hmmPoolManager = new HMMPoolManager(this.loader);
			}
			catch (IOException ex)
			{
				ex2 = ByteCodeHelper.MapException<IOException>(ex, 1);
				goto IL_42;
			}
			this.saveFormat = ps.getString("saveFormat");
			this.logInfo();
			return;
			IL_42:
			IOException ex3 = ex2;
			Exception e = ex3;
			
			throw new PropertyException(e);
		}

		[Throws(new string[]
		{
			"java.io.IOException"
		})]
		[LineNumberTable(new byte[]
		{
			46,
			109,
			127,
			11
		})]
		
		public virtual void save(string name)
		{
			this.saver.save(name, true);
			this.logger.info(new StringBuilder().append("saved models with ").append(this.saver).toString());
		}

		[Throws(new string[]
		{
			"java.io.IOException",
			"java.io.FileNotFoundException"
		})]
		[LineNumberTable(new byte[]
		{
			61,
			102,
			118
		})]
		
		public virtual void load()
		{
			this.logInfo();
			HMMPoolManager.__<clinit>();
			this.hmmPoolManager = new HMMPoolManager(this.loader);
		}

		[LineNumberTable(new byte[]
		{
			68,
			107
		})]
		
		public virtual void resetBuffers()
		{
			this.hmmPoolManager.resetBuffers();
		}

		[LineNumberTable(new byte[]
		{
			83,
			110
		})]
		
		public virtual void accumulate(int index, TrainerScore[] trainerScore, TrainerScore[] nextTrainerScore)
		{
			this.hmmPoolManager.accumulate(index, trainerScore, nextTrainerScore);
		}

		[LineNumberTable(new byte[]
		{
			95,
			109
		})]
		
		public virtual void accumulate(int index, TrainerScore[] trainerScore)
		{
			this.hmmPoolManager.accumulate(index, trainerScore);
		}

		[LineNumberTable(new byte[]
		{
			103,
			107
		})]
		
		public virtual void updateLogLikelihood()
		{
			this.hmmPoolManager.updateLogLikelihood();
		}

		[LineNumberTable(new byte[]
		{
			112,
			108,
			107
		})]
		
		public virtual float normalize()
		{
			float result = this.hmmPoolManager.normalize();
			this.hmmPoolManager.update();
			return result;
		}

		
		static TrainerAcousticModel()
		{
			TiedStateAcousticModel.__<clinit>();
		}

		[S4Component(new object[]
		{
			64,
			"Ledu/cmu/sphinx/util/props/S4Component;",
			"type",
			new object[]
			{
				99,
				"Ledu/cmu/sphinx/linguist/acoustic/tiedstate/Saver, sphinx4-core-5prealpha-SNAPSHOT, Version=0/0/0/0, Culture=neutral, PublicKeyToken=null;"
			}
		})]
		public const string SAVER = "saver";

		private Saver saver;

		[S4Double(new object[]
		{
			64,
			"Ledu/cmu/sphinx/util/props/S4Double;",
			"defaultValue",
			9.9999997473787516E-05
		})]
		public const string PROP_VARIANCE_FLOOR = "varianceFloor";

		[S4Double(new object[]
		{
			64,
			"Ledu/cmu/sphinx/util/props/S4Double;",
			"defaultValue",
			0.0
		})]
		public const string PROP_MC_FLOOR = "MixtureComponentScoreFloor";

		[S4Double(new object[]
		{
			64,
			"Ledu/cmu/sphinx/util/props/S4Double;",
			"defaultValue",
			1.0000000116860974E-07
		})]
		public const string PROP_MW_FLOOR = "mixtureWeightFloor";

		[S4String(new object[]
		{
			64,
			"Ledu/cmu/sphinx/util/props/S4String;",
			"defaultValue",
			"sphinx3.binary"
		})]
		public const string PROP_FORMAT_SAVE = "saveFormat";

		public string saveFormat;

		public const int ALL_MODELS = -1;

		private HMMPoolManager hmmPoolManager;
	}
}
