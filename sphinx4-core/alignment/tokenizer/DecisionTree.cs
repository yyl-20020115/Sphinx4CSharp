using System;

using IKVM.Attributes;
using ikvm.@internal;
using java.io;
using java.lang;
using java.net;
using java.util;
using java.util.logging;
using java.util.regex;

namespace edu.cmu.sphinx.alignment.tokenizer
{
	public class DecisionTree : java.lang.Object
	{
		
		public static void __<clinit>()
		{
		}

		[Throws(new string[]
		{
			"java.io.IOException"
		})]
		[LineNumberTable(new byte[]
		{
			76,
			232,
			50,
			231,
			69,
			231,
			77,
			113,
			103,
			99,
			109,
			135,
			137,
			102
		})]
		
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

		[LineNumberTable(new byte[]
		{
			160,
			195,
			162,
			111,
			110,
			138,
			127,
			17
		})]
		
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

		
		internal static Logger access$000()
		{
			return DecisionTree.logger;
		}

		[LineNumberTable(new byte[]
		{
			160,
			111,
			108,
			103,
			122,
			123,
			115,
			112,
			109,
			118,
			137,
			159,
			6
		})]
		
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

		[LineNumberTable(new byte[]
		{
			115,
			232,
			11,
			231,
			69,
			231,
			113,
			108
		})]
		
		private DecisionTree(int num)
		{
			this.cart = null;
			this.curNode = 0;
			this.cart = new DecisionTree.Node[num];
		}

		[LineNumberTable(new byte[]
		{
			160,
			94,
			104,
			134
		})]
		protected internal virtual string dumpDotNodeColor(DecisionTree.Node n)
		{
			if (n is DecisionTree.LeafNode)
			{
				return "green";
			}
			return "red";
		}

		protected internal virtual string dumpDotNodeShape(DecisionTree.Node n)
		{
			return "box";
		}

		[LineNumberTable(new byte[]
		{
			160,
			136,
			109,
			103,
			103,
			109,
			108,
			109,
			177,
			173,
			109,
			178
		})]
		
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
					return new DecisionTree.MatchingNode(text, Object.instancehelper_toString(obj), currentNode + 1, num);
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

