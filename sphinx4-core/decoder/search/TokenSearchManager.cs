using System;

using edu.cmu.sphinx.result;
using edu.cmu.sphinx.util.props;
using IKVM.Attributes;
using java.lang;

namespace edu.cmu.sphinx.decoder.search
{
	[Implements(new string[]
	{
		"edu.cmu.sphinx.decoder.search.SearchManager"
	})]
	public abstract class TokenSearchManager : java.lang.Object, SearchManager, Configurable
	{
		
		
		public TokenSearchManager()
		{
		}

		[Throws(new string[]
		{
			"edu.cmu.sphinx.util.props.PropertyException"
		})]
		[LineNumberTable(new byte[]
		{
			159,
			174,
			118,
			118
		})]
		
		public virtual void newProperties(PropertySheet ps)
		{
			this.buildWordLattice = ps.getBoolean("buildWordLattice").booleanValue();
			this.keepAllTokens = ps.getBoolean("keepAllTokens").booleanValue();
		}

		[LineNumberTable(new byte[]
		{
			11,
			104,
			162,
			104,
			104,
			130,
			167,
			102,
			102,
			134,
			107,
			106,
			106,
			106,
			170
		})]
		
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
				Token.__<clinit>();
				return new Token(token, token.getScore(), num3, num, num2);
			}
			if (token.isWord())
			{
				return token;
			}
			return token.getPredecessor();
		}

		[HideFromReflection]
		public abstract void allocate();

		[HideFromReflection]
		public abstract void deallocate();

		[HideFromReflection]
		public abstract void startRecognition();

		[HideFromReflection]
		public abstract void stopRecognition();

		[HideFromReflection]
		public abstract Result recognize(int);

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
