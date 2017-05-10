using System;
using System.ComponentModel;

using IKVM.Attributes;
using ikvm.@internal;
using IKVM.Runtime;
using java.lang;
using java.lang.annotation;
using java.lang.reflect;
using java.net;
using java.util;
using java.util.logging;

namespace edu.cmu.sphinx.util.props
{
	[Implements(new string[]
	{
		"java.lang.Cloneable"
	})]
	public class PropertySheet : java.lang.Object, Cloneable.__Interface
	{

		public virtual string getInstanceName()
		{
			return this.instanceName;
		}

		[LineNumberTable(new byte[]
		{
			162,
			196,
			127,
			12,
			104,
			159,
			11,
			167,
			113,
			99,
			159,
			2
		})]
		
		public virtual Logger getLogger()
		{
			string text = new StringBuilder().append(ConfigurationManagerUtils.getLogPrefix(this.cm)).append(this.ownerClass.getName()).toString();
			Logger logger;
			if (this.instanceName != null)
			{
				logger = Logger.getLogger(new StringBuilder().append(text).append('.').append(this.instanceName).toString());
			}
			else
			{
				logger = Logger.getLogger(text);
			}
			object obj = this.rawProps.get("logLevel");
			if (obj != null)
			{
				logger.setLevel((!(obj is string)) ? ((Level)obj) : Level.parse((string)obj));
			}
			return logger;
		}

		[Throws(new string[]
		{
			"edu.cmu.sphinx.util.props.PropertyException"
		})]
		[LineNumberTable(new byte[]
		{
			160,
			167,
			109,
			130,
			108,
			135,
			142,
			109,
			168,
			109,
			116,
			100,
			138,
			223,
			29,
			108,
			151,
			99,
			110,
			162,
			137,
			110
		})]
		
		public virtual Configurable getComponent(string name)
		{
			S4PropWrapper property = this.getProperty(name, ClassLiteral<S4Component>.Value);
			Configurable configurable = null;
			S4Component s4Component = (S4Component)property.getAnnotation();
			Class @class = s4Component.type();
			object obj = this.propValues.get(name);
			if (obj != null && obj is Configurable)
			{
				return (Configurable)obj;
			}
			if (obj != null && obj is string)
			{
				PropertySheet propertySheet = this.cm.getPropertySheet(this.flattenProp(name));
				if (propertySheet == null)
				{
					string text = this.getInstanceName();
					string text2 = new StringBuilder().append("component '").append(this.flattenProp(name)).append("' is missing").toString();
					
					throw new InternalConfigurationException(text, name, text2);
				}
				configurable = propertySheet.getOwner();
			}
			if (configurable != null && !@class.isInstance(configurable))
			{
				string text3 = this.getInstanceName();
				string text4 = "mismatch between annotation and component type";
				
				throw new InternalConfigurationException(text3, name, text4);
			}
			if (configurable != null)
			{
				this.propValues.put(name, configurable);
				return configurable;
			}
			configurable = this.getComponentFromAnnotation(name, s4Component);
			this.propValues.put(name, configurable);
			return configurable;
		}

		[Throws(new string[]
		{
			"edu.cmu.sphinx.util.props.PropertyException"
		})]
		[LineNumberTable(new byte[]
		{
			160,
			138,
			109,
			140,
			110,
			152,
			173,
			104,
			137,
			141
		})]
		
		public virtual java.lang.Boolean getBoolean(string name)
		{
			S4PropWrapper property = this.getProperty(name, ClassLiteral<S4Boolean>.Value);
			S4Boolean s4Boolean = (S4Boolean)property.getAnnotation();
			if (this.propValues.get(name) == null)
			{
				this.propValues.put(name, java.lang.Boolean.valueOf(s4Boolean.defaultValue()));
			}
			object obj = this.propValues.get(name);
			java.lang.Boolean result;
			if (obj is java.lang.Boolean)
			{
				result = (java.lang.Boolean)obj;
			}
			else
			{
				result = java.lang.Boolean.valueOf(this.flattenProp(name));
			}
			return result;
		}

		[Throws(new string[]
		{
			"edu.cmu.sphinx.util.props.InternalConfigurationException"
		})]
		
		[LineNumberTable(new byte[]
		{
			161,
			29,
			141,
			146,
			127,
			16,
			103,
			245,
			69,
			99,
			97,
			235,
			70,
			134,
			127,
			0,
			110,
			130,
			142,
			114,
			143,
			135,
			125,
			148,
			100,
			108,
			104,
			223,
			25,
			133,
			175,
			114,
			102,
			125,
			106,
			145,
			127,
			9,
			154,
			101
		})]
		
