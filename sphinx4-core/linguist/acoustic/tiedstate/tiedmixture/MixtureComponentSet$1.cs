using System;

using IKVM.Attributes;
using IKVM.Runtime;
using java.lang;
using java.util;

namespace edu.cmu.sphinx.linguist.acoustic.tiedstate.tiedmixture
{
	
	[Implements(new string[]
	{
		"java.util.Comparator"
	})]
	
	[EnclosingMethod("edu.cmu.sphinx.linguist.acoustic.tiedstate.tiedmixture.MixtureComponentSet", null, null)]
	[SourceFile("MixtureComponentSet.java")]
	
	internal sealed class MixtureComponentSet$1 : java.lang.Object, Comparator
	{
		
		
		public int compare(PrunableMixtureComponent prunableMixtureComponent, PrunableMixtureComponent prunableMixtureComponent2)
		{
			return ByteCodeHelper.f2i(prunableMixtureComponent.getStoredScore() - prunableMixtureComponent2.getStoredScore());
		}

		
		
		internal MixtureComponentSet$1(MixtureComponentSet mixtureComponentSet)
		{
		}

		
		
		
		public int compare(object obj, object obj2)
		{
			return this.compare((PrunableMixtureComponent)obj, (PrunableMixtureComponent)obj2);
		}

		
		bool Comparator.Object;)Zequals(object obj)
		{
			return java.lang.Object.instancehelper_equals(this, obj);
		}

		
		internal MixtureComponentSet this$0 = mixtureComponentSet;
	}
}
