using System;

using System.Runtime.Serialization;
using System.Security.Permissions;
using edu.cmu.sphinx.linguist.dictionary;
using IKVM.Attributes;
using java.io;

namespace edu.cmu.sphinx.linguist.flat
{
	[Implements(new string[]
	{
		"java.io.Serializable"
	})]
	[Serializable]
	public class AlternativeState : SentenceHMMState, Serializable.__Interface
	{
		
		public new static void __<clinit>()
		{
		}

		[LineNumberTable(new byte[]
		{
			159,
			172,
			109
		})]
		
		public AlternativeState(GrammarState parent, int which) : base("A", parent, which)
		{
		}

		[LineNumberTable(new byte[]
		{
			159,
			182,
			107,
			48
		})]
		
		public virtual Word[] getAlternative()
		{
			return ((GrammarState)this.getParent()).getGrammarNode().getWords(this.getWhich());
		}

		public override string getTypeLabel()
		{
			return "Alt";
		}

		public override int getOrder()
		{
			return 1;
		}

		
		static AlternativeState()
		{
			SentenceHMMState.__<clinit>();
		}

		
		public static implicit operator Serializable(AlternativeState _<ref>)
		{
			Serializable result;
			result.__<ref> = _<ref>;
			return result;
		}

		
		[PermissionSet(SecurityAction.Demand, XML = "<PermissionSet class=\"System.Security.PermissionSet\"\nversion=\"1\">\n<IPermission class=\"System.Security.Permissions.SecurityPermission, mscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089\"\nversion=\"1\"\nFlags=\"SerializationFormatter\"/>\n</PermissionSet>\n")]
		protected AlternativeState(SerializationInfo serializationInfo, StreamingContext streamingContext) : base(serializationInfo, streamingContext)
		{
		}
	}
}
