using edu.cmu.sphinx.fst.semiring;
using java.io;
using java.lang;
using java.util;

namespace edu.cmu.sphinx.fst
{
	public class ImmutableFst : Fst
	{		
		private ImmutableFst(int num) : base(0)
		{
			this.states = null;
			this.numStates = num;
			this.states = new ImmutableState[num];
		}
		
		private static ImmutableFst readImmutableFst(ObjectInputStream objectInputStream)
		{
			string[] isyms = Fst.readStringMap(objectInputStream);
			string[] osyms = Fst.readStringMap(objectInputStream);
			int num = objectInputStream.readInt();
			Semiring semiring = (Semiring)objectInputStream.readObject();
			int num2 = objectInputStream.readInt();
			ImmutableFst immutableFst = new ImmutableFst(num2);
			immutableFst.isyms = isyms;
			immutableFst.osyms = osyms;
			immutableFst.semiring = semiring;
			for (int i = 0; i < num2; i++)
			{
				int num3 = objectInputStream.readInt();
				ImmutableState immutableState = new ImmutableState(num3 + 1);
				float num4 = objectInputStream.readFloat();
				if (num4 == immutableFst.semiring.zero())
				{
					num4 = immutableFst.semiring.zero();
				}
				else if (num4 == immutableFst.semiring.one())
				{
					num4 = immutableFst.semiring.one();
				}
				immutableState.setFinalWeight(num4);
				immutableState.id = objectInputStream.readInt();
				immutableFst.states[immutableState.getId()] = immutableState;
			}
			immutableFst.setStart(immutableFst.states[num]);
			num2 = immutableFst.states.Length;
			for (int i = 0; i < num2; i++)
			{
				ImmutableState immutableState2 = immutableFst.states[i];
				for (int j = 0; j < immutableState2.initialNumArcs - 1; j++)
				{
					Arc arc = new Arc();
					arc.setIlabel(objectInputStream.readInt());
					arc.setOlabel(objectInputStream.readInt());
					arc.setWeight(objectInputStream.readFloat());
					arc.setNextState(immutableFst.states[objectInputStream.readInt()]);
					immutableState2.setArc(j, arc);
				}
			}
			return immutableFst;
		}

		public new virtual ImmutableState getState(int index)
		{
			return this.states[index];
		}
		
		private ImmutableFst()
		{
			this.states = null;
		}

		public override int getNumStates()
		{
			return this.numStates;
		}

		public override void addState(State state)
		{
			string text = "You cannot modify an ImmutableFst.";
			
			throw new IllegalArgumentException(text);
		}
		
		public override void saveModel(string filename)
		{
			string text = "You cannot serialize an ImmutableFst.";
			
			throw new IllegalArgumentException(text);
		}
		
		public static ImmutableFst loadModel(InputStream inputStream)
		{
			BufferedInputStream bufferedInputStream = new BufferedInputStream(inputStream);
			ObjectInputStream objectInputStream = new ObjectInputStream(bufferedInputStream);
			ImmutableFst result = ImmutableFst.readImmutableFst(objectInputStream);
			objectInputStream.close();
			bufferedInputStream.close();
			inputStream.close();
			return result;
		}
		
		public new static ImmutableFst loadModel(string filename)
		{
			ImmutableFst result;
			try
			{
				try
				{
					try
					{
						FileInputStream fileInputStream = new FileInputStream(filename);
						BufferedInputStream bufferedInputStream = new BufferedInputStream(fileInputStream);
						ObjectInputStream objectInputStream = new ObjectInputStream(bufferedInputStream);
						result = ImmutableFst.readImmutableFst(objectInputStream);
						objectInputStream.close();
						bufferedInputStream.close();
						fileInputStream.close();
					}
					catch (FileNotFoundException ex)
					{
						Throwable.instancehelper_printStackTrace(ex);
						return null;
					}
				}
				catch (IOException ex3)
				{
					Throwable.instancehelper_printStackTrace(ex3);
					return null;
				}
			}
			catch (ClassNotFoundException ex5)
			{
				Throwable.instancehelper_printStackTrace(ex5);
				return null;
			}
			return result;
		}
		
		public override void deleteState(State state)
		{
			string text = "You cannot modify an ImmutableFst.";
			
			throw new IllegalArgumentException(text);
		}
		
		public override string toString()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.append(new StringBuilder().append("Fst(start=").append(this.start).append(", isyms=").append(Arrays.toString(this.isyms)).append(", osyms=").append(Arrays.toString(this.osyms)).append(", semiring=").append(this.semiring).append(")\n").toString());
			int num = this.states.Length;
			for (int i = 0; i < num; i++)
			{
				ImmutableState immutableState = this.states[i];
				stringBuilder.append(new StringBuilder().append("  ").append(immutableState).append("\n").toString());
				int numArcs = immutableState.getNumArcs();
				for (int j = 0; j < numArcs; j++)
				{
					Arc arc = immutableState.getArc(j);
					stringBuilder.append(new StringBuilder().append("    ").append(arc).append("\n").toString());
				}
			}
			return stringBuilder.toString();
		}
		
		public override bool equals(object obj)
		{
			if (this == obj)
			{
				return true;
			}
			if (this.GetType() != obj.GetType())
			{
				return false;
			}
			ImmutableFst immutableFst = (ImmutableFst)obj;
			return Arrays.equals(this.states, immutableFst.states) && base.equals(obj);
		}
		
		public override int hashCode()
		{
			return Arrays.hashCode(this.states) + base.hashCode();
		}
		
		private ImmutableState[] states;

		private int numStates;
	}
}
