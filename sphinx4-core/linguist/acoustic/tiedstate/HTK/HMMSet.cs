using System;

using IKVM.Attributes;
using IKVM.Runtime;
using java.io;
using java.lang;
using java.util;

namespace edu.cmu.sphinx.linguist.acoustic.tiedstate.HTK
{
	public class HMMSet : java.lang.Object
	{
		[LineNumberTable(new byte[]
		{
			160,
			71,
			232,
			159,
			100,
			107,
			235,
			160,
			156,
			107,
			107,
			107
		})]
		
		public HMMSet()
		{
			this.__transitions = new ArrayList();
			this.__transNames = new HashMap();
			this.__states = new ArrayList();
			this.__hmms = new ArrayList();
			this.__gmms = new ArrayList();
		}

		[LineNumberTable(new byte[]
		{
			160,
			79,
			172,
			103,
			99,
			101,
			109,
			110,
			37,
			134,
			105,
			145,
			109,
			110,
			37,
			134,
			106,
			117,
			110,
			37,
			134,
			110,
			106,
			133,
			127,
			0,
			133,
			184,
			2,
			98,
			135
		})]
		
		public virtual void loadHTK(string nomFich)
		{
			IOException ex2;
			try
			{
				BufferedReader bufferedReader = new BufferedReader(new FileReader(nomFich));
				for (;;)
				{
					string text = bufferedReader.readLine();
					if (text == null)
					{
						break;
					}
					if (java.lang.String.instancehelper_startsWith(text, "~s"))
					{
						string text2 = java.lang.String.instancehelper_substring(text, java.lang.String.instancehelper_indexOf(text, 34) + 1, java.lang.String.instancehelper_lastIndexOf(text, 34));
						this.loadState(bufferedReader, text2, null);
					}
					else if (!java.lang.String.instancehelper_startsWith(text, "~v"))
					{
						if (java.lang.String.instancehelper_startsWith(text, "~t"))
						{
							string text2 = java.lang.String.instancehelper_substring(text, java.lang.String.instancehelper_indexOf(text, 34) + 1, java.lang.String.instancehelper_lastIndexOf(text, 34));
							this.loadTrans(bufferedReader, text2, null);
						}
						else if (java.lang.String.instancehelper_startsWith(text, "~h"))
						{
							string text2 = java.lang.String.instancehelper_substring(text, java.lang.String.instancehelper_indexOf(text, 34) + 1, java.lang.String.instancehelper_lastIndexOf(text, 34));
							if (java.lang.String.instancehelper_equals(java.lang.String.instancehelper_toUpperCase(text2), text2))
							{
								java.lang.System.@out.println("WARNING: HMM is in lowercase, converting to upper");
							}
							this.__hmms.add(this.loadHMM(bufferedReader, java.lang.String.instancehelper_toUpperCase(text2), this.__gmms));
						}
					}
				}
				bufferedReader.close();
			}
			catch (IOException ex)
			{
				ex2 = ByteCodeHelper.MapException<IOException>(ex, 1);
				goto IL_10F;
			}
			return;
			IL_10F:
			IOException ex3 = ex2;
			Throwable.instancehelper_printStackTrace(ex3);
		}

		
		
		public virtual int getNstates()
		{
			return this.__gmms.size();
		}

		
		
		public virtual int getNhmms()
		{
			return this.__hmms.size();
		}

		
		
		public virtual SingleHMM getHMM(int idx)
		{
			return (SingleHMM)this.__hmms.get(idx);
		}

		
		[LineNumberTable(new byte[]
		{
			159,
			175,
			231,
			86
		})]
		
		public virtual Iterator get1phIt()
		{
			return new HMMSet_1(this);
		}

		
		public virtual int getStateIdx(HMMState st)
		{
			return st.gmmidx;
		}

		
		[LineNumberTable(new byte[]
		{
			9,
			231,
			86
		})]
		
		public virtual Iterator get3phIt()
		{
			return new HMMSet_2(this);
		}

