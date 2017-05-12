using java.lang;
using java.util;

namespace edu.cmu.sphinx.alignment.tokenizer
{
	public class FeatureSet : Object
	{
		public virtual string getString(string name)
		{
			return (string)this.getObject(name);
		}
				
		public virtual bool isPresent(string name)
		{
			return this.featureMap.containsKey(name);
		}
	
		public virtual void setString(string name, string value)
		{
			this.setObject(name, value);
		}

		public virtual object getObject(string name)
		{
			return this.featureMap.get(name);
		}
	
		public virtual void setObject(string name, object value)
		{
			this.featureMap.put(name, value);
		}
	
		public FeatureSet()
		{
			this.featureMap = new LinkedHashMap();
		}

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
	
		public virtual void setInt(string name, int value)
		{
			this.setObject(name, new Integer(value));
		}

		public virtual void setFloat(string name, float value)
		{
			this.setObject(name, new Float(value));
		}
		
		private Map featureMap;

		//internal static DecimalFormat formatter;
	}
}
