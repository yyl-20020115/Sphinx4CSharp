using System;

using System.Runtime.Serialization;
using System.Security.Permissions;
using IKVM.Attributes;

namespace edu.cmu.sphinx.linguist.flat
{
	[SourceFile("CIPhoneLoop.java")]
	
	[Serializable]
	internal sealed class LoopBackState : SentenceHMMState
	{
		
		public new static void __<clinit>()
		{
		}

		[LineNumberTable(new byte[]
		{
			160,
			121,
			109
		})]
		
		internal LoopBackState(SentenceHMMState parent) : base("CIPhonesLoopBackState", parent, 0)
		{
		}

		public override int getOrder()
		{
			return 1;
		}

		
		static LoopBackState()
		{
			SentenceHMMState.__<clinit>();
		}

		
		[PermissionSet(SecurityAction.Demand, XML = "<PermissionSet class=\"System.Security.PermissionSet\"\nversion=\"1\">\n<IPermission class=\"System.Security.Permissions.SecurityPermission, mscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089\"\nversion=\"1\"\nFlags=\"SerializationFormatter\"/>\n</PermissionSet>\n")]
		protected LoopBackState(SerializationInfo serializationInfo, StreamingContext streamingContext) : base(serializationInfo, streamingContext)
		{
		}
	}
}
