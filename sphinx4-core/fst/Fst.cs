using System;

using edu.cmu.sphinx.fst.semiring;
using IKVM.Attributes;
using java.io;
using java.lang;
using java.util;

namespace edu.cmu.sphinx.fst
{
	public class Fst : java.lang.Object
	{
		public virtual string[] getIsyms()
		{
			return this.isyms;
		}

		public virtual string[] getOsyms()
		{
			return this.osyms;
		}

		public virtual State getStart()
		{
			return this.start;
		}

		
		
		public virtual int getNumStates()
		{
			return this.states.size();
		}

		
		
		public virtual State getState(int index)
		{
			return (State)this.states.get(index);
		}

		[LineNumberTable(new byte[]
		{
			33,
			104,
			103
		})]
		
		public Fst(Semiring s) : this()
		{
			this.semiring = s;
		}

		[LineNumberTable(new byte[]
		{
			93,
			109,
			115
		})]
		
		public virtual void addState(State state)
		{
			this.states.add(state);
			state.id = this.states.size() - 1;
		}

		public virtual void setStart(State start)
		{
			this.start = start;
		}

		public virtual void setIsyms(string[] isyms)
		{
			this.isyms = isyms;
		}

		public virtual void setOsyms(string[] osyms)
		{
			this.osyms = osyms;
		}

		[Throws(new string[]
		{
			"java.io.IOException",
			"java.lang.ClassNotFoundException"
		})]
		[LineNumberTable(new byte[]
		{
			160,
			231,
			171,
			98,
			98,
			98,
			103,
			103,
			103,
			104,
			102,
			102,
			134,
			116,
			63,
			2,
			165
		})]
		
		public static Fst loadModel(string filename)
		{
			long timeInMillis = Calendar.getInstance().getTimeInMillis();
			FileInputStream fileInputStream = new FileInputStream(filename);
			BufferedInputStream bufferedInputStream = new BufferedInputStream(fileInputStream);
			ObjectInputStream objectInputStream = new ObjectInputStream(bufferedInputStream);
			Fst result = Fst.readFst(objectInputStream);
			objectInputStream.close();
			bufferedInputStream.close();
			fileInputStream.close();
			java.lang.System.err.println(new StringBuilder().append("Load Time: ").append((double)(Calendar.getInstance().getTimeInMillis() - timeInMillis) / 1000.0).toString());
			return result;
		}

		[LineNumberTable(new byte[]
		{
			9,
			232,
			47,
			231,
			82,
			107
		})]
		
		public Fst()
		{
			this.states = null;
			this.states = new ArrayList();
		}

		[Throws(new string[]
		{
			"java.io.IOException"
		})]
		[LineNumberTable(new byte[]
		{
			160,
			81,
			104,
			103,
			41,
			166
		})]
		
		private void writeStringMap(ObjectOutputStream objectOutputStream, string[] array)
		{
			objectOutputStream.writeInt(array.Length);
			for (int i = 0; i < array.Length; i++)
			{
				objectOutputStream.writeObject(array[i]);
			}
		}

		[Throws(new string[]
		{
			"java.io.IOException"
		})]
		[LineNumberTable(new byte[]
		{
			160,
			96,
			109,
			109,
			151,
			108,
			145,
			102,
			112,
			112,
			114,
			108,
			108,
			108,
			238,
			59,
			230,
			72,
			108,
			105,
			115,
			105,
			105,
			107,
			109,
			109,
			109,
			253,
			59,
			232,
			61,
			233,
			75
		})]
		
		private void writeFst(ObjectOutputStream objectOutputStream)
		{
			this.writeStringMap(objectOutputStream, this.isyms);
			this.writeStringMap(objectOutputStream, this.osyms);
			objectOutputStream.writeInt(this.states.indexOf(this.start));
			objectOutputStream.writeObject(this.semiring);
			objectOutputStream.writeInt(this.states.size());
			HashMap hashMap = new HashMap(this.states.size(), 1f);
			int i;
			for (i = 0; i < this.states.size(); i++)
			{
				State state = (State)this.states.get(i);
				objectOutputStream.writeInt(state.getNumArcs());
				objectOutputStream.writeFloat(state.getFinalWeight());
				objectOutputStream.writeInt(state.getId());
				hashMap.put(state, Integer.valueOf(i));
			}
			i = this.states.size();
			for (int j = 0; j < i; j++)
			{
				State state2 = (State)this.states.get(j);
				int numArcs = state2.getNumArcs();
				for (int k = 0; k < numArcs; k++)
				{
					Arc arc = state2.getArc(k);
					objectOutputStream.writeInt(arc.getIlabel());
					objectOutputStream.writeInt(arc.getOlabel());
					objectOutputStream.writeFloat(arc.getWeight());
					objectOutputStream.writeInt(((Integer)hashMap.get(arc.getNextState())).intValue());
				}
			}
		}

