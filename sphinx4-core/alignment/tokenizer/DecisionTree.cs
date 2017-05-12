using java.io;
using java.lang;
using java.net;
using java.util;
using java.util.logging;
using java.util.regex;

namespace edu.cmu.sphinx.alignment.tokenizer
{
	public class DecisionTree : Object
	{
		public DecisionTree(URL url)
		{
			this.cart = null;
			this.curNode = 0;
			BufferedReader bufferedReader = new BufferedReader(new InputStreamReader(url.openStream()));
			for (string text = bufferedReader.readLine(); text != null; text = bufferedReader.readLine())
			{
				if (!java.lang.String.instancehelper_startsWith(text, "***"))
				{
					this.parseAndAdd(text);
				}
			}
			bufferedReader.close();
		}

		public virtual object interpret(Item item)
		{
			int num = 0;
			while (!(this.cart[num] is DecisionTree.LeafNode))
			{
				DecisionTree.DecisionNode decisionNode = (DecisionTree.DecisionNode)this.cart[num];
				num = decisionNode.getNextNode(item);
			}
			DecisionTree.logger.fine(new StringBuilder().append("LEAF ").append(this.cart[num].getValue()).toString());
			return ((DecisionTree.LeafNode)this.cart[num]).getValue();
		}


		internal static Logger access_000()
		{
			return DecisionTree.logger;
		}

		protected internal virtual void parseAndAdd(string line)
		{
			StringTokenizer stringTokenizer = new StringTokenizer(line, " ");
			string text = stringTokenizer.nextToken();
			if (java.lang.String.instancehelper_equals(text, "LEAF") || java.lang.String.instancehelper_equals(text, "NODE"))
			{
				this.cart[this.curNode] = this.getNode(text, stringTokenizer, this.curNode);
				this.cart[this.curNode].setCreationLine(line);
				this.curNode++;
			}
			else
			{
				if (!java.lang.String.instancehelper_equals(text, "TOTAL"))
				{
					string text2 = new StringBuilder().append("Invalid CART type: ").append(text).toString();

					throw new Error(text2);
				}
				this.cart = new DecisionTree.Node[Integer.parseInt(stringTokenizer.nextToken())];
				this.curNode = 0;
			}
		}

		private DecisionTree(int num)
		{
			this.cart = null;
			this.curNode = 0;
			this.cart = new DecisionTree.Node[num];
		}

		private string dumpDotNodeColor(DecisionTree.Node n)
		{
			if (n is DecisionTree.LeafNode)
			{
				return "green";
			}
			return "red";
		}

		private string dumpDotNodeShape(DecisionTree.Node n)
		{
			return "box";
		}

		protected internal virtual DecisionTree.Node getNode(string type, StringTokenizer tokenizer, int currentNode)
		{
			if (java.lang.String.instancehelper_equals(type, "NODE"))
			{
				string text = tokenizer.nextToken();
				string text2 = tokenizer.nextToken();
				object obj = this.parseValue(tokenizer.nextToken());
				int num = Integer.parseInt(tokenizer.nextToken());
				if (java.lang.String.instancehelper_equals(text2, "MATCHES"))
				{
					return new DecisionTree.MatchingNode(text, java.lang.Object.instancehelper_toString(obj), currentNode + 1, num);
				}
				return new DecisionTree.ComparisonNode(text, obj, text2, currentNode + 1, num);
			}
			else
			{
				if (java.lang.String.instancehelper_equals(type, "LEAF"))
				{
					return new DecisionTree.LeafNode(this.parseValue(tokenizer.nextToken()));
				}
				return null;
			}
		}

		protected internal virtual object parseValue(string @string)
		{
			int num = java.lang.String.instancehelper_indexOf(@string, "(");
			string text = java.lang.String.instancehelper_substring(@string, 0, num);
			string text2 = java.lang.String.instancehelper_substring(@string, num + 1, java.lang.String.instancehelper_length(@string) - 1);
			if (java.lang.String.instancehelper_equals(text, "java.lang.String"))
			{
				return text2;
			}
			if (java.lang.String.instancehelper_equals(text, "Float"))
			{
				return new Float(Float.parseFloat(text2));
			}
			if (java.lang.String.instancehelper_equals(text, "Integer"))
			{
				return new Integer(Integer.parseInt(text2));
			}
			if (java.lang.String.instancehelper_equals(text, "List"))
			{
				StringTokenizer stringTokenizer = new StringTokenizer(text2, ",");
				int num2 = stringTokenizer.countTokens();
				int[] array = new int[num2];
				for (int i = 0; i < num2; i++)
				{
					float num3 = Float.parseFloat(stringTokenizer.nextToken());
					array[i] = java.lang.Math.round(num3);
				}
				return array;
			}
			string text3 = new StringBuilder().append("Unknown type: ").append(text).toString();

			throw new Error(text3);
		}

