using edu.cmu.sphinx.linguist.acoustic;
using edu.cmu.sphinx.linguist.dictionary;
using ikvm.@internal;
using java.io;
using java.lang;
using java.util;

namespace edu.cmu.sphinx.linguist.lextree
{
	public class Node : java.lang.Object
	{
		public virtual Map getSuccessorMap()
		{
			if (this.successors == null)
			{
				this.successors = new HashMap(4);
			}
			if (!Node.assertionsDisabled && !(this.successors is Map))
			{
				
				throw new AssertionError();
			}
			return (Map)this.successors;
		}
		
		internal virtual void freeze()
		{
			if (this.successors is Map)
			{
				Map successorMap = this.getSuccessorMap();
				this.successors = successorMap.values().toArray(new Node[successorMap.size()]);
				Iterator iterator = successorMap.values().iterator();
				while (iterator.hasNext())
				{
					Node node = (Node)iterator.next();
					node.freeze();
				}
				Node.successorCount += successorMap.size();
			}
		}
		
		private Node getSuccessor(object obj)
		{
			Map successorMap = this.getSuccessorMap();
			return (Node)successorMap.get(obj);
		}
		
		internal virtual void putSuccessor(object obj, Node node)
		{
			Map successorMap = this.getSuccessorMap();
			successorMap.put(obj, node);
		}

		public virtual float getUnigramProbability()
		{
			return this.logUnigramProbability;
		}

		public virtual void setUnigramProbability(float num)
		{
			this.logUnigramProbability = num;
		}
		
		internal Node(float num)
		{
			this.logUnigramProbability = num;
			Node.nodeCount++;
		}
		
		internal static void dumpNodeInfo()
		{
			PrintStream @out = java.lang.System.@out;
			StringBuilder stringBuilder = new StringBuilder().append("Nodes: ").append(Node.nodeCount).append(" successors ").append(Node.successorCount).append(" avg ");
			int num = Node.successorCount;
			int num2 = Node.nodeCount;
			@out.println(stringBuilder.append((num2 != -1) ? (num / num2) : (-num)).toString());
		}
		
		internal virtual Node addSuccessor(HMM hmm, float num)
		{
			Node successor = this.getSuccessor(hmm);
			Node node;
			if (successor == null)
			{
				node = new HMMNode(hmm, num);
				this.putSuccessor(hmm, (HMMNode)node);
			}
			else
			{
				if (successor.getUnigramProbability() < num)
				{
					successor.setUnigramProbability(num);
				}
				node = successor;
			}
			return node;
		}
		
		internal virtual WordNode addSuccessor(Pronunciation pronunciation, float num, Map map)
		{
			WordNode wordNode = (WordNode)this.getSuccessor(pronunciation);
			WordNode wordNode2;
			if (wordNode == null)
			{
				wordNode2 = (WordNode)map.get(pronunciation);
				if (wordNode2 == null)
				{
					wordNode2 = new WordNode(pronunciation, num);
					map.put(pronunciation, wordNode2);
				}
				this.putSuccessor(pronunciation, wordNode2);
			}
			else
			{
				if (wordNode.getUnigramProbability() < num)
				{
					wordNode.setUnigramProbability(num);
				}
				wordNode2 = wordNode;
			}
			return wordNode2;
		}
		
		internal virtual void addSuccessor(WordNode wordNode)
		{
			this.putSuccessor(wordNode, wordNode);
		}
		
		internal virtual EndNode addSuccessor(EndNode endNode, float num)
		{
			Unit baseUnit = endNode.getBaseUnit();
			EndNode endNode2 = (EndNode)this.getSuccessor(baseUnit);
			if (endNode2 == null)
			{
				this.putSuccessor(baseUnit, endNode);
			}
			else
			{
				if (endNode2.getUnigramProbability() < num)
				{
					endNode2.setUnigramProbability(num);
				}
				endNode = endNode2;
			}
			return endNode;
		}
		
		internal virtual UnitNode addSuccessor(UnitNode unitNode)
		{
			UnitNode unitNode2 = (UnitNode)this.getSuccessor(unitNode.getKey());
			if (unitNode2 == null)
			{
				this.putSuccessor(unitNode.getKey(), unitNode);
			}
			else
			{
				unitNode = unitNode2;
			}
			return unitNode;
		}
		
		internal virtual Node[] getSuccessors()
		{
			if (this.successors is Map)
			{
				this.freeze();
			}
			return (Node[])((Node[])this.successors);
		}

		public override string toString()
		{
			return "Node ";
		}

		private static int nodeCount;

		private static int successorCount;

		private object successors;

		private float logUnigramProbability;
		
		internal static bool assertionsDisabled = !ClassLiteral<Node>.Value.desiredAssertionStatus();
	}
}
