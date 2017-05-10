using System;

using IKVM.Attributes;
using java.lang;

namespace edu.cmu.sphinx.trainer
{
	public class Edge : java.lang.Object
	{
		public virtual Node getDestination()
		{
			return this.destinationNode;
		}

		public virtual Node getSource()
		{
			return this.sourceNode;
		}

		[LineNumberTable(new byte[]
		{
			159,
			174,
			104,
			103,
			103,
			103
		})]
		
		internal Edge(Node node, Node node2, string text)
		{
			this.sourceNode = node;
			this.destinationNode = node2;
			this.id = text;
		}

		[LineNumberTable(new byte[]
		{
			159,
			185,
			105
		})]
		
		internal Edge(Node node, Node node2) : this(node, node2, null)
		{
		}

		public virtual void setDestination(Node node)
		{
			this.destinationNode = node;
		}

		public virtual void setSource(Node node)
		{
			this.sourceNode = node;
		}

		public virtual bool validate()
		{
			return this.sourceNode != null && this.destinationNode != null;
		}

		[LineNumberTable(new byte[]
		{
			53,
			127,
			10,
			127,
			10,
			127,
			10
		})]
		
		public virtual void print()
		{
			java.lang.System.@out.print(new StringBuilder().append("ID: ").append(this.id).toString());
			java.lang.System.@out.print(new StringBuilder().append(" | ").append(this.sourceNode).toString());
			java.lang.System.@out.println(new StringBuilder().append(" | ").append(this.destinationNode).toString());
		}

		public string id;

		public Node sourceNode;

		public Node destinationNode;
	}
}
