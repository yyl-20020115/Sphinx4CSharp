using System;

using System.Threading;
using edu.cmu.sphinx.util.props;
using IKVM.Attributes;
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
		
		public static void __<clinit>()
		{
		}

		
		
		internal static Logger access$000(Microphone microphone)
		{
			return microphone.logger;
		}

		
		
		internal static TargetDataLine access$100(Microphone microphone)
		{
			return microphone.audioLine;
		}

		
		
		internal static Logger access$200(Microphone microphone)
		{
			return microphone.logger;
		}

		
		
		internal static bool access$300(Microphone microphone)
		{
			return microphone.keepDataReference;
		}

		
		
		internal static AudioInputStream access$500(Microphone microphone)
		{
			return microphone.audioStream;
		}

		
		
		internal static Utterance access$402(Microphone microphone, Utterance result)
		{
			microphone.currentUtterance = result;
			return result;
		}

		
		
		internal static BlockingQueue access$700(Microphone microphone)
		{
			return microphone.audioList;
		}

		
		
		internal static int access$600(Microphone microphone)
		{
			return microphone.sampleRate;
		}

		
		
		internal static Logger access$800(Microphone microphone)
		{
			return microphone.logger;
		}

		
		
		internal static Utterance access$400(Microphone microphone)
		{
			return microphone.currentUtterance;
		}

		
		
		internal static bool access$900(Microphone microphone)
		{
			return microphone.closeBetweenUtterances;
		}

		
		
		internal static TargetDataLine access$102(Microphone microphone, TargetDataLine result)
		{
			microphone.audioLine = result;
			return result;
		}

		
		
		internal static Logger access$1000(Microphone microphone)
		{
			return microphone.logger;
		}

		
		
		internal static Logger access$1100(Microphone microphone)
		{
			return microphone.logger;
		}

		
		
		internal static Logger access$1200(Microphone microphone)
		{
			return microphone.logger;
		}

		
		
		internal static Logger access$1300(Microphone microphone)
		{
			return microphone.logger;
		}

		
		
		internal static int access$1400(Microphone microphone)
		{
			return microphone.frameSizeInBytes;
		}

		
		
		internal static Logger access$1500(Microphone microphone)
		{
			return microphone.logger;
		}

		
		
		internal static Logger access$1600(Microphone microphone)
		{
			return microphone.logger;
		}

		
		
		internal static bool access$1700(Microphone microphone)
		{
			return microphone.bigEndian;
		}

		
		
		internal static bool access$1800(Microphone microphone)
		{
			return microphone.signed;
		}

		
		
		
		internal static double[] access$1900(Microphone microphone, double[] array, int num)
		{
			return microphone.convertStereoToMono(array, num);
		}

		[LineNumberTable(new byte[]
		{
			162,
			50,
			127,
			2,
			114,
			117,
			105,
			104,
			104,
			43,
			168,
			232,
			59,
			235,
			71,
			114,
			142,
			38,
			204,
			191,
			11
		})]
		
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

		[LineNumberTable(new byte[]
		{
			160,
			173,
			114,
			130,
			102,
			114,
			140,
			108
		})]
		
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

		[LineNumberTable(new byte[]
		{
			160,
			191,
			104,
			231,
			78,
			159,
			11,
			241,
			71,
			103,
			99,
			147,
			242,
			70,
			255,
			4,
			71,
			2,
			97,
			191,
			11
		})]
		
		private TargetDataLine getAudioLine()
		{
			if (this.audioLine != null)
			{
				return this.audioLine;
			}
			LineUnavailableException ex2;
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
				this.audioLine.addLineListener(new Microphone$1(this));
			}
			catch (LineUnavailableException ex)
			{
				ex2 = ByteCodeHelper.MapException<LineUnavailableException>(ex, 1);
				goto IL_99;
			}
			goto IL_C9;
			IL_99:
			LineUnavailableException ex3 = ex2;
			this.logger.severe(new StringBuilder().append("microphone unavailable ").append(Throwable.instancehelper_getMessage(ex3)).toString());
			IL_C9:
			return this.audioLine;
		}

		[LineNumberTable(new byte[]
		{
			160,
			245,
			103,
			102,
			107,
			144,
			223,
			5,
			226,
			61,
			97,
			127,
			11,
			162,
			108,
			104,
			109,
			106,
			250,
			69,
			111,
			103,
			115,
			120,
			139,
			159,
			21,
			130,
			112
		})]
		
		private bool open()
		{
			TargetDataLine targetDataLine = this.getAudioLine();
			if (targetDataLine != null)
			{
				if (!targetDataLine.isOpen())
				{
					this.logger.info("open");
					LineUnavailableException ex2;
					try
					{
						targetDataLine.open(this.finalFormat, this.audioBufferSize);
					}
					catch (LineUnavailableException ex)
					{
						ex2 = ByteCodeHelper.MapException<LineUnavailableException>(ex, 1);
						goto IL_49;
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
					IL_49:
					LineUnavailableException ex3 = ex2;
					this.logger.severe(new StringBuilder().append("Can't open microphone ").append(Throwable.instancehelper_getMessage(ex3)).toString());
					return false;
				}
				return true;
			}
			this.logger.severe("Can't find microphone");
			return false;
		}

		[LineNumberTable(new byte[]
		{
			159,
			97,
			76,
			233,
			19,
			238,
			110,
			134,
			103,
			103,
			135,
			177,
			103,
			104,
			103,
			104,
			104,
			104,
			104
		})]
		
		public Microphone(int sampleRate, int bitsPerSample, int channels, bool bigEndian, bool signed, bool closeBetweenUtterances, int msecPerRead, bool keepLastAudio, string stereoToMono, int selectedChannel, string selectedMixerIndex, int audioBufferSize)
		{
			this.utteranceEndReached = true;
			Thread.MemoryBarrier();
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

		[LineNumberTable(new byte[]
		{
			160,
			85,
			232,
			0,
			238,
			160,
			66
		})]
		
		public Microphone()
		{
			this.utteranceEndReached = true;
			Thread.MemoryBarrier();
		}

		[Throws(new string[]
		{
			"edu.cmu.sphinx.util.props.PropertyException"
		})]
		[LineNumberTable(new byte[]
		{
			160,
			96,
			103,
			140,
			145,
			140,
			108,
			118,
			150,
			191,
			1,
			118,
			113,
			118,
			113,
			113,
			113,
			113
		})]
		
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

		[LineNumberTable(new byte[]
		{
			160,
			125,
			102,
			139,
			241,
			71,
			107,
			127,
			11,
			135,
			37,
			134,
			99,
			149,
			167,
			104,
			138,
			104,
			123,
			122,
			57,
			167,
			155,
			122,
			25,
			229,
			69,
			98,
			127,
			21,
			140
		})]
		
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

		[LineNumberTable(new byte[]
		{
			161,
			63,
			106,
			130,
			104,
			130,
			110,
			109,
			144,
			122,
			113,
			107,
			110
		})]
		
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
			Thread.MemoryBarrier();
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
			Thread.MemoryBarrier();
			return true;
		}

		[LineNumberTable(new byte[]
		{
			161,
			86,
			104,
			104,
			107,
			135,
			142
		})]
		
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
				Thread.MemoryBarrier();
			}
		}

		[LineNumberTable(new byte[]
		{
			162,
			77,
			107
		})]
		
		public virtual void clear()
		{
			this.audioList.clear();
		}

		[Throws(new string[]
		{
			"edu.cmu.sphinx.frontend.DataProcessingException"
		})]
		[LineNumberTable(new byte[]
		{
			162,
			92,
			130,
			138,
			191,
			4,
			2,
			97,
			145,
			104,
			174
		})]
		
		public override Data getData()
		{
			Data data = null;
			if (!this.utteranceEndReached)
			{
				InterruptedException ex2;
				try
				{
					data = (Data)this.audioList.take();
				}
				catch (InterruptedException ex)
				{
					ex2 = ByteCodeHelper.MapException<InterruptedException>(ex, 1);
					goto IL_2C;
				}
				if (data is DataEndSignal)
				{
					this.utteranceEndReached = true;
					Thread.MemoryBarrier();
					return data;
				}
				return data;
				IL_2C:
				InterruptedException ex3 = ex2;
				string message = "cannot take Data from audioList";
				Exception cause = ex3;
				
				throw new DataProcessingException(message, cause);
			}
			return data;
		}

		
		
		public virtual bool hasMoreData()
		{
			return !this.utteranceEndReached || !this.audioList.isEmpty();
		}

		
		static Microphone()
		{
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

		
		[SourceFile("Microphone.java")]
		
		internal sealed class RecordingThread : Thread
		{
			
			public static void __<clinit>()
			{
			}

			[LineNumberTable(new byte[]
			{
				161,
				220,
				106,
				209,
				2,
				97,
				149
			})]
			
			private void waitForStart()
			{
				try
				{
					while (!this.started)
					{
						Object.instancehelper_wait(this);
					}
				}
				catch (InterruptedException ex)
				{
					goto IL_18;
				}
				return;
				IL_18:
				Microphone.access$1300(this.this$0).warning("wait was interrupted");
			}

			[Throws(new string[]
			{
				"java.io.IOException"
			})]
			[LineNumberTable(new byte[]
			{
				161,
				239,
				145,
				118,
				147,
				181,
				106,
				105,
				110,
				102,
				176,
				119,
				191,
				21,
				100,
				130,
				102,
				115,
				154,
				142,
				111,
				176,
				168,
				109,
				231,
				69,
				109,
				108,
				142,
				108,
				172,
				100,
				176,
				104,
				58
			})]
			
			private Data readData(Utterance utterance)
			{
				byte[] array = new byte[Microphone.access$1400(this.this$0)];
				int channels = Microphone.access$500(this.this$0).getFormat().getChannels();
				long num = this.totalSamplesRead;
				long num2 = (long)channels;
				long firstSampleNumber = (num2 != -1L) ? (num / num2) : (-num);
				int num3 = Microphone.access$500(this.this$0).read(array, 0, array.Length);
				if (!this.started)
				{
					lock (this)
					{
						this.started = true;
						Thread.MemoryBarrier();
						Object.instancehelper_notifyAll(this);
					}
				}
				if (Microphone.access$1500(this.this$0).isLoggable(Level.FINE))
				{
					Microphone.access$1600(this.this$0).info(new StringBuilder().append("Read ").append(num3).append(" bytes from audio stream.").toString());
				}
				if (num3 <= 0)
				{
					return null;
				}
				int num4 = Microphone.access$500(this.this$0).getFormat().getSampleSizeInBits() / 8;
				long num5 = this.totalSamplesRead;
				int num6 = num3;
				int num7 = num4;
				this.totalSamplesRead = num5 + (long)((num7 != -1) ? (num6 / num7) : (-(long)num6));
				if (num3 != Microphone.access$1400(this.this$0))
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
				if (Microphone.access$300(this.this$0))
				{
					utterance.add(array);
				}
				double[] array2;
				if (Microphone.access$1700(this.this$0))
				{
					array2 = DataUtil.bytesToValues(array, 0, array.Length, num4, Microphone.access$1800(this.this$0));
				}
				else
				{
					array2 = DataUtil.littleEndianBytesToValues(array, 0, array.Length, num4, Microphone.access$1800(this.this$0));
				}
				if (channels > 1)
				{
					array2 = Microphone.access$1900(this.this$0, array2, channels);
				}
				return new DoubleData(array2, ByteCodeHelper.f2i(Microphone.access$500(this.this$0).getFormat().getSampleRate()), firstSampleNumber);
			}

			[LineNumberTable(new byte[]
			{
				161,
				112,
				103,
				233,
				55,
				235,
				74
			})]
			
			public RecordingThread(Microphone microphone, string text) : base(text)
			{
				this.@lock = new Object();
			}

			[LineNumberTable(new byte[]
			{
				161,
				122,
				110,
				102,
				102
			})]
			
			public override void start()
			{
				this.started = false;
				Thread.MemoryBarrier();
				base.start();
				this.waitForStart();
			}

			[LineNumberTable(new byte[]
			{
				161,
				133,
				144,
				109,
				104,
				141,
				191,
				43,
				2,
				98,
				231,
				69
			})]
			
			public void stopRecording()
			{
				Microphone.access$100(this.this$0).stop();
				object obj;
				Exception ex2;
				InterruptedException ex4;
				try
				{
					Monitor.Enter(obj = this.@lock);
					try
					{
						while (!this.done)
						{
							Object.instancehelper_wait(this.@lock);
						}
						Monitor.Exit(obj);
					}
					catch (Exception ex)
					{
						ex2 = ByteCodeHelper.MapException<Exception>(ex, 0);
						goto IL_54;
					}
					goto IL_57;
				}
				catch (InterruptedException ex3)
				{
					ex4 = ByteCodeHelper.MapException<InterruptedException>(ex3, 1);
					goto IL_59;
				}
				IL_54:
				Exception ex5 = ex2;
				InterruptedException ex8;
				try
				{
					Exception ex6 = ex5;
					Monitor.Exit(obj);
					throw Throwable.__<unmap>(ex6);
				}
				catch (InterruptedException ex7)
				{
					ex8 = ByteCodeHelper.MapException<InterruptedException>(ex7, 1);
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

			[LineNumberTable(new byte[]
			{
				161,
				154,
				104,
				149,
				109,
				113,
				47,
				198,
				127,
				2,
				149,
				112,
				104,
				114,
				99,
				103,
				130,
				114,
				98,
				112,
				237,
				75,
				112,
				112,
				111,
				255,
				0,
				69,
				226,
				61,
				97,
				127,
				16,
				134,
				141,
				159,
				2,
				119,
				117,
				149,
				110,
				107,
				112
			})]
			
			public override void run()
			{
				this.totalSamplesRead = 0L;
				Microphone.access$200(this.this$0).info("started recording");
				if (Microphone.access$300(this.this$0))
				{
					Microphone.access$402(this.this$0, new Utterance("Microphone", Microphone.access$500(this.this$0).getFormat()));
				}
				Microphone.access$700(this.this$0).add(new DataStartSignal(Microphone.access$600(this.this$0)));
				Microphone.access$800(this.this$0).info("DataStartSignal added");
				IOException ex2;
				try
				{
					Microphone.access$100(this.this$0).start();
					while (!this.done)
					{
						Data data = this.readData(Microphone.access$400(this.this$0));
						if (data == null)
						{
							this.done = true;
							break;
						}
						Microphone.access$700(this.this$0).add(data);
					}
					Microphone.access$100(this.this$0).flush();
					if (Microphone.access$900(this.this$0))
					{
						Microphone.access$500(this.this$0).close();
						Microphone.access$100(this.this$0).close();
						java.lang.System.err.println("set to null");
						Microphone.access$102(this.this$0, null);
					}
				}
				catch (IOException ex)
				{
					ex2 = ByteCodeHelper.MapException<IOException>(ex, 1);
					goto IL_138;
				}
				goto IL_173;
				IL_138:
				IOException ex3 = ex2;
				Microphone.access$1000(this.this$0).warning(new StringBuilder().append("IO Exception ").append(Throwable.instancehelper_getMessage(ex3)).toString());
				Throwable.instancehelper_printStackTrace(ex3);
				IL_173:
				long duration = ByteCodeHelper.d2l((double)this.totalSamplesRead / (double)Microphone.access$500(this.this$0).getFormat().getSampleRate() * 1000.0);
				Microphone.access$700(this.this$0).add(new DataEndSignal(duration));
				Microphone.access$1100(this.this$0).info("DataEndSignal ended");
				Microphone.access$1200(this.this$0).info("stopped recording");
				lock (this.@lock)
				{
					Object.instancehelper_notify(this.@lock);
				}
			}

			
			static RecordingThread()
			{
				Thread.__<clinit>();
			}

			private bool done;

			private volatile bool started;

			private long totalSamplesRead;

			
			private object @lock;

			
			internal Microphone this$0 = microphone;
		}
	}
}
