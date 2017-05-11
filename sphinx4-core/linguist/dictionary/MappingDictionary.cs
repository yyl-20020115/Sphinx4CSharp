using edu.cmu.sphinx.linguist.acoustic;
using edu.cmu.sphinx.util.props;
using java.io;
using java.net;
using java.util;

namespace edu.cmu.sphinx.linguist.dictionary
{
	public class MappingDictionary : TextDictionary, Dictionary, Configurable
	{		
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
		
		public MappingDictionary(URL mappingFile, URL wordDictionaryFile, URL fillerDictionaryFile, List addendaUrlList, string wordReplacement, UnitManager unitManager) : base(wordDictionaryFile, fillerDictionaryFile, addendaUrlList, wordReplacement, unitManager)
		{
			this.mapping = new HashMap();
			this.mappingFile = mappingFile;
		}
		
		public MappingDictionary()
		{
			this.mapping = new HashMap();
		}
		
		public override void newProperties(PropertySheet ps)
		{
			base.newProperties(ps);
			this.mappingFile = ConfigurationManagerUtils.getResource("mapFile", ps);
		}
		
		public override void allocate()
		{
			base.allocate();
			if (!java.lang.String.instancehelper_equals(this.mappingFile.getFile(), ""))
			{
				this.loadMapping(this.mappingFile.openStream());
			}
		}
		
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