		[Throws(new string[]
		{
			"java.io.IOException"
		})]
		[LineNumberTable(new byte[]
		{
			161,
			24,
			135,
			99,
			132,
			108,
			109,
			123,
			140,
			103,
			141,
			105,
			159,
			36,
			134,
			105,
			182,
			110,
			100,
			140,
			109,
			108,
			108,
			109,
			159,
			18,
			134,
			105,
			244,
			51,
			233,
			80,
			107,
			108,
			114
		})]
		
		private void loadState(BufferedReader bufferedReader, string nom, string text)
		{
			this.nGaussians = 1;
			string text2;
			if (text != null)
			{
				text2 = text;
			}
			else
			{
				text2 = java.lang.String.instancehelper_trim(bufferedReader.readLine());
			}
			if (java.lang.String.instancehelper_startsWith(text2, "<NUMMIXES>"))
			{
				this.nGaussians = Integer.parseInt(java.lang.String.instancehelper_substring(text2, java.lang.String.instancehelper_indexOf(text2, 32) + 1));
				text2 = java.lang.String.instancehelper_trim(bufferedReader.readLine());
			}
			this.g = null;
			if (!java.lang.String.instancehelper_startsWith(text2, "<MIXTURE>"))
			{
				if (this.nGaussians != 1)
				{
					java.lang.System.err.println(new StringBuilder().append("Error loading model: number of mixtures is ").append(this.nGaussians).append(" while state ").append(text2).append(" has 1 mixture.").toString());
					java.lang.System.exit(1);
				}
				this.loadHTKGauss(bufferedReader, 0, text2);
				this.g.setWeight(0, 1f);
			}
			else
			{
				for (int i = 0; i < this.nGaussians; i++)
				{
					if (i > 0)
					{
						text2 = java.lang.String.instancehelper_trim(bufferedReader.readLine());
					}
					if (java.lang.String.instancehelper_startsWith(text2, "<GCONST>"))
					{
						text2 = java.lang.String.instancehelper_trim(bufferedReader.readLine());
					}
					string[] array = java.lang.String.instancehelper_split(text2, " ");
					if (Integer.parseInt(array[1]) != i + 1)
					{
						java.lang.System.err.println(new StringBuilder().append("Error reading model: mixture conflict ").append(i).append(' ').append(text2).toString());
						java.lang.System.exit(1);
					}
					this.loadHTKGauss(bufferedReader, i, null);
					this.g.setWeight(i, Float.parseFloat(array[2]));
				}
			}
			this.g.precomputeDistance();
			this.g.setNom(nom);
			this.__gmms.add(this.g);
		}

		[Throws(new string[]
		{
			"java.io.IOException"
		})]
		[LineNumberTable(new byte[]
		{
			160,
			237,
			98,
			99,
			132,
			108,
			109,
			150,
			134,
			111,
			166,
			159,
			10,
			103,
			104,
			108,
			109,
			136,
			24,
			232,
			61,
			232,
			72,
			99,
			109,
			116,
			114,
			131
		})]
		
		private int loadTrans(BufferedReader bufferedReader, string text, string text2)
		{
			int num = 0;
			string text3;
			if (text2 != null)
			{
				text3 = text2;
			}
			else
			{
				text3 = java.lang.String.instancehelper_trim(bufferedReader.readLine());
			}
			if (java.lang.String.instancehelper_startsWith(text3, "<TRANSP>"))
			{
				num = Integer.parseInt(java.lang.String.instancehelper_substring(text3, java.lang.String.instancehelper_indexOf(text3, 32) + 1));
				num += -1;
			}
			else
			{
				java.lang.System.err.println("ERROR no TRANSP !");
				java.lang.System.exit(1);
			}
			int num2 = num;
			int num3 = num;
			int[] array = new int[2];
			int num4 = num3;
			array[1] = num4;
			num4 = num2;
			array[0] = num4;
			this.trans = (float[][])ByteCodeHelper.multianewarray(typeof(float[][]).TypeHandle, array);
			bufferedReader.readLine();
			for (int i = 0; i < num; i++)
			{
				text3 = java.lang.String.instancehelper_trim(bufferedReader.readLine());
				string[] array2 = java.lang.String.instancehelper_split(text3, " ");
				for (int j = 0; j < num; j++)
				{
					this.trans[i][j] = Float.parseFloat(array2[j + 1]);
				}
			}
			if (text != null)
			{
				int i = this.__transitions.size();
				this.__transNames.put(text, Integer.valueOf(i));
				this.__transitions.add(this.trans);
				return i;
			}
			return -1;
		}

