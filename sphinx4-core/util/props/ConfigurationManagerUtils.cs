using System;

using IKVM.Attributes;
using ikvm.@internal;
using IKVM.Runtime;
using java.io;
using java.lang;
using java.net;
using java.nio.charset;
using java.util;
using java.util.logging;
using java.util.regex;

namespace edu.cmu.sphinx.util.props
{
	public sealed class ConfigurationManagerUtils : java.lang.Object
	{
		
		public static void __<clinit>()
		{
		}

		[Throws(new string[]
		{
			"java.net.MalformedURLException"
		})]
		[LineNumberTable(new byte[]
		{
			161,
			110,
			124,
			104,
			104,
			140,
			107,
			156
		})]
		
		public static URL resourceToURL(string location)
		{
			Pattern pattern = ConfigurationManagerUtils.jarPattern;
			object _<ref> = location;
			CharSequence charSequence;
			charSequence.__<ref> = _<ref>;
			Matcher matcher = pattern.matcher(charSequence);
			if (matcher.matches())
			{
				string text = matcher.group(1);
				return ClassLiteral<ConfigurationManagerUtils>.Value.getResource(text);
			}
			if (java.lang.String.instancehelper_indexOf(location, 58) == -1)
			{
				location = new StringBuilder().append("file:").append(location).toString();
			}
			return new URL(location);
		}

		[LineNumberTable(new byte[]
		{
			161,
			238,
			149,
			103,
			135,
			127,
			19,
			191,
			37,
			137,
			119,
			191,
			3,
			2,
			98,
			173,
			161,
			127,
			18,
			127,
			1,
			255,
			5,
			70,
			125,
			109,
			102,
			102,
			98,
			190,
			106
		})]
		
		public static void setProperty(ConfigurationManager cm, string propName, string propValue)
		{
			if (!ConfigurationManagerUtils.assertionsDisabled && propValue == null)
			{
				
				throw new AssertionError();
			}
			Map map = ConfigurationManagerUtils.listAllsPropNames(cm);
			Set componentNames = cm.getComponentNames();
			object _<ref>;
			CharSequence charSequence;
			if (!map.containsKey(propName))
			{
				string text = propName;
				_<ref> = "->";
				charSequence.__<ref> = _<ref>;
				if (!java.lang.String.instancehelper_contains(text, charSequence) && !componentNames.contains(propName))
				{
					string text2 = new StringBuilder().append("No property or configurable '").append(propName).append("' in configuration '").append(cm.getConfigURL()).append("'!").toString();
					
					throw new RuntimeException(text2);
				}
			}
			if (componentNames.contains(propName))
			{
				ClassNotFoundException ex2;
				try
				{
					Class confClass = Class.forName(propValue, ConfigurationManagerUtils.__<GetCallerID>()).asSubclass(ClassLiteral<Configurable>.Value);
					ConfigurationManagerUtils.setClass(cm.getPropertySheet(propName), confClass);
				}
				catch (ClassNotFoundException ex)
				{
					ex2 = ByteCodeHelper.MapException<ClassNotFoundException>(ex, 1);
					goto IL_D7;
				}
				return;
				IL_D7:
				ClassNotFoundException ex3 = ex2;
				Exception ex4 = ex3;
				
				throw new RuntimeException(ex4);
			}
			string text3 = propName;
			_<ref> = "->";
			charSequence.__<ref> = _<ref>;
			if (!java.lang.String.instancehelper_contains(text3, charSequence) && ((List)map.get(propName)).size() > 1)
			{
				string text4 = new StringBuilder().append("Property-name '").append(propName).append("' is ambiguous with respect to configuration '").append(cm.getConfigURL()).append("'. Use 'componentName->propName' to disambiguate your request.").toString();
				
				throw new RuntimeException(text4);
			}
			string text5 = propName;
			_<ref> = "->";
			charSequence.__<ref> = _<ref>;
			string componentName;
			if (java.lang.String.instancehelper_contains(text5, charSequence))
			{
				string[] array = java.lang.String.instancehelper_split(propName, "->");
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

		[LineNumberTable(new byte[]
		{
			160,
			190,
			111,
			127,
			6,
			103,
			98
		})]
		
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

		[LineNumberTable(new byte[]
		{
			160,
			169,
			114,
			143,
			149,
			123,
			103,
			103,
			102,
			184,
			2,
			97,
			134
		})]
		
		public static void save(ConfigurationManager cm, File cmLocation)
		{
			if (!java.lang.String.instancehelper_endsWith(cmLocation.getName(), ".sxl"))
			{
				java.lang.System.err.println("WARNING: Serialized s4-configuration should have the suffix '.sxl'");
			}
			if (!ConfigurationManagerUtils.assertionsDisabled && cm == null)
			{
				
				throw new AssertionError();
			}
			FileNotFoundException ex2;
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
				ex2 = ByteCodeHelper.MapException<FileNotFoundException>(ex, 1);
				goto IL_7A;
			}
			return;
			IL_7A:
			FileNotFoundException ex3 = ex2;
			Throwable.instancehelper_printStackTrace(ex3);
		}

		[Throws(new string[]
		{
			"edu.cmu.sphinx.util.props.PropertyException"
		})]
		[LineNumberTable(new byte[]
		{
			161,
			89,
			104,
			99,
			223,
			23,
			135,
			99,
			159,
			13,
			119,
			98
		})]
		
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
			MalformedURLException ex2;
			try
			{
				URL url = ConfigurationManagerUtils.resourceToURL(@string);
				if (url == null)
				{
					string instanceName2 = ps.getInstanceName();
					string text2 = new StringBuilder().append("Can't locate ").append(@string).toString();
					
					throw new InternalConfigurationException(instanceName2, name, text2);
				}
				result = url;
			}
			catch (MalformedURLException ex)
			{
				ex2 = ByteCodeHelper.MapException<MalformedURLException>(ex, 1);
				goto IL_8A;
			}
			return result;
			IL_8A:
			MalformedURLException ex3 = ex2;
			Exception ex4 = ex3;
			string instanceName3 = ps.getInstanceName();
			string text3 = new StringBuilder().append("Bad URL ").append(@string).append(Throwable.instancehelper_getMessage(ex3)).toString();
			
			throw new InternalConfigurationException(ex4, instanceName3, name, text3);
		}

