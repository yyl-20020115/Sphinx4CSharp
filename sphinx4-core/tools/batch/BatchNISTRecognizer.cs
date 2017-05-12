using edu.cmu.sphinx.decoder.search;
using edu.cmu.sphinx.frontend;
using edu.cmu.sphinx.frontend.util;
using edu.cmu.sphinx.linguist;
using edu.cmu.sphinx.linguist.dictionary;
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

namespace edu.cmu.sphinx.tools.batch
{
	public class BatchNISTRecognizer : BatchModeRecognizer
	{		
		protected internal virtual void setInputStream(BatchNISTRecognizer.CTLUtterance utt)
		{
			Iterator iterator = this.inputDataProcessors.iterator();
			while (iterator.hasNext())
			{
				DataProcessor dataProcessor = (DataProcessor)iterator.next();
				if (dataProcessor is StreamDataSource)
				{
					((StreamDataSource)dataProcessor).setInputStream(utt.getInputStream());
				}
				else if (dataProcessor is StreamCepstrumSource)
				{
					int bigEndian = Utilities.isCepstraFileBigEndian(utt.getName()) ? 1 : 0;
					StreamCepstrumSource streamCepstrumSource = (StreamCepstrumSource)dataProcessor;
					streamCepstrumSource.setInputStream(utt.getInputStream(), bigEndian != 0);
				}
			}
		}
		
		protected internal virtual void handleResult(DataOutputStream @out, BatchNISTRecognizer.CTLUtterance utt, Result result)
		{
			this.dumpBestPath(@out, utt, result.getBestFinalToken());
		}
		
		private long dumpBestPath(DataOutputStream dataOutputStream, BatchNISTRecognizer.CTLUtterance ctlutterance, Token token)
		{
			if (token == null)
			{
				return 0L;
			}
			Token predecessor = token.getPredecessor();
			long num = this.dumpBestPath(dataOutputStream, ctlutterance, predecessor);
			if (token.isWord())
			{
				long collectTime = token.getCollectTime();
				WordSearchState wordSearchState = (WordSearchState)token.getSearchState();
				Word word = wordSearchState.getPronunciation().getWord();
				string spelling = word.getSpelling();
				if (!String.instancehelper_startsWith(spelling, "<"))
				{
					string[] array = String.instancehelper_split(ctlutterance.name, "_");
					dataOutputStream.write(String.instancehelper_getBytes(new StringBuilder().append(array[0]).append('_').append(array[1]).append('_').append(array[2]).append(" 1 ").append((double)((long)ctlutterance.startOffset + num) / 100.0).append(' ').append((double)(collectTime - num) / 100.0).append(' ').toString()));
					dataOutputStream.write(BatchNISTRecognizer.hex2Binary(spelling));
					dataOutputStream.write(String.instancehelper_getBytes(" 0.700000\n"));
				}
				return collectTime;
			}
			return num;
		}

		public static byte[] hex2Binary(string spelling)
		{
			byte[] array = new byte[String.instancehelper_length(spelling) / 2];
			for (int i = 0; i < String.instancehelper_length(spelling); i += 2)
			{
				int num = BatchNISTRecognizer.hexToByte(String.instancehelper_charAt(spelling, i));
				int num2 = BatchNISTRecognizer.hexToByte(String.instancehelper_charAt(spelling, i + 1));
				array[i / 2] = (byte)((sbyte)(num2 + 16 * num));
			}
			return array;
		}
		
		private static int hexToByte(char c)
		{
			switch (c)
			{
			case '0':
				return 0;
			case '1':
				return 1;
			case '2':
				return 2;
			case '3':
				return 3;
			case '4':
				return 4;
			case '5':
				return 5;
			case '6':
				return 6;
			case '7':
				return 7;
			case '8':
				return 8;
			case '9':
				return 9;
			case 'a':
				return 10;
			case 'b':
				return 11;
			case 'c':
				return 12;
			case 'd':
				return 13;
			case 'e':
				return 14;
			case 'f':
				return 15;
			}
			string text = new StringBuilder().append("Bad hex char ").append(c).toString();
			
			throw new Error(text);
		}
		
