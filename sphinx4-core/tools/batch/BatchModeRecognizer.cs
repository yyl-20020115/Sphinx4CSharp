using edu.cmu.sphinx.frontend;
using edu.cmu.sphinx.frontend.util;
using edu.cmu.sphinx.recognizer;
using edu.cmu.sphinx.result;
using edu.cmu.sphinx.util;
using edu.cmu.sphinx.util.props;
using ikvm.@internal;
using java.io;
using java.lang;
using java.net;
using java.util;
using java.util.logging;
using javax.sound.sampled;

namespace edu.cmu.sphinx.tools.batch
{
	public class BatchModeRecognizer : java.lang.Object, Configurable
	{	
		public virtual void setBatchFile(string batchFile)
		{
			if (this.usePooledBatchManager)
			{
				this.batchManager = new PooledBatchManager(batchFile, this.skip);
			}
			else
			{
				this.batchManager = new SimpleBatchManager(batchFile, this.skip, this.whichBatch, this.totalBatches);
			}
		}
		
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
				catch (UnsupportedAudioFileException)
				{
					this.logger.info(new StringBuilder().append("Reading ").append(text).append(" as raw audio file.").toString());
					inputStream = new FileInputStream(text);
					if (java.lang.String.instancehelper_endsWith(java.lang.String.instancehelper_toLowerCase(text), ".sph"))
					{
						this.logger.info("Skipping 1024-byte Sphere header.");
						((FileInputStream)inputStream).skip((long)((ulong)1024));
					}
				}
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
			}
		}

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

		public virtual void shell(string batchfile)
		{
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
				this.logger.severe(new StringBuilder().append("I/O error during decoding: ").append(Throwable.instancehelper_getMessage(ex)).toString());
				throw ex;
			}
		}
		
		public virtual void decode(string batchFile)
		{
			int num = 0;
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
				this.logger.severe(new StringBuilder().append("I/O error during decoding: ").append(Throwable.instancehelper_getMessage(ex)).toString());
				throw ex;
			}
			this.logger.info(new StringBuilder().append("BatchDecoder: ").append(num).append(" files decoded").toString());
		}

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
		
		public BatchModeRecognizer()
		{
		}
		
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

		public static void main(string[] argv)
		{
			if (argv.Length < 2)
			{
				java.lang.System.@out.println("Usage: BatchDecoder propertiesFile batchFile [-shell]");
				java.lang.System.exit(1);
			}
			string text = argv[0];
			string text2 = argv[1];
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
				}
				java.lang.System.err.println(new StringBuilder().append("Can't find batchModeRecognizer in ").append(text).toString());
			}
			catch (System.Exception ex)
			{
				java.lang.System.err.println("Error during decoding: \n  ");
				Throwable.instancehelper_printStackTrace(ex);
			}
		}
		
		public virtual void start(string batchFile)
		{
			this.recognizer.allocate();
			this.setBatchFile(batchFile);
			this.batchManager.start();
			this.logger.info(new StringBuilder().append("BatchDecoder: decoding files in ").append(this.batchManager.getFilename()).toString());
			this.count = 0;
		}
		
		public virtual void stop()
		{
			this.batchManager.stop();
			this.recognizer.deallocate();
		}
		
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
