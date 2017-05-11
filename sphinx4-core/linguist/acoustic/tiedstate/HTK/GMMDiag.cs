using edu.cmu.sphinx.util;
using IKVM.Runtime;
using java.io;
using java.lang;
using java.util;

namespace edu.cmu.sphinx.linguist.acoustic.tiedstate.HTK
{
	public class GMMDiag : java.lang.Object
	{
		public virtual int getNgauss()
		{
			return this.ngauss;
		}

		public virtual int getNcoefs()
		{
			return this.ncoefs;
		}

		public virtual float getMean(int i, int j)
		{
			return this.means[i][j];
		}
		
		public virtual float getVar(int i, int j)
		{
			return -1f / (2f * this.covar[i][j]);
		}
		
		public virtual float getWeight(int i)
		{
			return (float)this.logMath.logToLinear(this.weights[i]);
		}
		
		private void allocate()
		{
			if (this.weights == null)
			{
				this.allocateWeights();
			}
			if (this.means == null)
			{
				this.loglikes = new float[this.ngauss];
				int num = this.ngauss;
				int num2 = this.ncoefs;
				int[] array = new int[2];
				int num3 = num2;
				array[1] = num3;
				num3 = num;
				array[0] = num3;
				this.means = (float[][])ByteCodeHelper.multianewarray(typeof(float[][]).TypeHandle, array);
				int num4 = this.ngauss;
				int num5 = this.ncoefs;
				array = new int[2];
				num3 = num5;
				array[1] = num3;
				num3 = num4;
				array[0] = num3;
				this.covar = (float[][])ByteCodeHelper.multianewarray(typeof(float[][]).TypeHandle, array);
				this.logPreComputedGaussianFactor = new float[this.ngauss];
			}
		}
		
		public virtual void setWeight(int i, float w)
		{
			if (this.weights == null)
			{
				this.weights = new float[this.ngauss];
			}
			this.weights[i] = this.logMath.linearToLog((double)w);
		}

		public virtual void setMean(int i, int j, float v)
		{
			this.means[i][j] = v;
		}
		
		public virtual void setVar(int i, int j, float v)
		{
			if (v <= 0f)
			{
				java.lang.System.err.println(new StringBuilder().append("WARNING: setVar ").append(v).toString());
			}
			this.covar[i][j] = -1f / (2f * v);
		}
		
		public virtual void precomputeDistance()
		{
			for (int i = 0; i < this.ngauss; i++)
			{
				float num = 0f;
				for (int j = 0; j < this.ncoefs; j++)
				{
					num += this.logMath.linearToLog((double)this.getVar(i, j));
				}
				num += this.logMath.linearToLog(6.2831853071795862) * (float)this.ncoefs;
				this.logPreComputedGaussianFactor[i] = num * 0.5f;
			}
		}
		
		public virtual void saveHTK(string nomFich, string nomHMM, string parmKind)
		{
			try
			{
				PrintWriter printWriter = new PrintWriter(new FileWriter(nomFich));
				printWriter.println("~o");
				printWriter.println("<HMMSETID> tree");
				printWriter.println(new StringBuilder().append("<STREAMINFO> 1 ").append(this.getNcoefs()).toString());
				printWriter.println(new StringBuilder().append("<VECSIZE> ").append(this.getNcoefs()).append("<NULLD>").append(parmKind).append("<DIAGC>").toString());
				printWriter.println("~r \"rtree_1\"");
				printWriter.println("<REGTREE> 1");
				printWriter.println(new StringBuilder().append("<TNODE> 1 ").append(this.getNgauss()).toString());
				printWriter.println(new StringBuilder().append("~h \"").append(nomHMM).append('"').toString());
				printWriter.println("<BEGINHMM>");
				printWriter.println("<NUMSTATES> 3");
				printWriter.println("<STATE> 2");
				printWriter.println(new StringBuilder().append("<NUMMIXES> ").append(this.getNgauss()).toString());
				for (int i = 1; i <= this.getNgauss(); i++)
				{
					printWriter.println(new StringBuilder().append("<MIXTURE> ").append(i).append(' ').append(this.getWeight(i - 1)).toString());
					printWriter.println("<RCLASS> 1");
					printWriter.println(new StringBuilder().append("<MEAN> ").append(this.getNcoefs()).toString());
					for (int j = 0; j < this.getNcoefs(); j++)
					{
						printWriter.print(new StringBuilder().append(this.getMean(i - 1, j)).append(" ").toString());
					}
					printWriter.println();
					printWriter.println(new StringBuilder().append("<VARIANCE> ").append(this.getNcoefs()).toString());
					for (int j = 0; j < this.getNcoefs(); j++)
					{
						printWriter.print(new StringBuilder().append(this.getVar(i - 1, j)).append(" ").toString());
					}
					printWriter.println();
				}
				printWriter.println("<TRANSP> 3");
				printWriter.println("0 1 0");
				printWriter.println("0 0.7 0.3");
				printWriter.println("0 0 0");
				printWriter.println("<ENDHMM>");
				printWriter.close();
			}
			catch (IOException ex)
			{
				Throwable.instancehelper_printStackTrace(ex);
			}
		}
		
