using edu.cmu.sphinx.linguist.acoustic;
using edu.cmu.sphinx.linguist.dictionary;
using edu.cmu.sphinx.linguist.language.grammar;
using ikvm.@internal;
using IKVM.Runtime;
using java.lang;
using java.util;

namespace edu.cmu.sphinx.linguist.flat
{
	public class GrammarPoint : java.lang.Object
	{
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
		
		private Unit getUnitOrFill()
		{
			Unit unit = this.getUnit();
			if (unit == null)
			{
				unit = UnitManager.__SILENCE;
			}
			return unit;
		}
		
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
		
		public GrammarPoint(GrammarNode node) : this(node, -1, 0, 0, 0)
		{
		}
		
		public GrammarPoint(PronunciationState state, int which) : this(state)
		{
			this.unitIndex = which;
		}

		internal static void setBounded(bool flag)
		{
			GrammarPoint.bounded = flag;
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
