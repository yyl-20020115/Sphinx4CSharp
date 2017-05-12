using edu.cmu.sphinx.frontend;
using edu.cmu.sphinx.frontend.util;
using edu.cmu.sphinx.util.props;
using ikvm.@internal;
using java.io;
using java.lang;
using java.net;
using java.util;

namespace edu.cmu.sphinx.tools.endpoint
{
	public class Segmenter : Object
	{		
		private static void processFile(string text, string outFilePattern, ConfigurationManager configurationManager)
		{
			FrontEnd frontEnd = (FrontEnd)configurationManager.lookup("endpointer");
			AudioFileDataSource audioFileDataSource = (AudioFileDataSource)configurationManager.lookup("audioFileDataSource");
			java.lang.System.@out.println(text);
			audioFileDataSource.setAudioFile(new File(text), null);
			WavWriter wavWriter = (WavWriter)configurationManager.lookup("wavWriter");
			wavWriter.setOutFilePattern(outFilePattern);
			frontEnd.initialize();
			while (frontEnd.getData() != null)
			{
			}
		}
		
		private static void processCtl(string text, string text2, string text3, ConfigurationManager configurationManager)
		{
			Scanner scanner = new Scanner(new File(text));
			while (scanner.hasNext())
			{
				string text4 = scanner.next();
				string text5 = new StringBuilder().append(text2).append("/").append(text4).append(".wav").toString();
				string text6 = new StringBuilder().append(text3).append("/").append(text4).append(".wav").toString();
				Segmenter.processFile(text5, text6, configurationManager);
			}
			scanner.close();
		}
				
		public Segmenter()
		{
		}
		
		public static void main(string[] argv)
		{
			string text = null;
			string text2 = null;
			string text3 = null;
			string text4 = null;
			int num = 0;
			for (int i = 0; i < argv.Length; i++)
			{
				if (String.instancehelper_equals(argv[i], "-c"))
				{
					i++;
					text = argv[i];
				}
				if (String.instancehelper_equals(argv[i], "-i"))
				{
					i++;
					text2 = argv[i];
				}
				if (String.instancehelper_equals(argv[i], "-ctl"))
				{
					i++;
					text3 = argv[i];
				}
				if (String.instancehelper_equals(argv[i], "-o"))
				{
					i++;
					text4 = argv[i];
				}
				if (String.instancehelper_equals(argv[i], "-no-split"))
				{
					num = (Boolean.parseBoolean(argv[i]) ? 1 : 0);
				}
			}
			if ((text2 == null && text3 == null) || text4 == null)
			{
				java.lang.System.@out.println("Usage: java  -cp lib/batch.jar:lib/sphinx4.jar edu.cmu.sphinx.tools.endpoint.Segmenter [ -config configFile ] -name frontendName < -i input File -o outputFile | -ctl inputCtl -i inputFolder -o outputFolder >");
				java.lang.System.exit(1);
			}
			URL url;
			if (text == null)
			{
				url = ClassLiteral<Segmenter>.Value.getResource("frontend.config.xml");
			}
			else
			{
				url = new File(text).toURI().toURL();
			}
			ConfigurationManager configurationManager = new ConfigurationManager(url);
			if (num != 0)
			{
				ConfigurationManagerUtils.setProperty(configurationManager, "wavWriter", "captureUtterances", "false");
			}
			if (text3 != null)
			{
				ConfigurationManagerUtils.setProperty(configurationManager, "wavWriter", "isCompletePath", "true");
			}
			if (text3 == null)
			{
				Segmenter.processFile(text2, text4, configurationManager);
			}
			else
			{
				Segmenter.processCtl(text3, text2, text4, configurationManager);
			}
		}
	}
}
