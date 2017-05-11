using edu.cmu.sphinx.decoder.search;
using edu.cmu.sphinx.linguist.dictionary;
using edu.cmu.sphinx.util;
using ikvm.@internal;
using IKVM.Runtime;
using java.io;
using java.lang;
using java.util;

namespace edu.cmu.sphinx.result
{
	public class Lattice : java.lang.Object
	{	
		public Lattice(Result result) : this()
		{
			if (!Lattice.assertionsDisabled && result == null)
			{
				
				throw new AssertionError();
			}
			Token bestFinalToken = result.getBestFinalToken();
			if (bestFinalToken != null)
			{
				if (!Lattice.assertionsDisabled && !bestFinalToken.getWord().isSentenceEndWord())
				{
					
					throw new AssertionError();
				}
				this.terminalNode = new Node(this.getNodeID(bestFinalToken), bestFinalToken.getWord(), -1L, -1L);
				this.initialNode = this.terminalNode;
				this.addNode(this.terminalNode);
				this.visitedWordTokens = new HashSet();
				this.wordTokenFirst = result.getWordTokenFirst();
				this.loserManager = result.getAlternateHypothesisManager();
				if (this.loserManager != null)
				{
					this.loserManager.purge();
				}
				this.collapseWordToken(bestFinalToken);
			}
		}
		
		public virtual void computeNodePosteriors(float languageModelWeightAdjustment)
		{
			this.computeNodePosteriors(languageModelWeightAdjustment, false);
		}
		
		public virtual List getWordResultPath()
		{
			List viterbiPath = this.getViterbiPath();
			LinkedList linkedList = new LinkedList();
			Iterator iterator = viterbiPath.iterator();
			while (iterator.hasNext())
			{
				Node node = (Node)iterator.next();
				if (!node.getWord().isSentenceStartWord())
				{
					if (!node.getWord().isSentenceEndWord())
					{
						linkedList.add(new WordResult(node));
					}
				}
			}
			return linkedList;
		}
		
		public virtual Collection getNodes()
		{
			return this.nodes.values();
		}

		public virtual Node getInitialNode()
		{
			return this.initialNode;
		}

		public virtual Node getTerminalNode()
		{
			return this.terminalNode;
		}

		public virtual Collection getEdges()
		{
			return this.edges;
		}

		protected internal virtual Node getNode(string id)
		{
			return (Node)this.nodes.get(id);
		}
		
		public virtual Edge addEdge(Node fromNode, Node toNode, double acousticScore, double lmScore)
		{
			Edge edge = new Edge(fromNode, toNode, acousticScore, lmScore);
			fromNode.addLeavingEdge(edge);
			toNode.addEnteringEdge(edge);
			this.edges.add(edge);
			return edge;
		}
		
		public Lattice()
		{
			this.edges = new HashSet();
			this.nodes = new HashMap();
			this.logMath = LogMath.getLogMath();
		}
		
		protected internal virtual Node addNode(string id, Word word, long beginTime, long endTime)
		{
			Node node = new Node(id, word, beginTime, endTime);
			this.addNode(node);
			return node;
		}

		public virtual void setInitialNode(Node initialNode)
		{
			this.initialNode = initialNode;
		}

		public virtual void setTerminalNode(Node terminalNode)
		{
			this.terminalNode = terminalNode;
		}
		
		private string getNodeID(Token token)
		{
			return Integer.toString(java.lang.Object.instancehelper_hashCode(token));
		}
		
		protected internal virtual void addNode(Node n)
		{
			if (!Lattice.assertionsDisabled && this.hasNode(n.getId()))
			{
				
				throw new AssertionError();
			}
			this.nodes.put(n.getId(), n);
		}
		
		private void collapseWordToken(Token token)
		{
			if (!Lattice.assertionsDisabled && token == null)
			{
				
				throw new AssertionError();
			}
			if (this.visitedWordTokens.contains(token))
			{
				return;
			}
			this.visitedWordTokens.add(token);
			this.collapseWordPath(this.getNode(token), token.getPredecessor(), token.getAcousticScore() + token.getInsertionScore(), token.getLanguageScore());
			if (this.loserManager != null && this.loserManager.hasAlternatePredecessors(token))
			{
				Iterator iterator = this.loserManager.getAlternatePredecessors(token).iterator();
				while (iterator.hasNext())
				{
					Token token2 = (Token)iterator.next();
					this.collapseWordPath(this.getNode(token), token2, token.getAcousticScore(), token.getLanguageScore());
				}
			}
		}
				