		private void allocateWeights()
		{
			this.logMath = LogMath.getLogMath();
			this.weights = new float[this.ngauss];
			for (int i = 0; i < this.ngauss; i++)
			{
				this.setWeight(i, 1f / (float)this.ngauss);
			}
		}

		public GMMDiag(int ng, int nc)
		{
			this.ngauss = ng;
			this.ncoefs = nc;
			this.allocate();
		}
		
		private bool isDiff(float num, float num2)
		{
			return (double)java.lang.Math.abs(1f - num2 / num) > 0.01;
		}
		
		public GMMDiag()
		{
		}
		
		public virtual void save(string name)
		{
			try
			{
				PrintWriter printWriter = new PrintWriter(new FileWriter(name));
				printWriter.println(new StringBuilder().append(this.ngauss).append(" ").append(this.ncoefs).toString());
				for (int i = 0; i < this.ngauss; i++)
				{
					printWriter.println(new StringBuilder().append("gauss ").append(i).append(' ').append(this.getWeight(i)).toString());
					for (int j = 0; j < this.ncoefs; j++)
					{
						printWriter.print(new StringBuilder().append(this.means[i][j]).append(" ").toString());
					}
					printWriter.println();
					for (int j = 0; j < this.ncoefs; j++)
					{
						printWriter.print(new StringBuilder().append(this.getVar(i, j)).append(" ").toString());
					}
					printWriter.println();
				}
				printWriter.println(this.nT);
				printWriter.close();
			}
			catch (IOException ex)
			{
				Throwable.instancehelper_printStackTrace(ex);
			}
		}
		
		public virtual void load(string name)
		{
			try
			{
				BufferedReader bufferedReader = new BufferedReader(new FileReader(name));
				string text = bufferedReader.readLine();
				string[] array = java.lang.String.instancehelper_split(text, " ");
				this.ngauss = Integer.parseInt(array[0]);
				this.ncoefs = Integer.parseInt(array[1]);
				this.allocate();
				for (int i = 0; i < this.ngauss; i++)
				{
					text = bufferedReader.readLine();
					array = java.lang.String.instancehelper_split(text, " ");
					if (!java.lang.String.instancehelper_equals(array[0], "gauss") || Integer.parseInt(array[1]) != i)
					{
						java.lang.System.err.println(new StringBuilder().append("Error loading GMM ").append(text).append(' ').append(i).toString());
						java.lang.System.exit(1);
					}
					this.setWeight(i, Float.parseFloat(array[2]));
					text = bufferedReader.readLine();
					array = java.lang.String.instancehelper_split(text, " ");
					for (int j = 0; j < this.ncoefs; j++)
					{
						this.setMean(i, j, Float.parseFloat(array[j]));
					}
					text = bufferedReader.readLine();
					array = java.lang.String.instancehelper_split(text, " ");
					for (int j = 0; j < this.ncoefs; j++)
					{
						this.setVar(i, j, Float.parseFloat(array[j]));
					}
				}
				text = bufferedReader.readLine();
				if (text != null)
				{
					this.nT = Integer.parseInt(text);
				}
				bufferedReader.close();
				this.precomputeDistance();
			}
			catch (IOException ex)
			{
				Throwable.instancehelper_printStackTrace(ex);
			}
		}
		
