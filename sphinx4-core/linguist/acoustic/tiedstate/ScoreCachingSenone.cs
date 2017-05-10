using System;

using System.Runtime.Serialization;
using System.Security;
using System.Security.Permissions;
using System.Threading;
using edu.cmu.sphinx.frontend;
using IKVM.Attributes;
using ikvm.@internal;
using java.io;
using java.lang;

namespace edu.cmu.sphinx.linguist.acoustic.tiedstate
{
	[Implements(new string[]
	{
		"edu.cmu.sphinx.linguist.acoustic.tiedstate.Senone"
	})]
	[Serializable]
	public abstract class ScoreCachingSenone : java.lang.Object, Senone, Serializable.__Interface, ISerializable
	{
		protected internal abstract float calculateScore(Data d);

		[LineNumberTable(new byte[]
		{
			159,
			160,
			232,
			76
		})]
		
		public ScoreCachingSenone()
		{
			this.scoreCache = new ScoreCachingSenone.ScoreCache(this, null, 0f);
			Thread.MemoryBarrier();
		}

		[LineNumberTable(new byte[]
		{
			159,
			180,
			105,
			105,
			111,
			142
		})]
		
		public virtual float getScore(Data feature)
		{
			ScoreCachingSenone.ScoreCache scoreCache = this.scoreCache;
			if (feature != ScoreCachingSenone.ScoreCache.access_000(scoreCache))
			{
				scoreCache = new ScoreCachingSenone.ScoreCache(this, feature, this.calculateScore(feature));
				this.scoreCache = scoreCache;
				Thread.MemoryBarrier();
			}
			return ScoreCachingSenone.ScoreCache.access$100(scoreCache);
		}

		[HideFromReflection]
		public abstract float[] calculateComponentScore(Data);

		[HideFromReflection]
		public abstract long getID();

		[HideFromReflection]
		public abstract void dump(string);

		[HideFromReflection]
		public abstract MixtureComponent[] getMixtureComponents();

		[HideFromReflection]
		public abstract float[] getLogMixtureWeights();

		
		public static implicit operator Serializable(ScoreCachingSenone _ref)
		{
			Serializable result;
			result.__ref = _ref;
			return result;
		}

		[SecurityCritical]
		
		[PermissionSet(SecurityAction.Demand, XML = "<PermissionSet class=\"System.Security.PermissionSet\"\nversion=\"1\">\n<IPermission class=\"System.Security.Permissions.SecurityPermission, mscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089\"\nversion=\"1\"\nFlags=\"SerializationFormatter\"/>\n</PermissionSet>\n")]
		protected virtual void GetObjectData(SerializationInfo serializationInfo, StreamingContext streamingContext)
		{
			Serialization.writeObject(this, serializationInfo);
		}

		
		[PermissionSet(SecurityAction.Demand, XML = "<PermissionSet class=\"System.Security.PermissionSet\"\nversion=\"1\">\n<IPermission class=\"System.Security.Permissions.SecurityPermission, mscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089\"\nversion=\"1\"\nFlags=\"SerializationFormatter\"/>\n</PermissionSet>\n")]
		protected ScoreCachingSenone(SerializationInfo serializationInfo, StreamingContext streamingContext)
		{
			Serialization.readObject(this, serializationInfo);
		}

		private volatile ScoreCachingSenone.ScoreCache scoreCache;

		
		[SourceFile("ScoreCachingSenone.java")]
		
		internal sealed class ScoreCache : java.lang.Object
		{
			[LineNumberTable(new byte[]
			{
				159,
				166,
				111,
				103,
				104
			})]
			
			public ScoreCache(ScoreCachingSenone scoreCachingSenone, Data data, float num)
			{
				this.feature = data;
				this.score = num;
			}

			
			
			internal static Data access_000(ScoreCachingSenone.ScoreCache scoreCache)
			{
				return scoreCache.feature;
			}

			
			
			internal static float access$100(ScoreCachingSenone.ScoreCache scoreCache)
			{
				return scoreCache.score;
			}

			
			private Data feature;

			
			private float score;

			
			internal ScoreCachingSenone this$0 = scoreCachingSenone;
		}
	}
}
