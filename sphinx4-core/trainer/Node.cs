using java.lang;
using java.util;

namespace edu.cmu.sphinx.trainer
{
	public class Node : Object
	{
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
			return String.instancehelper_equals(type, this.nodeType.toString());
		}

		public virtual object getObject()
		{
			return this.@object;
		}
		
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
		
		internal Node(NodeType nodeType) : this(nodeType, null)
		{
		}
		
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
		
		public virtual void addOutgoingEdge(Edge edge)
		{
			this.outgoingEdges.add(edge);
		}
		
		public virtual void addIncomingEdge(Edge edge)
		{
			this.incomingEdges.add(edge);
		}
		
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
