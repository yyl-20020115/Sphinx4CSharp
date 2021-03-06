﻿using ikvm.@internal;
using java.io;
using java.lang;
using java.net;
using java.nio.charset;
using java.util;
using java.util.logging;
using java.util.regex;

namespace edu.cmu.sphinx.util.props
{
	public sealed class ConfigurationManagerUtils : Object
	{
		public static URL resourceToURL(string location)
		{
			Pattern pattern = ConfigurationManagerUtils.jarPattern;
			object _ref = location;
			CharSequence charSequence = CharSequence.Cast(_ref);
			Matcher matcher = pattern.matcher(charSequence);
			if (matcher.matches())
			{
				string text = matcher.group(1);
				return ClassLiteral<ConfigurationManagerUtils>.Value.getResource(text);
			}
			if (String.instancehelper_indexOf(location, 58) == -1)
			{
				location = new StringBuilder().append("file:").append(location).toString();
			}
			return new URL(location);
		}

		public static void setProperty(ConfigurationManager cm, string propName, string propValue)
		{
			if (!ConfigurationManagerUtils.assertionsDisabled && propValue == null)
			{

				throw new AssertionError();
			}
			Map map = ConfigurationManagerUtils.listAllsPropNames(cm);
			Set componentNames = cm.getComponentNames();
			object _ref;
			CharSequence charSequence;
			if (!map.containsKey(propName))
			{
				string text = propName;
				_ref = "->";
				charSequence = CharSequence.Cast(_ref);
				if (!String.instancehelper_contains(text, charSequence) && !componentNames.contains(propName))
				{
					string text2 = new StringBuilder().append("No property or configurable '").append(propName).append("' in configuration '").append(cm.getConfigURL()).append("'!").toString();

					throw new RuntimeException(text2);
				}
			}
			if (componentNames.contains(propName))
			{
				try
				{
					Class confClass = Class.forName(propValue, ConfigurationManagerUtils.__GetCallerID()).asSubclass(ClassLiteral<Configurable>.Value);
					ConfigurationManagerUtils.setClass(cm.getPropertySheet(propName), confClass);
				}
				catch (ClassNotFoundException ex)
				{
					throw new RuntimeException(ex);

				}
			}
			string text3 = propName;
			_ref = "->";
			charSequence = CharSequence.Cast(_ref);
			if (!String.instancehelper_contains(text3, charSequence) && ((List)map.get(propName)).size() > 1)
			{
				string text4 = new StringBuilder().append("Property-name '").append(propName).append("' is ambiguous with respect to configuration '").append(cm.getConfigURL()).append("'. Use 'componentName->propName' to disambiguate your request.").toString();

				throw new RuntimeException(text4);
			}
			string text5 = propName;
			_ref = "->";
			charSequence = CharSequence.Cast(_ref);
			string componentName;
			if (String.instancehelper_contains(text5, charSequence))
			{
				string[] array = String.instancehelper_split(propName, "->");
				componentName = array[0];
				propName = array[1];
			}
			else
			{
				componentName = ((PropertySheet)((List)map.get(propName)).get(0)).getInstanceName();
			}
			ConfigurationManagerUtils.setProperty(cm, componentName, propName, propValue);
		}

		public static ConfigurationManager getPropertyManager(PropertySheet ps)
		{
			return ps.getPropertyManager();
		}

		public static void showConfig(ConfigurationManager cm)
		{
			java.lang.System.@out.println(" ============ config ============= ");
			Iterator iterator = cm.getInstanceNames(ClassLiteral<Configurable>.Value).iterator();
			while (iterator.hasNext())
			{
				string name = (string)iterator.next();
				ConfigurationManagerUtils.showConfig(cm, name);
			}
		}

		public static void save(ConfigurationManager cm, File cmLocation)
		{
			if (!String.instancehelper_endsWith(cmLocation.getName(), ".sxl"))
			{
				java.lang.System.err.println("WARNING: Serialized s4-configuration should have the suffix '.sxl'");
			}
			if (!ConfigurationManagerUtils.assertionsDisabled && cm == null)
			{

				throw new AssertionError();
			}
			try
			{
				PrintWriter printWriter = new PrintWriter(new OutputStreamWriter(new FileOutputStream(cmLocation), Charset.forName("UTF-8")));
				string text = ConfigurationManagerUtils.toXML(cm);
				printWriter.print(text);
				printWriter.flush();
				printWriter.close();
			}
			catch (FileNotFoundException ex)
			{
				Throwable.instancehelper_printStackTrace(ex);
			}
		}

