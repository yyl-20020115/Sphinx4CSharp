using edu.cmu.sphinx.linguist.acoustic.tiedstate;
using edu.cmu.sphinx.util.props;
using java.lang;

namespace edu.cmu.sphinx.frontend.feature
{
	public class FeatureTransform : BaseDataProcessor
	{		
		private void init(Loader loader)
		{
			this.loader = loader;
			try
			{
				loader.load();
			}
			catch (System.Exception ex)
			{
				Throwable.instancehelper_printStackTrace(ex);
			}
			this.transform = loader.getTransformMatrix();
		}
		
		public FeatureTransform(Loader loader)
		{
			this.initLogger();
			this.init(loader);
		}
		
		public FeatureTransform()
		{
		}
		
		public override void newProperties(PropertySheet ps)
		{
			base.newProperties(ps);
			this.init((Loader)ps.getComponent("loader"));
		}
		
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
