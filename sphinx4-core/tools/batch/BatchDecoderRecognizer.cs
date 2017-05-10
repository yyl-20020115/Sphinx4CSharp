using System;

using edu.cmu.sphinx.frontend.util;
using edu.cmu.sphinx.recognizer;
using edu.cmu.sphinx.result;
using edu.cmu.sphinx.util.props;
using IKVM.Attributes;
using java.io;
using java.lang;
using java.util;

namespace edu.cmu.sphinx.tools.batch
{
	public class BatchDecoderRecognizer : java.lang.Object
	{
	
		private void init()
		{
			this.manager = new ConfigurationManager(this.config);
			this.recognizer = (Recognizer)this.manager.lookup("recognizer");
			this.source = (StreamDataSource)this.manager.lookup("streamDataSource");
			this.recognizer.allocate();
		}
		private void processFile(string text, string text2)
		{
			FileInputStream inputStream = new FileInputStream(text2);
			this.source.setInputStream(inputStream);
			Result result = this.recognizer.recognize();
			this.writer.println(new StringBuilder().append(result.getBestFinalResultNoFiller()).append(" (").append(text).append(")").toString());
		}
		public BatchDecoderRecognizer()
		{
			this.ctlOffset = -1;
			this.ctlCount = 1000000;
		}
		
		internal virtual void parseArgs(string[] array)
		{
			for (int i = 0; i < array.Length; i++)
			{
				if (java.lang.String.instancehelper_equals(array[i], "-ctl"))
				{
					i++;
					this.ctl = array[i];
				}
				if (java.lang.String.instancehelper_equals(array[i], "-config"))
				{
					i++;
					this.config = array[i];
				}
				if (java.lang.String.instancehelper_equals(array[i], "-hmm"))
				{
					i++;
					this.hmm = array[i];
				}
				if (java.lang.String.instancehelper_equals(array[i], "-ctloffset"))
				{
					i++;
					this.ctlOffset = Integer.parseInt(array[i]);
				}
				if (java.lang.String.instancehelper_equals(array[i], "-ctlcount"))
				{
					i++;
					this.ctlCount = Integer.parseInt(array[i]);
				}
				if (java.lang.String.instancehelper_equals(array[i], "-hyp"))
				{
					i++;
					this.hyp = array[i];
				}
				if (java.lang.String.instancehelper_equals(array[i], "-feat"))
				{
					i++;
					this.featDir = array[i];
				}
			}
		}
		
		internal virtual void recognize()
		{
			this.init();
			this.writer = new PrintWriter(new File(this.hyp), "UTF-8");
			Scanner scanner = new Scanner(new File(this.ctl));
			for (int i = 0; i < this.ctlOffset; i++)
			{
				if (scanner.hasNext())
				{
					scanner.next();
				}
			}
			for (int i = 0; i < this.ctlCount; i++)
			{
				if (scanner.hasNext())
				{
					string text = scanner.next();
					string text2 = new StringBuilder().append(this.featDir).append("/").append(text).append(".wav").toString();
					this.processFile(text, text2);
				}
			}
			this.writer.close();
			scanner.close();
			this.recognizer.deallocate();
		}
		
		public static void main(string[] argv)
		{
			BatchDecoderRecognizer batchDecoderRecognizer = new BatchDecoderRecognizer();
			batchDecoderRecognizer.parseArgs(argv);
			batchDecoderRecognizer.recognize();
		}

		internal int ctlOffset;

		internal int ctlCount;

		internal string config;

		internal string hmm;

		internal string ctl;

		internal string hyp;

		internal string featDir;

		internal ConfigurationManager manager;

		internal StreamDataSource source;

		internal Recognizer recognizer;

		internal PrintWriter writer;
	}
}