		public virtual void saveHTK(string nomFich, string nomHMM)
		{
			this.saveHTK(nomFich, nomHMM, "<USER>");
		}
		
		public virtual PrintWriter saveHTKheader(string nomFich, string parmKind)
		{
			PrintWriter result;
			try
			{
				PrintWriter printWriter = new PrintWriter(new FileWriter(nomFich));
				printWriter.println("~o");
				printWriter.println("<HMMSETID> tree");
				printWriter.println(new StringBuilder().append("<STREAMINFO> 1 ").append(this.getNcoefs()).toString());
				printWriter.println(new StringBuilder().append("<VECSIZE> ").append(this.getNcoefs()).append("<NULLD>").append(parmKind).append("<DIAGC>").toString());
				printWriter.println("~r \"rtree_1\"");
				printWriter.println("<REGTREE> 1");
				printWriter.println(new StringBuilder().append("<TNODE> 1 ").append(this.getNgauss()).toString());
				result = printWriter;
			}
			catch (IOException ex)
			{
				Throwable.instancehelper_printStackTrace(ex);
				return null;
			}
			return result;
		}

		public virtual void saveHTKState(PrintWriter fout)
		{
			fout.println(new StringBuilder().append("<NUMMIXES> ").append(this.getNgauss()).toString());
			for (int i = 1; i <= this.getNgauss(); i++)
			{
				fout.println(new StringBuilder().append("<MIXTURE> ").append(i).append(' ').append(this.getWeight(i - 1)).toString());
				fout.println("<RCLASS> 1");
				fout.println(new StringBuilder().append("<MEAN> ").append(this.getNcoefs()).toString());
				for (int j = 0; j < this.getNcoefs(); j++)
				{
					fout.print(new StringBuilder().append(this.getMean(i - 1, j)).append(" ").toString());
				}
				fout.println();
				fout.println(new StringBuilder().append("<VARIANCE> ").append(this.getNcoefs()).toString());
				for (int j = 0; j < this.getNcoefs(); j++)
				{
					fout.print(new StringBuilder().append(this.getVar(i - 1, j)).append(" ").toString());
				}
				fout.println();
			}
		}
		
		public virtual void saveHTKtailer(int nstates, PrintWriter fout)
		{
			fout.println(new StringBuilder().append("<TRANSP> ").append(nstates).toString());
			for (int i = 0; i < nstates; i++)
			{
				fout.print("0 ");
			}
			fout.println();
			for (int i = 1; i < nstates - 1; i++)
			{
				for (int j = 0; j < i; j++)
				{
					fout.print("0 ");
				}
				fout.print("0.5 0.5");
				for (int j = i + 3; j < nstates; j++)
				{
					fout.print("0 ");
				}
			}
			fout.println();
			fout.println("0 0 0");
			fout.println("<ENDHMM>");
		}
		
		public virtual void loadHTK(string nom)
		{
			try
			{
				BufferedReader bufferedReader = new BufferedReader(new FileReader(nom));
				this.ngauss = 0;
				this.ncoefs = 0;
				CharSequence charSequence;
				for (;;)
				{
					string text = bufferedReader.readLine();
					if (text == null)
					{
						break;
					}
					string text2 = text;
					object _ref = "<MEAN>";
					charSequence = CharSequence.Cast(_ref);
					if (java.lang.String.instancehelper_contains(text2, charSequence))
					{
						this.ngauss++;
						if (this.ncoefs == 0)
						{
							StringTokenizer stringTokenizer = new StringTokenizer(text);
							stringTokenizer.nextToken();
							this.ncoefs = Integer.parseInt(stringTokenizer.nextToken());
						}
					}
				}
				bufferedReader.close();
				this.allocate();
				bufferedReader = new BufferedReader(new FileReader(nom));
				int num = 0;
				for (;;)
				{
					string text = bufferedReader.readLine();
					if (text == null)
					{
						break;
					}
					string text3 = text;
					object _ref = "<MEAN>";
					charSequence =CharSequence.Cast( _ref);
					if (java.lang.String.instancehelper_contains(text3, charSequence))
					{
						text = bufferedReader.readLine();
						StringTokenizer stringTokenizer = new StringTokenizer(text);
						int num2 = 0;
						while (stringTokenizer.hasMoreTokens())
						{
							string text4 = stringTokenizer.nextToken();
							this.setMean(num, num2, Float.parseFloat(text4));
							num2++;
						}
						text = bufferedReader.readLine();
						string text5 = text;
						_ref = "<VARIANCE>";
						charSequence = CharSequence.Cast(_ref);
						if (!java.lang.String.instancehelper_contains(text5, charSequence))
						{
							goto Block_8;
						}
						text = bufferedReader.readLine();
						stringTokenizer = new StringTokenizer(text);
						num2 = 0;
						while (stringTokenizer.hasMoreTokens())
						{
							string text4 = stringTokenizer.nextToken();
							this.setVar(num, num2, Float.parseFloat(text4));
							num2++;
						}
						num++;
					}
				}
				bufferedReader.close();
				this.precomputeDistance();
				goto IL_19B;
				Block_8:
				bufferedReader.close();
				
				throw new IOException();
			}
			catch (IOException ex)
			{
				Throwable.instancehelper_printStackTrace(ex);
			}
			IL_19B:
			return;
		}
		
