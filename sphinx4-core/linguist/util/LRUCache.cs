using System;

using System.Runtime.Serialization;
using System.Security.Permissions;
using IKVM.Attributes;
using java.util;

namespace edu.cmu.sphinx.linguist.util
{
	
	[Serializable]
	public class LRUCache : LinkedHashMap
	{
		[LineNumberTable(new byte[]
		{
			159,
			160,
			104,
			103
		})]
		
		public LRUCache(int maxSize)
		{
			this.maxSize = maxSize;
		}

		
		
		
		protected internal override bool removeEldestEntry(Map.Entry eldest)
		{
			return this.size() > this.maxSize;
		}

		
		[PermissionSet(SecurityAction.Demand, XML = "<PermissionSet class=\"System.Security.PermissionSet\"\nversion=\"1\">\n<IPermission class=\"System.Security.Permissions.SecurityPermission, mscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089\"\nversion=\"1\"\nFlags=\"SerializationFormatter\"/>\n</PermissionSet>\n")]
		protected LRUCache(SerializationInfo serializationInfo, StreamingContext streamingContext) : base(serializationInfo, streamingContext)
		{
		}

		
		internal int maxSize;
	}
}