		private TimeFrame getTimeFrameWordTokenFirst(Token token)
		{
			return new TimeFrame(0L, 0L);
		}
		
		private TimeFrame getTimeFrameWordTokenLast(Token token)
		{
			TimeFrame result = new TimeFrame(0L, 0L);
			Word word = null;
			long num = -1L;
			long num2 = -1L;
			for (Token token2 = token; token2 != null; token2 = token2.getPredecessor())
			{
				if (token2.isWord())
				{
					if (word != null && num >= 0L)
					{
						return new TimeFrame(num, num2);
					}
					word = token2.getWord();
					num2 = token2.getCollectTime();
				}
				num = token2.getCollectTime();
			}
			if (num2 >= 0L && num >= 0L)
			{
				return new TimeFrame(num, num2);
			}
			return result;
		}
		
		private TimeFrame getTimeFrame(Token token)
		{
			if (this.wordTokenFirst)
			{
				return this.getTimeFrameWordTokenFirst(token);
			}
			return this.getTimeFrameWordTokenLast(token);
		}
		
		private Node getNode(Token token)
		{
			if (token.getWord().isSentenceEndWord())
			{
				return this.terminalNode;
			}
			Node node = (Node)this.nodes.get(this.getNodeID(token));
			if (node == null)
			{
				TimeFrame timeFrame = this.getTimeFrame(token);
				node = new Node(this.getNodeID(token), token.getWord(), timeFrame.getStart(), timeFrame.getEnd());
				this.addNode(node);
			}
			return node;
		}
		
		private void collapseWordPath(Node node, Token token, float num, float num2)
		{
			if (token == null)
			{
				return;
			}
			if (token.isWord())
			{
				Node node2 = this.getNode(token);
				this.addEdge(node2, node, (double)num, (double)num2);
				if (token.getPredecessor() != null)
				{
					this.collapseWordToken(token);
				}
				else
				{
					if (!Lattice.assertionsDisabled && !token.getWord().isSentenceStartWord())
					{
						
						throw new AssertionError();
					}
					this.initialNode = node2;
				}
				return;
			}
			for (;;)
			{
				num += token.getAcousticScore() + token.getInsertionScore();
				num2 += token.getLanguageScore();
				Token predecessor = token.getPredecessor();
				if (predecessor == null)
				{
					break;
				}
				if (predecessor.isWord())
				{
					goto IL_AD;
				}
				if (this.loserManager != null && this.loserManager.hasAlternatePredecessors(token))
				{
					goto Block_9;
				}
				token = predecessor;
			}
			return;
			Block_9:
			IL_AD:
			this.collapseWordPath(node, token.getPredecessor(), num, num2);
			if (this.loserManager != null && this.loserManager.hasAlternatePredecessors(token))
			{
				Iterator iterator = this.loserManager.getAlternatePredecessors(token).iterator();
				while (iterator.hasNext())
				{
					Token token2 = (Token)iterator.next();
					this.collapseWordPath(node, token2, num, num2);
				}
			}
		}
		
