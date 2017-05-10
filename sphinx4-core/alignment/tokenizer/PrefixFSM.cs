using System;

using IKVM.Attributes;
using java.net;

namespace edu.cmu.sphinx.alignment.tokenizer
{
	public class PrefixFSM : PronounceableFSM
	{
		[Throws(new string[]
		{
			"java.io.IOException"
		})]
		[LineNumberTable(new byte[]
		{
			159,
			169,
			104
		})]
		
		public PrefixFSM(URL url) : base(url, true)
		{
		}
	}
}
