using edu.cmu.sphinx.linguist.dictionary;
using IKVM.Runtime;
using java.io;
using java.lang;
using java.util;

namespace edu.cmu.sphinx.linguist.language.grammar
{
	public class GrammarNode : java.lang.Object
	{
		public virtual GrammarArc[] getSuccessors()
		{
			return (GrammarArc[])this.arcList.toArray(new GrammarArc[this.arcList.size()]);
		}

		public virtual bool isFinalNode()
		{
			return this.isFinal;
		}
		
		public virtual bool isEmpty()
		{
			return this.getNumAlternatives() == 0;
		}

		public virtual Word getWord()
		{
			return this.alternatives[0][0];
		}
		
		public virtual void dumpDot(string path)
		{
			try
			{
				PrintWriter printWriter = new PrintWriter(new FileOutputStream(path));
				printWriter.println(new StringBuilder().append("digraph \"").append(path).append("\" {").toString());
				printWriter.println("rankdir = LR\n");
				this.traverseDot(printWriter, new HashSet());
				printWriter.println("}");
				printWriter.close();
			}
			catch (FileNotFoundException ex)
			{
				java.lang.System.@out.println(new StringBuilder().append("Can't write to ").append(path).append(' ').append(ex).toString());
			}
			return;
		}
		
		public GrammarNode(int id, Word[][] alternatives) : this(id, false)
		{
			this.alternatives = alternatives;
		}
		
		protected internal GrammarNode(int id, bool isFinal)
		{
			this.arcList = new ArrayList();
			this.identity = id;
			this.isFinal = isFinal;
			int num = 0;
			int num2 = 0;
			int[] array = new int[2];
			int num3 = num2;
			array[1] = num3;
			num3 = num;
			array[0] = num3;
			this.alternatives = (Word[][])ByteCodeHelper.multianewarray(typeof(Word[][]).TypeHandle, array);
		}

		public virtual int getID()
		{
			return this.identity;
		}
		
		internal virtual void optimize()
		{
			for (int i = 0; i < this.arcList.size(); i++)
			{
				GrammarArc grammarArc = (GrammarArc)this.arcList.get(i);
				this.arcList.set(i, this.optimizeArc(grammarArc));
			}
			if (this.isEmpty())
			{
				ListIterator listIterator = this.arcList.listIterator();
				while (listIterator.hasNext())
				{
					GrammarArc grammarArc = (GrammarArc)listIterator.next();
					if (this == grammarArc.getGrammarNode())
					{
						listIterator.remove();
					}
				}
			}
		}
		
		internal virtual GrammarNode splitNode(int id)
		{
			GrammarNode grammarNode = new GrammarNode(id, false);
			grammarNode.arcList = this.arcList;
			this.arcList = new ArrayList();
			this.add(grammarNode, 0f);
			return grammarNode;
		}
		
		public virtual void add(GrammarNode node, float logProbability)
		{
			if (this.isEmpty() && this == node)
			{
				return;
			}
			this.arcList.add(new GrammarArc(node, logProbability));
		}

		public virtual void setFinalNode(bool isFinal)
		{
			this.isFinal = isFinal;
		}
		
		public virtual Word[] getWords(int alternative)
		{
			return this.alternatives[alternative];
		}

		public virtual Word[][] getAlternatives()
		{
			return this.alternatives;
		}
		
		public virtual int getNumAlternatives()
		{
			return this.alternatives.Length;
		}
		
		internal virtual GrammarArc optimizeArc(GrammarArc grammarArc)
		{
			GrammarNode grammarNode = grammarArc.getGrammarNode();
			while (grammarNode.isEmpty() && grammarNode.arcList.size() == 1)
			{
				GrammarArc grammarArc2 = (GrammarArc)grammarNode.arcList.get(0);
				grammarArc = new GrammarArc(grammarArc2.getGrammarNode(), grammarArc.getProbability() + grammarArc2.getProbability());
				grammarNode = grammarArc.getGrammarNode();
			}
			return grammarArc;
		}
		
		private string traverse(int num, Set set, float num2)
		{
			StringBuilder stringBuilder = new StringBuilder();
			for (int i = 0; i < num; i++)
			{
				stringBuilder.append("    ");
			}
			stringBuilder.append("N(").append(this.getID()).append("):");
			stringBuilder.append("p:").append(num2);
			if (this.isFinalNode())
			{
				stringBuilder.append(" !");
			}
			Word[][] array = this.getAlternatives();
			for (int j = 0; j < array.Length; j++)
			{
				for (int k = 0; k < array[j].Length; k++)
				{
					stringBuilder.append(' ').append(array[j][k].getSpelling());
				}
				if (j < array.Length - 1)
				{
					stringBuilder.append('|');
				}
			}
			java.lang.System.@out.println(stringBuilder);
			if (!this.isFinalNode() && !set.contains(this))
			{
				set.add(this);
				GrammarArc[] successors = this.getSuccessors();
				GrammarArc[] array2 = successors;
				int num3 = array2.Length;
				for (int l = 0; l < num3; l++)
				{
					GrammarArc grammarArc = array2[l];
					GrammarNode grammarNode = grammarArc.getGrammarNode();
					grammarNode.traverse(num + 1, set, grammarArc.getProbability());
				}
			}
			else if (this.isFinalNode())
			{
				set.add(this);
			}
			return stringBuilder.toString();
		}

