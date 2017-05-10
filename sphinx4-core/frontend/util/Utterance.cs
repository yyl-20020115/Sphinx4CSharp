using System;

using IKVM.Attributes;
using java.io;
using java.lang;
using javax.sound.sampled;

namespace edu.cmu.sphinx.frontend.util
{
	public class Utterance : java.lang.Object
	{
		[LineNumberTable(new byte[]
		{
			159,
			181,
			104,
			103,
			103,
			107
		})]
		
		public Utterance(string name, AudioFormat format)
		{
			this.name = name;
			this.audioFormat = format;
			this.audioBuffer = new ByteArrayOutputStream();
		}

		[LineNumberTable(new byte[]
		{
			22,
			109,
			111,
			111
		})]
		
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

		[LineNumberTable(new byte[]
		{
			44,
			114,
			107,
			17
		})]
		
		public virtual float getAudioTime()
		{
			return (float)this.audioBuffer.size() / (this.audioFormat.getSampleRate() * (float)this.audioFormat.getSampleSizeInBits() / 8f);
		}

		[Throws(new string[]
		{
			"java.io.IOException"
		})]
		[LineNumberTable(new byte[]
		{
			59,
			103,
			103,
			103,
			110,
			105
		})]
		
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
