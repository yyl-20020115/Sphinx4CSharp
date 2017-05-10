using System;

using edu.cmu.sphinx.linguist.dictionary;
using edu.cmu.sphinx.util;
using IKVM.Attributes;
using ikvm.@internal;
using java.io;
using java.lang;
using java.util;

namespace edu.cmu.sphinx.result
{
	public class Node : java.lang.Object
	{
		
		public static void __<clinit>()
		{
		}

		public virtual Word getWord()
		{
			return this.word;
		}

		
		
		public override bool equals(object obj)
		{
			return obj is Node && java.lang.String.instancehelper_equals(this.id, ((Node)obj).getId());
		}

		public virtual string getId()
		{
			return this.id;
		}

		[LineNumberTable(new byte[]
		{
			161,
			243,
			102,
			120,
			120,
			120,
			116,
			235,
			59
		})]
		
		public virtual bool isEquivalent(Node other)
		{
			return java.lang.String.instancehelper_equals(this.word.getSpelling(), other.getWord().getSpelling()) && this.getEnteringEdges().size() == other.getEnteringEdges().size() && this.getLeavingEdges().size() == other.getLeavingEdges().size() && this.getBeginTime() == other.getBeginTime() && this.endTime == other.getEndTime();
		}

		public virtual void setPosterior(double posterior)
		{
			this.posterior = posterior;
		}

		[LineNumberTable(new byte[]
		{
			35,
			232,
			25,
			104,
			232,
			76,
			107,
			107,
			236,
			89,
			103,
			103,
			103,
			136,
			157,
			112,
			112,
			112
		})]
		
		protected internal Node(string id, Word word, long beginTime, long endTime)
		{
			this.beginTime = -1L;
			this.endTime = -1L;
			this.enteringEdges = new ArrayList();
			this.leavingEdges = new ArrayList();
			Node.nodeCount++;
			this.id = id;
			this.word = word;
			this.beginTime = beginTime;
			this.endTime = endTime;
			if (!Node.assertionsDisabled && beginTime > endTime && endTime >= 0L)
			{
				
				throw new AssertionError();
			}
			this.forwardScore = -3.4028234663852886E+38;
			this.backwardScore = -3.4028234663852886E+38;
			this.posterior = -3.4028234663852886E+38;
		}

		[LineNumberTable(new byte[]
		{
			161,
			52,
			103,
			103,
			108,
			140,
			127,
			18,
			108
		})]
		
		internal static void load(Lattice lattice, StringTokenizer stringTokenizer)
		{
			string text = stringTokenizer.nextToken();
			string text2 = stringTokenizer.nextToken();
			long num = Long.parseLong(stringTokenizer.nextToken());
			long num2 = Long.parseLong(stringTokenizer.nextToken());
			Word.__<clinit>();
			Word word = new Word(text2, new Pronunciation[0], java.lang.String.instancehelper_startsWith(text2, "<") || java.lang.String.instancehelper_startsWith(text2, "["));
			lattice.addNode(text, word, num, num2);
		}

		
		public virtual Collection getLeavingEdges()
		{
			return this.leavingEdges;
		}

		public virtual long getEndTime()
		{
			return this.endTime;
		}

		[LineNumberTable(new byte[]
		{
			160,
			192,
			106,
			134
		})]
		
		public virtual long getBeginTime()
		{
			if (this.beginTime == -1L)
			{
				this.calculateBeginTime();
			}
			return this.beginTime;
		}

		[LineNumberTable(new byte[]
		{
			160,
			228,
			123,
			103
		})]
		
		public virtual void setEndTime(long endTime)
		{
			if (!Node.assertionsDisabled && this.beginTime > endTime)
			{
				
				throw new AssertionError();
			}
			this.endTime = endTime;
		}

		[LineNumberTable(new byte[]
		{
			160,
			142,
			109
		})]
		
