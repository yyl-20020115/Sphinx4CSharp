using System;

using edu.cmu.sphinx.util;
using edu.cmu.sphinx.util.props;
using IKVM.Attributes;
using IKVM.Runtime;
using java.io;
using java.lang;
using java.util;

namespace edu.cmu.sphinx.frontend.util
{
	[Implements(new string[]
	{
		"edu.cmu.sphinx.util.ReferenceSource"
	})]
	public class ConcatFileDataSource : StreamDataSource, ReferenceSource
	{
		
		
		internal static string access_000(ConcatFileDataSource concatFileDataSource)
		{
			return concatFileDataSource.silenceFileName;
		}

		
		
		internal static int access$102(ConcatFileDataSource concatFileDataSource, int result)
		{
			concatFileDataSource.silenceCount = result;
			return result;
		}

		
		
		internal static string access$200(ConcatFileDataSource concatFileDataSource)
		{
			return concatFileDataSource.nextFile;
		}

		
		
		internal static string access$202(ConcatFileDataSource concatFileDataSource, string result)
		{
			concatFileDataSource.nextFile = result;
			return result;
		}

		
		
		internal static FileWriter access$300(ConcatFileDataSource concatFileDataSource)
		{
			return concatFileDataSource.transcript;
		}

		
		
		internal static int access$100(ConcatFileDataSource concatFileDataSource)
		{
			return concatFileDataSource.silenceCount;
		}

		
		
		internal static int access$110(ConcatFileDataSource concatFileDataSource)
		{
			int num = concatFileDataSource.silenceCount;
			int result = num;
			concatFileDataSource.silenceCount = num - 1;
			return result;
		}

		
		
		internal static List access$400(ConcatFileDataSource concatFileDataSource)
		{
			return concatFileDataSource.referenceList;
		}

		
		
		internal static int access$500(ConcatFileDataSource concatFileDataSource)
		{
			return concatFileDataSource.skip;
		}

		
		
		internal static int access$600(ConcatFileDataSource concatFileDataSource)
		{
			return concatFileDataSource.maxSilence;
		}

		
		
		internal static long access$700(ConcatFileDataSource concatFileDataSource)
		{
			return concatFileDataSource.totalBytes;
		}

		
		
		
		internal static float access$800(ConcatFileDataSource concatFileDataSource, long num)
		{
			return concatFileDataSource.getSeconds(num);
		}

		
		
		internal static long access$702(ConcatFileDataSource concatFileDataSource, long result)
		{
			concatFileDataSource.totalBytes = result;
			return result;
		}

		
		
		internal static string access$900(ConcatFileDataSource concatFileDataSource)
		{
			return concatFileDataSource.context;
		}

		
		
		internal static long access$1000(ConcatFileDataSource concatFileDataSource)
		{
			return concatFileDataSource.silenceFileLength;
		}

		
		
		internal static bool access$1100(ConcatFileDataSource concatFileDataSource)
		{
			return concatFileDataSource.addRandomSilence;
		}

		private float getSeconds(long num)
		{
			return (float)num / (float)this.bytesPerSecond;
		}

		[LineNumberTable(new byte[]
		{
			159,
			105,
			73,
			141,
			107,
			103,
			104,
			104,
			104,
			104,
			104,
			104,
			104
		})]
		
		public ConcatFileDataSource(int sampleRate, int bytesPerRead, int bitsPerSample, bool bigEndian, bool signedData, bool addRandomSilence, int maxSilence, int skip, string silenceFileName, int startFile, int totalFiles, string transcriptFile, string batchFile) : base(sampleRate, bytesPerRead, bitsPerSample, bigEndian, signedData)
		{
			this.bytesPerSecond = sampleRate * (bitsPerSample / 8);
			this.addRandomSilence = addRandomSilence;
			this.maxSilence = maxSilence;
			this.skip = skip;
			this.silenceFileName = silenceFileName;
			this.startFile = startFile;
			this.totalFiles = totalFiles;
			this.transcriptFile = transcriptFile;
			this.batchFile = batchFile;
		}

		[LineNumberTable(new byte[]
		{
			111,
			134
		})]
		
		public ConcatFileDataSource()
		{
		}

		[Throws(new string[]
		{
			"edu.cmu.sphinx.util.props.PropertyException"
		})]
		[LineNumberTable(new byte[]
		{
			122,
			135,
			117,
			118,
			113,
			113,
			113,
			113,
			113,
			113,
			113
		})]
		
