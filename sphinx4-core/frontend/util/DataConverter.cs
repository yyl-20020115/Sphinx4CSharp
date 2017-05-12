using edu.cmu.sphinx.util;
using edu.cmu.sphinx.util.props;
using java.lang;

namespace edu.cmu.sphinx.frontend.util
{
	public class DataConverter : BaseDataProcessor
	{
		public DataConverter(string convMode)
		{
			this.initLogger();
			this.convMode = convMode;
		}
		
		public DataConverter()
		{
		}

		public override void newProperties(PropertySheet ps)
		{
			base.newProperties(ps);
			this.convMode = ps.getString("conversionMode");
		}

		public override Data getData()
		{
			object obj = this.getPredecessor().getData();
			if (((Data)obj) is DoubleData && String.instancehelper_equals(this.convMode, "d2f"))
			{
				DoubleData doubleData = (DoubleData)((Data)obj);
				obj = new FloatData(MatrixUtils.double2float(doubleData.getValues()), doubleData.getSampleRate(), doubleData.getFirstSampleNumber());
			}
			else if (((Data)obj) is FloatData && String.instancehelper_equals(this.convMode, "f2d"))
			{
				FloatData floatData = (FloatData)((Data)obj);
				obj = new DoubleData(MatrixUtils.float2double(floatData.getValues()), floatData.getSampleRate(), floatData.getFirstSampleNumber());
			}
			object obj2 = obj;
			Data result;
			if (obj2 != null)
			{
				if ((result = (obj2 as Data)) == null)
				{
					throw new IncompatibleClassChangeError();
				}
			}
			else
			{
				result = null;
			}
			return result;
		}

		public const string CONVERT_D2F = "d2f";

		public const string CONVERT_F2D = "f2d";

		[S4String(new object[]
		{
			64,
			"Ledu/cmu/sphinx/util/props/S4String;",
			"defaultValue",
			"d2f",
			"range",
			new object[]
			{
				91,
				"d2f",
				"f2d"
			}
		})]
		public const string PROP_CONVERSION_MODE = "conversionMode";

		private string convMode;
	}
}
