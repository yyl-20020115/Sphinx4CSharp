using edu.cmu.sphinx.frontend.util;
using edu.cmu.sphinx.linguist.acoustic.tiedstate;
using edu.cmu.sphinx.util;
using edu.cmu.sphinx.util.props;
using ikvm.@internal;
using java.io;
using java.lang;

namespace edu.cmu.sphinx.api
{
	public class Context : Object
	{
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

		public virtual void setAcousticModel(string path)
		{
			this.setLocalProperty("acousticModelLoader->location", path);
			this.setLocalProperty("dictionary->fillerPath", Utilities.pathJoin(path, "noisedict"));
		}

		public virtual void setDictionary(string path)
		{
			this.setLocalProperty("dictionary->dictionaryPath", path);
		}

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

		public virtual void setSampleRate(int sampleRate)
		{
			this.setLocalProperty("dataSource->sampleRate", Integer.toString(sampleRate));
		}

		public virtual void setLocalProperty(string name, object value)
		{
			ConfigurationManagerUtils.setProperty(this.configurationManager, name, java.lang.Object.instancehelper_toString(value));
		}

		public virtual void setSpeechSource(InputStream stream, TimeFrame timeFrame)
		{
			((StreamDataSource)this.getInstance(ClassLiteral<StreamDataSource>.Value)).setInputStream(stream, timeFrame);
			this.setLocalProperty("trivialScorer->frontend", "liveFrontEnd");
		}

		public virtual void setSpeechSource(InputStream stream)
		{
			((StreamDataSource)this.getInstance(ClassLiteral<StreamDataSource>.Value)).setInputStream(stream);
			this.setLocalProperty("trivialScorer->frontend", "liveFrontEnd");
		}

		public virtual void setGlobalProperty(string name, object value)
		{
			this.configurationManager.setGlobalProperty(name, java.lang.Object.instancehelper_toString(value));
		}

		private ConfigurationManager configurationManager;
	}
}