		public virtual void loadScaleKMeans(string nom)
		{
			int num = 0;
			try
			{
				BufferedReader bufferedReader = new BufferedReader(new FileReader(nom));
				while (bufferedReader.readLine() != null)
				{
					num++;
				}
				this.ngauss = num / 2;
				bufferedReader.close();
				bufferedReader = new BufferedReader(new FileReader(nom));
				string text = bufferedReader.readLine();
				string[] array = java.lang.String.instancehelper_split(text, " ");
				this.ncoefs = array.Length - 1;
				bufferedReader.close();
				bufferedReader = new BufferedReader(new FileReader(nom));
				this.allocate();
				this.nT = 0;
				for (int i = 0; i < this.ngauss; i++)
				{
					text = bufferedReader.readLine();
					array = java.lang.String.instancehelper_split(text, " ");
					this.weights[i] = Float.parseFloat(array[0]);
					this.nT = ByteCodeHelper.f2i((float)this.nT + this.weights[i]);
					for (int j = 0; j < this.ncoefs; j++)
					{
						this.setMean(i, j, Float.parseFloat(array[j + 1]));
					}
					text = bufferedReader.readLine();
					array = java.lang.String.instancehelper_split(text, " ");
					for (int j = 0; j < this.ncoefs; j++)
					{
						this.setVar(i, j, Float.parseFloat(array[j]));
					}
				}
				for (int i = 0; i < this.ngauss; i++)
				{
					this.setWeight(i, this.weights[i] / (float)this.nT);
				}
				bufferedReader.close();
				this.precomputeDistance();
			}
			catch (IOException ex)
			{
				Throwable.instancehelper_printStackTrace(ex);
			}
		}
		
		public virtual void computeLogLikes(float[] data)
		{
			for (int i = 0; i < this.ngauss; i++)
			{
				float num = 0f;
				for (int j = 0; j < data.Length; j++)
				{
					float num2 = data[j] - this.means[i][j];
					num += num2 * num2 * this.covar[i][j];
				}
				num -= this.logPreComputedGaussianFactor[i];
				if (Float.isNaN(num))
				{
					java.lang.System.err.println(new StringBuilder().append("gs2 is Nan, converting to 0 debug ").append(i).append(' ').append(this.logPreComputedGaussianFactor[i]).append(' ').append(this.means[i][0]).append(' ').append(this.covar[i][0]).toString());
					num = float.MinValue;
				}
				if (num < -3.40282347E+38f)
				{
					num = float.MinValue;
				}
				this.loglikes[i] = this.weights[i] + num;
			}
		}
		
		public virtual float getLogLike()
		{
			float num = this.loglikes[0];
			for (int i = 1; i < this.ngauss; i++)
			{
				num = this.logMath.addAsLinear(num, this.loglikes[i]);
			}
			return num;
		}

		public virtual int getWinningGauss()
		{
			int num = 0;
			for (int i = 1; i < this.ngauss; i++)
			{
				if (this.loglikes[i] > this.loglikes[num])
				{
					num = i;
				}
			}
			return num;
		}
		
