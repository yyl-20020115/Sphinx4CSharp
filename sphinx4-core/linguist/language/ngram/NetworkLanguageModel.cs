using edu.cmu.sphinx.linguist.dictionary;
using edu.cmu.sphinx.linguist.util;
using edu.cmu.sphinx.util;
using edu.cmu.sphinx.util.props;
using java.io;
using java.lang;
using java.net;
using java.util;

namespace edu.cmu.sphinx.linguist.language.ngram
{
	public class NetworkLanguageModel : LanguageModelBase
	{		
		public NetworkLanguageModel(string host, int port, URL location, int maxDepth)
		{
			this.host = host;
			this.port = port;
			this.maxDepth = maxDepth;
			this.location = location;
			this.logMath = LogMath.getLogMath();
		}
		
		public NetworkLanguageModel()
		{
		}
		
		public override void newProperties(PropertySheet ps)
		{
			if (this.allocated)
			{
				string text = "Can't change properties after allocation";
				
				throw new RuntimeException(text);
			}
			this.host = ps.getString("host");
			this.port = ps.getInt("port");
			this.location = ConfigurationManagerUtils.getResource("location", ps);
			this.maxDepth = ps.getInt("maxDepth");
			if (this.maxDepth == -1)
			{
				this.maxDepth = 3;
			}
		}
		
		public override void allocate()
		{
			this.allocated = true;
			this.socket = new Socket(this.host, this.port);
			this.inReader = new BufferedReader(new InputStreamReader(this.socket.getInputStream()));
			this.outWriter = new PrintWriter(this.socket.getOutputStream(), true);
			string text = this.inReader.readLine();
			if (!java.lang.String.instancehelper_equals(text, "probserver ready"))
			{
				string text2 = "Incorrect input";
				
				throw new IOException(text2);
			}
			this.cache = new LRUCache(1000);
		}
		
		public override void deallocate()
		{
			this.allocated = false;
			try
			{
				this.socket.close();
			}
			catch (IOException ex)
			{
				Throwable.instancehelper_printStackTrace(ex);
			}
		}

		public override int getMaxDepth()
		{
			return this.maxDepth;
		}
		
		public override float getProbability(WordSequence wordSequence)
		{
			Float @float = (Float)this.cache.get(wordSequence);
			if (@float != null)
			{
				return @float.floatValue();
			}
			StringBuilder stringBuilder = new StringBuilder();
			if (wordSequence.size() == 0)
			{
				return 0f;
			}
			Word[] words = wordSequence.getWords();
			int num = words.Length;
			for (int i = 0; i < num; i++)
			{
				Word word = words[i];
				stringBuilder.append(word.toString());
				stringBuilder.append(' ');
			}
			this.outWriter.println(stringBuilder.toString());
			string text = "0";
			try
			{
				text = this.inReader.readLine();
				if (java.lang.String.instancehelper_charAt(text, 0) == '\0')
				{
					text = java.lang.String.instancehelper_substring(text, 1);
				}
			}
			catch (IOException ex)
			{
				Throwable.instancehelper_printStackTrace(ex);
			}
			if (!java.lang.String.instancehelper_equals(text, "-inf"))
			{
				@float = Float.valueOf(this.logMath.log10ToLog(Float.parseFloat(text)));
			}
			else
			{
				@float = Float.valueOf(float.MinValue);
			}
			this.cache.put(wordSequence, @float);
			return @float.floatValue();
		}

		public override float getSmear(WordSequence wordSequence)
		{
			return 0f;
		}
		
		public override Set getVocabulary()
		{
			HashSet hashSet = new HashSet();
			try
			{
				BufferedReader bufferedReader = new BufferedReader(new InputStreamReader(this.location.openStream()));
				for (;;)
				{
					string text = bufferedReader.readLine();
					if (text == null)
					{
						break;
					}
					if (java.lang.String.instancehelper_length(text) != 0)
					{
						hashSet.add(java.lang.String.instancehelper_trim(text));
					}
				}
				bufferedReader.close();
			}
			catch (IOException ex)
			{
				Throwable.instancehelper_printStackTrace(ex);
			}
			return hashSet;
		}

		public override void onUtteranceEnd()
		{
		}

		[S4String(new object[]
		{
			64,
			"Ledu/cmu/sphinx/util/props/S4String;",
			"defaultValue",
			"localhost"
		})]
		public const string PROP_HOST = "host";

		[S4Integer(new object[]
		{
			64,
			"Ledu/cmu/sphinx/util/props/S4Integer;",
			"defaultValue",
			2525
		})]
		public const string PROP_PORT = "port";

		internal LogMath logMath;

		private string host;

		private int port;

		private URL location;

		internal int maxDepth;

		internal Socket socket;

		private BufferedReader inReader;

		private PrintWriter outWriter;
		
		internal LRUCache cache;

		private bool allocated;
	}
}
