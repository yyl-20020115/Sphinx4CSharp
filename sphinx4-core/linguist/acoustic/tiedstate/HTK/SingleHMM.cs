using System;

using IKVM.Attributes;
using java.lang;

namespace edu.cmu.sphinx.linguist.acoustic.tiedstate.HTK
{
	public class SingleHMM : java.lang.Object
	{
		public virtual int getTransIdx()
		{
			return this.transidx;
		}

		public virtual string getName()
		{
			return this.name;
		}

		
		public virtual int getNstates()
		{
			return this.states.Length;
		}

		public virtual int getNbEmittingStates()
		{
			return this.nbEmittingStates;
		}

		
		public virtual bool isEmitting(int idx)
		{
			return this.states[idx] != null;
		}

		
		public virtual HMMState getState(int idx)
		{
			return this.states[idx];
		}

		[LineNumberTable(new byte[]
		{
			159,
			180,
			110,
			100,
			98,
			111,
			105,
			100,
			103
		})]
		
		public virtual string getBaseName()
		{
			int num = java.lang.String.instancehelper_indexOf(this.name, 45);
			if (num < 0)
			{
				num = -1;
			}
			string text = java.lang.String.instancehelper_substring(this.name, num + 1);
			num = java.lang.String.instancehelper_indexOf(text, 43);
			if (num < 0)
			{
				num = java.lang.String.instancehelper_length(text);
			}
			return java.lang.String.instancehelper_substring(text, 0, num);
		}

		[LineNumberTable(new byte[]
		{
			159,
			191,
			110,
			100,
			102
		})]
		
		public virtual string getLeft()
		{
			int num = java.lang.String.instancehelper_indexOf(this.name, 45);
			if (num < 0)
			{
				return "-";
			}
			return java.lang.String.instancehelper_substring(this.name, 0, num);
		}

		[LineNumberTable(new byte[]
		{
			6,
			110,
			100,
			102
		})]
		
		public virtual string getRight()
		{
			int num = java.lang.String.instancehelper_indexOf(this.name, 43);
			if (num < 0)
			{
				return "-";
			}
			return java.lang.String.instancehelper_substring(this.name, num + 1);
		}

		[LineNumberTable(new byte[]
		{
			159,
			165,
			232,
			58,
			135,
			231,
			69,
			107,
			108,
			103
		})]
		
		public SingleHMM(int nbStates)
		{
			this.transidx = -1;
			this.trIdx = -1;
			this.name = "";
			this.states = new HMMState[nbStates];
			this.nbEmittingStates = 0;
		}

		public virtual void setName(string s)
		{
			this.name = s;
		}

		[LineNumberTable(new byte[]
		{
			13,
			109,
			110,
			105
		})]
		public virtual void setState(int idx, HMMState st)
		{
			if (this.states[idx] == null && st != null)
			{
				this.nbEmittingStates++;
			}
			this.states[idx] = st;
		}

		[LineNumberTable(new byte[]
		{
			27,
			108,
			103
		})]
		public virtual void setTrans(int i)
		{
			this.trans = (float[][])null;
			this.transidx = i;
		}

		public virtual void setTrans(float[][] tr)
		{
			this.trans = tr;
		}

		[LineNumberTable(new byte[]
		{
			36,
			104,
			159,
			2
		})]
		
		public virtual float getTrans(int i, int j)
		{
			if (this.trans == null)
			{
				this.trans = (float[][])this.hmmset.__transitions.get(this.transidx);
			}
			return this.trans[i][j];
		}

		
		private HMMState[] states;

		private string name;

		public float[][] trans;

		private int transidx;

		public int trIdx;

		private int nbEmittingStates;

		public HMMSet hmmset;
	}
}
