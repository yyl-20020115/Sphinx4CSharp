using System;

using IKVM.Attributes;
using java.lang;
using java.util;

namespace edu.cmu.sphinx.result
{
	public class Nbest : java.lang.Object
	{
		[LineNumberTable(new byte[]
		{
			159,
			167,
			104,
			103
		})]
		
		public Nbest(Lattice lattice)
		{
			this.lattice = lattice;
		}

		
		[LineNumberTable(new byte[]
		{
			5,
			102,
			167,
			159,
			7,
			120,
			108,
			120,
			109,
			162,
			127,
			10,
			137,
			104,
			145,
			141,
			139,
			144,
			104,
			133,
			133
		})]
		
		public virtual Collection getNbest(int n)
		{
			HashSet hashSet = new HashSet();
			BoundedPriorityQueue boundedPriorityQueue = new BoundedPriorityQueue(n);
			boundedPriorityQueue.add(new Nbest.NBestPath(this, "<s>", this.lattice.getInitialNode(), (double)0f, (double)0f));
			while (hashSet.size() < n && boundedPriorityQueue.size() > 0)
			{
				Nbest.NBestPath nbestPath = (Nbest.NBestPath)boundedPriorityQueue.poll();
				if (nbestPath.node.equals(this.lattice.terminalNode))
				{
					hashSet.add(nbestPath.path);
				}
				else
				{
					Iterator iterator = nbestPath.node.getLeavingEdges().iterator();
					while (iterator.hasNext())
					{
						Edge edge = (Edge)iterator.next();
						Node toNode = edge.getToNode();
						double num = nbestPath.forwardScore + edge.getAcousticScore() + edge.getLMScore();
						double num2 = num + toNode.getBackwardScore();
						string newPathString = this.getNewPathString(nbestPath, toNode);
						Nbest.NBestPath item = new Nbest.NBestPath(this, newPathString, toNode, num2, num);
						boundedPriorityQueue.add(item);
					}
				}
			}
			return hashSet;
		}

		[LineNumberTable(new byte[]
		{
			40,
			109,
			127,
			3,
			109,
			137,
			127,
			12
		})]
		
		private string getNewPathString(Nbest.NBestPath nbestPath, Node node)
		{
			string result;
			if (node.getWord().isSentenceEndWord())
			{
				result = new StringBuilder().append(nbestPath.path).append(" </s>").toString();
			}
			else if (node.getWord().isFiller())
			{
				result = nbestPath.path;
			}
			else
			{
				result = new StringBuilder().append(nbestPath.path).append(" ").append(node.getWord()).toString();
			}
			return result;
		}

		
		[LineNumberTable(new byte[]
		{
			51,
			106,
			123,
			107,
			98
		})]
		
		private void printQueue(BoundedPriorityQueue boundedPriorityQueue)
		{
			java.lang.System.@out.println();
			Iterator iterator = boundedPriorityQueue.iterator();
			while (iterator.hasNext())
			{
				Nbest.NBestPath nbestPath = (Nbest.NBestPath)iterator.next();
				java.lang.System.@out.println(nbestPath);
			}
		}

		protected internal Lattice lattice;

		
		[Implements(new string[]
		{
			"java.lang.Comparable"
		})]
		
		[SourceFile("Nbest.java")]
		
		internal sealed class NBestPath : java.lang.Object, Comparable
		{
			
			
			public int compareTo(Nbest.NBestPath nbestPath)
			{
				return Double.compare(this.score, nbestPath.score);
			}

			[LineNumberTable(new byte[]
			{
				159,
				178,
				103,
				104,
				103,
				103,
				106,
				106
			})]
			
			public NBestPath(Nbest nbest, string text, Node node, double num, double num2)
			{
				this.path = text;
				this.node = node;
				this.score = num;
				this.forwardScore = num2;
			}

			
			
			public override string toString()
			{
				return new StringBuilder().append(this.path).append(" [").append(this.score).append(',').append(this.forwardScore).append(']').toString();
			}

			
			
			
			public int compareTo(object obj)
			{
				return this.compareTo((Nbest.NBestPath)obj);
			}

			
			int IComparable.Object;)IcompareTo(object obj)
			{
				return this.compareTo(obj);
			}

			internal string path;

			internal Node node;

			internal double score;

			internal double forwardScore;

			
			internal Nbest this$0 = nbest;
		}
	}
}
