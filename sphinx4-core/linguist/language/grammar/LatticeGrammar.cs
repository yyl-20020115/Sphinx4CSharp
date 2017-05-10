using System;

using edu.cmu.sphinx.linguist.dictionary;
using edu.cmu.sphinx.result;
using edu.cmu.sphinx.util.props;
using IKVM.Attributes;
using java.lang;
using java.util;

namespace edu.cmu.sphinx.linguist.language.grammar
{
	public class LatticeGrammar : Grammar
	{
		
		public new static void __<clinit>()
		{
		}

		[LineNumberTable(new byte[]
		{
			159,
			135,
			170,
			111,
			103
		})]
		
		public LatticeGrammar(Lattice lattice, bool showGrammar, bool optimizeGrammar, bool addSilenceWords, bool addFillerWords, Dictionary dictionary) : base(showGrammar, optimizeGrammar, addSilenceWords, addFillerWords, dictionary)
		{
			this.lattice = lattice;
		}

		[LineNumberTable(new byte[]
		{
			159,
			177,
			134
		})]
		
		public LatticeGrammar()
		{
		}

		[Throws(new string[]
		{
			"edu.cmu.sphinx.util.props.PropertyException"
		})]
		[LineNumberTable(new byte[]
		{
			159,
			188,
			103
		})]
		
		public override void newProperties(PropertySheet ps)
		{
			base.newProperties(ps);
		}

		[Throws(new string[]
		{
			"java.io.IOException"
		})]
		[LineNumberTable(new byte[]
		{
			8,
			104,
			172,
			98,
			102,
			127,
			9,
			109,
			106,
			115,
			99,
			115,
			104,
			106,
			101,
			99,
			176,
			127,
			7,
			106,
			116,
			116,
			107,
			130
		})]
		
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

		[Throws(new string[]
		{
			"java.io.IOException"
		})]
		[LineNumberTable(new byte[]
		{
			39,
			103,
			166
		})]
		
		public virtual void setLattice(Lattice lattice)
		{
			this.lattice = lattice;
			this.allocate();
		}

		
		static LatticeGrammar()
		{
			Grammar.__<clinit>();
		}

		public Lattice lattice;
	}
}