		public static Lattice readSlf(InputStream stream)
		{
			Lattice lattice = new Lattice();
			LineNumberReader lineNumberReader = new LineNumberReader(new InputStreamReader(stream));
			int num = 0;
			int num2 = 0;
			int num3 = 0;
			int num4 = 1;
			double num5 = 9.5;
			string text;
			while ((text = lineNumberReader.readLine()) != null)
			{
				string text2 = text;
				object obj = "Node definitions";
				CharSequence charSequence = CharSequence.Cast(obj);
				if (java.lang.String.instancehelper_contains(text2, charSequence))
				{
					num2 = 0;
					num = 1;
				}
				else
				{
					string text3 = text;
					obj = "Link definitions";
					charSequence = CharSequence.Cast(obj);
					if (java.lang.String.instancehelper_contains(text3, charSequence))
					{
						num2 = 1;
						num = 0;
					}
					else if (!java.lang.String.instancehelper_startsWith(text, "#"))
					{
						if (num != 0)
						{
							string[] array = java.lang.String.instancehelper_split(text, "\\s+");
							if (array.Length != 3 || !java.lang.String.instancehelper_startsWith(array[0], "I=") || !java.lang.String.instancehelper_startsWith(array[1], "t=") || !java.lang.String.instancehelper_startsWith(array[2], "W="))
							{
								lineNumberReader.close();
								string text4 = new StringBuilder().append("Unknown node definition: ").append(text).toString();
								
								throw new IOException(text4);
							}
							int num6 = Integer.parseInt(java.lang.String.instancehelper_substring(array[0], 2));
							long beginTime = ByteCodeHelper.d2l(java.lang.Double.parseDouble(java.lang.String.instancehelper_substring(array[1], 2)) * 1000.0);
							string text5 = java.lang.String.instancehelper_substring(array[2], 2);
							int isFiller = 0;
							if (num6 == num3 || java.lang.String.instancehelper_equals(text5, "!ENTER"))
							{
								text5 = "<s>";
								isFiller = 1;
							}
							if (num6 == num4 || java.lang.String.instancehelper_equals(text5, "!EXIT"))
							{
								text5 = "</s>";
								isFiller = 1;
							}
							if (java.lang.String.instancehelper_equals(text5, "!NULL"))
							{
								text5 = "<sil>";
								isFiller = 1;
							}
							if (java.lang.String.instancehelper_startsWith(text5, "["))
							{
								isFiller = 1;
							}
							Word word = new Word(text5, new Pronunciation[0], isFiller != 0);
							Node node = lattice.addNode(Integer.toString(num6), word, beginTime, -1L);
							if (java.lang.String.instancehelper_equals(text5, "<s>"))
							{
								lattice.setInitialNode(node);
							}
							if (java.lang.String.instancehelper_equals(text5, "</s>"))
							{
								lattice.setTerminalNode(node);
							}
						}
						else if (num2 != 0)
						{
							string[] array = java.lang.String.instancehelper_split(text, "\\s+");
							if (array.Length != 5 || !java.lang.String.instancehelper_startsWith(array[1], "S=") || !java.lang.String.instancehelper_startsWith(array[2], "E=") || !java.lang.String.instancehelper_startsWith(array[3], "a=") || !java.lang.String.instancehelper_startsWith(array[4], "l="))
							{
								lineNumberReader.close();
								string text6 = new StringBuilder().append("Unknown edge definition: ").append(text).toString();
								
								throw new IOException(text6);
							}
							string text7 = java.lang.String.instancehelper_substring(array[1], 2);
							string text8 = java.lang.String.instancehelper_substring(array[2], 2);
							double acousticScore = java.lang.Double.parseDouble(java.lang.String.instancehelper_substring(array[3], 2));
							double lmScore = java.lang.Double.parseDouble(java.lang.String.instancehelper_substring(array[4], 2)) * num5;
							lattice.addEdge((Node)lattice.nodes.get(text7), (Node)lattice.nodes.get(text8), acousticScore, lmScore);
						}
						else
						{
							if (java.lang.String.instancehelper_startsWith(text, "start="))
							{
								string text9 = text;
								object obj2 = "start=";
								obj = "";
								object obj3 = obj2;
								charSequence = CharSequence.Cast(obj3);
								CharSequence charSequence2 = charSequence;
								obj3 = obj;
								charSequence = CharSequence.Cast(obj3);
								num3 = Integer.parseInt(java.lang.String.instancehelper_replace(text9, charSequence2, charSequence));
							}
							if (java.lang.String.instancehelper_startsWith(text, "end="))
							{
								string text10 = text;
								object obj4 = "end=";
								object obj3 = "";
								obj = obj4;
								charSequence = CharSequence.Cast(obj);
								CharSequence charSequence3 = charSequence;
								obj = obj3;
								charSequence = CharSequence.Cast(obj);
								num4 = Integer.parseInt(java.lang.String.instancehelper_replace(text10, charSequence3, charSequence));
							}
							if (java.lang.String.instancehelper_startsWith(text, "lmscale="))
							{
								string text11 = text;
								object obj5 = "lmscale=";
								obj = "";
								object obj3 = obj5;
								charSequence = CharSequence.Cast(obj3);
								CharSequence charSequence4 = charSequence;
								obj3 = obj;
								charSequence = CharSequence.Cast(obj3);
								num5 = java.lang.Double.parseDouble(java.lang.String.instancehelper_replace(text11, charSequence4, charSequence));
							}
						}
					}
				}
			}
			Iterator iterator = lattice.nodes.values().iterator();
			while (iterator.hasNext())
			{
				Node node2 = (Node)iterator.next();
				Iterator iterator2 = node2.getLeavingEdges().iterator();
				while (iterator2.hasNext())
				{
					Edge edge = (Edge)iterator2.next();
					if (node2.getEndTime() < 0L || node2.getEndTime() > edge.getToNode().getBeginTime())
					{
						node2.setEndTime(java.lang.Math.max(edge.getToNode().getBeginTime(), node2.getBeginTime()));
					}
				}
			}
			return lattice;
		}
		
