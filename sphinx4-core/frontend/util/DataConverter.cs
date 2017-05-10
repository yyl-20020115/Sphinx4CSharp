using System;

using edu.cmu.sphinx.util;
using edu.cmu.sphinx.util.props;
using IKVM.Attributes;
using java.lang;

namespace edu.cmu.sphinx.frontend.util
{
	public class DataConverter : BaseDataProcessor
	{
		[Throws(new string[]
		{
			"edu.cmu.sphinx.util.props.PropertyException"
		})]
		[LineNumberTable(new byte[]
		{
			159,
			167,
			104,
			102,
			103
		})]
		
		public DataConverter(string convMode)
		{
			this.initLogger();
			this.convMode = convMode;
		}

		[LineNumberTable(new byte[]
		{
			159,
			172,
			134
		})]
		
		public DataConverter()
		{
		}

		[Throws(new string[]
		{
			"edu.cmu.sphinx.util.props.PropertyException"
		})]
		[LineNumberTable(new byte[]
		{
			159,
			183,
			135,
			113
		})]
		
		public override void newProperties(PropertySheet ps)
		{
			base.newProperties(ps);
			this.convMode = ps.getString("conversionMode");
		}

		[Throws(new string[]
		{
			"edu.cmu.sphinx.frontend.DataProcessingException"
		})]
		[LineNumberTable(new byte[]
		{
			159,
			191,
			140,
			127,
			0,
			108,
			114,
			107,
			127,
			2,
			108,
			114,
			171
		})]
		
		public override Data getData()
		{
			object obj = this.getPredecessor().getData();
			if (((Data)obj) is DoubleData && java.lang.String.instancehelper_equals(this.convMode, "d2f"))
			{
				DoubleData doubleData = (DoubleData)((Data)obj);
				obj = new FloatData(MatrixUtils.double2float(doubleData.getValues()), doubleData.getSampleRate(), doubleData.getFirstSampleNumber());
			}
			else if (((Data)obj) is FloatData && java.lang.String.instancehelper_equals(this.convMode, "f2d"))
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
