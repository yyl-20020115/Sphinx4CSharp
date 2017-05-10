using System;

using edu.cmu.sphinx.linguist.acoustic.tiedstate;
using edu.cmu.sphinx.util.props;
using IKVM.Attributes;
using IKVM.Runtime;
using java.lang;

namespace edu.cmu.sphinx.frontend.feature
{
	public class FeatureTransform : BaseDataProcessor
	{
		[LineNumberTable(new byte[]
		{
			12,
			167,
			189,
			2,
			97,
			166,
			108
		})]
		
		private void init(Loader loader)
		{
			this.loader = loader;
			Exception ex3;
			try
			{
				loader.load();
			}
			catch (Exception ex)
			{
				Exception ex2 = ByteCodeHelper.MapException<Exception>(ex, 0);
				if (ex2 == null)
				{
					throw;
				}
				ex3 = ex2;
				goto IL_21;
			}
			goto IL_2D;
			IL_21:
			Exception ex4 = ex3;
			Throwable.instancehelper_printStackTrace(ex4);
			IL_2D:
			this.transform = loader.getTransformMatrix();
		}

		[LineNumberTable(new byte[]
		{
			159,
			182,
			104,
			102,
			103
		})]
		
		public FeatureTransform(Loader loader)
		{
			this.initLogger();
			this.init(loader);
		}

		[LineNumberTable(new byte[]
		{
			159,
			187,
			102
		})]
		
		public FeatureTransform()
		{
		}

		[Throws(new string[]
		{
			"edu.cmu.sphinx.util.props.PropertyException"
		})]
		[LineNumberTable(new byte[]
		{
			7,
			103,
			118
		})]
		
		public override void newProperties(PropertySheet ps)
		{
			base.newProperties(ps);
			this.init((Loader)ps.getComponent("loader"));
		}

		[Throws(new string[]
		{
			"edu.cmu.sphinx.frontend.DataProcessingException"
		})]
		[LineNumberTable(new byte[]
		{
			35,
			140,
			117,
			130,
			103,
			135,
			111,
			144,
			141,
			110,
			105,
			63,
			6,
			40,
			232,
			69,
			109,
			110,
			63,
			0,
			200,
			98,
			102,
			10
		})]
		
		public override Data getData()
		{
			Data data = this.getPredecessor().getData();
			if (null == this.transform || null == data || !(data is FloatData))
			{
				return data;
			}
			FloatData floatData = (FloatData)data;
			float[] array = floatData.getValues();
			if (array.Length > this.transform[0].Length + 1)
			{
				string text = "dimenstion mismatch";
				
				throw new IllegalArgumentException(text);
			}
			float[] array2 = new float[this.transform.Length];
			for (int i = 0; i < this.transform.Length; i++)
			{
				for (int j = 0; j < array.Length; j++)
				{
					float[] array3 = array2;
					int num = i;
					float[] array4 = array3;
					array4[num] += this.transform[i][j] * array[j];
				}
			}
			if (array.Length > this.transform[0].Length)
			{
				for (int i = 0; i < this.transform.Length; i++)
				{
					float[] array5 = array2;
					int num = i;
					float[] array4 = array5;
					array4[num] += this.transform[i][array.Length];
				}
			}
			return new FloatData(array2, floatData.getSampleRate(), floatData.getFirstSampleNumber());
		}

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
		public const string PROP_LOADER = "loader";

		internal float[][] transform;

		protected internal Loader loader;

		internal int rows;

		internal int values;
	}
}
