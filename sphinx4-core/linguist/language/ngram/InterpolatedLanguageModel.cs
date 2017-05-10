using System;

using edu.cmu.sphinx.util;
using edu.cmu.sphinx.util.props;
using IKVM.Attributes;
using ikvm.@internal;
using java.lang;
using java.util;

namespace edu.cmu.sphinx.linguist.language.ngram
{
	[Implements(new string[]
	{
		"edu.cmu.sphinx.linguist.language.ngram.LanguageModel"
	})]
	public class InterpolatedLanguageModel : java.lang.Object, LanguageModel, Configurable
	{
		
		[LineNumberTable(new byte[]
		{
			159,
			188,
			232,
			55,
			231,
			74,
			107,
			103,
			140,
			109,
			102,
			103,
			103,
			23,
			198,
			122,
			101,
			218
		})]
		
		public InterpolatedLanguageModel(List languageModels, float[] floats)
		{
			this.allocated = false;
			this.logMath = LogMath.getLogMath();
			this.languageModels = languageModels;
			this.numberOfLanguageModels = languageModels.size();
			this.weights = new float[floats.Length];
			float num = 0f;
			for (int i = 0; i < floats.Length; i++)
			{
				num += floats[i];
				this.weights[i] = this.logMath.linearToLog((double)floats[i]);
			}
			if ((double)num < 0.999 || (double)num > 1.001)
			{
				string name = ClassLiteral<InterpolatedLanguageModel>.Value.getName();
				string propertyName = "languageModelWeights";
				string msg = "Weights do not sum to 1.0";
				
				throw new PropertyException(name, propertyName, msg);
			}
		}

		[LineNumberTable(new byte[]
		{
			15,
			232,
			36,
			231,
			94
		})]
		
		public InterpolatedLanguageModel()
		{
			this.allocated = false;
		}

		[Throws(new string[]
		{
			"edu.cmu.sphinx.util.props.PropertyException"
		})]
		[LineNumberTable(new byte[]
		{
			20,
			104,
			144,
			108,
			106,
			177,
			108,
			110,
			208,
			108,
			109,
			102,
			142,
			116,
			103,
			255,
			3,
			71,
			226,
			58,
			97,
			101,
			187,
			255,
			0,
			54,
			233,
			77,
			122,
			101,
			218
		})]
		
		public virtual void newProperties(PropertySheet ps)
		{
			if (this.allocated)
			{
				string text = "Can't change properties after allocation";
				
				throw new RuntimeException(text);
			}
			this.languageModels = ps.getComponentList("languageModels", ClassLiteral<LanguageModel>.Value);
			this.numberOfLanguageModels = this.languageModels.size();
			List stringList = ps.getStringList("languageModelWeights");
			if (stringList.size() != this.numberOfLanguageModels)
			{
				string text2 = "Number of weights not equal to number of language models";
				
				throw new RuntimeException(text2);
			}
			float[] array = new float[stringList.size()];
			this.weights = new float[array.Length];
			float num = 0f;
			int i = 0;
			while (i < stringList.size())
			{
				try
				{
					array[i] = Float.parseFloat((string)stringList.get(i));
					num += array[i];
					this.weights[i] = this.logMath.linearToLog((double)array[i]);
				}
				catch (NumberFormatException ex)
				{
					goto IL_D0;
				}
				i++;
				continue;
				IL_D0:
				string name = ClassLiteral<InterpolatedLanguageModel>.Value.getName();
				string propertyName = "languageModelWeights";
				string msg = new StringBuilder().append("Float value expected from the property list. But found:").append((string)stringList.get(i)).toString();
				
				throw new PropertyException(name, propertyName, msg);
			}
			if ((double)num < 0.999 || (double)num > 1.001)
			{
				string name2 = ClassLiteral<InterpolatedLanguageModel>.Value.getName();
				string propertyName2 = "languageModelWeights";
				string msg2 = "Weights do not sum to 1.0";
				
				throw new PropertyException(name2, propertyName2, msg2);
			}
		}

		[Throws(new string[]
		{
			"java.io.IOException"
		})]
		[LineNumberTable(new byte[]
		{
			59,
			104,
			103,
			107,
			127,
			1,
			102,
			114,
			130
		})]
		
		public virtual void allocate()
		{
			if (!this.allocated)
			{
				this.allocated = true;
				this.vocabulary = new HashSet();
				Iterator iterator = this.languageModels.iterator();
				while (iterator.hasNext())
				{
					LanguageModel languageModel = (LanguageModel)iterator.next();
					languageModel.allocate();
					this.vocabulary.addAll(languageModel.getVocabulary());
				}
			}
		}

		[Throws(new string[]
		{
			"java.io.IOException"
		})]
		[LineNumberTable(new byte[]
		{
			70,
			103,
			127,
			1,
			102,
			98
		})]
		
		public virtual void deallocate()
		{
			this.allocated = false;
			Iterator iterator = this.languageModels.iterator();
			while (iterator.hasNext())
			{
				LanguageModel languageModel = (LanguageModel)iterator.next();
				languageModel.deallocate();
			}
		}

		[LineNumberTable(new byte[]
		{
			82,
			102,
			107,
			143,
			115,
			99,
			132,
			238,
			57,
			230,
			74
		})]
		
		public virtual float getProbability(WordSequence wordSequence)
		{
			float num = 0f;
			for (int i = 0; i < this.numberOfLanguageModels; i++)
			{
				float num2 = this.weights[i] + ((LanguageModel)this.languageModels.get(i)).getProbability(wordSequence);
				if (i == 0)
				{
					num = num2;
				}
				else
				{
					num = this.logMath.addAsLinear(num, num2);
				}
			}
			return num;
		}

		public virtual float getSmear(WordSequence wordSequence)
		{
			return 1f;
		}

		
		public virtual Set getVocabulary()
		{
			return this.vocabulary;
		}

		[LineNumberTable(new byte[]
		{
			121,
			98,
			127,
			1,
			103,
			100,
			130,
			98
		})]
		
		public virtual int getMaxDepth()
		{
			int num = 0;
			Iterator iterator = this.languageModels.iterator();
			while (iterator.hasNext())
			{
				LanguageModel languageModel = (LanguageModel)iterator.next();
				int maxDepth = languageModel.getMaxDepth();
				if (maxDepth > num)
				{
					num = maxDepth;
				}
			}
			return num;
		}

		public virtual void onUtteranceEnd()
		{
		}

		[S4ComponentList(new object[]
		{
			64,
			"Ledu/cmu/sphinx/util/props/S4ComponentList;",
			"type",
			new object[]
			{
				99,
				"Ledu/cmu/sphinx/linguist/language/ngram/LanguageModel, sphinx4-core-5prealpha-SNAPSHOT, Version=0/0/0/0, Culture=neutral, PublicKeyToken=null;"
			}
		})]
		public const string PROP_LANGUAGE_MODELS = "languageModels";

		[S4StringList(new object[]
		{
			64,
			"Ledu/cmu/sphinx/util/props/S4StringList;"
		})]
		public const string PROP_LANGUAGE_MODEL_WEIGHTS = "languageModelWeights";

		private LogMath logMath;

		private bool allocated;

		
		private List languageModels;

		private float[] weights;

		private int numberOfLanguageModels;

		
		private Set vocabulary;

		private const double EPSILON = 0.001;
	}
}
