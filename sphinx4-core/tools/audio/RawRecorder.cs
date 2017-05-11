using System;

using System.Threading;
using IKVM.Attributes;
using ikvm.@internal;
using IKVM.Runtime;
using java.io;
using java.lang;
using javax.sound.sampled;

namespace edu.cmu.sphinx.tools.audio
{
	public class RawRecorder : java.lang.Object
	{
		[Throws(new string[]
		{
			"javax.sound.sampled.LineUnavailableException"
		})]
		[LineNumberTable(new byte[]
		{
			159,
			180,
			232,
			49,
			235,
			81,
			103,
			231,
			73,
			177,
			104,
			103,
			245,
			69,
			145,
			104,
			255,
			6,
			69,
			113,
			119
		})]
		
		public RawRecorder(AudioFormat audioFormat)
		{
			this.@lock = new Object();
			this.inFormat = audioFormat;
			this.outFormat = audioFormat;
			DataLine.Info info = new DataLine.Info(ClassLiteral<TargetDataLine>.Value, this.inFormat);
			if (!AudioSystem.isLineSupported(info))
			{
				this.downsample = true;
				this.inFormat = new AudioFormat(44100f, 16, 1, true, false);
				info = new DataLine.Info(ClassLiteral<TargetDataLine>.Value, this.inFormat);
				if (!AudioSystem.isLineSupported(info))
				{
					string text = new StringBuilder().append("Unsupported format: ").append(audioFormat).toString();
					
					throw new LineUnavailableException(text);
				}
			}
			this.microphone = (TargetDataLine)AudioSystem.getLine(info);
			this.microphone.open(audioFormat, this.microphone.getBufferSize());
		}

		[LineNumberTable(new byte[]
		{
			29,
			109,
			104,
			140,
			108,
			107,
			111
		})]
		
		public virtual void start()
		{
			lock (this.@lock)
			{
				if (this.recorder != null)
				{
					this.recorder.stopRecording();
				}
				this.recorder = new RawRecorder.RecordThread(this);
				this.recorder.start();
			}
		}

		[LineNumberTable(new byte[]
		{
			46,
			109,
			104,
			146,
			108,
			107,
			103,
			103,
			136,
			111,
			104,
			136,
			119,
			241,
			61,
			231,
			69,
			255,
			0,
			69,
			244,
			60,
			98,
			103,
			142
		})]
		
		public virtual short[] stop()
		{
			object obj;
			Monitor.Enter(obj = this.@lock);
			short[] array3;
			IOException ex2;
			try
			{
				if (this.recorder != null)
				{
					ByteArrayOutputStream byteArrayOutputStream = this.recorder.stopRecording();
					this.microphone.close();
					this.recorder = null;
					byte[] array = byteArrayOutputStream.toByteArray();
					ByteArrayInputStream audioStream = new ByteArrayInputStream(array);
					try
					{
						short[] array2 = RawReader.readAudioData(audioStream, this.inFormat);
						if (this.downsample)
						{
							array2 = Downsampler.downsample(array2, ByteCodeHelper.f2i(this.inFormat.getSampleRate() / 1000f), ByteCodeHelper.f2i(this.outFormat.getSampleRate() / 1000f));
						}
						array3 = array2;
					}
					catch (IOException ex)
					{
						ex2 = ByteCodeHelper.MapException<IOException>(ex, 1);
						goto IL_CA;
					}
					short[] result = array3;
					Monitor.Exit(obj);
					return result;
				}
				short[] array4 = new short[0];
				Monitor.Exit(obj);
				array3 = array4;
			}
			catch
			{
				Monitor.Exit(obj);
				throw;
			}
			return array3;
			IL_CA:
			IOException ex3 = ex2;
			short[] result2;
			try
			{
				IOException ex4 = ex3;
				Throwable.instancehelper_printStackTrace(ex4);
				result2 = new short[0];
			}
			finally
			{
				Monitor.Exit(obj);
			}
			return result2;
		}

		
		internal object @lock;

		internal RawRecorder.RecordThread recorder;

		internal AudioFormat inFormat;

		
		internal AudioFormat outFormat;

		internal TargetDataLine microphone;

		internal bool downsample;

		
		.
		
		internal sealed class RecordThread : Thread
		{
			
			public static void __<clinit>()
			{
			}

			[LineNumberTable(new byte[]
			{
				72,
				175
			})]
			
			internal RecordThread(RawRecorder rawRecorder)
			{
				this.@lock = new Object();
			}

			[LineNumberTable(new byte[]
			{
				81,
				109,
				103,
				107,
				159,
				25,
				34,
				129
			})]
			
			public ByteArrayOutputStream stopRecording()
			{
				object obj;
				Exception ex2;
				try
				{
					Monitor.Enter(obj = this.@lock);
					try
					{
						this.done = true;
						java.lang.Object.instancehelper_wait(this.@lock);
						Monitor.Exit(obj);
					}
					catch (Exception ex)
					{
						ex2 = ByteCodeHelper.MapException<Exception>(ex, 0);
						goto IL_3A;
					}
					goto IL_3D;
				}
				catch (InterruptedException ex3)
				{
					goto IL_3F;
				}
				IL_3A:
				Exception ex4 = ex2;
				try
				{
					Exception ex5 = ex4;
					Monitor.Exit(obj);
					throw Throwable.__<unmap>(ex5);
				}
				catch (InterruptedException ex6)
				{
				}
				IL_3D:
				IL_3F:
				return this.@out;
			}

			[LineNumberTable(new byte[]
			{
				93,
				118,
				171,
				112,
				112,
				104,
				117,
				100,
				206,
				98,
				112,
				189,
				2,
				97,
				134,
				110,
				107,
				112
			})]
			
			public override void run()
			{
				byte[] array = new byte[this.this_0.microphone.getBufferSize()];
				this.@out = new ByteArrayOutputStream();
				IOException ex2;
				try
				{
					this.this_0.microphone.flush();
					this.this_0.microphone.start();
					while (!this.done)
					{
						int num = this.this_0.microphone.read(array, 0, array.Length);
						if (num == -1)
						{
							break;
						}
						this.@out.write(array, 0, num);
					}
					this.this_0.microphone.stop();
					this.@out.flush();
				}
				catch (IOException ex)
				{
					ex2 = ByteCodeHelper.MapException<IOException>(ex, 1);
					goto IL_9C;
				}
				goto IL_A8;
				IL_9C:
				IOException ex3 = ex2;
				Throwable.instancehelper_printStackTrace(ex3);
				IL_A8:
				lock (this.@lock)
				{
					java.lang.Object.instancehelper_notify(this.@lock);
				}
			}

			
			static RecordThread()
			{
				Thread.__<clinit>();
			}

			internal bool done;

			
			internal object @lock;

			internal ByteArrayOutputStream @out;

			
			internal RawRecorder this_0 = rawRecorder;
		}
	}
}