		public DecisionTree(BufferedReader reader, int nodes) : this(nodes)
		{
			for (int i = 0; i < nodes; i++)
			{
				string text = reader.readLine();
				if (!java.lang.String.instancehelper_startsWith(text, "***"))
				{
					this.parseAndAdd(text);
				}
			}
		}
		public virtual void dumpDot(PrintWriter @out)
		{
			@out.write("digraph \"CART Tree\" {\n");
			@out.write("rankdir = LR\n");
			DecisionTree.Node[] array = this.cart;
			int num = array.Length;
			for (int i = 0; i < num; i++)
			{
				DecisionTree.Node node = array[i];
				@out.println(new StringBuilder().append("\t\"node").append(java.lang.Object.instancehelper_hashCode(node)).append("\" [ label=\"").append(java.lang.Object.instancehelper_toString(node)).append("\", color=").append(this.dumpDotNodeColor(node)).append(", shape=").append(this.dumpDotNodeShape(node)).append(" ]\n").toString());
				if (node is DecisionTree.DecisionNode)
				{
					DecisionTree.DecisionNode decisionNode = (DecisionTree.DecisionNode)node;
					if (decisionNode.qtrue < this.cart.Length && this.cart[decisionNode.qtrue] != null)
					{
						@out.write(new StringBuilder().append("\t\"node").append(java.lang.Object.instancehelper_hashCode(node)).append("\" -> \"node").append(java.lang.Object.instancehelper_hashCode(this.cart[decisionNode.qtrue])).append("\" [ label=TRUE ]\n").toString());
					}
					if (decisionNode.qfalse < this.cart.Length && this.cart[decisionNode.qfalse] != null)
					{
						@out.write(new StringBuilder().append("\t\"node").append(java.lang.Object.instancehelper_hashCode(node)).append("\" -> \"node").append(java.lang.Object.instancehelper_hashCode(this.cart[decisionNode.qfalse])).append("\" [ label=FALSE ]\n").toString());
					}
				}
			}
			@out.write("}\n");
			@out.close();
		}

		protected internal string dumpDotNodeColor(object obj)
		{
			return this.dumpDotNodeColor((DecisionTree.Node)obj);
		}

		protected internal string dumpDotNodeShape(object obj)
		{
			return this.dumpDotNodeShape((DecisionTree.Node)obj);
		}

		private static Logger logger = Logger.getLogger(ikvm.@internal.ClassLiteral<DecisionTree>.Value.getSimpleName());

		internal const string TOTAL = "TOTAL";

		internal const string NODE = "NODE";

		internal const string LEAF = "LEAF";

		internal const string OPERAND_MATCHES = "MATCHES";

		internal DecisionTree.Node[] cart;

		[System.NonSerialized]
		internal int curNode;


		internal sealed class ComparisonNode : DecisionTree.DecisionNode
		{
			private string trace(object obj, bool flag, int num)
			{
				return new StringBuilder().append("NODE ").append(this.getFeature()).append(" [").append(obj).append("] ").append(this.comparisonType).append(" [").append(this.getValue()).append("] ").append((!flag) ? "No" : "Yes").append(" next ").append(num).toString();
			}
			public ComparisonNode(string text, object obj, string text2, int num, int num2) : base(text, obj, num, num2)
			{
				if (!java.lang.String.instancehelper_equals(text2, "<") && !java.lang.String.instancehelper_equals(text2, "=") && !java.lang.String.instancehelper_equals(text2, ">"))
				{
					string text3 = new StringBuilder().append("Invalid comparison type: ").append(text2).toString();

					throw new Error(text3);
				}
				this.comparisonType = text2;
			}