		internal virtual string getGDLID(GrammarNode grammarNode)
		{
			return new StringBuilder().append("\"").append(grammarNode.getID()).append('"').toString();
		}
		
		internal virtual string getGDLLabel(GrammarNode grammarNode)
		{
			string text = (!grammarNode.isEmpty()) ? grammarNode.getWord().getSpelling() : "";
			return new StringBuilder().append('"').append(text).append('"').toString();
		}

		internal virtual string getGDLShape(GrammarNode grammarNode)
		{
			return (!grammarNode.isEmpty()) ? "box" : "circle";
		}
		
		internal virtual string getGDLColor(GrammarNode grammarNode)
		{
			string result = "grey";
			if (grammarNode.isFinalNode())
			{
				result = "red";
			}
			else if (!grammarNode.isEmpty())
			{
				result = "green";
			}
			return result;
		}
		
		private void traverseGDL(PrintWriter printWriter, Set set)
		{
			if (!set.contains(this))
			{
				set.add(this);
				printWriter.println(new StringBuilder().append("   node: { title: ").append(this.getGDLID(this)).append(" label: ").append(this.getGDLLabel(this)).append(" shape: ").append(this.getGDLShape(this)).append(" color: ").append(this.getGDLColor(this)).append('}').toString());
				GrammarArc[] successors = this.getSuccessors();
				GrammarArc[] array = successors;
				int num = array.Length;
				for (int i = 0; i < num; i++)
				{
					GrammarArc grammarArc = array[i];
					GrammarNode grammarNode = grammarArc.getGrammarNode();
					float probability = grammarArc.getProbability();
					printWriter.println(new StringBuilder().append("   edge: { source: ").append(this.getGDLID(this)).append(" target: ").append(this.getGDLID(grammarNode)).append(" label: \"").append(probability).append("\"}").toString());
					grammarNode.traverseGDL(printWriter, set);
				}
			}
		}
		
		private void traverseDot(PrintWriter printWriter, Set set)
		{
			if (!set.contains(this))
			{
				set.add(this);
				printWriter.println(new StringBuilder().append("\tnode").append(this.getID()).append(" [ label=").append(this.getGDLLabel(this)).append(", color=").append(this.getGDLColor(this)).append(", shape=").append(this.getGDLShape(this)).append(" ]\n").toString());
				GrammarArc[] successors = this.getSuccessors();
				GrammarArc[] array = successors;
				int num = array.Length;
				for (int i = 0; i < num; i++)
				{
					GrammarArc grammarArc = array[i];
					GrammarNode grammarNode = grammarArc.getGrammarNode();
					float probability = grammarArc.getProbability();
					printWriter.write(new StringBuilder().append("\tnode").append(this.getID()).append(" -> node").append(grammarNode.getID()).append(" [ label=").append(probability).append(" ]\n").toString());
					grammarNode.traverseDot(printWriter, set);
				}
			}
		}
		
		public override string toString()
		{
			return new StringBuilder().append("G").append(this.getID()).toString();
		}

		public virtual void dumpGDL(string path)
		{
			try
			{
				try
				{
					PrintWriter printWriter = new PrintWriter(new FileOutputStream(path));
					printWriter.println("graph: {");
					printWriter.println("    orientation: left_to_right");
					printWriter.println("    layout_algorithm: dfs");
					this.traverseGDL(printWriter, new HashSet());
					printWriter.println("}");
					printWriter.close();
				}
				catch (FileNotFoundException ex)
				{
					java.lang.System.@out.println(new StringBuilder().append("Can't write to ").append(path).append(' ').append(ex).toString());
				}
			}
			catch (IOException ex3)
			{
				java.lang.System.@out.println(new StringBuilder().append("Trouble writing to ").append(path).append(' ').append(ex3).toString());
			}
		}
		
		public virtual void dump()
		{
			java.lang.System.@out.println(this.traverse(0, new HashSet(), 1f));
		}
		
		private int identity;

		private bool isFinal;

		private Word[][] alternatives;
		
		private List arcList;
	}
}
