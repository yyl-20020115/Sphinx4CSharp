using ikvm.@internal;
using java.lang;

namespace edu.cmu.sphinx.linguist.acoustic
{
	[System.Serializable]
	public sealed class HMMPosition : Enum
	{
		public static HMMPosition[] values()
		{
			return (HMMPosition[])HMMPosition._VALUES_.Clone();
		}
		
		private HMMPosition(string text, int num, char c) : base(text, num)
		{
			this.rep = String.valueOf(c);
			System.GC.KeepAlive(this);
		}
		
		public static HMMPosition valueOf(string name)
		{
			return (HMMPosition)Enum.valueOf(ClassLiteral<HMMPosition>.Value, name);
		}

		public static HMMPosition lookup(string rep)
		{
			return (rep != null && !String.instancehelper_isEmpty(rep)) ? HMMPosition.posByRep[(int)String.instancehelper_charAt(rep, 0)] : null;
		}
		public bool isWordEnd()
		{
			return this == HMMPosition.__SINGLE || this == HMMPosition.__END;
		}

		public bool isWordBeginning()
		{
			return this == HMMPosition.__SINGLE || this == HMMPosition.__BEGIN;
		}

		public override string toString()
		{
			return this.rep;
		}

		static HMMPosition()
		{
			int num = 0;
			HMMPosition[] array = HMMPosition.values();
			int num2 = array.Length;
			for (int i = 0; i < num2; i++)
			{
				HMMPosition hmmposition = array[i];
				if ((int)String.instancehelper_charAt(hmmposition.rep, 0) > num)
				{
					num = (int)String.instancehelper_charAt(hmmposition.rep, 0);
				}
			}
			HMMPosition.posByRep = new HMMPosition[num + 1];
			array = HMMPosition.values();
			num2 = array.Length;
			for (int i = 0; i < num2; i++)
			{
				HMMPosition hmmposition = array[i];
				HMMPosition.posByRep[(int)String.instancehelper_charAt(hmmposition.rep, 0)] = hmmposition;
			}
		}

		public static HMMPosition BEGIN
		{
			
			get
			{
				return HMMPosition.__BEGIN;
			}
		}
		
		public static HMMPosition END
		{
			
			get
			{
				return HMMPosition.__END;
			}
		}

		public static HMMPosition SINGLE
		{
			
			get
			{
				return HMMPosition.__SINGLE;
			}
		}

		public static HMMPosition INTERNAL
		{
			
			get
			{
				return HMMPosition.__INTERNAL;
			}
		}

		public static HMMPosition UNDEFINED
		{
			
			get
			{
				return HMMPosition.__UNDEFINED;
			}
		}

		internal static HMMPosition __BEGIN = new HMMPosition("BEGIN", 0, 'b');
		
		internal static HMMPosition __END = new HMMPosition("END", 1, 'e');
		
		internal static HMMPosition __SINGLE = new HMMPosition("SINGLE", 2, 's');
		
		internal static HMMPosition __INTERNAL = new HMMPosition("INTERNAL", 3, 'i');
		
		internal static HMMPosition __UNDEFINED = new HMMPosition("UNDEFINED", 4, '-');
		
		private static HMMPosition[] posByRep;

		private string rep;

		private static HMMPosition[] _VALUES_ = new HMMPosition[]
		{
			HMMPosition.__BEGIN,
			HMMPosition.__END,
			HMMPosition.__SINGLE,
			HMMPosition.__INTERNAL,
			HMMPosition.__UNDEFINED
		};

		
		[System.Serializable]
		public enum __Enum
		{
			BEGIN,
			END,
			SINGLE,
			INTERNAL,
			UNDEFINED
		}
	}
}
