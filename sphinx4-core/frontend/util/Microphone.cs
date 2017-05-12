using edu.cmu.sphinx.util.props;
using ikvm.@internal;
using IKVM.Runtime;
using java.io;
using java.lang;
using java.util;
using java.util.concurrent;
using java.util.logging;
using javax.sound.sampled;

namespace edu.cmu.sphinx.frontend.util
{
	public class Microphone : BaseDataProcessor
	{
		internal static Logger access_000(Microphone microphone)
		{
			return microphone.logger;
		}

		internal static TargetDataLine access_100(Microphone microphone)
		{
			return microphone.audioLine;
		}

		internal static Logger access_200(Microphone microphone)
		{
			return microphone.logger;
		}

		internal static bool access_300(Microphone microphone)
		{
			return microphone.keepDataReference;
		}

		internal static AudioInputStream access_500(Microphone microphone)
		{
			return microphone.audioStream;
		}
		
		internal static Utterance access_402(Microphone microphone, Utterance result)
		{
			microphone.currentUtterance = result;
			return result;
		}

		internal static BlockingQueue access_700(Microphone microphone)
		{
			return microphone.audioList;
		}

		internal static int access_600(Microphone microphone)
		{
			return microphone.sampleRate;
		}

		internal static Logger access_800(Microphone microphone)
		{
			return microphone.logger;
		}

		internal static Utterance access_400(Microphone microphone)
		{
			return microphone.currentUtterance;
		}

		internal static bool access_900(Microphone microphone)
		{
			return microphone.closeBetweenUtterances;
		}

		internal static TargetDataLine access_102(Microphone microphone, TargetDataLine result)
		{
			microphone.audioLine = result;
			return result;
		}

		internal static Logger access_1000(Microphone microphone)
		{
			return microphone.logger;
		}

		internal static Logger access_1100(Microphone microphone)
		{
			return microphone.logger;
		}
		
		internal static Logger access_1200(Microphone microphone)
		{
			return microphone.logger;
		}

		internal static Logger access_1300(Microphone microphone)
		{
			return microphone.logger;
		}

		internal static int access_1400(Microphone microphone)
		{
			return microphone.frameSizeInBytes;
		}

		internal static Logger access_1500(Microphone microphone)
		{
			return microphone.logger;
		}

		internal static Logger access_1600(Microphone microphone)
		{
			return microphone.logger;
		}

		internal static bool access_1700(Microphone microphone)
		{
			return microphone.bigEndian;
		}

		internal static bool access_1800(Microphone microphone)
		{
			return microphone.signed;
		}
		
		internal static double[] access_1900(Microphone microphone, double[] array, int num)
		{
			return microphone.convertStereoToMono(array, num);
		}

		private double[] convertStereoToMono(double[] array, int num)
		{
			if (!Microphone.assertionsDisabled)
			{
				bool flag = array.Length != 0;
				if (num != -1 && (flag ? 1 : 0) % num != 0)
				{
					
					throw new AssertionError();
				}
			}
			int num2 = array.Length;
			double[] array2 = new double[(num != -1) ? (num2 / num) : (-num2)];
			if (java.lang.String.instancehelper_equals(this.stereoToMono, "average"))
			{
				int i = 0;
				int num3 = 0;
				while (i < array.Length)
				{
					int num4 = i;
					i++;
					double num5 = array[num4];
					for (int j = 1; j < num; j++)
					{
						double num6 = num5;
						int num7 = i;
						i++;
						num5 = num6 + array[num7];
					}
					array2[num3] = num5 / (double)num;
					num3++;
				}
			}
			else
			{
				if (!java.lang.String.instancehelper_equals(this.stereoToMono, "selectChannel"))
				{
					string text = new StringBuilder().append("Unsupported stereo to mono conversion: ").append(this.stereoToMono).toString();
					
					throw new Error(text);
				}
				int i = this.selectedChannel;
				int num3 = 0;
				while (i < array.Length)
				{
					array2[num3] = array[i];
					i += num;
					num3++;
				}
			}
			return array2;
		}

		private Mixer getSelectedMixer()
		{
			if (java.lang.String.instancehelper_equals(this.selectedMixerIndex, "default"))
			{
				return null;
			}
			Mixer.Info[] mixerInfo = AudioSystem.getMixerInfo();
			if (java.lang.String.instancehelper_equals(this.selectedMixerIndex, "last"))
			{
				return AudioSystem.getMixer(mixerInfo[mixerInfo.Length - 1]);
			}
			int num = Integer.parseInt(this.selectedMixerIndex);
			return AudioSystem.getMixer(mixerInfo[num]);
		}
	
