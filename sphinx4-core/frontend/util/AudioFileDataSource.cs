using edu.cmu.sphinx.util.props;
using ikvm.@internal;
using IKVM.Runtime;
using java.io;
using java.lang;
using java.net;
using java.util;
using javax.sound.sampled;

namespace edu.cmu.sphinx.frontend.util
{
	public class AudioFileDataSource : BaseDataProcessor
	{	
		private void create(int num, List list)
		{
			this.bytesPerRead = num;
			if (list != null)
			{
				Iterator iterator = list.iterator();
				while (iterator.hasNext())
				{
					AudioFileProcessListener l = (AudioFileProcessListener)iterator.next();
					this.addNewFileListener(l);
				}
			}
			this.initialize();
		}
		
		public virtual void addNewFileListener(AudioFileProcessListener l)
		{
			if (l == null)
			{
				return;
			}
			this.__fileListeners.add(l);
		}
		
		public override void initialize()
		{
			base.initialize();
			this.streamEndReached = false;
			this.utteranceEndSent = false;
			this.utteranceStarted = false;
			int num = this.bytesPerRead;
			int num2 = 2;
			if (((num2 != -1) ? (num % num2) : 0) == 1)
			{
				this.bytesPerRead++;
			}
		}

		public virtual void setAudioFile(URL audioFileURL, string streamName)
		{
			if (this.dataStream != null)
			{
				IOException ex2;
				try
				{
					this.dataStream.close();
				}
				catch (IOException ex)
				{
					ex2 = ex;
					goto IL_22;
				}
				goto IL_2E;
				IL_22:
				IOException ex3 = ex2;
				Throwable.instancehelper_printStackTrace(ex3);
				IL_2E:
				this.dataStream = null;
			}
			if (!AudioFileDataSource.assertionsDisabled && audioFileURL == null)
			{
				
				throw new AssertionError();
			}
			if (streamName != null)
			{
				streamName = audioFileURL.getPath();
			}
			AudioInputStream inputStream = null;
			UnsupportedAudioFileException ex5;
			IOException ex7;
			try
			{
				try
				{
					inputStream = AudioSystem.getAudioInputStream(audioFileURL);
				}
				catch (UnsupportedAudioFileException ex4)
				{
					ex5 = ex4;
					goto IL_77;
				}
			}
			catch (IOException ex6)
			{
				ex7 = ex6;
				goto IL_7A;
			}
			goto IL_B9;
			IL_77:
			UnsupportedAudioFileException ex8 = ex5;
			java.lang.System.err.println(new StringBuilder().append("Audio file format not supported: ").append(ex8).toString());
			Throwable.instancehelper_printStackTrace(ex8);
			goto IL_B9;
			IL_7A:
			IOException ex9 = ex7;
			Throwable.instancehelper_printStackTrace(ex9);
			IL_B9:
			this.curAudioFile = new File(audioFileURL.getFile());
			Iterator iterator = this.__fileListeners.iterator();
			while (iterator.hasNext())
			{
				AudioFileProcessListener audioFileProcessListener = (AudioFileProcessListener)iterator.next();
				audioFileProcessListener.audioFileProcStarted(this.curAudioFile);
			}
			this.setInputStream(inputStream, streamName);
		}
		
		public virtual void setInputStream(AudioInputStream inputStream, string streamName)
		{
			this.dataStream = inputStream;
			this.streamEndReached = false;
			this.utteranceEndSent = false;
			this.utteranceStarted = false;
			AudioFormat format = inputStream.getFormat();
			this.sampleRate = ByteCodeHelper.f2i(format.getSampleRate());
			this.bigEndian = format.isBigEndian();
			string text = format.toString();
			this.logger.finer(new StringBuilder().append("input format is ").append(text).toString());
			bool sampleSizeInBits = format.getSampleSizeInBits() != 0;
			int num = 8;
			if (num != -1 && (sampleSizeInBits ? 1 : 0) % num != 0)
			{
				string text2 = "StreamDataSource: bits per sample must be a multiple of 8.";
				
				throw new Error(text2);
			}
			this.bytesPerValue = format.getSampleSizeInBits() / 8;
			AudioFormat.Encoding encoding = format.getEncoding();
			if (encoding.equals(AudioFormat.Encoding.PCM_SIGNED))
			{
				this.signedData = true;
			}
			else
			{
				if (!encoding.equals(AudioFormat.Encoding.PCM_UNSIGNED))
				{
					string text3 = "used file encoding is not supported";
					
					throw new RuntimeException(text3);
				}
				this.signedData = false;
			}
			this.totalValuesRead = 0L;
		}
		
		private DataEndSignal createDataEndSignal()
		{
			if (!(this is ConcatAudioFileDataSource))
			{
				Iterator iterator = this.__fileListeners.iterator();
				while (iterator.hasNext())
				{
					AudioFileProcessListener audioFileProcessListener = (AudioFileProcessListener)iterator.next();
					audioFileProcessListener.audioFileProcFinished(this.curAudioFile);
				}
			}
			return new DataEndSignal(this.getDuration());
		}
		
