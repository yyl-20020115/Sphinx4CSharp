using System;

using edu.cmu.sphinx.util;
using IKVM.Attributes;
using ikvm.@internal;
using java.lang;
using java.util;
using java.util.logging;

namespace edu.cmu.sphinx.linguist.acoustic
{
	public class HMMPool : java.lang.Object
	{
		
		public static void __<clinit>()
		{
		}

		[LineNumberTable(new byte[]
		{
			160,
			79,
			107,
			108,
			124,
			124,
			106,
			110,
			12,
			198
		})]
		
		public virtual int getID(Unit unit)
		{
			if (!unit.isContextDependent())
			{
				return this.getSimpleUnitID(unit);
			}
			LeftRightContext leftRightContext = (LeftRightContext)unit.getContext();
			if (!HMMPool.assertionsDisabled && leftRightContext.getLeftContext().Length != 1)
			{
				
				throw new AssertionError();
			}
			if (!HMMPool.assertionsDisabled && leftRightContext.getRightContext().Length != 1)
			{
				
				throw new AssertionError();
			}
			return this.buildID(this.getSimpleUnitID(unit), this.getSimpleUnitID(leftRightContext.getLeftContext()[0]), this.getSimpleUnitID(leftRightContext.getRightContext()[0]));
		}

		[LineNumberTable(new byte[]
		{
			68,
			104,
			104,
			136,
			105,
			162,
			105,
			106,
			138,
			117,
			118,
			150,
			104,
			104,
			102,
			102,
			139,
			103,
			45,
			199,
			114,
			191,
			29
		})]
		
		private Unit synthesizeUnit(int num)
		{
			int centralUnitID = this.getCentralUnitID(num);
			int leftUnitID = this.getLeftUnitID(num);
			int rightUnitID = this.getRightUnitID(num);
			if (centralUnitID == 0 || leftUnitID == 0 || rightUnitID == 0)
			{
				return null;
			}
			Unit unit = this.unitTable[centralUnitID];
			Unit unit2 = this.unitTable[leftUnitID];
			Unit unit3 = this.unitTable[rightUnitID];
			if (!HMMPool.assertionsDisabled && unit == null)
			{
				
				throw new AssertionError();
			}
			if (!HMMPool.assertionsDisabled && unit2 == null)
			{
				
				throw new AssertionError();
			}
			if (!HMMPool.assertionsDisabled && unit3 == null)
			{
				
				throw new AssertionError();
			}
			Unit[] array = new Unit[1];
			Unit[] array2 = new Unit[1];
			array[0] = unit2;
			array2[0] = unit3;
			LeftRightContext context = LeftRightContext.get(array, array2);
			Unit unit4 = this.unitManager.getUnit(unit.getName(), unit.isFiller(), context);
			if (this.logger.isLoggable(Level.FINER))
			{
				this.logger.finer(new StringBuilder().append("Missing ").append(this.getUnitNameFromID(num)).append(" returning ").append(unit4).toString());
			}
			return unit4;
		}

		
		private int getCentralUnitID(int num)
		{
			int num2 = this.numCIUnits * this.numCIUnits;
			return (num2 != -1) ? (num / num2) : (-num);
		}

		
		private int getLeftUnitID(int num)
		{
			int num2 = this.numCIUnits;
			int num3 = (num2 != -1) ? (num / num2) : (-num);
			int num4 = this.numCIUnits;
			return (num4 != -1) ? (num3 % num4) : 0;
		}

		
		private int getRightUnitID(int num)
		{
			int num2 = this.numCIUnits;
			return (num2 != -1) ? (num % num2) : 0;
		}

		[LineNumberTable(new byte[]
		{
			160,
			180,
			104,
			104,
			136,
			127,
			22,
			102,
			127,
			22,
			103,
			127,
			22,
			135
		})]
		
		private string getUnitNameFromID(int num)
		{
			int centralUnitID = this.getCentralUnitID(num);
			int leftUnitID = this.getLeftUnitID(num);
			int rightUnitID = this.getRightUnitID(num);
			string text = (this.unitTable[centralUnitID] != null) ? this.unitTable[centralUnitID].toString() : new StringBuilder().append("(").append(centralUnitID).append(')').toString();
			string text2 = (this.unitTable[leftUnitID] != null) ? this.unitTable[leftUnitID].toString() : new StringBuilder().append("(").append(leftUnitID).append(')').toString();
			string text3 = (this.unitTable[rightUnitID] != null) ? this.unitTable[rightUnitID].toString() : new StringBuilder().append("(").append(rightUnitID).append(')').toString();
			return new StringBuilder().append(text).append('[').append(text2).append(',').append(text3).append(']').toString();
		}

		
		
