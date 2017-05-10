using System;

using edu.cmu.sphinx.linguist.dictionary;
using edu.cmu.sphinx.util.props;
using IKVM.Attributes;
using java.net;

namespace edu.cmu.sphinx.linguist.language.ngram.large
{
	public class LargeTrigramModel : LargeNGramModel
	{
		[LineNumberTable(new byte[]
		{
			159,
			128,
			105,
			255,
			4,
			69
		})]
		
		public LargeTrigramModel(string format, URL urlLocation, string ngramLogFile, int maxTrigramCacheSize, int maxBigramCacheSize, bool clearCacheAfterUtterance, int maxDepth, Dictionary dictionary, bool applyLanguageWeightAndWip, float languageWeight, double wip, float unigramWeight, bool fullSmear) : base(format, urlLocation, ngramLogFile, maxTrigramCacheSize, clearCacheAfterUtterance, (maxDepth <= 3) ? maxDepth : 3, dictionary, applyLanguageWeightAndWip, languageWeight, wip, unigramWeight, fullSmear)
		{
		}

		[LineNumberTable(new byte[]
		{
			17,
			102
		})]
		
		public LargeTrigramModel()
		{
		}

		[Throws(new string[]
		{
			"edu.cmu.sphinx.util.props.PropertyException"
		})]
		[LineNumberTable(new byte[]
		{
			29,
			108,
			113,
			113,
			103,
			37,
			138,
			113,
			113,
			118,
			103,
			37,
			138,
			113,
			114,
			113,
			118
		})]
		
		public override void newProperties(PropertySheet ps)
		{
			this.logger = ps.getLogger();
			this.location = ConfigurationManagerUtils.getResource("location", ps);
			this.ngramLogFile = ps.getString("queryLogFile");
			this.clearCacheAfterUtterance = ps.getBoolean("clearCachesAfterUtterance").booleanValue();
			this.maxDepth = ps.getInt("maxDepth");
			this.ngramCacheSize = ps.getInt("trigramCacheSize");
			this.dictionary = (Dictionary)ps.getComponent("dictionary");
			this.applyLanguageWeightAndWip = ps.getBoolean("applyLanguageWeightAndWip").booleanValue();
			this.languageWeight = ps.getFloat("languageWeight");
			this.wip = ps.getDouble("wordInsertionProbability");
			this.unigramWeight = ps.getFloat("unigramWeight");
			this.fullSmear = ps.getBoolean("fullSmear").booleanValue();
		}

		[S4Integer(new object[]
		{
			64,
			"Ledu/cmu/sphinx/util/props/S4Integer;",
			"defaultValue",
			100000
		})]
		public const string PROP_TRIGRAM_CACHE_SIZE = "trigramCacheSize";
	}
}