		[Throws(new string[]
		{
			"java.io.IOException",
			"java.lang.ClassNotFoundException"
		})]
		[LineNumberTable(new byte[]
		{
			160,
			158,
			103,
			103,
			102,
			108,
			4,
			230,
			69
		})]
		
		protected internal static string[] readStringMap(ObjectInputStream @in)
		{
			int num = @in.readInt();
			string[] array = new string[num];
			for (int i = 0; i < num; i++)
			{
				string text = (string)@in.readObject();
				array[i] = text;
			}
			return array;
		}

		[LineNumberTable(new byte[]
		{
			20,
			232,
			36,
			231,
			93,
			100,
			140
		})]
		
		public Fst(int numStates)
		{
			this.states = null;
			if (numStates > 0)
			{
				this.states = new ArrayList(numStates);
			}
		}

		[Throws(new string[]
		{
			"java.io.IOException",
			"java.lang.ClassNotFoundException"
		})]
		[LineNumberTable(new byte[]
		{
			160,
			180,
			103,
			103,
			103,
			108,
			104,
			105,
			104,
			104,
			104,
			108,
			104,
			107,
			104,
			112,
			112,
			112,
			142,
			105,
			109,
			239,
			53,
			235,
			77,
			153,
			105,
			108,
			107,
			112,
			103,
			109,
			109,
			109,
			126,
			233,
			58,
			11,
			235,
			76
		})]
		
		private static Fst readFst(ObjectInputStream objectInputStream)
		{
			string[] array = Fst.readStringMap(objectInputStream);
			string[] array2 = Fst.readStringMap(objectInputStream);
			int num = objectInputStream.readInt();
			Semiring semiring = (Semiring)objectInputStream.readObject();
			int num2 = objectInputStream.readInt();
			Fst fst = new Fst(num2);
			fst.isyms = array;
			fst.osyms = array2;
			fst.semiring = semiring;
			for (int i = 0; i < num2; i++)
			{
				int num3 = objectInputStream.readInt();
				State state = new State(num3 + 1);
				float num4 = objectInputStream.readFloat();
				if (num4 == fst.semiring.zero())
				{
					num4 = fst.semiring.zero();
				}
				else if (num4 == fst.semiring.one())
				{
					num4 = fst.semiring.one();
				}
				state.setFinalWeight(num4);
				state.id = objectInputStream.readInt();
				fst.states.add(state);
			}
			fst.setStart((State)fst.states.get(num));
			num2 = fst.getNumStates();
			for (int i = 0; i < num2; i++)
			{
				State state2 = fst.getState(i);
				for (int j = 0; j < state2.initialNumArcs - 1; j++)
				{
					Arc arc = new Arc();
					arc.setIlabel(objectInputStream.readInt());
					arc.setOlabel(objectInputStream.readInt());
					arc.setWeight(objectInputStream.readFloat());
					arc.setNextState((State)fst.states.get(objectInputStream.readInt()));
					state2.addArc(arc);
				}
			}
			return fst;
		}

		[LineNumberTable(new byte[]
		{
			161,
			98,
			108,
			102,
			55,
			198
		})]
		
		public virtual void remapStateIds()
		{
			int num = this.states.size();
			for (int i = 0; i < num; i++)
			{
				((State)this.states.get(i)).id = i;
			}
		}

		public virtual Semiring getSemiring()
		{
			return this.semiring;
		}

		public virtual void setSemiring(Semiring semiring)
		{
			this.semiring = semiring;
		}

		[Throws(new string[]
		{
			"java.io.IOException"
		})]
		[LineNumberTable(new byte[]
		{
			160,
			135,
			103,
			103,
			103,
			103,
			102,
			102,
			102,
			102
		})]
		
		public virtual void saveModel(string filename)
		{
			FileOutputStream fileOutputStream = new FileOutputStream(filename);
			BufferedOutputStream bufferedOutputStream = new BufferedOutputStream(fileOutputStream);
			ObjectOutputStream objectOutputStream = new ObjectOutputStream(bufferedOutputStream);
			this.writeFst(objectOutputStream);
			objectOutputStream.flush();
			objectOutputStream.close();
			bufferedOutputStream.close();
			fileOutputStream.close();
		}

		[LineNumberTable(new byte[]
		{
			161,
			2,
			100,
			98,
			99,
			98,
			110,
			98,
			103,
			115,
			98,
			115,
			98,
			104,
			104,
			98,
			115,
			98,
			104,
			104,
			98,
			115,
			98,
			104,
			104,
			98,
			115,
			98
		})]
		
