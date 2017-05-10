using System;

using edu.cmu.sphinx.linguist.acoustic;
using edu.cmu.sphinx.linguist.dictionary;
using IKVM.Attributes;
using java.util;

namespace edu.cmu.sphinx.linguist.allphone
{
	[Implements(new string[]
	{
		"edu.cmu.sphinx.linguist.WordSearchState"
	})]
	public class PhoneWordSearchState : PhoneNonEmittingSearchState, WordSearchState, SearchState
	{
		[LineNumberTable(new byte[]
		{
			159,
			160,
			109
		})]
		
		public PhoneWordSearchState(Unit unit, AllphoneLinguist linguist, float insertionProb, float languageProb) : base(unit, linguist, insertionProb, languageProb)
		{
		}

		[LineNumberTable(new byte[]
		{
			159,
			164,
			102,
			102,
			108,
			109,
			120,
			127,
			9,
			127,
			0,
			127,
			12
		})]
		
		public override SearchStateArc[] getSuccessors()
		{
			ArrayList arrayList = new ArrayList();
			Unit @base = UnitManager.__SILENCE;
			Unit baseUnit = this.unit.getBaseUnit();
			if (this.unit.isContextDependent())
			{
				@base = ((LeftRightContext)this.unit.getContext()).getRightContext()[0];
			}
			ArrayList arrayList2 = (!this.linguist.useContextDependentPhones()) ? this.linguist.getCISuccessors() : this.linguist.getCDSuccessors(baseUnit, @base);
			Iterator iterator = arrayList2.iterator();
			while (iterator.hasNext())
			{
				HMM hmm = (HMM)iterator.next();
				arrayList.add(new PhoneHmmSearchState(hmm.getInitialState(), this.linguist, this.linguist.getPhoneInsertionProb(), 0f));
			}
			return (SearchStateArc[])arrayList.toArray(new SearchStateArc[arrayList.size()]);
		}

		public override bool isFinal()
		{
			return true;
		}

		[LineNumberTable(new byte[]
		{
			159,
			180,
			103,
			105,
			113,
			125
		})]
		
		public virtual Pronunciation getPronunciation()
		{
			Pronunciation pronunciation = new Pronunciation(new Unit[]
			{
				this.unit
			}, "", 1f);
			Pronunciation pronunciation2 = pronunciation;
			Word.__<clinit>();
			pronunciation2.setWord(new Word(this.unit.getName(), null, false));
			return pronunciation;
		}

		public virtual bool isWordStart()
		{
			return false;
		}

		public override int getOrder()
		{
			return 1;
		}

		[LineNumberTable(new byte[]
		{
			5,
			104,
			98,
			126,
			127,
			2
		})]
		
		public override bool equals(object obj)
		{
			if (!(obj is PhoneWordSearchState))
			{
				return false;
			}
			int num = (((PhoneWordSearchState)obj).unit.getBaseID() == this.unit.getBaseID()) ? 1 : 0;
			int num2 = ((PhoneWordSearchState)obj).unit.getContext().equals(this.unit.getContext()) ? 1 : 0;
			return num != 0 && num2 != 0;
		}

		
		
		public override int hashCode()
		{
			return this.unit.getContext().hashCode() * 91 + this.unit.getBaseID();
		}
	}
}
