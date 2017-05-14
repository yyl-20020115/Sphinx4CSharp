using edu.cmu.sphinx.demo.aligner;
using edu.cmu.sphinx.demo.dialog;
//using edu.cmu.sphinx.demo.speakerid;
using edu.cmu.sphinx.demo.transcriber;
using ikvm.@internal;
using java.lang;
using java.lang.reflect;
using java.util;

namespace edu.cmu.sphinx.demo
{
	public class DemoRunner : Object
	{
		public static void Main(string[] args)
		{
			if (0 == args.Length || !DemoRunner.classes.containsKey(args[0]))
			{
				DemoRunner.printUsage();
				java.lang.System.exit(1);
			}
			try
			{
				Method method = ((Class)DemoRunner.classes.get(args[0])).getMethod("main", DemoRunner.paramTypes, myid);
				method.invoke(null, new object[]
				{
					Arrays.copyOfRange(args, 1, args.Length)
				}, myid);
			}
			catch (InvocationTargetException ex)
			{
				System.Console.WriteLine(ex.getMessage());
			}
			return;
		}

		public static void printUsage()
		{
			java.lang.System.err.println("Usage: DemoRunner <DEMO> [<ARG> ...]\n");
			java.lang.System.err.println("Demo names:");
			Iterator iterator = DemoRunner.classes.keySet().iterator();
			while (iterator.hasNext())
			{
				string text = (string)iterator.next();
				java.lang.System.err.println(new StringBuilder().append("    ").append(text).toString());
			}
		}

		public DemoRunner()
		{
		}

		static DemoRunner()
		{
			DemoRunner.classes.put("aligner", ClassLiteral<AlignerDemo>.Value);
			DemoRunner.classes.put("dialog", ClassLiteral<DialogDemo>.Value);
			//DemoRunner.classes.put("speakerid", ClassLiteral<SpeakerIdentificationDemo>.Value);
			DemoRunner.classes.put("transcriber", ClassLiteral<TranscriberDemo>.Value);
		}
		
		internal static Class[] paramTypes = new Class[]
		{
			ClassLiteral<string[]>.Value
		};

		private static Map classes = new TreeMap();

		class CID : CallerID { };

		static CID myid = new CID();
	}
}
