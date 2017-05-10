using System;

using edu.cmu.sphinx.util;
using edu.cmu.sphinx.util.props;
using IKVM.Attributes;
using ikvm.@internal;
using java.io;
using java.lang;
using java.util;
using java.util.logging;

namespace edu.cmu.sphinx.linguist.acoustic.tiedstate.trainer
{
	[Implements(new string[]
	{
		"edu.cmu.sphinx.linguist.acoustic.tiedstate.Saver"
	})]
	public class Sphinx3Saver : java.lang.Object, Saver, Configurable
	{
		
		public static void __<clinit>()
		{
		}

		[Throws(new string[]
		{
			"java.io.FileNotFoundException",
			"java.io.IOException"
		})]
		
		[LineNumberTable(new byte[]
		{
			159,
			77,
			130,
			102,
			130,
			112,
			140,
			113,
			146,
			144,
			110,
			110,
			142,
			105,
			105,
			137,
			99,
			105,
			105,
			107,
			108,
			240,
			61,
			232,
			70,
			119,
			127,
			3,
			233,
			72,
			108,
			105,
			105,
			147,
			143,
			233,
			59,
			40,
			40,
			235,
			75,
			104,
			191,
			9,
			136,
			102
		})]
		
		private void saveDensityFileBinary(Pool pool, string text, bool append)
		{
			Properties properties = new Properties();
			int val = 0;
			this.logger.info("Saving density file to: ");
			this.logger.info(text);
			properties.setProperty("version", "1.0");
			properties.setProperty("chksum0", this.checksum);
			DataOutputStream dataOutputStream = this.writeS3BinaryHeader(this.location, text, properties, append);
			int feature = pool.getFeature(Pool.Feature.__NUM_SENONES, -1);
			int feature2 = pool.getFeature(Pool.Feature.__NUM_STREAMS, -1);
			int feature3 = pool.getFeature(Pool.Feature.__NUM_GAUSSIANS_PER_STATE, -1);
			this.writeInt(dataOutputStream, feature);
			this.writeInt(dataOutputStream, feature2);
			this.writeInt(dataOutputStream, feature3);
			int num = 0;
			int[] array = new int[feature2];
			for (int i = 0; i < feature2; i++)
			{
				array[i] = this.vectorLength;
				this.writeInt(dataOutputStream, array[i]);
				num += feature3 * feature * array[i];
			}
			if (!Sphinx3Saver.assertionsDisabled && feature2 != 1)
			{
				
				throw new AssertionError();
			}
			if (!Sphinx3Saver.assertionsDisabled && num != feature3 * feature * this.vectorLength)
			{
				
				throw new AssertionError();
			}
			this.writeInt(dataOutputStream, num);
			for (int i = 0; i < feature; i++)
			{
				for (int j = 0; j < feature2; j++)
				{
					for (int k = 0; k < feature3; k++)
					{
						int id = i * feature2 * feature3 + j * feature3 + k;
						float[] data = (float[])pool.get(id);
						this.writeFloatArray(dataOutputStream, data);
					}
				}
			}
			if (this.doCheckSum && !Sphinx3Saver.assertionsDisabled)
			{
				int num2 = 0;
				bool flag = num2 != 0;
				this.doCheckSum = (num2 != 0);
				if (!flag)
				{
					object obj = "Checksum not supported";
					
					throw new AssertionError(obj);
				}
			}
			this.writeInt(dataOutputStream, val);
			dataOutputStream.close();
		}

		[Throws(new string[]
		{
			"java.io.FileNotFoundException",
			"java.io.IOException"
		})]
		[LineNumberTable(new byte[]
		{
			158,
			246,
			162,
			112,
			140,
			134,
			113,
			104,
			178,
			144,
			103,
			104,
			136,
			104,
			105,
			137,
			151,
			105,
			137,
			107,
			108,
			105,
			105,
			105,
			49,
			136,
			111,
			233,
			58,
			43,
			235,
			74,
			104,
			223,
			9,
			102
		})]
		