		private TargetDataLine getAudioLine()
		{
			if (this.audioLine != null)
			{
				return this.audioLine;
			}
			try
			{
				this.logger.info(new StringBuilder().append("Final format: ").append(this.finalFormat).toString());
				DataLine.Info info = new DataLine.Info(ClassLiteral<TargetDataLine>.Value, this.finalFormat);
				Mixer selectedMixer = this.getSelectedMixer();
				if (selectedMixer == null)
				{
					this.audioLine = (TargetDataLine)AudioSystem.getLine(info);
				}
				else
				{
					this.audioLine = (TargetDataLine)selectedMixer.getLine(info);
				}
				this.audioLine.addLineListener(new Microphone_1(this));
			}
			catch (LineUnavailableException ex)
			{
				this.logger.severe(new StringBuilder().append("microphone unavailable ").append(Throwable.instancehelper_getMessage(ex)).toString());
			}
			return this.audioLine;
		}
	
		private bool open()
		{
			TargetDataLine targetDataLine = this.getAudioLine();
			if (targetDataLine != null)
			{
				if (!targetDataLine.isOpen())
				{
					this.logger.info("open");
					try
					{
						targetDataLine.open(this.finalFormat, this.audioBufferSize);
					}
					catch (LineUnavailableException ex)
					{
						this.logger.severe(new StringBuilder().append("Can't open microphone ").append(Throwable.instancehelper_getMessage(ex)).toString());
						return false;
					}
					this.audioStream = new AudioInputStream(targetDataLine);
					if (this.doConversion)
					{
						this.audioStream = AudioSystem.getAudioInputStream(this.desiredFormat, this.audioStream);
						if (!Microphone.assertionsDisabled && this.audioStream == null)
						{
							throw new AssertionError();
						}
					}
					float num = (float)this.msecPerRead / 1000f;
					this.frameSizeInBytes = this.audioStream.getFormat().getSampleSizeInBits() / 8 * ByteCodeHelper.f2i(num * this.audioStream.getFormat().getSampleRate()) * this.desiredFormat.getChannels();
					this.logger.info(new StringBuilder().append("Frame size: ").append(this.frameSizeInBytes).append(" bytes").toString());
					return true;

				}
				return true;
			}
			this.logger.severe("Can't find microphone");
			return false;
		}
	
		public Microphone(int sampleRate, int bitsPerSample, int channels, bool bigEndian, bool signed, bool closeBetweenUtterances, int msecPerRead, bool keepLastAudio, string stereoToMono, int selectedChannel, string selectedMixerIndex, int audioBufferSize)
		{
			this.utteranceEndReached = true;
			System.Threading.Thread.MemoryBarrier();
			this.initLogger();
			this.sampleRate = sampleRate;
			this.bigEndian = bigEndian;
			this.signed = signed;
			this.desiredFormat = new AudioFormat((float)sampleRate, bitsPerSample, channels, signed, bigEndian);
			this.closeBetweenUtterances = closeBetweenUtterances;
			this.msecPerRead = msecPerRead;
			this.keepDataReference = keepLastAudio;
			this.stereoToMono = stereoToMono;
			this.selectedChannel = selectedChannel;
			this.selectedMixerIndex = selectedMixerIndex;
			this.audioBufferSize = audioBufferSize;
		}
	
		public Microphone()
		{
			this.utteranceEndReached = true;
			System.Threading.Thread.MemoryBarrier();
		}
	
		public override void newProperties(PropertySheet ps)
		{
			base.newProperties(ps);
			this.logger = ps.getLogger();
			this.sampleRate = ps.getInt("sampleRate");
			int @int = ps.getInt("bitsPerSample");
			int int2 = ps.getInt("channels");
			this.bigEndian = ps.getBoolean("bigEndian").booleanValue();
			this.signed = ps.getBoolean("signed").booleanValue();
			this.desiredFormat = new AudioFormat((float)this.sampleRate, @int, int2, this.signed, this.bigEndian);
			this.closeBetweenUtterances = ps.getBoolean("closeBetweenUtterances").booleanValue();
			this.msecPerRead = ps.getInt("msecPerRead");
			this.keepDataReference = ps.getBoolean("keepLastAudio").booleanValue();
			this.stereoToMono = ps.getString("stereoToMono");
			this.selectedChannel = ps.getInt("selectChannel");
			this.selectedMixerIndex = ps.getString("selectMixer");
			this.audioBufferSize = ps.getInt("bufferSize");
		}
		
