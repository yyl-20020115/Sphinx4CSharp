using System;
using System.Runtime.CompilerServices;
using edu.cmu.sphinx.demo.aligner;
using edu.cmu.sphinx.demo.dialog;
using edu.cmu.sphinx.demo.speakerid;
using edu.cmu.sphinx.demo.transcriber;
using IKVM.Attributes;
using ikvm.@internal;
using IKVM.Runtime;
using java.lang;
using java.lang.reflect;
using java.util;

namespace edu.cmu.sphinx.demo
{
	public class DemoRunner : java.lang.Object
	{
		public static void main(string[] args)
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
				Console.WriteLine(ex.getMessage());
			}
			return;
		}

		[LineNumberTable(new byte[]
		{
			159,
			171,
			111,
			143,
			127,
			5,
			127,
			7
		})]
		[MethodImpl(MethodImplOptions.NoInlining)]
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

		[LineNumberTable(15)]
		[MethodImpl(MethodImplOptions.NoInlining)]
		public DemoRunner()
		{
		}

		[LineNumberTable(new byte[]
		{
			159,
			159,
			115,
			202,
			117,
			117,
			117,
			117
		})]
		static DemoRunner()
		{
			DemoRunner.classes.put("aligner", ClassLiteral<AlignerDemo>.Value);
			DemoRunner.classes.put("dialog", ClassLiteral<DialogDemo>.Value);
			DemoRunner.classes.put("speakerid", ClassLiteral<SpeakerIdentificationDemo>.Value);
			DemoRunner.classes.put("transcriber", ClassLiteral<TranscriberDemo>.Value);
		}
		

		[Modifiers((Modifiers)24)]
		[Signature("[Ljava/lang/Class<*>;")]
		internal static Class[] paramTypes = new Class[]
		{
			ClassLiteral<string[]>.Value
		};

		[Modifiers((Modifiers)26)]
		[Signature("Ljava/util/Map<Ljava/lang/String;Ljava/lang/Class<*>;>;")]
		private static Map classes = new TreeMap();

		class CID : CallerID { };

		static CID myid = new CID();
	}
}