		public override bool equals(object obj)
		{
			if (this == obj)
			{
				return true;
			}
			if (obj == null)
			{
				return false;
			}
			if (this.GetType() != obj.GetType())
			{
				return false;
			}
			Fst fst = (Fst)obj;
			if (!Arrays.equals(this.isyms, fst.isyms))
			{
				return false;
			}
			if (!Arrays.equals(this.osyms, fst.osyms))
			{
				return false;
			}
			if (this.start == null)
			{
				if (fst.start != null)
				{
					return false;
				}
			}
			else if (!this.start.equals(fst.start))
			{
				return false;
			}
			if (this.states == null)
			{
				if (fst.states != null)
				{
					return false;
				}
			}
			else if (!this.states.equals(fst.states))
			{
				return false;
			}
			if (this.semiring == null)
			{
				if (fst.semiring != null)
				{
					return false;
				}
			}
			else if (!this.semiring.equals(fst.semiring))
			{
				return false;
			}
			return true;
		}

		[LineNumberTable(new byte[]
		{
			161,
			33,
			117,
			120,
			120,
			120,
			238,
			60
		})]
		
		public override int hashCode()
		{
			return 31 * (Arrays.hashCode(this.isyms) + 31 * (Arrays.hashCode(this.osyms) + 31 * (((this.start != null) ? this.start.hashCode() : 0) + 31 * (((this.states != null) ? this.states.hashCode() : 0) + 31 * ((this.semiring != null) ? Object.instancehelper_hashCode(this.semiring) : 0)))));
		}

		[LineNumberTable(new byte[]
		{
			161,
			47,
			102,
			127,
			38,
			63,
			15,
			134,
			108,
			105,
			114,
			127,
			12,
			104,
			105,
			106,
			31,
			13,
			232,
			60,
			233,
			74
		})]
		
		public override string toString()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.append(new StringBuilder().append("Fst(start=").append(this.start).append(", isyms=").append(Arrays.toString(this.isyms)).append(", osyms=").append(Arrays.toString(this.osyms)).append(", semiring=").append(this.semiring).append(")\n").toString());
			int num = this.states.size();
			for (int i = 0; i < num; i++)
			{
				State state = (State)this.states.get(i);
				stringBuilder.append(new StringBuilder().append("  ").append(state).append("\n").toString());
				int numArcs = state.getNumArcs();
				for (int j = 0; j < numArcs; j++)
				{
					Arc arc = state.getArc(j);
					stringBuilder.append(new StringBuilder().append("    ").append(arc).append("\n").toString());
				}
			}
			return stringBuilder.toString();
		}

		[LineNumberTable(new byte[]
		{
			161,
			72,
			105,
			111,
			161,
			141,
			127,
			4,
			102,
			107,
			105,
			111,
			233,
			61,
			230,
			70,
			103,
			101
		})]
		
		public virtual void deleteState(State state)
		{
			if (state == this.start)
			{
				java.lang.System.err.println("Cannot delete start state.");
				return;
			}
			this.states.remove(state);
			Iterator iterator = this.states.iterator();
			while (iterator.hasNext())
			{
				State state2 = (State)iterator.next();
				ArrayList arrayList = new ArrayList();
				for (int i = 0; i < state2.getNumArcs(); i++)
				{
					Arc arc = state2.getArc(i);
					if (!arc.getNextState().equals(state))
					{
						arrayList.add(arc);
					}
				}
				state2.setArcs(arrayList);
			}
		}

		
		[LineNumberTable(new byte[]
		{
			161,
			107,
			110,
			111,
			161,
			134,
			127,
			4,
			108,
			104,
			102,
			109,
			106,
			111,
			233,
			61,
			232,
			70,
			135,
			101,
			135,
			102
		})]
		
		public virtual void deleteStates(HashSet toDelete)
		{
			if (toDelete.contains(this.start))
			{
				java.lang.System.err.println("Cannot delete start state.");
				return;
			}
			ArrayList arrayList = new ArrayList();
			Iterator iterator = this.states.iterator();
			while (iterator.hasNext())
			{
				State state = (State)iterator.next();
				if (!toDelete.contains(state))
				{
					arrayList.add(state);
					ArrayList arrayList2 = new ArrayList();
					for (int i = 0; i < state.getNumArcs(); i++)
					{
						Arc arc = state.getArc(i);
						if (!toDelete.contains(arc.getNextState()))
						{
							arrayList2.add(arc);
						}
					}
					state.setArcs(arrayList2);
				}
			}
			this.states = arrayList;
			this.remapStateIds();
		}

		
		private ArrayList states;

		protected internal State start;

		protected internal string[] isyms;

		protected internal string[] osyms;

		protected internal Semiring semiring;
	}
}