		public override void newProperties(PropertySheet ps)
		{
			base.newProperties(ps);
			this.bytesPerSecond = this.sampleRate * (this.bitsPerSample / 8);
			this.addRandomSilence = ps.getBoolean("addRandomSilence").booleanValue();
			this.maxSilence = ps.getInt("maxSilence");
			this.skip = ps.getInt("skip");
			this.silenceFileName = ps.getString("silenceFile");
			this.startFile = ps.getInt("startFile");
			this.totalFiles = ps.getInt("totalFiles");
			this.transcriptFile = ps.getString("transcriptFile");
			this.batchFile = ps.getString("batchFile");
		}

		[LineNumberTable(new byte[]
		{
			160,
			75,
			166,
			113,
			140,
			104,
			145,
			104,
			144,
			126,
			165,
			189,
			2,
			97,
			134
		})]
		
		public override void initialize()
		{
			base.initialize();
			IOException ex2;
			try
			{
				File.__<clinit>();
				File file = new File(this.silenceFileName);
				this.silenceFileLength = file.length();
				if (this.transcriptFile != null)
				{
					this.transcript = new FileWriter(this.transcriptFile);
				}
				if (this.batchFile == null)
				{
					string text = "BatchFile cannot be null!";
					
					throw new Error(text);
				}
				this.setInputStream(new SequenceInputStream(new ConcatFileDataSource.InputStreamEnumeration(this, this.batchFile, this.startFile, this.totalFiles)));
				this.referenceList = new LinkedList();
			}
			catch (IOException ex)
			{
				ex2 = ByteCodeHelper.MapException<IOException>(ex, 1);
				goto IL_91;
			}
			return;
			IL_91:
			IOException ex3 = ex2;
			Throwable.instancehelper_printStackTrace(ex3);
		}

		
		public virtual List getReferences()
		{
			return this.referenceList;
		}

		public virtual string getTranscriptFile()
		{
			return this.transcriptFile;
		}

