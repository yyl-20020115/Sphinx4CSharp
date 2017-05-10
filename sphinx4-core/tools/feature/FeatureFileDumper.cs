using System;

using edu.cmu.sphinx.frontend;
using edu.cmu.sphinx.frontend.util;
using edu.cmu.sphinx.util.props;
using IKVM.Attributes;
using ikvm.@internal;
using IKVM.Runtime;
using java.io;
using java.lang;
using java.net;
using java.util;
using java.util.logging;

namespace edu.cmu.sphinx.tools.feature
{
	public class FeatureFileDumper : java.lang.Object
	{

		private void getAllFeatures()
		{
			try
			{
				if (!FeatureFileDumper.assertionsDisabled && this.allFeatures == null)
				{
					
					throw new AssertionError();
				}
				Data data = this.frontEnd.getData();
				while (!(data is DataEndSignal))
				{
					if (data is DoubleData)
					{
						double[] values = ((DoubleData)data).getValues();
						if (this.featureLength < 0)
						{
							this.featureLength = values.Length;
							FeatureFileDumper.logger.info(new StringBuilder().append("Feature length: ").append(this.featureLength).toString());
						}
						float[] array = new float[values.Length];
						for (int i = 0; i < values.Length; i++)
						{
							array[i] = (float)values[i];
						}
						this.allFeatures.add(array);
					}
					else if (data is FloatData)
					{
						float[] values2 = ((FloatData)data).getValues();
						if (this.featureLength < 0)
						{
							this.featureLength = values2.Length;
							FeatureFileDumper.logger.info(new StringBuilder().append("Feature length: ").append(this.featureLength).toString());
						}
						this.allFeatures.add(values2);
					}
					data = this.frontEnd.getData();
				}
			}
			catch (java.lang.Exception ex)
			{
				Throwable.instancehelper_printStackTrace(ex);
			}
			return;
		}

		
		
		private int getNumberDataPoints()
		{
			return this.allFeatures.size() * this.featureLength;
		}
		
		public FeatureFileDumper(ConfigurationManager cm, string frontEndName)
		{
			this.featureLength = -1;
			try
			{
				this.frontEnd = (FrontEnd)cm.lookup(frontEndName);
				this.audioSource = (StreamDataSource)cm.lookup("streamDataSource");
			}
			catch (java.lang.Exception ex)
			{
				Throwable.instancehelper_printStackTrace(ex);

			}
			return;
		}

		private void processFile(string inputAudioFile, string outputFile, string text)
		{
			this.processFile(inputAudioFile);
			if (java.lang.String.instancehelper_equals(text, "binary"))
			{
				this.dumpBinary(outputFile);
			}
			else if (java.lang.String.instancehelper_equals(text, "ascii"))
			{
				this.dumpAscii(outputFile);
			}
			else
			{
				java.lang.System.@out.println(new StringBuilder().append("ERROR: unknown output format: ").append(text).toString());
			}
		}

		private void processCtl(string text, string text2, string text3, string text4)
		{
			Scanner scanner = new Scanner(new File(text));
			while (scanner.hasNext())
			{
				string text5 = scanner.next();
				string inputAudioFile = new StringBuilder().append(text2).append("/").append(text5).append(".wav").toString();
				string outputFile = new StringBuilder().append(text3).append("/").append(text5).append(".mfc").toString();
				this.processFile(inputAudioFile);
				if (java.lang.String.instancehelper_equals(text4, "binary"))
				{
					this.dumpBinary(outputFile);
				}
				else if (java.lang.String.instancehelper_equals(text4, "ascii"))
				{
					this.dumpAscii(outputFile);
				}
				else
				{
					java.lang.System.@out.println(new StringBuilder().append("ERROR: unknown output format: ").append(text4).toString());
				}
			}
			scanner.close();
		}
	
		public virtual void processFile(string inputAudioFile)
		{
			this.audioSource.setInputStream(new FileInputStream(inputAudioFile));
			this.allFeatures = new LinkedList();
			this.getAllFeatures();
			FeatureFileDumper.logger.info(new StringBuilder().append("Frames: ").append(this.allFeatures.size()).toString());
		}

