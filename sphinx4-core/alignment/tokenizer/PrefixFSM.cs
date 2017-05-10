using java.net;

namespace edu.cmu.sphinx.alignment.tokenizer
{
	public class PrefixFSM : PronounceableFSM
	{
		public PrefixFSM(URL url) : base(url, true)
		{
		}
	}
}