		public virtual List getComponentList(string name, Class tclass)
		{
			this.getProperty(name, ClassLiteral<S4ComponentList>.Value);
			List list = (List)this.propValues.get(name);
			if (!PropertySheet.assertionsDisabled && !(((S4PropWrapper)this.registeredProperties.get(name)).getAnnotation() is S4ComponentList))
			{
				
				throw new AssertionError();
			}
			S4ComponentList s4ComponentList = (S4ComponentList)((S4PropWrapper)this.registeredProperties.get(name)).getAnnotation();
			List list2;
			ArrayList arrayList;
			Iterator iterator;
			if (list == null)
			{
				list2 = Arrays.asList(s4ComponentList.defaultList());
				arrayList = new ArrayList();
				iterator = list2.iterator();
				while (iterator.hasNext())
				{
					Class targetClass = (Class)iterator.next();
					arrayList.add(ConfigurationManager.getInstance(targetClass));
				}
				this.propValues.put(name, arrayList);
			}
			else if (!list.isEmpty() && !(list.get(0) is Configurable))
			{
				ArrayList arrayList2 = new ArrayList();
				Iterator iterator2 = list.iterator();
				while (iterator2.hasNext())
				{
					object obj = iterator2.next();
					Configurable configurable = this.cm.lookup((string)obj);
					if (configurable != null)
					{
						arrayList2.add(configurable);
					}
					else if (!s4ComponentList.beTolerant())
					{
						string text = (string)obj;
						string text2 = new StringBuilder().append("lookup of list-element '").append(obj).append("' failed!").toString();
						
						throw new InternalConfigurationException(name, text, text2);
					}
				}
				this.propValues.put(name, arrayList2);
			}
			list2 = (List)this.propValues.get(name);
			arrayList = new ArrayList();
			iterator = list2.iterator();
			while (iterator.hasNext())
			{
				object obj2 = iterator.next();
				if (!tclass.isInstance(obj2))
				{
					string text3 = this.getInstanceName();
					string text4 = new StringBuilder().append("Not all elements have required type ").append(tclass).append(" Found one of type ").append(Object.instancehelper_getClass(obj2)).toString();
					
					throw new InternalConfigurationException(text3, name, text4);
				}
				arrayList.add(tclass.cast(obj2));
			}
			return arrayList;
		}

		[Throws(new string[]
		{
			"edu.cmu.sphinx.util.props.PropertyException"
		})]
		[LineNumberTable(new byte[]
		{
			106,
			109,
			140,
			113,
			145,
			104,
			99,
			119,
			99,
			151,
			184,
			109,
			158,
			104,
			102,
			159,
			19,
			122,
			159,
			26
		})]
		
		public virtual int getInt(string name)
		{
			S4PropWrapper property = this.getProperty(name, ClassLiteral<S4Integer>.Value);
			S4Integer s4Integer = (S4Integer)property.getAnnotation();
			if (this.propValues.get(name) == null)
			{
				int num = (s4Integer.defaultValue() != -918273645) ? 1 : 0;
				if (s4Integer.mandatory())
				{
					if (num == 0)
					{
						string text = this.getInstanceName();
						string text2 = "mandatory property is not set!";
						
						throw new InternalConfigurationException(text, name, text2);
					}
				}
				else if (num == 0)
				{
					string text3 = this.getInstanceName();
					string text4 = "no default value for non-mandatory property";
					
					throw new InternalConfigurationException(text3, name, text4);
				}
				this.propValues.put(name, Integer.valueOf(s4Integer.defaultValue()));
			}
			object obj = this.propValues.get(name);
			Integer integer = (!(obj is Integer)) ? Integer.decode(this.flattenProp(name)) : ((Integer)obj);
			int[] array = s4Integer.range();
			if (array.Length != 2)
			{
				string text5 = this.getInstanceName();
				string text6 = new StringBuilder().append(Arrays.toString(array)).append(" is not of expected range type, which is {minValue, maxValue)").toString();
				
				throw new InternalConfigurationException(text5, name, text6);
			}
			if (integer.intValue() < array[0] || integer.intValue() > array[1])
			{
				string text7 = this.getInstanceName();
				string text8 = new StringBuilder().append(" is not in range (").append(Arrays.toString(array)).append(')').toString();
				
				throw new InternalConfigurationException(text7, name, text8);
			}
			return integer.intValue();
		}

		[Throws(new string[]
		{
			"edu.cmu.sphinx.util.props.PropertyException"
		})]
		[LineNumberTable(new byte[]
		{
			160,
			93,
			109,
			140,
			113,
			149,
			104,
			99,
			119,
			99,
			151,
			184,
			173,
			104,
			106,
			104,
			148,
			142,
			104,
			102,
			159,
			19,
			122,
			159,
			26
		})]
		
		public virtual double getDouble(string name)
		{
			S4PropWrapper property = this.getProperty(name, ClassLiteral<S4Double>.Value);
			S4Double s4Double = (S4Double)property.getAnnotation();
			if (this.propValues.get(name) == null)
			{
				int num = (s4Double.defaultValue() != -918273645.12345) ? 1 : 0;
				if (s4Double.mandatory())
				{
					if (num == 0)
					{
						string text = this.getInstanceName();
						string text2 = "mandatory property is not set!";
						
						throw new InternalConfigurationException(text, name, text2);
					}
				}
				else if (num == 0)
				{
					string text3 = this.getInstanceName();
					string text4 = "no default value for non-mandatory property";
					
					throw new InternalConfigurationException(text3, name, text4);
				}
				this.propValues.put(name, Double.valueOf(s4Double.defaultValue()));
			}
			object obj = this.propValues.get(name);
			Double @double;
			if (obj is Double)
			{
				@double = (Double)obj;
			}
			else if (obj is Number)
			{
				@double = Double.valueOf(((Number)obj).doubleValue());
			}
			else
			{
				@double = Double.valueOf(this.flattenProp(name));
			}
			double[] array = s4Double.range();
			if (array.Length != 2)
			{
				string text5 = this.getInstanceName();
				string text6 = new StringBuilder().append(Arrays.toString(array)).append(" is not of expected range type, which is {minValue, maxValue)").toString();
				
				throw new InternalConfigurationException(text5, name, text6);
			}
			if (@double.doubleValue() < array[0] || @double.doubleValue() > array[1])
			{
				string text7 = this.getInstanceName();
				string text8 = new StringBuilder().append(" is not in range (").append(Arrays.toString(array)).append(')').toString();
				
				throw new InternalConfigurationException(text7, name, text8);
			}
			return @double.doubleValue();
		}

