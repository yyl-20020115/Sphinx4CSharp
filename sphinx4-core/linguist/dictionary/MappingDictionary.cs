using System;

using edu.cmu.sphinx.linguist.acoustic;
using edu.cmu.sphinx.util.props;
using IKVM.Attributes;
using java.io;
using java.lang;
using java.net;
using java.util;

namespace edu.cmu.sphinx.linguist.dictionary
{
	[Implements(new string[]
	{
		"edu.cmu.sphinx.linguist.dictionary.Dictionary"
	})]
	public class MappingDictionary : TextDictionary, Dictionary, Configurable
	{
		[Throws(new string[]
		{
			"java.io.IOException"
		})]
		[LineNumberTable(new byte[]
		{
			49,
			103,
			135,
			106,
			103,
			105,
			144,
			120,
			98,
			102,
			102,
			102
		})]
		
		protected internal virtual void loadMapping(InputStream inputStream)
		{
			InputStreamReader inputStreamReader = new InputStreamReader(inputStream);
			BufferedReader bufferedReader = new BufferedReader(inputStreamReader);
			string text;
			while ((text = bufferedReader.readLine()) != null)
			{
				StringTokenizer stringTokenizer = new StringTokenizer(text);
				if (stringTokenizer.countTokens() != 2)
				{
					string text2 = "Wrong file format";
					
					throw new IOException(text2);
				}
				this.mapping.put(stringTokenizer.nextToken(), stringTokenizer.nextToken());
			}
			bufferedReader.close();
			inputStreamReader.close();
			inputStream.close();
		}

		
		[LineNumberTable(new byte[]
		{
			2,
			240,
			60,
			235,
			69,
			103
		})]
		
		public MappingDictionary(URL mappingFile, URL wordDictionaryFile, URL fillerDictionaryFile, List addendaUrlList, string wordReplacement, UnitManager unitManager) : base(wordDictionaryFile, fillerDictionaryFile, addendaUrlList, wordReplacement, unitManager)
		{
			this.mapping = new HashMap();
			this.mappingFile = mappingFile;
		}

		[LineNumberTable(new byte[]
		{
			6,
			232,
			56,
			235,
			74
		})]
		
		public MappingDictionary()
		{
			this.mapping = new HashMap();
		}

		[Throws(new string[]
		{
			"edu.cmu.sphinx.util.props.PropertyException"
		})]
		[LineNumberTable(new byte[]
		{
			12,
			135,
			113
		})]
		
		public override void newProperties(PropertySheet ps)
		{
			base.newProperties(ps);
			this.mappingFile = ConfigurationManagerUtils.getResource("mapFile", ps);
		}

		[Throws(new string[]
		{
			"java.io.IOException"
		})]
		[LineNumberTable(new byte[]
		{
			24,
			102,
			119,
			113
		})]
		
		public override void allocate()
		{
			base.allocate();
			if (!java.lang.String.instancehelper_equals(this.mappingFile.getFile(), ""))
			{
				this.loadMapping(this.mappingFile.openStream());
			}
		}

		[LineNumberTable(new byte[]
		{
			159,
			119,
			66,
			110,
			147
		})]
		
		protected internal override Unit getCIUnit(string name, bool isFiller)
		{
			if (this.mapping.containsKey(name))
			{
				name = (string)this.mapping.get(name);
			}
			return this.unitManager.getUnit(name, isFiller, Context.__EMPTY_CONTEXT);
		}

		[S4String(new object[]
		{
			64,
			"Ledu/cmu/sphinx/util/props/S4String;",
			"mandatory",
			true,
			"defaultValue",
			""
		})]
		public const string PROP_MAP_FILE = "mapFile";

		private URL mappingFile;

		
		
		private Map mapping;
	}
}
