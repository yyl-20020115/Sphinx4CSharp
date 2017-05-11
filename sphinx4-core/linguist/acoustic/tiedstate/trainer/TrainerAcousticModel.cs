using edu.cmu.sphinx.util.props;
using java.io;
using java.lang;

namespace edu.cmu.sphinx.linguist.acoustic.tiedstate.trainer
{
	public class TrainerAcousticModel : TiedStateAcousticModel
	{
		public TrainerAcousticModel(Loader loader, UnitManager unitManager, bool useComposites, Saver saver, string saveFormat) : base(loader, unitManager, useComposites)
		{
			this.saver = saver;
			this.hmmPoolManager = new HMMPoolManager(loader);
			this.saveFormat = saveFormat;
			this.logInfo();
		}
		
		public TrainerAcousticModel()
		{
		}
		
		public override void newProperties(PropertySheet ps)
		{
			base.newProperties(ps);
			this.saver = (Saver)ps.getComponent("saver");
			IOException ex2;
			try
			{
				this.hmmPoolManager = new HMMPoolManager(this.loader);
			}
			catch (IOException ex)
			{
				throw new PropertyException(ex);
			}
			this.saveFormat = ps.getString("saveFormat");
			this.logInfo();
		}
		
		public virtual void save(string name)
		{
			this.saver.save(name, true);
			this.logger.info(new StringBuilder().append("saved models with ").append(this.saver).toString());
		}
		
		public virtual void load()
		{
			this.logInfo();
			this.hmmPoolManager = new HMMPoolManager(this.loader);
		}
		
		public virtual void resetBuffers()
		{
			this.hmmPoolManager.resetBuffers();
		}
		
		public virtual void accumulate(int index, TrainerScore[] trainerScore, TrainerScore[] nextTrainerScore)
		{
			this.hmmPoolManager.accumulate(index, trainerScore, nextTrainerScore);
		}
		
		public virtual void accumulate(int index, TrainerScore[] trainerScore)
		{
			this.hmmPoolManager.accumulate(index, trainerScore);
		}
		
		public virtual void updateLogLikelihood()
		{
			this.hmmPoolManager.updateLogLikelihood();
		}
		
		public virtual float normalize()
		{
			float result = this.hmmPoolManager.normalize();
			this.hmmPoolManager.update();
			return result;
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