		protected internal virtual bool hasNode(string ID)
		{
			return this.nodes.containsKey(ID);
		}
		
		protected internal virtual void dump(PrintWriter @out)
		{
			Iterator iterator = this.nodes.values().iterator();
			while (iterator.hasNext())
			{
				Node node = (Node)iterator.next();
				node.dump(@out);
			}
			iterator = this.edges.iterator();
			while (iterator.hasNext())
			{
				Edge edge = (Edge)iterator.next();
				edge.dump(@out);
			}
			@out.println(new StringBuilder().append("initialNode: ").append(this.initialNode.getId()).toString());
			@out.println(new StringBuilder().append("terminalNode: ").append(this.terminalNode.getId()).toString());
			@out.println(new StringBuilder().append("logBase: ").append(this.logMath.getLogBase()).toString());
			@out.flush();
		}
		
		internal virtual bool checkConsistency()
		{
			Iterator iterator = this.nodes.values().iterator();
			while (iterator.hasNext())
			{
				Node node = (Node)iterator.next();
				Iterator iterator2 = node.getEnteringEdges().iterator();
				while (iterator2.hasNext())
				{
					Edge edge = (Edge)iterator2.next();
					if (!this.hasEdge(edge))
					{
						string text = new StringBuilder().append("Lattice has NODE with missing FROM edge: ").append(node).append(',').append(edge).toString();
						
						throw new Error(text);
					}
				}
				iterator2 = node.getLeavingEdges().iterator();
				while (iterator2.hasNext())
				{
					Edge edge = (Edge)iterator2.next();
					if (!this.hasEdge(edge))
					{
						string text2 = new StringBuilder().append("Lattice has NODE with missing TO edge: ").append(node).append(',').append(edge).toString();
						
						throw new Error(text2);
					}
				}
			}
			iterator = this.edges.iterator();
			while (iterator.hasNext())
			{
				Edge edge2 = (Edge)iterator.next();
				if (!this.hasNode(edge2.getFromNode()))
				{
					string text3 = new StringBuilder().append("Lattice has EDGE with missing FROM node: ").append(edge2).toString();
					
					throw new Error(text3);
				}
				if (!this.hasNode(edge2.getToNode()))
				{
					string text4 = new StringBuilder().append("Lattice has EDGE with missing TO node: ").append(edge2).toString();
					
					throw new Error(text4);
				}
				if (!edge2.getToNode().hasEdgeFromNode(edge2.getFromNode()))
				{
					string text5 = new StringBuilder().append("Lattice has EDGE with TO node with no corresponding FROM edge: ").append(edge2).toString();
					
					throw new Error(text5);
				}
				if (!edge2.getFromNode().hasEdgeToNode(edge2.getToNode()))
				{
					string text6 = new StringBuilder().append("Lattice has EDGE with FROM node with no corresponding TO edge: ").append(edge2).toString();
					
					throw new Error(text6);
				}
			}
			return true;
		}
		
