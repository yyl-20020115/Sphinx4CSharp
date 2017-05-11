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
		
		internal static void load(Lattice lattice, StringTokenizer stringTokenizer)
		{
			string text = stringTokenizer.nextToken();
			string text2 = stringTokenizer.nextToken();
			double num = java.lang.Double.parseDouble(stringTokenizer.nextToken());
			double num2 = java.lang.Double.parseDouble(stringTokenizer.nextToken());
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

		internal virtual void dump(PrintWriter printWriter)
		{
			printWriter.println(new StringBuilder().append("edge: ").append(this.__fromNode.getId()).append(' ').append(this.__toNode.getId()).append(' ').append(this.acousticScore).append(' ').append(this.lmScore).toString());
		}

		internal virtual void dumpAISee(FileWriter fileWriter)
		{
			fileWriter.write(new StringBuilder().append("edge: { sourcename: \"").append(this.__fromNode.getId()).append("\" targetname: \"").append(this.__toNode.getId()).append("\" label: \"").append(this.acousticScore).append(',').append(this.lmScore).append("\" }\n").toString());
		}

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
