using System;

using System.Runtime.Serialization;
using System.Security.Permissions;
using edu.cmu.sphinx.linguist.acoustic;
using IKVM.Attributes;

namespace edu.cmu.sphinx.linguist.flat
{
	[Serializable]
	public class NonEmittingHMMState : HMMStateState
	{
		
		public new static void __<clinit>()
		{
		}

		[LineNumberTable(new byte[]
		{
			159,
			171,
			104
		})]
		
		public NonEmittingHMMState(SentenceHMMState parent, HMMState hmmState) : base(parent, hmmState)
		{
		}

		
		static NonEmittingHMMState()
		{
			HMMStateState.__<clinit>();
		}

		
		[PermissionSet(SecurityAction.Demand, XML = "<PermissionSet class=\"System.Security.PermissionSet\"\nversion=\"1\">\n<IPermission class=\"System.Security.Permissions.SecurityPermission, mscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089\"\nversion=\"1\"\nFlags=\"SerializationFormatter\"/>\n</PermissionSet>\n")]
		protected NonEmittingHMMState(SerializationInfo serializationInfo, StreamingContext streamingContext) : base(serializationInfo, streamingContext)
		{
		}
	}
}
