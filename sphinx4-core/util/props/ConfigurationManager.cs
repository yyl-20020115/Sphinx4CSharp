using System;
using System.ComponentModel;

using IKVM.Attributes;
using ikvm.@internal;
using IKVM.Runtime;
using java.io;
using java.lang;
using java.net;
using java.util;
using java.util.logging;

namespace edu.cmu.sphinx.util.props
{
	[Implements(new string[]
	{
		"java.lang.Cloneable"
	})]
	public class ConfigurationManager : java.lang.Object, Cloneable.__Interface
	{


		[Throws(new string[]
		{
			"edu.cmu.sphinx.util.props.PropertyException"
		})]
		[LineNumberTable(new byte[]
		{
			4,
			232,
			29,
			139,
			107,
			107,
			235,
			96,
			167,
			191,
			10,
			2,
			97,
			172,
			113,
			198,
			118,
			99,
			113
		})]
		
		public ConfigurationManager(URL url)
		{
			this.changeListeners = new ArrayList();
			this.symbolTable = new LinkedHashMap();
			this.rawPropertyMap = new HashMap();
			this.globalProperties = new HashMap();
			this.configURL = url;
			IOException ex2;
			try
			{
				this.rawPropertyMap = new SaxLoader(url, this.globalProperties).load();
			}
			catch (IOException ex)
			{
				ex2 = ByteCodeHelper.MapException<IOException>(ex, 1);
				goto IL_61;
			}
			ConfigurationManagerUtils.applySystemProperties(this.rawPropertyMap, this.globalProperties);
			ConfigurationManagerUtils.configureLogger(this);
			string text = (string)this.globalProperties.get("showCreations");
			if (text != null)
			{
				this.showCreations = java.lang.String.instancehelper_equals("true", text);
			}
			return;
			IL_61:
			IOException ex3 = ex2;
			Exception ex4 = ex3;
			
			throw new RuntimeException(ex4);
		}

		[Throws(new string[]
		{
			"edu.cmu.sphinx.util.props.InternalConfigurationException"
		})]
		
		[LineNumberTable(new byte[]
		{
			104,
			105,
			136,
			99,
			130,
			104,
			159,
			6
		})]
		
		public virtual Configurable lookup(string instanceName)
		{
			instanceName = this.getStrippedComponentName(instanceName);
			PropertySheet propertySheet = this.getPropertySheet(instanceName);
			if (propertySheet == null)
			{
				return null;
			}
			if (this.showCreations)
			{
				this.getRootLogger().config(new StringBuilder().append("Creating: ").append(instanceName).toString());
			}
			return propertySheet.getOwner();
		}

		[LineNumberTable(new byte[]
		{
			161,
			24,
			99,
			143,
			174,
			127,
			9,
			104,
			136,
			190,
			2,
			98,
			135,
			101
		})]
		
		public virtual void setGlobalProperty(string propertyName, string value)
		{
			if (value == null)
			{
				this.globalProperties.remove(propertyName);
			}
			else
			{
				this.globalProperties.put(propertyName, value);
			}
			Iterator iterator = this.getInstanceNames(ClassLiteral<Configurable>.Value).iterator();
			while (iterator.hasNext())
			{
				string instanceName = (string)iterator.next();
				PropertySheet propertySheet = this.getPropertySheet(instanceName);
				if (propertySheet.isInstanciated())
				{
					PropertyException ex2;
					try
					{
						propertySheet.getOwner().newProperties(propertySheet);
					}
					catch (PropertyException ex)
					{
						ex2 = ByteCodeHelper.MapException<PropertyException>(ex, 1);
						goto IL_73;
					}
					continue;
					IL_73:
					PropertyException ex3 = ex2;
					Throwable.instancehelper_printStackTrace(ex3);
				}
			}
		}

		
		[LineNumberTable(new byte[]
		{
			160,
			67,
			104,
			104,
			130,
			123
		})]
		
