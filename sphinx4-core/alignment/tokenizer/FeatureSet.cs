using System;

using IKVM.Attributes;
using java.lang;
using java.text;
using java.util;

namespace edu.cmu.sphinx.alignment.tokenizer
{
	public class FeatureSet : java.lang.Object
	{
		
		
		public virtual string getString(string name)
		{
			return (string)this.getObject(name);
		}

		
		
		public virtual bool isPresent(string name)
		{
			return this.featureMap.containsKey(name);
		}

		[LineNumberTable(new byte[]
		{
			83,
			104
		})]
		
		public virtual void setString(string name, string value)
		{
			this.setObject(name, value);
		}

		
		
		public virtual object getObject(string name)
		{
			return this.featureMap.get(name);
		}

		[LineNumberTable(new byte[]
		{
			93,
			110
		})]
		
		public virtual void setObject(string name, object value)
		{
			this.featureMap.put(name, value);
		}

		[LineNumberTable(new byte[]
		{
			159,
			170,
			104,
			107
		})]
		
		public FeatureSet()
		{
			this.featureMap = new LinkedHashMap();
		}

		[LineNumberTable(new byte[]
		{
			159,
			191,
			109
		})]
		
		public virtual void remove(string name)
		{
			this.featureMap.remove(name);
		}

		
		
		public virtual int getInt(string name)
		{
			return ((Integer)this.getObject(name)).intValue();
		}

		
		
		public virtual float getFloat(string name)
		{
			return ((Float)this.getObject(name)).floatValue();
		}

		[LineNumberTable(new byte[]
		{
			63,
			109
		})]
		
		public virtual void setInt(string name, int value)
		{
			this.setObject(name, new Integer(value));
		}

		[LineNumberTable(new byte[]
		{
			73,
			110
		})]
		
		public virtual void setFloat(string name, float value)
		{
			this.setObject(name, new Float(value));
		}

		
		
		private Map featureMap;

		internal static DecimalFormat formatter;
	}
}