		public virtual void decode()
		{
			try
			{
				this.utteranceId = 0;
				DataOutputStream @out = new DataOutputStream(new FileOutputStream(this.ctmFile));
				this.recognizer.allocate();
				BatchNISTRecognizer.CTLIterator ctliterator = new BatchNISTRecognizer.CTLIterator(this);
				while (ctliterator.hasNext())
				{
					BatchNISTRecognizer.CTLUtterance ctlutterance = (BatchNISTRecognizer.CTLUtterance)ctliterator.next();
					this.setInputStream(ctlutterance);
					Result result = this.recognizer.recognize();
					java.lang.System.@out.println(new StringBuilder().append("Utterance ").append(this.utteranceId).append(": ").append(ctlutterance.getName()).toString());
					java.lang.System.@out.println(new StringBuilder().append("Reference: ").append(ctlutterance.getRef()).toString());
					java.lang.System.@out.println(new StringBuilder().append("Result   : ").append(result).toString());
					this.logger.info(new StringBuilder().append("Utterance ").append(this.utteranceId).append(": ").append(ctlutterance.getName()).toString());
					this.logger.info(new StringBuilder().append("Result   : ").append(result).toString());
					this.handleResult(@out, ctlutterance, result);
					this.utteranceId++;
				}
				this.recognizer.deallocate();
			}
			catch (IOException ex)
			{
				this.logger.severe(new StringBuilder().append("I/O error during decoding: ").append(Throwable.instancehelper_getMessage(ex)).toString());
			}
			this.logger.info(new StringBuilder().append("BatchCTLDecoder: ").append(this.utteranceId).append(" utterances decoded").toString());
		}
		
		public BatchNISTRecognizer(Recognizer recognizer, List inputDataProcessors, string ctlFile, string dataDir, string refFile, string ctmFile, int bitsPerSample, int samplesPerSecond, int framesPerSecond, int channelCount)
		{
			this.logger = Logger.getLogger(Object.instancehelper_getClass(this).getName());
			this.recognizer = recognizer;
			this.inputDataProcessors = inputDataProcessors;
			this.dataDir = dataDir;
			this.ctlFile = ctlFile;
			this.refFile = refFile;
			this.ctmFile = ctmFile;
			this.bitsPerSample = bitsPerSample;
			this.channelCount = channelCount;
			this.samplesPerSecond = samplesPerSecond;
			this.framesPerSecond = framesPerSecond;
			int num = bitsPerSample / 8 * channelCount * samplesPerSecond;
			this.bytesPerFrame = ((framesPerSecond != -1) ? (num / framesPerSecond) : (-num));
			this.logger.info(new StringBuilder().append("BatchNISTRecognizer:\n  dataDirectory=").append(dataDir).append('\n').append("  ctlFile=").append(ctlFile).append('\n').append("  bitsPerSample=").append(bitsPerSample).append('\n').append("  channelCount=").append(channelCount).append('\n').append("  samplesPerSecond=").append(samplesPerSecond).append('\n').append("  framesPerSecond=").append(framesPerSecond).append('\n').toString());
		}
		
		public BatchNISTRecognizer()
		{
		}
		
		public override void newProperties(PropertySheet ps)
		{
			this.logger = ps.getLogger();
			this.recognizer = (Recognizer)ps.getComponent("recognizer");
			this.inputDataProcessors = ps.getComponentList("inputDataProcessors", ClassLiteral<DataProcessor>.Value);
			this.dataDir = ps.getString("dataDirectory");
			this.ctlFile = ps.getString("ctlFile");
			this.refFile = ps.getString("refFile");
			this.ctmFile = ps.getString("ctmFile");
			this.bitsPerSample = ps.getInt("bitsPerSample");
			this.channelCount = ps.getInt("channelCount");
			this.samplesPerSecond = ps.getInt("samplesPerSecond");
			this.framesPerSecond = ps.getInt("framesPerSecond");
			int num = this.bitsPerSample / 8 * this.channelCount * this.samplesPerSecond;
			int num2 = this.framesPerSecond;
			this.bytesPerFrame = ((num2 != -1) ? (num / num2) : (-num));
			this.logger.info(new StringBuilder().append("BatchNISTRecognizer:\n  dataDirectory=").append(this.dataDir).append('\n').append("  ctlFile=").append(this.ctlFile).append('\n').append("  bitsPerSample=").append(this.bitsPerSample).append('\n').append("  channelCount=").append(this.channelCount).append('\n').append("  samplesPerSecond=").append(this.samplesPerSecond).append('\n').append("  framesPerSecond=").append(this.framesPerSecond).append('\n').toString());
		}
		
