﻿using java.lang;
using java.util;

namespace edu.cmu.sphinx.util.props
{
	public class RawPropertyData : Object
	{		
		public RawPropertyData(string name, string className) : this(name, className, new HashMap())
		{
		}
		
		public virtual bool contains(string propName)
		{
			return this.properties.get(propName) != null;
		}
		
		public virtual void add(string propName, string propValue)
		{
			this.properties.put(propName, propValue);
		}

		public virtual string getName()
		{
			return this.name;
		}
		
		public virtual void add(string propName, List propValue)
		{
			this.properties.put(propName, propValue);
		}

		public virtual string getClassName()
		{
			return this.className;
		}
		
		public virtual Map getProperties()
		{
			return this.properties;
		}
		
		public RawPropertyData(string name, string className, Map properties)
		{
			this.name = name;
			this.className = className;
			this.properties = properties;
		}
		
		public virtual RawPropertyData flatten(ConfigurationManager cm)
		{
			RawPropertyData rawPropertyData = new RawPropertyData(this.name, this.className);
			Iterator iterator = this.properties.entrySet().iterator();
			while (iterator.hasNext())
			{
				Map.Entry entry = (Map.Entry)iterator.next();
				object obj = entry.getValue();
				if (obj is string && String.instancehelper_startsWith((string)obj, "_{"))
				{
					obj = cm.getGloPropReference(ConfigurationManagerUtils.stripGlobalSymbol((string)obj));
				}
				rawPropertyData.properties.put(entry.getKey(), obj);
			}
			return rawPropertyData;
		}
		
		public virtual void remove(string propName)
		{
			this.properties.remove(propName);
		}
		
		public virtual string getGlobalProperty(string key, Map globalProperties)
		{
			if (!String.instancehelper_startsWith(key, "_{"))
			{
				return key;
			}
			do
			{
				key = (string)globalProperties.get(key);
			}
			while (key != null && String.instancehelper_startsWith(key, "_{") && String.instancehelper_endsWith(key, "}"));
			return key;
		}
		
		public override string toString()
		{
			StringBuilder stringBuilder = new StringBuilder().append("name : ").append(this.name);
			Iterator iterator = this.properties.values().iterator();
			while (iterator.hasNext())
			{
				object obj = iterator.next();
				if (obj != null)
				{
					if (obj is string)
					{
						stringBuilder.append("value string : ");
					}
					stringBuilder.append(obj);
				}
			}
			return stringBuilder.toString();
		}

		private string name;

		private string className;

		private Map properties;
	}
}
