using System;

using IKVM.Attributes;
using ikvm.@internal;
using java.lang;
using java.util;

namespace edu.cmu.sphinx.trainer
{
	public class Graph : java.lang.Object
	{
		
		public static void __<clinit>()
		{
		}

		[LineNumberTable(new byte[]
		{
			160,
			202,
			98,
			102,
			107,
			103,
			105,
			103,
			103,
			100,
			105,
			127,
			5,
			162,
			102,
			104,
			150,
			100,
			105,
			127,
			5,
			162,
			102,
			104,
			150,
			101,
			102,
			104,
			104,
			106,
			117,
			117,
			98
		})]
		
		public virtual bool validate()
		{
			int num = 1;
			this.startNodeIterator();
			while (this.hasMoreNodes())
			{
				Node node = this.nextNode();
				num &= (node.validate() ? 1 : 0);
				int num2 = node.incomingEdgesSize();
				int num3 = node.outgoingEdgesSize();
				if (num2 < 1 && !this.isInitialNode(node))
				{
					java.lang.System.@out.println(new StringBuilder().append("No incoming edge: ").append(node).toString());
					num = 0;
				}
				node.startIncomingEdgeIterator();
				while (node.hasMoreIncomingEdges())
				{
					num &= (this.edges.contains(node.nextIncomingEdge()) ? 1 : 0);
				}
				if (num3 < 1 && !this.isFinalNode(node))
				{
					java.lang.System.@out.println(new StringBuilder().append("No outgoing edge: ").append(node).toString());
					num = 0;
				}
				node.startOutgoingEdgeIterator();
				while (node.hasMoreOutgoingEdges())
				{
					num &= (this.edges.contains(node.nextOutgoingEdge()) ? 1 : 0);
				}
			}
			this.startEdgeIterator();
			while (this.hasMoreEdges())
			{
				Edge edge = this.nextEdge();
				num &= (edge.validate() ? 1 : 0);
				num &= (this.nodes.contains(edge.getSource()) ? 1 : 0);
				num &= (this.nodes.contains(edge.getDestination()) ? 1 : 0);
			}
			return num != 0;
		}

		[LineNumberTable(new byte[]
		{
			159,
			179,
			104,
			107,
			107
		})]
		
		public Graph()
		{
			this.edges = new ArrayList();
			this.nodes = new ArrayList();
		}

		[LineNumberTable(new byte[]
		{
			160,
			66,
			109
		})]
		
		public virtual void addNode(Node node)
		{
			this.nodes.add(node);
		}

		[Throws(new string[]
		{
			"java.lang.IllegalArgumentException"
		})]
		[LineNumberTable(new byte[]
		{
			159,
			190,
			105,
			137,
			144
		})]
		
		public virtual void setInitialNode(Node node)
		{
			if (this.isNodeInGraph(node))
			{
				this.initialNode = node;
				return;
			}
			string text = "Initial node not in graph";
			
			throw new IllegalArgumentException(text);
		}

		[LineNumberTable(new byte[]
		{
			107,
			136,
			103,
			135,
			105,
			167,
			105,
			167,
			135
		})]
		
		public virtual Edge linkNodes(Node sourceNode, Node destinationNode)
		{
			Edge edge = new Edge(sourceNode, destinationNode);
			sourceNode.addOutgoingEdge(edge);
			destinationNode.addIncomingEdge(edge);
			if (!this.isNodeInGraph(sourceNode))
			{
				this.addNode(sourceNode);
			}
			if (!this.isNodeInGraph(destinationNode))
			{
				this.addNode(destinationNode);
			}
			this.addEdge(edge);
			return edge;
		}

		[Throws(new string[]
		{
			"java.lang.IllegalArgumentException"
		})]
		[LineNumberTable(new byte[]
		{
			11,
			105,
			137,
			144
		})]
		
		public virtual void setFinalNode(Node node)
		{
			if (this.isNodeInGraph(node))
			{
				this.finalNode = node;
				return;
			}
			string text = "Final node not in graph";
			
			throw new IllegalArgumentException(text);
		}

		[LineNumberTable(new byte[]
		{
			160,
			142,
			127,
			13,
			102,
			104,
			142,
			102,
			104,
			142,
			108,
			108
		})]
		
		public virtual void copyGraph(Graph graph)
		{
			if (!Graph.assertionsDisabled && (!this.nodes.isEmpty() || !this.edges.isEmpty()))
			{
				
				throw new AssertionError();
			}
			graph.startNodeIterator();
			while (graph.hasMoreNodes())
			{
				this.addNode(graph.nextNode());
			}
			graph.startEdgeIterator();
			while (graph.hasMoreEdges())
			{
				this.addEdge(graph.nextEdge());
			}
			this.setInitialNode(graph.getInitialNode());
			this.setFinalNode(graph.getFinalNode());
		}

		
		
		public virtual Node[] nodeToArray()
		{
			return (Node[])this.nodes.toArray(new Node[this.nodes.size()]);
		}

