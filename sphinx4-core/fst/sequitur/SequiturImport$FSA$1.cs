using System;

using IKVM.Attributes;
using java.lang;
using java.util;

namespace edu.cmu.sphinx.fst.sequitur
{
	
	[Implements(new string[]
	{
		"java.util.Comparator"
	})]
	
	[EnclosingMethod("edu.cmu.sphinx.fst.sequitur.SequiturImport$FSA", "afterUnmarshal", "(Ljavax.xml.bind.Unmarshaller;Ljava.lang.Object;)V")]
	[SourceFile("SequiturImport.java")]
	
	internal sealed class SequiturImport$FSA$1 : java.lang.Object, Comparator
	{
		
		public int compare(SequiturImport.State state, SequiturImport.State state2)
		{
			return state.id - state2.id;
		}

		
		
		internal SequiturImport$FSA$1(SequiturImport.FSA fsa)
		{
		}

		
		
		
		public int compare(object obj, object obj2)
		{
			return this.compare((SequiturImport.State)obj, (SequiturImport.State)obj2);
		}

		
		bool Comparator.Object;)Zequals(object obj)
		{
			return java.lang.Object.instancehelper_equals(this, obj);
		}

		
		internal SequiturImport.FSA this$0 = fsa;
	}
}
