using System;

using edu.cmu.sphinx.frontend;
using edu.cmu.sphinx.frontend.util;
using edu.cmu.sphinx.recognizer;
using edu.cmu.sphinx.result;
using edu.cmu.sphinx.util;
using edu.cmu.sphinx.util.props;
using IKVM.Attributes;
using ikvm.@internal;
using IKVM.Runtime;
using java.io;
using java.lang;
using java.net;
using java.util;
using java.util.logging;
using javax.sound.sampled;

namespace edu.cmu.sphinx.tools.batch
{
	[Implements(new string[]
	{
		"edu.cmu.sphinx.util.props.Configurable"
	})]
	public class BatchModeRecognizer : java.lang.Object, Configurable
	{
		[Throws(new string[]
		{
			"java.io.IOException"
		})]
		[LineNumberTable(new byte[]
		{
			160,
			76,
			104,
			153,
			190
		})]
		
		public virtual void setBatchFile(string batchFile)
		{
			if (this.usePooledBatchManager)
			{
				PooledBatchManager.__<clinit>();
				this.batchManager = new PooledBatchManager(batchFile, this.skip);
			}
			else
			{
				this.batchManager = new SimpleBatchManager(batchFile, this.skip, this.whichBatch, this.totalBatches);
			}
		}

		[Throws(new string[]
		{
			"java.io.IOException"
		})]
		[LineNumberTable(new byte[]
		{
			160,
			126,
			191,
			4,
			103,
			103,
			111,
			242,
			75,
			226,
			54,
			97,
			127,
			11,
			101,
			167,
			114,
			112,
			178,
			104,
			110,
			104,
			97,
			103,
			136,
			138,
			106,
			136,
			136,
			101
		})]
		
		internal virtual void setInputStream(string text)
		{
			Iterator iterator = this.inputDataProcessors.iterator();
			while (iterator.hasNext())
			{
				DataProcessor dataProcessor = (DataProcessor)iterator.next();
				InputStream inputStream;
				try
				{
					File file = new File(text);
					this.logger.info(AudioSystem.getAudioFileFormat(file).toString());
					inputStream = AudioSystem.getAudioInputStream(file);
				}
				catch (UnsupportedAudioFileException ex)
				{
					goto IL_4F;
				}
				IL_BF:
				if (dataProcessor is StreamDataSource)
				{
					((StreamDataSource)dataProcessor).setInputStream(inputStream);
				}
				else if (dataProcessor is StreamCepstrumSource)
				{
					int bigEndian = Utilities.isCepstraFileBigEndian(text) ? 1 : 0;
					StreamCepstrumSource streamCepstrumSource = (StreamCepstrumSource)dataProcessor;
					streamCepstrumSource.setInputStream(inputStream, bigEndian != 0);
				}
				else if (dataProcessor is StreamHTKCepstrum)
				{
					StreamHTKCepstrum streamHTKCepstrum = (StreamHTKCepstrum)dataProcessor;
					streamHTKCepstrum.setInputStream(inputStream);
				}
				continue;
				goto IL_BF;
				IL_4F:
				this.logger.info(new StringBuilder().append("Reading ").append(text).append(" as raw audio file.").toString());
				inputStream = new FileInputStream(text);
				if (java.lang.String.instancehelper_endsWith(java.lang.String.instancehelper_toLowerCase(text), ".sph"))
				{
					this.logger.info("Skipping 1024-byte Sphere header.");
					((FileInputStream)inputStream).skip((long)((ulong)1024));
					goto IL_BF;
				}
				goto IL_BF;
			}
		}

		[LineNumberTable(new byte[]
		{
			160,
			168,
			241,
			80,
			241,
			82,
			241,
			84,
			241,
			80,
			241,
			80,
			241,
			94,
			144,
			241,
			80,
			241,
			94,
			144,
			241,
			110,
			144,
			241,
			102,
			241,
			83,
			241,
			83
		})]
		
