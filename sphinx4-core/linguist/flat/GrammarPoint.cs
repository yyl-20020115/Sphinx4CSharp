using System;

using edu.cmu.sphinx.linguist.acoustic;
using edu.cmu.sphinx.linguist.dictionary;
using edu.cmu.sphinx.linguist.language.grammar;
using IKVM.Attributes;
using ikvm.@internal;
using IKVM.Runtime;
using java.lang;
using java.util;

namespace edu.cmu.sphinx.linguist.flat
{
	public class GrammarPoint : java.lang.Object
	{
		
		public static void __<clinit>()
		{
		}

		[LineNumberTable(new byte[]
		{
			49,
			104,
			117,
			103,
			103,
			103,
			104,
			104
		})]
		
		public GrammarPoint(GrammarNode node, int alternativeIndex, int wordIndex, int pronunciationIndex, int unitIndex)
		{
			if (!GrammarPoint.assertionsDisabled && node == null)
			{
				
				throw new AssertionError();
			}
			this.node = node;
			this.alternativeIndex = alternativeIndex;
			this.wordIndex = wordIndex;
			this.pronunciationIndex = pronunciationIndex;
			this.unitIndex = unitIndex;
		}

		[LineNumberTable(new byte[]
		{
			159,
			189,
			104,
			102,
			104,
			110,
			104,
			110,
			104,
			110,
			104,
			110,
			104,
			145,
			141,
			122
		})]
		
		public GrammarPoint(SentenceHMMState state)
		{
			while (state != null)
			{
				if (state is UnitState)
				{
					this.unitIndex = state.getWhich();
				}
				else if (state is PronunciationState)
				{
					this.pronunciationIndex = state.getWhich();
				}
				else if (state is WordState)
				{
					this.wordIndex = state.getWhich();
				}
				else if (state is AlternativeState)
				{
					this.alternativeIndex = state.getWhich();
				}
				else if (state is GrammarState)
				{
					this.node = ((GrammarState)state).getGrammarNode();
				}
				state = state.getParent();
			}
			if (!GrammarPoint.assertionsDisabled && this.node == null)
			{
				
				throw new AssertionError();
			}
		}

		[LineNumberTable(new byte[]
		{
			65,
			98,
			108,
			118,
			105,
			106,
			104,
			102,
			106,
			104,
			103,
			107,
			234,
			69
		})]
		
		private Unit getUnit()
		{
			Unit result = null;
			Word[][] alternatives = this.node.getAlternatives();
			if (this.alternativeIndex != -1 && this.alternativeIndex < alternatives.Length)
			{
				Word[] array = alternatives[this.alternativeIndex];
				if (this.wordIndex < array.Length)
				{
					Pronunciation[] pronunciations = array[this.wordIndex].getPronunciations();
					if (this.pronunciationIndex < pronunciations.Length)
					{
						Unit[] units = pronunciations[this.pronunciationIndex].getUnits();
						if (this.unitIndex < units.Length)
						{
							result = units[this.unitIndex];
						}
					}
				}
			}
			return result;
		}

		
		[LineNumberTable(new byte[]
		{
			159,
			88,
			130,
			102,
			226,
			69,
			120,
			113,
			112,
			8,
			235,
			75,
			110,
			241,
			71,
			99,
			133,
			191,
			8,
			102,
			115,
			135,
			145,
			107,
			142,
			104,
			103,
			110,
			126,
			108,
			143,
			159,
			0,
			233,
			61,
			232,
			69,
			105,
			205
		})]
		
		private List getNextGrammarPoints(bool flag)
		{
			ArrayList arrayList = new ArrayList();
			if (this.alternativeIndex == -1 && this.node.getAlternatives().Length > 0)
			{
				for (int i = 0; i < this.node.getAlternatives().Length; i++)
				{
					GrammarPoint grammarPoint = new GrammarPoint(this.node, i, 0, 0, 0);
					arrayList.add(grammarPoint);
				}
			}
			else if (this.node.getAlternatives().Length == 0)
			{
				GrammarPoint.addNextGrammarPointsWithWords(this.node, arrayList);
			}
			else
			{
				GrammarPoint grammarPoint2;
				if (flag)
				{
					grammarPoint2 = this;
				}
				else
				{
					grammarPoint2 = new GrammarPoint(this.node, this.alternativeIndex, this.wordIndex, this.pronunciationIndex, this.unitIndex + 1);
				}
				Pronunciation[] pronunciations = this.node.getAlternatives()[this.alternativeIndex][this.wordIndex].getPronunciations();
				int num = pronunciations[this.pronunciationIndex].getUnits().Length;
				if (grammarPoint2.unitIndex < num)
				{
					arrayList.add(grammarPoint2);
				}
				else
				{
					grammarPoint2.unitIndex = 0;
					Word[] array = grammarPoint2.node.getAlternatives()[this.alternativeIndex];
					GrammarPoint grammarPoint3 = grammarPoint2;
					int num2 = grammarPoint3.wordIndex + 1;
					GrammarPoint grammarPoint4 = grammarPoint3;
					int num3 = num2;
					grammarPoint4.wordIndex = num2;
					if (num3 < array.Length)
					{
						Word word = array[grammarPoint2.wordIndex];
						for (int j = 0; j < word.getPronunciations().Length; j++)
						{
							GrammarPoint grammarPoint5 = new GrammarPoint(grammarPoint2.node, grammarPoint2.alternativeIndex, grammarPoint2.wordIndex, j, 0);
							arrayList.add(grammarPoint5);
						}
					}
					else if (!GrammarPoint.bounded)
					{
						GrammarPoint.addNextGrammarPointsWithWords(grammarPoint2.node, arrayList);
					}
				}
			}
			return arrayList;
		}

		
		[LineNumberTable(new byte[]
		{
			160,
			87,
			123,
			105,
			129,
			98,
			104
		})]
		
