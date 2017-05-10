using edu.cmu.sphinx.result;
using java.util;

namespace edu.cmu.sphinx.api
{
	public sealed class SpeechResult : java.lang.Object
	{		
		public SpeechResult(Result result)
		{
			this.result = result;
			if (result.toCreateLattice())
			{
				this.lattice = new Lattice(result);
				new LatticeOptimizer(this.lattice).optimize();
				this.lattice.computeNodePosteriors(1f);
			}
			else
			{
				this.lattice = null;
			}
		}
		
		public List getWords()
		{
			return (this.lattice == null) ? this.result.getTimedBestResult(false) : this.lattice.getWordResultPath();
		}
		
		public string getHypothesis()
		{
			return this.result.getBestResultNoFiller();
		}
		
		public Collection getNbest(int n)
		{
			if (this.lattice == null)
			{
				return new HashSet();
			}
			return new Nbest(this.lattice).getNbest(n);
		}
		public Lattice getLattice()
		{
			return this.lattice;
		}

		public Result getResult()
		{
			return this.result;
		}
		
		private Result result;
		
		private Lattice lattice;
	}
}
