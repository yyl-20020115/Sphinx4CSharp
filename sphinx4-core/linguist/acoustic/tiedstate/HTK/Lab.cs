using System;

using IKVM.Attributes;
using java.lang;

namespace edu.cmu.sphinx.linguist.acoustic.tiedstate.HTK
{
	public class Lab : java.lang.Object
	{
		[LineNumberTable(new byte[]
		{
			159,
			171,
			232,
			54,
			103,
			238,
			74,
			103,
			103
		})]
		
		public Lab(string s, int n)
		{
			this.numState = -1;
			this.start = -1;
			this.end = -1;
			this.setName(s);
			this.setStateIdx(n);
		}

		[LineNumberTable(new byte[]
		{
			25,
			114,
			159,
			7
		})]
		
		public virtual bool isEqual(Lab l)
		{
			if (l.getState() != -1 && this.getState() != -1)
			{
				return java.lang.String.instancehelper_equals(l.getName(), this.getName()) && l.getState() == this.getState();
			}
			return java.lang.String.instancehelper_equals(l.getName(), this.getName());
		}

		public virtual string getName()
		{
			return this.nameHMM;
		}

		public virtual int getState()
		{
			return this.numState;
		}

		public virtual void setName(string s)
		{
			this.nameHMM = s;
		}

		public virtual void setStateIdx(int i)
		{
			this.numState = i;
		}

		public virtual int getStart()
		{
			return this.start;
		}

		public virtual void setDeb(int i)
		{
			this.start = i;
		}

		public virtual int getEnd()
		{
			return this.end;
		}

		public virtual void setFin(int i)
		{
			this.end = i;
		}

		[LineNumberTable(new byte[]
		{
			159,
			164,
			232,
			61,
			103,
			174
		})]
		
		public Lab()
		{
			this.numState = -1;
			this.start = -1;
			this.end = -1;
		}

		[LineNumberTable(new byte[]
		{
			159,
			167,
			232,
			58,
			103,
			238,
			70,
			103
		})]
		
		public Lab(string s)
		{
			this.numState = -1;
			this.start = -1;
			this.end = -1;
			this.setName(s);
		}

		[LineNumberTable(new byte[]
		{
			159,
			177,
			232,
			48,
			103,
			238,
			80,
			108,
			108,
			108,
			108
		})]
		
		public Lab(Lab @ref)
		{
			this.numState = -1;
			this.start = -1;
			this.end = -1;
			this.setDeb(@ref.getStart());
			this.setFin(@ref.getEnd());
			this.setName(@ref.getName());
			this.setStateIdx(@ref.getState());
		}

		[LineNumberTable(new byte[]
		{
			34,
			102,
			119,
			127,
			25,
			124,
			105,
			127,
			14
		})]
		
		public override string toString()
		{
			string text = "";
			if (this.start >= 0 && this.end >= this.start)
			{
				text = new StringBuilder().append(text).append(this.start).append(" ").append(this.end).append(' ').toString();
			}
			text = new StringBuilder().append(text).append(this.nameHMM).toString();
			if (this.numState >= 0)
			{
				text = new StringBuilder().append(text).append("[").append(this.numState).append(']').toString();
			}
			return text;
		}

		private string nameHMM;

		private int numState;

		private int start;

		private int end;
	}
}
