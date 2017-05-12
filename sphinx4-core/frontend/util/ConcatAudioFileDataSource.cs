using edu.cmu.sphinx.util;
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
	public class ConcatAudioFileDataSource : AudioFileDataSource, ReferenceSource
	{		
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
				while ((text2 = bufferedReader.readLine()) != null && String.instancehelper_length(String.instancehelper_trim(text2)) != 0)
				{
					File file2 = new File(text2);
					arrayList.add(file2.toURI().toURL());
				}
				bufferedReader.close();
			}
			catch (IOException ex)
			{
				ex2 = ex;
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

		public virtual void setBatchUrls(List urls)
		{
			this.batchFiles = new ArrayList(urls);
			this.initialize();
		}
		
		public override void initialize()
		{
			base.initialize();
			if (this.batchFiles == null)
			{
				return;
			}
			try
			{
				this.referenceList = new ArrayList();
				this.dataStream = new SequenceInputStream(new ConcatAudioFileDataSource.InputStreamEnumeration(this, this.batchFiles));
			}
			catch (IOException ex)
			{
				Throwable.instancehelper_printStackTrace(ex);
			}
		}
		
		public ConcatAudioFileDataSource(int bytesPerRead, List listeners) : base(bytesPerRead, listeners)
		{
		}
		
		public ConcatAudioFileDataSource()
		{
		}
		
		public virtual void setBatchFile(File file)
		{
			this.setBatchUrls(ConcatAudioFileDataSource.readDriver(file.getAbsolutePath()));
		}
		
		public virtual void setBatchFiles(List files)
		{
			ArrayList arrayList = new ArrayList();
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
				Throwable.instancehelper_printStackTrace(ex);
			}
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
			ConcatAudioFileDataSource.assertionsDisabled = !ClassLiteral<ConcatAudioFileDataSource>.Value.desiredAssertionStatus();
		}

		private URL nextFile;
	
		private List referenceList;

		private bool isInitialized;

		internal List batchFiles;
		
		internal new static bool assertionsDisabled;
		
		internal sealed class InputStreamEnumeration : Object, Enumeration
		{
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
			
			public AudioInputStream nextElement()
			{
				AudioInputStream result = null;
				if (this.lastFile == null)
				{
					ConcatAudioFileDataSource.access_002(this.this_0, this.readNext());
				}
				if (ConcatAudioFileDataSource.access_000(this.this_0) != null)
				{
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
								audioFileProcessListener2.audioFileProcStarted(new File(ConcatAudioFileDataSource.access_000(this.this_0).getFile()));
							}
							this.lastFile = ConcatAudioFileDataSource.access_000(this.this_0);
							ConcatAudioFileDataSource.access_002(this.this_0, null);
						}
						catch (IOException ex)
						{
							Throwable.instancehelper_printStackTrace(ex);

							throw new Error(new StringBuilder().append("Cannot convert ").append(ConcatAudioFileDataSource.access_000(this.this_0)).append(" to a FileInputStream").toString());
						}
					}
					catch (UnsupportedAudioFileException ex3)
					{
						Throwable.instancehelper_printStackTrace(ex3);
					}
					return result;
				}
				return result;
			}
			
			internal InputStreamEnumeration(ConcatAudioFileDataSource concatAudioFileDataSource, List list)
			{
				this_0 = concatAudioFileDataSource;
				this.fileIt = new ArrayList(list).iterator();
			}

			public bool hasMoreElements()
			{
				if (ConcatAudioFileDataSource.access_000(this.this_0) == null)
				{
					ConcatAudioFileDataSource.access_002(this.this_0, this.readNext());
				}
				return ConcatAudioFileDataSource.access_000(this.this_0) != null;
			}

			object Enumeration.nextElement()
			{
				return this.nextElement();
			}

			private URL lastFile;
			
			internal Iterator fileIt;

			internal ConcatAudioFileDataSource this_0;
		}
	}
}
