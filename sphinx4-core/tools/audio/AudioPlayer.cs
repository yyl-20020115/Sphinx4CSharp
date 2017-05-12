using ikvm.@internal;
using java.lang;
using javax.sound.sampled;

namespace edu.cmu.sphinx.tools.audio
{
	public class AudioPlayer : Thread
	{		
		public AudioPlayer(AudioData audio)
		{
			this.audio = audio;
			this.selectionStart = 0;
			this.selectionEnd = audio.getAudioData().Length;
		}
		
		public virtual void play(int selectionStart, int selectionEnd)
		{
			lock (this.audio)
			{
				this.selectionStart = selectionStart;
				this.selectionEnd = selectionEnd;
				Object.instancehelper_notify(this.audio);
			}
		}
		
		public override void run()
		{
			AudioData obj = this.audio;
			for (;;)
			{
				try
				{
					System.Threading.Monitor.Enter(obj);
					try
					{
						Object.instancehelper_wait(this.audio);
						AudioFormat audioFormat = this.audio.getAudioFormat();
						short[] audioData = this.audio.getAudioData();
						int num = Math.max(0, this.selectionStart);
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
						System.Threading.Monitor.Exit(obj);
					}
					catch (System.Exception ex)
					{
						Throwable.instancehelper_printStackTrace(ex);

						break;
					}
					continue;
				}
				catch (System.Exception ex3)
				{
					Throwable.instancehelper_printStackTrace(ex3);
				}
				break;
			}
			try
			{
				System.Threading.Monitor.Exit(obj);
			}
			catch (System.Exception ex8)
			{
				Throwable.instancehelper_printStackTrace(ex8);
			}
		}

		private AudioData audio;

		private SourceDataLine line;

		private int selectionStart;

		private int selectionEnd;
	}
}
