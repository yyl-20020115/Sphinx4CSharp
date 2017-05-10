using System;
using edu.cmu.sphinx.linguist.acoustic;
using IKVM.Attributes;

namespace edu.cmu.sphinx.linguist
{
	[Implements(new string[]
	{
		"edu.cmu.sphinx.linguist.SearchState"
	})]
	public interface UnitSearchState : SearchState
	{
		Unit getUnit();
	}
}