			public override int getNextNode(object obj)
			{
				int num3;
				if (java.lang.String.instancehelper_equals(this.comparisonType, "<") || java.lang.String.instancehelper_equals(this.comparisonType, ">"))
				{
					float num;
					if (this.value is Float)
					{
						num = ((Float)this.value).floatValue();
					}
					else
					{
						num = Float.parseFloat(java.lang.Object.instancehelper_toString(this.value));
					}
					float num2;
					if (obj is Float)
					{
						num2 = ((Float)obj).floatValue();
					}
					else
					{
						num2 = Float.parseFloat(java.lang.Object.instancehelper_toString(obj));
					}
					if (java.lang.String.instancehelper_equals(this.comparisonType, "<"))
					{
						num3 = ((num2 < num) ? 1 : 0);
					}
					else
					{
						num3 = ((num2 > num) ? 1 : 0);
					}
				}
				else
				{
					string text = java.lang.Object.instancehelper_toString(obj);
					string text2 = java.lang.Object.instancehelper_toString(this.value);
					num3 = (java.lang.String.instancehelper_equals(text, text2) ? 1 : 0);
				}
				int num4;
				if (num3 != 0)
				{
					num4 = this.qtrue;
				}
				else
				{
					num4 = this.qfalse;
				}
				DecisionTree.access_000().fine(this.trace(obj, num3 != 0, num4));
				return num4;
			}

			public override string toString()
			{
				return new StringBuilder().append("NODE ").append(this.getFeature()).append(" ").append(this.comparisonType).append(" ").append(this.getValueString()).append(" ").append(Integer.toString(this.qtrue)).append(" ").append(Integer.toString(this.qfalse)).toString();
			}

			internal const string LESS_THAN = "<";

			internal const string EQUALS = "=";

			internal const string GREATER_THAN = ">";

			internal string comparisonType;
		}

		internal abstract class DecisionNode : DecisionTree.Node
		{
			public virtual object findFeature(Item item)
			{
				return this.path.findFeature(item);
			}

			public abstract int getNextNode(object o);



			public virtual string getFeature()
			{
				return this.path.toString();
			}



			public int getNextNode(Item item)
			{
				return this.getNextNode(this.findFeature(item));
			}

			public DecisionNode(string pathAndFeature, object obj, int num, int num2) : base(obj)
			{
				this.path = new PathExtractor(pathAndFeature, true);
				this.qtrue = num;
				this.qfalse = num2;
			}

			private PathExtractor path;

			protected internal int qfalse;

			protected internal int qtrue;
		}


		internal sealed class LeafNode : DecisionTree.Node
		{
			public LeafNode(object obj) : base(obj)
			{
			}

			public override string toString()
			{
				return new StringBuilder().append("LEAF ").append(this.getValueString()).toString();
			}
		}

		internal sealed class MatchingNode : DecisionTree.DecisionNode
		{
			public MatchingNode(string text, string text2, int num, int num2) : base(text, text2, num, num2)
			{
				this.pattern = Pattern.compile(text2);
			}



			public override int getNextNode(object obj)
			{
				Pattern pattern = this.pattern;
				object _ref = (string)obj;
				CharSequence charSequence = CharSequence.Cast(_ref);
				return (!pattern.matcher(charSequence).matches()) ? this.qfalse : this.qtrue;
			}


			public override string toString()
			{
				StringBuffer stringBuffer = new StringBuffer(new StringBuilder().append("NODE ").append(this.getFeature()).append(" ").append("MATCHES").toString());
				stringBuffer.append(new StringBuilder().append(this.getValueString()).append(" ").toString());
				stringBuffer.append(new StringBuilder().append(Integer.toString(this.qtrue)).append(" ").toString());
				stringBuffer.append(Integer.toString(this.qfalse));
				return stringBuffer.toString();
			}

			internal Pattern pattern;
		}

		public abstract class Node : java.lang.Object
		{
			public Node(object obj)
			{
				this.value = obj;
			}

			public virtual object getValue()
			{
				return this.value;
			}

			public virtual string getValueString()
			{
				if (this.value == null)
				{
					return "NULL()";
				}
				if (this.value is string)
				{
					return new StringBuilder().append("java.lang.String(").append(java.lang.Object.instancehelper_toString(this.value)).append(")").toString();
				}
				if (this.value is Float)
				{
					return new StringBuilder().append("Float(").append(java.lang.Object.instancehelper_toString(this.value)).append(")").toString();
				}
				if (this.value is Integer)
				{
					return new StringBuilder().append("Integer(").append(java.lang.Object.instancehelper_toString(this.value)).append(")").toString();
				}
				return new StringBuilder().append(java.lang.Object.instancehelper_getClass(this.value).toString()).append("(").append(java.lang.Object.instancehelper_toString(this.value)).append(")").toString();
			}

			public virtual void setCreationLine(string text)
			{
			}

			protected internal object value;
		}
	}
}
