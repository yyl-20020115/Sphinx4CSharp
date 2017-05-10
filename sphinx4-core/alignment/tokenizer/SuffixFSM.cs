using System;

using IKVM.Attributes;
using java.net;

namespace edu.cmu.sphinx.alignment.tokenizer
{
	public class SuffixFSM : PronounceableFSM
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
		
		public SuffixFSM(URL url) : base(url, false)
		{
		}
	}
}