		protected internal virtual void removeNodeAndEdges(Node n)
		{
			Iterator iterator = n.getLeavingEdges().iterator();
			while (iterator.hasNext())
			{
				Edge edge = (Edge)iterator.next();
				edge.getToNode().removeEnteringEdge(edge);
				this.edges.remove(edge);
			}
			iterator = n.getEnteringEdges().iterator();
			while (iterator.hasNext())
			{
				Edge edge = (Edge)iterator.next();
				edge.getFromNode().removeLeavingEdge(edge);
				this.edges.remove(edge);
			}
			this.nodes.remove(n.getId());
			if (!Lattice.assertionsDisabled && !this.checkConsistency())
			{
				
				throw new AssertionError();
			}
		}
		
		public virtual List allPaths()
		{
			return this.allPathsFrom("", this.initialNode);
		}
		
		protected internal virtual List allPathsFrom(string path, Node n)
		{
			string text = new StringBuilder().append(path).append(' ').append(n.getWord()).toString();
			LinkedList linkedList = new LinkedList();
			if (n == this.terminalNode)
			{
				linkedList.add(text);
			}
			else
			{
				Iterator iterator = n.getLeavingEdges().iterator();
				while (iterator.hasNext())
				{
					Edge edge = (Edge)iterator.next();
					linkedList.addAll(this.allPathsFrom(text, edge.getToNode()));
				}
			}
			return linkedList;
		}
		
		internal virtual bool hasEdge(Edge edge)
		{
			return this.edges.contains(edge);
		}		
		
		internal virtual bool hasNode(Node node)
		{
			return this.hasNode(node.getId());
		}
		
		protected internal virtual void sortHelper(Node n, List sorted, Set visited)
		{
			if (visited.contains(n))
			{
				return;
			}
			visited.add(n);
			if (n == null)
			{
				string text = "Node is null";
				
				throw new Error(text);
			}
			Iterator iterator = n.getLeavingEdges().iterator();
			while (iterator.hasNext())
			{
				Edge edge = (Edge)iterator.next();
				this.sortHelper(edge.getToNode(), sorted, visited);
			}
			sorted.add(n);
		}
		
		public virtual void computeNodePosteriors(float languageModelWeightAdjustment, bool useAcousticScoresOnly)
		{
			if (this.initialNode == null)
			{
				return;
			}
			this.initialNode.setForwardScore((double)0f);
			this.initialNode.setViterbiScore((double)0f);
			List list = this.sortNodes();
			if (!Lattice.assertionsDisabled && list.get(0) != this.initialNode)
			{
				
				throw new AssertionError();
			}
			Iterator iterator = list.iterator();
			while (iterator.hasNext())
			{
				Node node = (Node)iterator.next();
				Iterator iterator2 = node.getLeavingEdges().iterator();
				while (iterator2.hasNext())
				{
					Edge edge = (Edge)iterator2.next();
					double num = edge.getFromNode().getForwardScore();
					double num2 = this.computeEdgeScore(edge, languageModelWeightAdjustment, useAcousticScoresOnly);
					num += num2;
					edge.getToNode().setForwardScore((double)this.logMath.addAsLinear((float)num, (float)edge.getToNode().getForwardScore()));
					double num3 = edge.getFromNode().getViterbiScore() + num2;
					if (edge.getToNode().getBestPredecessor() == null || num3 > edge.getToNode().getViterbiScore())
					{
						edge.getToNode().setBestPredecessor(node);
						edge.getToNode().setViterbiScore(num3);
					}
				}
			}
			this.terminalNode.setBackwardScore((double)0f);
			if (!Lattice.assertionsDisabled && list.get(list.size() - 1) != this.terminalNode)
			{
				
				throw new AssertionError();
			}
			ListIterator listIterator = list.listIterator(list.size() - 1);
			Iterator iterator3;
			while (listIterator.hasPrevious())
			{
				Node node = (Node)listIterator.previous();
				Collection leavingEdges = node.getLeavingEdges();
				iterator3 = leavingEdges.iterator();
				while (iterator3.hasNext())
				{
					Edge edge2 = (Edge)iterator3.next();
					double num4 = edge2.getToNode().getBackwardScore();
					num4 += this.computeEdgeScore(edge2, languageModelWeightAdjustment, useAcousticScoresOnly);
					edge2.getFromNode().setBackwardScore((double)this.logMath.addAsLinear((float)num4, (float)edge2.getFromNode().getBackwardScore()));
				}
			}
			double num5 = this.terminalNode.getForwardScore();
			iterator3 = this.nodes.values().iterator();
			while (iterator3.hasNext())
			{
				Node node2 = (Node)iterator3.next();
				node2.setPosterior(node2.getForwardScore() + node2.getBackwardScore() - num5);
			}
		}
		
