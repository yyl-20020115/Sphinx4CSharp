using System;
using System.Runtime.Serialization;
using System.Security.Permissions;
using edu.cmu.sphinx.decoder.scorer;
using edu.cmu.sphinx.frontend;
using edu.cmu.sphinx.linguist.acoustic;
using java.io;

namespace edu.cmu.sphinx.linguist.flat
{
	[Serializable]
	public class HMMStateState : SentenceHMMState, Serializable.__Interface, HMMSearchState, SearchState, ScoreProvider
	{
		public HMMStateState(SentenceHMMState parent, HMMState hmmState) : base("S", parent, hmmState.getState())
		{
			this.hmmState = hmmState;
			this._isEmitting = hmmState.isEmitting();
		}

		public virtual HMMState getHMMState()
		{
			return this.hmmState;
		}

		public override bool isEmitting()
		{
			return this._isEmitting;
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
			return (!this._isEmitting) ? 0 : 6;
		}
				
		public virtual float[] getComponentScore(Data feature)
		{
			return this.hmmState.calculateComponentScore(feature);
		}

		public static implicit operator Serializable(HMMStateState _ref)
		{
			Serializable result = Serializable.Cast(_ref);
			return result;
		}
		
		object SearchState.getLexState()
		{
			return this.getLexState();
		}
		
		[PermissionSet(SecurityAction.Demand, XML = "<PermissionSet class=\"System.Security.PermissionSet\"\nversion=\"1\">\n<IPermission class=\"System.Security.Permissions.SecurityPermission, mscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089\"\nversion=\"1\"\nFlags=\"SerializationFormatter\"/>\n</PermissionSet>\n")]
		protected HMMStateState(SerializationInfo serializationInfo, StreamingContext streamingContext) : base(serializationInfo, streamingContext)
		{
		}

		private HMMState hmmState;

		private bool _isEmitting;
	}
}