		[Throws(new string[]
		{
			"edu.cmu.sphinx.util.props.PropertyException"
		})]
		
		
		public virtual float getFloat(string name)
		{
			return Double.valueOf(this.getDouble(name)).floatValue();
		}

		[Throws(new string[]
		{
			"edu.cmu.sphinx.util.props.PropertyException"
		})]
		[LineNumberTable(new byte[]
		{
			67,
			109,
			140,
			110,
			151,
			104,
			99,
			151,
			185,
			168,
			109,
			115,
			159,
			21
		})]
		
		public virtual string getString(string name)
		{
			S4PropWrapper property = this.getProperty(name, ClassLiteral<S4String>.Value);
			S4String s4String = (S4String)property.getAnnotation();
			if (this.propValues.get(name) == null)
			{
				int num = java.lang.String.instancehelper_equals(s4String.defaultValue(), "nullnullnull") ? 0 : 1;
				if (s4String.mandatory() && num == 0)
				{
					string text = this.getInstanceName();
					string text2 = "mandatory property is not set!";
					
					throw new InternalConfigurationException(text, name, text2);
				}
				this.propValues.put(name, (num == 0) ? null : s4String.defaultValue());
			}
			string text3 = this.flattenProp(name);
			List list = Arrays.asList(s4String.range());
			if (!list.isEmpty() && !list.contains(text3))
			{
				string text4 = this.getInstanceName();
				string text5 = new StringBuilder().append(" is not in range (").append(list).append(')').toString();
				
				throw new InternalConfigurationException(text4, name, text5);
			}
			return text3;
		}

		
		[LineNumberTable(new byte[]
		{
			161,
			99,
			102,
			136,
			102,
			159,
			1,
			105,
			212,
			226,
			61,
			97,
			255,
			7,
			59,
			235,
			74
		})]
		
		public virtual List getResourceList(string name)
		{
			ArrayList arrayList = new ArrayList();
			string @string = this.getString(name);
			if (@string != null)
			{
				string[] array = java.lang.String.instancehelper_split(@string, ";");
				int num = array.Length;
				int i = 0;
				while (i < num)
				{
					string text = array[i];
					try
					{
						URL url = new URL(text);
						arrayList.add(url);
					}
					catch (MalformedURLException ex)
					{
						goto IL_4E;
					}
					i++;
					continue;
					IL_4E:
					string text2 = new StringBuilder().append(text).append(" is not a valid URL.").toString();
					
					throw new IllegalArgumentException(text2);
				}
			}
			return arrayList;
		}

		[Throws(new string[]
		{
			"edu.cmu.sphinx.util.props.InternalConfigurationException"
		})]
		
		[LineNumberTable(new byte[]
		{
			161,
			10,
			141
		})]
		
		public virtual List getStringList(string name)
		{
			this.getProperty(name, ClassLiteral<S4StringList>.Value);
			return ConfigurationManagerUtils.toStringList(this.propValues.get(name));
		}

		
		[LineNumberTable(new byte[]
		{
			159,
			187,
			232,
			41,
			107,
			235,
			71,
			235,
			80,
			103,
			103,
			135,
			103,
			167,
			110,
			146,
			127,
			6,
			118
		})]
		
		public PropertySheet(Class confClass, string name, ConfigurationManager cm, RawPropertyData rpd)
		{
			this.registeredProperties = new HashMap();
			this.propValues = new HashMap();
			this.rawProps = new HashMap();
			this.ownerClass = confClass;
			this.cm = cm;
			this.instanceName = name;
			PropertySheet.parseClass(confClass);
			this.setConfigurableClass(confClass);
			Map properties = rpd.flatten(cm).getProperties();
			this.rawProps = new HashMap(rpd.getProperties());
			Iterator iterator = this.rawProps.keySet().iterator();
			while (iterator.hasNext())
			{
				string text = (string)iterator.next();
				this.propValues.put(text, properties.get(text));
			}
		}

		public virtual bool isInstanciated()
		{
			return this.owner != null;
		}

		
		public virtual Class getConfigurableClass()
		{
			return this.ownerClass;
		}

		[LineNumberTable(new byte[]
		{
			161,
			139,
			136,
			103,
			104,
			103,
			181,
			123,
			255,
			11,
			70,
			226,
			60,
			97,
			127,
			19,
			98,
			191,
			20
		})]
		