		public override void initialize()
		{
			base.initialize();
			this.audioList = new LinkedBlockingQueue();
			DataLine.Info info = new DataLine.Info(ClassLiteral<TargetDataLine>.Value, this.desiredFormat);
			if (!AudioSystem.isLineSupported(info))
			{
				this.logger.info(new StringBuilder().append(this.desiredFormat).append(" not supported").toString());
				AudioFormat nativeAudioFormat = DataUtil.getNativeAudioFormat(this.desiredFormat, this.getSelectedMixer());
				if (nativeAudioFormat == null)
				{
					this.logger.severe("couldn't find suitable target audio format");
				}
				else
				{
					this.finalFormat = nativeAudioFormat;
					this.doConversion = AudioSystem.isConversionSupported(this.desiredFormat, nativeAudioFormat);
					if (this.doConversion)
					{
						this.logger.info(new StringBuilder().append("Converting from ").append(this.finalFormat.getSampleRate()).append("Hz to ").append(this.desiredFormat.getSampleRate()).append("Hz").toString());
					}
					else
					{
						this.logger.info(new StringBuilder().append("Using native format: Cannot convert from ").append(this.finalFormat.getSampleRate()).append("Hz to ").append(this.desiredFormat.getSampleRate()).append("Hz").toString());
					}
				}
			}
			else
			{
				this.logger.info(new StringBuilder().append("Desired format: ").append(this.desiredFormat).append(" supported.").toString());
				this.finalFormat = this.desiredFormat;
			}
		}

		public virtual AudioFormat getAudioFormat()
		{
			return this.finalFormat;
		}

		public virtual Utterance getUtterance()
		{
			return this.currentUtterance;
		}

		public virtual bool isRecording()
		{
			return this.recording;
		}
		
		public virtual bool startRecording()
		{
			if (this.recording)
			{
				return false;
			}
			if (!this.open())
			{
				return false;
			}
			this.utteranceEndReached = false;
			System.Threading.Thread.MemoryBarrier();
			if (this.audioLine.isRunning())
			{
				this.logger.severe("Whoops: audio line is running");
			}
			if (!Microphone.assertionsDisabled && this.recorder != null)
			{
				
				throw new AssertionError();
			}
			this.recorder = new Microphone.RecordingThread(this, "Microphone");
			this.recorder.start();
			this.recording = true;
			System.Threading.Thread.MemoryBarrier();
			return true;
		}
	
		public virtual void stopRecording()
		{
			if (this.audioLine != null)
			{
				if (this.recorder != null)
				{
					this.recorder.stopRecording();
					this.recorder = null;
				}
				this.recording = false;
				System.Threading.Thread.MemoryBarrier();
			}
		}
	
		public virtual void clear()
		{
			this.audioList.clear();
		}
	
		public override Data getData()
		{
			Data data = null;
			if (!this.utteranceEndReached)
			{
				try
				{
					data = (Data)this.audioList.take();
				}
				catch (InterruptedException ex)
				{
					throw new DataProcessingException("cannot take Data from audioList", ex);
				}
				if (data is DataEndSignal)
				{
					this.utteranceEndReached = true;
					System.Threading.Thread.MemoryBarrier();
					return data;
				}
				return data;
			}
			return data;
		}

		public virtual bool hasMoreData()
		{
			return !this.utteranceEndReached || !this.audioList.isEmpty();
		}

		[S4Integer(new object[]
		{
			64,
			"Ledu/cmu/sphinx/util/props/S4Integer;",
			"defaultValue",
			16000
		})]
		public const string PROP_SAMPLE_RATE = "sampleRate";

		[S4Boolean(new object[]
		{
			64,
			"Ledu/cmu/sphinx/util/props/S4Boolean;",
			"defaultValue",
			true
		})]
		public const string PROP_CLOSE_BETWEEN_UTTERANCES = "closeBetweenUtterances";

		[S4Integer(new object[]
		{
			64,
			"Ledu/cmu/sphinx/util/props/S4Integer;",
			"defaultValue",
			10
		})]
		public const string PROP_MSEC_PER_READ = "msecPerRead";

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
		public const string PROP_CHANNELS = "channels";