		public virtual void dumpBinary(string outputFile)
		{
			DataOutputStream dataOutputStream = new DataOutputStream(new FileOutputStream(outputFile));
			dataOutputStream.writeInt(this.getNumberDataPoints());
			Iterator iterator = this.allFeatures.iterator();
			while (iterator.hasNext())
			{
				float[] array = (float[])iterator.next();
				float[] array2 = array;
				int num = array2.Length;
				for (int i = 0; i < num; i++)
				{
					float num2 = array2[i];
					dataOutputStream.writeFloat(num2);
				}
			}
			dataOutputStream.close();
		}
	
		public virtual void dumpAscii(string outputFile)
		{
			PrintStream printStream = new PrintStream(new FileOutputStream(outputFile), true);
			printStream.print(this.getNumberDataPoints());
			printStream.print(' ');
			Iterator iterator = this.allFeatures.iterator();
			while (iterator.hasNext())
			{
				float[] array = (float[])iterator.next();
				float[] array2 = array;
				int num = array2.Length;
				for (int i = 0; i < num; i++)
				{
					float num2 = array2[i];
					printStream.print(num2);
					printStream.print(' ');
				}
			}
			printStream.close();
		}

		public static void main(string[] argv)
		{
			string text = null;
			string text2 = null;
			string text3 = null;
			string text4 = null;
			string text5 = null;
			string text6 = "binary";
			for (int i = 0; i < argv.Length; i++)
			{
				if (java.lang.String.instancehelper_equals(argv[i], "-c"))
				{
					i++;
					text = argv[i];
				}
				if (java.lang.String.instancehelper_equals(argv[i], "-name"))
				{
					i++;
					text2 = argv[i];
				}
				if (java.lang.String.instancehelper_equals(argv[i], "-i"))
				{
					i++;
					text3 = argv[i];
				}
				if (java.lang.String.instancehelper_equals(argv[i], "-ctl"))
				{
					i++;
					text4 = argv[i];
				}
				if (java.lang.String.instancehelper_equals(argv[i], "-o"))
				{
					i++;
					text5 = argv[i];
				}
				if (java.lang.String.instancehelper_equals(argv[i], "-format"))
				{
					i++;
					text6 = argv[i];
				}
			}
			if (text2 == null || (text3 == null && text4 == null) || text5 == null || text6 == null)
			{
				java.lang.System.@out.println("Usage: FeatureFileDumper [ -config configFile ] -name frontendName < -i input File -o outputFile | -ctl inputFile -i inputFolder -o outputFolder >\nPossible frontends are: cepstraFrontEnd, spectraFrontEnd, plpFrontEnd");
				java.lang.System.exit(1);
			}
			FeatureFileDumper.logger.info(new StringBuilder().append("Input file: ").append(text3).toString());
			FeatureFileDumper.logger.info(new StringBuilder().append("Output file: ").append(text5).toString());
			FeatureFileDumper.logger.info(new StringBuilder().append("Format: ").append(text6).toString());
			IOException ex2;
			PropertyException ex4;
			try
			{
				try
				{
					URL url;
					if (text != null)
					{
						url = new File(text).toURI().toURL();
					}
					else
					{
						url = ClassLiteral<FeatureFileDumper>.Value.getResource("frontend.config.xml");
					}
					ConfigurationManager configurationManager = new ConfigurationManager(url);
					if (configurationManager.lookup(text2) == null)
					{
						string text7 = new StringBuilder().append("No such frontend: ").append(text2).toString();
						
						throw new RuntimeException(text7);
					}
					FeatureFileDumper featureFileDumper = new FeatureFileDumper(configurationManager, text2);
					if (text4 == null)
					{
						featureFileDumper.processFile(text3, text5, text6);
					}
					else
					{
						featureFileDumper.processCtl(text4, text3, text5, text6);
					}
				}
				catch (IOException ex)
				{
					java.lang.System.err.println(new StringBuilder().append("I/O Error ").append(ex).toString());
				}
			}
			catch (PropertyException ex3)
			{
				java.lang.System.err.println(new StringBuilder().append("Bad configuration ").append(ex3).toString());
			}
		}
		static FeatureFileDumper()
		{
			FeatureFileDumper.logger = Logger.getLogger("edu.cmu.sphinx.tools.feature.FeatureFileDumper");
		}

		private FrontEnd frontEnd;

		private StreamDataSource audioSource;

		
		private List allFeatures;

		private int featureLength;

		
		private static Logger logger;

		
		internal static bool assertionsDisabled = !ClassLiteral<FeatureFileDumper>.Value.desiredAssertionStatus();
	}
}