		private void addContext(List list, Unit[] array)
		{
			Iterator iterator = list.iterator();
			while (iterator.hasNext())
			{
				Unit[] a = (Unit[])iterator.next();
				if (Unit.isContextMatch(a, array))
				{
					return;
				}
			}
			list.add(array);
		}

		[LineNumberTable(new byte[]
		{
			91,
			103,
			99,
			134
		})]
		
		private Unit getUnitOrFill()
		{
			Unit unit = this.getUnit();
			if (unit == null)
			{
				unit = UnitManager.__SILENCE;
			}
			return unit;
		}

		
		[LineNumberTable(new byte[]
		{
			159,
			102,
			98,
			102,
			136,
			104,
			103,
			104,
			101,
			127,
			3,
			100,
			104,
			107,
			105,
			101,
			104,
			38,
			135,
			127,
			1,
			108,
			107,
			110,
			105,
			130,
			105,
			130,
			133
		})]
		
		public virtual List getRightContexts(int size, bool startWithCurrent, int maxContexts)
		{
			ArrayList arrayList = new ArrayList();
			List nextGrammarPoints = this.getNextGrammarPoints(startWithCurrent);
			if (nextGrammarPoints.isEmpty())
			{
				Unit[] emptyContext = Unit.getEmptyContext(size);
				this.addContext(arrayList, emptyContext);
			}
			else
			{
				Iterator iterator = nextGrammarPoints.iterator();
				while (iterator.hasNext())
				{
					GrammarPoint grammarPoint = (GrammarPoint)iterator.next();
					if (size == 1)
					{
						Unit[] array = new Unit[size];
						array[0] = grammarPoint.getUnitOrFill();
						this.addContext(arrayList, array);
					}
					else
					{
						List rightContexts = grammarPoint.getRightContexts(size - 1, false, maxContexts - arrayList.size());
						Iterator iterator2 = rightContexts.iterator();
						while (iterator2.hasNext())
						{
							Unit[] array2 = (Unit[])iterator2.next();
							Unit[] emptyContext2 = Unit.getEmptyContext(array2.Length + 1);
							emptyContext2[0] = grammarPoint.getUnitOrFill();
							ByteCodeHelper.arraycopy(array2, 0, emptyContext2, 1, array2.Length);
							this.addContext(arrayList, emptyContext2);
						}
					}
					if (arrayList.size() >= maxContexts)
					{
						break;
					}
				}
			}
			return arrayList;
		}

		
		[LineNumberTable(new byte[]
		{
			160,
			198,
			127,
			1,
			108,
			107,
			8,
			198,
			98
		})]
		
		private static void addNextGrammarPointsWithWords(GrammarNode grammarNode, List list)
		{
			Iterator iterator = GrammarPoint.getNextGrammarNodesWithWords(grammarNode).iterator();
			while (iterator.hasNext())
			{
				GrammarNode grammarNode2 = (GrammarNode)iterator.next();
				for (int i = 0; i < grammarNode2.getAlternatives().Length; i++)
				{
					GrammarPoint grammarPoint = new GrammarPoint(grammarNode2, i, 0, 0, 0);
					list.add(grammarPoint);
				}
			}
		}

		
		[LineNumberTable(new byte[]
		{
			160,
			172,
			134,
			120,
			105,
			106,
			105,
			139,
			176,
			233,
			55,
			233,
			76
		})]
		
		private static List getNextGrammarNodesWithWords(GrammarNode grammarNode)
		{
			ArrayList arrayList = new ArrayList();
			GrammarArc[] successors = grammarNode.getSuccessors();
			int num = successors.Length;
			for (int i = 0; i < num; i++)
			{
				GrammarArc grammarArc = successors[i];
				GrammarNode grammarNode2 = grammarArc.getGrammarNode();
				if (grammarNode2.getAlternatives().Length == 0)
				{
					if (grammarNode2.isFinalNode())
					{
						arrayList.add(grammarNode2);
					}
					else
					{
						arrayList.addAll(GrammarPoint.getNextGrammarNodesWithWords(grammarNode2));
					}
				}
				else
				{
					arrayList.add(grammarNode2);
				}
			}
			return arrayList;
		}

		[LineNumberTable(new byte[]
		{
			23,
			107
		})]
		
		public GrammarPoint(GrammarNode node) : this(node, -1, 0, 0, 0)
		{
		}

		[LineNumberTable(new byte[]
		{
			34,
			105,
			103
		})]
		
		public GrammarPoint(PronunciationState state, int which) : this(state)
		{
			this.unitIndex = which;
		}

		internal static void setBounded(bool flag)
		{
			GrammarPoint.bounded = flag;
		}

		
		static GrammarPoint()
		{
		}

		private GrammarNode node;

		private int alternativeIndex;

		private int wordIndex;

		private int pronunciationIndex;

		private int unitIndex;

		private static bool bounded;

		
		internal static bool assertionsDisabled = !ClassLiteral<GrammarPoint>.Value.desiredAssertionStatus();
	}
}
