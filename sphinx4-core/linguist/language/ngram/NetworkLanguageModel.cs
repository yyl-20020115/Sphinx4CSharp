using System;

using edu.cmu.sphinx.linguist.dictionary;
using edu.cmu.sphinx.linguist.util;
using edu.cmu.sphinx.util;
using edu.cmu.sphinx.util.props;
using IKVM.Attributes;
using IKVM.Runtime;
using java.io;
using java.lang;
using java.net;
using java.util;

namespace edu.cmu.sphinx.linguist.language.ngram
{
	[Implements(new string[]
	{
		"edu.cmu.sphinx.linguist.language.ngram.LanguageModel"
	})]
	public class NetworkLanguageModel : java.lang.Object, LanguageModel, Configurable
	{
		[LineNumberTable(new byte[]
		{
			4,
			104,
			103,
			103,
			104,
			103,
			107
		})]
		
		public NetworkLanguageModel(string host, int port, URL location, int maxDepth)
		{
			this.host = host;
			this.port = port;
			this.maxDepth = maxDepth;
			this.location = location;
			this.logMath = LogMath.getLogMath();
		}

		[LineNumberTable(new byte[]
		{
			12,
			102
		})]
		
		public NetworkLanguageModel()
		{
		}

		[Throws(new string[]
		{
			"edu.cmu.sphinx.util.props.PropertyException"
		})]
		[LineNumberTable(new byte[]
		{
			23,
			104,
			144,
			113,
			113,
			145,
			113,
			105,
			103
		})]
		
		public virtual void newProperties(PropertySheet ps)
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

		[Throws(new string[]
		{
			"java.io.IOException"
		})]
		[LineNumberTable(new byte[]
		{
			36,
			135,
			119,
			103,
			116,
			119,
			108,
			109,
			144,
			112
		})]
		
		public virtual void allocate()
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

		[LineNumberTable(new byte[]
		{
			50,
			135,
			189,
			2,
			97,
			134
		})]
		
		public virtual void deallocate()
		{
			this.allocated = false;
			IOException ex2;
			try
			{
				this.socket.close();
			}
			catch (IOException ex)
			{
				ex2 = ByteCodeHelper.MapException<IOException>(ex, 1);
				goto IL_21;
			}
			return;
			IL_21:
			IOException ex3 = ex2;
			Throwable.instancehelper_printStackTrace(ex3);
		}

		public virtual int getMaxDepth()
		{
			return this.maxDepth;
		}

		[LineNumberTable(new byte[]
		{
			64,
			146,
			99,
			167,
			102,
			104,
			102,
			120,
			110,
			9,
			200,
			113,
			135,
			109,
			106,
			190,
			2,
			98,
			167,
			110,
			154,
			139,
			110
		})]
		
		public virtual float getProbability(WordSequence wordSequence)
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
			IOException ex2;
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
				ex2 = ByteCodeHelper.MapException<IOException>(ex, 1);
				goto IL_B0;
			}
			goto IL_BF;
			IL_B0:
			IOException ex3 = ex2;
			Throwable.instancehelper_printStackTrace(ex3);
			IL_BF:
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

		public virtual float getSmear(WordSequence wordSequence)
		{
			return 0f;
		}

		
		[LineNumberTable(new byte[]
		{
			101,
			134,
			102,
			176,
			103,
			99,
			98,
			104,
			98,
			143,
			184,
			2,
			98,
			135
		})]
		
		public virtual Set getVocabulary()
		{
			HashSet hashSet = new HashSet();
			IOException ex2;
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
				ex2 = ByteCodeHelper.MapException<IOException>(ex, 1);
				goto IL_56;
			}
			return hashSet;
			IL_56:
			IOException ex3 = ex2;
			Throwable.instancehelper_printStackTrace(ex3);
			return hashSet;
		}

		public virtual void onUtteranceEnd()
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