		[LineNumberTable(new byte[]
		{
			160,
			164,
			127,
			1,
			122,
			127,
			5,
			109,
			141,
			104,
			104,
			50,
			166,
			102,
			104,
			142,
			103,
			102,
			104,
			103,
			103,
			103,
			98,
			104,
			102,
			104,
			104,
			105,
			105,
			98
		})]
		
		public virtual void insertGraph(Graph graph, Node node)
		{
			if (!Graph.assertionsDisabled && !this.isNodeInGraph(node))
			{
				object obj = "Node not in graph";
				
				throw new AssertionError(obj);
			}
			if (!Graph.assertionsDisabled && graph == null)
			{
				object obj2 = "Graph not defined";
				
				throw new AssertionError(obj2);
			}
			if (!Graph.assertionsDisabled && (this.isFinalNode(node) || this.isInitialNode(node)))
			{
				
				throw new AssertionError();
			}
			int num = this.nodes.indexOf(node);
			this.nodes.remove(num);
			graph.startNodeIterator();
			int num2 = num;
			while (graph.hasMoreNodes())
			{
				this.nodes.add(num2, graph.nextNode());
				num2++;
			}
			graph.startEdgeIterator();
			while (graph.hasMoreEdges())
			{
				this.addEdge(graph.nextEdge());
			}
			Node node2 = graph.getInitialNode();
			node.startIncomingEdgeIterator();
			while (node.hasMoreIncomingEdges())
			{
				Edge edge = node.nextIncomingEdge();
				edge.setDestination(node2);
				node2.addIncomingEdge(edge);
			}
			Node node3 = graph.getFinalNode();
			node.startOutgoingEdgeIterator();
			while (node.hasMoreOutgoingEdges())
			{
				Edge edge2 = node.nextOutgoingEdge();
				edge2.setSource(node3);
				node3.addOutgoingEdge(edge2);
			}
		}

		
		
		public virtual Node getNode(int index)
		{
			return (Node)this.nodes.get(index);
		}

		
		
		public virtual bool isNodeInGraph(Node node)
		{
			return this.nodes.contains(node);
		}

		[LineNumberTable(new byte[]
		{
			160,
			74,
			109
		})]
		
		public virtual void addEdge(Edge edge)
		{
			this.edges.add(edge);
		}

		[LineNumberTable(new byte[]
		{
			160,
			98,
			113
		})]
		
		public virtual void startNodeIterator()
		{
			this.nodeIterator = this.nodes.iterator();
		}

		
		
		public virtual bool hasMoreNodes()
		{
			return this.nodeIterator.hasNext();
		}

		
		
		public virtual Node nextNode()
		{
			return (Node)this.nodeIterator.next();
		}

		[LineNumberTable(new byte[]
		{
			160,
			118,
			113
		})]
		
		public virtual void startEdgeIterator()
		{
			this.edgeIterator = this.edges.iterator();
		}

		
		
		public virtual bool hasMoreEdges()
		{
			return this.edgeIterator.hasNext();
		}

		
		
		public virtual Edge nextEdge()
		{
			return (Edge)this.edgeIterator.next();
		}

		public virtual Node getInitialNode()
		{
			return this.initialNode;
		}

		public virtual Node getFinalNode()
		{
			return this.finalNode;
		}

		public virtual bool isFinalNode(Node node)
		{
			return node == this.finalNode;
		}

		public virtual bool isInitialNode(Node node)
		{
			return node == this.initialNode;
		}

		
		
		public virtual int size()
		{
			return this.nodes.size();
		}

		
		
		public virtual int indexOf(Node node)
		{
			return this.nodes.indexOf(node);
		}

		
		
		public virtual bool isEdgeInGraph(Edge edge)
		{
			return this.edges.contains(edge);
		}

		[LineNumberTable(new byte[]
		{
			160,
			243,
			102,
			104,
			103,
			105,
			143,
			105,
			143,
			107,
			102,
			98,
			102,
			104,
			103,
			107,
			102,
			98
		})]
		
		public virtual void printGraph()
		{
			this.startNodeIterator();
			while (this.hasMoreNodes())
			{
				Node node = this.nextNode();
				if (this.isInitialNode(node))
				{
					java.lang.System.@out.println("Initial Node");
				}
				if (this.isFinalNode(node))
				{
					java.lang.System.@out.println("Final Node");
				}
				java.lang.System.@out.println(node);
				node.print();
			}
			this.startEdgeIterator();
			while (this.hasMoreEdges())
			{
				Edge edge = this.nextEdge();
				java.lang.System.@out.println(edge);
				edge.print();
			}
		}

		
		static Graph()
		{
		}

		
		private ArrayList edges;

		
		private ArrayList nodes;

		
		private Iterator edgeIterator;

		
		private Iterator nodeIterator;

		private Node initialNode;

		private Node finalNode;

		
		internal static bool assertionsDisabled = !ClassLiteral<Graph>.Value.desiredAssertionStatus();
	}
}
