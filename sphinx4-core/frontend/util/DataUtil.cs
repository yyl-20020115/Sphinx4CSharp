using edu.cmu.sphinx.util;
using ikvm.@internal;
using IKVM.Runtime;
using java.io;
using java.lang;
using java.text;
using javax.sound.sampled;

namespace edu.cmu.sphinx.frontend.util
{
	public class DataUtil : Object
	{	
		public static FloatData DoubleData2FloatData(DoubleData data)
		{
			int num = data.getValues().Length;
			float[] array = new float[num];
			double[] values = data.getValues();
			for (int i = 0; i < values.Length; i++)
			{
				array[i] = (float)values[i];
			}
			return new FloatData(array, data.getSampleRate(), data.getFirstSampleNumber());
		}
	
		public static double[] bytesToValues(byte[] byteArray, int offset, int length, int bytesPerValue, bool signedData)
		{
			if (0 >= length || offset + length > byteArray.Length)
			{
				string text = new StringBuilder().append("offset: ").append(offset).append(", length: ").append(length).append(", array length: ").append(byteArray.Length).toString();
				
				throw new ArrayIndexOutOfBoundsException(text);
			}
			if (!DataUtil.assertionsDisabled && (bytesPerValue != -1 && length % bytesPerValue != 0))
			{
				
				throw new AssertionError();
			}
			double[] array = new double[(bytesPerValue != -1) ? (length / bytesPerValue) : (-length)];
			int num = offset;
			for (int i = 0; i < array.Length; i++)
			{
				int num2 = num;
				num++;
				int num3 = (int)byteArray[num2];
				if (!signedData)
				{
					num3 &= 255;
				}
				for (int j = 1; j < bytesPerValue; j++)
				{
					int num4 = num;
					num++;
					int num5 = (int)byteArray[num4];
					num3 = (num3 << 8) + num5;
				}
				array[i] = (double)num3;
			}
			return array;
		}
	
		public static double[] littleEndianBytesToValues(byte[] data, int offset, int length, int bytesPerValue, bool signedData)
		{
			if (0 >= length || offset + length > data.Length)
			{
				string text = new StringBuilder().append("offset: ").append(offset).append(", length: ").append(length).append(", array length: ").append(data.Length).toString();
				
				throw new ArrayIndexOutOfBoundsException(text);
			}
			if (!DataUtil.assertionsDisabled && (bytesPerValue != -1 && length % bytesPerValue != 0))
			{
				
				throw new AssertionError();
			}
			double[] array = new double[(bytesPerValue != -1) ? (length / bytesPerValue) : (-length)];
			int num = offset + bytesPerValue - 1;
			for (int i = 0; i < array.Length; i++)
			{
				int num2 = num;
				num --;
				int num3 = (int)data[num2];
				if (!signedData)
				{
					num3 &= 255;
				}
				for (int j = 1; j < bytesPerValue; j++)
				{
					int num4 = num;
					num --;
					int num5 = (int)data[num4];
					num3 = (num3 << 8) + num5;
				}
				num += bytesPerValue * 2;
				array[i] = (double)num3;
			}
			return array;
		}
		
		private static string doubleArrayToString(double[] array, int num)
		{
			StringBuilder stringBuilder = new StringBuilder().append(array.Length);
			int num2 = array.Length;
			for (int i = 0; i < num2; i++)
			{
				double num3 = array[i];
				if (num == 3)
				{
					stringBuilder.append(' ').append(DataUtil.formatDouble(num3, 10, 5));
				}
				else if (num == 1)
				{
					DoubleConverter doubleConverter = new DoubleConverter();
					long num4 = DoubleConverter.ToLong(num3, ref doubleConverter);
					stringBuilder.append(" 0x").append(Long.toHexString(num4));
				}
				else if (num == 2)
				{
					stringBuilder.append(' ').append(Utilities.doubleToScientificString(num3, 8));
				}
			}
			return stringBuilder.toString();
		}
		
		public static string formatDouble(double number, int integerDigits, int fractionDigits)
		{
			StringBuilder stringBuilder = new StringBuilder(2 + fractionDigits).append("0.");
			for (int i = 0; i < fractionDigits; i++)
			{
				stringBuilder.append('0');
			}
			DataUtil.format.applyPattern(stringBuilder.toString());
			string text = DataUtil.format.format(number);
			int num = String.instancehelper_indexOf(text, 46);
			if (num == -1)
			{
				text = new StringBuilder().append(text).append(".").toString();
				num = String.instancehelper_length(text) - 1;
			}
			StringBuilder stringBuilder2 = new StringBuilder();
			for (int j = num; j < integerDigits; j++)
			{
				stringBuilder2.append(' ');
			}
			stringBuilder2.append(text);
			return stringBuilder2.toString();
		}
		
		private static string floatArrayToString(float[] array, int num)
		{
			StringBuilder stringBuilder = new StringBuilder().append(array.Length);
			int num2 = array.Length;
			for (int i = 0; i < num2; i++)
			{
				float num3 = array[i];
				if (num == 3)
				{
					stringBuilder.append(' ').append(DataUtil.formatDouble((double)num3, 10, 5));
				}
				else if (num == 1)
				{
					FloatConverter floatConverter = new FloatConverter();
					int num4 = FloatConverter.ToInt(num3, ref floatConverter);
					stringBuilder.append(" 0x").append(Integer.toHexString(num4));
				}
				else if (num == 2)
				{
					stringBuilder.append(' ').append(Utilities.doubleToScientificString((double)num3, 8));
				}
			}
			return stringBuilder.toString();
		}
		
