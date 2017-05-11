using System;

using IKVM.Attributes;
using ikvm.@internal;
using IKVM.Runtime;
using java.io;
using java.lang;
using java.net;
using java.util;
using java.util.logging;
using org.xml.sax;
using org.xml.sax.helpers;

namespace edu.cmu.sphinx.util.props
{
	public class ConfigHandler : DefaultHandler
	{		
		public ConfigHandler(Map rpdMap, Map globalProperties, bool replaceDuplicates, URL baseURL)
		{
			this.__rpdMap = rpdMap;
			this.__globalProperties = globalProperties;
			this.replaceDuplicates = replaceDuplicates;
			this.baseURL = baseURL;
		}
		
		private void mergeConfigs(string text, bool flag)
		{
			try
			{
				try
				{
					File parentFile = new File(this.baseURL.toURI().getPath()).getParentFile();
					URL url = new File(new StringBuilder().append(parentFile.getPath()).append(File.separatorChar).append(text).toString()).toURI().toURL();
					Logger logger = Logger.getLogger(ClassLiteral<ConfigHandler>.Value.getSimpleName());
					logger.fine(new StringBuilder().append((!flag) ? "including" : "extending").append(" config:").append(url.toURI()).toString());
					SaxLoader saxLoader = new SaxLoader(url, this.__globalProperties, this.__rpdMap, flag);
					saxLoader.load();
				}
				catch (IOException ex)
				{
					throw new RuntimeException(new StringBuilder().append("Error while processing <include file=\"").append(text).append("\">: ").append(ex5).toString(), ex);
				}
			}
			catch (URISyntaxException ex3)
			{
				Throwable.instancehelper_printStackTrace(ex3);
			}
		}
		
		public ConfigHandler(Map rpdMap, Map globalProperties) : this(rpdMap, globalProperties, false, null)
		{
		}
		
		public override void startElement(string uri, string localName, string qName, Attributes attributes)
		{
			if (java.lang.String.instancehelper_equals(qName, "config"))
			{
				string value = attributes.getValue("extends");
				if (value != null)
				{
					this.mergeConfigs(value, true);
					this.replaceDuplicates = true;
				}
			}
			else if (java.lang.String.instancehelper_equals(qName, "include"))
			{
				string value = attributes.getValue("file");
				this.mergeConfigs(value, false);
			}
			else if (java.lang.String.instancehelper_equals(qName, "extendwith"))
			{
				string value = attributes.getValue("file");
				this.mergeConfigs(value, true);
			}
			else if (java.lang.String.instancehelper_equals(qName, "component"))
			{
				string value = attributes.getValue("name");
				string value2 = attributes.getValue("type");
				if (this.__rpdMap.get(value) != null && !this.replaceDuplicates)
				{
					string text = new StringBuilder().append("duplicate definition for ").append(value).toString();
					Locator locator = this.locator;
					
					throw new SAXParseException(text, locator);
				}
				this.rpd = new RawPropertyData(value, value2);
			}
			else if (java.lang.String.instancehelper_equals(qName, "property"))
			{
				string value = attributes.getValue("name");
				string value2 = attributes.getValue("value");
				if (attributes.getLength() != 2 || value == null || value2 == null)
				{
					string text2 = "property element must only have 'name' and 'value' attributes";
					Locator locator2 = this.locator;
					
					throw new SAXParseException(text2, locator2);
				}
				if (this.rpd == null)
				{
					this.__globalProperties.put(value, value2);
				}
				else
				{
					if (this.rpd.contains(value) && !this.replaceDuplicates)
					{
						string text3 = new StringBuilder().append("Duplicate property: ").append(value).toString();
						Locator locator3 = this.locator;
						
						throw new SAXParseException(text3, locator3);
					}
					this.rpd.add(value, value2);
				}
			}
			else if (java.lang.String.instancehelper_equals(qName, "propertylist"))
			{
				this.itemListName = attributes.getValue("name");
				if (attributes.getLength() != 1 || this.itemListName == null)
				{
					string text4 = "list element must only have the 'name'  attribute";
					Locator locator4 = this.locator;
					
					throw new SAXParseException(text4, locator4);
				}
				this.itemList = new ArrayList();
			}
			else
			{
				if (!java.lang.String.instancehelper_equals(qName, "item"))
				{
					string text5 = new StringBuilder().append("Unknown element '").append(qName).append('\'').toString();
					Locator locator5 = this.locator;
					
					throw new SAXParseException(text5, locator5);
				}
				if (attributes.getLength() != 0)
				{
					string text6 = "unknown 'item' attribute";
					Locator locator6 = this.locator;
					
					throw new SAXParseException(text6, locator6);
				}
				this.curItem = new StringBuilder();
			}
		}
		
		public override void characters(char[] ch, int start, int length)
		{
			if (this.curItem != null)
			{
				this.curItem.append(ch, start, length);
			}
		}
		
		public override void endElement(string uri, string localName, string qName)
		{
			if (java.lang.String.instancehelper_equals(qName, "component"))
			{
				this.__rpdMap.put(this.rpd.getName(), this.rpd);
				this.rpd = null;
			}
			else if (!java.lang.String.instancehelper_equals(qName, "property"))
			{
				if (java.lang.String.instancehelper_equals(qName, "propertylist"))
				{
					if (this.rpd.contains(this.itemListName))
					{
						string text = new StringBuilder().append("Duplicate property: ").append(this.itemListName).toString();
						Locator locator = this.locator;
						
						throw new SAXParseException(text, locator);
					}
					this.rpd.add(this.itemListName, this.itemList);
					this.itemList = null;
				}
				else if (java.lang.String.instancehelper_equals(qName, "item"))
				{
					this.itemList.add(java.lang.String.instancehelper_trim(this.curItem.toString()));
					this.curItem = null;
				}
			}
		}

		public override void setDocumentLocator(Locator locator)
		{
			this.locator = locator;
		}

		protected internal Map rpdMap
		{
			
			get
			{
				return this.__rpdMap;
			}
			
			private set
			{
				this.__rpdMap = value;
			}
		}
		
		protected internal Map globalProperties
		{
			
			get
			{
				return this.__globalProperties;
			}
			
			private set
			{
				this.__globalProperties = value;
			}
		}

		protected internal RawPropertyData rpd;

		protected internal Locator locator;

		protected internal List itemList;

		protected internal string itemListName;

		protected internal StringBuilder curItem;
		
		internal Map __rpdMap;
		
		internal Map __globalProperties;

		private bool replaceDuplicates;
		
		private URL baseURL;
	}
}
