using System;

using IKVM.Attributes;
using java.lang;

namespace edu.cmu.sphinx.fst
{
	public class Export : java.lang.Object
	{
		[LineNumberTable(new byte[]
		{
			159,
			171,
			102
		})]
		
		private Export()
		{
		}

		[Throws(new string[]
		{
			"java.io.IOException",
			"java.lang.ClassNotFoundException"
		})]
		[LineNumberTable(new byte[]
		{
			159,
			188,
			101,
			111,
			106,
			101,
			111,
			166,
			169,
			111,
			105
		})]
		
		public static void main(string[] args)
		{
			if (args.Length < 2)
			{
				java.lang.System.err.println("Input and output files not provided");
				java.lang.System.err.println("You need to provide both the input serialized java fst model");
				java.lang.System.err.println("and the output binary openfst model.");
				java.lang.System.exit(1);
			}
			Fst fst = Fst.loadModel(args[0]);
			java.lang.System.@out.println("Saving as openfst text model...");
			Convert.export(fst, args[1]);
		}
	}
}