		public static URL getResource(string name, PropertySheet ps)
		{
			string @string = ps.getString(name);
			if (@string == null)
			{
				string instanceName = ps.getInstanceName();
				string text = new StringBuilder().append("Required resource property '").append(name).append("' not set").toString();

				throw new InternalConfigurationException(instanceName, name, text);
			}
			URL result;
			try
			{
				URL url = ConfigurationManagerUtils.resourceToURL(@string);
				result = url ?? throw new InternalConfigurationException(ps.getInstanceName(), name, new StringBuilder().append("Can't locate ").append(@string).toString());
			}
			catch (MalformedURLException ex)
			{
				throw new InternalConfigurationException(ex, ps.getInstanceName(), name, new StringBuilder().append("Bad URL ").append(@string).append(Throwable.instancehelper_getMessage(ex)).toString());
			}
			return result;
		}

		public static void showConfig(ConfigurationManager cm, string name)
		{
			if (!cm.getComponentNames().contains(name))
			{
				java.lang.System.@out.println(new StringBuilder().append("No component: ").append(name).toString());
				return;
			}
			java.lang.System.@out.println(new StringBuilder().append(name).append(':').toString());
			PropertySheet propertySheet = cm.getPropertySheet(name);
			Iterator iterator = propertySheet.getRegisteredProperties().iterator();
			while (iterator.hasNext())
			{
				string text = (string)iterator.next();
				java.lang.System.@out.print(new StringBuilder().append("    ").append(text).append(" = ").toString());
				object raw = propertySheet.getRaw(text);
				if (raw is string)
				{
					java.lang.System.@out.println(raw);
				}
				else if (raw is List)
				{
					List list = (List)raw;
					Iterator iterator2 = list.iterator();
					while (iterator2.hasNext())
					{
						java.lang.System.@out.print(iterator2.next());
						if (iterator2.hasNext())
						{
							java.lang.System.@out.print(", ");
						}
					}
					java.lang.System.@out.println();
				}
				else
				{
					java.lang.System.@out.println("[DEFAULT]");
				}
			}
		}

		public static void editConfig(ConfigurationManager cm, string name)
		{
			PropertySheet propertySheet = cm.getPropertySheet(name);
			if (propertySheet == null)
			{
				java.lang.System.@out.println(new StringBuilder().append("No component: ").append(name).toString());
				return;
			}
			java.lang.System.@out.println(new StringBuilder().append(name).append(':').toString());
			Collection registeredProperties = propertySheet.getRegisteredProperties();
			BufferedReader bufferedReader = new BufferedReader(new InputStreamReader(java.lang.System.@in));
			Iterator iterator = registeredProperties.iterator();
			while (iterator.hasNext())
			{
				string text = (string)iterator.next();
				object raw;
				try
				{
					raw = propertySheet.getRaw(text);
					if (raw is List)
					{
						continue;
					}
				}
				catch (IOException)
				{
					goto IL_15D;
				}
				try
				{
					string text2;
					if (raw is string)
					{
						text2 = (string)raw;
					}
					else
					{
						text2 = "DEFAULT";
					}
					int num = 0;
					while (num == 0)
					{
						java.lang.System.@out.print(new StringBuilder().append("  ").append(text).append(" [").append(text2).append("]: ").toString());
						string text3 = bufferedReader.readLine();
						if (String.instancehelper_isEmpty(text3))
						{
							num = 1;
						}
						else
						{
							if (String.instancehelper_equals(text3, "."))
							{
								return;
							}
							cm.getPropertySheet(name).setRaw(text, text3);
							num = 1;
						}
					}
				}
				catch (IOException)
				{
					goto IL_158;
				}
				continue;
				IL_158:
				IL_15D:
				java.lang.System.@out.println("Trouble reading input");
				return;
			}
		}

