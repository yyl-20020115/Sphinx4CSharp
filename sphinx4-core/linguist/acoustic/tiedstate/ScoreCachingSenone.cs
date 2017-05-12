using edu.cmu.sphinx.frontend;
using ikvm.@internal;
using java.io;

namespace edu.cmu.sphinx.linguist.acoustic.tiedstate
{
	[System.Serializable]
	public abstract class ScoreCachingSenone : java.lang.Object, Senone, Serializable.__Interface, System.Runtime.Serialization.ISerializable
	{
		protected internal abstract float calculateScore(Data d);

		public ScoreCachingSenone()
		{
			this.scoreCache = new ScoreCachingSenone.ScoreCache(this, null, 0f);
			System.Threading.Thread.MemoryBarrier();
		}

		public virtual float getScore(Data feature)
		{
			ScoreCachingSenone.ScoreCache scoreCache = this.scoreCache;
			if (feature != ScoreCachingSenone.ScoreCache.access_000(scoreCache))
			{
				scoreCache = new ScoreCachingSenone.ScoreCache(this, feature, this.calculateScore(feature));
				this.scoreCache = scoreCache;
				System.Threading.Thread.MemoryBarrier();
			}
			return ScoreCachingSenone.ScoreCache.access_100(scoreCache);
		}

		public abstract float[] calculateComponentScore(Data d);

		public abstract long getID();

		public abstract void dump(string s);

		public abstract MixtureComponent[] getMixtureComponents();

		public abstract float[] getLogMixtureWeights();

		public static implicit operator Serializable(ScoreCachingSenone _ref)
		{
			Serializable result = Serializable.Cast(_ref);
			return result;
		}

		[System.Security.SecurityCritical]
		[System.Security.Permissions.PermissionSet(System.Security.Permissions.SecurityAction.Demand, XML = "<PermissionSet class=\"System.Security.PermissionSet\"\nversion=\"1\">\n<IPermission class=\"System.Security.Permissions.SecurityPermission, mscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089\"\nversion=\"1\"\nFlags=\"SerializationFormatter\"/>\n</PermissionSet>\n")]
		public virtual void GetObjectData(System.Runtime.Serialization.SerializationInfo serializationInfo, System.Runtime.Serialization.StreamingContext streamingContext)
		{
			Serialization.writeObject(this, serializationInfo);
		}
		
		[System.Security.Permissions.PermissionSet(System.Security.Permissions.SecurityAction.Demand, XML = "<PermissionSet class=\"System.Security.PermissionSet\"\nversion=\"1\">\n<IPermission class=\"System.Security.Permissions.SecurityPermission, mscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089\"\nversion=\"1\"\nFlags=\"SerializationFormatter\"/>\n</PermissionSet>\n")]
		protected ScoreCachingSenone(System.Runtime.Serialization.SerializationInfo serializationInfo, System.Runtime.Serialization.StreamingContext streamingContext)
		{
			Serialization.readObject(this, serializationInfo);
		}

		private volatile ScoreCachingSenone.ScoreCache scoreCache;
		
		internal sealed class ScoreCache : java.lang.Object
		{			
			public ScoreCache(ScoreCachingSenone scoreCachingSenone, Data data, float num)
			{
				this.this_0 = scoreCachingSenone;
				this.feature = data;
				this.score = num;
			}
			
			internal static Data access_000(ScoreCachingSenone.ScoreCache scoreCache)
			{
				return scoreCache.feature;
			}
			
			internal static float access_100(ScoreCachingSenone.ScoreCache scoreCache)
			{
				return scoreCache.score;
			}
			
			private Data feature;
			
			private float score;
			
			internal ScoreCachingSenone this_0;
		}
	}
}
