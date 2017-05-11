using System;

using IKVM.Attributes;
using IKVM.Runtime;
using java.io;
using java.lang;
using java.net;
using java.util;
using javax.xml.parsers;
using org.xml.sax;

namespace edu.cmu.sphinx.util.props
{
	public class SaxLoader : java.lang.Object
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
			SAXParseException ex2;
			SAXException ex4;
			ParserConfigurationException ex6;
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
						ex2 = ByteCodeHelper.MapException<SAXParseException>(ex, 1);
						goto IL_6B;
					}
				}
				catch (SAXException ex3)
				{
					ex4 = ByteCodeHelper.MapException<SAXException>(ex3, 1);
					goto IL_6E;
				}
			}
			catch (ParserConfigurationException ex5)
			{
				ex6 = ByteCodeHelper.MapException<ParserConfigurationException>(ex5, 1);
				goto IL_72;
			}
			return this.rpdMap;
			IL_6B:
			SAXParseException ex7 = ex2;
			string text = new StringBuilder().append("Error while parsing line ").append(ex7.getLineNumber()).append(" of ").append(this.url).append(": ").append(ex7.getMessage()).toString();
			string text2 = text;
			
			throw new IOException(text2);
			IL_6E:
			SAXException ex8 = ex4;
			string text3 = new StringBuilder().append("Problem with XML: ").append(ex8).toString();
			
			throw new IOException(text3);
			IL_72:
			ParserConfigurationException ex9 = ex6;
			string text4 = Throwable.instancehelper_getMessage(ex9);
			
			throw new IOException(text4);
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