		internal virtual void addCommands(CommandInterpreter commandInterpreter)
		{
			commandInterpreter.add("ls", new BatchModeRecognizer_1(this));
			commandInterpreter.add("show", new BatchModeRecognizer_2(this));
			commandInterpreter.add("edit", new BatchModeRecognizer_3(this));
			commandInterpreter.add("save", new BatchModeRecognizer_4(this));
			commandInterpreter.add("set", new BatchModeRecognizer_5(this));
			commandInterpreter.add("recognize", new BatchModeRecognizer_6(this));
			commandInterpreter.addAlias("recognize", "rec");
			commandInterpreter.add("statsReset", new BatchModeRecognizer_7(this));
			commandInterpreter.add("batchRecognize", new BatchModeRecognizer_8(this));
			commandInterpreter.addAlias("batchRecognize", "br");
			commandInterpreter.add("batchNext", new BatchModeRecognizer_9(this));
			commandInterpreter.addAlias("batchNext", "bn");
			commandInterpreter.add("batchAll", new BatchModeRecognizer_10(this));
			commandInterpreter.add("batchReset", new BatchModeRecognizer_11(this));
			commandInterpreter.add("batchLoad", new BatchModeRecognizer_12(this));
		}

		[Throws(new string[]
		{
			"java.io.IOException"
		})]
		[LineNumberTable(new byte[]
		{
			161,
			207,
			102,
			107,
			103,
			103,
			107,
			102,
			107,
			114,
			253,
			69,
			226,
			61,
			97,
			127,
			11,
			135
		})]
		
		public virtual void shell(string batchfile)
		{
			IOException ex2;
			try
			{
				CommandInterpreter commandInterpreter = new CommandInterpreter();
				commandInterpreter.setPrompt("s4> ");
				this.addCommands(commandInterpreter);
				this.setBatchFile(batchfile);
				this.recognizer.allocate();
				commandInterpreter.run();
				this.batchManager.stop();
				if (this.recognizer.getState() == Recognizer.State.__READY)
				{
					this.recognizer.deallocate();
				}
			}
			catch (IOException ex)
			{
				ex2 = ByteCodeHelper.MapException<IOException>(ex, 1);
				goto IL_67;
			}
			return;
			IL_67:
			IOException ex3 = ex2;
			this.logger.severe(new StringBuilder().append("I/O error during decoding: ").append(Throwable.instancehelper_getMessage(ex3)).toString());
			throw Throwable.__<unmap>(ex3);
		}

		[Throws(new string[]
		{
			"java.io.IOException"
		})]
		[LineNumberTable(new byte[]
		{
			160,
			92,
			130,
			107,
			135,
			107,
			123,
			47,
			165,
			114,
			108,
			108,
			114,
			127,
			11,
			127,
			6,
			100,
			101,
			107,
			221,
			226,
			61,
			98,
			127,
			12,
			136,
			127,
			16
		})]
		
		public virtual void decode(string batchFile)
		{
			int num = 0;
			IOException ex2;
			try
			{
				this.recognizer.allocate();
				this.setBatchFile(batchFile);
				this.batchManager.start();
				this.logger.info(new StringBuilder().append("BatchDecoder: decoding files in ").append(this.batchManager.getFilename()).toString());
				BatchItem nextItem;
				while (num < this.utteranceId && (nextItem = this.batchManager.getNextItem()) != null)
				{
					this.setInputStream(nextItem.getFilename());
					Result result = this.recognizer.recognize(nextItem.getTranscript());
					this.logger.info(new StringBuilder().append("File  : ").append(nextItem.getFilename()).toString());
					this.logger.info(new StringBuilder().append("Result: ").append(result).toString());
					num++;
				}
				this.batchManager.stop();
				this.recognizer.deallocate();
			}
			catch (IOException ex)
			{
				ex2 = ByteCodeHelper.MapException<IOException>(ex, 1);
				goto IL_107;
			}
			this.logger.info(new StringBuilder().append("BatchDecoder: ").append(num).append(" files decoded").toString());
			return;
			IL_107:
			IOException ex3 = ex2;
			this.logger.severe(new StringBuilder().append("I/O error during decoding: ").append(Throwable.instancehelper_getMessage(ex3)).toString());
			throw Throwable.__<unmap>(ex3);
		}

		
		[LineNumberTable(new byte[]
		{
			159,
			107,
			99,
			104,
			118,
			135,
			103,
			104,
			104,
			104,
			135,
			103,
			103
		})]
		