		private void saveMixtureWeightsBinary(GaussianWeights gaussianWeights, string text, bool append)
		{
			this.logger.info("Saving mixture weights to: ");
			this.logger.info(text);
			Properties properties = new Properties();
			properties.setProperty("version", "1.0");
			if (this.doCheckSum)
			{
				properties.setProperty("chksum0", this.checksum);
			}
			DataOutputStream dataOutputStream = this.writeS3BinaryHeader(this.location, text, properties, append);
			int statesNum = gaussianWeights.getStatesNum();
			int streamsNum = gaussianWeights.getStreamsNum();
			int gauPerState = gaussianWeights.getGauPerState();
			this.writeInt(dataOutputStream, statesNum);
			this.writeInt(dataOutputStream, streamsNum);
			this.writeInt(dataOutputStream, gauPerState);
			if (!Sphinx3Saver.assertionsDisabled && streamsNum != 1)
			{
				
				throw new AssertionError();
			}
			int val = gauPerState * statesNum * streamsNum;
			this.writeInt(dataOutputStream, val);
			for (int i = 0; i < statesNum; i++)
			{
				for (int j = 0; j < streamsNum; j++)
				{
					float[] array = new float[gauPerState];
					float[] array2 = new float[gauPerState];
					for (int k = 0; k < gauPerState; k++)
					{
						array2[k] = gaussianWeights.get(i, j, k);
					}
					this.logMath.logToLinear(array2, array);
					this.writeFloatArray(dataOutputStream, array);
				}
			}
			if (this.doCheckSum && !Sphinx3Saver.assertionsDisabled)
			{
				int num = 0;
				bool flag = num != 0;
				this.doCheckSum = (num != 0);
				if (!flag)
				{
					object obj = "Checksum not supported";
					
					throw new AssertionError(obj);
				}
			}
			dataOutputStream.close();
		}

		[Throws(new string[]
		{
			"java.io.IOException"
		})]
		
		[LineNumberTable(new byte[]
		{
			158,
			215,
			66,
			112,
			108,
			134,
			113,
			104,
			178,
			144,
			103,
			118,
			136,
			110,
			103,
			134,
			105,
			105,
			105,
			137,
			107,
			175,
			105,
			138,
			105,
			62,
			200,
			105,
			103,
			106,
			111,
			233,
			60,
			232,
			53,
			235,
			82,
			104,
			191,
			9,
			102
		})]
		
		protected internal virtual void saveTransitionMatricesBinary(Pool pool, string path, bool append)
		{
			this.logger.info("Saving transition matrices to: ");
			this.logger.info(path);
			Properties properties = new Properties();
			properties.setProperty("version", "1.0");
			if (this.doCheckSum)
			{
				properties.setProperty("chksum0", this.checksum);
			}
			DataOutputStream dataOutputStream = this.writeS3BinaryHeader(this.location, path, properties, append);
			int num = pool.size();
			if (!Sphinx3Saver.assertionsDisabled && num <= 0)
			{
				
				throw new AssertionError();
			}
			this.writeInt(dataOutputStream, num);
			float[][] array = (float[][])pool.get(0);
			int num2 = array[0].Length;
			int num3 = num2 - 1;
			this.writeInt(dataOutputStream, num3);
			this.writeInt(dataOutputStream, num2);
			int val = num2 * num3 * num;
			this.writeInt(dataOutputStream, val);
			for (int i = 0; i < num; i++)
			{
				array = (float[][])pool.get(i);
				float[] array2 = array[num2 - 1];
				float[] array3 = new float[array2.Length];
				for (int j = 0; j < num2; j++)
				{
					if (!Sphinx3Saver.assertionsDisabled && array3[j] != 0f)
					{
						
						throw new AssertionError();
					}
				}
				for (int j = 0; j < num3; j++)
				{
					array2 = array[j];
					array3 = new float[array2.Length];
					this.logMath.logToLinear(array2, array3);
					this.writeFloatArray(dataOutputStream, array3);
				}
			}
			if (this.doCheckSum && !Sphinx3Saver.assertionsDisabled)
			{
				int num4 = 0;
				bool flag = num4 != 0;
				this.doCheckSum = (num4 != 0);
				if (!flag)
				{
					object obj = "Checksum not supported";
					
					throw new AssertionError(obj);
				}
			}
			dataOutputStream.close();
		}

		[Throws(new string[]
		{
			"java.io.FileNotFoundException",
			"java.io.IOException"
		})]
		
		[LineNumberTable(new byte[]
		{
			159,
			89,
			98,
			112,
			140,
			142,
			99,
			159,
			17,
			136,
			107,
			109,
			127,
			1,
			110,
			127,
			2,
			110,
			136,
			107,
			127,
			2,
			107,
			140,
			159,
			2,
			106,
			111,
			109,
			63,
			5,
			232,
			69,
			230,
			53,
			235,
			61,
			235,
			81,
			102
		})]
		