		public virtual List sortNodes()
		{
			ArrayList arrayList = new ArrayList(this.nodes.size());
			this.sortHelper(this.initialNode, arrayList, new HashSet());
			Collections.reverse(arrayList);
			return arrayList;
		}
		
		private double computeEdgeScore(Edge edge, float num, bool flag)
		{
			if (flag)
			{
				return edge.getAcousticScore();
			}
			return edge.getAcousticScore() + edge.getLMScore() * (double)num;
		}
		
		public virtual List getViterbiPath()
		{
			LinkedList linkedList = new LinkedList();
			for (Node bestPredecessor = this.terminalNode; bestPredecessor != this.initialNode; bestPredecessor = bestPredecessor.getBestPredecessor())
			{
				linkedList.addFirst(bestPredecessor);
			}
			linkedList.addFirst(this.initialNode);
			return linkedList;
		}
		
		private bool checkNodesEquivalent(Node node, Node node2)
		{
			if (!Lattice.assertionsDisabled && (node == null || node2 == null))
			{
				
				throw new AssertionError();
			}
			int num = node.isEquivalent(node2) ? 1 : 0;
			if (num != 0)
			{
				Collection copyOfLeavingEdges = node.getCopyOfLeavingEdges();
				Collection copyOfLeavingEdges2 = node2.getCopyOfLeavingEdges();
				java.lang.System.@out.println(new StringBuilder().append("# edges: ").append(copyOfLeavingEdges.size()).append(' ').append(copyOfLeavingEdges2.size()).toString());
				Iterator iterator = copyOfLeavingEdges.iterator();
				while (iterator.hasNext())
				{
					Edge edge = (Edge)iterator.next();
					Edge edge2 = node2.findEquivalentLeavingEdge(edge);
					if (edge2 == null)
					{
						java.lang.System.@out.println("Equivalent edge not found, lattices not equivalent.");
						return false;
					}
					if (!copyOfLeavingEdges2.remove(edge2))
					{
						java.lang.System.@out.println("Equivalent edge already matched, lattices not equivalent.");
						return false;
					}
					num &= (this.checkNodesEquivalent(edge.getToNode(), edge2.getToNode()) ? 1 : 0);
					if (num == 0)
					{
						return false;
					}
				}
				if (!copyOfLeavingEdges2.isEmpty())
				{
					java.lang.System.@out.println("One lattice has too many edges.");
					return false;
				}
			}
			return num != 0;
		}
		
