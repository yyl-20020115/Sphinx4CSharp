using System;

using edu.cmu.sphinx.frontend.denoise;
using edu.cmu.sphinx.frontend.frequencywarp;
using edu.cmu.sphinx.frontend.transform;
using edu.cmu.sphinx.linguist.acoustic.tiedstate;
using edu.cmu.sphinx.util.props;
using IKVM.Attributes;
using ikvm.@internal;
using IKVM.Runtime;
using java.io;
using java.lang;
using java.util;

namespace edu.cmu.sphinx.frontend
{
	public class AutoCepstrum : BaseDataProcessor
	{
		[LineNumberTable(new byte[]
		{
			77,
			108,
			139,
			114,
			114,
			209,
			109,
			208,
			206,
			146,
			115,
			116,
			122,
			106,
			111,
			111,
			106,
			111,
			111,
			106,
			111,
			111,
			106,
			111,
			111,
			106,
			111,
			111,
			106,
			111,
			111,
			106,
			143,
			114,
			49,
			133,
			178,
			115,
			113,
			172,
			111,
			106,
			116,
			115,
			145,
			172,
			111,
			106,
			145,
			140,
			111,
			106,
			143,
			114,
			49,
			133,
			146,
			109,
			103,
			42,
			143,
			114,
			49,
			133,
			146,
			118,
			47,
			217,
			2,
			98,
			141
		})]
		
		private void initDataProcessors()
		{
			NoSuchFieldException ex2;
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
				if (properties.get("-remove_noise") == null || java.lang.Object.instancehelper_equals(properties.get("-remove_noise"), "yes"))
				{
					this.denoise = new Denoise(((S4Double)ClassLiteral<Denoise>.Value.getField("LAMBDA_POWER", AutoCepstrum.__<GetCallerID>()).getAnnotation(ClassLiteral<S4Double>.Value)).defaultValue(), ((S4Double)ClassLiteral<Denoise>.Value.getField("LAMBDA_A", AutoCepstrum.__<GetCallerID>()).getAnnotation(ClassLiteral<S4Double>.Value)).defaultValue(), ((S4Double)ClassLiteral<Denoise>.Value.getField("LAMBDA_B", AutoCepstrum.__<GetCallerID>()).getAnnotation(ClassLiteral<S4Double>.Value)).defaultValue(), ((S4Double)ClassLiteral<Denoise>.Value.getField("LAMBDA_T", AutoCepstrum.__<GetCallerID>()).getAnnotation(ClassLiteral<S4Double>.Value)).defaultValue(), ((S4Double)ClassLiteral<Denoise>.Value.getField("MU_T", AutoCepstrum.__<GetCallerID>()).getAnnotation(ClassLiteral<S4Double>.Value)).defaultValue(), ((S4Double)ClassLiteral<Denoise>.Value.getField("MAX_GAIN", AutoCepstrum.__<GetCallerID>()).getAnnotation(ClassLiteral<S4Double>.Value)).defaultValue(), ((S4Integer)ClassLiteral<Denoise>.Value.getField("SMOOTH_WINDOW", AutoCepstrum.__<GetCallerID>()).getAnnotation(ClassLiteral<S4Integer>.Value)).defaultValue());
					this.denoise.setPredecessor((DataProcessor)this.selectedDataProcessors.get(this.selectedDataProcessors.size() - 1));
					this.selectedDataProcessors.add(this.denoise);
				}
				if (properties.get("-transform") != null && java.lang.Object.instancehelper_equals(properties.get("-transform"), "dct"))
				{
					this.dct = new DiscreteCosineTransform2(num, ((S4Integer)ClassLiteral<DiscreteCosineTransform>.Value.getField("PROP_CEPSTRUM_LENGTH", AutoCepstrum.__<GetCallerID>()).getAnnotation(ClassLiteral<S4Integer>.Value)).defaultValue());
				}
				else if (properties.get("-transform") != null && java.lang.Object.instancehelper_equals(properties.get("-transform"), "kaldi"))
				{
					this.dct = new KaldiDiscreteCosineTransform(num, ((S4Integer)ClassLiteral<DiscreteCosineTransform>.Value.getField("PROP_CEPSTRUM_LENGTH", AutoCepstrum.__<GetCallerID>()).getAnnotation(ClassLiteral<S4Integer>.Value)).defaultValue());
				}
				else
				{
					this.dct = new DiscreteCosineTransform(num, ((S4Integer)ClassLiteral<DiscreteCosineTransform>.Value.getField("PROP_CEPSTRUM_LENGTH", AutoCepstrum.__<GetCallerID>()).getAnnotation(ClassLiteral<S4Integer>.Value)).defaultValue());
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
				ex2 = ByteCodeHelper.MapException<NoSuchFieldException>(ex, 1);
				goto IL_3D3;
			}
			return;
			IL_3D3:
			NoSuchFieldException ex3 = ex2;
			Exception ex4 = ex3;
			
			throw new RuntimeException(ex4);
		}

		[LineNumberTable(new byte[]
		{
			160,
			152,
			112,
			102,
			127,
			1,
			116,
			110
		})]
		
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

		[Throws(new string[]
		{
			"java.io.IOException"
		})]
		[LineNumberTable(new byte[]
		{
			47,
			104,
			102,
			103,
			102,
			102
		})]
		
		public AutoCepstrum(Loader loader)
		{
			this.initLogger();
			this.loader = loader;
			loader.load();
			this.initDataProcessors();
		}

		[LineNumberTable(new byte[]
		{
			54,
			102
		})]
		
		public AutoCepstrum()
		{
		}

		[Throws(new string[]
		{
			"edu.cmu.sphinx.util.props.PropertyException"
		})]
		[LineNumberTable(new byte[]
		{
			65,
			103,
			150,
			189,
			2,
			97,
			140,
			102
		})]
		
		public override void newProperties(PropertySheet ps)
		{
			base.newProperties(ps);
			this.loader = (Loader)ps.getComponent("loader");
			IOException ex2;
			try
			{
				this.loader.load();
			}
			catch (IOException ex)
			{
				ex2 = ByteCodeHelper.MapException<IOException>(ex, 1);
				goto IL_37;
			}
			this.initDataProcessors();
			return;
			IL_37:
			IOException ex3 = ex2;
			Exception e = ex3;
			
			throw new PropertyException(e);
		}

		[LineNumberTable(new byte[]
		{
			160,
			112,
			134,
			127,
			1,
			104
		})]
		
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

		[Throws(new string[]
		{
			"edu.cmu.sphinx.frontend.DataProcessingException"
		})]
		[LineNumberTable(new byte[]
		{
			160,
			128,
			126
		})]
		
		public override Data getData()
		{
			DataProcessor dataProcessor = (DataProcessor)this.selectedDataProcessors.get(this.selectedDataProcessors.size() - 1);
			return dataProcessor.getData();
		}

		[LineNumberTable(new byte[]
		{
			160,
			140,
			108
		})]
		
		public override void setPredecessor(DataProcessor predecessor)
		{
			this.filterBank.setPredecessor(predecessor);
		}

		private static CallerID __<GetCallerID>()
		{
			if (AutoCepstrum.__<callerID> == null)
			{
				AutoCepstrum.__<callerID> = new AutoCepstrum.__<CallerID>();
			}
			return AutoCepstrum.__<callerID>;
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

		private static CallerID __<callerID>;

		private sealed class __<CallerID> : CallerID
		{
			internal __<CallerID>()
			{
			}
		}
	}
}