		[Throws(new string[]
		{
			"java.io.IOException"
		})]
		
		[LineNumberTable(new byte[]
		{
			160,
			161,
			130,
			98,
			134,
			109,
			137,
			150,
			100,
			104,
			104,
			104,
			109,
			105,
			112,
			151,
			102,
			103,
			98,
			112,
			110,
			37,
			167,
			109,
			110,
			111,
			226,
			61,
			232,
			69,
			100,
			106,
			127,
			15,
			134,
			98,
			109,
			111,
			158,
			112,
			105,
			110,
			109,
			135,
			109,
			103,
			101,
			141,
			110,
			37,
			135,
			106,
			105,
			130,
			109,
			127,
			5,
			134,
			106,
			141,
			103,
			109,
			127,
			5,
			134
		})]
		
		private SingleHMM loadHMM(BufferedReader bufferedReader, string text, List list)
		{
			GMMDiag gmmdiag = null;
			string text2 = "";
			while (!java.lang.String.instancehelper_startsWith(text2, "<NUMSTATES>"))
			{
				text2 = bufferedReader.readLine();
			}
			int num = Integer.parseInt(java.lang.String.instancehelper_substring(text2, java.lang.String.instancehelper_indexOf(text2, 32) + 1));
			num += -1;
			SingleHMM singleHMM = new SingleHMM(num);
			singleHMM.setName(text);
			singleHMM.hmmset = this;
			while (!java.lang.String.instancehelper_startsWith(text2, "<STATE>"))
			{
				text2 = bufferedReader.readLine();
			}
			while (java.lang.String.instancehelper_startsWith(text2, "<STATE>"))
			{
				int num2 = Integer.parseInt(java.lang.String.instancehelper_substring(text2, java.lang.String.instancehelper_indexOf(text2, 32) + 1));
				num2 += -1;
				text2 = bufferedReader.readLine();
				int gmmidx;
				if (java.lang.String.instancehelper_startsWith(text2, "~s"))
				{
					string text3 = java.lang.String.instancehelper_substring(text2, java.lang.String.instancehelper_indexOf(text2, 34) + 1, java.lang.String.instancehelper_lastIndexOf(text2, 34));
					int i;
					for (i = 0; i < list.size(); i++)
					{
						gmmdiag = (GMMDiag)list.get(i);
						if (java.lang.String.instancehelper_equals(gmmdiag.nom, text3))
						{
							break;
						}
					}
					gmmidx = i;
					if (i == list.size())
					{
						java.lang.System.err.println(new StringBuilder().append("Error creatiing HMM : state ").append(text).append(" not found").toString());
						java.lang.System.exit(1);
					}
				}
				else
				{
					this.loadState(bufferedReader, "", text2);
					gmmidx = this.__gmms.size() - 1;
					gmmdiag = (GMMDiag)this.__gmms.get(this.__gmms.size() - 1);
				}
				HMMState hmmstate = new HMMState(gmmdiag, new Lab(text, num2));
				hmmstate.gmmidx = gmmidx;
				this.__states.add(hmmstate);
				singleHMM.setState(num2 - 1, hmmstate);
				text2 = bufferedReader.readLine();
				if (java.lang.String.instancehelper_startsWith(text2, "<GCONST>"))
				{
					text2 = bufferedReader.readLine();
				}
			}
			if (java.lang.String.instancehelper_startsWith(text2, "~t"))
			{
				string text4 = java.lang.String.instancehelper_substring(text2, java.lang.String.instancehelper_indexOf(text2, 34) + 1, java.lang.String.instancehelper_lastIndexOf(text2, 34));
				int num3 = this.getTrans(text4);
				singleHMM.setTrans(num3);
			}
			else
			{
				if (!java.lang.String.instancehelper_startsWith(text2, "<TRANSP>"))
				{
					java.lang.System.err.println(new StringBuilder().append("Error reading model: missing transitions.").append(text2).toString());
					java.lang.System.exit(1);
				}
				this.loadTrans(bufferedReader, null, text2);
				singleHMM.setTrans(this.trans);
			}
			text2 = bufferedReader.readLine();
			if (!java.lang.String.instancehelper_startsWith(text2, "<ENDHMM>"))
			{
				java.lang.System.err.println(new StringBuilder().append("Error reading model: missing ENDHMM.").append(text2).toString());
				java.lang.System.exit(1);
			}
			return singleHMM;
		}