		public virtual Configurable lookup(Class confClass)
		{
			List propSheets = this.getPropSheets(confClass);
			if (propSheets.isEmpty())
			{
				return null;
			}
			if (!ConfigurationManager.assertionsDisabled && propSheets.size() != 1)
			{
				
				throw new AssertionError();
			}
			return (Configurable)confClass.cast(this.lookup(((PropertySheet)propSheets.get(0)).getInstanceName()));
		}

		[LineNumberTable(new byte[]
		{
			31,
			177,
			114,
			102,
			135,
			172,
			153,
			255,
			41,
			72,
			229,
			58,
			98,
			255,
			6,
			69,
			226,
			60,
			98,
			191,
			6,
			2,
			98,
			255,
			6,
			69
		})]
		
		public virtual PropertySheet getPropertySheet(string instanceName)
		{
			if (!this.symbolTable.containsKey(instanceName))
			{
				RawPropertyData rawPropertyData = (RawPropertyData)this.rawPropertyMap.get(instanceName);
				if (rawPropertyData != null)
				{
					string className = rawPropertyData.getClassName();
					ClassNotFoundException ex2;
					ClassCastException ex5;
					ExceptionInInitializerError exceptionInInitializerError2;
					try
					{
						try
						{
							try
							{
								Class @class = Class.forName(className, ConfigurationManager.__<GetCallerID>());
								PropertySheet.__<clinit>();
								PropertySheet propertySheet = new PropertySheet(@class.asSubclass(ClassLiteral<Configurable>.Value), instanceName, this, rawPropertyData);
								this.symbolTable.put(instanceName, propertySheet);
							}
							catch (ClassNotFoundException ex)
							{
								ex2 = ByteCodeHelper.MapException<ClassNotFoundException>(ex, 1);
								goto IL_91;
							}
						}
						catch (Exception ex3)
						{
							ClassCastException ex4 = ByteCodeHelper.MapException<ClassCastException>(ex3, 0);
							if (ex4 == null)
							{
								throw;
							}
							ex5 = ex4;
							goto IL_95;
						}
					}
					catch (Exception ex6)
					{
						ExceptionInInitializerError exceptionInInitializerError = ByteCodeHelper.MapException<ExceptionInInitializerError>(ex6, 0);
						if (exceptionInInitializerError == null)
						{
							throw;
						}
						exceptionInInitializerError2 = exceptionInInitializerError;
						goto IL_99;
					}
					goto IL_11B;
					IL_91:
					ClassNotFoundException ex7 = ex2;
					java.lang.System.err.println(new StringBuilder().append("class not found !").append(ex7).toString());
					goto IL_11B;
					IL_95:
					ClassCastException ex8 = ex5;
					java.lang.System.err.println(new StringBuilder().append("can not cast class !").append(ex8).toString());
					goto IL_11B;
					IL_99:
					ExceptionInInitializerError exceptionInInitializerError3 = exceptionInInitializerError2;
					java.lang.System.err.println(new StringBuilder().append("couldn't load class !").append(exceptionInInitializerError3).toString());
				}
			}
			IL_11B:
			return (PropertySheet)this.symbolTable.get(instanceName);
		}

		[Throws(new string[]
		{
			"edu.cmu.sphinx.util.props.PropertyException"
		})]
		[LineNumberTable(new byte[]
		{
			159,
			186,
			113
		})]
		
		public ConfigurationManager(string configFileName) : this(ConfigurationManagerUtils.getURL(new File(configFileName)))
		{
		}

		
		[LineNumberTable(new byte[]
		{
			66,
			134,
			127,
			6,
			104,
			130,
			110,
			109,
			130
		})]
		
		public virtual Collection getInstanceNames(Class type)
		{
			ArrayList arrayList = new ArrayList();
			Iterator iterator = this.symbolTable.values().iterator();
			while (iterator.hasNext())
			{
				PropertySheet propertySheet = (PropertySheet)iterator.next();
				if (propertySheet.isInstanciated())
				{
					if (ConfigurationManagerUtils.isDerivedClass(propertySheet.getConfigurableClass(), type))
					{
						arrayList.add(propertySheet.getInstanceName());
					}
				}
			}
			return arrayList;
		}

