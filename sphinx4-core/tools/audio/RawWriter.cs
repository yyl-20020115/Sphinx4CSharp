using System;

using IKVM.Attributes;
using java.io;
using java.lang;
using javax.sound.sampled;

namespace edu.cmu.sphinx.tools.audio
{
	public class RawWriter : java.lang.Object
	{
		[LineNumberTable(new byte[]
		{
			159,
			171,
			104,
			103,
			103,
			110,
			104,
			105,
			104,
			137,
			127,
			5,
			134
		})]
		
		public RawWriter(OutputStream outputStream, AudioFormat audioFormat)
		{
			AudioFormat.Encoding encoding = audioFormat.getEncoding();
			this.outputStream = outputStream;
			this.bytesPerSample = audioFormat.getSampleSizeInBits() / 8;
			if (encoding == AudioFormat.Encoding.PCM_SIGNED)
			{
				this.signedData = true;
			}
			else if (encoding == AudioFormat.Encoding.PCM_UNSIGNED)
			{
				this.signedData = false;
			}
			else
			{
				java.lang.System.err.println(new StringBuilder().append("Unsupported audio encoding: ").append(encoding).toString());
				java.lang.System.exit(-1);
			}
		}

		[Throws(new string[]
		{
			"java.io.IOException"
		})]
		[LineNumberTable(new byte[]
		{
			3,
			104,
			156,
			255,
			1,
			69,
			109,
			57,
			166
		})]
		
		public virtual void writeSample(int sample)
		{
			if (this.signedData)
			{
				this.outputStream.write(sample >> (this.bytesPerSample - 1) * 8);
			}
			else
			{
				this.outputStream.write(sample >> (this.bytesPerSample - 1) * 8 & 255);
			}
			for (int i = this.bytesPerSample - 2; i >= 0; i += -1)
			{
				this.outputStream.write(sample >> i * 8 & 255);
			}
		}

		
		private OutputStream outputStream;

		
		private int bytesPerSample;

		private bool signedData;
	}
}
