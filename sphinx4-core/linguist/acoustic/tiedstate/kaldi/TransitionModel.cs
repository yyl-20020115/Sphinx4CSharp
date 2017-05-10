﻿using System;

using edu.cmu.sphinx.util;
using IKVM.Attributes;
using IKVM.Runtime;
using java.lang;
using java.util;

namespace edu.cmu.sphinx.linguist.acoustic.tiedstate.kaldi
{
	public class TransitionModel : java.lang.Object
	{
		[LineNumberTable(new byte[]
		{
			46,
			104,
			107,
			135,
			107,
			107,
			103,
			130,
			105,
			104,
			104,
			104,
			109,
			116,
			105,
			255,
			8,
			57,
			233,
			74,
			107,
			107,
			108,
			107,
			139,
			103,
			110,
			57,
			136
		})]
		
		public TransitionModel(KaldiTextParser parser)
		{
			parser.expectToken("<TransitionModel>");
			this.parseTopology(parser);
			parser.expectToken("<Triples>");
			this.transitionStates = new HashMap();
			int @int = parser.getInt();
			int num = 1;
			for (int i = 0; i < @int; i++)
			{
				int j = parser.getInt();
				int int2 = parser.getInt();
				int int3 = parser.getInt();
				Triple triple = new Triple(j, int2, int3);
				this.transitionStates.put(triple, Integer.valueOf(num));
				num += ((HmmState)((List)this.phoneStates.get(Integer.valueOf(j))).get(int2)).getTransitions().size();
			}
			parser.expectToken("</Triples>");
			parser.expectToken("<LogProbs>");
			this.logProbabilities = parser.getFloatArray();
			parser.expectToken("</LogProbs>");
			parser.expectToken("</TransitionModel>");
			LogMath logMath = LogMath.getLogMath();
			for (int j = 0; j < this.logProbabilities.Length; j++)
			{
				this.logProbabilities[j] = logMath.lnToLog(this.logProbabilities[j]);
			}
		}

		[LineNumberTable(new byte[]
		{
			160,
			65,
			127,
			5,
			141,
			127,
			21,
			105,
			110,
			110,
			153,
			127,
			6,
			125,
			133
		})]
		
		public virtual float[][] getTransitionMatrix(int phone, int[] pdfs)
		{
			int num = 4;
			int num2 = 4;
			int[] array = new int[2];
			int num3 = num2;
			array[1] = num3;
			num3 = num;
			array[0] = num3;
			float[][] array2 = (float[][])ByteCodeHelper.multianewarray(typeof(float[][]).TypeHandle, array);
			Arrays.fill(array2[3], float.MinValue);
			Iterator iterator = ((List)this.phoneStates.get(Integer.valueOf(phone))).iterator();
			while (iterator.hasNext())
			{
				HmmState hmmState = (HmmState)iterator.next();
				int id = hmmState.getId();
				Arrays.fill(array2[id], float.MinValue);
				Triple triple = new Triple(phone, id, pdfs[id]);
				int num4 = ((Integer)this.transitionStates.get(triple)).intValue();
				Iterator iterator2 = hmmState.getTransitions().iterator();
				while (iterator2.hasNext())
				{
					Integer integer = (Integer)iterator2.next();
					float[] array3 = array2[id];
					int num5 = integer.intValue();
					float[] array4 = this.logProbabilities;
					int num6 = num4;
					num4++;
					array3[num5] = array4[num6];
				}
			}
			return array2;
		}

		[LineNumberTable(new byte[]
		{
			77,
			139,
			171,
			119,
			108,
			139,
			102,
			116,
			148,
			103,
			151,
			103,
			135,
			109,
			104,
			103,
			116,
			147,
			169,
			108,
			145,
			133,
			127,
			0,
			113,
			133,
			108
		})]
		
		private void parseTopology(KaldiTextParser kaldiTextParser)
		{
			kaldiTextParser.expectToken("<Topology>");
			this.phoneStates = new HashMap();
			string token;
			while (java.lang.String.instancehelper_equals("<TopologyEntry>", token = kaldiTextParser.getToken()))
			{
				kaldiTextParser.assertToken("<TopologyEntry>", token);
				kaldiTextParser.expectToken("<ForPhones>");
				ArrayList arrayList = new ArrayList();
				while (!java.lang.String.instancehelper_equals("</ForPhones>", token = kaldiTextParser.getToken()))
				{
					arrayList.add(Integer.valueOf(Integer.parseInt(token)));
				}
				ArrayList arrayList2 = new ArrayList(3);
				while (java.lang.String.instancehelper_equals("<State>", kaldiTextParser.getToken()))
				{
					int @int = kaldiTextParser.getInt();
					token = kaldiTextParser.getToken();
					if (java.lang.String.instancehelper_equals("<PdfClass>", token))
					{
						int int2 = kaldiTextParser.getInt();
						ArrayList arrayList3 = new ArrayList();
						while (java.lang.String.instancehelper_equals("<Transition>", token = kaldiTextParser.getToken()))
						{
							arrayList3.add(Integer.valueOf(kaldiTextParser.getInt()));
							kaldiTextParser.getToken();
						}
						kaldiTextParser.assertToken("</State>", token);
						arrayList2.add(new HmmState(@int, int2, arrayList3));
					}
				}
				Iterator iterator = arrayList.iterator();
				while (iterator.hasNext())
				{
					Integer integer = (Integer)iterator.next();
					this.phoneStates.put(integer, arrayList2);
				}
			}
			kaldiTextParser.assertToken("</Topology>", token);
		}

		
		private Map phoneStates;

		
		private Map transitionStates;

		private float[] logProbabilities;
	}
}