		private void saveDensityFileAscii(Pool pool, string text, bool append)
		{
			this.logger.info("Saving density file to: ");
			this.logger.info(text);
			OutputStream outputStream = StreamFactory.getOutputStream(this.location, text, append);
			if (outputStream == null)
			{
				string text2 = new StringBuilder().append("Error trying to write file ").append(this.location).append(text).toString();
				
				throw new IOException(text2);
			}
			PrintWriter printWriter = new PrintWriter(outputStream, true);
			printWriter.print("param ");
			int feature = pool.getFeature(Pool.Feature.__NUM_SENONES, -1);
			printWriter.print(new StringBuilder().append(feature).append(" ").toString());
			int feature2 = pool.getFeature(Pool.Feature.__NUM_STREAMS, -1);
			printWriter.print(new StringBuilder().append(feature2).append(" ").toString());
			int feature3 = pool.getFeature(Pool.Feature.__NUM_GAUSSIANS_PER_STATE, -1);
			printWriter.println(feature3);
			for (int i = 0; i < feature; i++)
			{
				printWriter.println(new StringBuilder().append("mgau ").append(i).toString());
				printWriter.println("feat 0");
				for (int j = 0; j < feature3; j++)
				{
					printWriter.print(new StringBuilder().append("density \t").append(j).toString());
					int id = i * feature3 + j;
					float[] array = (float[])pool.get(id);
					for (int k = 0; k < this.vectorLength; k++)
					{
						printWriter.print(new StringBuilder().append(" ").append(array[k]).toString());
					}
					printWriter.println();
				}
			}
			outputStream.close();
		}

		[Throws(new string[]
		{
			"java.io.FileNotFoundException",
			"java.io.IOException"
		})]
		[LineNumberTable(new byte[]
		{
			159,
			2,
			130,
			112,
			140,
			110,
			99,
			159,
			17,
			136,
			107,
			103,
			127,
			1,
			104,
			127,
			2,
			104,
			136,
			107,
			108,
			127,
			29,
			105,
			105,
			105,
			49,
			136,
			111,
			103,
			105,
			43,
			136,
			104,
			107,
			105,
			63,
			5,
			136,
			230,
			50,
			43,
			235,
			82,
			102
		})]
		
		private void saveMixtureWeightsAscii(GaussianWeights gaussianWeights, string text, bool append)
		{
			this.logger.info("Saving mixture weights to: ");
			this.logger.info(text);
			OutputStream outputStream = StreamFactory.getOutputStream(this.location, text, append);
			if (outputStream == null)
			{
				string text2 = new StringBuilder().append("Error trying to write file ").append(this.location).append(text).toString();
				
				throw new IOException(text2);
			}
			PrintWriter printWriter = new PrintWriter(outputStream, true);
			printWriter.print("mixw ");
			int statesNum = gaussianWeights.getStatesNum();
			printWriter.print(new StringBuilder().append(statesNum).append(" ").toString());
			int streamsNum = gaussianWeights.getStreamsNum();
			printWriter.print(new StringBuilder().append(streamsNum).append(" ").toString());
			int gauPerState = gaussianWeights.getGauPerState();
			printWriter.println(gauPerState);
			for (int i = 0; i < statesNum; i++)
			{
				for (int j = 0; j < streamsNum; j++)
				{
					printWriter.print(new StringBuilder().append("mixw [").append(i).append(" ").append(j).append("] ").toString());
					float[] array = new float[gauPerState];
					float[] array2 = new float[gauPerState];
					for (int k = 0; k < gauPerState; k++)
					{
						array2[k] = gaussianWeights.get(i, j, k);
					}
					this.logMath.logToLinear(array2, array);
					float num = 0f;
					for (int l = 0; l < gauPerState; l++)
					{
						num += array[l];
					}
					printWriter.println(num);
					printWriter.print("\n\t");
					for (int l = 0; l < gauPerState; l++)
					{
						printWriter.print(new StringBuilder().append(" ").append(array[l]).toString());
					}
					printWriter.println();
				}
			}
			outputStream.close();
		}

		[Throws(new string[]
		{
			"java.io.FileNotFoundException",
			"java.io.IOException"
		})]
		
		[LineNumberTable(new byte[]
		{
			158,
			232,
			98,
			110,
			99,
			159,
			17,
			136,
			112,
			108,
			135,
			118,
			110,
			135,
			159,
			15,
			107,
			159,
			9,
			111,
			108,
			236,
			69,
			107,
			104,
			102,
			139,
			110,
			111,
			38,
			199,
			154,
			104,
			136,
			235,
			69,
			114,
			255,
			47,
			38,
			43,
			235,
			60,
			235,
			101,
			102
		})]
		
