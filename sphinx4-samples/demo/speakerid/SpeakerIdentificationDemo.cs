using edu.cmu.sphinx.api;
using edu.cmu.sphinx.decoder.adaptation;
using edu.cmu.sphinx.speakerid;
using edu.cmu.sphinx.util;
using ikvm.@internal;
using java.lang;
using java.net;
using java.util;

namespace edu.cmu.sphinx.demo.speakerid
{
	public class SpeakerIdentificationDemo : java.lang.Object
	{
		public static string time(int milliseconds)
		{
			StringBuilder stringBuilder = new StringBuilder().append(milliseconds / 60000).append(":");
			int num = 60000;
			return stringBuilder.append(java.lang.Math.round((double)((num != -1) ? (milliseconds % num) : 0) / 1000.0)).toString();
		}

		public static void printSpeakerIntervals(ArrayList speakers, string fileName)
		{
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
					java.lang.System.@out.println(new StringBuilder().append(fileName).append("  ").append(SpeakerIdentificationDemo.time(segment.getStartTime())).append(" ").append(SpeakerIdentificationDemo.time(segment.getLength())).append(" Speaker").append(num).toString());
				}
			}
		}

		public static void speakerAdaptiveDecoding(ArrayList speakers, URL url)
		{
			Configuration configuration = new Configuration();
			configuration.setAcousticModelPath("resource:/edu/cmu/sphinx/models/en-us/en-us");
			configuration.setDictionaryPath("resource:/edu/cmu/sphinx/models/en-us/cmudict-en-us.dict");
			configuration.setLanguageModelPath("resource:/edu/cmu/sphinx/models/en-us/en-us.lm.bin");
			StreamSpeechRecognizer streamSpeechRecognizer = new StreamSpeechRecognizer(configuration);
			Iterator iterator = speakers.iterator();
			while (iterator.hasNext())
			{
				SpeakerCluster speakerCluster = (SpeakerCluster)iterator.next();
				Stats stats = streamSpeechRecognizer.createStats(1);
				ArrayList speakerIntervals = speakerCluster.getSpeakerIntervals();
				Iterator iterator2 = speakerIntervals.iterator();
				while (iterator2.hasNext())
				{
					Segment segment = (Segment)iterator2.next();
					long start = (long)segment.getStartTime();
					long end = (long)(segment.getStartTime() + segment.getLength());
					TimeFrame timeFrame = new TimeFrame(start, end);
					streamSpeechRecognizer.startRecognition(url.openStream(), timeFrame);
					SpeechResult result;
					while ((result = streamSpeechRecognizer.getResult()) != null)
					{
						stats.collect(result);
					}
					streamSpeechRecognizer.stopRecognition();
				}
				Transform transform = stats.createTransform();
				streamSpeechRecognizer.setTransform(transform);
				Iterator iterator3 = speakerIntervals.iterator();
				while (iterator3.hasNext())
				{
					Segment segment2 = (Segment)iterator3.next();
					long start2 = (long)segment2.getStartTime();
					long end2 = (long)(segment2.getStartTime() + segment2.getLength());
					TimeFrame timeFrame = new TimeFrame(start2, end2);
					streamSpeechRecognizer.startRecognition(url.openStream(), timeFrame);
					SpeechResult result;
					while ((result = streamSpeechRecognizer.getResult()) != null)
					{
						java.lang.System.@out.format("Hypothesis: %s\n", new object[]
						{
							result.getHypothesis()
						});
					}
					streamSpeechRecognizer.stopRecognition();
				}
			}
		}

		public SpeakerIdentificationDemo()
		{
		}

		public static void main(string[] args)
		{
			SpeakerIdentification speakerIdentification = new SpeakerIdentification();
			URL resource = ClassLiteral<SpeakerIdentificationDemo>.Value.getResource("test.wav");
			ArrayList speakers = speakerIdentification.cluster(resource.openStream());
			SpeakerIdentificationDemo.printSpeakerIntervals(speakers, resource.getPath());
			SpeakerIdentificationDemo.speakerAdaptiveDecoding(speakers, resource);
		}
	}
}
