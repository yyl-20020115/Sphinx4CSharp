using System;

using IKVM.Attributes;
using java.lang;
using java.util;

namespace edu.cmu.sphinx.trainer
{
	public class Node : java.lang.Object
	{
		[LineNumberTable(new byte[]
		{
			97,
			113
		})]
		
		public virtual void startOutgoingEdgeIterator()
		{
			this.outgoingEdgeIterator = this.outgoingEdges.iterator();
		}

		
		
		public virtual bool hasMoreOutgoingEdges()
		{
			return this.outgoingEdgeIterator.hasNext();
		}

		
		
		public virtual Edge nextOutgoingEdge()
		{
			return (Edge)this.outgoingEdgeIterator.next();
		}

		
		
		public virtual bool isType(string type)
		{
			return java.lang.String.instancehelper_equals(type, this.nodeType.toString());
		}

		public virtual object getObject()
		{
			return this.@object;
		}

		[LineNumberTable(new byte[]
		{
			50,
			113
		})]
		
		public virtual void startIncomingEdgeIterator()
		{
			this.incomingEdgeIterator = this.incomingEdges.iterator();
		}

		
		
		public virtual bool hasMoreIncomingEdges()
		{
			return this.incomingEdgeIterator.hasNext();
		}

		
		
		public virtual Edge nextIncomingEdge()
		{
			return (Edge)this.incomingEdgeIterator.next();
		}

		[LineNumberTable(new byte[]
		{
			13,
			104
		})]
		
		internal Node(NodeType nodeType) : this(nodeType, null)
		{
		}

		[LineNumberTable(new byte[]
		{
			159,
			190,
			104,
			107,
			107,
			103,
			103,
			103
		})]
		
		internal Node(NodeType nodeType, string text)
		{
			this.incomingEdges = new ArrayList();
			this.outgoingEdges = new ArrayList();
			this.nodeId = text;
			this.nodeType = nodeType;
			this.@object = null;
		}

		public virtual NodeType getType()
		{
			return this.nodeType;
		}

		public virtual string getID()
		{
			return this.nodeId;
		}

		public virtual void setObject(object @object)
		{
			this.@object = @object;
		}

		[LineNumberTable(new byte[]
		{
			91,
			109
		})]
		
		public virtual void addOutgoingEdge(Edge edge)
		{
			this.outgoingEdges.add(edge);
		}

		[LineNumberTable(new byte[]
		{
			44,
			109
		})]
		
		public virtual void addIncomingEdge(Edge edge)
		{
			this.incomingEdges.add(edge);
		}

		[LineNumberTable(new byte[]
		{
			160,
			104,
			130,
			122,
			104,
			111,
			162,
			112,
			111,
			130
		})]
		
		public virtual bool validate()
		{
			int result = 1;
			if ((this.isType("WORD") || this.isType("PHONE")) && this.nodeId == null)
			{
				java.lang.System.@out.println("Content null in a WORD node.");
				result = 0;
			}
			if (this.incomingEdgesSize() == 0 && this.outgoingEdgesSize() == 0)
			{
				java.lang.System.@out.println("Node not connected anywhere.");
				result = 0;
			}
			return result != 0;
		}

		
		
		public virtual int incomingEdgesSize()
		{
			return this.incomingEdges.size();
		}

		
		
		public virtual int outgoingEdgesSize()
		{
			return this.outgoingEdges.size();
		}

		[LineNumberTable(new byte[]
		{
			160,
			122,
			127,
			10,
			127,
			20,
			102,
			104,
			159,
			12,
			111,
			102,
			104,
			159,
			12,
			106
		})]
		
		public virtual void print()
		{
			java.lang.System.@out.print(new StringBuilder().append("ID: ").append(this.nodeId).toString());
			java.lang.System.@out.print(new StringBuilder().append(" Type: ").append(this.nodeType).append(" | ").toString());
			this.startIncomingEdgeIterator();
			while (this.hasMoreIncomingEdges())
			{
				java.lang.System.@out.print(new StringBuilder().append(this.nextIncomingEdge()).append(" ").toString());
			}
			java.lang.System.@out.print(" | ");
			this.startOutgoingEdgeIterator();
			while (this.hasMoreOutgoingEdges())
			{
				java.lang.System.@out.print(new StringBuilder().append(this.nextOutgoingEdge()).append(" ").toString());
			}
			java.lang.System.@out.println();
		}

		private string nodeId;

		private object @object;

		private NodeType nodeType;

		
		private List incomingEdges;

		
		private Iterator incomingEdgeIterator;

		
		private List outgoingEdges;

		
		private Iterator outgoingEdgeIterator;
	}
}
