using System;

using edu.cmu.sphinx.fst.semiring;
using edu.cmu.sphinx.fst.utils;
using IKVM.Attributes;
using java.lang;
using java.util;

namespace edu.cmu.sphinx.fst.operations
{
	public class Determinize : java.lang.Object
	{
		
		[LineNumberTable(new byte[]
		{
			23,
			134,
			126,
			105,
			140,
			127,
			38,
			101
		})]
		
		private static State getStateLabel(ArrayList arrayList, HashMap hashMap)
		{
			StringBuilder stringBuilder = new StringBuilder();
			Iterator iterator = arrayList.iterator();
			while (iterator.hasNext())
			{
				Pair pair = (Pair)iterator.next();
				if (stringBuilder.length() > 0)
				{
					stringBuilder.append(",");
				}
				stringBuilder.append(new StringBuilder().append("(").append(pair.getLeft()).append(",").append(pair.getRight()).append(")").toString());
			}
			return (State)hashMap.get(stringBuilder.toString());
		}

		
		[LineNumberTable(new byte[]
		{
			5,
			134,
			126,
			140,
			104,
			105,
			106,
			116,
			243,
			61,
			232,
			70,
			101
		})]
		
		private static ArrayList getUniqueLabels(Fst fst, ArrayList arrayList)
		{
			ArrayList arrayList2 = new ArrayList();
			Iterator iterator = arrayList.iterator();
			while (iterator.hasNext())
			{
				Pair pair = (Pair)iterator.next();
				State state = (State)pair.getLeft();
				int numArcs = state.getNumArcs();
				for (int i = 0; i < numArcs; i++)
				{
					Arc arc = state.getArc(i);
					if (!arrayList2.contains(Integer.valueOf(arc.getIlabel())))
					{
						arrayList2.add(Integer.valueOf(arc.getIlabel()));
					}
				}
			}
			return arrayList2;
		}

		
		[LineNumberTable(new byte[]
		{
			159,
			179,
			98,
			123,
			120,
			98,
			130,
			130,
			99,
			104,
			168
		})]
		
		private static Pair getPair(ArrayList arrayList, State state, Float right)
		{
			Pair pair = null;
			Iterator iterator = arrayList.iterator();
			while (iterator.hasNext())
			{
				Pair pair2 = (Pair)iterator.next();
				if (state.getId() == ((State)pair2.getLeft()).getId())
				{
					pair = pair2;
					break;
				}
			}
			if (pair == null)
			{
				pair = new Pair(state, right);
				arrayList.add(pair);
			}
			return pair;
		}

		[LineNumberTable(new byte[]
		{
			159,
			173,
			134
		})]
		
		private Determinize()
		{
		}

		[LineNumberTable(new byte[]
		{
			44,
			136,
			194,
			103,
			103,
			108,
			172,
			134,
			134,
			109,
			127,
			33,
			108,
			127,
			8,
			104,
			107,
			136,
			107,
			109,
			106,
			106,
			127,
			9,
			141,
			127,
			4,
			110,
			110,
			105,
			105,
			107,
			107,
			107,
			49,
			236,
			61,
			232,
			71,
			197,
			103,
			127,
			4,
			110,
			110,
			122,
			105,
			108,
			107,
			110,
			105,
			101,
			42,
			135,
			101,
			114,
			104,
			49,
			5,
			239,
			58,
			235,
			76,
			165,
			103,
			127,
			4,
			110,
			110,
			110,
			157,
			127,
			32,
			133,
			109,
			109,
			104,
			139,
			110,
			127,
			1,
			117,
			54,
			145,
			98,
			142,
			137,
			127,
			5,
			101,
			133
		})]
		
