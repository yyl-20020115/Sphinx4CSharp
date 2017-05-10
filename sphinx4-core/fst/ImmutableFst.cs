using System;
using System.ComponentModel;

using edu.cmu.sphinx.fst.semiring;
using IKVM.Attributes;
using IKVM.Runtime;
using java.io;
using java.lang;
using java.util;

namespace edu.cmu.sphinx.fst
{
	public class ImmutableFst : Fst
	{
		[LineNumberTable(new byte[]
		{
			4,
			233,
			37,
			231,
			92,
			103,
			108
		})]
		
		private ImmutableFst(int num) : base(0)
		{
			this.states = null;
			this.numStates = num;
			this.states = new ImmutableState[num];
		}

		[Throws(new string[]
		{
			"java.io.IOException",
			"java.lang.ClassNotFoundException"
		})]
		[LineNumberTable(new byte[]
		{
			62,
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
			241,
			53,
			235,
			77,
			144,
			106,
			108,
			108,
			112,
			103,
			109,
			109,
			109,
			117,
			235,
			58,
			8,
			235,
			76
		})]
		
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

		[LineNumberTable(new byte[]
		{
			159,
			181,
			232,
			52,
			231,
			78
		})]
		
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

		[Throws(new string[]
		{
			"java.io.IOException"
		})]
		
		
		public override void saveModel(string filename)
		{
			string text = "You cannot serialize an ImmutableFst.";
			
			throw new IllegalArgumentException(text);
		}

		[Throws(new string[]
		{
			"java.io.IOException",
			"java.lang.ClassNotFoundException"
		})]
		[LineNumberTable(new byte[]
		{
			116,
			98,
			98,
			103,
			103,
			103,
			102,
			102,
			134
		})]
		
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

		[LineNumberTable(new byte[]
		{
			160,
			75,
			98,
			98,
			98,
			103,
			103,
			103,
			103,
			102,
			102,
			255,
			23,
			74,
			226,
			55,
			98,
			103,
			98,
			98,
			103,
			98,
			98,
			103,
			162
		})]
		
		public new static ImmutableFst loadModel(string filename)
		{
			ImmutableFst result;
			FileNotFoundException ex2;
			IOException ex4;
			ClassNotFoundException ex6;
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
						ex2 = ByteCodeHelper.MapException<FileNotFoundException>(ex, 1);
						goto IL_58;
					}
				}
				catch (IOException ex3)
				{
					ex4 = ByteCodeHelper.MapException<IOException>(ex3, 1);
					goto IL_5C;
				}
			}
			catch (ClassNotFoundException ex5)
			{
				ex6 = ByteCodeHelper.MapException<ClassNotFoundException>(ex5, 1);
				goto IL_60;
			}
			return result;
			IL_58:
			FileNotFoundException ex7 = ex2;
			Throwable.instancehelper_printStackTrace(ex7);
			return null;
			IL_5C:
			IOException ex8 = ex4;
			Throwable.instancehelper_printStackTrace(ex8);
			return null;
			IL_60:
			ClassNotFoundException ex9 = ex6;
			Throwable.instancehelper_printStackTrace(ex9);
			return null;
		}

		
		
		public override void deleteState(State state)
		{
			string text = "You cannot modify an ImmutableFst.";
			
			throw new IllegalArgumentException(text);
		}

		[LineNumberTable(new byte[]
		{
			160,
			116,
			102,
			127,
			38,
			63,
			15,
			134,
			104,
			105,
			105,
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

		[LineNumberTable(new byte[]
		{
			160,
			140,
			100,
			98,
			110,
			98,
			103,
			115,
			98,
			105,
			98
		})]
		
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

		
		[EditorBrowsable(EditorBrowsableState.Never)]
		
		
		public virtual State <bridge>getState(int i)
		{
			return this.getState(i);
		}

		private ImmutableState[] states;

		private int numStates;
	}
}
