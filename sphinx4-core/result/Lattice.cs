using System;

using edu.cmu.sphinx.decoder.search;
using edu.cmu.sphinx.linguist.dictionary;
using edu.cmu.sphinx.util;
using IKVM.Attributes;
using ikvm.@internal;
using IKVM.Runtime;
using java.io;
using java.lang;
using java.util;

namespace edu.cmu.sphinx.result
{
	public class Lattice : java.lang.Object
	{
		
		public static void __<clinit>()
		{
		}

		[LineNumberTable(new byte[]
		{
			111,
			104,
			117,
			103,
			102,
			127,
			0,
			127,
			2,
			108,
			140,
			107,
			108,
			108,
			104,
			139,
			135
		})]
		
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
				Node.__<clinit>();
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

		[LineNumberTable(new byte[]
		{
			163,
			89,
			105
		})]
		
		public virtual void computeNodePosteriors(float languageModelWeightAdjustment)
		{
			this.computeNodePosteriors(languageModelWeightAdjustment, false);
		}

		
		[LineNumberTable(new byte[]
		{
			163,
			177,
			103,
			102,
			123,
			122,
			98,
			109,
			98
		})]
		
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

		[LineNumberTable(new byte[]
		{
			161,
			112,
			109,
			103,
			103,
			109
		})]
		
		public virtual Edge addEdge(Node fromNode, Node toNode, double acousticScore, double lmScore)
		{
			Edge edge = new Edge(fromNode, toNode, acousticScore, lmScore);
			fromNode.addLeavingEdge(edge);
			toNode.addEnteringEdge(edge);
			this.edges.add(edge);
			return edge;
		}

		[LineNumberTable(new byte[]
		{
			95,
			104,
			107,
			107,
			107
		})]
		
		public Lattice()
		{
			this.edges = new HashSet();
			this.nodes = new HashMap();
			this.logMath = LogMath.getLogMath();
		}

		[LineNumberTable(new byte[]
		{
			161,
			131,
			107,
			103
		})]
		
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

		[LineNumberTable(new byte[]
		{
			161,
			190,
			127,
			1,
			115
		})]
		
		protected internal virtual void addNode(Node n)
		{
			if (!Lattice.assertionsDisabled && this.hasNode(n.getId()))
			{
				
				throw new AssertionError();
			}
			this.nodes.put(n.getId(), n);
		}

		[LineNumberTable(new byte[]
		{
			160,
			132,
			117,
			110,
			129,
			141,
			125,
			37,
			165,
			118,
			127,
			7,
			122,
			130
		})]
		
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

		[LineNumberTable(new byte[]
		{
			160,
			72,
			138,
			98,
			99,
			99,
			99,
			100,
			105,
			104,
			168,
			104,
			136,
			104,
			139,
			106,
			136
		})]
		
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

		[LineNumberTable(new byte[]
		{
			160,
			97,
			104,
			136
		})]
		
		private TimeFrame getTimeFrame(Token token)
		{
			if (this.wordTokenFirst)
			{
				return this.getTimeFrameWordTokenFirst(token);
			}
			return this.getTimeFrameWordTokenLast(token);
		}