		[LineNumberTable(new byte[]
		{
			160,
			163,
			108,
			105,
			114,
			109,
			98,
			109,
			113,
			109,
			113,
			109,
			108,
			136,
			105,
			105,
			109,
			12,
			200,
			131
		})]
		
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
				Float.__<clinit>();
				return new Float(Float.parseFloat(text2));
			}
			if (java.lang.String.instancehelper_equals(text, "Integer"))
			{
				Integer.__<clinit>();
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

		[Throws(new string[]
		{
			"java.io.IOException"
		})]
		[LineNumberTable(new byte[]
		{
			100,
			137,
			102,
			103,
			109,
			231,
			61,
			230,
			70
		})]
		
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

		[LineNumberTable(new byte[]
		{
			160,
			67,
			107,
			139,
			119,
			127,
			7,
			127,
			13,
			25,
			165,
			107,
			104,
			127,
			1,
			127,
			20,
			57,
			197,
			127,
			1,
			127,
			20,
			57,
			229,
			52,
			233,
			83,
			107,
			102
		})]
		
		public virtual void dumpDot(PrintWriter @out)
		{
			@out.write("digraph \"CART Tree\" {\n");
			@out.write("rankdir = LR\n");
			DecisionTree.Node[] array = this.cart;
			int num = array.Length;
			for (int i = 0; i < num; i++)
			{
				DecisionTree.Node node = array[i];
				@out.println(new StringBuilder().append("\t\"node").append(Object.instancehelper_hashCode(node)).append("\" [ label=\"").append(Object.instancehelper_toString(node)).append("\", color=").append(this.dumpDotNodeColor(node)).append(", shape=").append(this.dumpDotNodeShape(node)).append(" ]\n").toString());
				if (node is DecisionTree.DecisionNode)
				{
					DecisionTree.DecisionNode decisionNode = (DecisionTree.DecisionNode)node;
					if (decisionNode.qtrue < this.cart.Length && this.cart[decisionNode.qtrue] != null)
					{
						@out.write(new StringBuilder().append("\t\"node").append(Object.instancehelper_hashCode(node)).append("\" -> \"node").append(Object.instancehelper_hashCode(this.cart[decisionNode.qtrue])).append("\" [ label=TRUE ]\n").toString());
					}
					if (decisionNode.qfalse < this.cart.Length && this.cart[decisionNode.qfalse] != null)
					{
						@out.write(new StringBuilder().append("\t\"node").append(Object.instancehelper_hashCode(node)).append("\" -> \"node").append(Object.instancehelper_hashCode(this.cart[decisionNode.qfalse])).append("\" [ label=FALSE ]\n").toString());
					}
				}
			}
			@out.write("}\n");
			@out.close();
		}

		
		static DecisionTree()
		{
		}

		
		[NameSig("dumpDotNodeColor", "(Ledu.cmu.sphinx.alignment.tokenizer.DecisionTree$Node;)Ljava.lang.java.lang.String;")]
		protected internal string dumpDotNodeColor(object obj)
		{
			return this.dumpDotNodeColor((DecisionTree.Node)obj);
		}

		
		[NameSig("dumpDotNodeColor", "(Ledu.cmu.sphinx.alignment.tokenizer.DecisionTree$Node;)Ljava.lang.java.lang.String;")]
		protected internal string <nonvirtual>0(object obj)
		{
			return this.dumpDotNodeColor((DecisionTree.Node)obj);
		}

		
		[NameSig("dumpDotNodeShape", "(Ledu.cmu.sphinx.alignment.tokenizer.DecisionTree$Node;)Ljava.lang.java.lang.String;")]
		protected internal string dumpDotNodeShape(object obj)
		{
			return this.dumpDotNodeShape((DecisionTree.Node)obj);
		}

		
		[NameSig("dumpDotNodeShape", "(Ledu.cmu.sphinx.alignment.tokenizer.DecisionTree$Node;)Ljava.lang.java.lang.String;")]
		protected internal string <nonvirtual>1(object obj)
		{
			return this.dumpDotNodeShape((DecisionTree.Node)obj);
		}

		
		[NameSig("getNode", "(Ljava.lang.java.lang.String;Ljava.util.StringTokenizer;I)Ledu.cmu.sphinx.alignment.tokenizer.DecisionTree$Node;")]
		protected internal object getNode(string type, StringTokenizer tokenizer, int currentNode)
		{
			return this.getNode(type, tokenizer, currentNode);
		}

		
		[NameSig("getNode", "(Ljava.lang.java.lang.String;Ljava.util.StringTokenizer;I)Ledu.cmu.sphinx.alignment.tokenizer.DecisionTree$Node;")]
		protected internal object <nonvirtual>2(string type, StringTokenizer tokenizer, int currentNode)
		{
			return this.getNode(type, tokenizer, currentNode);
		}

		
		private static Logger logger = Logger.getLogger(ClassLiteral<DecisionTree>.Value.getSimpleName());

		internal const string TOTAL = "TOTAL";

		internal const string NODE = "NODE";

		internal const string LEAF = "LEAF";

		internal const string OPERAND_MATCHES = "MATCHES";

		internal DecisionTree.Node[] cart;

		[NonSerialized]
		internal int curNode;

		
		[SourceFile("DecisionTree.java")]
		
		internal sealed class ComparisonNode : DecisionTree.DecisionNode
		{
			[LineNumberTable(new byte[]
			{
				159,
				10,
				162,
				127,
				43,
				63,
				30
			})]
			
			private string trace(object obj, bool flag, int num)
			{
				return new StringBuilder().append("NODE ").append(this.getFeature()).append(" [").append(obj).append("] ").append(this.comparisonType).append(" [").append(this.getValue()).append("] ").append((!flag) ? "No" : "Yes").append(" next ").append(num).toString();
			}

			[LineNumberTable(new byte[]
			{
				161,
				104,
				110,
				115,
				109,
				103,
				159,
				6,
				135
			})]
			
			public ComparisonNode(string text, object obj, string text2, int num, int num2) : base(text, obj, num, num2)
			{
				if (!java.lang.String.instancehelper_equals(text2, "<") && !java.lang.String.instancehelper_equals(text2, "=") && !java.lang.String.instancehelper_equals(text2, ">"))
				{
					string text3 = new StringBuilder().append("Invalid comparison type: ").append(text2).toString();
					
					throw new Error(text3);
				}
				this.comparisonType = text2;
			}

			[LineNumberTable(new byte[]
			{
				161,
				124,
				162,
				125,
				170,
				109,
				147,
				145,
				104,
				142,
				140,
				114,
				135,
				133,
				98,
				103,
				109,
				137,
				99,
				138,
				136,
				116
			})]
			
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
						num = Float.parseFloat(Object.instancehelper_toString(this.value));
					}
					float num2;
					if (obj is Float)
					{
						num2 = ((Float)obj).floatValue();
					}
					else
					{
						num2 = Float.parseFloat(Object.instancehelper_toString(obj));
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
					string text = Object.instancehelper_toString(obj);
					string text2 = Object.instancehelper_toString(this.value);
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
				DecisionTree.access$000().fine(this.trace(obj, num3 != 0, num4));
				return num4;
			}

			[LineNumberTable(new byte[]
			{
				161,
				170,
				127,
				27,
				127,
				21,
				15
			})]
			
			public override string toString()
			{
				return new StringBuilder().append("NODE ").append(this.getFeature()).append(" ").append(this.comparisonType).append(" ").append(this.getValueString()).append(" ").append(Integer.toString(this.qtrue)).append(" ").append(Integer.toString(this.qfalse)).toString();
			}

			internal const string LESS_THAN = "<";

			internal const string EQUALS = "=";

			internal const string GREATER_THAN = ">";

			internal string comparisonType;
		}

		
		[SourceFile("DecisionTree.java")]
		internal abstract class DecisionNode : DecisionTree.Node
		{
			
			
			public virtual object findFeature(Item item)
			{
				return this.path.findFeature(item);
			}

