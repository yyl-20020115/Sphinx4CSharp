using System;

using System.Runtime.Serialization;
using System.Security.Permissions;
using edu.cmu.sphinx.linguist.dictionary;
using IKVM.Attributes;
using java.lang;

namespace edu.cmu.sphinx.linguist.flat
{
	[Serializable]
	public class WordState : SentenceHMMState
	{
		
		public new static void __<clinit>()
		{
		}

		
		
		public virtual Word getWord()
		{
			return ((AlternativeState)this.getParent()).getAlternative()[this.getWhich()];
		}

		[LineNumberTable(new byte[]
		{
			159,
			169,
			109
		})]
		
		public WordState(AlternativeState parent, int which) : base("W", parent, which)
		{
		}

		
		
		public override string getPrettyName()
		{
			return new StringBuilder().append(this.getName()).append('(').append(this.getWord().getSpelling()).append(')').toString();
		}

		public override string getTypeLabel()
		{
			return "Word";
		}

		public override int getOrder()
		{
			return 1;
		}

		
		static WordState()
		{
			SentenceHMMState.__<clinit>();
		}

		
		[PermissionSet(SecurityAction.Demand, XML = "<PermissionSet class=\"System.Security.PermissionSet\"\nversion=\"1\">\n<IPermission class=\"System.Security.Permissions.SecurityPermission, mscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089\"\nversion=\"1\"\nFlags=\"SerializationFormatter\"/>\n</PermissionSet>\n")]
		protected WordState(SerializationInfo serializationInfo, StreamingContext streamingContext) : base(serializationInfo, streamingContext)
		{
		}
	}
}