		public Lattice(string fileName) : this()
		{
			try
			{
				java.lang.System.err.println(new StringBuilder().append("Loading from ").append(fileName).toString());
				LineNumberReader lineNumberReader = new LineNumberReader(new FileReader(fileName));
				string text;
				while ((text = lineNumberReader.readLine()) != null)
				{
					StringTokenizer stringTokenizer = new StringTokenizer(text);
					if (stringTokenizer.hasMoreTokens())
					{
						string text2 = stringTokenizer.nextToken();
						if (java.lang.String.instancehelper_equals(text2, "edge:"))
						{
							Edge.load(this, stringTokenizer);
						}
						else if (java.lang.String.instancehelper_equals(text2, "node:"))
						{
							Node.load(this, stringTokenizer);
						}
						else if (java.lang.String.instancehelper_equals(text2, "initialNode:"))
						{
							this.setInitialNode(this.getNode(stringTokenizer.nextToken()));
						}
						else if (java.lang.String.instancehelper_equals(text2, "terminalNode:"))
						{
							this.setTerminalNode(this.getNode(stringTokenizer.nextToken()));
						}
						else
						{
							if (!java.lang.String.instancehelper_equals(text2, "logBase:"))
							{
								lineNumberReader.close();
								string text3 = new StringBuilder().append("SYNTAX ERROR: ").append(fileName).append('[').append(lineNumberReader.getLineNumber()).append("] ").append(text).toString();
								
								throw new Error(text3);
							}
							this.logBase = java.lang.Double.parseDouble(stringTokenizer.nextToken());
						}
					}
				}
				lineNumberReader.close();
			}
			catch (System.Exception ex)
			{
				throw new Error(Throwable.instancehelper_toString(ex));

			}
		}
		
		public static Lattice readSlf(string fileName)
		{
			FileInputStream fileInputStream = new FileInputStream(fileName);
			Lattice result = Lattice.readSlf(fileInputStream);
			fileInputStream.close();
			return result;
		}
		
		public virtual Node addNode(string id, string word, long beginTime, long endTime)
		{
			Word word2 = new Word(word, new Pronunciation[0], false);
			return this.addNode(id, word2, beginTime, endTime);
		}
		
		protected internal virtual void removeNode(Node n)
		{
			if (!Lattice.assertionsDisabled && !this.hasNode(n.getId()))
			{
				throw new AssertionError();
			}
			this.nodes.remove(n.getId());
		}
		
		protected internal virtual Collection getCopyOfNodes()
		{
			return new ArrayList(this.nodes.values());
		}
		
		protected internal virtual void removeEdge(Edge e)
		{
			this.edges.remove(e);
		}
		
		public virtual void dumpAISee(string fileName, string title)
		{
			try
			{
				java.lang.System.err.println(new StringBuilder().append("Dumping ").append(title).append(" to ").append(fileName).toString());
				FileWriter fileWriter = new FileWriter(fileName);
				fileWriter.write("graph: {\n");
				fileWriter.write(new StringBuilder().append("title: \"").append(title).append("\"\n").toString());
				fileWriter.write("display_edge_labels: yes\n");
				Iterator iterator = this.nodes.values().iterator();
				while (iterator.hasNext())
				{
					Node node = (Node)iterator.next();
					node.dumpAISee(fileWriter);
				}
				iterator = this.edges.iterator();
				while (iterator.hasNext())
				{
					Edge edge = (Edge)iterator.next();
					edge.dumpAISee(fileWriter);
				}
				fileWriter.write("}\n");
				fileWriter.close();
			}
			catch (IOException ex)
			{
				throw new Error(Throwable.instancehelper_toString(ex));
			}			
		}
		
		public virtual void dumpDot(string fileName, string title)
		{
			try
			{
				java.lang.System.err.println(new StringBuilder().append("Dumping ").append(title).append(" to ").append(fileName).toString());
				FileWriter fileWriter = new FileWriter(fileName);
				fileWriter.write(new StringBuilder().append("digraph \"").append(title).append("\" {\n").toString());
				fileWriter.write("rankdir = LR\n");
				Iterator iterator = this.nodes.values().iterator();
				while (iterator.hasNext())
				{
					Node node = (Node)iterator.next();
					node.dumpDot(fileWriter);
				}
				iterator = this.edges.iterator();
				while (iterator.hasNext())
				{
					Edge edge = (Edge)iterator.next();
					edge.dumpDot(fileWriter);
				}
				fileWriter.write("}\n");
				fileWriter.close();
			}
			catch (IOException ex)
			{
				throw new Error(Throwable.instancehelper_toString(ex));
			}

		}
		
