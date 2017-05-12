using edu.cmu.sphinx.util.props;
using ikvm.@internal;
using java.io;
using java.lang;
using java.util;
using java.util.logging;

namespace edu.cmu.sphinx.trainer
{
	public class SimpleControlFile : ControlFileBase
	{
		private List getLines(string text)
		{
			object obj = new ArrayList();
			BufferedReader bufferedReader = new BufferedReader(new FileReader(text));
			string text2;
			while ((text2 = bufferedReader.readLine()) != null)
			{
				((ArrayList)obj).add(text2);
			}
			bufferedReader.close();
			if (this.numberOfPartitions > 1)
			{
				int num = ((ArrayList)obj).size();
				int num2 = this.numberOfPartitions;
				int num3 = (num2 != -1) ? (num / num2) : (-num);
				if (num3 < 1)
				{
					num3 = 1;
				}
				if (this.currentPartition >= this.numberOfPartitions)
				{
					this.currentPartition = this.numberOfPartitions - 1;
				}
				int num4 = this.currentPartition * num3;
				if (this.currentPartition == this.numberOfPartitions - 1)
				{
					obj = ((ArrayList)obj).subList(num4, ((ArrayList)obj).size());
				}
				else
				{
					obj = ((ArrayList)obj).subList(num4, num4 + num3);
				}
			}
			object obj2 = obj;
			List result;
			if (obj2 != null)
			{
				if ((result = (obj2 as List)) == null)
				{
					throw new IncompatibleClassChangeError();
				}
			}
			else
			{
				result = null;
			}
			return result;
		}
		
		public SimpleControlFile()
		{
		}
		
		public override void newProperties(PropertySheet ps)
		{
			this.logger = ps.getLogger();
			this.dictionary = (TrainerDictionary)ps.getComponent("dictionary");
			try
			{
				this.dictionary.allocate();
			}
			catch (IOException ex)
			{
				throw new PropertyException(ex);
			}
			this.audioFile = ps.getString("audioFile");
			this.transcriptFile = ps.getString("transcriptFile");
			this.currentPartition = ps.getInt("whichBatch");
			this.numberOfPartitions = ps.getInt("totalBatches");
			this.logger.info(new StringBuilder().append("Audio control file: ").append(this.audioFile).toString());
			this.logger.info(new StringBuilder().append("Transcript file: ").append(this.transcriptFile).toString());
			this.wordSeparator = " \t\n\r\f";
			this.logger.info(new StringBuilder().append("Processing part ").append(this.currentPartition).append(" of ").append(this.numberOfPartitions).toString());
			try
			{
				this.audioFileList = this.getLines(this.audioFile);
			}
			catch (IOException ex3)
			{
				throw new Error(new StringBuilder().append("IOE: Can't open file ").append(this.audioFile).toString(), ex3);
			}
			try
			{
				this.transcriptFileList = this.getLines(this.transcriptFile);
			}
			catch (IOException ex5)
			{
				throw new Error(new StringBuilder().append("IOE: Can't open file ").append(this.transcriptFile).toString(), ex5);
			}
		}
		
		public override void startUtteranceIterator()
		{
			this.audioFileIterator = this.audioFileList.iterator();
			this.transcriptFileIterator = this.transcriptFileList.iterator();
		}
		
		public override bool hasMoreUtterances()
		{
			return this.audioFileIterator.hasNext() && this.transcriptFileIterator.hasNext();
		}
		
		public override Utterance nextUtterance()
		{
			this.logger.fine("processing ext utterance");
			string text = new StringBuilder().append((string)this.audioFileIterator.next()).append(".mfc").toString();
			SimpleUtterance simpleUtterance = new SimpleUtterance(text);
			string text2 = java.lang.String.instancehelper_replaceFirst(java.lang.String.instancehelper_replaceFirst(text, "^.*/", ""), "\\..*_", "");
			string text3 = (string)this.transcriptFileIterator.next();
			if (!SimpleControlFile.assertionsDisabled && !java.lang.String.instancehelper_matches(text3, new StringBuilder().append(".*[ \t]\\(").append(text2).append("\\)_").toString()))
			{
				object obj = new StringBuilder().append("File name in transcript \"").append(text3).append("\" and control file \"").append(text2).append("\" have to match.").toString();
				
				throw new AssertionError(obj);
			}
			string str = java.lang.String.instancehelper_replaceFirst(text3, "[ \t]\\(.*\\)_", "");
			((Utterance)simpleUtterance).add(str, this.dictionary, false, this.wordSeparator);
			return simpleUtterance;
		}

		[S4Component(new object[]
		{
			64,
			"Ledu/cmu/sphinx/util/props/S4Component;",
			"type",
			new object[]
			{
				99,
				"Ledu/cmu/sphinx/trainer/TrainerDictionary, sphinx4-core-5prealpha-SNAPSHOT, Version=0/0/0/0, Culture=neutral, PublicKeyToken=null;"
			}
		})]
		public const string DICTIONARY = "dictionary";

		private TrainerDictionary dictionary;

		private string audioFile;

		private string transcriptFile;

		private string wordSeparator;

		private int currentPartition;

		private int numberOfPartitions;
		
		private Iterator audioFileIterator;
		
		private Iterator transcriptFileIterator;
		
		private List audioFileList;
		
		private List transcriptFileList;

		private Logger logger;
		
		internal static bool assertionsDisabled = !ClassLiteral<SimpleControlFile>.Value.desiredAssertionStatus();
	}
}
