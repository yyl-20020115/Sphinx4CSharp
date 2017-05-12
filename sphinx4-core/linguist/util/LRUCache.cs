using java.util;

namespace edu.cmu.sphinx.linguist.util
{
	[System.Serializable]
	public class LRUCache : LinkedHashMap
	{		
		public LRUCache(int maxSize)
		{
			this.maxSize = maxSize;
		}

		protected override bool removeEldestEntry(Map.Entry eldest)
		{
			return this.size() > this.maxSize;
		}
		
		[System.Security.Permissions.PermissionSet(System.Security.Permissions.SecurityAction.Demand, XML = "<PermissionSet class=\"System.Security.PermissionSet\"\nversion=\"1\">\n<IPermission class=\"System.Security.Permissions.SecurityPermission, mscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089\"\nversion=\"1\"\nFlags=\"SerializationFormatter\"/>\n</PermissionSet>\n")]
		protected LRUCache(System.Runtime.Serialization.SerializationInfo serializationInfo, System.Runtime.Serialization.StreamingContext streamingContext) : base(serializationInfo, streamingContext)
		{
		}
		
		internal int maxSize;
	}
}