		protected internal virtual void saveTransitionMatricesAscii(Pool pool, string path, bool append)
		{
			OutputStream outputStream = StreamFactory.getOutputStream(this.location, path, append);
			if (outputStream == null)
			{
				string text = new StringBuilder().append("Error trying to write file ").append(this.location).append(path).toString();
				
				throw new IOException(text);
			}
			PrintWriter printWriter = new PrintWriter(outputStream, true);
			this.logger.info("Saving transition matrices to: ");
			this.logger.info(path);
			int num = pool.size();
			if (!Sphinx3Saver.assertionsDisabled && num <= 0)
			{
				
				throw new AssertionError();
			}
			float[][] array = (float[][])pool.get(0);
			int num2 = array[0].Length;
			printWriter.println(new StringBuilder().append("tmat ").append(num).append(' ').append(num2).toString());
			for (int i = 0; i < num; i++)
			{
				printWriter.println(new StringBuilder().append("tmat [").append(i).append(']').toString());
				array = (float[][])pool.get(i);
				for (int j = 0; j < num2; j++)
				{
					for (int k = 0; k < num2; k++)
					{
						if (j < num2 - 1)
						{
							if (this.sparseForm)
							{
								if (k < j)
								{
									printWriter.print("\t");
								}
								if (k == j || k == j + 1)
								{
									printWriter.print((float)this.logMath.logToLinear(array[j][k]));
								}
							}
							else
							{
								printWriter.print((float)this.logMath.logToLinear(array[j][k]));
							}
							if (num2 - 1 == k)
							{
								printWriter.println();
							}
							else
							{
								printWriter.print(" ");
							}
						}
						if (this.logger.isLoggable(Level.FINE))
						{
							this.logger.fine(new StringBuilder().append("tmat j ").append(j).append(" k ").append(k).append(" tm ").append(array[j][k]).toString());
						}
					}
				}
			}
			outputStream.close();
		}

		[Throws(new string[]
		{
			"java.io.FileNotFoundException",
			"java.io.IOException"
		})]
		[LineNumberTable(new byte[]
		{
			161,
			51,
			112,
			140,
			99,
			191,
			17,
			232,
			73,
			98,
			98,
			98,
			99,
			127,
			5,
			110,
			110,
			134,
			100,
			138,
			98,
			107,
			127,
			1,
			127,
			1,
			127,
			2,
			109,
			127,
			2,
			127,
			1,
			99,
			127,
			2,
			159,
			2,
			107,
			107,
			171,
			127,
			8,
			105,
			105,
			162,
			137,
			105,
			126,
			103,
			127,
			2,
			103,
			126,
			110,
			126,
			119,
			126,
			116,
			120,
			159,
			2,
			105,
			105,
			124,
			111,
			124,
			255,
			2,
			61,
			235,
			69,
			139,
			114,
			191,
			7,
			197,
			127,
			8,
			105,
			105,
			162,
			105,
			110,
			105,
			105,
			158,
			105,
			126,
			107,
			127,
			2,
			107,
			126,
			110,
			126,
			119,
			127,
			1,
			126,
			116,
			120,
			159,
			2,
			105,
			105,
			124,
			111,
			125,
			255,
			2,
			61,
			235,
			69,
			139,
			114,
			191,
			7,
			133,
			102
		})]
		