		[LineNumberTable(new byte[]
		{
			161,
			18,
			119
		})]
		
		private int getTrans(string text)
		{
			return ((Integer)this.__transNames.get(text)).intValue();
		}

		[Throws(new string[]
		{
			"java.io.IOException"
		})]
		[LineNumberTable(new byte[]
		{
			161,
			76,
			131,
			132,
			108,
			109,
			108,
			109,
			108,
			109,
			127,
			5,
			134,
			118,
			104,
			114,
			108,
			108,
			101,
			159,
			49,
			134,
			102,
			53,
			166,
			108,
			109,
			127,
			5,
			134,
			108,
			108,
			101,
			159,
			18,
			134,
			102,
			53,
			166
		})]
		
		private void loadHTKGauss(BufferedReader bufferedReader, int i, string text)
		{
			string text2;
			if (text != null)
			{
				text2 = text;
			}
			else
			{
				text2 = java.lang.String.instancehelper_trim(bufferedReader.readLine());
			}
			if (java.lang.String.instancehelper_startsWith(text2, "<GCONST>"))
			{
				text2 = java.lang.String.instancehelper_trim(bufferedReader.readLine());
			}
			if (java.lang.String.instancehelper_startsWith(text2, "<RCLASS>"))
			{
				text2 = java.lang.String.instancehelper_trim(bufferedReader.readLine());
			}
			if (!java.lang.String.instancehelper_startsWith(text2, "<MEAN>"))
			{
				java.lang.System.err.println(new StringBuilder().append("Error loading model: can't find <MEAN> ! ").append(text2).toString());
				java.lang.System.exit(1);
			}
			int num = Integer.parseInt(java.lang.String.instancehelper_substring(text2, java.lang.String.instancehelper_indexOf(text2, 32) + 1));
			if (this.g == null)
			{
				this.g = new GMMDiag(this.nGaussians, num);
			}
			text2 = java.lang.String.instancehelper_trim(bufferedReader.readLine());
			string[] array = java.lang.String.instancehelper_split(text2, " ");
			if (array.Length != num)
			{
				java.lang.System.err.println(new StringBuilder().append("Error loading model: incorrect number of coefficients ").append(num).append(' ').append(text2).append(' ').append(array[0]).append(' ').append(array[39]).toString());
				java.lang.System.exit(1);
			}
			for (int j = 0; j < num; j++)
			{
				this.g.setMean(i, j, Float.parseFloat(array[j]));
			}
			text2 = java.lang.String.instancehelper_trim(bufferedReader.readLine());
			if (!java.lang.String.instancehelper_startsWith(text2, "<VARIANCE>"))
			{
				java.lang.System.err.println(new StringBuilder().append("Error loading model: missing <VARIANCE> ! ").append(text2).toString());
				java.lang.System.exit(1);
			}
			text2 = java.lang.String.instancehelper_trim(bufferedReader.readLine());
			array = java.lang.String.instancehelper_split(text2, " ");
			if (array.Length != num)
			{
				java.lang.System.err.println(new StringBuilder().append("Error loading model: incorrect number of coefficients ").append(num).append(' ').append(text2).toString());
				java.lang.System.exit(1);
			}
			for (int j = 0; j < num; j++)
			{
				this.g.setVar(i, j, Float.parseFloat(array[j]));
			}
		}