		public static void setProperty(ConfigurationManager cm, string componentName, string propName, string propValue)
		{
			PropertySheet propertySheet = cm.getPropertySheet(componentName);
			if (propertySheet == null)
			{
				string text = new StringBuilder().append("Component '").append(propName).append("' is not registered to this system configuration '").toString();

				throw new RuntimeException(text);
			}
			if (String.instancehelper_equals(propValue, "null"))
			{
				propValue = null;
			}
			switch (ConfigurationManagerUtils_1._SwitchMap_edu_cmu_sphinx_util_props_PropertyType[propertySheet.getType(propName).ordinal()])
			{
				case 1:
					{
						ArrayList arrayList = new ArrayList();
						string[] array = String.instancehelper_split(propValue, ";");
						int num = array.Length;
						for (int i = 0; i < num; i++)
						{
							string text2 = array[i];
							arrayList.add(String.instancehelper_trim(text2));
						}
						propertySheet.setComponentList(propName, arrayList, null);
						break;
					}
				case 2:
					propertySheet.setComponent(propName, propValue, null);
					break;
				case 3:
					propertySheet.setBoolean(propName, Boolean.valueOf(Boolean.parseBoolean(propValue)));
					break;
				case 4:
					propertySheet.setDouble(propName, Double.parseDouble(propValue));
					break;
				case 5:
					propertySheet.setInt(propName, Integer.parseInt(propValue));
					break;
				case 6:
					propertySheet.setString(propName, propValue);
					break;
				default:
					{
						string text3 = "unknown property-type";

						throw new RuntimeException(text3);
					}
			}
		}

		public static void dumpPropStructure(ConfigurationManager cm)
		{
			Map map = ConfigurationManagerUtils.listAllsPropNames(cm);
			java.lang.System.@out.println(new StringBuilder().append("Property-structure of '").append(cm.getConfigURL()).append("':").toString());
			java.lang.System.@out.println("\nUnambiguous properties = ");
			Iterator iterator = map.entrySet().iterator();
			while (iterator.hasNext())
			{
				Map.Entry entry = (Map.Entry)iterator.next();
				if (((List)entry.getValue()).size() == 1)
				{
					java.lang.System.@out.print(new StringBuilder().append((string)entry.getKey()).append(", ").toString());
				}
			}
			java.lang.System.@out.println("\n\nAmbiguous properties: ");
			iterator = map.entrySet().iterator();
			while (iterator.hasNext())
			{
				Map.Entry entry = (Map.Entry)iterator.next();
				if (((List)entry.getValue()).size() != 1)
				{
					java.lang.System.@out.print(new StringBuilder().append((string)entry.getKey()).append('=').toString());
					Iterator iterator2 = ((List)entry.getValue()).iterator();
					while (iterator2.hasNext())
					{
						PropertySheet propertySheet = (PropertySheet)iterator2.next();
						java.lang.System.@out.print(new StringBuilder().append(propertySheet.getInstanceName()).append(", ").toString());
					}
					java.lang.System.@out.println();
				}
			}
		}

		public static URL getURL(File file)
		{
			URL result;
			try
			{
				result = file.toURI().toURL();
			}
			catch (MalformedURLException ex)
			{
				Throwable.instancehelper_printStackTrace(ex);
				return null;
			}
			return result;
		}

		internal static void applySystemProperties(Map map, Map map2)
		{
			Properties properties = java.lang.System.getProperties();
			Enumeration enumeration = properties.keys();
			while (enumeration.hasMoreElements())
			{
				string text = (string)enumeration.nextElement();
				string property = properties.getProperty(text);
				int num = String.instancehelper_indexOf(text, 91);
				int num2 = String.instancehelper_indexOf(text, 93);
				if (num > 0 && num2 > num)
				{
					string text2 = String.instancehelper_substring(text, 0, num);
					string propName = String.instancehelper_substring(text, num + 1, num2);
					RawPropertyData rawPropertyData = (RawPropertyData)map.get(text2);
					if (rawPropertyData == null)
					{
						string text3 = text2;
						string text4 = text;
						string text5 = new StringBuilder().append("System property attempting to set parameter  for unknown component ").append(text2).append(" (").append(text).append(')').toString();

						throw new InternalConfigurationException(text3, text4, text5);
					}
					rawPropertyData.add(propName, property);
				}
				else if (String.instancehelper_indexOf(text, 46) == -1)
				{
					map2.put(text, property);
				}
			}
		}