		private void saveHMMPool(bool flag, OutputStream outputStream, string text)
		{
			this.logger.info("Saving HMM file to: ");
			this.logger.info(text);
			if (outputStream == null)
			{
				string text2 = new StringBuilder().append("Error trying to write file ").append(this.location).append(text).toString();
				
				throw new IOException(text2);
			}
			PrintWriter printWriter = new PrintWriter(outputStream, true);
			int num = 0;
			int num2 = 0;
			int num3 = 0;
			int num4 = 0;
			Iterator iterator = this.hmmManager.iterator();
			while (iterator.hasNext())
			{
				HMM hmm = (HMM)iterator.next();
				num4 += hmm.getOrder() + 1;
				if (((SenoneHMM)hmm).isContextDependent())
				{
					num2++;
				}
				else
				{
					num++;
					num3 += hmm.getOrder();
				}
			}
			printWriter.println("0.3");
			printWriter.println(new StringBuilder().append(num).append(" n_base").toString());
			printWriter.println(new StringBuilder().append(num2).append(" n_tri").toString());
			printWriter.println(new StringBuilder().append(num4).append(" n_state_map").toString());
			int statesNum = this.mixtureWeights.getStatesNum();
			printWriter.println(new StringBuilder().append(statesNum).append(" n_tied_state").toString());
			printWriter.println(new StringBuilder().append(num3).append(" n_tied_ci_state").toString());
			int num5 = num;
			if (!Sphinx3Saver.assertionsDisabled && num5 != this.matrixPool.size())
			{
				
				throw new AssertionError();
			}
			printWriter.println(new StringBuilder().append(num5).append(" n_tied_tmat").toString());
			printWriter.println("#");
			printWriter.println("# Columns definitions");
			printWriter.println("#base lft  rt p attrib tmat      ... state id's ...");
			Iterator iterator2 = this.hmmManager.iterator();
			while (iterator2.hasNext())
			{
				HMM hmm2 = (HMM)iterator2.next();
				SenoneHMM senoneHMM = (SenoneHMM)hmm2;
				if (!senoneHMM.isContextDependent())
				{
					Unit unit = senoneHMM.getUnit();
					string name = unit.getName();
					printWriter.print(new StringBuilder().append(name).append('\t').toString());
					string text3 = "-";
					printWriter.print(new StringBuilder().append(text3).append("   ").toString());
					string text4 = "-";
					printWriter.print(new StringBuilder().append(text4).append(' ').toString());
					string text5 = senoneHMM.getPosition().toString();
					printWriter.print(new StringBuilder().append(text5).append('\t').toString());
					string text6 = (!unit.isFiller()) ? "n/a" : "filler";
					printWriter.print(new StringBuilder().append(text6).append('\t').toString());
					int num6 = this.matrixPool.indexOf(senoneHMM.getTransitionMatrix());
					if (!Sphinx3Saver.assertionsDisabled && num6 >= num5)
					{
						
						throw new AssertionError();
					}
					printWriter.print(new StringBuilder().append(num6).append("\t").toString());
					SenoneSequence senoneSequence = senoneHMM.getSenoneSequence();
					Senone[] senones = senoneSequence.getSenones();
					Senone[] array = senones;
					int num7 = array.Length;
					for (int i = 0; i < num7; i++)
					{
						Senone @object = array[i];
						int num8 = this.senonePool.indexOf(@object);
						if (!Sphinx3Saver.assertionsDisabled && (num8 < 0 || num8 >= num3))
						{
							
							throw new AssertionError();
						}
						printWriter.print(new StringBuilder().append(num8).append("\t").toString());
					}
					printWriter.println("N");
					if (this.logger.isLoggable(Level.FINE))
					{
						this.logger.fine(new StringBuilder().append("Saved ").append(unit).toString());
					}
				}
			}
			iterator2 = this.hmmManager.iterator();
			while (iterator2.hasNext())
			{
				HMM hmm2 = (HMM)iterator2.next();
				SenoneHMM senoneHMM = (SenoneHMM)hmm2;
				if (senoneHMM.isContextDependent())
				{
					Unit unit = senoneHMM.getUnit();
					LeftRightContext leftRightContext = (LeftRightContext)unit.getContext();
					Unit[] leftContext = leftRightContext.getLeftContext();
					Unit[] rightContext = leftRightContext.getRightContext();
					if (!Sphinx3Saver.assertionsDisabled && (leftContext.Length != 1 || rightContext.Length != 1))
					{
						
						throw new AssertionError();
					}
					string text5 = unit.getName();
					printWriter.print(new StringBuilder().append(text5).append('\t').toString());
					string text6 = leftContext[0].getName();
					printWriter.print(new StringBuilder().append(text6).append("   ").toString());
					string name2 = rightContext[0].getName();
					printWriter.print(new StringBuilder().append(name2).append(' ').toString());
					string text7 = senoneHMM.getPosition().toString();
					printWriter.print(new StringBuilder().append(text7).append('\t').toString());
					string text8 = (!unit.isFiller()) ? "n/a" : "filler";
					if (!Sphinx3Saver.assertionsDisabled && !java.lang.String.instancehelper_equals(text8, "n/a"))
					{
						
						throw new AssertionError();
					}
					printWriter.print(new StringBuilder().append(text8).append('\t').toString());
					int num9 = this.matrixPool.indexOf(senoneHMM.getTransitionMatrix());
					if (!Sphinx3Saver.assertionsDisabled && num9 >= num5)
					{
						
						throw new AssertionError();
					}
					printWriter.print(new StringBuilder().append(num9).append("\t").toString());
					SenoneSequence senoneSequence2 = senoneHMM.getSenoneSequence();
					Senone[] senones2 = senoneSequence2.getSenones();
					Senone[] array2 = senones2;
					int num8 = array2.Length;
					for (int j = 0; j < num8; j++)
					{
						Senone object2 = array2[j];
						int num10 = this.senonePool.indexOf(object2);
						if (!Sphinx3Saver.assertionsDisabled && (num10 < 0 || num10 >= statesNum))
						{
							
							throw new AssertionError();
						}
						printWriter.print(new StringBuilder().append(num10).append("\t").toString());
					}
					printWriter.println("N");
					if (this.logger.isLoggable(Level.FINE))
					{
						this.logger.fine(new StringBuilder().append("Saved ").append(unit).toString());
					}
				}
			}
			outputStream.close();
		}

