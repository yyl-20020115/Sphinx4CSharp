using edu.cmu.sphinx.frontend.denoise;
using edu.cmu.sphinx.frontend.frequencywarp;
using edu.cmu.sphinx.frontend.transform;
using edu.cmu.sphinx.linguist.acoustic.tiedstate;
using edu.cmu.sphinx.util.props;
using ikvm.@internal;
using java.io;
using java.lang;
using java.util;

namespace edu.cmu.sphinx.frontend
{
	public class AutoCepstrum : BaseDataProcessor
	{
		private void initDataProcessors()
		{
			try
			{
				Properties properties = this.loader.getProperties();
				this.selectedDataProcessors = new ArrayList();
				double minFreq = Double.parseDouble(properties.getProperty("-lowerf"));
				double maxFreq = Double.parseDouble(properties.getProperty("-upperf"));
				int num = Integer.parseInt(properties.getProperty("-nfilt"));
				if (this.loader is KaldiLoader)
				{
					this.filterBank = new MelFrequencyFilterBank2(minFreq, maxFreq, num);
				}
				else
				{
					this.filterBank = new MelFrequencyFilterBank(minFreq, maxFreq, num);
				}
				this.selectedDataProcessors.add(this.filterBank);
				if (properties.get("-remove_noise") == null || Object.instancehelper_equals(properties.get("-remove_noise"), "yes"))
				{
					this.denoise = new Denoise(((S4Double)ClassLiteral<Denoise>.Value.getField("LAMBDA_POWER", AutoCepstrum.__GetCallerID()).getAnnotation(ClassLiteral<S4Double>.Value)).defaultValue(), ((S4Double)ClassLiteral<Denoise>.Value.getField("LAMBDA_A", AutoCepstrum.__GetCallerID()).getAnnotation(ClassLiteral<S4Double>.Value)).defaultValue(), ((S4Double)ClassLiteral<Denoise>.Value.getField("LAMBDA_B", AutoCepstrum.__GetCallerID()).getAnnotation(ClassLiteral<S4Double>.Value)).defaultValue(), ((S4Double)ClassLiteral<Denoise>.Value.getField("LAMBDA_T", AutoCepstrum.__GetCallerID()).getAnnotation(ClassLiteral<S4Double>.Value)).defaultValue(), ((S4Double)ClassLiteral<Denoise>.Value.getField("MU_T", AutoCepstrum.__GetCallerID()).getAnnotation(ClassLiteral<S4Double>.Value)).defaultValue(), ((S4Double)ClassLiteral<Denoise>.Value.getField("MAX_GAIN", AutoCepstrum.__GetCallerID()).getAnnotation(ClassLiteral<S4Double>.Value)).defaultValue(), ((S4Integer)ClassLiteral<Denoise>.Value.getField("SMOOTH_WINDOW", AutoCepstrum.__GetCallerID()).getAnnotation(ClassLiteral<S4Integer>.Value)).defaultValue());
					this.denoise.setPredecessor((DataProcessor)this.selectedDataProcessors.get(this.selectedDataProcessors.size() - 1));
					this.selectedDataProcessors.add(this.denoise);
				}
				if (properties.get("-transform") != null && Object.instancehelper_equals(properties.get("-transform"), "dct"))
				{
					this.dct = new DiscreteCosineTransform2(num, ((S4Integer)ClassLiteral<DiscreteCosineTransform>.Value.getField("PROP_CEPSTRUM_LENGTH", AutoCepstrum.__GetCallerID()).getAnnotation(ClassLiteral<S4Integer>.Value)).defaultValue());
				}
				else if (properties.get("-transform") != null && Object.instancehelper_equals(properties.get("-transform"), "kaldi"))
				{
					this.dct = new KaldiDiscreteCosineTransform(num, ((S4Integer)ClassLiteral<DiscreteCosineTransform>.Value.getField("PROP_CEPSTRUM_LENGTH", AutoCepstrum.__GetCallerID()).getAnnotation(ClassLiteral<S4Integer>.Value)).defaultValue());
				}
				else
				{
					this.dct = new DiscreteCosineTransform(num, ((S4Integer)ClassLiteral<DiscreteCosineTransform>.Value.getField("PROP_CEPSTRUM_LENGTH", AutoCepstrum.__GetCallerID()).getAnnotation(ClassLiteral<S4Integer>.Value)).defaultValue());
				}
				this.dct.setPredecessor((DataProcessor)this.selectedDataProcessors.get(this.selectedDataProcessors.size() - 1));
				this.selectedDataProcessors.add(this.dct);
				if (properties.get("-lifter") != null)
				{
					this.lifter = new Lifter(Integer.parseInt((string)properties.get("-lifter")));
					this.lifter.setPredecessor((DataProcessor)this.selectedDataProcessors.get(this.selectedDataProcessors.size() - 1));
					this.selectedDataProcessors.add(this.lifter);
				}
				this.logger.info(new StringBuilder().append("Cepstrum component auto-configured as follows: ").append(this.toString()).toString());
			}
			catch (NoSuchFieldException ex)
			{
				throw new RuntimeException(ex);
			}
		}
		