		public BatchModeRecognizer(Recognizer recognizer, List inputDataProcessors, int skip, int utteranceId, int whichBatch, int totalBatches, bool usePooledBatchManager)
		{
			this.logger = Logger.getLogger(java.lang.Object.instancehelper_getClass(this).getName());
			this.cm = null;
			this.skip = skip;
			this.utteranceId = utteranceId;
			this.whichBatch = whichBatch;
			this.totalBatches = totalBatches;
			this.usePooledBatchManager = usePooledBatchManager;
			this.recognizer = recognizer;
			this.inputDataProcessors = inputDataProcessors;
		}

		[LineNumberTable(new byte[]
		{
			105,
			134
		})]
		
		public BatchModeRecognizer()
		{
		}

		[Throws(new string[]
		{
			"edu.cmu.sphinx.util.props.PropertyException"
		})]
		[LineNumberTable(new byte[]
		{
			116,
			108,
			140,
			113,
			113,
			105,
			171,
			113,
			113,
			150,
			118,
			118
		})]
		
		public virtual void newProperties(PropertySheet ps)
		{
			this.logger = ps.getLogger();
			this.cm = ConfigurationManagerUtils.getPropertyManager(ps);
			this.skip = ps.getInt("skip");
			this.utteranceId = ps.getInt("count");
			if (this.utteranceId <= 0)
			{
				this.utteranceId = int.MaxValue;
			}
			this.whichBatch = ps.getInt("whichBatch");
			this.totalBatches = ps.getInt("totalBatches");
			this.usePooledBatchManager = ps.getBoolean("usePooledBatchManager").booleanValue();
			this.recognizer = (Recognizer)ps.getComponent("recognizer");
			this.inputDataProcessors = ps.getComponentList("inputDataProcessors", ClassLiteral<DataProcessor>.Value);
		}

		[LineNumberTable(new byte[]
		{
			161,
			230,
			101,
			143,
			134,
			100,
			228,
			69,
			113,
			103,
			114,
			100,
			127,
			5,
			133,
			116,
			138,
			136,
			255,
			45,
			79,
			226,
			61,
			98,
			111,
			135
		})]
		
		public static void main(string[] argv)
		{
			if (argv.Length < 2)
			{
				java.lang.System.@out.println("Usage: BatchDecoder propertiesFile batchFile [-shell]");
				java.lang.System.exit(1);
			}
			string text = argv[0];
			string text2 = argv[1];
			Exception ex3;
			try
			{
				URL url = new File(text).toURI().toURL();
				ConfigurationManager configurationManager = new ConfigurationManager(url);
				BatchModeRecognizer batchModeRecognizer = (BatchModeRecognizer)configurationManager.lookup("batch");
				if (batchModeRecognizer != null)
				{
					if (argv.Length >= 3 && java.lang.String.instancehelper_equals(argv[2], "-shell"))
					{
						batchModeRecognizer.shell(text2);
					}
					else
					{
						batchModeRecognizer.decode(text2);
					}
					java.lang.System.@out.println(new StringBuilder().append("Tokens created: ").append(StatisticsVariable.getStatisticsVariable("totalTokensScored").value).toString());
					goto IL_E5;
				}
				java.lang.System.err.println(new StringBuilder().append("Can't find batchModeRecognizer in ").append(text).toString());
			}
			catch (Exception ex)
			{
				Exception ex2 = ByteCodeHelper.MapException<Exception>(ex, 0);
				if (ex2 == null)
				{
					throw;
				}
				ex3 = ex2;
				goto IL_E7;
			}
			return;
			IL_E5:
			return;
			IL_E7:
			Exception ex4 = ex3;
			java.lang.System.err.println("Error during decoding: \n  ");
			Throwable.instancehelper_printStackTrace(ex4);
		}

		[Throws(new string[]
		{
			"java.io.IOException"
		})]
		[LineNumberTable(new byte[]
		{
			162,
			20,
			107,
			103,
			107,
			123,
			47,
			133,
			103
		})]
		