		[Throws(new string[]
		{
			"java.io.IOException"
		})]
		[LineNumberTable(new byte[]
		{
			159,
			59,
			67,
			105,
			104,
			151,
			140,
			140,
			111,
			109,
			106,
			127,
			14,
			98,
			140,
			140
		})]
		
		protected internal virtual DataOutputStream writeS3BinaryHeader(string location, string path, Properties props, bool append)
		{
			OutputStream outputStream = StreamFactory.getOutputStream(location, path, append);
			if (this.doCheckSum && !Sphinx3Saver.assertionsDisabled)
			{
				object obj = "Checksum not supported";
				
				throw new AssertionError(obj);
			}
			DataOutputStream dataOutputStream = new DataOutputStream(new BufferedOutputStream(outputStream));
			this.writeWord(dataOutputStream, "s3\n");
			Enumeration enumeration = props.keys();
			while (enumeration.hasMoreElements())
			{
				string text = (string)enumeration.nextElement();
				string property = props.getProperty(text);
				this.writeWord(dataOutputStream, new StringBuilder().append(text).append(' ').append(property).append('\n').toString());
			}
			this.writeWord(dataOutputStream, "endhdr\n");
			this.writeInt(dataOutputStream, 287454020);
			return dataOutputStream;
		}

		[Throws(new string[]
		{
			"java.io.IOException"
		})]
		[LineNumberTable(new byte[]
		{
			161,
			2,
			104,
			142,
			135
		})]
		
		protected internal virtual void writeInt(DataOutputStream dos, int val)
		{
			if (this.swap)
			{
				dos.writeInt(Utilities.swapInteger(val));
			}
			else
			{
				dos.writeInt(val);
			}
		}

		[Throws(new string[]
		{
			"java.io.IOException"
		})]
		[LineNumberTable(new byte[]
		{
			161,
			35,
			111,
			40,
			166
		})]
		
		protected internal virtual void writeFloatArray(DataOutputStream dos, float[] data)
		{
			int num = data.Length;
			for (int i = 0; i < num; i++)
			{
				float val = data[i];
				this.writeFloat(dos, val);
			}
		}

		[Throws(new string[]
		{
			"java.io.IOException"
		})]
		[LineNumberTable(new byte[]
		{
			160,
			246,
			103
		})]
		
		internal virtual void writeWord(DataOutputStream dataOutputStream, string text)
		{
			dataOutputStream.writeBytes(text);
		}

		[Throws(new string[]
		{
			"java.io.IOException"
		})]
		[LineNumberTable(new byte[]
		{
			161,
			18,
			104,
			143,
			136
		})]
		
		protected internal virtual void writeFloat(DataOutputStream dos, float val)
		{
			if (this.swap)
			{
				dos.writeFloat(Utilities.swapFloat(val));
			}
			else
			{
				dos.writeFloat(val);
			}
		}

		
		
		public Sphinx3Saver()
		{
		}

		[Throws(new string[]
		{
			"edu.cmu.sphinx.util.props.PropertyException"
		})]
		[LineNumberTable(new byte[]
		{
			58,
			140,
			113,
			145,
			118,
			118,
			171,
			145,
			113,
			108,
			108,
			108,
			108,
			108,
			108,
			171,
			107,
			127,
			5,
			103
		})]
		
