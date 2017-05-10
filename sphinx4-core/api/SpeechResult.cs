using System;

using edu.cmu.sphinx.result;
using IKVM.Attributes;
using java.lang;
using java.util;

namespace edu.cmu.sphinx.api
{
	public sealed class SpeechResult : java.lang.Object
	{
		[LineNumberTable(new byte[]
		{
			159,
			177,
			104,
			103,
			104,
			108,
			117,
			146,
			103
		})]
		
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

		
		[LineNumberTable(new byte[]
		{
			19,
			104,
			102
		})]
		
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