		public virtual Configurable getOwner()
		{
			IllegalAccessException ex2;
			InstantiationException ex4;
			try
			{
				try
				{
					if (!this.isInstanciated())
					{
						Collection undefinedMandatoryProps = this.getUndefinedMandatoryProps();
						if (!undefinedMandatoryProps.isEmpty())
						{
							string text = this.getInstanceName();
							string text2 = Object.instancehelper_toString(undefinedMandatoryProps);
							string text3 = "not all mandatory properties are defined";
							
							throw new InternalConfigurationException(text, text2, text3);
						}
						this.owner = (Configurable)this.ownerClass.newInstance(PropertySheet.__<GetCallerID>());
						this.owner.newProperties(this);
					}
				}
				catch (IllegalAccessException ex)
				{
					ex2 = ByteCodeHelper.MapException<IllegalAccessException>(ex, 1);
					goto IL_72;
				}
			}
			catch (InstantiationException ex3)
			{
				ex4 = ByteCodeHelper.MapException<InstantiationException>(ex3, 1);
				goto IL_75;
			}
			return this.owner;
			IL_72:
			IllegalAccessException ex5 = ex2;
			Exception ex6 = ex5;
			string text4 = this.getInstanceName();
			string text5 = null;
			string text6 = new StringBuilder().append("Can't access class ").append(this.ownerClass).toString();
			
			throw new InternalConfigurationException(ex6, text4, text5, text6);
			IL_75:
			InstantiationException ex7 = ex4;
			Exception ex8 = ex7;
			string text7 = this.getInstanceName();
			string text8 = null;
			string text9 = new StringBuilder().append("Can't instantiate class ").append(this.ownerClass).toString();
			
			throw new InternalConfigurationException(ex8, text7, text8, text9);
		}

		[LineNumberTable(new byte[]
		{
			159,
			182,
			114,
			103
		})]
		
		public PropertySheet(Configurable configurable, string name, RawPropertyData rpd, ConfigurationManager ConfigurationManager) : this(Object.instancehelper_getClass(configurable), name, ConfigurationManager, rpd)
		{
			this.owner = configurable;
		}

		public virtual void setCM(ConfigurationManager cm)
		{
			this.cm = cm;
		}

		[LineNumberTable(new byte[]
		{
			162,
			228,
			107,
			130,
			103,
			125,
			162
		})]
		
		public override bool equals(object obj)
		{
			if (obj == null || !(obj is PropertySheet))
			{
				return false;
			}
			PropertySheet propertySheet = (PropertySheet)obj;
			return this.rawProps.keySet().equals(propertySheet.rawProps.keySet());
		}

		[Throws(new string[]
		{
			"java.lang.CloneNotSupportedException"
		})]
		[LineNumberTable(new byte[]
		{
			162,
			252,
			140,
			113,
			145,
			177,
			127,
			1,
			110,
			126,
			142,
			130,
			108,
			103,
			140
		})]
		
		protected internal virtual PropertySheet clone()
		{
			PropertySheet propertySheet = (PropertySheet)base.clone();
			propertySheet.registeredProperties = new HashMap(this.registeredProperties);
			propertySheet.propValues = new HashMap(this.propValues);
			propertySheet.rawProps = new HashMap(this.rawProps);
			Iterator iterator = propertySheet.getRegisteredProperties().iterator();
			while (iterator.hasNext())
			{
				string text = (string)iterator.next();
				if (this.getType(text) == PropertyType.__COMPONENT_LIST)
				{
					propertySheet.rawProps.put(text, ConfigurationManagerUtils.toStringList(this.rawProps.get(text)));
					propertySheet.propValues.put(text, null);
				}
			}
			propertySheet.cm = this.cm;
			propertySheet.owner = null;
			propertySheet.instanceName = this.instanceName;
			return propertySheet;
		}

		[LineNumberTable(new byte[]
		{
			163,
			24,
			127,
			6,
			109,
			130,
			110,
			98,
			130
		})]
		
		public virtual bool validate()
		{
			Iterator iterator = this.rawProps.keySet().iterator();
			while (iterator.hasNext())
			{
				string text = (string)iterator.next();
				if (!java.lang.String.instancehelper_equals(text, "logLevel"))
				{
					if (!this.registeredProperties.containsKey(text))
					{
						return false;
					}
				}
			}
			return true;
		}

		
		
		
		public virtual Collection getRegisteredProperties()
		{
			return Collections.unmodifiableCollection(this.registeredProperties.keySet());
		}

		
		
		public virtual object getRaw(string name)
		{
			return this.rawProps.get(name);
		}

		[LineNumberTable(new byte[]
		{
			162,
			115,
			110,
			110
		})]
		
		internal virtual void setRaw(string text, object obj)
		{
			this.rawProps.put(text, obj);
			this.propValues.put(text, null);
		}

		
		
		public virtual object getRawNoReplacement(string name)
		{
			return this.rawProps.get(name);
		}

		[LineNumberTable(new byte[]
		{
			162,
			149,
			114,
			99,
			191,
			18,
			103,
			104,
			102,
			104,
			102,
			104,
			102,
			104,
			102,
			104,
			102,
			104,
			134
		})]
		
