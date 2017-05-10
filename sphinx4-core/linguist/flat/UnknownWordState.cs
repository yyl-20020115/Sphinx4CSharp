using System;

using System.Runtime.Serialization;
using System.Security.Permissions;
using edu.cmu.sphinx.linguist.dictionary;
using IKVM.Attributes;

namespace edu.cmu.sphinx.linguist.flat
{
	[Implements(new string[]
	{
		"edu.cmu.sphinx.linguist.WordSearchState"
	})]
	[SourceFile("CIPhoneLoop.java")]
	
	[Serializable]
	internal sealed class UnknownWordState : SentenceHMMState, WordSearchState, SearchState
	{
		
		public new static void __<clinit>()
		{
		}

		
		
		internal UnknownWordState()
		{
		}

		
		
		public Pronunciation getPronunciation()
		{
			return Word.__UNKNOWN.getPronunciations()[0];
		}

		public override int getOrder()
		{
			return 0;
		}

		public override string getName()
		{
			return "UnknownWordState";
		}

		public override bool isWordStart()
		{
			return true;
		}

		
		static UnknownWordState()
		{
			SentenceHMMState.__<clinit>();
		}

		
		object SearchState.Object;getLexState()
		{
			return this.<bridge>getLexState();
		}

		
		[PermissionSet(SecurityAction.Demand, XML = "<PermissionSet class=\"System.Security.PermissionSet\"\nversion=\"1\">\n<IPermission class=\"System.Security.Permissions.SecurityPermission, mscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089\"\nversion=\"1\"\nFlags=\"SerializationFormatter\"/>\n</PermissionSet>\n")]
		protected UnknownWordState(SerializationInfo serializationInfo, StreamingContext streamingContext) : base(serializationInfo, streamingContext)
		{
		}
	}
}
