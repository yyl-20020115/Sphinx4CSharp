using System;

using System.Runtime.Serialization;
using System.Security.Permissions;
using edu.cmu.sphinx.linguist.acoustic;
using IKVM.Attributes;
using java.lang;

namespace edu.cmu.sphinx.linguist.flat
{
	[Implements(new string[]
	{
		"edu.cmu.sphinx.linguist.UnitSearchState"
	})]
	[Serializable]
	public class UnitState : SentenceHMMState, UnitSearchState, SearchState
	{
		
		public new static void __<clinit>()
		{
		}

		[LineNumberTable(new byte[]
		{
			15,
			232,
			24,
			235,
			105,
			103,
			103
		})]
		
		public UnitState(Unit unit, HMMPosition position)
		{
			this.position = HMMPosition.__INTERNAL;
			this.unit = unit;
			this.position = position;
		}

		public virtual Unit getUnit()
		{
			return this.unit;
		}

		public virtual HMMPosition getPosition()
		{
			return this.position;
		}

		[LineNumberTable(new byte[]
		{
			159,
			178,
			239,
			53,
			235,
			76,
			103,
			108,
			227,
			70,
			112,
			164,
			100,
			109,
			99,
			109,
			102,
			139
		})]
		
		public UnitState(PronunciationState parent, int which, Unit unit) : base("U", parent, which)
		{
			this.position = HMMPosition.__INTERNAL;
			this.unit = unit;
			Unit[] units = parent.getPronunciation().getUnits();
			int num = units.Length;
			if (units[num - 1] == UnitManager.__SILENCE && num > 1)
			{
				num += -1;
			}
			if (num == 1)
			{
				this.position = HMMPosition.__SINGLE;
			}
			else if (which == 0)
			{
				this.position = HMMPosition.__BEGIN;
			}
			else if (which == num - 1)
			{
				this.position = HMMPosition.__END;
			}
		}

		
		
		public virtual bool isLast()
		{
			return this.position == HMMPosition.__SINGLE || this.position == HMMPosition.__END;
		}

		
		
		public override string getName()
		{
			return new StringBuilder().append(base.getName()).append('<').append(this.unit).append('>').toString();
		}

		
		
		public override string getValueSignature()
		{
			return this.unit.toString();
		}

		
		
		public override string getPrettyName()
		{
			return this.unit.toString();
		}

		public override string getTypeLabel()
		{
			return "Unit";
		}

		public override bool isUnit()
		{
			return true;
		}

		public override int getOrder()
		{
			return 5;
		}

		
		static UnitState()
		{
			SentenceHMMState.__<clinit>();
		}

		
		object SearchState.Object;getLexState()
		{
			return this.<bridge>getLexState();
		}

		
		[PermissionSet(SecurityAction.Demand, XML = "<PermissionSet class=\"System.Security.PermissionSet\"\nversion=\"1\">\n<IPermission class=\"System.Security.Permissions.SecurityPermission, mscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089\"\nversion=\"1\"\nFlags=\"SerializationFormatter\"/>\n</PermissionSet>\n")]
		protected UnitState(SerializationInfo serializationInfo, StreamingContext streamingContext) : base(serializationInfo, streamingContext)
		{
		}

		
		private Unit unit;

		private HMMPosition position;
	}
}