		[S4Boolean(new object[]
		{
			64,
			"Ledu/cmu/sphinx/util/props/S4Boolean;",
			"defaultValue",
			true
		})]
		public const string PROP_BIG_ENDIAN = "bigEndian";

		[S4Boolean(new object[]
		{
			64,
			"Ledu/cmu/sphinx/util/props/S4Boolean;",
			"defaultValue",
			true
		})]
		public const string PROP_SIGNED = "signed";

		[S4Boolean(new object[]
		{
			64,
			"Ledu/cmu/sphinx/util/props/S4Boolean;",
			"defaultValue",
			false
		})]
		public const string PROP_KEEP_LAST_AUDIO = "keepLastAudio";

		[S4String(new object[]
		{
			64,
			"Ledu/cmu/sphinx/util/props/S4String;",
			"defaultValue",
			"average",
			"range",
			new object[]
			{
				91,
				"average",
				"selectChannel"
			}
		})]
		public const string PROP_STEREO_TO_MONO = "stereoToMono";

		[S4Integer(new object[]
		{
			64,
			"Ledu/cmu/sphinx/util/props/S4Integer;",
			"defaultValue",
			0
		})]
		public const string PROP_SELECT_CHANNEL = "selectChannel";

		[S4String(new object[]
		{
			64,
			"Ledu/cmu/sphinx/util/props/S4String;",
			"defaultValue",
			"default"
		})]
		public const string PROP_SELECT_MIXER = "selectMixer";

		[S4Integer(new object[]
		{
			64,
			"Ledu/cmu/sphinx/util/props/S4Integer;",
			"defaultValue",
			6400
		})]
		public const string PROP_BUFFER_SIZE = "bufferSize";

		private AudioFormat finalFormat;

		private AudioInputStream audioStream;

		private TargetDataLine audioLine;

		
		private BlockingQueue audioList;

		private Utterance currentUtterance;

		private bool doConversion;

		private volatile bool recording;

		private volatile bool utteranceEndReached;

		private Microphone.RecordingThread recorder;

		private AudioFormat desiredFormat;

		private bool closeBetweenUtterances;

		private bool keepDataReference;

		private bool signed;

		private bool bigEndian;

		private int frameSizeInBytes;

		private int msecPerRead;

		private int selectedChannel;

		private string selectedMixerIndex;

		private string stereoToMono;

		private int sampleRate;

		private int audioBufferSize;
		
		internal static bool assertionsDisabled = !ClassLiteral<Microphone>.Value.desiredAssertionStatus();
		
		internal sealed class RecordingThread : java.lang.Thread
		{
			private void waitForStart()
			{
				try
				{
					while (!this.started)
					{
						java.lang.Object.instancehelper_wait(this);
					}
				}
				catch (InterruptedException)
				{
					goto IL_18;
				}
				return;
				IL_18:
				Microphone.access_1300(this.this_0).warning("wait was interrupted");
			}
	
			private Data readData(Utterance utterance)
			{
				byte[] array = new byte[Microphone.access_1400(this.this_0)];
				int channels = Microphone.access_500(this.this_0).getFormat().getChannels();
				long num = this.totalSamplesRead;
				long num2 = (long)channels;
				long firstSampleNumber = (num2 != -1L) ? (num / num2) : (-num);
				int num3 = Microphone.access_500(this.this_0).read(array, 0, array.Length);
				if (!this.started)
				{
					lock (this)
					{
						this.started = true;
						System.Threading.Thread.MemoryBarrier();
						java.lang.Object.instancehelper_notifyAll(this);
					}
				}
				if (Microphone.access_1500(this.this_0).isLoggable(Level.FINE))
				{
					Microphone.access_1600(this.this_0).info(new StringBuilder().append("Read ").append(num3).append(" bytes from audio stream.").toString());
				}
				if (num3 <= 0)
				{
					return null;
				}
				int num4 = Microphone.access_500(this.this_0).getFormat().getSampleSizeInBits() / 8;
				long num5 = this.totalSamplesRead;
				int num6 = num3;
				int num7 = num4;
				this.totalSamplesRead = num5 + (long)((num7 != -1) ? (num6 / num7) : (-(long)num6));
				if (num3 != Microphone.access_1400(this.this_0))
				{
					bool flag = num3 != 0;
					int num8 = num4;
					if (num8 != -1 && (flag ? 1 : 0) % num8 != 0)
					{
						string text = "Incomplete sample read.";
						
						throw new Error(text);
					}
					array = Arrays.copyOf(array, num3);
				}
				if (Microphone.access_300(this.this_0))
				{
					utterance.add(array);
				}
				double[] array2;
				if (Microphone.access_1700(this.this_0))
				{
					array2 = DataUtil.bytesToValues(array, 0, array.Length, num4, Microphone.access_1800(this.this_0));
				}
				else
				{
					array2 = DataUtil.littleEndianBytesToValues(array, 0, array.Length, num4, Microphone.access_1800(this.this_0));
				}
				if (channels > 1)
				{
					array2 = Microphone.access_1900(this.this_0, array2, channels);
				}
				return new DoubleData(array2, ByteCodeHelper.f2i(Microphone.access_500(this.this_0).getFormat().getSampleRate()), firstSampleNumber);
			}
	
