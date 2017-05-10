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
	[Serializable]
	public class PronunciationState : SentenceHMMState, WordSearchState, SearchState
	{
		
		public new static void __<clinit>()
		{
		}

		public virtual Pronunciation getPronunciation()
		{
			return this.pronunciation;
		}

		[LineNumberTable(new byte[]
		{
			159,
			188,
			107,
			103
		})]
		
		public PronunciationState(string name, Pronunciation p, int which) : base(name, null, which)
		{
			this.pronunciation = p;
		}

		[LineNumberTable(new byte[]
		{
			159,
			174,
			143,
			115
		})]
		
		public PronunciationState(WordState parent, int which) : base("P", parent, which)
		{
			this.pronunciation = parent.getWord().getPronunciations()[which];
		}

		public override string getTypeLabel()
		{
			return "Pron";
		}

		public override int getOrder()
		{
			return 4;
		}

		public override bool isWordStart()
		{
			return true;
		}

		
		static PronunciationState()
		{
			SentenceHMMState.__<clinit>();
		}

		
		object SearchState.Object;getLexState()
		{
			return this.<bridge>getLexState();
		}

		
		[PermissionSet(SecurityAction.Demand, XML = "<PermissionSet class=\"System.Security.PermissionSet\"\nversion=\"1\">\n<IPermission class=\"System.Security.Permissions.SecurityPermission, mscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089\"\nversion=\"1\"\nFlags=\"SerializationFormatter\"/>\n</PermissionSet>\n")]
		protected PronunciationState(SerializationInfo serializationInfo, StreamingContext streamingContext) : base(serializationInfo, streamingContext)
		{
		}

		
		private Pronunciation pronunciation;
	}
}
