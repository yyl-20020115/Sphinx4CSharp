namespace edu.cmu.sphinx.linguist.acoustic.tiedstate.HTK
{
	public class HMMState : java.lang.Object//, acoustic.HMMState
	{
		public HMMState(GMMDiag g, Lab l)
		{
			this.gmmidx = -1;
			this.lab = l;
			this.__gmm = g;
		}

		public virtual Lab getLab()
		{
			return this.lab;
		}		
		
		public virtual float getLogLike()
		{
			return this.__gmm.getLogLike();
		}

		public virtual void setLab(Lab l)
		{
			this.lab = l;
		}
	
		public GMMDiag gmm
		{
			
			get
			{
				return this.__gmm;
			}
			
			private set
			{
				this.__gmm = value;
			}
		}

		public int gmmidx;

		public Lab lab;

		internal GMMDiag __gmm;
	}
}
