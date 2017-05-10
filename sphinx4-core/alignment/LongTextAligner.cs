using System;

using edu.cmu.sphinx.util;
using IKVM.Attributes;
using ikvm.@internal;
using IKVM.Runtime;
using java.lang;
using java.util;

namespace edu.cmu.sphinx.alignment
{
	public class LongTextAligner : java.lang.Object
	{
		
		public static void __<clinit>()
		{
		}

		
		
		internal static List access$300(LongTextAligner longTextAligner)
		{
			return longTextAligner.reftup;
		}

		
		
		internal static HashMap access$400(LongTextAligner longTextAligner)
		{
			return longTextAligner.tupleIndex;
		}

		
		[LineNumberTable(new byte[]
		{
			160,
			170,
			102,
			134,
			103,
			109,
			45,
			166,
			104,
			108,
			109,
			137
		})]
		
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

		
		[LineNumberTable(new byte[]
		{
			160,
			145,
			127,
			4,
			191,
			5,
			108,
			103,
			98,
			110,
			37,
			186,
			109,
			112,
			107,
			7,
			166,
			101
		})]
		
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
				LongTextAligner$Alignment$Node longTextAligner$Alignment$Node = (LongTextAligner$Alignment$Node)iterator.next();
				for (i = java.lang.Math.max(i, longTextAligner$Alignment$Node.getQueryIndex()); i < longTextAligner$Alignment$Node.getQueryIndex() + this.tupleSize; i++)
				{
					array[i] = longTextAligner$Alignment$Node.getDatabaseIndex() + i - longTextAligner$Alignment$Node.getQueryIndex();
				}
			}
			return array;
		}

		
		[LineNumberTable(new byte[]
		{
			160,
			187,
			105,
			105,
			159,
			6,
			103,
			104,
			41,
			200,
			104,
			41,
			200,
			107,
			107,
			110,
			113,
			113,
			107,
			134,
			110,
			110,
			248,
			55,
			43,
			235,
			78,
			100,
			100,
			104,
			104,
			103,
			99,
			134,
			112,
			112,
			159,
			15,
			103,
			145,
			114,
			134,
			164,
			165
		})]
		
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
			num2 += -1;
			i += -1;
			int[] array3 = new int[i];
			Arrays.fill(array3, -1);
			while (i > 0)
			{
				if (num2 == 0)
				{
					i += -1;
				}
				else
				{
					string text3 = (string)list.get(num2 - 1);
					string text4 = (string)list2.get(i - 1);
					if (array2[num2 - 1][i - 1] <= array2[num2 - 1][i - 1] && array2[num2 - 1][i - 1] <= array2[num2][i - 1] && java.lang.String.instancehelper_equals(text3, text4))
					{
						int[] array4 = array3;
						i += -1;
						int num9 = i;
						num2 += -1;
						array4[num9] = num2 + num;
					}
					else if (array2[num2 - 1][i] < array2[num2][i - 1])
					{
						num2 += -1;
					}
					else
					{
						i += -1;
					}
				}
			}
			return array3;
		}

		
		[LineNumberTable(new byte[]
		{
			160,
			105,
			104,
			117,
			150,
			103,
			135,
			98,
			141,
			107,
			127,
			1,
			115,
			100,
			103,
			143,
			114,
			98
		})]
		
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

		
		static LongTextAligner()
		{
		}

		
		private int tupleSize;

		
		
		private List reftup;

		
		
		private HashMap tupleIndex;

		
		private List refWords;

		
		internal static bool assertionsDisabled = !ClassLiteral<LongTextAligner>.Value.desiredAssertionStatus();

		
		[SourceFile("LongTextAligner.java")]
		[NonNestedInnerClass("edu.cmu.sphinx.alignment.LongTextAligner$Alignment$Node")]
		internal sealed class Alignment : java.lang.Object
		{
			
			
			internal static List access_000(LongTextAligner.Alignment alignment)
			{
				return alignment.shifts;
			}

			
			
			internal static List access$100(LongTextAligner.Alignment alignment)
			{
				return alignment.indices;
			}

			
			
			internal static List access$200(LongTextAligner.Alignment alignment)
			{
				return alignment.query;
			}

			
			[LineNumberTable(new byte[]
			{
				73,
				111,
				103,
				107,
				102,
				110,
				119,
				114,
				127,
				19,
				111,
				105,
				226,
				58,
				233,
				74,
				140,
				103,
				246,
				69,
				103,
				135,
				107,
				112,
				138,
				108,
				110,
				107,
				130,
				105,
				103,
				107,
				114,
				106,
				146,
				109,
				107,
				161,
				106,
				159,
				9,
				107,
				162,
				127,
				2,
				38,
				139,
				115,
				104,
				6,
				200,
				112,
				112,
				100,
				108,
				100,
				140,
				116,
				107,
				113,
				106,
				140,
				101,
				133,
				107
			})]
			
			public Alignment(LongTextAligner longTextAligner, List list, Range range)
			{
				this.query = list;
				this.indices = new ArrayList();
				TreeSet treeSet = new TreeSet();
				for (int i = 0; i < list.size(); i++)
				{
					if (LongTextAligner.access$400(longTextAligner).containsKey(list.get(i)))
					{
						this.indices.add(Integer.valueOf(i));
						Iterator iterator = ((ArrayList)LongTextAligner.access$400(longTextAligner).get(list.get(i))).iterator();
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
				PriorityQueue.__<clinit>();
				PriorityQueue priorityQueue = new PriorityQueue(1, new LongTextAligner$Alignment$1(this, longTextAligner, hashMap));
				HashSet hashSet = new HashSet();
				HashMap hashMap2 = new HashMap();
				LongTextAligner$Alignment$Node longTextAligner$Alignment$Node = new LongTextAligner$Alignment$Node(this, 0, 0, null);
				hashMap.put(longTextAligner$Alignment$Node, Integer.valueOf(0));
				priorityQueue.add(longTextAligner$Alignment$Node);
				while (!priorityQueue.isEmpty())
				{
					LongTextAligner$Alignment$Node longTextAligner$Alignment$Node2 = (LongTextAligner$Alignment$Node)priorityQueue.poll();
					if (!hashSet.contains(longTextAligner$Alignment$Node2))
					{
						if (longTextAligner$Alignment$Node2.isTarget())
						{
							ArrayList arrayList = new ArrayList();
							while (hashMap2.containsKey(longTextAligner$Alignment$Node2))
							{
								if (!longTextAligner$Alignment$Node2.isBoundary() && longTextAligner$Alignment$Node2.hasMatch())
								{
									arrayList.add(longTextAligner$Alignment$Node2);
								}
								longTextAligner$Alignment$Node2 = (LongTextAligner$Alignment$Node)hashMap2.get(longTextAligner$Alignment$Node2);
							}
							this.alignment = new ArrayList(arrayList);
							Collections.reverse(this.alignment);
							return;
						}
						hashSet.add(longTextAligner$Alignment$Node2);
						Iterator iterator2 = longTextAligner$Alignment$Node2.adjacent().iterator();
						while (iterator2.hasNext())
						{
							LongTextAligner$Alignment$Node longTextAligner$Alignment$Node3 = (LongTextAligner$Alignment$Node)iterator2.next();
							if (!hashSet.contains(longTextAligner$Alignment$Node3))
							{
								int num = java.lang.Math.abs(this.indices.size() - this.shifts.size() - LongTextAligner$Alignment$Node.access$600(longTextAligner$Alignment$Node2) + LongTextAligner$Alignment$Node.access$700(longTextAligner$Alignment$Node2)) - java.lang.Math.abs(this.indices.size() - this.shifts.size() - LongTextAligner$Alignment$Node.access$600(longTextAligner$Alignment$Node3) + LongTextAligner$Alignment$Node.access$700(longTextAligner$Alignment$Node3));
								Integer integer2 = (Integer)hashMap.get(longTextAligner$Alignment$Node3);
								Integer integer3 = (Integer)hashMap.get(longTextAligner$Alignment$Node2);
								if (integer2 == null)
								{
									integer2 = Integer.valueOf(int.MaxValue);
								}
								if (integer3 == null)
								{
									integer3 = Integer.valueOf(int.MaxValue);
								}
								int num2 = integer3.intValue() + longTextAligner$Alignment$Node3.getValue() - num;
								if (num2 < integer2.intValue())
								{
									hashMap.put(longTextAligner$Alignment$Node3, Integer.valueOf(num2));
									priorityQueue.add(longTextAligner$Alignment$Node3);
									hashMap2.put(longTextAligner$Alignment$Node3, longTextAligner$Alignment$Node2);
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

			
			internal LongTextAligner this$0 = longTextAligner;
		}
	}
}