		public virtual void newProperties(PropertySheet ps)
		{
			this.logger = ps.getLogger();
			this.location = ps.getString("saveLocation");
			this.dataDir = ps.getString("dataLocation");
			this.sparseForm = ps.getBoolean("sparseForm").booleanValue();
			this.useCDUnits = ps.getBoolean("useCDUnits").booleanValue();
			this.logMath = LogMath.getLogMath();
			this.vectorLength = ps.getInt("vectorLength");
			Loader loader = (Loader)ps.getComponent("loader");
			this.hmmManager = loader.getHMMManager();
			this.meansPool = loader.getMeansPool();
			this.variancePool = loader.getVariancePool();
			this.mixtureWeights = loader.getMixtureWeights();
			this.matrixPool = loader.getTransitionMatrixPool();
			this.senonePool = loader.getSenonePool();
			this.contextIndependentUnits = new LinkedHashMap();
			this.checksum = "no";
			this.doCheckSum = (this.checksum != null && java.lang.String.instancehelper_equals(this.checksum, "yes"));
			this.swap = false;
		}

		protected internal virtual string getCheckSum()
		{
			return this.checksum;
		}

		protected internal virtual bool getDoCheckSum()
		{
			return this.doCheckSum;
		}

		protected internal virtual string getLocation()
		{
			return this.location;
		}

		[Throws(new string[]
		{
			"java.io.IOException"
		})]
		[LineNumberTable(new byte[]
		{
			119,
			127,
			6,
			127,
			11,
			127,
			6,
			255,
			11,
			69,
			139,
			159,
			13,
			127,
			13,
			127,
			13,
			191,
			18,
			127,
			13,
			127,
			13,
			127,
			13,
			255,
			13,
			69,
			127,
			39
		})]
		
		public virtual void save(string modelName, bool b)
		{
			this.logger.info(new StringBuilder().append("Saving acoustic model: ").append(modelName).toString());
			this.logger.info(new StringBuilder().append("    Path      : ").append(this.location).toString());
			this.logger.info(new StringBuilder().append("    modellName: ").append(modelName).toString());
			this.logger.info(new StringBuilder().append("    dataDir   : ").append(this.dataDir).toString());
			if (this.binary)
			{
				this.saveDensityFileBinary(this.meansPool, new StringBuilder().append(this.dataDir).append("means").toString(), true);
				this.saveDensityFileBinary(this.variancePool, new StringBuilder().append(this.dataDir).append("variances").toString(), true);
				this.saveMixtureWeightsBinary(this.mixtureWeights, new StringBuilder().append(this.dataDir).append("mixture_weights").toString(), true);
				this.saveTransitionMatricesBinary(this.matrixPool, new StringBuilder().append(this.dataDir).append("transition_matrices").toString(), true);
			}
			else
			{
				this.saveDensityFileAscii(this.meansPool, new StringBuilder().append(this.dataDir).append("means.ascii").toString(), true);
				this.saveDensityFileAscii(this.variancePool, new StringBuilder().append(this.dataDir).append("variances.ascii").toString(), true);
				this.saveMixtureWeightsAscii(this.mixtureWeights, new StringBuilder().append(this.dataDir).append("mixture_weights.ascii").toString(), true);
				this.saveTransitionMatricesAscii(this.matrixPool, new StringBuilder().append(this.dataDir).append("transition_matrices.ascii").toString(), true);
			}
			this.saveHMMPool(this.useCDUnits, StreamFactory.getOutputStream(this.location, "mdef", true), new StringBuilder().append(this.location).append(File.separator).append("mdef").toString());
		}

		
		public virtual Map getContextIndependentUnits()
		{
			return this.contextIndependentUnits;
		}

		
		public virtual Pool getMeansPool()
		{
			return this.meansPool;
		}

		
		public virtual Pool getMeansTransformationMatrixPool()
		{
			return this.meanTransformationMatrixPool;
		}

		
		public virtual Pool getMeansTransformationVectorPool()
		{
			return this.meanTransformationVectorPool;
		}

		
		public virtual Pool getVariancePool()
		{
			return this.variancePool;
		}

		
		public virtual Pool getVarianceTransformationMatrixPool()
		{
			return this.varianceTransformationMatrixPool;
		}

		
		public virtual Pool getVarianceTransformationVectorPool()
		{
			return this.varianceTransformationVectorPool;
		}

		
		public virtual Pool getSenonePool()
		{
			return this.senonePool;
		}

		public virtual int getLeftContextSize()
		{
			return 1;
		}

		public virtual int getRightContextSize()
		{
			return 1;
		}

		public virtual HMMManager getHMMManager()
		{
			return this.hmmManager;
		}