		[LineNumberTable(new byte[]
		{
			161,
			43,
			149,
			109,
			159,
			0
		})]
		
		public virtual string getStrippedComponentName(string propertyName)
		{
			if (!ConfigurationManager.assertionsDisabled && propertyName == null)
			{
				
				throw new AssertionError();
			}
			while (java.lang.String.instancehelper_startsWith(propertyName, "$"))
			{
				propertyName = java.lang.String.instancehelper_toString((string)this.globalProperties.get(ConfigurationManagerUtils.stripGlobalSymbol(propertyName)));
			}
			return propertyName;
		}

		
		
		public virtual Logger getRootLogger()
		{
			return Logger.getLogger(ConfigurationManagerUtils.getLogPrefix(this));
		}

		
		[LineNumberTable(new byte[]
		{
			160,
			83,
			134,
			127,
			6,
			110,
			104,
			130
		})]
		
		public virtual List getPropSheets(Class confClass)
		{
			ArrayList arrayList = new ArrayList();
			Iterator iterator = this.symbolTable.values().iterator();
			while (iterator.hasNext())
			{
				PropertySheet propertySheet = (PropertySheet)iterator.next();
				if (ConfigurationManagerUtils.isDerivedClass(propertySheet.getConfigurableClass(), confClass))
				{
					arrayList.add(propertySheet);
				}
			}
			return arrayList;
		}

		
		[LineNumberTable(new byte[]
		{
			160,
			119,
			99,
			136,
			110,
			159,
			6,
			106,
			110,
			153,
			127,
			1,
			106
		})]
		
		public virtual void addConfigurable(Class confClass, string name, Map props)
		{
			if (name == null)
			{
				name = confClass.getName();
			}
			if (this.symbolTable.containsKey(name))
			{
				string text = new StringBuilder().append("tried to override existing component name : ").append(name).toString();
				
				throw new IllegalArgumentException(text);
			}
			PropertySheet propSheetInstanceFromClass = ConfigurationManager.getPropSheetInstanceFromClass(confClass, props, name, this);
			this.symbolTable.put(name, propSheetInstanceFromClass);
			this.rawPropertyMap.put(name, new RawPropertyData(name, confClass.getName()));
			Iterator iterator = this.changeListeners.iterator();
			while (iterator.hasNext())
			{
				ConfigurationChangeListener configurationChangeListener = (ConfigurationChangeListener)iterator.next();
				configurationChangeListener.componentAdded(this, propSheetInstanceFromClass);
			}
		}

		
		[LineNumberTable(new byte[]
		{
			161,
			206,
			141,
			127,
			1,
			135,
			104,
			140,
			115,
			130
		})]
		
		private static PropertySheet getPropSheetInstanceFromClass(Class @class, Map map, string name, ConfigurationManager cm)
		{
			RawPropertyData rawPropertyData = new RawPropertyData(name, @class.getName());
			Iterator iterator = map.entrySet().iterator();
			while (iterator.hasNext())
			{
				Map.Entry entry = (Map.Entry)iterator.next();
				object obj = entry.getValue();
				if (obj is Class)
				{
					obj = ((Class)obj).getName();
				}
				rawPropertyData.getProperties().put(entry.getKey(), obj);
			}
			return new PropertySheet(@class, name, cm, rawPropertyData);
		}

		[LineNumberTable(new byte[]
		{
			161,
			91,
			159,
			1,
			127,
			1,
			111,
			98
		})]
		
		internal virtual void fireRenamedConfigurable(string str, string text)
		{
			if (!ConfigurationManager.assertionsDisabled && !this.getComponentNames().contains(text))
			{
				
				throw new AssertionError();
			}
			Iterator iterator = this.changeListeners.iterator();
			while (iterator.hasNext())
			{
				ConfigurationChangeListener configurationChangeListener = (ConfigurationChangeListener)iterator.next();
				configurationChangeListener.componentRenamed(this, this.getPropertySheet(text), str);
			}
		}

		
		
		
		public virtual Set getComponentNames()
		{
			return this.rawPropertyMap.keySet();
		}

