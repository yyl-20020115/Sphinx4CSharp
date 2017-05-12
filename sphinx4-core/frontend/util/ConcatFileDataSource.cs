using edu.cmu.sphinx.util;
using edu.cmu.sphinx.util.props;
using java.io;
using java.lang;
using java.util;

namespace edu.cmu.sphinx.frontend.util
{
	public class ConcatFileDataSource : StreamDataSource, ReferenceSource
	{
		internal static string access_000(ConcatFileDataSource concatFileDataSource)
		{
			return concatFileDataSource.silenceFileName;
		}

		internal static int access_102(ConcatFileDataSource concatFileDataSource, int result)
		{
			concatFileDataSource.silenceCount = result;
			return result;
		}

		internal static string access_200(ConcatFileDataSource concatFileDataSource)
		{
			return concatFileDataSource.nextFile;
		}

		internal static string access_202(ConcatFileDataSource concatFileDataSource, string result)
		{
			concatFileDataSource.nextFile = result;
			return result;
		}

		internal static FileWriter access_300(ConcatFileDataSource concatFileDataSource)
		{
			return concatFileDataSource.transcript;
		}

		internal static int access_100(ConcatFileDataSource concatFileDataSource)
		{
			return concatFileDataSource.silenceCount;
		}

		internal static int access_110(ConcatFileDataSource concatFileDataSource)
		{
			int num = concatFileDataSource.silenceCount;
			int result = num;
			concatFileDataSource.silenceCount = num - 1;
			return result;
		}

		internal static List access_400(ConcatFileDataSource concatFileDataSource)
		{
			return concatFileDataSource.referenceList;
		}

		internal static int access_500(ConcatFileDataSource concatFileDataSource)
		{
			return concatFileDataSource.skip;
		}

		internal static int access_600(ConcatFileDataSource concatFileDataSource)
		{
			return concatFileDataSource.maxSilence;
		}

		internal static long access_700(ConcatFileDataSource concatFileDataSource)
		{
			return concatFileDataSource.totalBytes;
		}

		internal static float access_800(ConcatFileDataSource concatFileDataSource, long num)
		{
			return concatFileDataSource.getSeconds(num);
		}

		internal static long access_702(ConcatFileDataSource concatFileDataSource, long result)
		{
			concatFileDataSource.totalBytes = result;
			return result;
		}

		internal static string access_900(ConcatFileDataSource concatFileDataSource)
		{
			return concatFileDataSource.context;
		}

		internal static long access_1000(ConcatFileDataSource concatFileDataSource)
		{
			return concatFileDataSource.silenceFileLength;
		}

		internal static bool access_1100(ConcatFileDataSource concatFileDataSource)
		{
			return concatFileDataSource.addRandomSilence;
		}

		private float getSeconds(long num)
		{
			return (float)num / (float)this.bytesPerSecond;
		}

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

		public ConcatFileDataSource()
		{
		}

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

