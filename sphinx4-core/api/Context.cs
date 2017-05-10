using System;

using edu.cmu.sphinx.frontend.util;
using edu.cmu.sphinx.linguist.acoustic.tiedstate;
using edu.cmu.sphinx.util;
using edu.cmu.sphinx.util.props;
using IKVM.Attributes;
using ikvm.@internal;
using java.io;
using java.lang;

namespace edu.cmu.sphinx.api
{
	public class Context : java.lang.Object
	{
		[Throws(new string[]
		{
			"java.io.IOException",
			"java.net.MalformedURLException"
		})]
		[LineNumberTable(new byte[]
		{
			159,
			187,
			108
		})]
		
		public Context(Configuration config) : this("resource:/edu/cmu/sphinx/api/default.config.xml", config)
		{
		}

		
		
		
		public virtual Configurable getInstance(Class clazz)
		{
			return this.configurationManager.lookup(clazz);
		}

		
		
		public virtual Loader getLoader()
		{
			return (Loader)this.configurationManager.lookup("acousticModelLoader");
		}

		[Throws(new string[]
		{
			"java.io.IOException",
			"java.net.MalformedURLException"
		})]
		[LineNumberTable(new byte[]
		{
			8,
			104,
			150,
			108,
			140,
			113,
			114,
			113,
			140,
			204,
			113
		})]
		
		public Context(string path, Configuration config)
		{
			this.configurationManager = new ConfigurationManager(ConfigurationManagerUtils.resourceToURL(path));
			this.setAcousticModel(config.getAcousticModelPath());
			this.setDictionary(config.getDictionaryPath());
			if (null != config.getGrammarPath() && config.getUseGrammar())
			{
				this.setGrammar(config.getGrammarPath(), config.getGrammarName());
			}
			if (null != config.getLanguageModelPath() && !config.getUseGrammar())
			{
				this.setLanguageModel(config.getLanguageModelPath());
			}
			this.setSampleRate(config.getSampleRate());
			this.configurationManager.lookup("recognizer");
		}

		[Throws(new string[]
		{
			"java.io.IOException"
		})]
		[LineNumberTable(new byte[]
		{
			38,
			108,
			118
		})]
		
		public virtual void setAcousticModel(string path)
		{
			this.setLocalProperty("acousticModelLoader->location", path);
			this.setLocalProperty("dictionary->fillerPath", Utilities.pathJoin(path, "noisedict"));
		}

		[LineNumberTable(new byte[]
		{
			48,
			108
		})]
		
		public virtual void setDictionary(string path)
		{
			this.setLocalProperty("dictionary->dictionaryPath", path);
		}

		[LineNumberTable(new byte[]
		{
			72,
			109,
			127,
			2,
			146,
			108,
			108,
			144,
			112
		})]
		
		public virtual void setGrammar(string path, string name)
		{
			if (java.lang.String.instancehelper_endsWith(name, ".grxml"))
			{
				this.setLocalProperty("grXmlGrammar->grammarLocation", new StringBuilder().append(path).append(name).toString());
				this.setLocalProperty("flatLinguist->grammar", "grXmlGrammar");
			}
			else
			{
				this.setLocalProperty("jsgfGrammar->grammarLocation", path);
				this.setLocalProperty("jsgfGrammar->grammarName", name);
				this.setLocalProperty("flatLinguist->grammar", "jsgfGrammar");
			}
			this.setLocalProperty("decoder->searchManager", "simpleSearchManager");
		}

		[LineNumberTable(new byte[]
		{
			95,
			109,
			108,
			149,
			109,
			108,
			146,
			109,
			108,
			178,
			223,
			6
		})]
		
		public virtual void setLanguageModel(string path)
		{
			if (java.lang.String.instancehelper_endsWith(path, ".lm"))
			{
				this.setLocalProperty("simpleNGramModel->location", path);
				this.setLocalProperty("lexTreeLinguist->languageModel", "simpleNGramModel");
			}
			else if (java.lang.String.instancehelper_endsWith(path, ".dmp"))
			{
				this.setLocalProperty("largeTrigramModel->location", path);
				this.setLocalProperty("lexTreeLinguist->languageModel", "largeTrigramModel");
			}
			else
			{
				if (!java.lang.String.instancehelper_endsWith(path, ".bin"))
				{
					string text = new StringBuilder().append("Unknown format extension: ").append(path).toString();
					
					throw new IllegalArgumentException(text);
				}
				this.setLocalProperty("trieNgramModel->location", path);
				this.setLocalProperty("lexTreeLinguist->languageModel", "trieNgramModel");
			}
		}

		[LineNumberTable(new byte[]
		{
			57,
			113
		})]
		
		public virtual void setSampleRate(int sampleRate)
		{
			this.setLocalProperty("dataSource->sampleRate", Integer.toString(sampleRate));
		}

		[LineNumberTable(new byte[]
		{
			160,
			77,
			114
		})]
		
		public virtual void setLocalProperty(string name, object value)
		{
			ConfigurationManagerUtils.setProperty(this.configurationManager, name, java.lang.Object.instancehelper_toString(value));
		}

		[LineNumberTable(new byte[]
		{
			116,
			119,
			112
		})]
		
		public virtual void setSpeechSource(InputStream stream, TimeFrame timeFrame)
		{
			((StreamDataSource)this.getInstance(ClassLiteral<StreamDataSource>.Value)).setInputStream(stream, timeFrame);
			this.setLocalProperty("trivialScorer->frontend", "liveFrontEnd");
		}

		[LineNumberTable(new byte[]
		{
			126,
			118,
			112
		})]
		
		public virtual void setSpeechSource(InputStream stream)
		{
			((StreamDataSource)this.getInstance(ClassLiteral<StreamDataSource>.Value)).setInputStream(stream);
			this.setLocalProperty("trivialScorer->frontend", "liveFrontEnd");
		}

		[LineNumberTable(new byte[]
		{
			160,
			91,
			114
		})]
		
		public virtual void setGlobalProperty(string name, object value)
		{
			this.configurationManager.setGlobalProperty(name, java.lang.Object.instancehelper_toString(value));
		}

		
		private ConfigurationManager configurationManager;
	}
}