		[LineNumberTable(new byte[]
		{
			159,
			63,
			130,
			135,
			127,
			4,
			108,
			113,
			105,
			111,
			159,
			0,
			98,
			191,
			6,
			133,
			159,
			6,
			123,
			99,
			159,
			6,
			130,
			177,
			127,
			7,
			104,
			130,
			113,
			113
		})]
		
		public virtual void addSubConfiguration(ConfigurationManager subCM, bool doOverrideComponents)
		{
			Set componentNames = this.getComponentNames();
			Iterator iterator = subCM.getComponentNames().iterator();
			while (iterator.hasNext())
			{
				string text = (string)iterator.next();
				if (componentNames.contains(text))
				{
					if (!doOverrideComponents || this.getPropertySheet(text).isInstanciated())
					{
						string text2 = new StringBuilder().append(text).append(" is already registered to system configuration").toString();
						
						throw new RuntimeException(text2);
					}
					PropertySheet propertySheet = subCM.getPropertySheet(text);
					this.symbolTable.put(text, propertySheet);
					this.rawPropertyMap.put(text, new RawPropertyData(text, propertySheet.getConfigurableClass().getSimpleName()));
				}
			}
			iterator = subCM.globalProperties.keySet().iterator();
			while (iterator.hasNext())
			{
				string text = (string)iterator.next();
				if (this.globalProperties.containsKey(text) && !System.getProperties().containsKey(text) && !doOverrideComponents)
				{
					string text3 = new StringBuilder().append(text).append(" is already registered as global property").toString();
					
					throw new RuntimeException(text3);
				}
			}
			this.globalProperties.putAll(subCM.globalProperties);
			iterator = subCM.symbolTable.values().iterator();
			while (iterator.hasNext())
			{
				PropertySheet propertySheet2 = (PropertySheet)iterator.next();
				propertySheet2.setCM(this);
			}
			this.symbolTable.putAll(subCM.symbolTable);
			this.rawPropertyMap.putAll(subCM.rawPropertyMap);
		}

		
		
		
		public virtual Map getGlobalProperties()
		{
			return new HashMap(this.globalProperties);
		}

		[Throws(new string[]
		{
			"edu.cmu.sphinx.util.props.PropertyException"
		})]
		
		
		
		public static Configurable getInstance(Class targetClass, Map props)
		{
			return ConfigurationManager.getInstance(targetClass, props, null);
		}

		[Throws(new string[]
		{
			"edu.cmu.sphinx.util.props.PropertyException"
		})]
		
		[LineNumberTable(new byte[]
		{
			161,
			195,
			110,
			103
		})]
		
		public static Configurable getInstance(Class targetClass, Map props, string compName)
		{
			PropertySheet propSheetInstanceFromClass = ConfigurationManager.getPropSheetInstanceFromClass(targetClass, props, compName, new ConfigurationManager());
			Configurable owner = propSheetInstanceFromClass.getOwner();
			return (Configurable)targetClass.cast(owner);
		}

		[LineNumberTable(new byte[]
		{
			159,
			175,
			232,
			50,
			139,
			107,
			107,
			235,
			75
		})]
		
		public ConfigurationManager()
		{
			this.changeListeners = new ArrayList();
			this.symbolTable = new LinkedHashMap();
			this.rawPropertyMap = new HashMap();
			this.globalProperties = new HashMap();
		}

		[Throws(new string[]
		{
			"java.lang.CloneNotSupportedException"
		})]
		[LineNumberTable(new byte[]
		{
			161,
			137,
			140,
			107,
			107,
			127,
			6,
			127,
			3,
			130,
			113,
			177
		})]
		