		public virtual void start(string batchFile)
		{
			this.recognizer.allocate();
			this.setBatchFile(batchFile);
			this.batchManager.start();
			this.logger.info(new StringBuilder().append("BatchDecoder: decoding files in ").append(this.batchManager.getFilename()).toString());
			this.count = 0;
		}

		[Throws(new string[]
		{
			"java.io.IOException"
		})]
		[LineNumberTable(new byte[]
		{
			162,
			30,
			107,
			107
		})]
		
		public virtual void stop()
		{
			this.batchManager.stop();
			this.recognizer.deallocate();
		}

		[Throws(new string[]
		{
			"java.io.IOException"
		})]
		[LineNumberTable(new byte[]
		{
			162,
			36,
			130,
			119,
			108,
			108,
			114,
			127,
			11,
			127,
			6,
			142,
			127,
			21
		})]
		
		public virtual Result recognize()
		{
			Result result = null;
			BatchItem nextItem;
			if (this.count < this.utteranceId && (nextItem = this.batchManager.getNextItem()) != null)
			{
				this.setInputStream(nextItem.getFilename());
				result = this.recognizer.recognize(nextItem.getTranscript());
				this.logger.info(new StringBuilder().append("File  : ").append(nextItem.getFilename()).toString());
				this.logger.info(new StringBuilder().append("Result: ").append(result).toString());
				this.count++;
			}
			this.logger.info(new StringBuilder().append("BatchDecoder: ").append(this.count).append(" files decoded").toString());
			return result;
		}

		[S4Integer(new object[]
		{
			64,
			"Ledu/cmu/sphinx/util/props/S4Integer;",
			"defaultValue",
			0
		})]
		public const string PROP_SKIP = "skip";

		[S4Integer(new object[]
		{
			64,
			"Ledu/cmu/sphinx/util/props/S4Integer;",
			"defaultValue",
			1000000
		})]
		public const string PROP_COUNT = "count";

		[S4Integer(new object[]
		{
			64,
			"Ledu/cmu/sphinx/util/props/S4Integer;",
			"defaultValue",
			0
		})]
		public const string PROP_WHICH_BATCH = "whichBatch";

		[S4Integer(new object[]
		{
			64,
			"Ledu/cmu/sphinx/util/props/S4Integer;",
			"defaultValue",
			1
		})]
		public const string PROP_TOTAL_BATCHES = "totalBatches";

		[S4Boolean(new object[]
		{
			64,
			"Ledu/cmu/sphinx/util/props/S4Boolean;",
			"defaultValue",
			false
		})]
		public const string PROP_USE_POOLED_BATCH_MANAGER = "usePooledBatchManager";

		[S4Component(new object[]
		{
			64,
			"Ledu/cmu/sphinx/util/props/S4Component;",
			"type",
			new object[]
			{
				99,
				"Ledu/cmu/sphinx/recognizer/Recognizer, sphinx4-core-5prealpha-SNAPSHOT, Version=0/0/0/0, Culture=neutral, PublicKeyToken=null;"
			}
		})]
		public const string PROP_RECOGNIZER = "recognizer";

		[S4ComponentList(new object[]
		{
			64,
			"Ledu/cmu/sphinx/util/props/S4ComponentList;",
			"type",
			new object[]
			{
				99,
				"Ledu/cmu/sphinx/frontend/BaseDataProcessor, sphinx4-core-5prealpha-SNAPSHOT, Version=0/0/0/0, Culture=neutral, PublicKeyToken=null;"
			}
		})]
		public const string PROP_INPUT_DATA_PROCESSORS = "inputDataProcessors";

		protected internal string name;

		
		protected internal List inputDataProcessors;

		protected internal int skip;

		protected internal int utteranceId;

		protected internal int whichBatch;

		protected internal int totalBatches;

		protected internal bool usePooledBatchManager;

		protected internal BatchManager batchManager;

		protected internal Recognizer recognizer;

		protected internal Logger logger;

		protected internal BatchItem curBatchItem;

		protected internal ConfigurationManager cm;

		internal int count;
	}
}