		private Data readNextFrame()
		{
			int num = 0;
			int num2 = this.bytesPerRead;
			byte[] array = new byte[this.bytesPerRead];
			long firstSampleNumber = this.totalValuesRead;
			Data result;
			try
			{
				int num3;
				do
				{
					num3 = this.dataStream.read(array, num, num2 - num);
					if (num3 > 0)
					{
						num += num3;
					}
				}
				while (num3 != -1 && num < num2);
				if (num > 0)
				{
					long num4 = this.totalValuesRead;
					int num5 = num;
					int num6 = this.bytesPerValue;
					this.totalValuesRead = num4 + (long)((num6 != -1) ? (num5 / num6) : (-(long)num5));
					if (num < num2)
					{
						bool flag = num != 0;
						int num7 = 2;
						num = ((num7 != -1 && (flag ? 1 : 0) % num7 != 0) ? (num + 3) : (num + 2));
						byte[] array2 = new byte[num];
						ByteCodeHelper.arraycopy_primitive_1(array, 0, array2, 0, num);
						array = array2;
						this.closeDataStream();
					}
					goto IL_B9;
				}
				this.closeDataStream();
				result = null;
			}
			catch (IOException ex)
			{
				throw new DataProcessingException("Error reading data", ex);
			}
			return result;
			IL_B9:
			double[] values;
			if (this.bigEndian)
			{
				values = DataUtil.bytesToValues(array, 0, num, this.bytesPerValue, this.signedData);
			}
			else
			{
				values = DataUtil.littleEndianBytesToValues(array, 0, num, this.bytesPerValue, this.signedData);
			}
			return new DoubleData(values, this.sampleRate, firstSampleNumber);			
		}
		private long getDuration()
		{
			return ByteCodeHelper.d2l((double)this.totalValuesRead / (double)this.sampleRate * 1000.0);
		}
		
		private void closeDataStream()
		{
			this.streamEndReached = true;
			if (this.dataStream != null)
			{
				this.dataStream.close();
			}
		}
		
		public AudioFileDataSource(int bytesPerRead, List listeners)
		{
			this.__fileListeners = new ArrayList();
			this.initLogger();
			this.create(bytesPerRead, listeners);
		}
		
		public AudioFileDataSource()
		{
			this.__fileListeners = new ArrayList();
		}
		
		public override void newProperties(PropertySheet ps)
		{
			base.newProperties(ps);
			this.logger = ps.getLogger();
			this.create(ps.getInt("bytesPerRead"), ps.getComponentList("audioFileListners", ClassLiteral<AudioFileProcessListener>.Value));
		}
		
		public virtual void setAudioFile(File audioFile, string streamName)
		{
			try
			{
				this.setAudioFile(audioFile.toURI().toURL(), streamName);
			}
			catch (MalformedURLException ex)
			{
				Throwable.instancehelper_printStackTrace(ex);
			}
		}
		
		public override Data getData()
		{
			object obj = null;
			if (this.streamEndReached)
			{
				if (!this.utteranceEndSent)
				{
					obj = this.createDataEndSignal();
					this.utteranceEndSent = true;
				}
			}
			else if (!this.utteranceStarted)
			{
				this.utteranceStarted = true;
				obj = new DataStartSignal(this.sampleRate);
			}
			else if (this.dataStream != null)
			{
				obj = this.readNextFrame();
				if ((Data)obj == null && !this.utteranceEndSent)
				{
					obj = this.createDataEndSignal();
					this.utteranceEndSent = true;
				}
			}
			object obj2 = obj;
			Data result;
			if (obj2 != null)
			{
				if ((result = (obj2 as Data)) == null)
				{
					throw new IncompatibleClassChangeError();
				}
			}
			else
			{
				result = null;
			}
			return result;
		}
		public virtual int getSampleRate()
		{
			return this.sampleRate;
		}

		public virtual bool isBigEndian()
		{
			return this.bigEndian;
		}
		
		public virtual void removeNewFileListener(AudioFileProcessListener l)
		{
			if (l == null)
			{
				return;
			}
			this.__fileListeners.remove(l);
		}
		
		protected internal List fileListeners
		{
			
			get
			{
				return this.__fileListeners;
			}
			
			private set
			{
				this.__fileListeners = value;
			}
		}

		[S4Integer(new object[]
		{
			64,
			"Ledu/cmu/sphinx/util/props/S4Integer;",
			"defaultValue",
			3200
		})]
		public const string PROP_BYTES_PER_READ = "bytesPerRead";

		[S4ComponentList(new object[]
		{
			64,
			"Ledu/cmu/sphinx/util/props/S4ComponentList;",
			"type",
			new object[]
			{
				99,
				"Ledu/cmu/sphinx/util/props/Configurable, sphinx4-core-5prealpha-SNAPSHOT, Version=0/0/0/0, Culture=neutral, PublicKeyToken=null;"
			}
		})]
		public const string AUDIO_FILE_LISTENERS = "audioFileListners";
		
		internal List __fileListeners;

		protected internal InputStream dataStream;

		protected internal int sampleRate;

		protected internal int bytesPerRead;

		protected internal int bytesPerValue;

		private long totalValuesRead;

		protected internal bool bigEndian;

		protected internal bool signedData;

		private bool streamEndReached;

		private bool utteranceEndSent;

		private bool utteranceStarted;

		private File curAudioFile;
		
		internal static bool assertionsDisabled = !ClassLiteral<AudioFileDataSource>.Value.desiredAssertionStatus();
	}
}
