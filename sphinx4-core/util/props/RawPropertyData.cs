using System;

using IKVM.Attributes;
using java.lang;
using java.util;

namespace edu.cmu.sphinx.util.props
{
	public class RawPropertyData : java.lang.Object
	{
		[LineNumberTable(new byte[]
		{
			159,
			176,
			109
		})]
		
		public RawPropertyData(string name, string className) : this(name, className, new HashMap())
		{
		}

		
		
		public virtual bool contains(string propName)
		{
			return this.properties.get(propName) != null;
		}

		[LineNumberTable(new byte[]
		{
			7,
			110
		})]
		
		public virtual void add(string propName, string propValue)
		{
			this.properties.put(propName, propValue);
		}

		public virtual string getName()
		{
			return this.name;
		}

		
		[LineNumberTable(new byte[]
		{
			17,
			110
		})]
		
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

		
		[LineNumberTable(new byte[]
		{
			159,
			186,
			104,
			103,
			103,
			103
		})]
		
		public RawPropertyData(string name, string className, Map properties)
		{
			this.name = name;
			this.className = className;
			this.properties = properties;
		}

		[LineNumberTable(new byte[]
		{
			59,
			146,
			127,
			9,
			103,
			104,
			114,
			178,
			115,
			133
		})]
		
		public virtual RawPropertyData flatten(ConfigurationManager cm)
		{
			RawPropertyData rawPropertyData = new RawPropertyData(this.name, this.className);
			Iterator iterator = this.properties.entrySet().iterator();
			while (iterator.hasNext())
			{
				Map.Entry entry = (Map.Entry)iterator.next();
				object obj = entry.getValue();
				if (obj is string && java.lang.String.instancehelper_startsWith((string)obj, "_{"))
				{
					obj = cm.getGloPropReference(ConfigurationManagerUtils.stripGlobalSymbol((string)obj));
				}
				rawPropertyData.properties.put(entry.getKey(), obj);
			}
			return rawPropertyData;
		}

		[LineNumberTable(new byte[]
		{
			26,
			109
		})]
		
		public virtual void remove(string propName)
		{
			this.properties.remove(propName);
		}

		
		[LineNumberTable(new byte[]
		{
			82,
			109,
			162,
			110,
			125
		})]
		
		public virtual string getGlobalProperty(string key, Map globalProperties)
		{
			if (!java.lang.String.instancehelper_startsWith(key, "_{"))
			{
				return key;
			}
			do
			{
				key = (string)globalProperties.get(key);
			}
			while (key != null && java.lang.String.instancehelper_startsWith(key, "_{") && java.lang.String.instancehelper_endsWith(key, "}"));
			return key;
		}

		[LineNumberTable(new byte[]
		{
			99,
			123,
			127,
			1,
			99,
			104,
			140,
			136,
			98
		})]
		
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