		public virtual PropertyType getType(string propName)
		{
			S4PropWrapper s4PropWrapper = (S4PropWrapper)this.registeredProperties.get(propName);
			if (s4PropWrapper == null)
			{
				string text = this.getInstanceName();
				string text2 = new StringBuilder().append(" is not a valid property of").append(this.getConfigurableClass()).toString();
				
				throw new InternalConfigurationException(text, propName, text2);
			}
			Annotation annotation = s4PropWrapper.getAnnotation();
			if (annotation is S4Component)
			{
				return PropertyType.__COMPONENT;
			}
			if (annotation is S4ComponentList)
			{
				return PropertyType.__COMPONENT_LIST;
			}
			if (annotation is S4Integer)
			{
				return PropertyType.__INT;
			}
			if (annotation is S4Double)
			{
				return PropertyType.__DOUBLE;
			}
			if (annotation is S4Boolean)
			{
				return PropertyType.__BOOLEAN;
			}
			if (annotation is S4String)
			{
				return PropertyType.__STRING;
			}
			string text3 = "Unknown property type";
			
			throw new RuntimeException(text3);
		}

		public virtual void setInstanceName(string newInstanceName)
		{
			this.instanceName = newInstanceName;
		}

		internal virtual ConfigurationManager getPropertyManager()
		{
			return this.cm;
		}

		[Throws(new string[]
		{
			"edu.cmu.sphinx.util.props.PropertyException"
		})]
		[LineNumberTable(new byte[]
		{
			162,
			39,
			110,
			191,
			20,
			119,
			104,
			159,
			20,
			105
		})]
		
		public virtual void setBoolean(string name, java.lang.Boolean value)
		{
			if (!this.registeredProperties.containsKey(name))
			{
				string text = this.getInstanceName();
				string text2 = new StringBuilder().append('\'').append(name).append("' is not a registered boolean-property").toString();
				
				throw new InternalConfigurationException(text, name, text2);
			}
			Annotation annotation = ((S4PropWrapper)this.registeredProperties.get(name)).getAnnotation();
			if (!(annotation is S4Boolean))
			{
				string text3 = this.getInstanceName();
				string text4 = new StringBuilder().append('\'').append(name).append("' is of type boolean").toString();
				
				throw new InternalConfigurationException(text3, name, text4);
			}
			this.applyConfigurationChange(name, value, value);
		}

		[Throws(new string[]
		{
			"edu.cmu.sphinx.util.props.PropertyException"
		})]
		[LineNumberTable(new byte[]
		{
			162,
			20,
			110,
			191,
			20,
			119,
			104,
			159,
			20,
			117
		})]
		
		public virtual void setDouble(string name, double value)
		{
			if (!this.registeredProperties.containsKey(name))
			{
				string text = this.getInstanceName();
				string text2 = new StringBuilder().append('\'').append(name).append("' is not a registered double-property").toString();
				
				throw new InternalConfigurationException(text, name, text2);
			}
			Annotation annotation = ((S4PropWrapper)this.registeredProperties.get(name)).getAnnotation();
			if (!(annotation is S4Double))
			{
				string text3 = this.getInstanceName();
				string text4 = new StringBuilder().append('\'').append(name).append("' is of type double").toString();
				
				throw new InternalConfigurationException(text3, name, text4);
			}
			this.applyConfigurationChange(name, Double.valueOf(value), Double.valueOf(value));
		}

		[Throws(new string[]
		{
			"edu.cmu.sphinx.util.props.PropertyException"
		})]
		[LineNumberTable(new byte[]
		{
			162,
			0,
			110,
			191,
			20,
			119,
			104,
			159,
			20,
			115
		})]
		
		public virtual void setInt(string name, int value)
		{
			if (!this.registeredProperties.containsKey(name))
			{
				string text = this.getInstanceName();
				string text2 = new StringBuilder().append('\'').append(name).append("' is not a registered int-property").toString();
				
				throw new InternalConfigurationException(text, name, text2);
			}
			Annotation annotation = ((S4PropWrapper)this.registeredProperties.get(name)).getAnnotation();
			if (!(annotation is S4Integer))
			{
				string text3 = this.getInstanceName();
				string text4 = new StringBuilder().append('\'').append(name).append("' is of type int").toString();
				
				throw new InternalConfigurationException(text3, name, text4);
			}
			this.applyConfigurationChange(name, Integer.valueOf(value), Integer.valueOf(value));
		}

		[Throws(new string[]
		{
			"edu.cmu.sphinx.util.props.PropertyException"
		})]
		[LineNumberTable(new byte[]
		{
			161,
			236,
			110,
			191,
			20,
			119,
			104,
			159,
			20,
			105
		})]
		
		public virtual void setString(string name, string value)
		{
			if (!this.registeredProperties.containsKey(name))
			{
				string text = this.getInstanceName();
				string text2 = new StringBuilder().append('\'').append(name).append("' is not a registered string-property").toString();
				
				throw new InternalConfigurationException(text, name, text2);
			}
			Annotation annotation = ((S4PropWrapper)this.registeredProperties.get(name)).getAnnotation();
			if (!(annotation is S4String))
			{
				string text3 = this.getInstanceName();
				string text4 = new StringBuilder().append('\'').append(name).append("' is of type string").toString();
				
				throw new InternalConfigurationException(text3, name, text4);
			}
			this.applyConfigurationChange(name, value, value);
		}

