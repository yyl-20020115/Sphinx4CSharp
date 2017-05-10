using System;

using IKVM.Attributes;
using java.io;
using java.lang;
using java.util;

namespace edu.cmu.sphinx.result
{
	public class Edge : java.lang.Object
	{
		public virtual double getLMScore()
		{
			return this.lmScore;
		}

		public virtual Node getFromNode()
		{
			return this.__fromNode;
		}

		public virtual Node getToNode()
		{
			return this.__toNode;
		}

		public virtual double getAcousticScore()
		{
			return this.acousticScore;
		}

		[LineNumberTable(new byte[]
		{
			159,
			179,
			104,
			105,
			106,
			103,
			103
		})]
		
		protected internal Edge(Node fromNode, Node toNode, double acousticScore, double lmScore)
		{
			this.acousticScore = acousticScore;
			this.lmScore = lmScore;
			this.__fromNode = fromNode;
			this.__toNode = toNode;
		}

		
		
		public override string toString()
		{
			return new StringBuilder().append("Edge(").append(this.__fromNode).append("-->").append(this.__toNode).append('[').append(this.acousticScore).append(',').append(this.lmScore).append("])").toString();
		}

		[LineNumberTable(new byte[]
		{
			10,
			103,
			103,
			109,
			141,
			105,
			100,
			191,
			16,
			105,
			100,
			191,
			16,
			109
		})]
		
		internal static void load(Lattice lattice, StringTokenizer stringTokenizer)
		{
			string text = stringTokenizer.nextToken();
			string text2 = stringTokenizer.nextToken();
			double num = Double.parseDouble(stringTokenizer.nextToken());
			double num2 = Double.parseDouble(stringTokenizer.nextToken());
			Node node = lattice.getNode(text);
			if (node == null)
			{
				string text3 = new StringBuilder().append("Edge fromNode \"").append(text).append("\" does not exist").toString();
				
				throw new Error(text3);
			}
			Node node2 = lattice.getNode(text2);
			if (node2 == null)
			{
				string text4 = new StringBuilder().append("Edge toNode \"").append(text2).append("\" does not exist").toString();
				
				throw new Error(text4);
			}
			lattice.addEdge(node, node2, num, num2);
		}

		[Throws(new string[]
		{
			"java.io.IOException"
		})]
		[LineNumberTable(new byte[]
		{
			36,
			159,
			70
		})]
		
		internal virtual void dump(PrintWriter printWriter)
		{
			printWriter.println(new StringBuilder().append("edge: ").append(this.__fromNode.getId()).append(' ').append(this.__toNode.getId()).append(' ').append(this.acousticScore).append(' ').append(this.lmScore).toString());
		}

		[Throws(new string[]
		{
			"java.io.IOException"
		})]
		[LineNumberTable(new byte[]
		{
			48,
			127,
			17,
			63,
			33,
			165
		})]
		
		internal virtual void dumpAISee(FileWriter fileWriter)
		{
			fileWriter.write(new StringBuilder().append("edge: { sourcename: \"").append(this.__fromNode.getId()).append("\" targetname: \"").append(this.__toNode.getId()).append("\" label: \"").append(this.acousticScore).append(',').append(this.lmScore).append("\" }\n").toString());
		}

		[Throws(new string[]
		{
			"java.io.IOException"
		})]
		[LineNumberTable(new byte[]
		{
			60,
			127,
			22,
			159,
			63
		})]
		
		public virtual void dumpDot(FileWriter f)
		{
			string text = new StringBuilder().append("").append(this.acousticScore).append(",").append(this.lmScore).toString();
			f.write(new StringBuilder().append("\tnode").append(this.__fromNode.getId()).append(" -> node").append(this.__toNode.getId()).append(" [ label=\"").append(text).append("\" ]\n").toString());
		}

		public virtual void setAcousticScore(double v)
		{
			this.acousticScore = v;
		}

		public virtual void setLMScore(double v)
		{
			this.lmScore = v;
		}

		[LineNumberTable(new byte[]
		{
			160,
			76,
			119,
			124,
			110,
			115,
			240,
			61
		})]
		
		public virtual bool isEquivalent(Edge other)
		{
			double num = java.lang.Math.abs(this.acousticScore) * 1E-05;
			return java.lang.Math.abs(this.acousticScore - other.getAcousticScore()) <= num && this.lmScore == other.getLMScore() && this.__fromNode.isEquivalent(other.getFromNode()) && this.__toNode.isEquivalent(other.getToNode());
		}

		
		protected internal Node fromNode
		{
			
			get
			{
				return this.__fromNode;
			}
			
			private set
			{
				this.__fromNode = value;
			}
		}

		
		protected internal Node toNode
		{
			
			get
			{
				return this.__toNode;
			}
			
			private set
			{
				this.__toNode = value;
			}
		}

		protected internal double acousticScore;

		protected internal double lmScore;

		internal Node __fromNode;

		internal Node __toNode;
	}
}