		public virtual ConfigurationManager clone()
		{
			ConfigurationManager configurationManager = (ConfigurationManager)base.clone();
			configurationManager.changeListeners = new ArrayList();
			configurationManager.symbolTable = new LinkedHashMap();
			Iterator iterator = this.symbolTable.entrySet().iterator();
			while (iterator.hasNext())
			{
				Map.Entry entry = (Map.Entry)iterator.next();
				configurationManager.symbolTable.put(entry.getKey(), ((PropertySheet)entry.getValue()).clone());
			}
			configurationManager.globalProperties = new HashMap(this.globalProperties);
			configurationManager.rawPropertyMap = new HashMap(this.rawPropertyMap);
			return configurationManager;
		}

		
		[LineNumberTable(new byte[]
		{
			160,
			104,
			109
		})]
		
		public virtual void addConfigurable(Class confClass, string name)
		{
			this.addConfigurable(confClass, name, new HashMap());
		}

		[LineNumberTable(new byte[]
		{
			160,
			141,
			110,
			144,
			146,
			106,
			110,
			142,
			127,
			1,
			106
		})]
		
		public virtual void addConfigurable(Configurable configurable, string name)
		{
			if (this.symbolTable.containsKey(name))
			{
				string text = "tried to override existing component name";
				
				throw new IllegalArgumentException(text);
			}
			RawPropertyData rawPropertyData = new RawPropertyData(name, Object.instancehelper_getClass(configurable).getName());
			PropertySheet propertySheet = new PropertySheet(configurable, name, rawPropertyData, this);
			this.symbolTable.put(name, propertySheet);
			this.rawPropertyMap.put(name, rawPropertyData);
			Iterator iterator = this.changeListeners.iterator();
			while (iterator.hasNext())
			{
				ConfigurationChangeListener configurationChangeListener = (ConfigurationChangeListener)iterator.next();
				configurationChangeListener.componentAdded(this, propertySheet);
			}
		}

		[LineNumberTable(new byte[]
		{
			160,
			156,
			136,
			99,
			191,
			16,
			136,
			109,
			142,
			114,
			159,
			0,
			104
		})]
		
		public virtual void renameConfigurable(string oldName, string newName)
		{
			PropertySheet propertySheet = this.getPropertySheet(oldName);
			if (propertySheet == null)
			{
				string text = new StringBuilder().append("no configurable (to be renamed) named ").append(oldName).append(" is contained in the CM").toString();
				
				throw new RuntimeException(text);
			}
			ConfigurationManagerUtils.renameComponent(this, oldName, newName);
			this.symbolTable.remove(oldName);
			this.symbolTable.put(newName, propertySheet);
			RawPropertyData rawPropertyData = (RawPropertyData)this.rawPropertyMap.remove(oldName);
			this.rawPropertyMap.put(newName, new RawPropertyData(newName, rawPropertyData.getClassName(), rawPropertyData.getProperties()));
			this.fireRenamedConfigurable(oldName, newName);
		}

		[LineNumberTable(new byte[]
		{
			160,
			178,
			159,
			1,
			114,
			141,
			127,
			1,
			106
		})]
		
		public virtual void removeConfigurable(string name)
		{
			if (!ConfigurationManager.assertionsDisabled && !this.getComponentNames().contains(name))
			{
				
				throw new AssertionError();
			}
			PropertySheet ps = (PropertySheet)this.symbolTable.remove(name);
			this.rawPropertyMap.remove(name);
			Iterator iterator = this.changeListeners.iterator();
			while (iterator.hasNext())
			{
				ConfigurationChangeListener configurationChangeListener = (ConfigurationChangeListener)iterator.next();
				configurationChangeListener.componentRemoved(this, ps);
			}
		}

		[LineNumberTable(new byte[]
		{
			160,
			190,
			104
		})]
		
		public virtual void addSubConfiguration(ConfigurationManager subCM)
		{
			this.addSubConfiguration(subCM, false);
		}

		[LineNumberTable(new byte[]
		{
			160,
			253,
			114
		})]
		
		public virtual string getGlobalProperty(string propertyName)
		{
			string text = (string)this.globalProperties.get(propertyName);
			return (text == null) ? null : java.lang.String.instancehelper_toString(text);
		}

		
		
		public virtual string getGloPropReference(string propertyName)
		{
			return (string)this.globalProperties.get(propertyName);
		}

