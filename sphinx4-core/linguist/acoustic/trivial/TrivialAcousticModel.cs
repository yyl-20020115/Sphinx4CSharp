using edu.cmu.sphinx.util.props;
using java.util;

namespace edu.cmu.sphinx.linguist.acoustic.trivial
{
	public class TrivialAcousticModel : AcousticModelBase
	{
		private void init(int num, int num2)
		{
			this.leftContextSize = num;
			this.rightContextSize = num2;
			string[] array = new string[]
			{
				"AX_one",
				"AY_five",
				"AY_nine",
				"EH_seven",
				"EY_eight",
				"E_seven",
				"F_five",
				"F_four",
				"II_three",
				"II_zero",
				"I_six",
				"K_six",
				"N_nine",
				"N_nine_2",
				"N_one",
				"N_seven",
				"OO_two",
				"OW_four",
				"OW_oh",
				"OW_zero",
				"R_four",
				"R_three",
				"R_zero",
				"S_seven",
				"S_six",
				"S_six_2",
				"TH_three",
				"T_eight",
				"T_two",
				"V_five",
				"V_seven",
				"W_one",
				"Z_zero",
				"AX_one",
				"SIL"
			};
			string[] array2 = array;
			int num3 = array2.Length;
			for (int i = 0; i < num3; i++)
			{
				string text = array2[i];
				this.createTrivialHMM(text);
			}
		}

		private void createTrivialHMM(string text)
		{
		}
		
		public TrivialAcousticModel(int leftContextSize, int rightContextSize)
		{
			this.hmmMap = new HashMap();
			this.init(leftContextSize, rightContextSize);
		}
		
		public TrivialAcousticModel()
		{
			this.hmmMap = new HashMap();
		}
		
		public override void newProperties(PropertySheet ps)
		{
			this.init(ps.getInt("leftContextSize"), ps.getInt("leftContextSize"));
		}

		public override string getName()
		{
			return this.name;
		}		
		
		public override Properties getProperties()
		{
			return new Properties();
		}

		public override HMM lookupNearestHMM(Unit unit, HMMPosition position, bool exactMatch)
		{
			HMM result = null;
			if (!exactMatch || position == HMMPosition.__UNDEFINED)
			{
				unit = unit.getBaseUnit();
				result = (HMM)this.hmmMap.get(unit);
			}
			return result;
		}
		
		public override Iterator getHMMIterator()
		{
			return this.hmmMap.values().iterator();
		}
		
		public override Iterator getContextIndependentUnitIterator()
		{
			return this.hmmMap.keySet().iterator();
		}

		public override int getLeftContextSize()
		{
			return this.leftContextSize;
		}

		public override int getRightContextSize()
		{
			return this.rightContextSize;
		}

		public override void allocate()
		{
		}

		public override void deallocate()
		{
		}

		[S4Integer(new object[]
		{
			64,
			"Ledu/cmu/sphinx/util/props/S4Integer;",
			"defaultValue",
			1
		})]
		public const string LEFT_CONTEXT_SIZE = "leftContextSize";

		[S4Integer(new object[]
		{
			64,
			"Ledu/cmu/sphinx/util/props/S4Integer;",
			"defaultValue",
			1
		})]
		public const string RIGHT_CONTEXT_SIZE = "leftContextSize";

		private string name;

		private Map hmmMap;

		private int leftContextSize;

		private int rightContextSize;
	}
}