		public new static void main(string[] argv)
		{
			if (argv.Length != 1)
			{
				java.lang.System.@out.println("Usage: BatchNISTRecognizer propertiesFile");
				java.lang.System.exit(1);
			}
			string text = argv[0];
			BatchNISTRecognizer batchNISTRecognizer;
			try
			{
				try
				{
					URL url = new File(text).toURI().toURL();
					ConfigurationManager configurationManager = new ConfigurationManager(url);
					batchNISTRecognizer = (BatchNISTRecognizer)configurationManager.lookup("batchNIST");
				}
				catch (IOException ex)
				{
					java.lang.System.err.println(new StringBuilder().append("I/O error during initialization: \n   ").append(ex).toString());
					return;
				}
			}
			catch (PropertyException ex3)
			{
				java.lang.System.err.println(new StringBuilder().append("Error during initialization: \n  ").append(ex3).toString());
				return;
			}
			if (batchNISTRecognizer == null)
			{
				java.lang.System.err.println(new StringBuilder().append("Can't find batchNIST in ").append(text).toString());
				return;
			}
			batchNISTRecognizer.decode();
		}

		protected internal string ctlFile;

		protected internal string dataDir;

		protected internal string refFile;

		protected internal string ctmFile;

		protected internal int bitsPerSample;

		protected internal int samplesPerSecond;

		protected internal int framesPerSecond;

		protected internal int channelCount;

		protected internal int bytesPerFrame;

		[S4String(new object[]
		{
			64,
			"Ledu/cmu/sphinx/util/props/S4String;",
			"defaultValue",
			"<raw data directory not set>"
		})]
		public const string PROP_DATA_DIR = "dataDirectory";

		[S4String(new object[]
		{
			64,
			"Ledu/cmu/sphinx/util/props/S4String;",
			"defaultValue",
			"<ctl file not set>"
		})]
		public const string PROP_CTL_FILE = "ctlFile";

		[S4String(new object[]
		{
			64,
			"Ledu/cmu/sphinx/util/props/S4String;",
			"defaultValue",
			"<ref file not set>"
		})]
		public const string PROP_REF_FILE = "refFile";

		[S4String(new object[]
		{
			64,
			"Ledu/cmu/sphinx/util/props/S4String;",
			"defaultValue",
			"<ctm file not set>"
		})]
		public const string PROP_CTM_FILE = "ctmFile";

		[S4Integer(new object[]
		{
			64,
			"Ledu/cmu/sphinx/util/props/S4Integer;",
			"defaultValue",
			16
		})]
		public const string PROP_BITS_PER_SAMPLE = "bitsPerSample";

		[S4Integer(new object[]
		{
			64,
			"Ledu/cmu/sphinx/util/props/S4Integer;",
			"defaultValue",
			1
		})]
		public const string PROP_CHANNEL_COUNT = "channelCount";

		[S4Integer(new object[]
		{
			64,
			"Ledu/cmu/sphinx/util/props/S4Integer;",
			"defaultValue",
			16000
		})]
		public const string PROP_SAMPLES_PER_SECOND = "samplesPerSecond";

		[S4Integer(new object[]
		{
			64,
			"Ledu/cmu/sphinx/util/props/S4Integer;",
			"defaultValue",
			100
		})]
		public const string PROP_FRAMES_PER_SECOND = "framesPerSecond";

		[System.Serializable]
		public class CTLException : System.Exception
		{
			internal CTLException(BatchNISTRecognizer batchNISTRecognizer, string text)
				:base(text)
			{
				this.this_0 = batchNISTRecognizer;
			}
			
			[System.Security.Permissions.PermissionSet(System.Security.Permissions.SecurityAction.Demand, XML = "<PermissionSet class=\"System.Security.PermissionSet\"\nversion=\"1\">\n<IPermission class=\"System.Security.Permissions.SecurityPermission, mscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089\"\nversion=\"1\"\nFlags=\"SerializationFormatter\"/>\n</PermissionSet>\n")]
			protected CTLException(System.Runtime.Serialization.SerializationInfo serializationInfo, System.Runtime.Serialization.StreamingContext streamingContext) : base(serializationInfo, streamingContext)
			{
			}