		[Throws(new string[]
		{
			"edu.cmu.sphinx.util.props.PropertyException"
		})]
		[LineNumberTable(new byte[]
		{
			162,
			59,
			110,
			191,
			20,
			119,
			104,
			191,
			20,
			105
		})]
		
		public virtual void setComponent(string name, string cmName, Configurable value)
		{
			if (!this.registeredProperties.containsKey(name))
			{
				string text = this.getInstanceName();
				string text2 = new StringBuilder().append('\'').append(name).append("' is not a registered compontent").toString();
				
				throw new InternalConfigurationException(text, name, text2);
			}
			Annotation annotation = ((S4PropWrapper)this.registeredProperties.get(name)).getAnnotation();
			if (!(annotation is S4Component))
			{
				string text3 = this.getInstanceName();
				string text4 = new StringBuilder().append('\'').append(name).append("' is of type component").toString();
				
				throw new InternalConfigurationException(text3, name, text4);
			}
			this.applyConfigurationChange(name, cmName, value);
		}

		[Throws(new string[]
		{
			"edu.cmu.sphinx.util.props.PropertyException"
		})]
		
		[LineNumberTable(new byte[]
		{
			162,
			81,
			110,
			191,
			20,
			119,
			104,
			159,
			20,
			110,
			142,
			105
		})]
		
		public virtual void setComponentList(string name, List valueNames, List value)
		{
			if (!this.registeredProperties.containsKey(name))
			{
				string text = this.getInstanceName();
				string text2 = new StringBuilder().append('\'').append(name).append("' is not a registered component-list").toString();
				
				throw new InternalConfigurationException(text, name, text2);
			}
			Annotation annotation = ((S4PropWrapper)this.registeredProperties.get(name)).getAnnotation();
			if (!(annotation is S4ComponentList))
			{
				string text3 = this.getInstanceName();
				string text4 = new StringBuilder().append('\'').append(name).append("' is of type component-list").toString();
				
				throw new InternalConfigurationException(text3, name, text4);
			}
			this.rawProps.put(name, valueNames);
			this.propValues.put(name, value);
			this.applyConfigurationChange(name, valueNames, value);
		}

		
		[LineNumberTable(new byte[]
		{
			161,
			199,
			167,
			104,
			240,
			69,
			102,
			108,
			159,
			4,
			189,
			191,
			40,
			120,
			189,
			2,
			98,
			135,
			101
		})]
		
		internal virtual void setConfigurableClass(Class @class)
		{
			this.ownerClass = @class;
			if (this.isInstanciated())
			{
				string text = "class is already instantiated";
				
				throw new RuntimeException(text);
			}
			HashSet hashSet = new HashSet();
			Map map = PropertySheet.parseClass(this.ownerClass);
			Iterator iterator = map.entrySet().iterator();
			while (iterator.hasNext())
			{
				Map.Entry entry = (Map.Entry)iterator.next();
				IllegalAccessException ex2;
				try
				{
					string text2 = (string)((Field)entry.getKey()).get(null, PropertySheet.__<GetCallerID>());
					if (!PropertySheet.assertionsDisabled && hashSet.contains(text2))
					{
						object obj = new StringBuilder().append("duplicate property-name for different properties: ").append(text2).append(" for the class ").append(@class).toString();
						
						throw new AssertionError(obj);
					}
					this.registerProperty(text2, new S4PropWrapper((Annotation)entry.getValue()));
					hashSet.add(text2);
				}
				catch (IllegalAccessException ex)
				{
					ex2 = ByteCodeHelper.MapException<IllegalAccessException>(ex, 1);
					goto IL_E9;
				}
				continue;
				IL_E9:
				IllegalAccessException ex3 = ex2;
				Throwable.instancehelper_printStackTrace(ex3);
			}
		}

		
		[LineNumberTable(new byte[]
		{
			163,
			42,
			140,
			102,
			118,
			137,
			124,
			142,
			124,
			108,
			105,
			127,
			1,
			127,
			1,
			127,
			1,
			159,
			11,
			235,
			56,
			235,
			61,
			235,
			61,
			235,
			84
		})]
		
		private static Map parseClass(Class @class)
		{
			Field[] fields = @class.getFields(PropertySheet.__<GetCallerID>());
			HashMap hashMap = new HashMap();
			Field[] array = fields;
			int num = array.Length;
			for (int i = 0; i < num; i++)
			{
				Field field = array[i];
				Annotation[] annotations = field.getAnnotations();
				Annotation[] array2 = annotations;
				int num2 = array2.Length;
				for (int j = 0; j < num2; j++)
				{
					Annotation annotation = array2[j];
					Annotation[] annotations2 = annotation.annotationType().getAnnotations();
					Annotation[] array3 = annotations2;
					int num3 = array3.Length;
					for (int k = 0; k < num3; k++)
					{
						Annotation annotation2 = array3[k];
						if (annotation2 is S4Property)
						{
							int modifiers = field.getModifiers();
							if (!PropertySheet.assertionsDisabled && !Modifier.isStatic(modifiers))
							{
								object obj = "property fields are assumed to be static";
								
								throw new AssertionError(obj);
							}
							if (!PropertySheet.assertionsDisabled && !Modifier.isPublic(modifiers))
							{
								object obj2 = "property fields are assumed to be public";
								
								throw new AssertionError(obj2);
							}
							if (!PropertySheet.assertionsDisabled && !Modifier.isFinal(modifiers))
							{
								object obj3 = "property fields are assumed to be final";
								
								throw new AssertionError(obj3);
							}
							if (!PropertySheet.assertionsDisabled && !Object.instancehelper_equals(field.getType(), ClassLiteral<java.lang.String>.Value))
							{
								object obj4 = "properties fields are assumed to be instances of java.lang.String";
								
								throw new AssertionError(obj4);
							}
							hashMap.put(field, annotation);
						}
					}
				}
			}
			return hashMap;
		}

