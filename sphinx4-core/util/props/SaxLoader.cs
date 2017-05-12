using java.io;
using java.lang;
using java.net;
using java.util;
using javax.xml.parsers;
using org.xml.sax;

namespace edu.cmu.sphinx.util.props
{
	public class SaxLoader : Object
	{		
		public SaxLoader(URL url, Map globalProperties, Map initRPD, bool replaceDuplicates)
		{
			this.url = url;
			this.globalProperties = globalProperties;
			this.replaceDuplicates = replaceDuplicates;
			Map map = (initRPD != null) ? initRPD : new HashMap();
			Map map2;
			if (map != null)
			{
				if ((map2 = (map as Map)) == null)
				{
					throw new IncompatibleClassChangeError();
				}
			}
			else
			{
				map2 = null;
			}
			this.rpdMap = map2;
		}
		
		public virtual Map load()
		{
			try
			{
				try
				{
					try
					{
						SAXParserFactory saxparserFactory = SAXParserFactory.newInstance();
						XMLReader xmlreader = saxparserFactory.newSAXParser().getXMLReader();
						ConfigHandler contentHandler = new ConfigHandler(this.rpdMap, this.globalProperties, this.replaceDuplicates, this.url);
						xmlreader.setContentHandler(contentHandler);
						xmlreader.parse(this.url.toString());
					}
					catch (SAXParseException ex)
					{
						throw new IOException(new StringBuilder().append("Error while parsing line ").append(ex.getLineNumber()).append(" of ").append(this.url).append(": ").append(ex.getMessage()).toString());
					}
				}
				catch (SAXException ex3)
				{
					throw new IOException(new StringBuilder().append("Problem with XML: ").append(ex3).toString());
				}
			}
			catch (ParserConfigurationException ex5)
			{
				throw new IOException(Throwable.instancehelper_getMessage(ex5));
			}
			return this.rpdMap;
		}
		
		public SaxLoader(URL url, Map globalProperties) : this(url, globalProperties, null, false)
		{
		}
		
		private URL url;
		
		private Map rpdMap;
		
		private Map globalProperties;
		
		private bool replaceDuplicates;
	}
}