		public static void configureLogger(ConfigurationManager cm)
		{
			if (java.lang.System.getProperty("java.util.logging.config.class") != null
				|| java.lang.System.getProperty("java.util.logging.config.file") != null)
			{
				return;
			}
			string logPrefix = ConfigurationManagerUtils.getLogPrefix(cm);
			Logger logger = Logger.getLogger(String.instancehelper_substring(logPrefix, 0, String.instancehelper_length(logPrefix) - 1));
			Level level = Logger.getLogger("").getLevel();
			ConfigurationManagerUtils.configureLogger(logger);
			string text = cm.getGlobalProperty("logLevel");
			if (text == null)
			{
				text = Level.WARNING.getName();
			}
			logger.setLevel(Level.parse(text));
			Logger.getLogger("").setLevel(level);
		}

		public static bool isDerivedClass(Class derived, Class parent)
		{
			return parent.isAssignableFrom(derived);
		}

		internal static void renameComponent(ConfigurationManager configurationManager, string text, string text2)
		{
			if (!ConfigurationManagerUtils.assertionsDisabled && configurationManager == null)
			{

				throw new AssertionError();
			}
			if (!ConfigurationManagerUtils.assertionsDisabled && (text == null || text2 == null))
			{

				throw new AssertionError();
			}
			if (configurationManager.getPropertySheet(text) == null)
			{
				string text3 = new StringBuilder().append("no configurable (to be renamed) named ").append(text).append(" is contained in the CM").toString();

				throw new RuntimeException(text3);
			}
			Iterator iterator = configurationManager.getComponentNames().iterator();
			while (iterator.hasNext())
			{
				string instanceName = (string)iterator.next();
				PropertySheet propertySheet = configurationManager.getPropertySheet(instanceName);
				Iterator iterator2 = propertySheet.getRegisteredProperties().iterator();
				while (iterator2.hasNext())
				{
					string text4 = (string)iterator2.next();
					if (propertySheet.getRawNoReplacement(text4) != null)
					{
						int num = ConfigurationManagerUtils_1._SwitchMap_edu_cmu_sphinx_util_props_PropertyType[propertySheet.getType(text4).ordinal()];
						if (num == 1)
						{
							List list = ConfigurationManagerUtils.toStringList(propertySheet.getRawNoReplacement(text4));
							for (int i = 0; i < list.size(); i++)
							{
								string text5 = (string)list.get(i);
								if (String.instancehelper_equals(text5, text))
								{
									list.set(i, text2);
								}
							}
						}
						else if (num == 2)
						{
							if (Object.instancehelper_equals(propertySheet.getRawNoReplacement(text4), text))
							{
								propertySheet.setRaw(text4, text2);
							}
						}
					}
				}
			}
			PropertySheet propertySheet2 = configurationManager.getPropertySheet(text);
			propertySheet2.setInstanceName(text2);
			Iterator iterator3 = configurationManager.getGlobalProperties().entrySet().iterator();
			while (iterator3.hasNext())
			{
				Map.Entry entry = (Map.Entry)iterator3.next();
				if (String.instancehelper_equals((string)entry.getValue(), text))
				{
					configurationManager.setGlobalProperty((string)entry.getKey(), text2);
				}
			}
		}

		public static string stripGlobalSymbol(string symbol)
		{
			Pattern pattern = ConfigurationManagerUtils.globalSymbolPattern;
			CharSequence charSequence = CharSequence.Cast(symbol);
			Matcher matcher = pattern.matcher(charSequence);
			if (matcher.matches())
			{
				return matcher.group(1);
			}
			return symbol;
		}

		public static string getLogPrefix(ConfigurationManager cm)
		{
			if (cm.getConfigURL() != null)
			{
				StringBuilder stringBuilder = new StringBuilder();
				string name = new File(cm.getConfigURL().getFile()).getName();
				object obj = ".sxl";
				object obj2 = "";
				object obj3 = obj;
				CharSequence charSequence = CharSequence.Cast(obj3);
				CharSequence charSequence2 = charSequence;
				obj3 = obj2;
				charSequence = CharSequence.Cast(obj3);
				string text = String.instancehelper_replace(name, charSequence2, charSequence);
				object obj4 = ".xml";
				obj3 = "";
				obj2 = obj4;
				charSequence = CharSequence.Cast(obj2);
				CharSequence charSequence3 = charSequence;
				obj2 = obj3;
				charSequence = CharSequence.Cast(obj2);
				return stringBuilder.append(String.instancehelper_replace(text, charSequence3, charSequence)).append('.').toString();
			}
			return "S4CM.";
		}

