using edu.cmu.sphinx.frontend.util;
using edu.cmu.sphinx.recognizer;
using edu.cmu.sphinx.result;
using edu.cmu.sphinx.util;
using edu.cmu.sphinx.util.props;
using java.io;
using java.lang;
using java.net;
using java.util;

namespace edu.cmu.sphinx.tools.live
{
	public class LiveModeRecognizer : java.lang.Object, Configurable
	{
		private void alignResults(List list, List list2)
		{
			java.lang.System.@out.println();
			java.lang.System.@out.println("Aligning results...");
			java.lang.System.@out.println(new StringBuilder().append("   Utterances: Found: ").append(list.size()).append("   Actual: ").append(list2.size()).toString());
			string text = this.listToString(list);
			string text2 = this.listToString(list2);
			this.saveAlignedText(text, text2);
			this.getAlignTimer().start();
			this.aligner.align(text2, text);
			this.getAlignTimer().stop();
			java.lang.System.@out.println(" ...done aligning");
			java.lang.System.@out.println();
		}
		
		private void showLiveSummary()
		{
			int num = this.referenceSource.getReferences().size();
			int num2 = this.detectGapInsertionErrors();
			java.lang.System.@out.println(new StringBuilder().append("   Utterances:  Actual: ").append(num).append("  Found: ").append(this.numUtterances).toString());
			java.lang.System.@out.println(new StringBuilder().append("   Gap Insertions: ").append(num2).toString());
		}
		
		private int detectGapInsertionErrors()
		{
			util.Timer timer = TimerPool.getTimer(this, "GapInsertionDetector");
			timer.start();
			GapInsertionDetector gapInsertionDetector = new GapInsertionDetector(this.dataSource.getTranscriptFile(), this.hypothesisFile, this.showGapInsertions);
			int result = gapInsertionDetector.detect();
			timer.stop();
			return result;
		}

		private string listToString(List list)
		{
			StringBuilder stringBuilder = new StringBuilder();
			Iterator iterator = list.iterator();
			while (iterator.hasNext())
			{
				string text = (string)iterator.next();
				stringBuilder.append(text).append(' ');
			}
			return stringBuilder.toString();
		}
		
		private void saveAlignedText(string text, string text2)
		{
			try
			{
				FileWriter fileWriter = new FileWriter("align.txt");
				fileWriter.write(text);
				fileWriter.write("\n");
				fileWriter.write(text2);
				fileWriter.close();
			}
			catch (IOException ex)
			{
				Throwable.instancehelper_printStackTrace(ex);
			}
			return;
		}

		private util.Timer getAlignTimer()
		{
			return TimerPool.getTimer(this, "Align");
		}
	
		public virtual void decode()
		{
			LinkedList linkedList = new LinkedList();
			int num = 0;
			this.hypothesisTranscript = new FileWriter(this.hypothesisFile);
			this.recognizer.allocate();
			Result result;
			List list;
			while ((result = this.recognizer.recognize()) != null)
			{
				this.numUtterances++;
				string bestResultNoFiller = result.getBestResultNoFiller();
				java.lang.System.@out.println(new StringBuilder().append("\nHYP: ").append(bestResultNoFiller).toString());
				java.lang.System.@out.println(new StringBuilder().append("   Sentences: ").append(this.numUtterances).toString());
				linkedList.add(bestResultNoFiller);
				Iterator iterator = result.getTimedBestResult(false).iterator();
				while (iterator.hasNext())
				{
					WordResult wordResult = (WordResult)iterator.next();
					this.hypothesisTranscript.write(wordResult.toString());
					this.hypothesisTranscript.write(32);
				}
				this.hypothesisTranscript.write(10);
				this.hypothesisTranscript.flush();
				if (this.alignInterval > 0)
				{
					bool flag = this.numUtterances != 0;
					int num2 = this.alignInterval;
					if (num2 == -1 || (flag ? 1 : 0) % num2 == 0)
					{
						list = this.referenceSource.getReferences();
						List list2 = list.subList(num, list.size());
						this.alignResults(linkedList, list2);
						linkedList = new LinkedList();
						num = list.size();
					}
				}
			}
			this.hypothesisTranscript.close();
			List references = this.referenceSource.getReferences();
			list = references.subList(num, references.size());
			if (!linkedList.isEmpty() || !list.isEmpty())
			{
				this.alignResults(linkedList, list);
			}
			java.lang.System.@out.println("# ------------- Summary Statistics -------------");
			this.aligner.printTotalSummary();
			this.recognizer.deallocate();
			this.showLiveSummary();
			java.lang.System.@out.println();
		}
		