		public virtual GMMDiag getMarginal(bool[] mask)
		{
			int num = 0;
			int num2 = mask.Length;
			for (int i = 0; i < num2; i++)
			{
				int j = mask[i] ? 1 : 0;
				if (j != 0)
				{
					num++;
				}
			}
			GMMDiag gmmdiag = new GMMDiag(this.getNgauss(), num);
			num2 = 0;
			for (int i = 0; i < this.ncoefs; i++)
			{
				if (mask[i])
				{
					for (int j = 0; j < this.ngauss; j++)
					{
						gmmdiag.setMean(j, num2, this.getMean(j, i));
						gmmdiag.setVar(j, num2, this.getVar(j, i));
					}
					num2++;
				}
			}
			for (int i = 0; i < this.ngauss; i++)
			{
				gmmdiag.setWeight(i, this.getWeight(i));
			}
			gmmdiag.precomputeDistance();
			return gmmdiag;
		}
		
		public virtual GMMDiag merge(GMMDiag g, float w1)
		{
			GMMDiag gmmdiag = new GMMDiag(this.getNgauss() + g.getNgauss(), this.getNcoefs());
			for (int i = 0; i < this.getNgauss(); i++)
			{
				ByteCodeHelper.arraycopy_primitive_4(this.means[i], 0, gmmdiag.means[i], 0, this.getNcoefs());
				ByteCodeHelper.arraycopy_primitive_4(this.covar[i], 0, gmmdiag.covar[i], 0, this.getNcoefs());
				gmmdiag.setWeight(i, this.getWeight(i) * w1);
			}
			for (int i = 0; i < g.getNgauss(); i++)
			{
				ByteCodeHelper.arraycopy_primitive_4(g.means[i], 0, gmmdiag.means[this.ngauss + i], 0, this.getNcoefs());
				ByteCodeHelper.arraycopy_primitive_4(g.covar[i], 0, gmmdiag.covar[this.ngauss + i], 0, this.getNcoefs());
				gmmdiag.setWeight(this.ngauss + i, g.getWeight(i) * (1f - w1));
			}
			gmmdiag.precomputeDistance();
			return gmmdiag;
		}
		
		public virtual GMMDiag getGauss(int i)
		{
			GMMDiag gmmdiag = new GMMDiag(1, this.getNcoefs());
			ByteCodeHelper.arraycopy_primitive_4(this.means[i], 0, gmmdiag.means[0], 0, this.getNcoefs());
			ByteCodeHelper.arraycopy_primitive_4(this.covar[i], 0, gmmdiag.covar[0], 0, this.getNcoefs());
			gmmdiag.setWeight(0, 1f);
			gmmdiag.precomputeDistance();
			return gmmdiag;
		}

		public virtual void setNom(string s)
		{
			this.nom = s;
		}
		
		public virtual bool isEqual(GMMDiag g)
		{
			if (this.getNgauss() != g.getNgauss())
			{
				return false;
			}
			if (this.getNgauss() != g.getNcoefs())
			{
				return false;
			}
			for (int i = 0; i < this.getNgauss(); i++)
			{
				if (this.isDiff(this.getWeight(i), g.getWeight(i)))
				{
					return false;
				}
				for (int j = 0; j < this.getNcoefs(); j++)
				{
					if (this.isDiff(this.getMean(i, j), g.getMean(i, j)))
					{
						return false;
					}
					if (this.isDiff(this.getVar(i, j), g.getVar(i, j)))
					{
						return false;
					}
				}
			}
			return true;
		}
		
		public override string toString()
		{
			StringBuilder stringBuilder = new StringBuilder();
			for (int i = 0; i < this.getNgauss(); i++)
			{
				stringBuilder.append(this.getMean(i, 0)).append(' ').append(this.getVar(i, 0)).append('\n');
			}
			return stringBuilder.toString();
		}

		public int nT;

		public string nom;

		public LogMath logMath;

		private int ncoefs;

		private int ngauss;

		protected internal float[] weights;

		protected internal float[][] means;

		protected internal float[][] covar;

		private float[] logPreComputedGaussianFactor;

		protected internal float[] loglikes;

		private const float distFloor = -3.40282347E+38f;
	}
}
