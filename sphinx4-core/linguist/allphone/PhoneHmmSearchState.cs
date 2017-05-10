using System;

using edu.cmu.sphinx.decoder.scorer;
using edu.cmu.sphinx.frontend;
using edu.cmu.sphinx.linguist.acoustic;
using edu.cmu.sphinx.linguist.acoustic.tiedstate;
using IKVM.Attributes;
using java.lang;
using java.util;

namespace edu.cmu.sphinx.linguist.allphone
{
	[Implements(new string[]
	{
		"edu.cmu.sphinx.linguist.SearchState",
		"edu.cmu.sphinx.linguist.SearchStateArc",
		"edu.cmu.sphinx.decoder.scorer.ScoreProvider"
	})]
	public class PhoneHmmSearchState : java.lang.Object, SearchState, SearchStateArc, ScoreProvider
	{
		
		
		public virtual int getBaseId()
		{
			return ((SenoneHMM)this.state.getHMM()).getBaseUnit().getBaseID();
		}

		[LineNumberTable(new byte[]
		{
			159,
			166,
			104,
			103,
			103,
			104,
			105
		})]
		
		public PhoneHmmSearchState(HMMState hmmState, AllphoneLinguist linguist, float insertionProb, float languageProb)
		{
			this.state = hmmState;
			this.linguist = linguist;
			this.insertionProb = insertionProb;
			this.languageProb = languageProb;
		}

		public virtual float getLanguageProbability()
		{
			return this.languageProb;
		}

		public virtual float getInsertionProbability()
		{
			return this.insertionProb;
		}

		public virtual SearchState getState()
		{
			return this;
		}

		
		
		public virtual float getProbability()
		{
			return this.getLanguageProbability() + this.getInsertionProbability();
		}

		[LineNumberTable(new byte[]
		{
			5,
			112,
			127,
			2,
			108,
			103,
			63,
			7,
			134,
			130,
			108,
			104,
			103,
			63,
			3,
			166
		})]
		
		public virtual SearchStateArc[] getSuccessors()
		{
			SearchStateArc[] array;
			if (this.state.isExitState())
			{
				ArrayList units = this.linguist.getUnits(((SenoneHMM)this.state.getHMM()).getSenoneSequence());
				array = new SearchStateArc[units.size()];
				for (int i = 0; i < array.Length; i++)
				{
					array[i] = new PhoneNonEmittingSearchState((Unit)units.get(i), this.linguist, this.insertionProb, this.languageProb);
				}
				return array;
			}
			HMMStateArc[] successors = this.state.getSuccessors();
			array = new SearchStateArc[successors.Length];
			for (int i = 0; i < successors.Length; i++)
			{
				array[i] = new PhoneHmmSearchState(successors[i].getHMMState(), this.linguist, this.insertionProb, this.languageProb);
			}
			return array;
		}

		
		
		public virtual bool isEmitting()
		{
			return this.state.isEmitting();
		}

		public virtual bool isFinal()
		{
			return false;
		}

		
		
		public virtual string toPrettyString()
		{
			return new StringBuilder().append("HMM ").append(java.lang.Object.instancehelper_toString(this.state)).toString();
		}

		public virtual string getSignature()
		{
			return null;
		}

		public virtual WordSequence getWordHistory()
		{
			return null;
		}

		public virtual object getLexState()
		{
			return null;
		}

		public virtual int getOrder()
		{
			return 2;
		}

		
		
		public virtual float getScore(Data data)
		{
			return this.state.getScore(data);
		}

		
		
		public virtual float[] getComponentScore(Data feature)
		{
			return this.state.calculateComponentScore(feature);
		}

		[LineNumberTable(new byte[]
		{
			59,
			104,
			98,
			123,
			118
		})]
		
		public override bool equals(object obj)
		{
			if (!(obj is PhoneHmmSearchState))
			{
				return false;
			}
			SenoneSequence senoneSequence = ((SenoneHMM)((PhoneHmmSearchState)obj).state.getHMM()).getSenoneSequence();
			SenoneSequence senoneSequence2 = ((SenoneHMM)this.state.getHMM()).getSenoneSequence();
			return senoneSequence2.equals(senoneSequence);
		}

		
		
		public override int hashCode()
		{
			return ((SenoneHMM)this.state.getHMM()).getSenoneSequence().hashCode() + this.state.getState() * 37;
		}

		private HMMState state;

		private AllphoneLinguist linguist;

		private float insertionProb;

		private float languageProb;
	}
}
