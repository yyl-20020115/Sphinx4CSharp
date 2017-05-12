using edu.cmu.sphinx.util;
using ikvm.@internal;
using IKVM.Runtime;
using java.lang;
using java.util;

namespace edu.cmu.sphinx.alignment
{
	public class LongTextAligner : Object
	{
		internal static List access_300(LongTextAligner longTextAligner)
		{
			return longTextAligner.reftup;
		}

		internal static HashMap access_400(LongTextAligner longTextAligner)
		{
			return longTextAligner.tupleIndex;
		}

		private List getTuples(List list)
		{
			ArrayList arrayList = new ArrayList();
			LinkedList linkedList = new LinkedList();
			Iterator iterator = list.iterator();
			for (int i = 0; i < this.tupleSize - 1; i++)
			{
				linkedList.add(iterator.next());
			}
			while (iterator.hasNext())
			{
				linkedList.addLast(iterator.next());
				arrayList.add(Utilities.join(linkedList));
				linkedList.removeFirst();
			}
			return arrayList;
		}

		public virtual int[] align(List words, Range range)
		{
			if (range.upperEndpoint() - range.lowerEndpoint() < this.tupleSize || words.size() < this.tupleSize)
			{
				return LongTextAligner.alignTextSimple(this.refWords.subList(range.lowerEndpoint(), range.upperEndpoint()), words, range.lowerEndpoint());
			}
			int[] array = new int[words.size()];
			Arrays.fill(array, -1);
			int i = 0;
			Iterator iterator = new LongTextAligner.Alignment(this, this.getTuples(words), range).getIndices().iterator();
			while (iterator.hasNext())
			{
				LongTextAligner_Alignment_Node longTextAligner_Alignment_Node = (LongTextAligner_Alignment_Node)iterator.next();
				for (i = java.lang.Math.max(i, longTextAligner_Alignment_Node.getQueryIndex()); i < longTextAligner_Alignment_Node.getQueryIndex() + this.tupleSize; i++)
				{
					array[i] = longTextAligner_Alignment_Node.getDatabaseIndex() + i - longTextAligner_Alignment_Node.getQueryIndex();
				}
			}
			return array;
		}

		internal static int[] alignTextSimple(List list, List list2, int num)
		{
			int num2 = list.size() + 1;
			int i = list2.size() + 1;
			int num3 = num2;
			int num4 = i;
			int[] array = new int[2];
			int num5 = num4;
			array[1] = num5;
			num5 = num3;
			array[0] = num5;
			int[][] array2 = (int[][])ByteCodeHelper.multianewarray(typeof(int[][]).TypeHandle, array);
			array2[0][0] = 0;
			for (int j = 1; j < num2; j++)
			{
				array2[j][0] = j;
			}
			for (int j = 1; j < i; j++)
			{
				array2[0][j] = j;
			}
			for (int j = 1; j < num2; j++)
			{
				for (int k = 1; k < i; k++)
				{
					int num6 = array2[j - 1][k - 1];
					string text = (string)list.get(j - 1);
					string text2 = (string)list2.get(k - 1);
					if (!java.lang.String.instancehelper_equals(text, text2))
					{
						num6++;
					}
					int num7 = array2[j][k - 1] + 1;
					int num8 = array2[j - 1][k] + 1;
					array2[j][k] = java.lang.Math.min(num6, java.lang.Math.min(num7, num8));
				}
			}
			num2 --;
			i --;
			int[] array3 = new int[i];
			Arrays.fill(array3, -1);
			while (i > 0)
			{
				if (num2 == 0)
				{
					i --;
				}
				else
				{
					string text3 = (string)list.get(num2 - 1);
					string text4 = (string)list2.get(i - 1);
					if (array2[num2 - 1][i - 1] <= array2[num2 - 1][i - 1] && array2[num2 - 1][i - 1] <= array2[num2][i - 1] && java.lang.String.instancehelper_equals(text3, text4))
					{
						int[] array4 = array3;
						i --;
						int num9 = i;
						num2 --;
						array4[num9] = num2 + num;
					}
					else if (array2[num2 - 1][i] < array2[num2][i - 1])
					{
						num2 --;
					}
					else
					{
						i --;
					}
				}
			}
			return array3;
		}

		public LongTextAligner(List words, int tupleSize)
		{
			if (!LongTextAligner.assertionsDisabled && words == null)
			{

				throw new AssertionError();
			}
			if (!LongTextAligner.assertionsDisabled && tupleSize <= 0)
			{

				throw new AssertionError();
			}
			this.tupleSize = tupleSize;
			this.refWords = words;
			int num = 0;
			this.reftup = this.getTuples(words);
			this.tupleIndex = new HashMap();
			Iterator iterator = this.reftup.iterator();
			while (iterator.hasNext())
			{
				string text = (string)iterator.next();
				ArrayList arrayList = (ArrayList)this.tupleIndex.get(text);
				if (arrayList == null)
				{
					arrayList = new ArrayList();
					this.tupleIndex.put(text, arrayList);
				}
				ArrayList arrayList2 = arrayList;
				int num2 = num;
				num++;
				arrayList2.add(Integer.valueOf(num2));
			}
		}

