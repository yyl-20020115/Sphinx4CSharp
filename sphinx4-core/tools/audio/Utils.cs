using System;

using IKVM.Attributes;
using java.io;
using java.lang;
using javax.sound.sampled;

namespace edu.cmu.sphinx.tools.audio
{
	public class Utils : java.lang.Object
	{
		
		public static void __<clinit>()
		{
		}

		[Throws(new string[]
		{
			"java.io.IOException"
		})]
		[LineNumberTable(new byte[]
		{
			104,
			135,
			123,
			140,
			98,
			106,
			176
		})]
		
		public static short[] toSignedPCM(AudioInputStream ais)
		{
			AudioFormat format = ais.getFormat();
			int num = ais.available();
			int frameSize = format.getFrameSize();
			short[] array = new short[(frameSize != -1) ? (num / frameSize) : (-num)];
			byte[] array2 = new byte[format.getFrameSize()];
			int num2 = 0;
			while (ais.read(array2) != -1)
			{
				short[] array3 = array;
				int num3 = num2;
				num2++;
				array3[num3] = Utils.bytesToShort(format, array2);
			}
			return array;
		}

		[LineNumberTable(new byte[]
		{
			159,
			117,
			68,
			99,
			103,
			141,
			107,
			135
		})]
		public static void toBytes(short sVal, byte[] bytes, bool bigEndian)
		{
			if (bigEndian)
			{
				bytes[0] = (byte)((sbyte)(sVal >> 8));
				bytes[1] = (byte)((sbyte)(sVal & 255));
			}
			else
			{
				bytes[0] = (byte)((sbyte)(sVal & 255));
				bytes[1] = (byte)((sbyte)(sVal >> 8));
			}
		}

		[Throws(new string[]
		{
			"java.io.IOException"
		})]
		[LineNumberTable(new byte[]
		{
			160,
			105,
			103,
			240,
			70,
			104,
			103,
			120,
			40,
			168,
			102,
			102
		})]
		
		public static void writeRawFile(AudioData audio, string filename)
		{
			FileOutputStream fileOutputStream = new FileOutputStream(filename);
			AudioFormat audioFormat = new AudioFormat(8000f, 16, 1, true, false);
			RawWriter rawWriter = new RawWriter(fileOutputStream, audioFormat);
			short[] audioData = audio.getAudioData();
			short[] array = audioData;
			int num = array.Length;
			for (int i = 0; i < num; i++)
			{
				int sample = (int)array[i];
				rawWriter.writeSample(sample);
			}
			fileOutputStream.flush();
			fileOutputStream.close();
		}

		[Throws(new string[]
		{
			"java.io.IOException"
		})]
		[LineNumberTable(new byte[]
		{
			160,
			64,
			145,
			103,
			103,
			102,
			112,
			97
		})]
		
		public static AudioData readAudioFile(string filename)
		{
			AudioData result;
			try
			{
				BufferedInputStream.__<clinit>();
				BufferedInputStream bufferedInputStream = new BufferedInputStream(new FileInputStream(filename));
				AudioInputStream audioInputStream = AudioSystem.getAudioInputStream(bufferedInputStream);
				AudioData audioData = new AudioData(audioInputStream);
				bufferedInputStream.close();
				result = audioData;
			}
			catch (UnsupportedAudioFileException ex)
			{
				goto IL_31;
			}
			return result;
			IL_31:
			return null;
		}

		[Throws(new string[]
		{
			"java.io.IOException"
		})]
		[LineNumberTable(new byte[]
		{
			160,
			85,
			103,
			240,
			69,
			104,
			102
		})]
		
		public static AudioData readRawFile(string filename)
		{
			FileInputStream fileInputStream = new FileInputStream(filename);
			AudioFormat audioFormat = new AudioFormat(8000f, 16, 1, true, false);
			short[] data = RawReader.readAudioData(fileInputStream, audioFormat);
			fileInputStream.close();
			return new AudioData(data, 8000f);
		}

		[LineNumberTable(new byte[]
		{
			159,
			125,
			66,
			101,
			101,
			99,
			145
		})]
		public static short toShort(byte[] bytes, bool bigEndian)
		{
			if (bytes.Length == 1)
			{
				return (short)bytes[0];
			}
			if (bigEndian)
			{
				return (short)((int)bytes[0] << 8 | (int)(byte.MaxValue & bytes[1]));
			}
			return (short)((int)bytes[1] << 8 | (int)(byte.MaxValue & bytes[0]));
		}

		[LineNumberTable(new byte[]
		{
			159,
			121,
			66,
			101,
			106,
			99,
			144
		})]
		public static int toUnsignedShort(byte[] bytes, bool bigEndian)
		{
			if (bytes.Length == 1)
			{
				return (int)(byte.MaxValue & bytes[0]);
			}
			if (bigEndian)
			{
				return (int)bytes[0] << 8 | (int)(byte.MaxValue & bytes[1]);
			}
			return (int)bytes[1] << 8 | (int)(byte.MaxValue & bytes[0]);
		}