		[Throws(new string[]
		{
			"edu.cmu.sphinx.util.props.PropertyException"
		})]
		
		[LineNumberTable(new byte[]
		{
			40,
			110,
			191,
			23,
			146,
			99,
			223,
			18,
			191,
			5,
			2,
			97,
			191,
			40
		})]
		
		public virtual S4PropWrapper getProperty(string name, Class propertyClass)
		{
			if (!this.propValues.containsKey(name))
			{
				string text = this.getInstanceName();
				string text2 = new StringBuilder().append("Unknown property '").append(name).append("' ! Make sure that you've annotated it.").toString();
				
				throw new InternalConfigurationException(text, name, text2);
			}
			S4PropWrapper s4PropWrapper = (S4PropWrapper)this.registeredProperties.get(name);
			if (s4PropWrapper == null)
			{
				string text3 = this.getInstanceName();
				string text4 = new StringBuilder().append("Property is not an annotated property of ").append(this.getConfigurableClass()).toString();
				
				throw new InternalConfigurationException(text3, name, text4);
			}
			ClassCastException ex3;
			try
			{
				propertyClass.cast(s4PropWrapper.getAnnotation());
			}
			catch (Exception ex)
			{
				ClassCastException ex2 = ByteCodeHelper.MapException<ClassCastException>(ex, 0);
				if (ex2 == null)
				{
					throw;
				}
				ex3 = ex2;
				goto IL_AB;
			}
			return s4PropWrapper;
			IL_AB:
			ClassCastException ex4 = ex3;
			Exception ex5 = ex4;
			string text5 = this.getInstanceName();
			string text6 = new StringBuilder().append("Property annotation ").append(s4PropWrapper.getAnnotation()).append(" doesn't match the required type ").append(propertyClass.getName()).toString();
			
			throw new InternalConfigurationException(ex5, text5, name, text6);
		}

		[LineNumberTable(new byte[]
		{
			92,
			109
		})]
		
		private string flattenProp(string text)
		{
			object obj = this.propValues.get(text);
			return (!(obj is string)) ? null : ((string)obj);
		}

		[LineNumberTable(new byte[]
		{
			160,
			205,
			135,
			117,
			183,
			117,
			223,
			18,
			109,
			104,
			191,
			24,
			194,
			103,
			99,
			183
		})]
		
		private Configurable getComponentFromAnnotation(string text, S4Component s4Component)
		{
			Class @class = s4Component.defaultClass();
			if (Object.instancehelper_equals(@class, ClassLiteral<Configurable>.Value) && s4Component.mandatory())
			{
				string text2 = this.getInstanceName();
				string text3 = "mandatory property is not set!";
				
				throw new InternalConfigurationException(text2, text, text3);
			}
			if (Modifier.isAbstract(@class.getModifiers()) && s4Component.mandatory())
			{
				string text4 = this.getInstanceName();
				string text5 = new StringBuilder().append(@class.getName()).append(" is abstract!").toString();
				
				throw new InternalConfigurationException(text4, text, text5);
			}
			if (Object.instancehelper_equals(@class, ClassLiteral<Configurable>.Value))
			{
				if (s4Component.mandatory())
				{
					string text6 = this.getInstanceName();
					string text7 = new StringBuilder().append(this.instanceName).append(": no default class defined for ").append(text).toString();
					
					throw new InternalConfigurationException(text6, text, text7);
				}
				return null;
			}
			else
			{
				Configurable instance = ConfigurationManager.getInstance(@class);
				if (instance == null)
				{
					string text8 = this.getInstanceName();
					string text9 = "instantiation of referenenced configurable failed";
					
					throw new InternalConfigurationException(text8, text, text9);
				}
				return instance;
			}
		}

		
		[LineNumberTable(new byte[]
		{
			161,
			165,
			102,
			127,
			4,
			151,
			99,
			104,
			127,
			6,
			104,
			127,
			16,
			104,
			127,
			8,
			104,
			191,
			10,
			127,
			1,
			104,
			101
		})]
		
