using System;

using System.Runtime.Serialization;
using System.Security.Permissions;
using edu.cmu.sphinx.decoder.scorer;
using edu.cmu.sphinx.frontend;
using edu.cmu.sphinx.linguist.acoustic;
using IKVM.Attributes;
using java.io;

namespace edu.cmu.sphinx.linguist.flat
{
	[Implements(new string[]
	{
		"java.io.Serializable",
		"edu.cmu.sphinx.linguist.HMMSearchState",
		"edu.cmu.sphinx.decoder.scorer.ScoreProvider"
	})]
	[Serializable]
	public class HMMStateState : SentenceHMMState, Serializable.__Interface, HMMSearchState, SearchState, ScoreProvider
	{
		
		public new static void __<clinit>()
		{
		}

		[LineNumberTable(new byte[]
		{
			159,
			181,
			116,
			103,
			108
		})]
		
		public HMMStateState(SentenceHMMState parent, HMMState hmmState) : base("S", parent, hmmState.getState())
		{
			this.hmmState = hmmState;
			this.isEmitting = hmmState.isEmitting();
		}

		public virtual HMMState getHMMState()
		{
			return this.hmmState;
		}

		public override bool isEmitting()
		{
			return this.isEmitting;
		}

		public override string getTypeLabel()
		{
			return "HMM";
		}

		
		
		public virtual float getScore(Data feature)
		{
			return this.hmmState.getScore(feature);
		}

		public override int getOrder()
		{
			return (!this.isEmitting) ? 0 : 6;
		}

		
		
		public virtual float[] getComponentScore(Data feature)
		{
			return this.hmmState.calculateComponentScore(feature);
		}

		
		static HMMStateState()
		{
			SentenceHMMState.__<clinit>();
		}

		
		public static implicit operator Serializable(HMMStateState _ref)
		{
			Serializable result;
			result.__ref = _ref;
			return result;
		}

		
		object SearchState.Object;getLexState()
		{
			return this.<bridge>getLexState();
		}

		
		[PermissionSet(SecurityAction.Demand, XML = "<PermissionSet class=\"System.Security.PermissionSet\"\nversion=\"1\">\n<IPermission class=\"System.Security.Permissions.SecurityPermission, mscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089\"\nversion=\"1\"\nFlags=\"SerializationFormatter\"/>\n</PermissionSet>\n")]
		protected HMMStateState(SerializationInfo serializationInfo, StreamingContext streamingContext) : base(serializationInfo, streamingContext)
		{
		}

		private HMMState hmmState;

		private new bool isEmitting;
	}
}
