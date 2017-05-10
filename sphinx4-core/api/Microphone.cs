using System;

using IKVM.Attributes;
using IKVM.Runtime;
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

		[LineNumberTable(new byte[]
		{
			159,
			186,
			107
		})]
		
		public virtual void startRecording()
		{
			this.line.start();
		}

		[LineNumberTable(new byte[]
		{
			159,
			190,
			107
		})]
		
		public virtual void stopRecording()
		{
			this.line.stop();
		}

		[LineNumberTable(new byte[]
		{
			159,
			135,
			165,
			104,
			172,
			108,
			191,
			0,
			2,
			98,
			141,
			113
		})]
		
		public Microphone(float sampleRate, int sampleSize, bool signed, bool bigEndian)
		{
			AudioFormat audioFormat = new AudioFormat(sampleRate, sampleSize, 1, signed, bigEndian);
			LineUnavailableException ex2;
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
