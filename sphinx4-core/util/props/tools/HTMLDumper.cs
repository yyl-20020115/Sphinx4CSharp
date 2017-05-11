using ikvm.@internal;
using java.io;
using java.lang;
using java.util;

namespace edu.cmu.sphinx.util.props.tools
{
	public class HTMLDumper : java.lang.Object
	{		
		public static void showConfigAsHTML(ConfigurationManager ConfigurationManager, string path)
		{
			PrintStream printStream = new PrintStream(new FileOutputStream(path));
			HTMLDumper.dumpHeader(printStream);
			Iterator iterator = ConfigurationManager.getInstanceNames(ClassLiteral<Configurable>.Value).iterator();
			while (iterator.hasNext())
			{
				string text = (string)iterator.next();
				HTMLDumper.dumpComponentAsHTML(printStream, text, ConfigurationManager.getPropertySheet(text));
			}
			HTMLDumper.dumpFooter(printStream);
			printStream.close();
		}
		
		public static void dumpHeader(PrintStream @out)
		{
			@out.println("<html><head>");
			@out.println("    <title> Sphinx-4 Configuration</title");
			@out.println("</head>");
			@out.println("<body>");
		}
		
		public static void dumpComponentAsHTML(PrintStream @out, string name, PropertySheet properties)
		{
			@out.println("<table border=1>");
			@out.print("    <tr><th bgcolor=\"#CCCCFF\" colspan=2>");
			@out.print(name);
			@out.print("</a>");
			@out.println("</td></tr>");
			@out.println("    <tr><th bgcolor=\"#CCCCFF\">Property</th><th bgcolor=\"#CCCCFF\"> Value</th></tr>");
			Collection registeredProperties = properties.getRegisteredProperties();
			Iterator iterator = registeredProperties.iterator();
			while (iterator.hasNext())
			{
				string text = (string)iterator.next();
				@out.print(new StringBuilder().append("    <tr><th align=\"leftt\">").append(text).append("</th>").toString());
				object raw = properties.getRaw(text);
				if (raw is string)
				{
					@out.println(new StringBuilder().append("<td>").append(raw).append("</td></tr>").toString());
				}
				else if (raw is List)
				{
					List list = (List)raw;
					@out.println("    <td><ul>");
					Iterator iterator2 = list.iterator();
					while (iterator2.hasNext())
					{
						object obj = iterator2.next();
						@out.println(new StringBuilder().append("        <li>").append(obj).append("</li>").toString());
					}
					@out.println("    </ul></td>");
				}
				else
				{
					@out.println("<td>DEFAULT</td></tr>");
				}
			}
			@out.println("</table><br>");
		}
		
		public static void dumpFooter(PrintStream @out)
		{
			@out.println("</body>");
			@out.println("</html>");
		}
		
		public HTMLDumper()
		{
		}
	}
}