		public virtual void dumpSlf(Writer w)
		{
			w.write("VERSION=1.1\n");
			w.write("UTTERANCE=test\n");
			w.write("base=1.0001\n");
			w.write("lmscale=9.5\n");
			w.write("start=0\n");
			w.write("end=1\n");
			w.write("#\n# Size line.\n#\n");
			w.write(new StringBuilder().append("NODES=").append(this.nodes.size()).append("    LINKS=").append(this.edges.size()).append("\n").toString());
			HashMap hashMap = new HashMap();
			hashMap.put(this.initialNode.getId(), Integer.valueOf(0));
			hashMap.put(this.terminalNode.getId(), Integer.valueOf(1));
			int num = 2;
			w.write("#\n# Node definitions.\n#\n");
			Iterator iterator = this.nodes.values().iterator();
			while (iterator.hasNext())
			{
				Node node = (Node)iterator.next();
				if (hashMap.containsKey(node.getId()))
				{
					w.write(new StringBuilder().append("I=").append(hashMap.get(node.getId())).toString());
				}
				else
				{
					hashMap.put(node.getId(), Integer.valueOf(num));
					w.write(new StringBuilder().append("I=").append(num).toString());
					num++;
				}
				w.write(new StringBuilder().append("    t=").append((double)node.getBeginTime() * (double)1f / 1000.0).toString());
				string text = node.getWord().getSpelling();
				if (java.lang.String.instancehelper_startsWith(text, "<"))
				{
					text = "!NULL";
				}
				w.write(new StringBuilder().append("    W=").append(text).toString());
				w.write("\n");
			}
			w.write("#\n# Link definitions.\n#\n");
			num = 0;
			iterator = this.edges.iterator();
			while (iterator.hasNext())
			{
				Edge edge = (Edge)iterator.next();
				w.write(new StringBuilder().append("J=").append(num).toString());
				w.write(new StringBuilder().append("    S=").append(hashMap.get(edge.getFromNode().getId())).toString());
				w.write(new StringBuilder().append("    E=").append(hashMap.get(edge.getToNode().getId())).toString());
				w.write(new StringBuilder().append("    a=").append(edge.getAcousticScore()).toString());
				w.write(new StringBuilder().append("    l=").append(edge.getLMScore() / 9.5).toString());
				w.write("\n");
				num++;
			}
			w.flush();
		}
		
		public virtual void dump(string file)
		{
			try
			{
				this.dump(new PrintWriter(new FileWriter(file)));
			}
			catch (IOException ex)
			{
				throw new Error(Throwable.instancehelper_toString(ex));
			}
		}
		
		protected internal virtual void removeNodeAndCrossConnectEdges(Node n)
		{
			java.lang.System.err.println(new StringBuilder().append("Removing node ").append(n).append(" and cross connecting edges").toString());
			Iterator iterator = n.getEnteringEdges().iterator();
			while (iterator.hasNext())
			{
				Edge edge = (Edge)iterator.next();
				Iterator iterator2 = n.getLeavingEdges().iterator();
				while (iterator2.hasNext())
				{
					Edge edge2 = (Edge)iterator2.next();
					this.addEdge(edge.getFromNode(), edge2.getToNode(), edge.getAcousticScore(), edge.getLMScore());
				}
			}
			this.removeNodeAndEdges(n);
			if (!Lattice.assertionsDisabled && !this.checkConsistency())
			{
				
				throw new AssertionError();
			}
		}
		
		public virtual void dumpAllPaths()
		{
			Iterator iterator = this.allPaths().iterator();
			while (iterator.hasNext())
			{
				string text = (string)iterator.next();
				java.lang.System.@out.println(text);
			}
		}
		
		public virtual bool isEquivalent(Lattice other)
		{
			return this.checkNodesEquivalent(this.initialNode, other.getInitialNode());
		}

		internal virtual bool isFillerNode(Node node)
		{
			Word word = node.getWord();
			return !word.isSentenceStartWord() && !word.isSentenceEndWord() && word.isFiller();
		}

		protected internal Node initialNode;

		protected internal Node terminalNode;

		protected internal Set edges;
		
		protected internal Map nodes;

		protected internal double logBase;

		protected internal LogMath logMath;

		private bool wordTokenFirst;
		
		private Set visitedWordTokens;

		private AlternateHypothesisManager loserManager;

		
		internal static bool assertionsDisabled = !ClassLiteral<Lattice>.Value.desiredAssertionStatus();
	}
}