		public static Fst get(Fst fst)
		{
			if (fst.getSemiring() == null)
			{
				return null;
			}
			Semiring semiring = fst.getSemiring();
			Fst fst2 = new Fst(semiring);
			fst2.setIsyms(fst.getIsyms());
			fst2.setOsyms(fst.getOsyms());
			LinkedList linkedList = new LinkedList();
			HashMap hashMap = new HashMap();
			State state = new State(semiring.zero());
			string text = new StringBuilder().append("(").append(fst.getStart()).append(",").append(semiring.one()).append(")").toString();
			linkedList.add(new ArrayList());
			((ArrayList)linkedList.peek()).add(new Pair(fst.getStart(), Float.valueOf(semiring.one())));
			fst2.addState(state);
			hashMap.put(text, state);
			fst2.setStart(state);
			while (!linkedList.isEmpty())
			{
				ArrayList arrayList = (ArrayList)linkedList.remove();
				State stateLabel = Determinize.getStateLabel(arrayList, hashMap);
				ArrayList uniqueLabels = Determinize.getUniqueLabels(fst, arrayList);
				Iterator iterator = uniqueLabels.iterator();
				while (iterator.hasNext())
				{
					int num = ((Integer)iterator.next()).intValue();
					Float @float = Float.valueOf(semiring.zero());
					Iterator iterator2 = arrayList.iterator();
					while (iterator2.hasNext())
					{
						Pair pair = (Pair)iterator2.next();
						State state2 = (State)pair.getLeft();
						Float float2 = (Float)pair.getRight();
						int numArcs = state2.getNumArcs();
						for (int i = 0; i < numArcs; i++)
						{
							Arc arc = state2.getArc(i);
							if (num == arc.getIlabel())
							{
								@float = Float.valueOf(semiring.plus(@float.floatValue(), semiring.times(float2.floatValue(), arc.getWeight())));
							}
						}
					}
					ArrayList arrayList2 = new ArrayList();
					Iterator iterator3 = arrayList.iterator();
					while (iterator3.hasNext())
					{
						Pair pair2 = (Pair)iterator3.next();
						State state3 = (State)pair2.getLeft();
						Float float3 = (Float)pair2.getRight();
						Float float4 = Float.valueOf(semiring.divide(semiring.one(), @float.floatValue()));
						int numArcs2 = state3.getNumArcs();
						for (int j = 0; j < numArcs2; j++)
						{
							Arc arc2 = state3.getArc(j);
							if (num == arc2.getIlabel())
							{
								State nextState = arc2.getNextState();
								Pair pair3 = Determinize.getPair(arrayList2, nextState, Float.valueOf(semiring.zero()));
								pair3.setRight(Float.valueOf(semiring.plus(((Float)pair3.getRight()).floatValue(), semiring.times(float4.floatValue(), semiring.times(float3.floatValue(), arc2.getWeight())))));
							}
						}
					}
					string text2 = "";
					Iterator iterator4 = arrayList2.iterator();
					while (iterator4.hasNext())
					{
						Pair pair4 = (Pair)iterator4.next();
						State state4 = (State)pair4.getLeft();
						Float float4 = (Float)pair4.getRight();
						if (!java.lang.String.instancehelper_equals(text2, ""))
						{
							text2 = new StringBuilder().append(text2).append(",").toString();
						}
						text2 = new StringBuilder().append(text2).append("(").append(state4).append(",").append(float4).append(")").toString();
					}
					if (hashMap.get(text2) == null)
					{
						State state2 = new State(semiring.zero());
						fst2.addState(state2);
						hashMap.put(text2, state2);
						Float float2 = Float.valueOf(state2.getFinalWeight());
						Iterator iterator5 = arrayList2.iterator();
						while (iterator5.hasNext())
						{
							Pair pair5 = (Pair)iterator5.next();
							float2 = Float.valueOf(semiring.plus(float2.floatValue(), semiring.times(((State)pair5.getLeft()).getFinalWeight(), ((Float)pair5.getRight()).floatValue())));
						}
						state2.setFinalWeight(float2.floatValue());
						linkedList.add(arrayList2);
					}
					stateLabel.addArc(new Arc(num, num, @float.floatValue(), (State)hashMap.get(text2)));
				}
			}
			return fst2;
		}
	}
}