		public virtual int[] align(List query)
		{
			return this.align(query, new Range(0, this.refWords.size()));
		}

		private int tupleSize;

		private List reftup;

		private HashMap tupleIndex;

		private List refWords;

		internal static bool assertionsDisabled = !ClassLiteral<LongTextAligner>.Value.desiredAssertionStatus();

		internal sealed class Alignment : java.lang.Object
		{
			internal static List access_000(LongTextAligner.Alignment alignment)
			{
				return alignment.shifts;
			}

			internal static List access_100(LongTextAligner.Alignment alignment)
			{
				return alignment.indices;
			}

			internal static List access_200(LongTextAligner.Alignment alignment)
			{
				return alignment.query;
			}

			public Alignment(LongTextAligner longTextAligner, List list, Range range)
			{
				this_0 = longTextAligner;
				this.query = list;
				this.indices = new ArrayList();
				TreeSet treeSet = new TreeSet();
				for (int i = 0; i < list.size(); i++)
				{
					if (LongTextAligner.access_400(longTextAligner).containsKey(list.get(i)))
					{
						this.indices.add(Integer.valueOf(i));
						Iterator iterator = ((ArrayList)LongTextAligner.access_400(longTextAligner).get(list.get(i))).iterator();
						while (iterator.hasNext())
						{
							Integer integer = (Integer)iterator.next();
							if (range.contains(integer.intValue()))
							{
								treeSet.add(integer);
							}
						}
					}
				}
				this.shifts = new ArrayList(treeSet);
				HashMap hashMap = new HashMap();
				PriorityQueue priorityQueue = new PriorityQueue(1, new LongTextAligner_Alignment_1(this, longTextAligner, hashMap));
				HashSet hashSet = new HashSet();
				HashMap hashMap2 = new HashMap();
				LongTextAligner_Alignment_Node longTextAligner_Alignment_Node = new LongTextAligner_Alignment_Node(this, 0, 0, null);
				hashMap.put(longTextAligner_Alignment_Node, Integer.valueOf(0));
				priorityQueue.add(longTextAligner_Alignment_Node);
				while (!priorityQueue.isEmpty())
				{
					LongTextAligner_Alignment_Node longTextAligner_Alignment_Node2 = (LongTextAligner_Alignment_Node)priorityQueue.poll();
					if (!hashSet.contains(longTextAligner_Alignment_Node2))
					{
						if (longTextAligner_Alignment_Node2.isTarget())
						{
							ArrayList arrayList = new ArrayList();
							while (hashMap2.containsKey(longTextAligner_Alignment_Node2))
							{
								if (!longTextAligner_Alignment_Node2.isBoundary() && longTextAligner_Alignment_Node2.hasMatch())
								{
									arrayList.add(longTextAligner_Alignment_Node2);
								}
								longTextAligner_Alignment_Node2 = (LongTextAligner_Alignment_Node)hashMap2.get(longTextAligner_Alignment_Node2);
							}
							this.alignment = new ArrayList(arrayList);
							Collections.reverse(this.alignment);
							return;
						}
						hashSet.add(longTextAligner_Alignment_Node2);
						Iterator iterator2 = longTextAligner_Alignment_Node2.adjacent().iterator();
						while (iterator2.hasNext())
						{
							LongTextAligner_Alignment_Node longTextAligner_Alignment_Node3 = (LongTextAligner_Alignment_Node)iterator2.next();
							if (!hashSet.contains(longTextAligner_Alignment_Node3))
							{
								int num = java.lang.Math.abs(this.indices.size() - this.shifts.size() - LongTextAligner_Alignment_Node.access_600(longTextAligner_Alignment_Node2) + LongTextAligner_Alignment_Node.access_700(longTextAligner_Alignment_Node2)) - java.lang.Math.abs(this.indices.size() - this.shifts.size() - LongTextAligner_Alignment_Node.access_600(longTextAligner_Alignment_Node3) + LongTextAligner_Alignment_Node.access_700(longTextAligner_Alignment_Node3));
								Integer integer2 = (Integer)hashMap.get(longTextAligner_Alignment_Node3);
								Integer integer3 = (Integer)hashMap.get(longTextAligner_Alignment_Node2);
								if (integer2 == null)
								{
									integer2 = Integer.valueOf(int.MaxValue);
								}
								if (integer3 == null)
								{
									integer3 = Integer.valueOf(int.MaxValue);
								}
								int num2 = integer3.intValue() + longTextAligner_Alignment_Node3.getValue() - num;
								if (num2 < integer2.intValue())
								{
									hashMap.put(longTextAligner_Alignment_Node3, Integer.valueOf(num2));
									priorityQueue.add(longTextAligner_Alignment_Node3);
									hashMap2.put(longTextAligner_Alignment_Node3, longTextAligner_Alignment_Node2);
								}
							}
						}
					}
				}
				this.alignment = Collections.emptyList();
			}

			public List getIndices()
			{
				return this.alignment;
			}

			private List shifts;

			private List query;

			private List indices;

			private List alignment;

			internal LongTextAligner this_0;
		}
	}
}