		[LineNumberTable(new byte[]
		{
			39,
			112,
			114,
			100,
			226,
			61,
			230,
			69
		})]
		
		public virtual int getHMMidx(SingleHMM hmm)
		{
			for (int i = 0; i < this.__hmms.size(); i++)
			{
				SingleHMM singleHMM = (SingleHMM)this.__hmms.get(i);
				if (singleHMM == hmm)
				{
					return i;
				}
			}
			return -1;
		}

		[LineNumberTable(new byte[]
		{
			52,
			113,
			103,
			114,
			9,
			198
		})]
		
		public virtual string[] getHMMnames()
		{
			string[] array = new string[this.__hmms.size()];
			for (int i = 0; i < array.Length; i++)
			{
				SingleHMM singleHMM = (SingleHMM)this.__hmms.get(i);
				array[i] = singleHMM.getName();
			}
			return array;
		}

		[LineNumberTable(new byte[]
		{
			74,
			98,
			127,
			1,
			127,
			1,
			100,
			98
		})]
		
		public virtual int getNhmmsMono()
		{
			int num = 0;
			Iterator iterator = this.__hmms.iterator();
			while (iterator.hasNext())
			{
				SingleHMM singleHMM = (SingleHMM)iterator.next();
				if (java.lang.String.instancehelper_indexOf(singleHMM.getName(), 45) < 0 && java.lang.String.instancehelper_indexOf(singleHMM.getName(), 43) < 0)
				{
					num++;
				}
			}
			return num;
		}

		[LineNumberTable(new byte[]
		{
			83,
			98,
			127,
			1,
			113,
			111,
			100,
			98
		})]
		
		public virtual int getNhmmsTri()
		{
			int num = 0;
			Iterator iterator = this.__hmms.iterator();
			while (iterator.hasNext())
			{
				SingleHMM singleHMM = (SingleHMM)iterator.next();
				if (java.lang.String.instancehelper_indexOf(singleHMM.getName(), 45) >= 0 || java.lang.String.instancehelper_indexOf(singleHMM.getName(), 43) >= 0)
				{
					num++;
				}
			}
			return num;
		}

		
		
		public virtual int getHMMIndex(SingleHMM h)
		{
			return this.__hmms.indexOf(h);
		}

		[LineNumberTable(new byte[]
		{
			104,
			98,
			102,
			114,
			9,
			198,
			114,
			102,
			105,
			4,
			198,
			105,
			162
		})]
		
		public virtual int getStateIdx(int hmmidx, int stateidx)
		{
			int num = 0;
			SingleHMM singleHMM;
			for (int i = 0; i < hmmidx; i++)
			{
				singleHMM = (SingleHMM)this.__hmms.get(i);
				num += singleHMM.getNbEmittingStates();
			}
			singleHMM = (SingleHMM)this.__hmms.get(hmmidx);
			for (int i = 1; i < stateidx; i++)
			{
				if (singleHMM.isEmitting(i))
				{
					num++;
				}
			}
			if (singleHMM.isEmitting(stateidx))
			{
				return num;
			}
			return -1;
		}

		[LineNumberTable(new byte[]
		{
			126,
			98,
			127,
			1,
			98,
			110,
			98,
			98
		})]
		
		public virtual SingleHMM getHMM(string nom)
		{
			SingleHMM singleHMM = null;
			Iterator iterator = this.__hmms.iterator();
			while (iterator.hasNext())
			{
				SingleHMM singleHMM2 = (SingleHMM)iterator.next();
				singleHMM = singleHMM2;
				if (java.lang.String.instancehelper_equals(singleHMM.getName(), nom))
				{
					break;
				}
			}
			return singleHMM;
		}

		[LineNumberTable(new byte[]
		{
			160,
			115,
			172,
			130,
			103,
			99,
			98,
			108,
			133,
			166,
			127,
			18,
			102,
			108,
			99,
			103,
			99,
			98,
			108,
			133,
			110,
			182,
			186,
			2,
			98,
			135
		})]
		
