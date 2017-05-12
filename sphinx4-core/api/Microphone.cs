using java.io;
using java.lang;
using javax.sound.sampled;

namespace edu.cmu.sphinx.api
{
	public class Microphone : java.lang.Object
	{
		public virtual InputStream getStream()
		{
			return this.inputStream;
		}		
		public virtual void startRecording()
		{
			this.line.start();
		}
		
		public virtual void stopRecording()
		{
			this.line.stop();
		}		
		public Microphone(float sampleRate, int sampleSize, bool signed, bool bigEndian)
		{
			AudioFormat audioFormat = new AudioFormat(sampleRate, sampleSize, 1, signed, bigEndian);
			try
			{
				this.line = AudioSystem.getTargetDataLine(audioFormat);
				this.line.open();
			}
			catch (LineUnavailableException ex)
			{

			throw new IllegalStateException(ex);
			}
			this.inputStream = new AudioInputStream(this.line);
			return;

		}

		private TargetDataLine line;

		private InputStream inputStream;
	}
}
