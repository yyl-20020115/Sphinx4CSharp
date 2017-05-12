using java.net;

namespace edu.cmu.sphinx.alignment.tokenizer
{
	public class SuffixFSM : PronounceableFSM
	{
		public SuffixFSM(URL url) : base(url, false)
		{
		}
	}
}
