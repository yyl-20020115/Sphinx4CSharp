using ikvm.@internal;
using java.lang;
using java.lang.annotation;
using java.lang.reflect;
using java.net;
using java.util;
using java.util.logging;

namespace edu.cmu.sphinx.util.props
{
	public class PropertySheet : Object, Cloneable.__Interface
	{
		public virtual string getInstanceName()
		{
			return this.instanceName;
		}
		
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
		
		public virtual Boolean getBoolean(string name)
		{
			S4PropWrapper property = this.getProperty(name, ClassLiteral<S4Boolean>.Value);
			S4Boolean s4Boolean = (S4Boolean)property.getAnnotation();
			if (this.propValues.get(name) == null)
			{
				this.propValues.put(name, Boolean.valueOf(s4Boolean.defaultValue()));
			}
			object obj = this.propValues.get(name);
			Boolean result;
			if (obj is Boolean)
			{
				result = (Boolean)obj;
			}
			else
			{
				result = Boolean.valueOf(this.flattenProp(name));
			}
			return result;
		}
		
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
		
		public virtual float getFloat(string name)
		{
			return Double.valueOf(this.getDouble(name)).floatValue();
		}
		
		public virtual string getString(string name)
		{
			S4PropWrapper property = this.getProperty(name, ClassLiteral<S4String>.Value);
			S4String s4String = (S4String)property.getAnnotation();
			if (this.propValues.get(name) == null)
			{
				int num = String.instancehelper_equals(s4String.defaultValue(), "nullnullnull") ? 0 : 1;
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
		
		public virtual List getResourceList(string name)
		{
			ArrayList arrayList = new ArrayList();
			string @string = this.getString(name);
			if (@string != null)
			{
				string[] array = String.instancehelper_split(@string, ";");
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
					catch (MalformedURLException)
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
		
		public virtual List getStringList(string name)
		{
			this.getProperty(name, ClassLiteral<S4StringList>.Value);
			return ConfigurationManagerUtils.toStringList(this.propValues.get(name));
		}
		
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
		
		public virtual Configurable getOwner()
		{
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
						this.owner = (Configurable)this.ownerClass.newInstance(PropertySheet.__GetCallerID());
						this.owner.newProperties(this);
					}
				}
				catch (IllegalAccessException ex)
				{
					throw new InternalConfigurationException(ex, this.getInstanceName(), null, new StringBuilder().append("Can't access class ").append(this.ownerClass).toString());
				}
			}
			catch (InstantiationException ex3)
			{
				throw new InternalConfigurationException(ex3, this.getInstanceName(), null, new StringBuilder().append("Can't instantiate class ").append(this.ownerClass).toString());
			}
			return this.owner;
			
		}
		
		public PropertySheet(Configurable configurable, string name, RawPropertyData rpd, ConfigurationManager ConfigurationManager) : this(Object.instancehelper_getClass(configurable), name, ConfigurationManager, rpd)
		{
			this.owner = configurable;
		}

		public virtual void setCM(ConfigurationManager cm)
		{
			this.cm = cm;
		}
		
		public override bool equals(object obj)
		{
			if (obj == null || !(obj is PropertySheet))
			{
				return false;
			}
			PropertySheet propertySheet = (PropertySheet)obj;
			return this.rawProps.keySet().equals(propertySheet.rawProps.keySet());
		}
		
		protected internal new virtual PropertySheet clone()
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
		
		public virtual bool validate()
		{
			Iterator iterator = this.rawProps.keySet().iterator();
			while (iterator.hasNext())
			{
				string text = (string)iterator.next();
				if (!String.instancehelper_equals(text, "logLevel"))
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
		
		internal virtual void setRaw(string text, object obj)
		{
			this.rawProps.put(text, obj);
			this.propValues.put(text, null);
		}
		
		public virtual object getRawNoReplacement(string name)
		{
			return this.rawProps.get(name);
		}
		
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
		
		public virtual void setBoolean(string name, Boolean value)
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
				try
				{
					string text2 = (string)((Field)entry.getKey()).get(null, PropertySheet.__GetCallerID());
					if (!PropertySheet.assertionsDisabled && hashSet.contains(text2))
					{
						object obj = new StringBuilder().append("duplicate property-name for different properties: ").append(text2).append(" for the class ").append(@class).toString();
						
						throw new AssertionError(obj);
					}
					this.registerProperty(text2, new S4PropWrapper((java.lang.annotation.Annotation)entry.getValue()));
					hashSet.add(text2);
				}
				catch (IllegalAccessException ex)
				{
					Throwable.instancehelper_printStackTrace(ex);
				}
				continue;
			}
		}
		
		private static Map parseClass(Class @class)
		{
			Field[] fields = @class.getFields(PropertySheet.__GetCallerID());
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
							if (!PropertySheet.assertionsDisabled && !Object.instancehelper_equals(field.getType(), ClassLiteral<string>.Value))
							{
								object obj4 = "properties fields are assumed to be instances of String";
								
								throw new AssertionError(obj4);
							}
							hashMap.put(field, annotation);
						}
					}
				}
			}
			return hashMap;
		}
		
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
			try
			{
				propertyClass.cast(s4PropWrapper.getAnnotation());
			}
			catch (System.Exception ex)
			{
				throw new InternalConfigurationException(ex, this.getInstanceName(), name, new StringBuilder().append("Property annotation ").append(s4PropWrapper.getAnnotation()).append(" doesn't match the required type ").append(propertyClass.getName()).toString());

			}
			return s4PropWrapper;
		}
		
		private string flattenProp(string text)
		{
			object obj = this.propValues.get(text);
			return (!(obj is string)) ? null : ((string)obj);
		}
		
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
					num = ((!((S4String)annotation).mandatory() || !String.instancehelper_equals(((S4String)annotation).defaultValue(), "nullnullnull")) ? 0 : 1);
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
		
		public virtual Class getComponentClass(string propName)
		{
			Class result;
			if (this.propValues.get(propName) != null)
			{
				try
				{
					Class @class = Class.forName((string)this.propValues.get(propName), PropertySheet.__GetCallerID());
					result = @class.asSubclass(ClassLiteral<Configurable>.Value);
				}
				catch (ClassNotFoundException)
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

		private static CallerID __GetCallerID()
		{
			if (PropertySheet.__callerID == null)
			{
				PropertySheet.__callerID = new PropertySheet.__CallerID();
			}
			return PropertySheet.__callerID;
		}
		
		public static implicit operator Cloneable(PropertySheet _ref)
		{
			Cloneable result = Cloneable.Cast(_ref);
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

		private static CallerID __callerID;

		private sealed class __CallerID : CallerID
		{
			internal __CallerID()
			{
			}
		}
	}
}
