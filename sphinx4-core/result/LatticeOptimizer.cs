using ikvm.@internal;
using java.lang;
using java.util;

namespace edu.cmu.sphinx.result
{
	public class LatticeOptimizer : java.lang.Object
	{
		public LatticeOptimizer(Lattice lattice)
		{
			this.__lattice = lattice;
		}
	
		public virtual void optimize()
		{
			this.optimizeForward();
			this.optimizeBackward();
		}
		
		protected internal virtual void optimizeForward()
		{
			int num = 1;
			while (num != 0)
			{
				num = 0;
				Iterator iterator = this.__lattice.getCopyOfNodes().iterator();
				while (iterator.hasNext())
				{
					Node node = (Node)iterator.next();
					if (this.__lattice.hasNode(node))
					{
						num |= (this.optimizeNodeForward(node) ? 1 : 0);
					}
				}
			}
		}
		
		protected internal virtual void optimizeBackward()
		{
			int num = 1;
			while (num != 0)
			{
				num = 0;
				Iterator iterator = this.__lattice.getCopyOfNodes().iterator();
				while (iterator.hasNext())
				{
					Node node = (Node)iterator.next();
					if (this.__lattice.hasNode(node))
					{
						num |= (this.optimizeNodeBackward(node) ? 1 : 0);
					}
				}
			}
		}
		
		protected internal virtual bool optimizeNodeForward(Node n)
		{
			if (!LatticeOptimizer.assertionsDisabled && !this.__lattice.hasNode(n))
			{
				
				throw new AssertionError();
			}
			ArrayList arrayList = new ArrayList(n.getLeavingEdges());
			for (int i = 0; i < arrayList.size(); i++)
			{
				Edge edge = (Edge)arrayList.get(i);
				for (int j = i + 1; j < arrayList.size(); j++)
				{
					Edge edge2 = (Edge)arrayList.get(j);
					if (!LatticeOptimizer.assertionsDisabled && edge == edge2)
					{
						
						throw new AssertionError();
					}
					if (this.equivalentNodesForward(edge.getToNode(), edge2.getToNode()))
					{
						this.mergeNodesAndEdgesForward(edge, edge2);
						return true;
					}
				}
			}
			return false;
		}
		
		protected internal virtual bool equivalentNodesForward(Node n1, Node n2)
		{
			if (!LatticeOptimizer.assertionsDisabled && !this.__lattice.hasNode(n1))
			{
				
				throw new AssertionError();
			}
			if (!LatticeOptimizer.assertionsDisabled && !this.__lattice.hasNode(n2))
			{
				
				throw new AssertionError();
			}
			return this.equivalentNodeLabels(n1, n2) && n1.hasEquivalentEnteringEdges(n2);
		}
		
		protected internal virtual void mergeNodesAndEdgesForward(Edge e1, Edge e2)
		{
			if (!LatticeOptimizer.assertionsDisabled && !this.__lattice.hasNode(e1.getFromNode()))
			{
				
				throw new AssertionError();
			}
			if (!LatticeOptimizer.assertionsDisabled && !this.__lattice.hasEdge(e1))
			{
				
				throw new AssertionError();
			}
			if (!LatticeOptimizer.assertionsDisabled && !this.__lattice.hasEdge(e2))
			{
				
				throw new AssertionError();
			}
			if (!LatticeOptimizer.assertionsDisabled && e1.getFromNode() != e2.getFromNode())
			{
				
				throw new AssertionError();
			}
			Node toNode = e1.getToNode();
			Node toNode2 = e2.getToNode();
			if (!LatticeOptimizer.assertionsDisabled && !toNode.hasEquivalentEnteringEdges(toNode2))
			{
				
				throw new AssertionError();
			}
			if (!LatticeOptimizer.assertionsDisabled && !toNode.getWord().equals(toNode2.getWord()))
			{
				
				throw new AssertionError();
			}
			Iterator iterator = toNode2.getEnteringEdges().iterator();
			while (iterator.hasNext())
			{
				Edge edge = (Edge)iterator.next();
				Edge edge2 = toNode.getEdgeFromNode(edge.getFromNode());
				if (!LatticeOptimizer.assertionsDisabled && edge2 == null)
				{
					
					throw new AssertionError();
				}
				edge2.setAcousticScore(this.mergeAcousticScores(edge.getAcousticScore(), edge2.getAcousticScore()));
				edge2.setLMScore(this.mergeLanguageScores(edge.getLMScore(), edge2.getLMScore()));
			}
			iterator = toNode2.getLeavingEdges().iterator();
			while (iterator.hasNext())
			{
				Edge edge = (Edge)iterator.next();
				Edge edge2 = toNode.getEdgeToNode(edge.getToNode());
				if (edge2 == null)
				{
					this.__lattice.addEdge(toNode, edge.getToNode(), edge.getAcousticScore(), edge.getLMScore());
				}
				else
				{
					edge2.setAcousticScore(this.mergeAcousticScores(edge.getAcousticScore(), edge2.getAcousticScore()));
					edge2.setLMScore(this.mergeLanguageScores(edge.getLMScore(), edge2.getLMScore()));
				}
			}
			this.__lattice.removeNodeAndEdges(toNode2);
		}
		
