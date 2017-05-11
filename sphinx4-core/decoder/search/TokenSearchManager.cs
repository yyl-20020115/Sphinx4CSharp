using edu.cmu.sphinx.result;
using edu.cmu.sphinx.util.props;

namespace edu.cmu.sphinx.decoder.search
{
	public abstract class TokenSearchManager : java.lang.Object, SearchManager, Configurable
	{
		public TokenSearchManager()
		{
		}
	
		public virtual void newProperties(PropertySheet ps)
		{
			this.buildWordLattice = ps.getBoolean("buildWordLattice").booleanValue();
			this.keepAllTokens = ps.getBoolean("keepAllTokens").booleanValue();
		}
		
		protected internal virtual Token getResultListPredecessor(Token token)
		{
			if (this.keepAllTokens)
			{
				return token;
			}
			if (this.buildWordLattice)
			{
				float num = 0f;
				float num2 = 0f;
				float num3 = 0f;
				while (token != null && !token.isWord())
				{
					num += token.getAcousticScore();
					num2 += token.getLanguageScore();
					num3 += token.getInsertionScore();
					token = token.getPredecessor();
				}
				return new Token(token, token.getScore(), num3, num, num2);
			}
			if (token.isWord())
			{
				return token;
			}
			return token.getPredecessor();
		}
		public abstract void allocate();
		public abstract void deallocate();
		public abstract void startRecognition();
		public abstract void stopRecognition();
		public abstract Result recognize(int t);

		[S4Boolean(new object[]
		{
			64,
			"Ledu/cmu/sphinx/util/props/S4Boolean;",
			"defaultValue",
			true
		})]
		public const string PROP_BUILD_WORD_LATTICE = "buildWordLattice";

		[S4Boolean(new object[]
		{
			64,
			"Ledu/cmu/sphinx/util/props/S4Boolean;",
			"defaultValue",
			false
		})]
		public const string PROP_KEEP_ALL_TOKENS = "keepAllTokens";

		protected internal bool buildWordLattice;

		protected internal bool keepAllTokens;
	}
}
