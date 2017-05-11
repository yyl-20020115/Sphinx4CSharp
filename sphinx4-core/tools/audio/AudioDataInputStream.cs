using java.io;
using javax.sound.sampled;

namespace edu.cmu.sphinx.tools.audio
{
	public class AudioDataInputStream : InputStream
	{
		public AudioDataInputStream(AudioData audio)
		{
			this.shorts = audio.getAudioData();
			this.bytes = new byte[2 * this.shorts.Length];
			byte[] array = new byte[2];
			for (int i = 0; i < this.shorts.Length; i++)
			{
				Utils.toBytes(this.shorts[i], array, true);
				this.bytes[i * 2 + 1] = array[0];
				this.bytes[i * 2] = array[1];
			}
		}

		public override int read()
		{
			if (this.currentIndex >= this.bytes.Length)
			{
				return -1;
			}
			byte[] array = this.bytes;
			int num = this.currentIndex;
			int num2 = num;
			this.currentIndex = num + 1;
			return array[num2];
		}

		public override int read(byte[] buf)
		{
			int num = 0;
			for (int i = 0; i < buf.Length; i++)
			{
				if (this.currentIndex >= this.bytes.Length)
				{
					break;
				}
				int num2 = i;
				byte[] array = this.bytes;
				int num3 = this.currentIndex;
				int num4 = num3;
				this.currentIndex = num3 + 1;
				buf[num2] = array[num4];
				num++;
			}
			return (num != 0) ? num : -1;
		}

		public override int read(byte[] buf, int off, int len)
		{
			int num = 0;
			int num2 = 0;
			while (num2 < len && num2 + off < buf.Length)
			{
				if (this.currentIndex >= this.bytes.Length)
				{
					break;
				}
				int num3 = num2 + off;
				byte[] array = this.bytes;
				int num4 = this.currentIndex;
				int num5 = num4;
				this.currentIndex = num4 + 1;
				buf[num3] = array[num5];
				num++;
				num2++;
			}
			return (num != 0) ? num : -1;
		}

		public virtual long skip(int n)
		{
			int num = n;
			if (this.currentIndex + n > this.bytes.Length)
			{
				num = this.bytes.Length - this.currentIndex;
			}
			this.currentIndex += num;
			return (long)num;
		}
		
		public override int available()
		{
			return this.bytes.Length - this.currentIndex;
		}
		
		public override void close()
		{
			base.close();
		}

		public override void mark(int readLimit)
		{
			this.markIndex = this.currentIndex;
		}

		public override bool markSupported()
		{
			return true;
		}

		public override void reset()
		{
			this.currentIndex = this.markIndex;
		}

		internal AudioFormat format;

		internal int currentIndex;

		internal int markIndex;
		
		internal short[] shorts;

		internal byte[] bytes;
	}
}