		public static void configureLogger(Logger logger)
		{
			logger.setUseParentHandlers(false);
			int num = 0;
			Handler[] handlers = logger.getHandlers();
			int num2 = handlers.Length;
			for (int i = 0; i < num2; i++)
			{
				Handler handler = handlers[i];
				if (handler.getFormatter() is SphinxLogFormatter)
				{
					num = 1;
					break;
				}
			}
			if (num == 0)
			{
				ConsoleHandler consoleHandler = new ConsoleHandler();
				consoleHandler.setFormatter(new SphinxLogFormatter());
				logger.addHandler(consoleHandler);
			}
		}

		private static string propSheet2XML(string text, PropertySheet propertySheet)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.append("\t<component name=\"").append(text).append("\" type=\"").append(propertySheet.getConfigurableClass().getName()).append("\">");
			Iterator iterator = propertySheet.getRegisteredProperties().iterator();
			while (iterator.hasNext())
			{
				string text2 = (string)iterator.next();
				string text3 = new StringBuilder().append("\n\t\t<property name=\"").append(text2).append("\" ").toString();
				if (propertySheet.getRawNoReplacement(text2) != null)
				{
					if (ConfigurationManagerUtils_1._SwitchMap_edu_cmu_sphinx_util_props_PropertyType[propertySheet.getType(text2).ordinal()] == 1)
					{
						stringBuilder.append("\n\t\t<propertylist name=\"").append(text2).append("\">");
						List list = ConfigurationManagerUtils.toStringList(propertySheet.getRawNoReplacement(text2));
						Iterator iterator2 = list.iterator();
						while (iterator2.hasNext())
						{
							string text4 = (string)iterator2.next();
							stringBuilder.append("\n\t\t\t<item>").append(text4).append("</item>");
						}
						stringBuilder.append("\n\t\t</propertylist>");
					}
					else
					{
						stringBuilder.append(text3).append("value=\"").append(propertySheet.getRawNoReplacement(text2)).append("\"/>");
					}
				}
			}
			stringBuilder.append("\n\t</component>\n\n");
			return stringBuilder.toString();
		}

		public static List toStringList(object obj)
		{
			ArrayList arrayList = new ArrayList();
			if (!(obj is List))
			{
				return null;
			}
			Iterator iterator = ((List)obj).iterator();
			while (iterator.hasNext())
			{
				object obj2 = iterator.next();
				if (obj2 is string)
				{
					arrayList.add((string)obj2);
				}
			}
			return arrayList;
		}