		protected internal virtual bool equivalentNodeLabels(Node n1, Node n2)
		{
			return n1.getWord().equals(n2.getWord()) && n1.getBeginTime() == n2.getBeginTime() && n1.getEndTime() == n2.getEndTime();
		}
		
		private double mergeAcousticScores(double num, double num2)
		{
			return java.lang.Math.max(num, num2);
		}
		
		private double mergeLanguageScores(double num, double num2)
		{
			return java.lang.Math.max(num, num2);
		}
		
		protected internal virtual bool optimizeNodeBackward(Node n)
		{
			ArrayList arrayList = new ArrayList(n.getEnteringEdges());
			for (int i = 0; i < arrayList.size(); i++)
			{
				Edge edge = (Edge)arrayList.get(i);
				for (int j = i + 1; j < n.getEnteringEdges().size(); j++)
				{
					Edge edge2 = (Edge)arrayList.get(j);
					if (!LatticeOptimizer.assertionsDisabled && edge == edge2)
					{
						
						throw new AssertionError();
					}
					if (this.equivalentNodesBackward(edge.getFromNode(), edge2.getFromNode()))
					{
						this.mergeNodesAndEdgesBackward(edge, edge2);
						return true;
					}
				}
			}
			return false;
		}
		
		protected internal virtual bool equivalentNodesBackward(Node n1, Node n2)
		{
			if (!LatticeOptimizer.assertionsDisabled && !this.__lattice.hasNode(n1))
			{
				
				throw new AssertionError();
			}
			if (!LatticeOptimizer.assertionsDisabled && !this.__lattice.hasNode(n2))
			{
				
				throw new AssertionError();
			}
			return this.equivalentNodeLabels(n1, n2) && n1.hasEquivalentLeavingEdges(n2);
		}
		
		protected internal virtual void mergeNodesAndEdgesBackward(Edge e1, Edge e2)
		{
			if (!LatticeOptimizer.assertionsDisabled && !this.__lattice.hasNode(e1.getToNode()))
			{
				
				throw new AssertionError();
			}
			if (!LatticeOptimizer.assertionsDisabled && !this.__lattice.hasEdge(e1))
			{
				
				throw new AssertionError();
			}
			if (!LatticeOptimizer.assertionsDisabled && !this.__lattice.hasEdge(e2))
			{
				
				throw new AssertionError();
			}
			if (!LatticeOptimizer.assertionsDisabled && e1.getToNode() != e2.getToNode())
			{
				
				throw new AssertionError();
			}
			Node fromNode = e1.getFromNode();
			Node fromNode2 = e2.getFromNode();
			if (!LatticeOptimizer.assertionsDisabled && !fromNode.hasEquivalentLeavingEdges(fromNode2))
			{
				
				throw new AssertionError();
			}
			if (!LatticeOptimizer.assertionsDisabled && !fromNode.getWord().equals(fromNode2.getWord()))
			{
				
				throw new AssertionError();
			}
			Iterator iterator = fromNode2.getLeavingEdges().iterator();
			while (iterator.hasNext())
			{
				Edge edge = (Edge)iterator.next();
				Edge edge2 = fromNode.getEdgeToNode(edge.getToNode());
				if (!LatticeOptimizer.assertionsDisabled && edge2 == null)
				{
					
					throw new AssertionError();
				}
				edge2.setAcousticScore(this.mergeAcousticScores(edge.getAcousticScore(), edge2.getAcousticScore()));
				edge2.setLMScore(this.mergeLanguageScores(edge.getLMScore(), edge2.getLMScore()));
			}
			iterator = fromNode2.getEnteringEdges().iterator();
			while (iterator.hasNext())
			{
				Edge edge = (Edge)iterator.next();
				Edge edge2 = fromNode.getEdgeFromNode(edge.getFromNode());
				if (edge2 == null)
				{
					this.__lattice.addEdge(edge.getFromNode(), fromNode, edge.getAcousticScore(), edge.getLMScore());
				}
				else
				{
					edge2.setAcousticScore(this.mergeAcousticScores(edge.getAcousticScore(), edge2.getAcousticScore()));
					edge2.setLMScore(this.mergeLanguageScores(edge.getLMScore(), edge2.getLMScore()));
				}
			}
			this.__lattice.removeNodeAndEdges(fromNode2);
		}
		
		protected internal virtual void removeHangingNodes()
		{
			Iterator iterator = this.__lattice.getCopyOfNodes().iterator();
			while (iterator.hasNext())
			{
				Node node = (Node)iterator.next();
				if (this.__lattice.hasNode(node))
				{
					if (node != this.__lattice.getInitialNode())
					{
						if (node != this.__lattice.getTerminalNode())
						{
							if (node.getLeavingEdges().isEmpty() || node.getEnteringEdges().isEmpty())
							{
								this.__lattice.removeNodeAndEdges(node);
								this.removeHangingNodes();
								return;
							}
						}
					}
				}
			}
		}
		
		protected internal Lattice lattice
		{
			
			get
			{
				return this.__lattice;
			}
			
			private set
			{
				this.__lattice = value;
			}
		}

		internal Lattice __lattice;

		internal static bool assertionsDisabled = !ClassLiteral<LatticeOptimizer>.Value.desiredAssertionStatus();
	}
}