			internal BatchNISTRecognizer this_0;
		}

		public class CTLIterator : Object, Iterator
		{			
			public CTLIterator(BatchNISTRecognizer this_0)
			{
				this.this_0 = this_0;
				this.ctlReader = new LineNumberReader(new FileReader(this_0.ctlFile));
				this.refReader = new LineNumberReader(new FileReader(this_0.refFile));
				this.utterance = this.nextUtterance();
			}

			private BatchNISTRecognizer.CTLUtterance nextUtterance()
			{
				BatchNISTRecognizer.CTLUtterance result;
				try
				{
					string text = this.ctlReader.readLine();
					string text2 = this.refReader.readLine();
					if (text != null && text2 != null)
					{
						return new BatchNISTRecognizer.CTLUtterance(this.this_0, text, text2);
					}
					result = null;
				}
				catch (System.Exception ex)
				{
					throw new Error(Throwable.instancehelper_getMessage(ex));
				}
				return result;
			}

			public virtual BatchNISTRecognizer.CTLUtterance next()
			{
				BatchNISTRecognizer.CTLUtterance result = this.utterance;
				this.utterance = this.nextUtterance();
				return result;
			}

			public virtual bool hasNext()
			{
				return this.utterance != null;
			}
			
			public virtual void remove()
			{				
				throw new Error("Not implemented");
			}

			object Iterator.next()
			{
				return this.next();
			}

			internal BatchNISTRecognizer.CTLUtterance utterance;

			internal LineNumberReader ctlReader;

			internal LineNumberReader refReader;

			internal BatchNISTRecognizer this_0;
		}

		public class CTLUtterance : Object
		{
			public virtual InputStream getInputStream()
			{
				return new ByteArrayInputStream(this.data);
			}

			public virtual string getName()
			{
				return this.name;
			}

			public virtual string getRef()
			{
				return this.@ref;
			}
	
			internal CTLUtterance(BatchNISTRecognizer batchNISTRecognizer, string text, string text2)
			{
				this_0 = batchNISTRecognizer;
				this.@ref = text2;
				string[] array = String.instancehelper_split(text, " ");
				if (array.Length != 4)
				{
					string text3 = new StringBuilder().append("CTL Syntax Error: ").append(text).toString();
					
					throw new BatchNISTRecognizer.CTLException(batchNISTRecognizer, text3);
				}
				this.startOffset = Integer.parseInt(array[1]);
				this.endOffset = Integer.parseInt(array[2]);
				this.name = array[3];
				this.data = new byte[(this.endOffset - this.startOffset) * batchNISTRecognizer.bytesPerFrame];
				int num = String.instancehelper_indexOf(array[0], 46);
				this.file = array[0];
				if (num >= 0)
				{
					this.file = String.instancehelper_substring(this.file, 0, num);
				}
				this.file = new StringBuilder().append(batchNISTRecognizer.dataDir).append('/').append(this.file).append(".raw").toString();
				try
				{
					FileInputStream fileInputStream = new FileInputStream(this.file);
					fileInputStream.skip((long)(this.startOffset * batchNISTRecognizer.bytesPerFrame));
					if (fileInputStream.read(this.data) != this.data.Length)
					{
						fileInputStream.close();
						string text4 = new StringBuilder().append("Unable to read ").append(this.data.Length).append(" bytes of utterance ").append(this.name).toString();
						
						throw new BatchNISTRecognizer.CTLException(batchNISTRecognizer, text4);
					}
					fileInputStream.close();
				}
				catch (IOException ex)
				{
					throw new BatchNISTRecognizer.CTLException(batchNISTRecognizer, new StringBuilder().append("Unable to read utterance ").append(this.name).append(": ").append(Throwable.instancehelper_getMessage(ex)).toString());
				}
			}

			public virtual string getFile()
			{
				return this.file;
			}

			public virtual int getStartOffset()
			{
				return this.startOffset;
			}

			public virtual int getEndOffset()
			{
				return this.endOffset;
			}

			internal int startOffset;

			internal int endOffset;

			internal string name;

			internal byte[] data;
			
			internal string @ref;

			internal string file;

			internal BatchNISTRecognizer this_0;
		}
	}
}