			public abstract int getNextNode(object);

			
			
			public virtual string getFeature()
			{
				return this.path.toString();
			}

			
			
			public int getNextNode(Item item)
			{
				return this.getNextNode(this.findFeature(item));
			}

			[LineNumberTable(new byte[]
			{
				161,
				56,
				105,
				109,
				103,
				104
			})]
			
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

		
		[SourceFile("DecisionTree.java")]
		
		internal sealed class LeafNode : DecisionTree.Node
		{
			[LineNumberTable(new byte[]
			{
				161,
				228,
				103
			})]
			
			public LeafNode(object obj) : base(obj)
			{
			}

			
			
			public override string toString()
			{
				return new StringBuilder().append("LEAF ").append(this.getValueString()).toString();
			}
		}

		
		[SourceFile("DecisionTree.java")]
		
		internal sealed class MatchingNode : DecisionTree.DecisionNode
		{
			[LineNumberTable(new byte[]
			{
				161,
				191,
				109,
				108
			})]
			
			public MatchingNode(string text, string text2, int num, int num2) : base(text, text2, num, num2)
			{
				this.pattern = Pattern.compile(text2);
			}

			
			
			public override int getNextNode(object obj)
			{
				Pattern pattern = this.pattern;
				object _<ref> = (string)obj;
				CharSequence charSequence;
				charSequence.__<ref> = _<ref>;
				return (!pattern.matcher(charSequence).matches()) ? this.qfalse : this.qtrue;
			}

			[LineNumberTable(new byte[]
			{
				161,
				208,
				117,
				159,
				10,
				127,
				7,
				127,
				12,
				114
			})]
			
			public override string toString()
			{
				StringBuffer.__<clinit>();
				StringBuffer stringBuffer = new StringBuffer(new StringBuilder().append("NODE ").append(this.getFeature()).append(" ").append("MATCHES").toString());
				stringBuffer.append(new StringBuilder().append(this.getValueString()).append(" ").toString());
				stringBuffer.append(new StringBuilder().append(Integer.toString(this.qtrue)).append(" ").toString());
				stringBuffer.append(Integer.toString(this.qfalse));
				return stringBuffer.toString();
			}

			internal Pattern pattern;
		}

		
		[SourceFile("DecisionTree.java")]
		internal abstract class Node : java.lang.Object
		{
			[LineNumberTable(new byte[]
			{
				160,
				218,
				104,
				103
			})]
			
			public Node(object obj)
			{
				this.value = obj;
			}

			public virtual object getValue()
			{
				return this.value;
			}

			[LineNumberTable(new byte[]
			{
				160,
				233,
				104,
				102,
				109,
				127,
				16,
				109,
				127,
				16,
				109,
				159,
				16
			})]
			
			public virtual string getValueString()
			{
				if (this.value == null)
				{
					return "NULL()";
				}
				if (this.value is string)
				{
					return new StringBuilder().append("java.lang.String(").append(Object.instancehelper_toString(this.value)).append(")").toString();
				}
				if (this.value is Float)
				{
					return new StringBuilder().append("Float(").append(Object.instancehelper_toString(this.value)).append(")").toString();
				}
				if (this.value is Integer)
				{
					return new StringBuilder().append("Integer(").append(Object.instancehelper_toString(this.value)).append(")").toString();
				}
				return new StringBuilder().append(Object.instancehelper_getClass(this.value).toString()).append("(").append(Object.instancehelper_toString(this.value)).append(")").toString();
			}

			public virtual void setCreationLine(string text)
			{
			}

			protected internal object value;
		}
	}
}
