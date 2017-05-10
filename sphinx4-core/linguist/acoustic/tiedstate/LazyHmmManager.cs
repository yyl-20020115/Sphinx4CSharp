using System;

using edu.cmu.sphinx.linguist.acoustic.tiedstate.kaldi;
using IKVM.Attributes;
using java.lang;
using java.util;

namespace edu.cmu.sphinx.linguist.acoustic.tiedstate
{
	public class LazyHmmManager : HMMManager
	{
		
		[LineNumberTable(new byte[]
		{
			159,
			178,
			104,
			103,
			103,
			136,
			107,
			103,
			103,
			107,
			109,
			107
		})]
		
		public LazyHmmManager(KaldiTextParser parser, TransitionModel transitionModel, Pool senonePool, Map symbolTable)
		{
			this.transitionModel = transitionModel;
			this.senonePool = senonePool;
			this.symbolTable = symbolTable;
			parser.expectToken("ContextDependency");
			parser.getInt();
			parser.getInt();
			parser.expectToken("ToPdf");
			this.eventMap = this.parseEventMap(parser);
			parser.expectToken("EndContextDependency");
		}

		[LineNumberTable(new byte[]
		{
			0,
			135,
			109,
			140,
			109,
			136,
			109,
			136,
			109,
			130
		})]
		
		private EventMap parseEventMap(KaldiTextParser kaldiTextParser)
		{
			string token = kaldiTextParser.getToken();
			if (java.lang.String.instancehelper_equals("CE", token))
			{
				return new ConstantEventMap(kaldiTextParser.getInt());
			}
			if (java.lang.String.instancehelper_equals("SE", token))
			{
				return this.parseSplitEventMap(kaldiTextParser);
			}
			if (java.lang.String.instancehelper_equals("TE", token))
			{
				return this.parseTableEventMap(kaldiTextParser);
			}
			if (java.lang.String.instancehelper_equals("NULL", token))
			{
				return null;
			}
			string text = token;
			
			throw new InputMismatchException(text);
		}

		[LineNumberTable(new byte[]
		{
			18,
			135,
			102,
			125,
			41,
			168,
			107,
			105,
			105,
			109,
			139
		})]
		
		private EventMap parseSplitEventMap(KaldiTextParser kaldiTextParser)
		{
			int @int = kaldiTextParser.getInt();
			ArrayList arrayList = new ArrayList();
			int[] intArray = kaldiTextParser.getIntArray();
			int num = intArray.Length;
			for (int i = 0; i < num; i++)
			{
				Integer integer = Integer.valueOf(intArray[i]);
				arrayList.add(integer);
			}
			kaldiTextParser.expectToken("{");
			EventMap yesMap = this.parseEventMap(kaldiTextParser);
			EventMap noMap = this.parseEventMap(kaldiTextParser);
			SplitEventMap result = new SplitEventMap(@int, arrayList, yesMap, noMap);
			kaldiTextParser.expectToken("}");
			return result;
		}

		[LineNumberTable(new byte[]
		{
			34,
			103,
			103,
			135,
			139,
			104,
			144,
			107
		})]
		
		private EventMap parseTableEventMap(KaldiTextParser kaldiTextParser)
		{
			int @int = kaldiTextParser.getInt();
			int num = kaldiTextParser.getInt();
			ArrayList arrayList = new ArrayList(num);
			kaldiTextParser.expectToken("(");
			for (;;)
			{
				int num2 = 0;
				int num3 = num;
				num += -1;
				if (num2 >= num3)
				{
					break;
				}
				arrayList.add(this.parseEventMap(kaldiTextParser));
			}
			kaldiTextParser.expectToken(")");
			return new TableEventMap(@int, arrayList);
		}

		[LineNumberTable(new byte[]
		{
			49,
			105,
			134,
			103,
			158,
			104,
			108,
			105,
			106,
			126,
			127,
			0,
			98,
			125,
			189,
			104,
			113,
			113,
			145,
			104,
			120,
			120,
			120,
			169,
			114,
			109,
			136
		})]
		
		public override HMM get(HMMPosition position, Unit unit)
		{
			HMM hmm = base.get(position, unit);
			if (null != hmm)
			{
				return hmm;
			}
			int[] array = new int[3];
			array[1] = ((Integer)this.symbolTable.get(unit.getName())).intValue();
			if (unit.isContextDependent())
			{
				LeftRightContext leftRightContext = (LeftRightContext)unit.getContext();
				Unit unit2 = leftRightContext.getLeftContext()[0];
				Unit unit3 = leftRightContext.getRightContext()[0];
				array[0] = ((Integer)this.symbolTable.get(unit2.getName())).intValue();
				array[2] = ((Integer)this.symbolTable.get(unit3.getName())).intValue();
			}
			else
			{
				array[0] = ((Integer)this.symbolTable.get("SIL")).intValue();
				array[2] = ((Integer)this.symbolTable.get("SIL")).intValue();
			}
			int[] array2 = new int[]
			{
				this.eventMap.map(0, array),
				this.eventMap.map(1, array),
				this.eventMap.map(2, array)
			};
			SenoneSequence senoneSequence = new SenoneSequence(new Senone[]
			{
				(Senone)this.senonePool.get(array2[0]),
				(Senone)this.senonePool.get(array2[1]),
				(Senone)this.senonePool.get(array2[2])
			});
			float[][] transitionMatrix = this.transitionModel.getTransitionMatrix(array[1], array2);
			SenoneHMM senoneHMM = new SenoneHMM(unit, senoneSequence, transitionMatrix, position);
			this.put(senoneHMM);
			return senoneHMM;
		}

		
		private EventMap eventMap;

		
		
		private Pool senonePool;

		
		
		private Map symbolTable;

		
		private TransitionModel transitionModel;
	}
}