		[LineNumberTable(new byte[]
		{
			160,
			111,
			109,
			135,
			120,
			99,
			104,
			127,
			5,
			135
		})]
		
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
				Node.__<clinit>();
				node = new Node(this.getNodeID(token), token.getWord(), timeFrame.getStart(), timeFrame.getEnd());
				this.addNode(node);
			}
			return node;
		}

		[LineNumberTable(new byte[]
		{
			160,
			161,
			99,
			129,
			232,
			69,
			104,
			112,
			136,
			169,
			127,
			0,
			135,
			225,
			73,
			116,
			109,
			135,
			99,
			129,
			126,
			98,
			99,
			130,
			178,
			118,
			127,
			7,
			109,
			130
		})]
		
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

		[Throws(new string[]
		{
			"java.lang.NumberFormatException",
			"java.io.IOException"
		})]
		[LineNumberTable(new byte[]
		{
			161,
			7,
			102,
			140,
			98,
			98,
			99,
			99,
			107,
			110,
			127,
			1,
			98,
			98,
			130,
			127,
			1,
			98,
			98,
			133,
			142,
			101,
			166,
			110,
			127,
			23,
			102,
			159,
			7,
			145,
			127,
			1,
			108,
			99,
			116,
			103,
			131,
			116,
			103,
			131,
			110,
			103,
			131,
			110,
			99,
			118,
			117,
			110,
			104,
			110,
			104,
			171,
			110,
			127,
			32,
			103,
			102,
			159,
			7,
			108,
			108,
			114,
			117,
			127,
			16,
			133,
			110,
			127,
			33,
			110,
			127,
			33,
			113,
			191,
			39,
			159,
			13,
			127,
			6,
			127,
			1,
			63,
			0,
			34,
			165
		})]
		
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
				CharSequence charSequence;
				charSequence.__ref = obj;
				if (java.lang.String.instancehelper_contains(text2, charSequence))
				{
					num2 = 0;
					num = 1;
				}
				else
				{
					string text3 = text;
					obj = "Link definitions";
					charSequence.__ref = obj;
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
							long beginTime = ByteCodeHelper.d2l(Double.parseDouble(java.lang.String.instancehelper_substring(array[1], 2)) * 1000.0);
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
							Word.__<clinit>();
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
							double acousticScore = Double.parseDouble(java.lang.String.instancehelper_substring(array[3], 2));
							double lmScore = Double.parseDouble(java.lang.String.instancehelper_substring(array[4], 2)) * num5;
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
								charSequence.__ref = obj3;
								CharSequence charSequence2 = charSequence;
								obj3 = obj;
								charSequence.__ref = obj3;
								num3 = Integer.parseInt(java.lang.String.instancehelper_replace(text9, charSequence2, charSequence));
							}
							if (java.lang.String.instancehelper_startsWith(text, "end="))
							{
								string text10 = text;
								object obj4 = "end=";
								object obj3 = "";
								obj = obj4;
								charSequence.__ref = obj;
								CharSequence charSequence3 = charSequence;
								obj = obj3;
								charSequence.__ref = obj;
								num4 = Integer.parseInt(java.lang.String.instancehelper_replace(text10, charSequence3, charSequence));
							}
							if (java.lang.String.instancehelper_startsWith(text, "lmscale="))
							{
								string text11 = text;
								object obj5 = "lmscale=";
								obj = "";
								object obj3 = obj5;
								charSequence.__ref = obj3;
								CharSequence charSequence4 = charSequence;
								obj3 = obj;
								charSequence.__ref = obj3;
								num5 = Double.parseDouble(java.lang.String.instancehelper_replace(text11, charSequence4, charSequence));
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

		[Throws(new string[]
		{
			"java.io.IOException"
		})]
		[LineNumberTable(new byte[]
		{
			162,
			123,
			127,
			6,
			103,
			98,
			127,
			1,
			103,
			98,
			127,
			11,
			127,
			11,
			127,
			11,
			102
		})]
		
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

		[LineNumberTable(new byte[]
		{
			163,
			20,
			127,
			9,
			127,
			1,
			105,
			159,
			19,
			98,
			127,
			1,
			105,
			159,
			19,
			98,
			101,
			127,
			5,
			111,
			159,
			7,
			111,
			159,
			7,
			117,
			159,
			7,
			117,
			159,
			7,
			101
		})]
		
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

		[LineNumberTable(new byte[]
		{
			162,
			158,
			127,
			1,
			140,
			109,
			98,
			127,
			1,
			140,
			109,
			130,
			146,
			122
		})]
		
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

		
		[LineNumberTable(new byte[]
		{
			163,
			7,
			127,
			4,
			102,
			105,
			138,
			127,
			1,
			116,
			130
		})]
		
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

		
		[LineNumberTable(new byte[]
		{
			163,
			50,
			105,
			129,
			104,
			99,
			144,
			127,
			1,
			110,
			98,
			104
		})]
		
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

		[LineNumberTable(new byte[]
		{
			158,
			151,
			162,
			104,
			129,
			112,
			112,
			103,
			127,
			2,
			126,
			127,
			8,
			111,
			110,
			104,
			114,
			49,
			133,
			114,
			126,
			109,
			142,
			101,
			165,
			112,
			127,
			9,
			112,
			108,
			109,
			104,
			127,
			1,
			111,
			113,
			114,
			49,
			133,
			98,
			165,
			110,
			127,
			10,
			121,
			98
		})]
		
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

		
		[LineNumberTable(new byte[]
		{
			163,
			69,
			113,
			114,
			102
		})]
		
		public virtual List sortNodes()
		{
			ArrayList arrayList = new ArrayList(this.nodes.size());
			this.sortHelper(this.initialNode, arrayList, new HashSet());
			Collections.reverse(arrayList);
			return arrayList;
		}

		[LineNumberTable(new byte[]
		{
			158,
			128,
			98,
			99,
			135
		})]
		
		private double computeEdgeScore(Edge edge, float num, bool flag)
		{
			if (flag)
			{
				return edge.getAcousticScore();
			}
			return edge.getAcousticScore() + edge.getLMScore() * (double)num;
		}

		
		[LineNumberTable(new byte[]
		{
			163,
			160,
			102,
			103,
			105,
			103,
			137,
			108
		})]
		
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

		[LineNumberTable(new byte[]
		{
			163,
			230,
			152,
			104,
			102,
			103,
			135,
			159,
			28,
			159,
			0,
			138,
			100,
			111,
			130,
			234,
			69,
			111,
			162,
			119,
			99,
			194,
			101,
			104,
			111,
			162
		})]
		
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

		[LineNumberTable(new byte[]
		{
			160,
			228,
			136,
			191,
			5,
			140,
			109,
			103,
			107,
			136,
			110,
			108,
			110,
			108,
			110,
			119,
			110,
			119,
			110,
			148,
			102,
			191,
			40,
			101,
			191,
			0,
			2,
			98,
			146
		})]
		
		public Lattice(string fileName) : this()
		{
			Exception ex3;
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
							this.logBase = Double.parseDouble(stringTokenizer.nextToken());
						}
					}
				}
				lineNumberReader.close();
			}
			catch (Exception ex)
			{
				Exception ex2 = ByteCodeHelper.MapException<Exception>(ex, 0);
				if (ex2 == null)
				{
					throw;
				}
				ex3 = ex2;
				goto IL_16C;
			}
			return;
			IL_16C:
			Exception ex4 = ex3;
			string text4 = Throwable.instancehelper_toString(ex4);
			
			throw new Error(text4);
		}

		[Throws(new string[]
		{
			"java.io.IOException"
		})]
		[LineNumberTable(new byte[]
		{
			161,
			95,
			103,
			103,
			102
		})]
		
		public static Lattice readSlf(string fileName)
		{
			FileInputStream fileInputStream = new FileInputStream(fileName);
			Lattice result = Lattice.readSlf(fileInputStream);
			fileInputStream.close();
			return result;
		}

		[LineNumberTable(new byte[]
		{
			161,
			148,
			115
		})]
		
		public virtual Node addNode(string id, string word, long beginTime, long endTime)
		{
			Word.__<clinit>();
			Word word2 = new Word(word, new Pronunciation[0], false);
			return this.addNode(id, word2, beginTime, endTime);
		}

		[LineNumberTable(new byte[]
		{
			161,
			200,
			127,
			1,
			114
		})]
		
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

		[LineNumberTable(new byte[]
		{
			161,
			239,
			109
		})]
		
		protected internal virtual void removeEdge(Edge e)
		{
			this.edges.remove(e);
		}

		[LineNumberTable(new byte[]
		{
			162,
			4,
			127,
			21,
			103,
			107,
			127,
			11,
			235,
			80,
			127,
			6,
			103,
			98,
			127,
			1,
			103,
			98,
			107,
			186,
			2,
			98,
			146
		})]
		
		public virtual void dumpAISee(string fileName, string title)
		{
			IOException ex2;
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
				ex2 = ByteCodeHelper.MapException<IOException>(ex, 1);
				goto IL_F3;
			}
			return;
			IL_F3:
			IOException ex3 = ex2;
			string text = Throwable.instancehelper_toString(ex3);
			
			throw new Error(text);
		}

		[LineNumberTable(new byte[]
		{
			162,
			46,
			127,
			21,
			103,
			127,
			11,
			139,
			127,
			6,
			103,
			98,
			127,
			1,
			103,
			98,
			107,
			186,
			2,
			98,
			146
		})]
		
		public virtual void dumpDot(string fileName, string title)
		{
			IOException ex2;
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
				ex2 = ByteCodeHelper.MapException<IOException>(ex, 1);
				goto IL_E8;
			}
			return;
			IL_E8:
			IOException ex3 = ex2;
			string text = Throwable.instancehelper_toString(ex3);
			
			throw new Error(text);
		}

		[Throws(new string[]
		{
			"java.io.IOException"
		})]
		[LineNumberTable(new byte[]
		{
			162,
			65,
			107,
			107,
			107,
			107,
			107,
			107,
			107,
			255,
			47,
			71,
			134,
			120,
			152,
			98,
			107,
			127,
			9,
			110,
			159,
			14,
			115,
			127,
			1,
			132,
			127,
			23,
			109,
			110,
			103,
			127,
			2,
			107,
			101,
			107,
			98,
			127,
			5,
			127,
			1,
			127,
			18,
			127,
			18,
			127,
			7,
			127,
			17,
			107,
			100,
			101,
			102
		})]
		
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

		[LineNumberTable(new byte[]
		{
			162,
			143,
			191,
			4,
			2,
			97,
			145
		})]
		
		public virtual void dump(string file)
		{
			IOException ex2;
			try
			{
				this.dump(new PrintWriter(new FileWriter(file)));
			}
			catch (IOException ex)
			{
				ex2 = ByteCodeHelper.MapException<IOException>(ex, 1);
				goto IL_20;
			}
			return;
			IL_20:
			IOException ex3 = ex2;
			string text = Throwable.instancehelper_toString(ex3);
			
			throw new Error(text);
		}

		[LineNumberTable(new byte[]
		{
			162,
			188,
			127,
			15,
			127,
			1,
			127,
			1,
			127,
			0,
			98,
			101,
			135,
			122
		})]
		
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

		[LineNumberTable(new byte[]
		{
			162,
			241,
			127,
			1,
			107,
			98
		})]
		
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

		[LineNumberTable(new byte[]
		{
			164,
			16,
			103,
			112,
			98
		})]
		
		internal virtual bool isFillerNode(Node node)
		{
			Word word = node.getWord();
			return !word.isSentenceStartWord() && !word.isSentenceEndWord() && word.isFiller();
		}

		
		static Lattice()
		{
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