		[LineNumberTable(new byte[]
		{
			160,
			206,
			110,
			127,
			5,
			129,
			159,
			2,
			136,
			127,
			4,
			159,
			15,
			104,
			104,
			112,
			104,
			104,
			114,
			113,
			105,
			177,
			106,
			98,
			143,
			101
		})]
		
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

		[LineNumberTable(new byte[]
		{
			25,
			168,
			99,
			127,
			5,
			129,
			159,
			2,
			103,
			144,
			159,
			0,
			170,
			123,
			98,
			105,
			139,
			135,
			131,
			103,
			127,
			33,
			104,
			105,
			101,
			110,
			162,
			112,
			131,
			207,
			226,
			61,
			97,
			111,
			129,
			101
		})]
		
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
			BufferedReader bufferedReader = new BufferedReader(new InputStreamReader(System.@in));
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
				catch (IOException ex)
				{
					goto IL_A6;
				}
				goto IL_A2;
				continue;
				IL_A6:
				goto IL_15D;
				IL_A2:
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
						if (java.lang.String.instancehelper_isEmpty(text3))
						{
							num = 1;
						}
						else
						{
							if (java.lang.String.instancehelper_equals(text3, "."))
							{
								return;
							}
							cm.getPropertySheet(name).setRaw(text, text3);
							num = 1;
						}
					}
				}
				catch (IOException ex2)
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

		[LineNumberTable(new byte[]
		{
			162,
			24,
			104,
			99,
			191,
			16,
			109,
			131,
			159,
			23,
			114,
			133,
			109,
			133,
			109,
			133,
			104,
			133,
			105,
			133,
			102,
			125,
			46,
			200,
			105,
			130,
			144
		})]
		
		public static void setProperty(ConfigurationManager cm, string componentName, string propName, string propValue)
		{
			PropertySheet propertySheet = cm.getPropertySheet(componentName);
			if (propertySheet == null)
			{
				string text = new StringBuilder().append("Component '").append(propName).append("' is not registered to this system configuration '").toString();
				
				throw new RuntimeException(text);
			}
			if (java.lang.String.instancehelper_equals(propValue, "null"))
			{
				propValue = null;
			}
			switch (ConfigurationManagerUtils$1.$SwitchMap$edu$cmu$sphinx$util$props$PropertyType[propertySheet.getType(propName).ordinal()])
			{
			case 1:
			{
				ArrayList arrayList = new ArrayList();
				string[] array = java.lang.String.instancehelper_split(propValue, ";");
				int num = array.Length;
				for (int i = 0; i < num; i++)
				{
					string text2 = array[i];
					arrayList.add(java.lang.String.instancehelper_trim(text2));
				}
				propertySheet.setComponentList(propName, arrayList, null);
				break;
			}
			case 2:
				propertySheet.setComponent(propName, propValue, null);
				break;
			case 3:
				propertySheet.setBoolean(propName, java.lang.Boolean.valueOf(java.lang.Boolean.parseBoolean(propValue)));
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

		[LineNumberTable(new byte[]
		{
			161,
			199,
			135,
			191,
			20,
			111,
			127,
			1,
			115,
			127,
			15,
			162,
			111,
			127,
			4,
			115,
			130,
			127,
			12,
			127,
			7,
			127,
			11,
			98,
			106,
			101
		})]
		
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

		[LineNumberTable(new byte[]
		{
			162,
			63,
			127,
			2,
			97,
			166
		})]
		
		public static URL getURL(File file)
		{
			URL result;
			MalformedURLException ex2;
			try
			{
				result = file.toURI().toURL();
			}
			catch (MalformedURLException ex)
			{
				ex2 = ByteCodeHelper.MapException<MalformedURLException>(ex, 1);
				goto IL_1D;
			}
			return result;
			IL_1D:
			MalformedURLException ex3 = ex2;
			Throwable.instancehelper_printStackTrace(ex3);
			return null;
		}

		[Throws(new string[]
		{
			"edu.cmu.sphinx.util.props.PropertyException"
		})]
		
		[LineNumberTable(new byte[]
		{
			160,
			248,
			102,
			114,
			108,
			200,
			106,
			138,
			113,
			107,
			110,
			111,
			100,
			140,
			255,
			33,
			69,
			226,
			69,
			107,
			137,
			101
		})]
		
		internal static void applySystemProperties(Map map, Map map2)
		{
			Properties properties = java.lang.System.getProperties();
			Enumeration enumeration = properties.keys();
			while (enumeration.hasMoreElements())
			{
				string text = (string)enumeration.nextElement();
				string property = properties.getProperty(text);
				int num = java.lang.String.instancehelper_indexOf(text, 91);
				int num2 = java.lang.String.instancehelper_indexOf(text, 93);
				if (num > 0 && num2 > num)
				{
					string text2 = java.lang.String.instancehelper_substring(text, 0, num);
					string propName = java.lang.String.instancehelper_substring(text, num + 1, num2);
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
				else if (java.lang.String.instancehelper_indexOf(text, 46) == -1)
				{
					map2.put(text, property);
				}
			}
		}

		[LineNumberTable(new byte[]
		{
			87,
			113,
			103,
			193,
			103,
			181,
			144,
			134,
			108,
			99,
			139,
			172,
			112
		})]
		
		public static void configureLogger(ConfigurationManager cm)
		{
			if (System.getProperty("java.util.logging.config.class") != null || java.lang.System.getProperty("java.util.logging.config.file") != null)
			{
				return;
			}
			string logPrefix = ConfigurationManagerUtils.getLogPrefix(cm);
			Logger logger = Logger.getLogger(java.lang.String.instancehelper_substring(logPrefix, 0, java.lang.String.instancehelper_length(logPrefix) - 1));
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

		[LineNumberTable(new byte[]
		{
			161,
			32,
			117,
			120,
			105,
			223,
			16,
			127,
			4,
			136,
			127,
			5,
			106,
			130,
			191,
			8,
			111,
			110,
			112,
			106,
			235,
			61,
			232,
			71,
			130,
			112,
			233,
			69,
			101,
			133,
			105,
			168,
			127,
			10,
			116,
			115,
			98
		})]
		
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
						int num = ConfigurationManagerUtils$1.$SwitchMap$edu$cmu$sphinx$util$props$PropertyType[propertySheet.getType(text4).ordinal()];
						if (num == 1)
						{
							List list = ConfigurationManagerUtils.toStringList(propertySheet.getRawNoReplacement(text4));
							for (int i = 0; i < list.size(); i++)
							{
								string text5 = (string)list.get(i);
								if (java.lang.String.instancehelper_equals(text5, text))
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
				if (java.lang.String.instancehelper_equals((string)entry.getValue(), text))
				{
					configurationManager.setGlobalProperty((string)entry.getKey(), text2);
				}
			}
		}

		[LineNumberTable(new byte[]
		{
			15,
			124,
			104,
			136
		})]
		
		public static string stripGlobalSymbol(string symbol)
		{
			Pattern pattern = ConfigurationManagerUtils.globalSymbolPattern;
			CharSequence charSequence;
			charSequence.__<ref> = symbol;
			Matcher matcher = pattern.matcher(charSequence);
			if (matcher.matches())
			{
				return matcher.group(1);
			}
			return symbol;
		}

		[LineNumberTable(new byte[]
		{
			74,
			107,
			159,
			116
		})]
		
		public static string getLogPrefix(ConfigurationManager cm)
		{
			if (cm.getConfigURL() != null)
			{
				StringBuilder stringBuilder = new StringBuilder();
				File.__<clinit>();
				string name = new File(cm.getConfigURL().getFile()).getName();
				object obj = ".sxl";
				object obj2 = "";
				object obj3 = obj;
				CharSequence charSequence;
				charSequence.__<ref> = obj3;
				CharSequence charSequence2 = charSequence;
				obj3 = obj2;
				charSequence.__<ref> = obj3;
				string text = java.lang.String.instancehelper_replace(name, charSequence2, charSequence);
				object obj4 = ".xml";
				obj3 = "";
				obj2 = obj4;
				charSequence.__<ref> = obj2;
				CharSequence charSequence3 = charSequence;
				obj2 = obj3;
				charSequence.__<ref> = obj2;
				return stringBuilder.append(java.lang.String.instancehelper_replace(text, charSequence3, charSequence)).append('.').toString();
			}
			return "S4CM.";
		}

		[LineNumberTable(new byte[]
		{
			118,
			135,
			130,
			117,
			110,
			98,
			226,
			61,
			230,
			71,
			99,
			103,
			108,
			136
		})]
		
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

		[LineNumberTable(new byte[]
		{
			160,
			141,
			102,
			159,
			23,
			127,
			4,
			127,
			6,
			105,
			130,
			191,
			0,
			124,
			110,
			127,
			1,
			127,
			0,
			108,
			130,
			159,
			9,
			133,
			108
		})]
		
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
					if (ConfigurationManagerUtils$1.$SwitchMap$edu$cmu$sphinx$util$props$PropertyType[propertySheet.getType(text2).ordinal()] == 1)
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

		
		[LineNumberTable(new byte[]
		{
			162,
			98,
			102,
			104,
			98,
			123,
			104,
			141,
			98
		})]
		
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

		[LineNumberTable(new byte[]
		{
			160,
			114,
			102,
			108,
			140,
			140,
			139,
			103,
			127,
			5,
			142,
			124,
			151,
			127,
			25,
			133,
			127,
			2,
			159,
			3,
			108
		})]
		
		public static string toXML(ConfigurationManager cm)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.append("<?xml version=\"1.0\" encoding=\"UTF-8\" ?>\n");
			stringBuilder.append("\n<!--    Sphinx-4 Configuration file--> \n\n");
			stringBuilder.append("<config>");
			Pattern pattern = Pattern.compile("\\$\\{(\\w+)\\}");
			Map globalProperties = cm.getGlobalProperties();
			Iterator iterator = globalProperties.entrySet().iterator();
			while (iterator.hasNext())
			{
				Map.Entry entry = (Map.Entry)iterator.next();
				string text = (string)entry.getKey();
				Pattern pattern2 = pattern;
				object _<ref> = text;
				CharSequence charSequence;
				charSequence.__<ref> = _<ref>;
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

		
		[LineNumberTable(new byte[]
		{
			161,
			135,
			154,
			103,
			108,
			130,
			117,
			116,
			2,
			230,
			69
		})]
		
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

		
		[LineNumberTable(new byte[]
		{
			161,
			181,
			134,
			127,
			4,
			136,
			127,
			5,
			106,
			142,
			116,
			98,
			133
		})]
		
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

		
		[LineNumberTable(new byte[]
		{
			162,
			90,
			104,
			159,
			21,
			103
		})]
		
		public static void setClass(PropertySheet ps, Class confClass)
		{
			if (ps.isInstanciated())
			{
				string text = new StringBuilder().append("configurable ").append(ps.getInstanceName()).append("has already been instantiated").toString();
				
				throw new RuntimeException(text);
			}
			ps.setConfigurableClass(confClass);
		}

		[LineNumberTable(new byte[]
		{
			159,
			179,
			102
		})]
		
		private ConfigurationManagerUtils()
		{
		}

		[LineNumberTable(new byte[]
		{
			159,
			191,
			127,
			1,
			110,
			98,
			130
		})]
		
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

		
		[LineNumberTable(new byte[]
		{
			160,
			84,
			134,
			127,
			4,
			130,
			131,
			111,
			150,
			102,
			186,
			104,
			105,
			133
		})]
		
		public static Map fixDuplicateNames(ConfigurationManager baseCM, ConfigurationManager subCM)
		{
			HashMap hashMap = new HashMap();
			Iterator iterator = subCM.getComponentNames().iterator();
			while (iterator.hasNext())
			{
				string text = (string)iterator.next();
				string text2 = text;
				int num = 0;
				while (baseCM.getComponentNames().contains(text2) || (subCM.getComponentNames().contains(text2) && !java.lang.String.instancehelper_equals(text2, text)))
				{
					num++;
					text2 = new StringBuilder().append(text).append(num).toString();
				}
				subCM.renameConfigurable(text, text2);
				hashMap.put(text, text2);
			}
			return hashMap;
		}

		
		[LineNumberTable(new byte[]
		{
			161,
			151,
			112,
			136,
			108,
			162
		})]
		
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

		
		[LineNumberTable(new byte[]
		{
			162,
			79,
			134,
			127,
			1,
			110,
			104,
			98
		})]
		
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

		[LineNumberTable(new byte[]
		{
			159,
			161,
			181,
			239,
			161,
			199
		})]
		static ConfigurationManagerUtils()
		{
			ConfigurationManagerUtils.globalSymbolPattern = Pattern.compile("\\$\\{(\\w+)\\}");
			ConfigurationManagerUtils.jarPattern = Pattern.compile("resource:(.*)", 2);
		}

		private static CallerID __<GetCallerID>()
		{
			if (ConfigurationManagerUtils.__<callerID> == null)
			{
				ConfigurationManagerUtils.__<callerID> = new ConfigurationManagerUtils.__<CallerID>();
			}
			return ConfigurationManagerUtils.__<callerID>;
		}

		
		private static Pattern globalSymbolPattern;

		public const string GLOBAL_COMMON_LOGLEVEL = "logLevel";

		public const string CM_FILE_SUFFIX = ".sxl";

		
		internal static Pattern jarPattern;

		
		internal static bool assertionsDisabled = !ClassLiteral<ConfigurationManagerUtils>.Value.desiredAssertionStatus();

		private static CallerID __<callerID>;

		private sealed class __<CallerID> : CallerID
		{
			internal __<CallerID>()
			{
			}
		}
	}
}