		protected internal virtual void addLeavingEdge(Edge e)
		{
			this.leavingEdges.add(e);
		}

		[LineNumberTable(new byte[]
		{
			160,
			132,
			109
		})]
		
		protected internal virtual void addEnteringEdge(Edge e)
		{
			this.enteringEdges.add(e);
		}

		[Throws(new string[]
		{
			"java.io.IOException"
		})]
		[LineNumberTable(new byte[]
		{
			161,
			6,
			108,
			113,
			134,
			127,
			7,
			63,
			46,
			165
		})]
		
		internal virtual void dumpAISee(FileWriter fileWriter)
		{
			string text = java.lang.String.valueOf(this.getPosterior());
			if (this.getPosterior() == -3.4028234663852886E+38)
			{
				text = "log zero";
			}
			fileWriter.write(new StringBuilder().append("node: { title: \"").append(this.id).append("\" label: \"").append(this.getWord()).append('[').append(this.getBeginTime()).append(',').append(this.getEndTime()).append(" p:").append(text).append("]\" }\n").toString());
		}

		[Throws(new string[]
		{
			"java.io.IOException"
		})]
		[LineNumberTable(new byte[]
		{
			161,
			22,
			108,
			113,
			134,
			127,
			55,
			127,
			32
		})]
		
		public virtual void dumpDot(FileWriter f)
		{
			string text = java.lang.String.valueOf(this.getPosterior());
			if (this.getPosterior() == -3.4028234663852886E+38)
			{
				text = "log zero";
			}
			string text2 = new StringBuilder().append(this.getWord().toString()).append('[').append(this.getBeginTime()).append(',').append(this.getEndTime()).append(" p:").append(text).append(']').toString();
			f.write(new StringBuilder().append("\tnode").append(this.id).append(" [ label=\"").append(text2).append("\" ]\n").toString());
		}

		[Throws(new string[]
		{
			"java.io.IOException"
		})]
		[LineNumberTable(new byte[]
		{
			161,
			37,
			191,
			27,
			255,
			2,
			61,
			197
		})]
		
		internal virtual void dump(PrintWriter printWriter)
		{
			printWriter.println(new StringBuilder().append("node: ").append(this.id).append(' ').append(this.word.getSpelling()).append(' ').append(this.getBeginTime()).append(' ').append(this.getEndTime()).toString());
		}

		[LineNumberTable(new byte[]
		{
			160,
			152,
			109
		})]
		
		protected internal virtual void removeEnteringEdge(Edge e)
		{
			this.enteringEdges.remove(e);
		}

		
		public virtual Collection getEnteringEdges()
		{
			return this.enteringEdges;
		}

		[LineNumberTable(new byte[]
		{
			160,
			162,
			109
		})]
		
		public virtual void removeLeavingEdge(Edge e)
		{
			this.leavingEdges.remove(e);
		}

		
		
		protected internal virtual bool hasEdgeFromNode(Node n)
		{
			return this.getEdgeFromNode(n) != null;
		}

		
		
		protected internal virtual bool hasEdgeToNode(Node n)
		{
			return this.getEdgeToNode(n) != null;
		}

		public virtual void setForwardScore(double forwardScore)
		{
			this.forwardScore = forwardScore;
		}

		public virtual void setViterbiScore(double viterbiScore)
		{
			this.viterbiScore = viterbiScore;
		}

		public virtual double getForwardScore()
		{
			return this.forwardScore;
		}

		public virtual double getViterbiScore()
		{
			return this.viterbiScore;
		}

		public virtual Node getBestPredecessor()
		{
			return this.bestPredecessor;
		}

		public virtual void setBestPredecessor(Node bestPredecessor)
		{
			this.bestPredecessor = bestPredecessor;
		}

		public virtual void setBackwardScore(double backwardScore)
		{
			this.backwardScore = backwardScore;
		}