		public static AudioFormat getNativeAudioFormat(AudioFormat format, Mixer mixer)
		{
			Line.Info[] targetLineInfo;
			if (mixer != null)
			{
				targetLineInfo = mixer.getTargetLineInfo(new Line.Info(ClassLiteral<TargetDataLine>.Value));
			}
			else
			{
				targetLineInfo = AudioSystem.getTargetLineInfo(new Line.Info(ClassLiteral<TargetDataLine>.Value));
			}
			AudioFormat audioFormat = null;
			Line.Info[] array = targetLineInfo;
			int num = array.Length;
			for (int i = 0; i < num; i++)
			{
				Line.Info info = array[i];
				AudioFormat[] formats = ((DataLine.Info)info).getFormats();
				AudioFormat[] array2 = formats;
				int num2 = array2.Length;
				for (int j = 0; j < num2; j++)
				{
					AudioFormat audioFormat2 = array2[j];
					if (audioFormat2.getEncoding() == format.getEncoding() && audioFormat2.isBigEndian() == format.isBigEndian() && audioFormat2.getSampleSizeInBits() == format.getSampleSizeInBits() && audioFormat2.getSampleRate() >= format.getSampleRate())
					{
						audioFormat = audioFormat2;
						break;
					}
				}
				if (audioFormat != null)
				{
					break;
				}
			}
			return audioFormat;
		}
		
		private DataUtil()
		{
		}

		public static short[] byteToShortArray(byte[] byteArray, int offset, int length)
		{
			if (0 < length && offset + length <= byteArray.Length)
			{
				int num = length / 2;
				short[] array = new short[num];
				int num2 = offset;
				for (int i = 0; i < num; i++)
				{
					int num3 = num2;
					num2++;
					int num4 = (int)byteArray[num3] << 8;
					int num5 = num4;
					int num6 = 255;
					int num7 = num2;
					num2++;
					num4 = (num5 | (num6 & (int)byteArray[num7]));
					array[i] = (short)num4;
				}
				return array;
			}
			string text = new StringBuilder().append("offset: ").append(offset).append(", length: ").append(length).append(", array length: ").append(byteArray.Length).toString();
			
			throw new ArrayIndexOutOfBoundsException(text);
		}

		public static short bytesToShort(byte[] byteArray, int offset)
		{
			int num = offset;
			offset++;
			return (short)((int)byteArray[num] << 8 | (int)(byte.MaxValue & byteArray[offset]));
		}
		
		public static string shortArrayToString(short[] data)
		{
			StringBuilder stringBuilder = new StringBuilder().append(data.Length);
			int num = data.Length;
			for (int i = 0; i < num; i++)
			{
				int num2 = (int)data[i];
				stringBuilder.append(' ').append(num2);
			}
			return stringBuilder.toString();
		}
		
		public static string doubleArrayToString(double[] data)
		{
			return DataUtil.doubleArrayToString(data, DataUtil.dumpFormat);
		}
		
		public static string floatArrayToString(float[] data)
		{
			return DataUtil.floatArrayToString(data, DataUtil.dumpFormat);
		}

		public static int getSamplesPerWindow(int sampleRate, float windowSizeInMs)
		{
			return ByteCodeHelper.f2i((float)sampleRate * windowSizeInMs / 1000f);
		}

		public static int getSamplesPerShift(int sampleRate, float windowShiftInMs)
		{
			return ByteCodeHelper.f2i((float)sampleRate * windowShiftInMs / 1000f);
		}
		
		public static void bytesToFile(byte[] data, string filename)
		{
			FileOutputStream fileOutputStream = new FileOutputStream(filename);
			fileOutputStream.write(data);
			fileOutputStream.close();
		}
		
		public static AudioFormat getNativeAudioFormat(AudioFormat format)
		{
			return DataUtil.getNativeAudioFormat(format, null);
		}
		
		public static DoubleData FloatData2DoubleData(FloatData data)
		{
			int num = data.getValues().Length;
			double[] array = new double[num];
			float[] values = data.getValues();
			for (int i = 0; i < values.Length; i++)
			{
				array[i] = (double)values[i];
			}
			return new DoubleData(array, data.getSampleRate(), data.getFirstSampleNumber());
		}

		static DataUtil()
		{
			DataUtil.format = new DecimalFormat();
			DataUtil.dumpFormat = 2;
			string property = java.lang.System.getProperty("frontend.util.dumpformat", "SCIENTIFIC");
			if (String.instancehelper_compareToIgnoreCase(property, "DECIMAL") == 0)
			{
				DataUtil.dumpFormat = 3;
			}
			else if (String.instancehelper_compareToIgnoreCase(property, "HEXADECIMAL") == 0)
			{
				DataUtil.dumpFormat = 1;
			}
			else if (String.instancehelper_compareToIgnoreCase(property, "SCIENTIFIC") == 0)
			{
				DataUtil.dumpFormat = 2;
			}
		}

		private const int HEXADECIMAL = 1;

		private const int SCIENTIFIC = 2;

		private const int DECIMAL = 3;
		
		private static DecimalFormat format;

		private const int decimalIntegerDigits = 10;

		private const int decimalFractionDigits = 5;

		private const int floatScientificFractionDigits = 8;

		private const int doubleScientificFractionDigits = 8;

		private static int dumpFormat;
	
		internal static bool assertionsDisabled = !ClassLiteral<DataUtil>.Value.desiredAssertionStatus();
	}
}