		public LiveModeRecognizer(Recognizer recognizer, ConcatFileDataSource dataSource, int skip, bool showGapInsertions, string hypothesisFile, int alignInterval)
		{
			this.aligner = new NISTAlign(true, true);
			this.recognizer = recognizer;
			this.dataSource = dataSource;
			this.showGapInsertions = showGapInsertions;
			this.hypothesisFile = hypothesisFile;
			this.alignInterval = alignInterval;
			this.referenceSource = dataSource;
		}
		
		public LiveModeRecognizer()
		{
			this.aligner = new NISTAlign(true, true);
		}
		
		public virtual void newProperties(PropertySheet ps)
		{
			this.recognizer = (Recognizer)ps.getComponent("recognizer");
			this.dataSource = (ConcatFileDataSource)ps.getComponent("inputSource");
			this.showGapInsertions = ps.getBoolean("showGapInsertions").booleanValue();
			this.hypothesisFile = ps.getString("hypothesisTranscript");
			this.alignInterval = ps.getInt("alignInterval");
			this.referenceSource = this.dataSource;
		}
		
		public virtual void close()
		{
			this.hypothesisTranscript.close();
		}
		
		public static void main(string[] argv)
		{
			if (argv.Length != 1)
			{
				java.lang.System.@out.println("Usage: LiveModeRecognizer config-file.xml ");
				java.lang.System.exit(1);
			}
			string text = argv[0];
			LiveModeRecognizer liveModeRecognizer;
			IOException ex2;
			PropertyException ex4;
			try
			{
				try
				{
					URL url = new File(text).toURI().toURL();
					ConfigurationManager configurationManager = new ConfigurationManager(url);
					liveModeRecognizer = (LiveModeRecognizer)configurationManager.lookup("live");
				}
				catch (IOException ex)
				{
					java.lang.System.err.println(new StringBuilder().append("I/O error during initialization: \n   ").append(ex).toString());
					return;
				}
			}
			catch (PropertyException ex3)
			{
				Throwable.instancehelper_printStackTrace(ex3);
				return;
			}
			if (liveModeRecognizer == null)
			{
				java.lang.System.err.println(new StringBuilder().append("Can't find liveModeRecognizer in ").append(text).toString());
				return;
			}
			try
			{
				liveModeRecognizer.decode();
			}
			catch (IOException ex5)
			{
				java.lang.System.err.println(new StringBuilder().append("I/O error during decoding: ").append(Throwable.instancehelper_getMessage(ex5)).toString());
			}
		}

		[S4Component(new object[]
		{
			64,
			"Ledu/cmu/sphinx/util/props/S4Component;",
			"type",
			new object[]
			{
				99,
				"Ledu/cmu/sphinx/recognizer/Recognizer, sphinx4-core-5prealpha-SNAPSHOT, Version=0/0/0/0, Culture=neutral, PublicKeyToken=null;"
			}
		})]
		public const string PROP_RECOGNIZER = "recognizer";

		[S4Component(new object[]
		{
			64,
			"Ledu/cmu/sphinx/util/props/S4Component;",
			"type",
			new object[]
			{
				99,
				"Ledu/cmu/sphinx/frontend/util/ConcatFileDataSource, sphinx4-core-5prealpha-SNAPSHOT, Version=0/0/0/0, Culture=neutral, PublicKeyToken=null;"
			}
		})]
		public const string PROP_INPUT_SOURCE = "inputSource";

		[S4Boolean(new object[]
		{
			64,
			"Ledu/cmu/sphinx/util/props/S4Boolean;",
			"defaultValue",
			false
		})]
		public const string PROP_SHOW_GAP_INSERTIONS = "showGapInsertions";

		[S4String(new object[]
		{
			64,
			"Ledu/cmu/sphinx/util/props/S4String;",
			"defaultValue",
			"hypothesis.txt"
		})]
		public const string PROP_HYPOTHESIS_TRANSCRIPT = "hypothesisTranscript";

		[S4Integer(new object[]
		{
			64,
			"Ledu/cmu/sphinx/util/props/S4Integer;",
			"defaultValue",
			-1
		})]
		public const string PROP_ALIGN_INTERVAL = "alignInterval";

		private Recognizer recognizer;

		private ConcatFileDataSource dataSource;

		private string hypothesisFile;

		private bool showGapInsertions;

		private int alignInterval;

		private int numUtterances;

		private FileWriter hypothesisTranscript;

		private ReferenceSource referenceSource;

		private NISTAlign aligner;
	}
}
