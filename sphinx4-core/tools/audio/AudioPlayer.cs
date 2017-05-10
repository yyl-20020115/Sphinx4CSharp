using System;

using System.Threading;
using IKVM.Attributes;
using ikvm.@internal;
using IKVM.Runtime;
using java.lang;
using javax.sound.sampled;

namespace edu.cmu.sphinx.tools.audio
{
	public class AudioPlayer : Thread
	{
		
		public static void __<clinit>()
		{
		}

		[LineNumberTable(new byte[]
		{
			159,
			172,
			104,
			103,
			103,
			109
		})]
		
		public AudioPlayer(AudioData audio)
		{
			this.audio = audio;
			this.selectionStart = 0;
			this.selectionEnd = audio.getAudioData().Length;
		}

		[LineNumberTable(new byte[]
		{
			159,
			181,
			109,
			103,
			103,
			107,
			111
		})]
		
		public virtual void play(int selectionStart, int selectionEnd)
		{
			lock (this.audio)
			{
				this.selectionStart = selectionStart;
				this.selectionEnd = selectionEnd;
				Object.instancehelper_notify(this.audio);
			}
		}

		[LineNumberTable(new byte[]
		{
			2,
			109,
			107,
			108,
			108,
			109,
			104,
			101,
			164,
			173,
			114,
			108,
			139,
			104,
			99,
			108,
			108,
			18,
			232,
			69,
			107,
			107,
			103,
			223,
			59,
			229,
			61,
			98,
			103,
			162
		})]
		
		public override void run()
		{
			AudioData obj;
			Exception ex2;
			Exception ex5;
			for (;;)
			{
				try
				{
					Monitor.Enter(obj = this.audio);
					try
					{
						Object.instancehelper_wait(this.audio);
						AudioFormat audioFormat = this.audio.getAudioFormat();
						short[] audioData = this.audio.getAudioData();
						int num = java.lang.Math.max(0, this.selectionStart);
						int num2 = this.selectionEnd;
						if (num2 == -1)
						{
							num2 = audioData.Length;
						}
						DataLine.Info info = new DataLine.Info(ClassLiteral<SourceDataLine>.Value, audioFormat);
						this.line = (SourceDataLine)AudioSystem.getLine(info);
						this.line.open(audioFormat);
						this.line.start();
						byte[] array = new byte[2];
						int num3 = num;
						while (num3 < num2 && num3 < audioData.Length)
						{
							Utils.toBytes(audioData[num3], array, false);
							this.line.write(array, 0, array.Length);
							num3++;
						}
						this.line.drain();
						this.line.close();
						this.line = null;
						Monitor.Exit(obj);
					}
					catch (Exception ex)
					{
						ex2 = ByteCodeHelper.MapException<Exception>(ex, 0);
						break;
					}
					continue;
				}
				catch (Exception ex3)
				{
					Exception ex4 = ByteCodeHelper.MapException<Exception>(ex3, 0);
					if (ex4 == null)
					{
						throw;
					}
					ex5 = ex4;
					goto IL_10D;
				}
				break;
			}
			Exception ex6 = ex2;
			Exception ex10;
			try
			{
				Exception ex7 = ex6;
				Monitor.Exit(obj);
				throw Throwable.__<unmap>(ex7);
			}
			catch (Exception ex8)
			{
				Exception ex9 = ByteCodeHelper.MapException<Exception>(ex8, 0);
				if (ex9 == null)
				{
					throw;
				}
				ex10 = ex9;
			}
			Exception ex11 = ex10;
			goto IL_13D;
			IL_10D:
			ex11 = ex5;
			IL_13D:
			Exception ex12 = ex11;
			Throwable.instancehelper_printStackTrace(ex12);
		}

		
		static AudioPlayer()
		{
			Thread.__<clinit>();
		}

		
		private AudioData audio;

		private SourceDataLine line;

		private int selectionStart;

		private int selectionEnd;
	}
}