		private int getSimpleUnitID(Unit unit)
		{
			return unit.getBaseID();
		}

		[LineNumberTable(new byte[]
		{
			160,
			123,
			106,
			162,
			111,
			132,
			251,
			69,
			156
		})]
		
		public virtual int buildID(int unitID, int leftID, int rightID)
		{
			if (this.unitTable[unitID] == null)
			{
				return -1;
			}
			int num;
			if (this.unitTable[unitID].isFiller())
			{
				num = unitID;
			}
			else
			{
				num = unitID * (this.numCIUnits * this.numCIUnits) + leftID * this.numCIUnits + rightID;
			}
			if (!HMMPool.assertionsDisabled && num >= this.unitTable.Length)
			{
				
				throw new AssertionError();
			}
			return num;
		}

		
		public virtual bool isValidID(int unitID)
		{
			return unitID >= 0 && unitID < this.unitTable.Length && this.unitTable[unitID] != null;
		}

		
		
		public virtual HMM getHMM(int unitID, HMMPosition position)
		{
			return ((HMM[])this.hmmTable.get(position))[unitID];
		}

		[LineNumberTable(new byte[]
		{
			159,
			183,
			102
		})]
		
		protected internal HMMPool()
		{
		}

		[LineNumberTable(new byte[]
		{
			2,
			104,
			103,
			98,
			103,
			135,
			105,
			144,
			105,
			176,
			111,
			108,
			127,
			1,
			105,
			135,
			130,
			137,
			159,
			0,
			114,
			109,
			105,
			106,
			107,
			109,
			159,
			19,
			165,
			117,
			127,
			0,
			110,
			112,
			110,
			107,
			100,
			138,
			100,
			112,
			249,
			57,
			235,
			61,
			235,
			78
		})]
		
		public HMMPool(AcousticModel model, Logger logger, UnitManager unitManager)
		{
			this.logger = logger;
			int num = 0;
			this.model = model;
			this.unitManager = unitManager;
			if (model.getLeftContextSize() != 1)
			{
				string text = "LexTreeLinguist: Unsupported left context size";
				
				throw new Error(text);
			}
			if (model.getRightContextSize() != 1)
			{
				string text2 = "LexTreeLinguist: Unsupported right context size";
				
				throw new Error(text2);
			}
			Iterator iterator = model.getContextIndependentUnitIterator();
			while (iterator.hasNext())
			{
				Unit unit = (Unit)iterator.next();
				logger.fine(new StringBuilder().append("CI unit ").append(unit).toString());
				if (unit.getBaseID() > num)
				{
					num = unit.getBaseID();
				}
			}
			this.numCIUnits = num + 1;
			this.unitTable = new Unit[this.numCIUnits * this.numCIUnits * this.numCIUnits];
			iterator = model.getHMMIterator();
			while (iterator.hasNext())
			{
				HMM hmm = (HMM)iterator.next();
				Unit unit2 = hmm.getUnit();
				int id = this.getID(unit2);
				this.unitTable[id] = unit2;
				if (logger.isLoggable(Level.FINER))
				{
					logger.finer(new StringBuilder().append("Unit ").append(unit2).append(" id ").append(id).toString());
				}
			}
			EnumMap.__<clinit>();
			this.hmmTable = new EnumMap(ClassLiteral<HMMPosition>.Value);
			HMMPosition[] array = HMMPosition.values();
			int num2 = array.Length;
			for (int i = 0; i < num2; i++)
			{
				HMMPosition hmmposition = array[i];
				HMM[] array2 = new HMM[this.unitTable.Length];
				this.hmmTable.put(hmmposition, array2);
				for (int j = 1; j < this.unitTable.Length; j++)
				{
					Unit unit3 = this.unitTable[j];
					if (unit3 == null)
					{
						unit3 = this.synthesizeUnit(j);
					}
					if (unit3 != null)
					{
						array2[j] = model.lookupNearestHMM(unit3, hmmposition, false);
						if (!HMMPool.assertionsDisabled && array2[j] == null)
						{
							
							throw new AssertionError();
						}
					}
				}
			}
		}

		public virtual AcousticModel getModel()
		{
			return this.model;
		}

		public virtual int getNumCIUnits()
		{
			return this.numCIUnits;
		}

		
		public virtual Unit getUnit(int unitID)
		{
			return this.unitTable[unitID];
		}

