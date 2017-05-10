using System;

using edu.cmu.sphinx.linguist;
using edu.cmu.sphinx.linguist.dictionary;
using edu.cmu.sphinx.linguist.language.ngram;
using IKVM.Attributes;
using java.lang;
using java.util;

namespace edu.cmu.sphinx.result
{
	public class LatticeRescorer : java.lang.Object
	{
		
		[LineNumberTable(new byte[]
		{
			23,
			102,
			98,
			114,
			99,
			159,
			14,
			101,
			119,
			138,
			104,
			127,
			10,
			115,
			130
		})]
		
		protected internal virtual List allPathsTo(string path, Edge edge, int currentDepth)
		{
			LinkedList linkedList = new LinkedList();
			string text = path;
			int num = this.__lattice.isFillerNode(edge.getFromNode()) ? 1 : 0;
			if (num == 0)
			{
				text = new StringBuilder().append(edge.getFromNode().getWord().toString()).append(' ').append(text).toString();
			}
			if (currentDepth == 2 || edge.getFromNode().equals(this.__lattice.getInitialNode()))
			{
				linkedList.add(text);
			}
			else
			{
				int num2 = (num == 0) ? 1 : 0;
				Iterator iterator = edge.getFromNode().getEnteringEdges().iterator();
				while (iterator.hasNext())
				{
					Edge edge2 = (Edge)iterator.next();
					linkedList.addAll(this.allPathsTo(text, edge2, currentDepth - num2));
				}
			}
			return linkedList;
		}

		[LineNumberTable(new byte[]
		{
			159,
			189,
			159,
			9,
			102,
			115,
			104,
			162,
			115,
			127,
			3,
			103,
			127,
			4,
			49,
			168,
			147,
			105,
			119,
			101,
			99,
			101,
			104,
			101
		})]
		
		private void rescoreEdges()
		{
			Iterator iterator = this.__lattice.edges.iterator();
			while (iterator.hasNext())
			{
				Edge edge = (Edge)iterator.next();
				float num = float.MinValue;
				if (this.__lattice.isFillerNode(edge.getToNode()))
				{
					edge.setLMScore((double)num);
				}
				else
				{
					List list = this.allPathsTo("", edge, this.depth);
					Iterator iterator2 = list.iterator();
					while (iterator2.hasNext())
					{
						string text = (string)iterator2.next();
						LinkedList linkedList = new LinkedList();
						string[] array = java.lang.String.instancehelper_split(text, " ");
						int num2 = array.Length;
						for (int i = 0; i < num2; i++)
						{
							string spelling = array[i];
							linkedList.add(new Word(spelling, null, false));
						}
						linkedList.add(edge.getToNode().getWord());
						WordSequence ws = new WordSequence(linkedList);
						float num3 = this.__model.getProbability(ws) * this.languageWeigth;
						if (num < num3)
						{
							num = num3;
						}
					}
					edge.setLMScore((double)num);
				}
			}
		}

		[LineNumberTable(new byte[]
		{
			159,
			181,
			232,
			56,
			235,
			73,
			103,
			103,
			108
		})]
		
		public LatticeRescorer(Lattice lattice, LanguageModel model)
		{
			this.languageWeigth = 8f;
			this.__lattice = lattice;
			this.__model = model;
			this.depth = model.getMaxDepth();
		}

		[LineNumberTable(new byte[]
		{
			42,
			102
		})]
		
		public virtual void rescore()
		{
			this.rescoreEdges();
		}

		
		protected internal Lattice lattice
		{
			
			get
			{
				return this.__lattice;
			}
			
			private set
			{
				this.__lattice = value;
			}
		}

		
		protected internal LanguageModel model
		{
			
			get
			{
				return this.__model;
			}
			
			private set
			{
				this.__model = value;
			}
		}

		internal Lattice __lattice;

		internal LanguageModel __model;

		private int depth;

		private float languageWeigth;
	}
}
