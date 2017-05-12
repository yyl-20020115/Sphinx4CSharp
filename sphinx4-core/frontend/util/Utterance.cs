using java.io;
using javax.sound.sampled;
using java.lang;

namespace edu.cmu.sphinx.frontend.util
{
	public class Utterance : Object
	{		
		public Utterance(string name, AudioFormat format)
		{
			this.name = name;
			this.audioFormat = format;
			this.audioBuffer = new ByteArrayOutputStream();
		}
		
		public virtual void add(byte[] audio)
		{
			lock (this.audioBuffer)
			{
				this.audioBuffer.write(audio, 0, audio.Length);
			}
		}		
		
		public virtual byte[] getAudio()
		{
			return this.audioBuffer.toByteArray();
		}

		public virtual AudioFormat getAudioFormat()
		{
			return this.audioFormat;
		}

		public virtual string getName()
		{
			return this.name;
		}
		
		public virtual float getAudioTime()
		{
			return (float)this.audioBuffer.size() / (this.audioFormat.getSampleRate() * (float)this.audioFormat.getSampleSizeInBits() / 8f);
		}
		
		public virtual void save(string fileName, AudioFileFormat.Type fileFormat)
		{
			File file = new File(fileName);
			byte[] audio = this.getAudio();
			AudioInputStream audioInputStream = new AudioInputStream(new ByteArrayInputStream(audio), this.getAudioFormat(), (long)audio.Length);
			AudioSystem.write(audioInputStream, fileFormat, file);
		}
		
		private string name;
		
		private ByteArrayOutputStream audioBuffer;
		
		private AudioFormat audioFormat;
	}
}
