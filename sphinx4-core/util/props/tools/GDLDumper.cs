using System;

using IKVM.Attributes;
using ikvm.@internal;
using java.io;
using java.lang;
using java.util;

namespace edu.cmu.sphinx.util.props.tools
{
	public class GDLDumper : java.lang.Object
	{
		[Throws(new string[]
		{
			"java.io.IOException"
		})]
		[LineNumberTable(new byte[]
		{
			12,
			108,
			102,
			127,
			6,
			104,
			98,
			102,
			102
		})]
		
		public static void showConfigAsGDL(ConfigurationManager ConfigurationManager, string path)
		{
			PrintStream printStream = new PrintStream(new FileOutputStream(path));
			GDLDumper.dumpGDLHeader(printStream);
			Iterator iterator = ConfigurationManager.getInstanceNames(ClassLiteral<Configurable>.Value).iterator();
			while (iterator.hasNext())
			{
				string name = (string)iterator.next();
				GDLDumper.dumpComponentAsGDL(ConfigurationManager, printStream, name);
			}
			GDLDumper.dumpGDLFooter(printStream);
			printStream.close();
		}

		[LineNumberTable(new byte[]
		{
			52,
			104,
			103,
			115,
			107,
			115,
			107,
			115,
			107,
			115,
			107,
			115,
			107,
			115,
			104,
			115,
			104,
			115,
			223,
			7,
			2,
			97,
			134
		})]
		
		public static string getColor(ConfigurationManager ConfigurationManager, string componentName)
		{
			string result;
			try
			{
				Configurable configurable = ConfigurationManager.lookup(componentName);
				Class @class = java.lang.Object.instancehelper_getClass(configurable);
				if (java.lang.String.instancehelper_indexOf(@class.getName(), ".recognizer") > 1)
				{
					result = "cyan";
				}
				else
				{
					if (java.lang.String.instancehelper_indexOf(@class.getName(), ".tools") > 1)
					{
						return "darkcyan";
					}
					if (java.lang.String.instancehelper_indexOf(@class.getName(), ".decoder") > 1)
					{
						return "green";
					}
					if (java.lang.String.instancehelper_indexOf(@class.getName(), ".frontend") > 1)
					{
						return "orange";
					}
					if (java.lang.String.instancehelper_indexOf(@class.getName(), ".acoustic") > 1)
					{
						return "turquoise";
					}
					if (java.lang.String.instancehelper_indexOf(@class.getName(), ".linguist") > 1)
					{
						return "lightblue";
					}
					if (java.lang.String.instancehelper_indexOf(@class.getName(), ".instrumentation") > 1)
					{
						return "lightgrey";
					}
					if (java.lang.String.instancehelper_indexOf(@class.getName(), ".util") > 1)
					{
						return "lightgrey";
					}
					goto IL_10B;
				}
			}
			catch (PropertyException ex)
			{
				goto IL_110;
			}
			return result;
			IL_10B:
			return "darkgrey";
			IL_110:
			return "black";
		}

		[LineNumberTable(new byte[]
		{
			28,
			107,
			107,
			107,
			107,
			107,
			107,
			107,
			107,
			107,
			107,
			107,
			107
		})]
		
		public static void dumpGDLHeader(PrintStream @out)
		{
			@out.println(" graph: {title: \"unix evolution\" ");
			@out.println("         layoutalgorithm: tree");
			@out.println("          scaling        : 2.0");
			@out.println("          colorentry 42  : 152 222 255");
			@out.println("     node.shape     : ellipse");
			@out.println("      node.color     : 42 ");
			@out.println("node.height    : 32  ");
			@out.println("node.fontname  : \"helvB08\"");
			@out.println("edge.color     : darkred");
			@out.println("edge.arrowsize :  6    ");
			@out.println("node.textcolor : darkblue ");
			@out.println("splines        : yes");
		}

		[LineNumberTable(new byte[]
		{
			159,
			170,
			191,
			30,
			104,
			135,
			126,
			105,
			137,
			103,
			105,
			159,
			33,
			105,
			105,
			123,
			159,
			28,
			162,
			101
		})]
		
		public static void dumpComponentAsGDL(ConfigurationManager cm, PrintStream @out, string name)
		{
			@out.println(new StringBuilder().append("node: {title: \"").append(name).append("\" color: ").append(GDLDumper.getColor(cm, name)).append('}').toString());
			PropertySheet propertySheet = cm.getPropertySheet(name);
			Collection registeredProperties = propertySheet.getRegisteredProperties();
			Iterator iterator = registeredProperties.iterator();
			while (iterator.hasNext())
			{
				string text = (string)iterator.next();
				PropertyType type = propertySheet.getType(text);
				object raw = propertySheet.getRaw(text);
				if (raw != null)
				{
					if (type == PropertyType.__COMPONENT)
					{
						@out.println(new StringBuilder().append("edge: {source: \"").append(name).append("\" target: \"").append(raw).append("\"}").toString());
					}
					else if (type == PropertyType.__COMPONENT_LIST)
					{
						List list = (List)raw;
						Iterator iterator2 = list.iterator();
						while (iterator2.hasNext())
						{
							object obj = iterator2.next();
							@out.println(new StringBuilder().append("edge: {source: \"").append(name).append("\" target: \"").append(obj).append("\"}").toString());
						}
					}
				}
			}
		}

		[LineNumberTable(new byte[]
		{
			84,
			107
		})]
		
		public static void dumpGDLFooter(PrintStream @out)
		{
			@out.println("}");
		}

		
		
		public GDLDumper()
		{
		}
	}
}
