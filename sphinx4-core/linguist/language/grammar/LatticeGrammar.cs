using edu.cmu.sphinx.result;
using edu.cmu.sphinx.util.props;
using java.lang;
using java.util;

namespace edu.cmu.sphinx.linguist.language.grammar
{
	public class LatticeGrammar : Grammar
	{
		public LatticeGrammar(Lattice lattice, bool showGrammar, bool optimizeGrammar, bool addSilenceWords, bool addFillerWords, dictionary.Dictionary dictionary) : base(showGrammar, optimizeGrammar, addSilenceWords, addFillerWords, dictionary)
		{
			this.lattice = lattice;
		}
		
		public LatticeGrammar()
		{
		}
		
		public override void newProperties(PropertySheet ps)
		{
			base.newProperties(ps);
		}
		
		protected internal override GrammarNode createGrammar()
		{
			if (this.lattice == null)
			{
				return this.createGrammarNode("<s>");
			}
			GrammarNode grammarNode = null;
			HashMap hashMap = new HashMap();
			Iterator iterator = this.lattice.getNodes().iterator();
			while (iterator.hasNext())
			{
				Node node = (Node)iterator.next();
				string word = node.getWord().toString();
				GrammarNode grammarNode2 = this.createGrammarNode(word);
				if (node.equals(this.lattice.getInitialNode()))
				{
					grammarNode = grammarNode2;
				}
				if (node.equals(this.lattice.getTerminalNode()))
				{
					grammarNode2.setFinalNode(true);
				}
				hashMap.put(node, grammarNode2);
			}
			if (grammarNode == null)
			{
				string text = "No lattice start found";
				
				throw new Error(text);
			}
			iterator = this.lattice.getEdges().iterator();
			while (iterator.hasNext())
			{
				Edge edge = (Edge)iterator.next();
				float logProbability = (float)edge.getLMScore();
				GrammarNode grammarNode2 = (GrammarNode)hashMap.get(edge.getFromNode());
				GrammarNode node2 = (GrammarNode)hashMap.get(edge.getToNode());
				grammarNode2.add(node2, logProbability);
			}
			return grammarNode;
		}
		
		public virtual void setLattice(Lattice lattice)
		{
			this.lattice = lattice;
			this.allocate();
		}

		public Lattice lattice;
	}
}
