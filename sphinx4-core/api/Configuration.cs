using java.lang;

namespace edu.cmu.sphinx.api
{
	public class Configuration : Object
	{		
		public Configuration()
		{
			this.sampleRate = 16000;
			this.useGrammar = false;
		}

		public virtual string getAcousticModelPath()
		{
			return this.acousticModelPath;
		}

		public virtual void setAcousticModelPath(string acousticModelPath)
		{
			this.acousticModelPath = acousticModelPath;
		}

		public virtual string getDictionaryPath()
		{
			return this.dictionaryPath;
		}

		public virtual void setDictionaryPath(string dictionaryPath)
		{
			this.dictionaryPath = dictionaryPath;
		}

		public virtual string getLanguageModelPath()
		{
			return this.languageModelPath;
		}

		public virtual void setLanguageModelPath(string languageModelPath)
		{
			this.languageModelPath = languageModelPath;
		}

		public virtual string getGrammarPath()
		{
			return this.grammarPath;
		}

		public virtual void setGrammarPath(string grammarPath)
		{
			this.grammarPath = grammarPath;
		}

		public virtual string getGrammarName()
		{
			return this.grammarName;
		}

		public virtual void setGrammarName(string grammarName)
		{
			this.grammarName = grammarName;
		}

		public virtual bool getUseGrammar()
		{
			return this.useGrammar;
		}

		public virtual void setUseGrammar(bool useGrammar)
		{
			this.useGrammar = useGrammar;
		}

		public virtual int getSampleRate()
		{
			return this.sampleRate;
		}

		public virtual void setSampleRate(int sampleRate)
		{
			this.sampleRate = sampleRate;
		}

		private string acousticModelPath;

		private string dictionaryPath;

		private string languageModelPath;

		private string grammarPath;

		private string grammarName;

		private int sampleRate;

		private bool useGrammar;
	}
}
