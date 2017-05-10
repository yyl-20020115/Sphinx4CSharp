using System;

using edu.cmu.sphinx.util.props;
using IKVM.Attributes;
using ikvm.@internal;
using IKVM.Runtime;
using java.io;
using java.lang;
using java.util;
using java.util.logging;

namespace edu.cmu.sphinx.trainer
{
	[Implements(new string[]
	{
		"edu.cmu.sphinx.trainer.ControlFile"
	})]
	public class SimpleControlFile : java.lang.Object, ControlFile, Configurable
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
			90,
			102,
			172,
			130,
			106,
			143,
			134,
			108,
			123,
			100,
			130,
			110,
			142,
			138,
			112,
			155,
			210
		})]
		
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

		[Throws(new string[]
		{
			"edu.cmu.sphinx.util.props.PropertyException"
		})]
		[LineNumberTable(new byte[]
		{
			2,
			108,
			150,
			189,
			2,
			97,
			172,
			113,
			113,
			113,
			177,
			127,
			11,
			127,
			11,
			107,
			191,
			32,
			191,
			5,
			2,
			97,
			191,
			12,
			191,
			5,
			2,
			97,
			159,
			12
		})]
		
		public virtual void newProperties(PropertySheet ps)
		{
			this.logger = ps.getLogger();
			this.dictionary = (TrainerDictionary)ps.getComponent("dictionary");
			IOException ex2;
			try
			{
				this.dictionary.allocate();
			}
			catch (IOException ex)
			{
				ex2 = ByteCodeHelper.MapException<IOException>(ex, 1);
				goto IL_3C;
			}
			this.audioFile = ps.getString("audioFile");
			this.transcriptFile = ps.getString("transcriptFile");
			this.currentPartition = ps.getInt("whichBatch");
			this.numberOfPartitions = ps.getInt("totalBatches");
			this.logger.info(new StringBuilder().append("Audio control file: ").append(this.audioFile).toString());
			this.logger.info(new StringBuilder().append("Transcript file: ").append(this.transcriptFile).toString());
			this.wordSeparator = " \t\n\r\f";
			this.logger.info(new StringBuilder().append("Processing part ").append(this.currentPartition).append(" of ").append(this.numberOfPartitions).toString());
			IOException ex4;
			try
			{
				this.audioFileList = this.getLines(this.audioFile);
			}
			catch (IOException ex3)
			{
				ex4 = ByteCodeHelper.MapException<IOException>(ex3, 1);
				goto IL_151;
			}
			IOException ex6;
			try
			{
				this.transcriptFileList = this.getLines(this.transcriptFile);
			}
			catch (IOException ex5)
			{
				ex6 = ByteCodeHelper.MapException<IOException>(ex5, 1);
				goto IL_1A3;
			}
			return;
			IL_1A3:
			IOException ex7 = ex6;
			string text = new StringBuilder().append("IOE: Can't open file ").append(this.transcriptFile).toString();
			Exception ex8 = ex7;
			
			throw new Error(text, ex8);
			IL_151:
			ex7 = ex4;
			string text2 = new StringBuilder().append("IOE: Can't open file ").append(this.audioFile).toString();
			Exception ex9 = ex7;
			
			throw new Error(text2, ex9);
			IL_3C:
			ex7 = ex2;
			Exception e = ex7;
			
			throw new PropertyException(e);
		}

		[LineNumberTable(new byte[]
		{
			36,
			113,
			113
		})]
		
		public virtual void startUtteranceIterator()
		{
			this.audioFileIterator = this.audioFileList.iterator();
			this.transcriptFileIterator = this.transcriptFileList.iterator();
		}

		[LineNumberTable(new byte[]
		{
			49,
			115,
			43
		})]
		
		public virtual bool hasMoreUtterances()
		{
			return this.audioFileIterator.hasNext() && this.transcriptFileIterator.hasNext();
		}

		[LineNumberTable(new byte[]
		{
			60,
			144,
			127,
			11,
			103,
			107,
			117,
			145,
			255,
			83,
			71,
			114,
			117
		})]
		
		public virtual Utterance nextUtterance()
		{
			this.logger.fine("processing ext utterance");
			string text = new StringBuilder().append((string)this.audioFileIterator.next()).append(".mfc").toString();
			SimpleUtterance simpleUtterance = new SimpleUtterance(text);
			string text2 = java.lang.String.instancehelper_replaceFirst(java.lang.String.instancehelper_replaceFirst(text, "^.*/", ""), "\\..*$", "");
			string text3 = (string)this.transcriptFileIterator.next();
			if (!SimpleControlFile.assertionsDisabled && !java.lang.String.instancehelper_matches(text3, new StringBuilder().append(".*[ \t]\\(").append(text2).append("\\)$").toString()))
			{
				object obj = new StringBuilder().append("File name in transcript \"").append(text3).append("\" and control file \"").append(text2).append("\" have to match.").toString();
				
				throw new AssertionError(obj);
			}
			string str = java.lang.String.instancehelper_replaceFirst(text3, "[ \t]\\(.*\\)$", "");
			((Utterance)simpleUtterance).add(str, this.dictionary, false, this.wordSeparator);
			return simpleUtterance;
		}

		
		static SimpleControlFile()
		{
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