		public virtual double getBackwardScore()
		{
			return this.backwardScore;
		}

		
		
		
		public virtual Collection getCopyOfLeavingEdges()
		{
			return new ArrayList(this.leavingEdges);
		}

		[LineNumberTable(new byte[]
		{
			162,
			4,
			127,
			1,
			105,
			130,
			98
		})]
		
		public virtual Edge findEquivalentLeavingEdge(Edge edge)
		{
			Iterator iterator = this.leavingEdges.iterator();
			while (iterator.hasNext())
			{
				Edge edge2 = (Edge)iterator.next();
				if (edge2.isEquivalent(edge))
				{
					return edge2;
				}
			}
			return null;
		}

		[LineNumberTable(new byte[]
		{
			120,
			120,
			130,
			127,
			1,
			103,
			105,
			130,
			98
		})]
		
		protected internal virtual bool hasEquivalentEnteringEdges(Node n)
		{
			if (this.enteringEdges.size() != n.getEnteringEdges().size())
			{
				return false;
			}
			Iterator iterator = this.enteringEdges.iterator();
			while (iterator.hasNext())
			{
				Edge edge = (Edge)iterator.next();
				Node fromNode = edge.getFromNode();
				if (!n.hasEdgeFromNode(fromNode))
				{
					return false;
				}
			}
			return true;
		}

		[LineNumberTable(new byte[]
		{
			104,
			127,
			1,
			105,
			130,
			98
		})]
		
		public virtual Edge getEdgeFromNode(Node n)
		{
			Iterator iterator = this.enteringEdges.iterator();
			while (iterator.hasNext())
			{
				Edge edge = (Edge)iterator.next();
				if (edge.getFromNode() == n)
				{
					return edge;
				}
			}
			return null;
		}

		[LineNumberTable(new byte[]
		{
			77,
			127,
			1,
			105,
			130,
			98
		})]
		
		public virtual Edge getEdgeToNode(Node n)
		{
			Iterator iterator = this.leavingEdges.iterator();
			while (iterator.hasNext())
			{
				Edge edge = (Edge)iterator.next();
				if (edge.getToNode() == n)
				{
					return edge;
				}
			}
			return null;
		}

		[LineNumberTable(new byte[]
		{
			160,
			76,
			120,
			130,
			127,
			1,
			103,
			105,
			130,
			98
		})]
		
		public virtual bool hasEquivalentLeavingEdges(Node n)
		{
			if (this.leavingEdges.size() != n.getLeavingEdges().size())
			{
				return false;
			}
			Iterator iterator = this.leavingEdges.iterator();
			while (iterator.hasNext())
			{
				Edge edge = (Edge)iterator.next();
				Node toNode = edge.getToNode();
				if (!n.hasEdgeToNode(toNode))
				{
					return false;
				}
			}
			return true;
		}

		
		
		protected internal static string getNextNodeId()
		{
			return Integer.toString(Node.nodeCount);
		}

		[LineNumberTable(new byte[]
		{
			161,
			147,
			104,
			127,
			1,
			115,
			145,
			98
		})]
		
		private void calculateBeginTime()
		{
			this.beginTime = 0L;
			Iterator iterator = this.enteringEdges.iterator();
			while (iterator.hasNext())
			{
				Edge edge = (Edge)iterator.next();
				if (edge.getFromNode().getEndTime() > this.beginTime)
				{
					this.beginTime = edge.getFromNode().getEndTime();
				}
			}
		}

		public virtual double getPosterior()
		{
			return this.posterior;
		}

		[LineNumberTable(new byte[]
		{
			161,
			177,
			127,
			1,
			110,
			130,
			109,
			103,
			98
		})]
		
		protected internal virtual void cacheDescendantsHelper(Node n)
		{
			Iterator iterator = n.getChildNodes().iterator();
			while (iterator.hasNext())
			{
				Node node = (Node)iterator.next();
				if (!this.descendants.contains(node))
				{
					this.descendants.add(node);
					this.cacheDescendantsHelper(node);
				}
			}
		}

		
		[LineNumberTable(new byte[]
		{
			161,
			162,
			102,
			127,
			1,
			109,
			98
		})]
		