		public override void initialize()
		{
			base.initialize();
			try
			{
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
				Throwable.instancehelper_printStackTrace(ex);
			}
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

		internal sealed class InputStreamEnumeration : java.lang.Object, Enumeration
		{
			private int getSilenceCount()
			{
				if (ConcatFileDataSource.access_1100(this.this_0))
				{
					return this.silenceRandom.nextInt(ConcatFileDataSource.access_600(this.this_0)) + 1;
				}
				return ConcatFileDataSource.access_600(this.this_0);
			}

			public string readNext()
			{
				if (!this.inSilence)
				{
					return this.readNextDataFile();
				}
				string result = null;
				if (ConcatFileDataSource.access_100(this.this_0) > 0)
				{
					result = ConcatFileDataSource.access_000(this.this_0);
					if (ConcatFileDataSource.access_300(this.this_0) != null)
					{
						this.writeSilenceToTranscript();
					}
					ConcatFileDataSource.access_110(this.this_0);
					if (ConcatFileDataSource.access_100(this.this_0) <= 0)
					{
						this.inSilence = false;
					}
				}
				return result;
			}

			private string readNextDataFile()
			{
				string result;
				try
				{
					if (0 > this.totalFiles || this.totalFiles > ConcatFileDataSource.access_400(this.this_0).size())
					{
						string text = this.reader.readLine();
						if (text != null)
						{
							string reference = BatchFile.getReference(text);
							ConcatFileDataSource.access_400(this.this_0).add(reference);
							text = BatchFile.getFilename(text);
							for (int i = 1; i < ConcatFileDataSource.access_500(this.this_0); i++)
							{
								this.reader.readLine();
							}
							if (ConcatFileDataSource.access_000(this.this_0) != null && ConcatFileDataSource.access_600(this.this_0) > 0)
							{
								ConcatFileDataSource.access_102(this.this_0, this.getSilenceCount());
								this.inSilence = true;
							}
							if (ConcatFileDataSource.access_300(this.this_0) != null)
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
					Throwable.instancehelper_printStackTrace(ex);

					throw new Error("Problem reading from batch file");

				}
				return result;
			}

			private void writeSilenceToTranscript()
			{
				try
				{
					float num = ConcatFileDataSource.access_800(this.this_0, ConcatFileDataSource.access_700(this.this_0));
					ConcatFileDataSource.access_702(this.this_0, ConcatFileDataSource.access_700(this.this_0) + ConcatFileDataSource.access_1000(this.this_0));
					float num2 = ConcatFileDataSource.access_800(this.this_0, ConcatFileDataSource.access_700(this.this_0));
					ConcatFileDataSource.access_300(this.this_0).write(new StringBuilder().append(ConcatFileDataSource.access_900(this.this_0)).append(" 1 ").append("inter_segment_gap").append(' ').append(num).append(' ').append(num2).append(" \n").toString());
					ConcatFileDataSource.access_300(this.this_0).flush();
				}
				catch (IOException ex)
				{
					Throwable.instancehelper_printStackTrace(ex);
				}
			}

			private void writeTranscript(string text, string text2)
			{
				try
				{
					File file = new File(text);
					float num = ConcatFileDataSource.access_800(this.this_0, ConcatFileDataSource.access_700(this.this_0));
					ConcatFileDataSource.access_702(this.this_0, ConcatFileDataSource.access_700(this.this_0) + file.length());
					float num2 = ConcatFileDataSource.access_800(this.this_0, ConcatFileDataSource.access_700(this.this_0));
					ConcatFileDataSource.access_300(this.this_0).write(new StringBuilder().append(ConcatFileDataSource.access_900(this.this_0)).append(" 1 ").append(text).append(' ').append(num).append(' ').append(num2).append("  ").append(text2).append('\n').toString());
					ConcatFileDataSource.access_300(this.this_0).flush();
				}
				catch (IOException ex)
				{
					Throwable.instancehelper_printStackTrace(ex);
				}
			}

			public InputStream nextElement()
			{
				FileInputStream fileInputStream = null;
				if (ConcatFileDataSource.access_200(this.this_0) == null)
				{
					ConcatFileDataSource.access_202(this.this_0, this.readNext());
				}
				if (ConcatFileDataSource.access_200(this.this_0) != null)
				{
					try
					{
						fileInputStream = new FileInputStream(ConcatFileDataSource.access_200(this.this_0));
						ConcatFileDataSource.access_202(this.this_0, null);
					}
					catch (IOException ex)
					{
						Throwable.instancehelper_printStackTrace(ex);

						throw new Error(new StringBuilder().append("Cannot convert ").append(ConcatFileDataSource.access_200(this.this_0)).append(" to a FileInputStream").toString());

					}
				}
				if (fileInputStream == null && ConcatFileDataSource.access_300(this.this_0) != null)
				{
					try
					{
						ConcatFileDataSource.access_300(this.this_0).close();
					}
					catch (IOException ex4)
					{
						Throwable.instancehelper_printStackTrace(ex4);
					}
					return fileInputStream;
				}
				return fileInputStream;
			}

			internal InputStreamEnumeration(ConcatFileDataSource concatFileDataSource, string text, int num, int num2)
			{
				this_0 = concatFileDataSource;
				this.totalFiles = num2;
				this.reader = new BufferedReader(new FileReader(text));
				if (ConcatFileDataSource.access_000(concatFileDataSource) != null)
				{
					this.inSilence = true;
					this.silenceRandom = new java.util.Random(java.lang.System.currentTimeMillis());
					ConcatFileDataSource.access_102(concatFileDataSource, this.getSilenceCount());
				}
				for (int i = 1; i < num; i++)
				{
					this.reader.readLine();
				}
			}

			public bool hasMoreElements()
			{
				if (ConcatFileDataSource.access_200(this.this_0) == null)
				{
					ConcatFileDataSource.access_202(this.this_0, this.readNext());
				}
				return ConcatFileDataSource.access_200(this.this_0) != null;
			}

			object Enumeration.nextElement()
			{
				return this.nextElement();
			}

			private int totalFiles;

			private bool inSilence;

			private java.util.Random silenceRandom;

			private BufferedReader reader;

			internal ConcatFileDataSource this_0;
		}
	}
}