		[LineNumberTable(new byte[]
		{
			160,
			210,
			98,
			104,
			104,
			136,
			105,
			127,
			11,
			130,
			105,
			127,
			11,
			130,
			105,
			127,
			11,
			130,
			106,
			100,
			127,
			12,
			63,
			5,
			133,
			130,
			107,
			100,
			127,
			12,
			63,
			5,
			197
		})]
		
		public virtual HMM getHMM(Unit @base, Unit lc, Unit rc, HMMPosition pos)
		{
			int id = this.getID(@base);
			int id2 = this.getID(lc);
			int id3 = this.getID(rc);
			if (!this.isValidID(id))
			{
				this.logger.severe(new StringBuilder().append("Bad HMM Unit: ").append(@base.getName()).toString());
				return null;
			}
			if (!this.isValidID(id2))
			{
				this.logger.severe(new StringBuilder().append("Bad HMM Unit: ").append(lc.getName()).toString());
				return null;
			}
			if (!this.isValidID(id3))
			{
				this.logger.severe(new StringBuilder().append("Bad HMM Unit: ").append(rc.getName()).toString());
				return null;
			}
			int num = this.buildID(id, id2, id3);
			if (num < 0)
			{
				this.logger.severe(new StringBuilder().append("Unable to build HMM Unit ID for ").append(@base.getName()).append(" lc=").append(lc.getName()).append(" rc=").append(rc.getName()).toString());
				return null;
			}
			HMM hmm = this.getHMM(num, pos);
			if (hmm == null)
			{
				this.logger.severe(new StringBuilder().append("Missing HMM Unit for ").append(@base.getName()).append(" lc=").append(lc.getName()).append(" rc=").append(rc.getName()).toString());
			}
			return hmm;
		}

		[LineNumberTable(new byte[]
		{
			160,
			246,
			127,
			11,
			159,
			12,
			114,
			108,
			63,
			21,
			198
		})]
		
		public virtual void dumpInfo()
		{
			this.logger.info(new StringBuilder().append("Max CI Units ").append(this.numCIUnits).toString());
			this.logger.info(new StringBuilder().append("Unit table size ").append(this.unitTable.Length).toString());
			if (this.logger.isLoggable(Level.FINER))
			{
				for (int i = 0; i < this.unitTable.Length; i++)
				{
					this.logger.finer(new StringBuilder().append(java.lang.String.valueOf(i)).append(' ').append(this.unitTable[i]).toString());
				}
			}
		}

		[LineNumberTable(new byte[]
		{
			161,
			13,
			98,
			111,
			144,
			106,
			120,
			120,
			106,
			100,
			228,
			59,
			233,
			72,
			113,
			127,
			5
		})]
		
		internal virtual void benchmark()
		{
			int num = 0;
			java.lang.System.@out.println("benchmarking ...");
			TimerPool.getTimer(this, "hmmPoolBenchmark").start();
			for (int i = 0; i < 1000000; i++)
			{
				int[] array = HMMPool.ids;
				int num2 = i;
				int num3 = HMMPool.ids.Length;
				int unitID = array[(num3 != -1) ? (num2 % num3) : 0];
				HMMPosition[] array2 = HMMPool.pos;
				int num4 = i;
				int num5 = HMMPool.pos.Length;
				HMMPosition position = array2[(num5 != -1) ? (num4 % num5) : 0];
				if (this.getHMM(unitID, position) == null)
				{
					num++;
				}
			}
			TimerPool.getTimer(this, "hmmPoolBenchmark").stop();
			java.lang.System.@out.println(new StringBuilder().append("null count ").append(num).toString());
		}

		[LineNumberTable(new byte[]
		{
			159,
			173,
			245,
			161,
			89,
			223,
			12
		})]
		static HMMPool()
		{
			HMMPool.pos = new HMMPosition[]
			{
				HMMPosition.__BEGIN,
				HMMPosition.__END,
				HMMPosition.__SINGLE,
				HMMPosition.__INTERNAL
			};
			HMMPool.ids = new int[]
			{
				9206,
				9320,
				9620,
				9865,
				14831,
				15836
			};
		}

		private AcousticModel model;

		private Unit[] unitTable;

		
		private Map hmmTable;

		private int numCIUnits;

		private Logger logger;

		private UnitManager unitManager;

		
		internal static HMMPosition[] pos;

		
		internal static int[] ids;

		
		internal static bool assertionsDisabled = !ClassLiteral<HMMPool>.Value.desiredAssertionStatus();
	}
}