		public override string toString()
		{
			StringBuilder stringBuilder = new StringBuilder(base.toString()).append(" {");
			Iterator iterator = this.selectedDataProcessors.iterator();
			while (iterator.hasNext())
			{
				DataProcessor dataProcessor = (DataProcessor)iterator.next();
				stringBuilder.append(dataProcessor).append(", ");
			}
			stringBuilder.setLength(stringBuilder.length() - 2);
			return stringBuilder.append('}').toString();
		}
		
		public AutoCepstrum(Loader loader)
		{
			this.initLogger();
			this.loader = loader;
			loader.load();
			this.initDataProcessors();
		}
		
		public AutoCepstrum()
		{
		}
		
		public override void newProperties(PropertySheet ps)
		{
			base.newProperties(ps);
			this.loader = (Loader)ps.getComponent("loader");
			try
			{
				this.loader.load();
			}
			catch (IOException ex)
			{
				throw new PropertyException(ex);
			}
			this.initDataProcessors();
		}

		public override void initialize()
		{
			base.initialize();
			Iterator iterator = this.selectedDataProcessors.iterator();
			while (iterator.hasNext())
			{
				DataProcessor dataProcessor = (DataProcessor)iterator.next();
				dataProcessor.initialize();
			}
		}
		
		public override Data getData()
		{
			DataProcessor dataProcessor = (DataProcessor)this.selectedDataProcessors.get(this.selectedDataProcessors.size() - 1);
			return dataProcessor.getData();
		}
	
		public override void setPredecessor(DataProcessor predecessor)
		{
			this.filterBank.setPredecessor(predecessor);
		}

		private static CallerID __GetCallerID()
		{
			if (AutoCepstrum.__callerID == null)
			{
				AutoCepstrum.__callerID = new AutoCepstrum.__CallerID();
			}
			return AutoCepstrum.__callerID;
		}

		[S4Component(new object[]
		{
			64,
			"Ledu/cmu/sphinx/util/props/S4Component;",
			"type",
			new object[]
			{
				99,
				"Ledu/cmu/sphinx/linguist/acoustic/tiedstate/Loader, sphinx4-core-5prealpha-SNAPSHOT, Version=0/0/0/0, Culture=neutral, PublicKeyToken=null;"
			}
		})]
		public const string PROP_LOADER = "loader";

		protected internal Loader loader;

		protected internal BaseDataProcessor filterBank;

		protected internal Denoise denoise;

		protected internal DiscreteCosineTransform dct;

		protected internal Lifter lifter;

		
		protected internal List selectedDataProcessors;

		private static CallerID __callerID;

		private sealed class __CallerID : CallerID
		{
			internal __CallerID()
			{
			}
		}
	}
}
