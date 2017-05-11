using System;

using edu.cmu.sphinx.util;
using IKVM.Attributes;
using ikvm.@internal;
using IKVM.Runtime;
using java.io;
using java.lang;
using java.net;
using java.util;
using java.util.logging;
using javax.sound.sampled;

namespace edu.cmu.sphinx.frontend.util
{
	[Implements(new string[]
	{
		"edu.cmu.sphinx.util.ReferenceSource"
	})]
	public class ConcatAudioFileDataSource : AudioFileDataSource, ReferenceSource
	{
		
		public new static void __<clinit>()
		{
		}

		
		
		internal static URL access_000(ConcatAudioFileDataSource concatAudioFileDataSource)
		{
			return concatAudioFileDataSource.nextFile;
		}

		
		
		internal static URL access_002(ConcatAudioFileDataSource concatAudioFileDataSource, URL result)
		{
			concatAudioFileDataSource.nextFile = result;
			return result;
		}

		
		
		internal static bool access_100(ConcatAudioFileDataSource concatAudioFileDataSource)
		{
			return concatAudioFileDataSource.isInitialized;
		}

		
		
		internal static bool access_102(ConcatAudioFileDataSource concatAudioFileDataSource, bool result)
		{
			concatAudioFileDataSource.isInitialized = result;
			return result;
		}

		
		
		internal static Logger access_200(ConcatAudioFileDataSource concatAudioFileDataSource)
		{
			return concatAudioFileDataSource.logger;
		}

		
		
		internal static Logger access_300(ConcatAudioFileDataSource concatAudioFileDataSource)
		{
			return concatAudioFileDataSource.logger;
		}

		
		[LineNumberTable(new byte[]
		{
			35,
			103,
			162,
			108,
			166,
			119,
			104,
			115,
			130,
			186,
			2,
			98,
			167,
			117
		})]
		
		private static List readDriver(string text)
		{
			File file = new File(text);
			ArrayList arrayList = null;
			IOException ex2;
			try
			{
				BufferedReader bufferedReader = new BufferedReader(new FileReader(file));
				arrayList = new ArrayList();
				string text2;
				while ((text2 = bufferedReader.readLine()) != null && java.lang.String.instancehelper_length(java.lang.String.instancehelper_trim(text2)) != 0)
				{
					File file2 = new File(text2);
					arrayList.add(file2.toURI().toURL());
				}
				bufferedReader.close();
			}
			catch (IOException ex)
			{
				ex2 = ByteCodeHelper.MapException<IOException>(ex, 1);
				goto IL_65;
			}
			goto IL_74;
			IL_65:
			IOException ex3 = ex2;
			Throwable.instancehelper_printStackTrace(ex3);
			IL_74:
			if (!ConcatAudioFileDataSource.assertionsDisabled && arrayList == null)
			{
				
				throw new AssertionError();
			}
			return arrayList;
		}

		
		[LineNumberTable(new byte[]
		{
			25,
			108,
			102
		})]
		
		public virtual void setBatchUrls(List urls)
		{
			this.batchFiles = new ArrayList(urls);
			this.initialize();
		}

		[LineNumberTable(new byte[]
		{
			159,
			186,
			134,
			104,
			161,
			107,
			191,
			10,
			2,
			97,
			134
		})]
		
		public override void initialize()
		{
			base.initialize();
			if (this.batchFiles == null)
			{
				return;
			}
			IOException ex2;
			try
			{
				this.referenceList = new ArrayList();
				this.dataStream = new SequenceInputStream(new ConcatAudioFileDataSource.InputStreamEnumeration(this, this.batchFiles));
			}
			catch (IOException ex)
			{
				ex2 = ByteCodeHelper.MapException<IOException>(ex, 1);
				goto IL_40;
			}
			return;
			IL_40:
			IOException ex3 = ex2;
			Throwable.instancehelper_printStackTrace(ex3);
		}

		
		[LineNumberTable(new byte[]
		{
			159,
			176,
			104
		})]
		