		[S4Integer(new object[]
		{
			64,
			"Ledu/cmu/sphinx/util/props/S4Integer;",
			"defaultValue",
			1
		})]
		public const string PROP_START_FILE = "startFile";

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
			-1
		})]
		public const string PROP_TOTAL_FILES = "totalFiles";

		[S4String(new object[]
		{
			64,
			"Ledu/cmu/sphinx/util/props/S4String;"
		})]
		public const string PROP_SILENCE_FILE = "silenceFile";

		[S4Boolean(new object[]
		{
			64,
			"Ledu/cmu/sphinx/util/props/S4Boolean;",
			"defaultValue",
			false
		})]
		public const string PROP_ADD_RANDOM_SILENCE = "addRandomSilence";

		[S4Integer(new object[]
		{
			64,
			"Ledu/cmu/sphinx/util/props/S4Integer;",
			"defaultValue",
			3
		})]
		public const string PROP_MAX_SILENCE = "maxSilence";

		[S4String(new object[]
		{
			64,
			"Ledu/cmu/sphinx/util/props/S4String;"
		})]
		public const string PROP_TRANSCRIPT_FILE = "transcriptFile";

		[S4String(new object[]
		{
			64,
			"Ledu/cmu/sphinx/util/props/S4String;"
		})]
		public const string PROP_BATCH_FILE = "batchFile";

		private const string GAP_LABEL = "inter_segment_gap";

		private bool addRandomSilence;

		private int skip;

		private int maxSilence;

		private int silenceCount;

		private int bytesPerSecond;

		private long totalBytes;

		private long silenceFileLength;

		private string silenceFileName;

		private string nextFile;

		private string context;

		private string transcriptFile;

		
		private List referenceList;

		private FileWriter transcript;

		private int startFile;

		private int totalFiles;

		private string batchFile;

		
		[Implements(new string[]
		{
			"java.util.Enumeration"
		})]
		
		[SourceFile("ConcatFileDataSource.java")]
		
		internal sealed class InputStreamEnumeration : java.lang.Object, Enumeration
		{
			[LineNumberTable(new byte[]
			{
				161,
				55,
				109,
				153
			})]
			
			private int getSilenceCount()
			{
				if (ConcatFileDataSource.access$1100(this.this$0))
				{
					return this.silenceRandom.nextInt(ConcatFileDataSource.access$600(this.this$0)) + 1;
				}
				return ConcatFileDataSource.access$600(this.this$0);
			}

			[LineNumberTable(new byte[]
			{
				160,
				214,
				104,
				167,
				98,
				110,
				108,
				109,
				134,
				108,
				110,
				167
			})]
			
			public string readNext()
			{
				if (!this.inSilence)
				{
					return this.readNextDataFile();
				}
				string result = null;
				if (ConcatFileDataSource.access$100(this.this$0) > 0)
				{
					result = ConcatFileDataSource.access_000(this.this$0);
					if (ConcatFileDataSource.access$300(this.this$0) != null)
					{
						this.writeSilenceToTranscript();
					}
					ConcatFileDataSource.access$110(this.this$0);
					if (ConcatFileDataSource.access$100(this.this$0) <= 0)
					{
						this.inSilence = false;
					}
				}
				return result;
			}

			[LineNumberTable(new byte[]
			{
				160,
				241,
				117,
				108,
				135,
				108,
				102,
				103,
				114,
				103,
				112,
				44,
				166,
				123,
				114,
				135,
				109,
				168,
				123,
				98,
				103
			})]
			
			private string readNextDataFile()
			{
				string result;
				IOException ex2;
				try
				{
					if (0 > this.totalFiles || this.totalFiles > ConcatFileDataSource.access$400(this.this$0).size())
					{
						string text = this.reader.readLine();
						if (text != null)
						{
							string reference = BatchFile.getReference(text);
							ConcatFileDataSource.access$400(this.this$0).add(reference);
							text = BatchFile.getFilename(text);
							for (int i = 1; i < ConcatFileDataSource.access$500(this.this$0); i++)
							{
								this.reader.readLine();
							}
							if (ConcatFileDataSource.access_000(this.this$0) != null && ConcatFileDataSource.access$600(this.this$0) > 0)
							{
								ConcatFileDataSource.access$102(this.this$0, this.getSilenceCount());
								this.inSilence = true;
							}
							if (ConcatFileDataSource.access$300(this.this$0) != null)
							{
								this.writeTranscript(text, reference);
							}
						}
						return text;
					}
					result = null;
				}
				catch (IOException ex)
				{
					ex2 = ByteCodeHelper.MapException<IOException>(ex, 1);
					goto IL_DB;
				}
				return result;
				IL_DB:
				IOException ex3 = ex2;
				Throwable.instancehelper_printStackTrace(ex3);
				string text2 = "Problem reading from batch file";
				
				throw new Error(text2);
			}

			[LineNumberTable(new byte[]
			{
				161,
				37,
				119,
				127,
				4,
				119,
				159,
				67,
				191,
				3,
				2,
				97,
				134
			})]
			
			private void writeSilenceToTranscript()
			{
				IOException ex2;
				try
				{
					float num = ConcatFileDataSource.access$800(this.this$0, ConcatFileDataSource.access$700(this.this$0));
					ConcatFileDataSource.access$702(this.this$0, ConcatFileDataSource.access$700(this.this$0) + ConcatFileDataSource.access$1000(this.this$0));
					float num2 = ConcatFileDataSource.access$800(this.this$0, ConcatFileDataSource.access$700(this.this$0));
					ConcatFileDataSource.access$300(this.this$0).write(new StringBuilder().append(ConcatFileDataSource.access$900(this.this$0)).append(" 1 ").append("inter_segment_gap").append(' ').append(num).append(' ').append(num2).append(" \n").toString());
					ConcatFileDataSource.access$300(this.this$0).flush();
				}
				catch (IOException ex)
				{
					ex2 = ByteCodeHelper.MapException<IOException>(ex, 1);
					goto IL_D2;
				}
				return;
				IL_D2:
				IOException ex3 = ex2;
				Throwable.instancehelper_printStackTrace(ex3);
			}

			[LineNumberTable(new byte[]
			{
				161,
				21,
				103,
				119,
				126,
				119,
				159,
				76,
				191,
				3,
				2,
				98,
				135
			})]
			
			private void writeTranscript(string text, string text2)
			{
				IOException ex2;
				try
				{
					File file = new File(text);
					float num = ConcatFileDataSource.access$800(this.this$0, ConcatFileDataSource.access$700(this.this$0));
					ConcatFileDataSource.access$702(this.this$0, ConcatFileDataSource.access$700(this.this$0) + file.length());
					float num2 = ConcatFileDataSource.access$800(this.this$0, ConcatFileDataSource.access$700(this.this$0));
					ConcatFileDataSource.access$300(this.this$0).write(new StringBuilder().append(ConcatFileDataSource.access$900(this.this$0)).append(" 1 ").append(text).append(' ').append(num).append(' ').append(num2).append("  ").append(text2).append('\n').toString());
					ConcatFileDataSource.access$300(this.this$0).flush();
				}
				catch (IOException ex)
				{
					ex2 = ByteCodeHelper.MapException<IOException>(ex, 1);
					goto IL_DD;
				}
				return;
				IL_DD:
				IOException ex3 = ex2;
				Throwable.instancehelper_printStackTrace(ex3);
			}

			[LineNumberTable(new byte[]
			{
				160,
				180,
				98,
				109,
				146,
				144,
				150,
				255,
				0,
				69,
				226,
				60,
				97,
				102,
				255,
				26,
				70,
				144,
				191,
				3,
				2,
				97,
				166
			})]
			
			public InputStream nextElement()
			{
				FileInputStream fileInputStream = null;
				if (ConcatFileDataSource.access$200(this.this$0) == null)
				{
					ConcatFileDataSource.access$202(this.this$0, this.readNext());
				}
				if (ConcatFileDataSource.access$200(this.this$0) != null)
				{
					IOException ex2;
					try
					{
						FileInputStream.__<clinit>();
						fileInputStream = new FileInputStream(ConcatFileDataSource.access$200(this.this$0));
						ConcatFileDataSource.access$202(this.this$0, null);
					}
					catch (IOException ex)
					{
						ex2 = ByteCodeHelper.MapException<IOException>(ex, 1);
						goto IL_63;
					}
					goto IL_A8;
					IL_63:
					IOException ex3 = ex2;
					Throwable.instancehelper_printStackTrace(ex3);
					string text = new StringBuilder().append("Cannot convert ").append(ConcatFileDataSource.access$200(this.this$0)).append(" to a FileInputStream").toString();
					
					throw new Error(text);
				}
				IL_A8:
				if (fileInputStream == null && ConcatFileDataSource.access$300(this.this$0) != null)
				{
					IOException ex5;
					try
					{
						ConcatFileDataSource.access$300(this.this$0).close();
					}
					catch (IOException ex4)
					{
						ex5 = ByteCodeHelper.MapException<IOException>(ex4, 1);
						goto IL_D7;
					}
					return fileInputStream;
					IL_D7:
					IOException ex3 = ex5;
					Throwable.instancehelper_printStackTrace(ex3);
				}
				return fileInputStream;
			}

			[Throws(new string[]
			{
				"java.io.IOException"
			})]
			[LineNumberTable(new byte[]
			{
				160,
				144,
				111,
				104,
				113,
				104,
				103,
				117,
				173,
				102,
				44,
				166
			})]
			
			internal InputStreamEnumeration(ConcatFileDataSource concatFileDataSource, string text, int num, int num2)
			{
				this.totalFiles = num2;
				this.reader = new BufferedReader(new FileReader(text));
				if (ConcatFileDataSource.access_000(concatFileDataSource) != null)
				{
					this.inSilence = true;
					Random.__<clinit>();
					this.silenceRandom = new Random(System.currentTimeMillis());
					ConcatFileDataSource.access$102(concatFileDataSource, this.getSilenceCount());
				}
				for (int i = 1; i < num; i++)
				{
					this.reader.readLine();
				}
			}

			[LineNumberTable(new byte[]
			{
				160,
				166,
				109,
				146
			})]
			
			public bool hasMoreElements()
			{
				if (ConcatFileDataSource.access$200(this.this$0) == null)
				{
					ConcatFileDataSource.access$202(this.this$0, this.readNext());
				}
				return ConcatFileDataSource.access$200(this.this$0) != null;
			}

			
			
			
			public object nextElement()
			{
				return this.nextElement();
			}

			
			private int totalFiles;

			private bool inSilence;

			private Random silenceRandom;

			private BufferedReader reader;

			
			internal ConcatFileDataSource this$0 = concatFileDataSource;
		}
	}
}
