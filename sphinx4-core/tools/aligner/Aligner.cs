using edu.cmu.sphinx.api;
using edu.cmu.sphinx.result;
using edu.cmu.sphinx.util;
using java.io;
using java.lang;
using java.util;
using javax.sound.sampled;

namespace edu.cmu.sphinx.tools.aligner
{
	public class Aligner : Object
	{
		private static void splitStream(File file, List list)
		{
			java.lang.System.err.println(list.size());
			ArrayList arrayList = new ArrayList();
			ArrayList arrayList2 = null;
			int num = 0;
			Iterator iterator = list.iterator();
			while (iterator.hasNext())
			{
				WordResult wordResult = (WordResult)iterator.next();
				if (wordResult.isFiller())
				{
					num = (int)((long)num + wordResult.getTimeFrame().length());
					if (num > Aligner.MIN_FILLER_LENGTH)
					{
						if (arrayList2 != null)
						{
							arrayList.add(arrayList2);
						}
						arrayList2 = null;
					}
				}
				else
				{
					num = 0;
					if (arrayList2 == null)
					{
						arrayList2 = new ArrayList();
					}
					arrayList2.add(wordResult);
				}
			}
			if (null != arrayList2)
			{
				arrayList.add(arrayList2);
			}
			int num2 = 0;
			Iterator iterator2 = arrayList.iterator();
			while (iterator2.hasNext())
			{
				List list2 = (List)iterator2.next();
				long num3 = long.MaxValue;
				long num4 = long.MinValue;
				Iterator iterator3 = list2.iterator();
				while (iterator3.hasNext())
				{
					WordResult wordResult2 = (WordResult)iterator3.next();
					TimeFrame timeFrame = wordResult2.getTimeFrame();
					num3 = Math.min(num3, timeFrame.getStart());
					num4 = Math.max(num4, timeFrame.getEnd());
					java.lang.System.@out.print(wordResult2.getPronunciation().getWord());
					java.lang.System.@out.print(' ');
				}
				string[] array = String.instancehelper_split(file.getName(), "\\.wav_");
				string text = String.format("%03d0", new object[]
				{
					Integer.valueOf(num2)
				});
				string text2 = String.format("%s-%s.wav", new object[]
				{
					array[0],
					text
				});
				java.lang.System.@out.println(new StringBuilder().append("(").append(text).append(")").toString());
				num2++;
				Aligner.dumpStreamChunk(file, text2, num3 - (long)Aligner.MIN_FILLER_LENGTH, num4 - num3 + (long)Aligner.MIN_FILLER_LENGTH);
			}
		}
		
		private static void dumpStreamChunk(File file, string text, long num, long num2)
		{
			AudioFileFormat audioFileFormat = AudioSystem.getAudioFileFormat(file);
			AudioInputStream audioInputStream = AudioSystem.getAudioInputStream(file);
			AudioFormat format = audioFileFormat.getFormat();
			int num3 = Math.round((float)format.getFrameSize() * format.getFrameRate() / 1000f);
			audioInputStream.skip(num * (long)num3);
			AudioInputStream audioInputStream2 = new AudioInputStream(audioInputStream, format, num2 * (long)num3);
			AudioSystem.write(audioInputStream2, audioFileFormat.getType(), new File(text));
			audioInputStream.close();
			audioInputStream2.close();
		}
		
		public Aligner()
		{
		}
		
		public static void main(string[] args)
		{
			File file = new File(args[2]);
			SpeechAligner speechAligner = new SpeechAligner(args[0], args[1], null);
			Aligner.splitStream(file, speechAligner.align(file.toURI().toURL(), args[3]));
		}

		static Aligner()
		{
			// Note: this type is marked as 'beforefieldinit'.
		}

		private static int MIN_FILLER_LENGTH = 200;
	}
}