		[LineNumberTable(new byte[]
		{
			70,
			98,
			103,
			135,
			104,
			109,
			103,
			138,
			104,
			109,
			100,
			132,
			105,
			106,
			146,
			159,
			5
		})]
		
		public static short bytesToShort(AudioFormat format, byte[] byteArray)
		{
			int num = 0;
			AudioFormat.Encoding encoding = format.getEncoding();
			int frameSize = format.getFrameSize();
			if (encoding == AudioFormat.Encoding.PCM_SIGNED)
			{
				num = (int)Utils.toShort(byteArray, format.isBigEndian());
				if (frameSize == 1)
				{
					num = (int)((short)(num << 8));
				}
			}
			else if (encoding == AudioFormat.Encoding.PCM_UNSIGNED)
			{
				int num2 = Utils.toUnsignedShort(byteArray, format.isBigEndian());
				if (frameSize == 1)
				{
					num2 <<= 8;
				}
				num = (int)((short)(num2 - 32768));
			}
			else if (encoding == AudioFormat.Encoding.ULAW)
			{
				num = (int)Utils.ulawTable[(int)(byteArray[0] + 128)];
			}
			else
			{
				java.lang.System.@out.println(new StringBuilder().append("Unknown encoding: ").append(encoding).toString());
			}
			return (short)num;
		}

		[LineNumberTable(new byte[]
		{
			8,
			102
		})]
		
		private Utils()
		{
		}

		
		static Utils()
		{
		}

		
		private static short[] ulawTable = new short[]
		{
			32760,
			31608,
			30584,
			29560,
			28536,
			27512,
			26488,
			25464,
			24440,
			23416,
			22392,
			21368,
			20344,
			19320,
			18296,
			17272,
			16248,
			15736,
			15224,
			14712,
			14200,
			13688,
			13176,
			12664,
			12152,
			11640,
			11128,
			10616,
			10104,
			9592,
			9080,
			8568,
			8056,
			7800,
			7544,
			7288,
			7032,
			6776,
			6520,
			6264,
			6008,
			5752,
			5496,
			5240,
			4984,
			4728,
			4472,
			4216,
			3960,
			3832,
			3704,
			3576,
			3448,
			3320,
			3192,
			3064,
			2936,
			2808,
			2680,
			2552,
			2424,
			2296,
			2168,
			2040,
			1912,
			1848,
			1784,
			1720,
			1656,
			1592,
			1528,
			1464,
			1400,
			1336,
			1272,
			1208,
			1144,
			1080,
			1016,
			952,
			888,
			856,
			824,
			792,
			760,
			728,
			696,
			664,
			632,
			600,
			568,
			536,
			504,
			472,
			440,
			408,
			376,
			360,
			344,
			328,
			312,
			296,
			280,
			264,
			248,
			232,
			216,
			200,
			184,
			168,
			152,
			136,
			120,
			112,
			104,
			96,
			88,
			80,
			72,
			64,
			56,
			48,
			40,
			32,
			24,
			16,
			8,
			0,
			-32760,
			-31608,
			-30584,
			-29560,
			-28536,
			-27512,
			-26488,
			-25464,
			-24440,
			-23416,
			-22392,
			-21368,
			-20344,
			-19320,
			-18296,
			-17272,
			-16248,
			-15736,
			-15224,
			-14712,
			-14200,
			-13688,
			-13176,
			-12664,
			-12152,
			-11640,
			-11128,
			-10616,
			-10104,
			-9592,
			-9080,
			-8568,
			-8056,
			-7800,
			-7544,
			-7288,
			-7032,
			-6776,
			-6520,
			-6264,
			-6008,
			-5752,
			-5496,
			-5240,
			-4984,
			-4728,
			-4472,
			-4216,
			-3960,
			-3832,
			-3704,
			-3576,
			-3448,
			-3320,
			-3192,
			-3064,
			-2936,
			-2808,
			-2680,
			-2552,
			-2424,
			-2296,
			-2168,
			-2040,
			-1912,
			-1848,
			-1784,
			-1720,
			-1656,
			-1592,
			-1528,
			-1464,
			-1400,
			-1336,
			-1272,
			-1208,
			-1144,
			-1080,
			-1016,
			-952,
			-888,
			-856,
			-824,
			-792,
			-760,
			-728,
			-696,
			-664,
			-632,
			-600,
			-568,
			-536,
			-504,
			-472,
			-440,
			-408,
			-376,
			-360,
			-344,
			-328,
			-312,
			-296,
			-280,
			-264,
			-248,
			-232,
			-216,
			-200,
			-184,
			-168,
			-152,
			-136,
			-120,
			-112,
			-104,
			-96,
			-88,
			-80,
			-72,
			-64,
			-56,
			-48,
			-40,
			-32,
			-24,
			-16,
			-8,
			0
		};
	}
}