		[LineNumberTable(new byte[]
		{
			162,
			196,
			112,
			113,
			113,
			113,
			113,
			113,
			113,
			113,
			113,
			113,
			113,
			127,
			16,
			113
		})]
		
		public virtual void logInfo()
		{
			this.logger.info("Sphinx3Saver");
			this.meansPool.logInfo(this.logger);
			this.variancePool.logInfo(this.logger);
			this.matrixPool.logInfo(this.logger);
			this.senonePool.logInfo(this.logger);
			this.meanTransformationMatrixPool.logInfo(this.logger);
			this.meanTransformationVectorPool.logInfo(this.logger);
			this.varianceTransformationMatrixPool.logInfo(this.logger);
			this.varianceTransformationVectorPool.logInfo(this.logger);
			this.mixtureWeights.logInfo(this.logger);
			this.senonePool.logInfo(this.logger);
			this.logger.info(new StringBuilder().append("Context Independent Unit Entries: ").append(this.contextIndependentUnits.size()).toString());
			this.hmmManager.logInfo(this.logger);
		}

		
		static Sphinx3Saver()
		{
		}

		[S4Boolean(new object[]
		{
			64,
			"Ledu/cmu/sphinx/util/props/S4Boolean;",
			"defaultValue",
			true
		})]
		public const string PROP_SPARSE_FORM = "sparseForm";

		protected internal bool sparseForm;

		[S4Boolean(new object[]
		{
			64,
			"Ledu/cmu/sphinx/util/props/S4Boolean;",
			"defaultValue",
			true
		})]
		public const string PROP_USE_CD_UNITS = "useCDUnits";

		[S4Double(new object[]
		{
			64,
			"Ledu/cmu/sphinx/util/props/S4Double;",
			"defaultValue",
			0.0
		})]
		public const string PROP_MC_FLOOR = "MixtureComponentScoreFloor";

		[S4Component(new object[]
		{
			64,
			"Ledu/cmu/sphinx/util/props/S4Component;",
			"type",
			new object[]
			{
				99,
				"Ledu/cmu/sphinx/linguist/acoustic/tiedstate/Loader, sphinx4-core-5prealpha-SNAPSHOT, Version=0/0/0/0, Culture=neutral, PublicKeyToken=null;"
			}
		})]
		public const string LOADER = "loader";

		[S4Integer(new object[]
		{
			64,
			"Ledu/cmu/sphinx/util/props/S4Integer;",
			"defaultValue",
			39
		})]
		public const string PROP_VECTOR_LENGTH = "vectorLength";

		protected internal Logger logger;

		protected internal const string FILLER = "filler";

		protected internal const string SILENCE_CIPHONE = "SIL";

		protected internal const int BYTE_ORDER_MAGIC = 287454020;

		public const string MODEL_VERSION = "0.3";

		protected internal const int CONTEXT_SIZE = 1;

		private string checksum;

		private bool doCheckSum;

		
		private Pool meansPool;

		
		private Pool variancePool;

		
		private Pool matrixPool;

		
		private Pool meanTransformationMatrixPool;

		
		private Pool meanTransformationVectorPool;

		
		private Pool varianceTransformationMatrixPool;

		
		private Pool varianceTransformationVectorPool;

		private GaussianWeights mixtureWeights;

		
		private Pool senonePool;

		private int vectorLength;

		
		private Map contextIndependentUnits;

		private HMMManager hmmManager;

		protected internal LogMath logMath;

		private bool binary;

		private string location;

		private bool swap;

		protected internal const string DENSITY_FILE_VERSION = "1.0";

		protected internal const string MIXW_FILE_VERSION = "1.0";

		protected internal const string TMAT_FILE_VERSION = "1.0";

		[S4String(new object[]
		{
			64,
			"Ledu/cmu/sphinx/util/props/S4String;",
			"defaultValue",
			"."
		})]
		public const string SAVE_LOCATION = "saveLocation";

		[S4String(new object[]
		{
			64,
			"Ledu/cmu/sphinx/util/props/S4String;",
			"mandatory",
			false,
			"defaultValue",
			""
		})]
		public const string DATA_LOCATION = "dataLocation";

		private string dataDir;

		[S4String(new object[]
		{
			64,
			"Ledu/cmu/sphinx/util/props/S4String;",
			"mandatory",
			false,
			"defaultValue",
			""
		})]
		public const string DEF_FILE = "definitionFile";

		public bool useCDUnits;

		
		internal static bool assertionsDisabled = !ClassLiteral<Sphinx3Saver>.Value.desiredAssertionStatus();
	}
}