		public virtual URL getConfigURL()
		{
			return this.configURL;
		}

		[LineNumberTable(new byte[]
		{
			161,
			56,
			99,
			129,
			109
		})]
		
		public virtual void addConfigurationChangeListener(ConfigurationChangeListener l)
		{
			if (l == null)
			{
				return;
			}
			this.changeListeners.add(l);
		}

		[LineNumberTable(new byte[]
		{
			161,
			67,
			99,
			129,
			109
		})]
		
		public virtual void removeConfigurationChangeListener(ConfigurationChangeListener l)
		{
			if (l == null)
			{
				return;
			}
			this.changeListeners.remove(l);
		}

		[LineNumberTable(new byte[]
		{
			161,
			79,
			159,
			1,
			127,
			1,
			107
		})]
		
		internal virtual void fireConfChanged(string text, string str)
		{
			if (!ConfigurationManager.assertionsDisabled && !this.getComponentNames().contains(text))
			{
				
				throw new AssertionError();
			}
			Iterator iterator = this.changeListeners.iterator();
			while (iterator.hasNext())
			{
				ConfigurationChangeListener configurationChangeListener = (ConfigurationChangeListener)iterator.next();
				configurationChangeListener.configurationChanged(text, str, this);
			}
		}

		[LineNumberTable(new byte[]
		{
			161,
			105,
			104,
			130,
			135,
			103,
			110,
			162,
			123,
			105,
			137,
			107,
			98,
			162
		})]
		
		public override bool equals(object obj)
		{
			if (!(obj is ConfigurationManager))
			{
				return false;
			}
			ConfigurationManager configurationManager = (ConfigurationManager)obj;
			Set componentNames = this.getComponentNames();
			if (!componentNames.equals(configurationManager.getComponentNames()))
			{
				return false;
			}
			Iterator iterator = componentNames.iterator();
			while (iterator.hasNext())
			{
				string instanceName = (string)iterator.next();
				PropertySheet propertySheet = this.getPropertySheet(instanceName);
				PropertySheet propertySheet2 = configurationManager.getPropertySheet(instanceName);
				if (!propertySheet2.equals(propertySheet))
				{
					return false;
				}
			}
			return configurationManager.getGlobalProperties().equals(this.getGlobalProperties());
		}

		[LineNumberTable(new byte[]
		{
			161,
			129,
			119
		})]
		
		public override int hashCode()
		{
			if (!ConfigurationManager.assertionsDisabled)
			{
				object obj = "hashCode not designed";
				
				throw new AssertionError(obj);
			}
			return 1;
		}

		[Throws(new string[]
		{
			"edu.cmu.sphinx.util.props.PropertyException"
		})]
		
		
		
		public static Configurable getInstance(Class targetClass)
		{
			return ConfigurationManager.getInstance(targetClass, new HashMap());
		}

		[Throws(new string[]
		{
			"java.lang.CloneNotSupportedException"
		})]
		
		[EditorBrowsable(EditorBrowsableState.Never)]
		
		
		public virtual object <bridge>clone()
		{
			return this.clone();
		}

		
		static ConfigurationManager()
		{
		}

		private static CallerID __<GetCallerID>()
		{
			if (ConfigurationManager.__<callerID> == null)
			{
				ConfigurationManager.__<callerID> = new ConfigurationManager.__<CallerID>();
			}
			return ConfigurationManager.__<callerID>;
		}

		
		public static implicit operator Cloneable(ConfigurationManager _<ref>)
		{
			Cloneable result;
			result.__<ref> = _<ref>;
			return result;
		}

		
		private List changeListeners;

		
		private Map symbolTable;

		
		private Map rawPropertyMap;

		
		private Map globalProperties;

		private bool showCreations;

		private URL configURL;

		
		internal static bool assertionsDisabled = !ClassLiteral<ConfigurationManager>.Value.desiredAssertionStatus();

		private static CallerID __<callerID>;

		private sealed class __<CallerID> : CallerID
		{
			internal __<CallerID>()
			{
			}
		}
	}
}
