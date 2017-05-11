using System;
using System.Runtime.Serialization;
using System.Security.Permissions;
using edu.cmu.sphinx.linguist.dictionary;

namespace edu.cmu.sphinx.linguist.flat
{
	[Serializable]
	internal sealed class UnknownWordState : SentenceHMMState, WordSearchState, SearchState
	{
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
		
		object SearchState.getLexState()
		{
			return this.getLexState();
		}
		
		[PermissionSet(SecurityAction.Demand, XML = "<PermissionSet class=\"System.Security.PermissionSet\"\nversion=\"1\">\n<IPermission class=\"System.Security.Permissions.SecurityPermission, mscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089\"\nversion=\"1\"\nFlags=\"SerializationFormatter\"/>\n</PermissionSet>\n")]
		public UnknownWordState(SerializationInfo serializationInfo, StreamingContext streamingContext) : base(serializationInfo, streamingContext)
		{
		}
	}
}
