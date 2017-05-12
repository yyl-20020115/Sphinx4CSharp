using edu.cmu.sphinx.linguist.acoustic.tiedstate.kaldi;
using java.lang;
using java.util;

namespace edu.cmu.sphinx.linguist.acoustic.tiedstate
{
	public class LazyHmmManager : HMMManager
	{		
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

		private EventMap parseEventMap(KaldiTextParser kaldiTextParser)
		{
			string token = kaldiTextParser.getToken();
			if (String.instancehelper_equals("CE", token))
			{
				return new ConstantEventMap(kaldiTextParser.getInt());
			}
			if (String.instancehelper_equals("SE", token))
			{
				return this.parseSplitEventMap(kaldiTextParser);
			}
			if (String.instancehelper_equals("TE", token))
			{
				return this.parseTableEventMap(kaldiTextParser);
			}
			if (String.instancehelper_equals("NULL", token))
			{
				return null;
			}
			string text = token;
			
			throw new InputMismatchException(text);
		}
		
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
				num --;
				if (num2 >= num3)
				{
					break;
				}
				arrayList.add(this.parseEventMap(kaldiTextParser));
			}
			kaldiTextParser.expectToken(")");
			return new TableEventMap(@int, arrayList);
		}
		
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
