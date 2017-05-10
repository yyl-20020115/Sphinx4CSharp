using System;

using edu.cmu.sphinx.fst.semiring;
using IKVM.Attributes;
using java.io;
using java.lang;

namespace edu.cmu.sphinx.fst
{
	public class Import : java.lang.Object
	{
		[LineNumberTable(new byte[]
		{
			159,
			173,
			102
		})]
		
		private Import()
		{
		}

		[Throws(new string[]
		{
			"java.lang.NumberFormatException",
			"java.io.IOException"
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
			174,
			143,
			244,
			69,
			226,
			61,
			97,
			127,
			7,
			134
		})]
		
		public static void main(string[] args)
		{
			if (args.Length < 2)
			{
				java.lang.System.err.println("Input and output files not provided");
				java.lang.System.err.println("You need to provide both the input binary openfst model");
				java.lang.System.err.println("and the output serialized java fst model.");
				java.lang.System.exit(1);
			}
			Fst fst = Convert.importFst(args[0], new TropicalSemiring());
			java.lang.System.@out.println("Saving as binary java fst model...");
			try
			{
				fst.saveModel(args[1]);
			}
			catch (IOException ex)
			{
				goto IL_66;
			}
			return;
			IL_66:
			java.lang.System.err.println(new StringBuilder().append("Cannot write to file ").append(args[1]).toString());
			java.lang.System.exit(1);
		}
	}
}
