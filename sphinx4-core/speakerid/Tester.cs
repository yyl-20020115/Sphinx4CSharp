using System;

using IKVM.Attributes;
using java.io;
using java.lang;
using java.util;

namespace edu.cmu.sphinx.speakerid
{
	public class Tester : java.lang.Object
	{
		
		
		public static string time(int seconds)
		{
			StringBuilder stringBuilder = new StringBuilder().append(seconds / 60000).append(":");
			int num = 60000;
			return stringBuilder.append(java.lang.Math.round((double)((num != -1) ? (seconds % num) : 0) / 1000.0)).toString();
		}

		
		[LineNumberTable(new byte[]
		{
			159,
			176,
			102,
			102,
			103,
			105,
			104,
			63,
			5,
			136,
			117,
			104,
			104,
			104,
			62,
			136,
			233,
			60,
			232,
			60,
			233,
			75
		})]
		
		public static ArrayList generateDistinctSpeakers(int vectorSize, int vectorsCount, int speakersCount)
		{
			java.util.Random random = new java.util.Random();
			ArrayList arrayList = new ArrayList();
			float[] array = new float[vectorSize];
			for (int i = 0; i < speakersCount; i++)
			{
				for (int j = 0; j < vectorSize; j++)
				{
					array[j] = (float)(i + 1) / 10f + (float)random.nextInt(5000) / 50000f;
				}
				array[0] = 3f + (float)(i + 1) / 10f;
				for (int j = 0; j < vectorsCount; j++)
				{
					float[] array2 = new float[vectorSize];
					for (int k = 0; k < vectorSize; k++)
					{
						array2[k] = array[k] + (float)random.nextInt(5000) / 50000f;
					}
					arrayList.add(array2);
				}
			}
			return arrayList;
		}

		
		[LineNumberTable(new byte[]
		{
			17,
			127,
			20,
			98,
			126,
			127,
			19,
			103,
			127,
			0,
			127,
			55,
			106,
			101
		})]
		
		public static void printIntervals(ArrayList speakers)
		{
			java.lang.System.@out.println(new StringBuilder().append("Detected ").append(speakers.size()).append(" Speakers :").toString());
			int num = 0;
			Iterator iterator = speakers.iterator();
			while (iterator.hasNext())
			{
				SpeakerCluster speakerCluster = (SpeakerCluster)iterator.next();
				PrintStream @out = java.lang.System.@out;
				StringBuilder stringBuilder = new StringBuilder().append("Speaker ");
				num++;
				@out.print(stringBuilder.append(num).append(": ").toString());
				ArrayList speakerIntervals = speakerCluster.getSpeakerIntervals();
				Iterator iterator2 = speakerIntervals.iterator();
				while (iterator2.hasNext())
				{
					Segment segment = (Segment)iterator2.next();
					java.lang.System.@out.print(new StringBuilder().append("[").append(Tester.time(segment.getStartTime())).append(" ").append(Tester.time(segment.getLength())).append("]").toString());
				}
				java.lang.System.@out.println();
			}
		}

		[Throws(new string[]
		{
			"java.io.IOException"
		})]
		
		[LineNumberTable(new byte[]
		{
			38,
			127,
			10,
			103,
			98,
			127,
			0,
			100,
			105,
			127,
			4,
			159,
			88,
			101,
			102
		})]
		
		public static void printSpeakerIntervals(ArrayList speakers, string fileName)
		{
			string text = new StringBuilder().append(java.lang.String.instancehelper_substring(fileName, 0, java.lang.String.instancehelper_indexOf(fileName, 46))).append(".seg").toString();
			FileWriter fileWriter = new FileWriter(text);
			int num = 0;
			Iterator iterator = speakers.iterator();
			while (iterator.hasNext())
			{
				SpeakerCluster speakerCluster = (SpeakerCluster)iterator.next();
				num++;
				ArrayList speakerIntervals = speakerCluster.getSpeakerIntervals();
				Iterator iterator2 = speakerIntervals.iterator();
				while (iterator2.hasNext())
				{
					Segment segment = (Segment)iterator2.next();
					fileWriter.write(new StringBuilder().append(fileName).append(" ").append(1).append(" ").append(segment.getStartTime() / 10).append(" ").append(segment.getLength() / 10).append("U U U Speaker").append(num).append("\n").toString());
				}
			}
			fileWriter.close();
		}

		[Throws(new string[]
		{
			"java.io.IOException"
		})]
		[LineNumberTable(new byte[]
		{
			97,
			103,
			108,
			102,
			103
		})]
		
		public static void testSpeakerIdentification(string inputFile)
		{
			FileInputStream stream = new FileInputStream(inputFile);
			ArrayList speakers = new SpeakerIdentification().cluster(stream);
			Tester.printIntervals(speakers);
			Tester.printSpeakerIntervals(speakers, inputFile);
		}

		
		
		public Tester()
		{
		}

		[LineNumberTable(new byte[]
		{
			63,
			105,
			112
		})]
		
		public static void testDistinctSpeakerIdentification(int vectorSize, int vectorsCount, int speakersCount)
		{
			ArrayList features = Tester.generateDistinctSpeakers(vectorSize, vectorsCount, speakersCount);
			Tester.printIntervals(new SpeakerIdentification().cluster(features));
		}

		[LineNumberTable(new byte[]
		{
			82,
			102,
			105,
			102,
			40,
			134,
			112
		})]
		
		public static void testRepeatedSpeakerIdentification(int vectorSize, int vectorsCount, int speakersCount, int repeatFactor)
		{
			ArrayList arrayList = new ArrayList();
			ArrayList arrayList2 = Tester.generateDistinctSpeakers(vectorSize, vectorsCount, speakersCount);
			for (int i = 0; i < repeatFactor; i++)
			{
				arrayList.addAll(arrayList2);
			}
			Tester.printIntervals(new SpeakerIdentification().cluster(arrayList));
		}

		[Throws(new string[]
		{
			"java.io.IOException"
		})]
		[LineNumberTable(new byte[]
		{
			104,
			98,
			103,
			111,
			8,
			166,
			102
		})]
		
		public static void main(string[] args)
		{
			string inputFile = null;
			for (int i = 0; i < args.Length; i++)
			{
				if (java.lang.String.instancehelper_equals(args[i], "-i"))
				{
					i++;
					inputFile = args[i];
				}
			}
			Tester.testSpeakerIdentification(inputFile);
		}
	}
}
