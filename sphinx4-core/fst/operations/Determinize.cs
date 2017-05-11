using edu.cmu.sphinx.fst.semiring;
using edu.cmu.sphinx.fst.utils;
using java.lang;
using java.util;

namespace edu.cmu.sphinx.fst.operations
{
	public class Determinize : java.lang.Object
	{		
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
		
		private Determinize()
		{
		}
		
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