		public virtual void loadTiedList(string nomFich)
		{
			IOException ex2;
			try
			{
				BufferedReader bufferedReader = new BufferedReader(new FileReader(nomFich));
				int num = 0;
				for (;;)
				{
					string text = bufferedReader.readLine();
					if (text == null)
					{
						break;
					}
					string[] array = java.lang.String.instancehelper_split(text, " ");
					if (array.Length >= 2)
					{
						num++;
					}
				}
				int num2 = num;
				int num3 = 2;
				int[] array2 = new int[2];
				int num4 = num3;
				array2[1] = num4;
				num4 = num2;
				array2[0] = num4;
				this.tiedHMMs = (string[][])ByteCodeHelper.multianewarray(typeof(string[][]).TypeHandle, array2);
				bufferedReader.close();
				bufferedReader = new BufferedReader(new FileReader(nomFich));
				int num5 = 0;
				for (;;)
				{
					string text = bufferedReader.readLine();
					if (text == null)
					{
						break;
					}
					string[] array = java.lang.String.instancehelper_split(text, " ");
					if (array.Length >= 2)
					{
						this.tiedHMMs[num5][0] = array[0];
						string[][] array3 = this.tiedHMMs;
						int num6 = num5;
						num5++;
						array3[num6][1] = array[1];
					}
				}
				bufferedReader.close();
			}
			catch (IOException ex)
			{
				ex2 = ByteCodeHelper.MapException<IOException>(ex, 1);
				goto IL_CE;
			}
			return;
			IL_CE:
			IOException ex3 = ex2;
			Throwable.instancehelper_printStackTrace(ex3);
		}

		[LineNumberTable(new byte[]
		{
			161,
			121,
			130,
			112,
			114,
			110,
			226,
			61,
			230,
			69,
			110,
			135,
			136,
			108,
			119,
			2,
			230,
			69,
			106,
			119,
			165,
			127,
			5
		})]
		
		public virtual GMMDiag findState(Lab l)
		{
			HMMState hmmstate;
			for (;;)
			{
				hmmstate = null;
				int i;
				for (i = 0; i < this.__states.size(); i++)
				{
					hmmstate = (HMMState)this.__states.get(i);
					if (hmmstate.getLab().isEqual(l))
					{
						break;
					}
				}
				if (i < this.__states.size())
				{
					break;
				}
				if (this.tiedHMMs == null)
				{
					goto IL_A8;
				}
				for (i = 0; i < this.tiedHMMs.Length; i++)
				{
					if (java.lang.String.instancehelper_equals(this.tiedHMMs[i][0], l.getName()))
					{
						break;
					}
				}
				if (i >= this.tiedHMMs.Length)
				{
					goto IL_A8;
				}
				l = new Lab(this.tiedHMMs[i][1], l.getState());
			}
			return hmmstate.__gmm;
			IL_A8:
			java.lang.System.err.println(new StringBuilder().append("WARNING: state is not found in hmmset ").append(l).toString());
			return null;
		}

		
		public List states
		{
			
			get
			{
				return this.__states;
			}
			
			private set
			{
				this.__states = value;
			}
		}

		
		public List transitions
		{
			
			get
			{
				return this.__transitions;
			}
			
			private set
			{
				this.__transitions = value;
			}
		}

		
		public Map transNames
		{
			
			get
			{
				return this.__transNames;
			}
			
			private set
			{
				this.__transNames = value;
			}
		}

		
		public List gmms
		{
			
			get
			{
				return this.__gmms;
			}
			
			private set
			{
				this.__gmms = value;
			}
		}

		
		public List hmms
		{
			
			get
			{
				return this.__hmms;
			}
			
			private set
			{
				this.__hmms = value;
			}
		}

		private GMMDiag g;

		private int nGaussians;

		internal float[][] trans;

		
		internal List __states;

		
		internal List __transitions;

		
		internal Map __transNames;

		
		internal List __gmms;

		
		internal List __hmms;

		private string[][] tiedHMMs;
	}
}