		public virtual Collection getUndefinedMandatoryProps()
		{
			ArrayList arrayList = new ArrayList();
			Iterator iterator = this.getRegisteredProperties().iterator();
			while (iterator.hasNext())
			{
				string text = (string)iterator.next();
				Annotation annotation = ((S4PropWrapper)this.registeredProperties.get(text)).getAnnotation();
				int num = 0;
				if (annotation is S4Component)
				{
					num = ((!((S4Component)annotation).mandatory() || ((S4Component)annotation).defaultClass() != null) ? 0 : 1);
				}
				else if (annotation is S4String)
				{
					num = ((!((S4String)annotation).mandatory() || !java.lang.String.instancehelper_equals(((S4String)annotation).defaultValue(), "nullnullnull")) ? 0 : 1);
				}
				else if (annotation is S4Integer)
				{
					num = ((!((S4Integer)annotation).mandatory() || ((S4Integer)annotation).defaultValue() != -918273645) ? 0 : 1);
				}
				else if (annotation is S4Double)
				{
					num = ((!((S4Double)annotation).mandatory() || ((S4Double)annotation).defaultValue() != -918273645.12345) ? 0 : 1);
				}
				if (num != 0 && this.rawProps.get(text) == null && this.propValues.get(text) == null)
				{
					arrayList.add(text);
				}
			}
			return arrayList;
		}

		[LineNumberTable(new byte[]
		{
			19,
			102,
			151,
			110,
			142,
			110,
			110,
			142
		})]
		
		private void registerProperty(string text, S4PropWrapper s4PropWrapper)
		{
			if (s4PropWrapper == null || text == null)
			{
				string text2 = this.getInstanceName();
				string text3 = "property or its value is null";
				
				throw new InternalConfigurationException(text2, text, text3);
			}
			if (!this.registeredProperties.containsKey(text))
			{
				this.registeredProperties.put(text, s4PropWrapper);
			}
			if (!this.propValues.containsKey(text))
			{
				this.propValues.put(text, null);
				this.rawProps.put(text, null);
			}
		}

		[Throws(new string[]
		{
			"edu.cmu.sphinx.util.props.PropertyException"
		})]
		[LineNumberTable(new byte[]
		{
			162,
			97,
			110,
			148,
			104,
			146,
			104,
			108
		})]
		
		private void applyConfigurationChange(string text, object obj, object obj2)
		{
			this.rawProps.put(text, obj);
			this.propValues.put(text, (obj2 == null) ? obj : obj2);
			if (this.getInstanceName() != null)
			{
				this.cm.fireConfChanged(this.getInstanceName(), text);
			}
			if (this.owner != null)
			{
				this.owner.newProperties(this);
			}
		}

		
		[LineNumberTable(new byte[]
		{
			160,
			238,
			130,
			142,
			124,
			215,
			226,
			61,
			97,
			115,
			103,
			130,
			124,
			103,
			104,
			162
		})]
		
		public virtual Class getComponentClass(string propName)
		{
			Class result;
			if (this.propValues.get(propName) != null)
			{
				try
				{
					Class @class = Class.forName((string)this.propValues.get(propName), PropertySheet.__<GetCallerID>());
					result = @class.asSubclass(ClassLiteral<Configurable>.Value);
				}
				catch (ClassNotFoundException ex)
				{
					goto IL_40;
				}
				return result;
				IL_40:
				PropertySheet propertySheet = this.cm.getPropertySheet(this.flattenProp(propName));
				result = propertySheet.ownerClass;
			}
			else
			{
				S4Component s4Component = (S4Component)((S4PropWrapper)this.registeredProperties.get(propName)).getAnnotation();
				result = s4Component.defaultClass();
				if (s4Component.mandatory())
				{
					result = null;
				}
			}
			return result;
		}

		[LineNumberTable(new byte[]
		{
			162,
			241,
			119
		})]
		
		public override int hashCode()
		{
			if (!PropertySheet.assertionsDisabled)
			{
				object obj = "hashCode not designed";
				
				throw new AssertionError(obj);
			}
			return 1;
		}

		
		
		public override string toString()
		{
			return new StringBuilder().append(this.getInstanceName()).append("; isInstantiated=").append(this.isInstanciated()).append("; props=").append(this.rawProps.keySet()).toString();
		}

		[Throws(new string[]
		{
			"java.lang.CloneNotSupportedException"
		})]
		
		[EditorBrowsable(EditorBrowsableState.Never)]
		
		
		protected internal virtual object <bridge>clone()
		{
			return this.clone();
		}

		
		static PropertySheet()
		{
		}

		private static CallerID __<GetCallerID>()
		{
			if (PropertySheet.__<callerID> == null)
			{
				PropertySheet.__<callerID> = new PropertySheet.__<CallerID>();
			}
			return PropertySheet.__<callerID>;
		}

		
		public static implicit operator Cloneable(PropertySheet _<ref>)
		{
			Cloneable result;
			result.__<ref> = _<ref>;
			return result;
		}

		public const string COMP_LOG_LEVEL = "logLevel";

		
		private Map registeredProperties;

		
		private Map propValues;

		
		private Map rawProps;

		private ConfigurationManager cm;

		private Configurable owner;

		
		private Class ownerClass;

		private string instanceName;

		
		internal static bool assertionsDisabled = !ClassLiteral<PropertySheet>.Value.desiredAssertionStatus();

		private static CallerID __<callerID>;

		private sealed class __<CallerID> : CallerID
		{
			internal __<CallerID>()
			{
			}
		}
	}
}
