using java.io;
using java.lang;
using javax.sound.sampled;

namespace edu.cmu.sphinx.tools.audio
{
	public class RawWriter : Object
	{
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
			for (int i = this.bytesPerSample - 2; i >= 0; i --)
			{
				this.outputStream.write(sample >> i * 8 & 255);
			}
		}

		private OutputStream outputStream;
		
		private int bytesPerSample;

		private bool signedData;
	}
}
