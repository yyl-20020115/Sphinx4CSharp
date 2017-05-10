using System;

using edu.cmu.sphinx.linguist.acoustic;
using edu.cmu.sphinx.linguist.dictionary;
using IKVM.Attributes;
using ikvm.@internal;
using java.io;
using java.lang;
using java.util;

namespace edu.cmu.sphinx.linguist.lextree
{
	[SourceFile("HMMTree.java")]
	internal class Node : java.lang.Object
	{
		
		public static void __<clinit>()
		{
		}

		
		[LineNumberTable(new byte[]
		{
			74,
			104,
			172,
			127,
			0
		})]
		
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

		[LineNumberTable(new byte[]
		{
			85,
			112,
			103,
			124,
			127,
			1,
			102,
			98,
			145
		})]
		
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

		[LineNumberTable(new byte[]
		{
			50,
			103
		})]
		
		private Node getSuccessor(object obj)
		{
			Map successorMap = this.getSuccessorMap();
			return (Node)successorMap.get(obj);
		}

		[LineNumberTable(new byte[]
		{
			62,
			103,
			105
		})]
		
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

		[LineNumberTable(new byte[]
		{
			14,
			104,
			104,
			204
		})]
		
		internal Node(float num)
		{
			this.logUnigramProbability = num;
			Node.nodeCount++;
		}

		[LineNumberTable(new byte[]
		{
			97,
			159,
			63
		})]
		
		internal static void dumpNodeInfo()
		{
			PrintStream @out = java.lang.System.@out;
			StringBuilder stringBuilder = new StringBuilder().append("Nodes: ").append(Node.nodeCount).append(" successors ").append(Node.successorCount).append(" avg ");
			int num = Node.successorCount;
			int num2 = Node.nodeCount;
			@out.println(stringBuilder.append((num2 != -1) ? (num / num2) : (-num)).toString());
		}

		[LineNumberTable(new byte[]
		{
			111,
			98,
			104,
			99,
			105,
			143,
			106,
			136,
			130
		})]
		
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

		
		[LineNumberTable(new byte[]
		{
			160,
			72,
			98,
			109,
			99,
			109,
			99,
			105,
			137,
			138,
			106,
			136,
			130
		})]
		
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

		[LineNumberTable(new byte[]
		{
			160,
			92,
			104
		})]
		
		internal virtual void addSuccessor(WordNode wordNode)
		{
			this.putSuccessor(wordNode, wordNode);
		}

		[LineNumberTable(new byte[]
		{
			160,
			105,
			103,
			109,
			99,
			138,
			106,
			136,
			131
		})]
		
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

		[LineNumberTable(new byte[]
		{
			160,
			128,
			114,
			99,
			143,
			163
		})]
		
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

		[LineNumberTable(new byte[]
		{
			160,
			145,
			109,
			134
		})]
		
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

		
		static Node()
		{
		}

		private static int nodeCount;

		private static int successorCount;

		private object successors;

		private float logUnigramProbability;

		
		internal static bool assertionsDisabled = !ClassLiteral<Node>.Value.desiredAssertionStatus();
	}
}