		public ConcatAudioFileDataSource(int bytesPerRead, List listeners) : base(bytesPerRead, listeners)
		{
		}

		[LineNumberTable(new byte[]
		{
			159,
			179,
			134
		})]
		
		public ConcatAudioFileDataSource()
		{
		}

		[LineNumberTable(new byte[]
		{
			8,
			113
		})]
		
		public virtual void setBatchFile(File file)
		{
			this.setBatchUrls(ConcatAudioFileDataSource.readDriver(file.getAbsolutePath()));
		}

		
		[LineNumberTable(new byte[]
		{
			12,
			166,
			123,
			191,
			5,
			2,
			98,
			167,
			103
		})]
		
		public virtual void setBatchFiles(List files)
		{
			ArrayList arrayList = new ArrayList();
			MalformedURLException ex2;
			try
			{
				Iterator iterator = files.iterator();
				while (iterator.hasNext())
				{
					File file = (File)iterator.next();
					arrayList.add(file.toURI().toURL());
				}
			}
			catch (MalformedURLException ex)
			{
				ex2 = ByteCodeHelper.MapException<MalformedURLException>(ex, 1);
				goto IL_42;
			}
			goto IL_50;
			IL_42:
			MalformedURLException ex3 = ex2;
			Throwable.instancehelper_printStackTrace(ex3);
			IL_50:
			this.setBatchUrls(arrayList);
		}

		
		
		public override void setAudioFile(URL audioFileURL, string streamName)
		{
			
			throw new UnsupportedOperationException();
		}

		
		public virtual List getReferences()
		{
			return this.referenceList;
		}

		
		static ConcatAudioFileDataSource()
		{
			AudioFileDataSource.__<clinit>();
			ConcatAudioFileDataSource.assertionsDisabled = !ClassLiteral<ConcatAudioFileDataSource>.Value.desiredAssertionStatus();
		}

		private URL nextFile;

		
		private List referenceList;

		private bool isInitialized;

		
		internal List batchFiles;

		
		internal new static bool assertionsDisabled;

		
		[Implements(new string[]
		{
			"java.util.Enumeration"
		})]
		
		.
		
		internal sealed class InputStreamEnumeration : java.lang.Object, Enumeration
		{
			[LineNumberTable(new byte[]
			{
				160,
				98,
				107,
				159,
				28,
				127,
				6,
				157,
				167,
				109,
				150
			})]
			
			public URL readNext()
			{
				if (this.lastFile != null)
				{
					ConcatAudioFileDataSource.access_300(this.this_0).finest(new StringBuilder().append("Finished processing of '").append(this.lastFile.getFile()).append('\'').toString());
					Iterator iterator = this.this_0.__fileListeners.iterator();
					while (iterator.hasNext())
					{
						AudioFileProcessListener audioFileProcessListener = (AudioFileProcessListener)iterator.next();
						AudioFileProcessListener audioFileProcessListener2 = audioFileProcessListener;
						File.__<clinit>();
						audioFileProcessListener2.audioFileProcFinished(new File(this.lastFile.getFile()));
					}
					this.lastFile = null;
				}
				if (this.fileIt.hasNext())
				{
					this.lastFile = (URL)this.fileIt.next();
				}
				return this.lastFile;
			}

			[LineNumberTable(new byte[]
			{
				107,
				98,
				104,
				178,
				144,
				102,
				203,
				103,
				109,
				141,
				113,
				118,
				113,
				106,
				179,
				117,
				105,
				114,
				176,
				98,
				127,
				1,
				54,
				133,
				127,
				7,
				159,
				4,
				113,
				255,
				16,
				71,
				226,
				58,
				98,
				103,
				159,
				26,
				98,
				199
			})]
			
