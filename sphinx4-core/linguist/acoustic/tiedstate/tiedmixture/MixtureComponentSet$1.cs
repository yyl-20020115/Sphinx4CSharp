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
	
	
	.
	
	internal sealed class MixtureComponentSet_1 : java.lang.Object, Comparator
	{
		
		
		public int compare(PrunableMixtureComponent prunableMixtureComponent, PrunableMixtureComponent prunableMixtureComponent2)
		{
			return ByteCodeHelper.f2i(prunableMixtureComponent.getStoredScore() - prunableMixtureComponent2.getStoredScore());
		}

		
		
		internal MixtureComponentSet_1(MixtureComponentSet mixtureComponentSet)
		{
		}

		
		
		
		public int compare(object obj, object obj2)
		{
			return this.compare((PrunableMixtureComponent)obj, (PrunableMixtureComponent)obj2);
		}

		
		bool Comparator.equals(object obj)
		{
			return java.lang.Object.instancehelper_equals(this, obj);
		}

		
		internal MixtureComponentSet this_0 = mixtureComponentSet;
	}
}