			public RecordingThread(Microphone microphone, string text) : base(text)
			{
				this_0 = microphone;
				this.@lock = new System.Object();
			}

			public override void start()
			{
				this.started = false;
				System.Threading.Thread.MemoryBarrier();
				base.start();
				this.waitForStart();
			}
	
			public void stopRecording()
			{
				Microphone.access_100(this.this_0).stop();
				object obj;
				System.Exception ex2;
				InterruptedException ex4;
				try
				{
					System.Threading.Monitor.Enter(obj = this.@lock);
					try
					{
						while (!this.done)
						{
							java.lang.Object.instancehelper_wait(this.@lock);
						}
						System.Threading.Monitor.Exit(obj);
					}
					catch (System.Exception ex)
					{
						ex2 = ex;
						goto IL_54;
					}
					goto IL_57;
				}
				catch (InterruptedException ex3)
				{
					ex4 = ex3;
					goto IL_59;
				}
				IL_54:
				InterruptedException ex8;
				try
				{
					System.Threading.Monitor.Exit(obj);
					throw ex2;
				}
				catch (InterruptedException ex7)
				{
					ex8 = ex7;
				}
				InterruptedException ex9 = ex8;
				goto IL_7E;
				IL_57:
				return;
				IL_59:
				ex9 = ex4;
				IL_7E:
				InterruptedException ex10 = ex9;
				Throwable.instancehelper_printStackTrace(ex10);
			}
			
			public override void run()
			{
				this.totalSamplesRead = 0L;
				Microphone.access_200(this.this_0).info("started recording");
				if (Microphone.access_300(this.this_0))
				{
					Microphone.access_402(this.this_0, new Utterance("Microphone", Microphone.access_500(this.this_0).getFormat()));
				}
				Microphone.access_700(this.this_0).add(new DataStartSignal(Microphone.access_600(this.this_0)));
				Microphone.access_800(this.this_0).info("DataStartSignal added");
				IOException ex2;
				try
				{
					Microphone.access_100(this.this_0).start();
					while (!this.done)
					{
						Data data = this.readData(Microphone.access_400(this.this_0));
						if (data == null)
						{
							this.done = true;
							break;
						}
						Microphone.access_700(this.this_0).add(data);
					}
					Microphone.access_100(this.this_0).flush();
					if (Microphone.access_900(this.this_0))
					{
						Microphone.access_500(this.this_0).close();
						Microphone.access_100(this.this_0).close();
						java.lang.System.err.println("set to null");
						Microphone.access_102(this.this_0, null);
					}
				}
				catch (IOException ex)
				{
					ex2 = ex;
					goto IL_138;
				}
				goto IL_173;
				IL_138:
				IOException ex3 = ex2;
				Microphone.access_1000(this.this_0).warning(new StringBuilder().append("IO Exception ").append(Throwable.instancehelper_getMessage(ex3)).toString());
				Throwable.instancehelper_printStackTrace(ex3);
				IL_173:
				long duration = ByteCodeHelper.d2l((double)this.totalSamplesRead / (double)Microphone.access_500(this.this_0).getFormat().getSampleRate() * 1000.0);
				Microphone.access_700(this.this_0).add(new DataEndSignal(duration));
				Microphone.access_1100(this.this_0).info("DataEndSignal ended");
				Microphone.access_1200(this.this_0).info("stopped recording");
				lock (this.@lock)
				{
					java.lang.Object.instancehelper_notify(this.@lock);
				}
			}

			private bool done;

			private volatile bool started;

			private long totalSamplesRead;

			private object @lock;
			
			internal Microphone this_0;
		}
	}
}