		public static string toXML(ConfigurationManager cm)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.append("<?xml version=\"1.0\" encoding=\"UTF-8\" ?>\n");
			stringBuilder.append("\n<!--    Sphinx-4 Configuration file--> \n\n");
			stringBuilder.append("<config>");
			Pattern pattern = Pattern.compile("\\_\\{(\\w+)\\}");
			Map globalProperties = cm.getGlobalProperties();
			Iterator iterator = globalProperties.entrySet().iterator();
			while (iterator.hasNext())
			{
				Map.Entry entry = (Map.Entry)iterator.next();
				string text = (string)entry.getKey();
				Pattern pattern2 = pattern;
				object _ref = text;
				CharSequence charSequence = CharSequence.Cast(_ref);
				Matcher matcher = pattern2.matcher(charSequence);
				text = ((!matcher.matches()) ? text : matcher.group(1));
				stringBuilder.append("\n\t<property name=\"").append(text).append("\" value=\"").append((string)entry.getValue()).append("\"/>");
			}
			iterator = cm.getComponentNames().iterator();
			while (iterator.hasNext())
			{
				string text2 = (string)iterator.next();
				stringBuilder.append("\n\n").append(ConfigurationManagerUtils.propSheet2XML(text2, cm.getPropertySheet(text2)));
			}
			stringBuilder.append("\n</config>");
			return stringBuilder.toString();
		}

		public static bool isImplementingInterface(Class aClass, Class interfaceClass)
		{
			if (!ConfigurationManagerUtils.assertionsDisabled && !interfaceClass.isInterface())
			{

				throw new AssertionError();
			}
			Class superclass = aClass.getSuperclass();
			if (superclass != null && ConfigurationManagerUtils.isImplementingInterface(superclass, interfaceClass))
			{
				return true;
			}
			Class[] interfaces = aClass.getInterfaces();
			int num = interfaces.Length;
			for (int i = 0; i < num; i++)
			{
				Class @class = interfaces[i];
				if (Object.instancehelper_equals(@class, interfaceClass) || ConfigurationManagerUtils.isImplementingInterface(@class, interfaceClass))
				{
					return true;
				}
			}
			return false;
		}

		public static Map listAllsPropNames(ConfigurationManager cm)
		{
			HashMap hashMap = new HashMap();
			Iterator iterator = cm.getComponentNames().iterator();
			while (iterator.hasNext())
			{
				string instanceName = (string)iterator.next();
				PropertySheet propertySheet = cm.getPropertySheet(instanceName);
				Iterator iterator2 = propertySheet.getRegisteredProperties().iterator();

				while (iterator2.hasNext())
				{
					string text = (string)iterator2.next();
					if (!hashMap.containsKey(text))
					{
						hashMap.put(text, new ArrayList());
					}
					((List)hashMap.get(text)).add(propertySheet);
				}
			}
			return hashMap;
		}

		public static void setClass(PropertySheet ps, Class confClass)
		{
			if (ps.isInstanciated())
			{
				string text = new StringBuilder().append("configurable ").append(ps.getInstanceName()).append("has already been instantiated").toString();

				throw new RuntimeException(text);
			}
			ps.setConfigurableClass(confClass);
		}

		private ConfigurationManagerUtils()
		{
		}

		public bool validateConfiguration(ConfigurationManager cm)
		{
			Iterator iterator = cm.getComponentNames().iterator();
			while (iterator.hasNext())
			{
				string instanceName = (string)iterator.next();
				if (!cm.getPropertySheet(instanceName).validate())
				{
					return false;
				}
			}
			return true;
		}

		public static Map fixDuplicateNames(ConfigurationManager baseCM, ConfigurationManager subCM)
		{
			HashMap hashMap = new HashMap();
			Iterator iterator = subCM.getComponentNames().iterator();
			while (iterator.hasNext())
			{
				string text = (string)iterator.next();
				string text2 = text;
				int num = 0;
				while (baseCM.getComponentNames().contains(text2) || (subCM.getComponentNames().contains(text2) && !String.instancehelper_equals(text2, text)))
				{
					num++;
					text2 = new StringBuilder().append(text).append(num).toString();
				}
				subCM.renameConfigurable(text, text2);
				hashMap.put(text, text2);
			}
			return hashMap;
		}

		public static bool isSubClass(Class aClass, Class possibleSuperclass)
		{
			while (aClass != null && !Object.instancehelper_equals(aClass, ClassLiteral<Object>.Value))
			{
				aClass = aClass.getSuperclass();
				if (aClass != null && Object.instancehelper_equals(aClass, possibleSuperclass))
				{
					return true;
				}
			}
			return false;
		}

		public static Collection getNonInstaniatedComps(ConfigurationManager cm)
		{
			ArrayList arrayList = new ArrayList();
			Iterator iterator = cm.getComponentNames().iterator();
			while (iterator.hasNext())
			{
				string text = (string)iterator.next();
				if (!cm.getPropertySheet(text).isInstanciated())
				{
					arrayList.add(text);
				}
			}
			return arrayList;
		}

		static ConfigurationManagerUtils()
		{
			ConfigurationManagerUtils.globalSymbolPattern = Pattern.compile("\\_\\{(\\w+)\\}");
			ConfigurationManagerUtils.jarPattern = Pattern.compile("resource:(.*)", 2);
		}

		private static CallerID __GetCallerID()
		{
			if (ConfigurationManagerUtils.__callerID == null)
			{
				ConfigurationManagerUtils.__callerID = new ConfigurationManagerUtils.__CallerID();
			}
			return ConfigurationManagerUtils.__callerID;
		}

		private static Pattern globalSymbolPattern;

		public const string GLOBAL_COMMON_LOGLEVEL = "logLevel";

		public const string CM_FILE_SUFFIX = ".sxl";

		internal static Pattern jarPattern;

		internal static bool assertionsDisabled = !ClassLiteral<ConfigurationManagerUtils>.Value.desiredAssertionStatus();

		private static CallerID __callerID;

		private sealed class __CallerID : CallerID
		{
			internal __CallerID()
			{
			}
		}
	}
}
