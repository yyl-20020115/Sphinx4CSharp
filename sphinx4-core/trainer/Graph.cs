using ikvm.@internal;
using java.lang;
using java.util;

namespace edu.cmu.sphinx.trainer
{
	public class Graph : java.lang.Object
	{				
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
		
		public Graph()
		{
			this.edges = new ArrayList();
			this.nodes = new ArrayList();
		}
		
		public virtual void addNode(Node node)
		{
			this.nodes.add(node);
		}
		
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
		
		public virtual void addEdge(Edge edge)
		{
			this.edges.add(edge);
		}
		
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
	
		private ArrayList edges;
		
		private ArrayList nodes;
		
		private Iterator edgeIterator;
		
		private Iterator nodeIterator;

		private Node initialNode;

		private Node finalNode;
		
		internal static bool assertionsDisabled = !ClassLiteral<Graph>.Value.desiredAssertionStatus();
	}
}