		public virtual List getChildNodes()
		{
			LinkedList linkedList = new LinkedList();
			Iterator iterator = this.leavingEdges.iterator();
			while (iterator.hasNext())
			{
				Edge edge = (Edge)iterator.next();
				linkedList.add(edge.getToNode());
			}
			return linkedList;
		}

		
		[LineNumberTable(new byte[]
		{
			161,
			188,
			123,
			105,
			130,
			104,
			105,
			130,
			112,
			130,
			98
		})]
		
		protected internal virtual bool isAncestorHelper(List children, Node node, Set seenNodes)
		{
			Iterator iterator = children.iterator();
			while (iterator.hasNext())
			{
				Node node2 = (Node)iterator.next();
				if (!seenNodes.contains(node2))
				{
					seenNodes.add(node2);
					if (node2.equals(node))
					{
						return true;
					}
					if (this.isAncestorHelper(node2.getChildNodes(), node, seenNodes))
					{
						return true;
					}
				}
			}
			return false;
		}

		[LineNumberTable(new byte[]
		{
			161,
			211,
			104,
			141,
			105,
			130,
			102,
			104
		})]
		
		public virtual bool isAncestorOf(Node node)
		{
			if (this.descendants != null)
			{
				return this.descendants.contains(node);
			}
			if (this.equals(node))
			{
				return true;
			}
			HashSet hashSet = new HashSet();
			hashSet.add(this);
			return this.isAncestorHelper(this.getChildNodes(), node, hashSet);
		}

		[LineNumberTable(new byte[]
		{
			23,
			110
		})]
		
		protected internal Node(Word word, long beginTime, long endTime) : this(Node.getNextNodeId(), word, beginTime, endTime)
		{
		}

		
		
		
		public virtual Collection getCopyOfEnteringEdges()
		{
			return new ArrayList(this.enteringEdges);
		}

		[LineNumberTable(new byte[]
		{
			160,
			206,
			123,
			103
		})]
		
		public virtual void setBeginTime(long beginTime)
		{
			if (!Node.assertionsDisabled && beginTime > this.endTime)
			{
				
				throw new AssertionError();
			}
			this.beginTime = beginTime;
		}

		
		
		public virtual TimeFrame getTimeFrame()
		{
			TimeFrame.__<clinit>();
			return new TimeFrame(this.getBeginTime(), this.getEndTime());
		}

		[LineNumberTable(new byte[]
		{
			160,
			250,
			127,
			26,
			54
		})]
		
		public override string toString()
		{
			return new StringBuilder().append("Node(").append(this.word.getSpelling()).append(',').append(this.getBeginTime()).append('|').append(this.getEndTime()).append(')').toString();
		}

		
		
		public override int hashCode()
		{
			return java.lang.String.instancehelper_hashCode(this.id);
		}

		[LineNumberTable(new byte[]
		{
			161,
			171,
			107,
			103
		})]
		
		protected internal virtual void cacheDescendants()
		{
			this.descendants = new HashSet();
			this.cacheDescendantsHelper(this);
		}

		
		
		public virtual bool hasAncestralRelationship(Node node)
		{
			return this.isAncestorOf(node) || node.isAncestorOf(this);
		}

		
		static Node()
		{
		}

		private static int nodeCount;

		private string id;

		private Word word;

		private long beginTime;

		private long endTime;

		
		private List enteringEdges;

		
		private List leavingEdges;

		private double forwardScore;

		private double backwardScore;

		private double posterior;

		private Node bestPredecessor;

		private double viterbiScore;

		
		private Set descendants;

		
		internal static bool assertionsDisabled = !ClassLiteral<Node>.Value.desiredAssertionStatus();
	}
}
