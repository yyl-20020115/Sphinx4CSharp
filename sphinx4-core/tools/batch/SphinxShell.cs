using edu.cmu.sphinx.util.props;
using ikvm.@internal;
using java.io;
using java.lang;

namespace edu.cmu.sphinx.tools.batch
{
	public class SphinxShell : Object
	{
		public SphinxShell()
		{
		}

		public static void main(string[] args)
		{
			if (args.Length == 0 || (args.Length == 1 && (String.instancehelper_startsWith(args[0], "-h") || String.instancehelper_startsWith(args[0], "--h"))))
			{
				java.lang.System.@out.println("Usage: SphinxShell <config-xml-file> *([[<component>->]<parameter>=<value>] )");
				java.lang.System.@out.println("Example: SphinxShell foobar.xml beamWidth=123 phoneDecoder->autoAllocate=true");
				java.lang.System.@out.println("\nOther options are: ");
				java.lang.System.@out.println(" -h : Prints this help message");
				java.lang.System.@out.println(" -l <config-xml-file> : Prints a list of all component properties");
				java.lang.System.exit(-1);
			}
			if (args.Length == 2 && String.instancehelper_equals(args[0], "-l"))
			{
				ConfigurationManagerUtils.dumpPropStructure(new ConfigurationManager(new File(args[1]).toURI().toURL()));
				java.lang.System.exit(0);
			}
			File file = new File(args[0]);
			if (!file.isFile())
			{
				string text = new StringBuilder().append("Can not open '").append(file).append('\'').toString();
				
				throw new FileNotFoundException(text);
			}
			ConfigurationManager cm = new ConfigurationManager(file.toURI().toURL());
			for (int i = 1; i < args.Length; i++)
			{
				string[] array = String.instancehelper_split(args[i], "=");
				if (!SphinxShell.assertionsDisabled && array.Length != 2)
				{
					
					throw new AssertionError();
				}
				string propName = array[0];
				string propValue = array[1];
				ConfigurationManagerUtils.setProperty(cm, propName, propValue);
			}
		}
		
		internal static bool assertionsDisabled = !ClassLiteral<SphinxShell>.Value.desiredAssertionStatus();
	}
}
