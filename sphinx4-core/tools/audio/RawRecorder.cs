using ikvm.@internal;
using IKVM.Runtime;
using java.io;
using java.lang;
using javax.sound.sampled;

namespace edu.cmu.sphinx.tools.audio
{
	public class RawRecorder : Object
	{		
		public RawRecorder(AudioFormat audioFormat)
		{
			this.@lock = new object();
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
		
		public virtual short[] stop()
		{
			object obj;
			System.Threading.Monitor.Enter(obj = this.@lock);
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
						ex2 = ex;
						goto IL_CA;
					}
					short[] result = array3;
					System.Threading.Monitor.Exit(obj);
					return result;
				}
				short[] array4 = new short[0];
				System.Threading.Monitor.Exit(obj);
				array3 = array4;
			}
			catch
			{
				System.Threading.Monitor.Exit(obj);
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
				System.Threading.Monitor.Exit(obj);
			}
			return result2;
		}
		
		internal object @lock;

		internal RawRecorder.RecordThread recorder;

		internal AudioFormat inFormat;

		internal AudioFormat outFormat;

		internal TargetDataLine microphone;

		internal bool downsample;
		
		internal sealed class RecordThread : Thread
		{			
			internal RecordThread(RawRecorder rawRecorder)
			{
				this_0 = rawRecorder;
				this.@lock = new object();
			}
			
			public ByteArrayOutputStream stopRecording()
			{
				object obj;
				System.Exception ex2;
				try
				{
					System.Threading.Monitor.Enter(obj = this.@lock);
					try
					{
						this.done = true;
						Object.instancehelper_wait(this.@lock);
						System.Threading.Monitor.Exit(obj);
					}
					catch (System.Exception ex)
					{
						ex2 = ex;
						goto IL_3A;
					}
					goto IL_3D;
				}
				catch (InterruptedException)
				{
					goto IL_3F;
				}
				IL_3A:
				System.Exception ex4 = ex2;
				try
				{
					System.Threading.Monitor.Exit(obj);
					throw ex4;
				}
				catch (InterruptedException)
				{
				}
				IL_3D:
				IL_3F:
				return this.@out;
			}
			
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
					ex2 =ex;
					goto IL_9C;
				}
				goto IL_A8;
				IL_9C:
				IOException ex3 = ex2;
				Throwable.instancehelper_printStackTrace(ex3);
				IL_A8:
				lock (this.@lock)
				{
					Object.instancehelper_notify(this.@lock);
				}
			}

			internal bool done;

			internal object @lock;

			internal ByteArrayOutputStream @out;
			
			internal RawRecorder this_0;
		}
	}
}