			public AudioInputStream nextElement()
			{
				AudioInputStream result = null;
				if (this.lastFile == null)
				{
					ConcatAudioFileDataSource.access_002(this.this_0, this.readNext());
				}
				if (ConcatAudioFileDataSource.access_000(this.this_0) != null)
				{
					IOException ex2;
					UnsupportedAudioFileException ex4;
					try
					{
						try
						{
							AudioInputStream audioInputStream = AudioSystem.getAudioInputStream(ConcatAudioFileDataSource.access_000(this.this_0));
							AudioFormat format = audioInputStream.getFormat();
							if (!ConcatAudioFileDataSource.access_100(this.this_0))
							{
								ConcatAudioFileDataSource.access_102(this.this_0, true);
								this.this_0.bigEndian = format.isBigEndian();
								this.this_0.sampleRate = ByteCodeHelper.f2i(format.getSampleRate());
								this.this_0.signedData = format.getEncoding().equals(AudioFormat.Encoding.PCM_SIGNED);
								this.this_0.bytesPerValue = format.getSampleSizeInBits() / 8;
							}
							if (format.getSampleRate() != (float)this.this_0.sampleRate || format.getChannels() != 1 || format.isBigEndian() != this.this_0.bigEndian)
							{
								string text = "format mismatch for subsequent files";
								
								throw new RuntimeException(text);
							}
							result = audioInputStream;
							ConcatAudioFileDataSource.access_200(this.this_0).finer(new StringBuilder().append("Strating processing of '").append(this.lastFile.getFile()).append('\'').toString());
							Iterator iterator = this.this_0.__fileListeners.iterator();
							while (iterator.hasNext())
							{
								AudioFileProcessListener audioFileProcessListener = (AudioFileProcessListener)iterator.next();
								AudioFileProcessListener audioFileProcessListener2 = audioFileProcessListener;
								File.__<clinit>();
								audioFileProcessListener2.audioFileProcStarted(new File(ConcatAudioFileDataSource.access_000(this.this_0).getFile()));
							}
							this.lastFile = ConcatAudioFileDataSource.access_000(this.this_0);
							ConcatAudioFileDataSource.access_002(this.this_0, null);
						}
						catch (IOException ex)
						{
							ex2 = ByteCodeHelper.MapException<IOException>(ex, 1);
							goto IL_1B1;
						}
					}
					catch (UnsupportedAudioFileException ex3)
					{
						ex4 = ByteCodeHelper.MapException<UnsupportedAudioFileException>(ex3, 1);
						goto IL_1B5;
					}
					return result;
					IL_1B1:
					IOException ex5 = ex2;
					Throwable.instancehelper_printStackTrace(ex5);
					string text2 = new StringBuilder().append("Cannot convert ").append(ConcatAudioFileDataSource.access_000(this.this_0)).append(" to a FileInputStream").toString();
					
					throw new Error(text2);
					IL_1B5:
					UnsupportedAudioFileException ex6 = ex4;
					Throwable.instancehelper_printStackTrace(ex6);
				}
				return result;
			}

			[Throws(new string[]
			{
				"java.io.IOException"
			})]
			
			[LineNumberTable(new byte[]
			{
				83,
				111,
				113
			})]
			
			internal InputStreamEnumeration(ConcatAudioFileDataSource concatAudioFileDataSource, List list)
			{
				this.fileIt = new ArrayList(list).iterator();
			}

			[LineNumberTable(new byte[]
			{
				94,
				109,
				146
			})]
			
			public bool hasMoreElements()
			{
				if (ConcatAudioFileDataSource.access_000(this.this_0) == null)
				{
					ConcatAudioFileDataSource.access_002(this.this_0, this.readNext());
				}
				return ConcatAudioFileDataSource.access_000(this.this_0) != null;
			}

			
			
			
			public object nextElement()
			{
				return this.nextElement();
			}

			private URL lastFile;

			
			
			internal Iterator fileIt;

			
			internal ConcatAudioFileDataSource this_0 = concatAudioFileDataSource;
		}
	}
}
