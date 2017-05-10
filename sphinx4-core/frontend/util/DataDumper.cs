using System;

using edu.cmu.sphinx.frontend.endpoint;
using edu.cmu.sphinx.util.props;
using IKVM.Attributes;
using java.lang;
using java.text;
using java.util;

namespace edu.cmu.sphinx.frontend.util
{
	public class DataDumper : BaseDataProcessor
	{
		[LineNumberTable(new byte[]
		{
			47,
			99,
			116,
			104,
			107,
			159,
			10,
			107,
			103,
			103,
			127,
			6,
			115,
			63,
			14,
			168,
			106,
			112,
			104,
			104,
			111,
			105,
			142,
			108,
			127,
			6,
			115,
			63,
			14,
			168,
			106,
			112,
			104,
			105,
			127,
			7,
			119,
			63,
			15,
			168,
			138
		})]
		
		private void dumpData(Data data)
		{
			if (data == null)
			{
				java.lang.System.@out.println("Data: null");
			}
			else if (data is Signal)
			{
				if (this.outputSignals)
				{
					java.lang.System.@out.println(new StringBuilder().append("Signal: ").append(data).toString());
				}
			}
			else if (data is DoubleData)
			{
				DoubleData doubleData = (DoubleData)data;
				double[] values = doubleData.getValues();
				java.lang.System.@out.print(new StringBuilder().append("Frame ").append(values.Length).toString());
				double[] array = values;
				int num = array.Length;
				for (int i = 0; i < num; i++)
				{
					double num2 = array[i];
					java.lang.System.@out.print(new StringBuilder().append(' ').append(this.formatter.format(num2)).toString());
				}
				java.lang.System.@out.println();
			}
			else if (data is SpeechClassifiedData)
			{
				SpeechClassifiedData speechClassifiedData = (SpeechClassifiedData)data;
				double[] values = speechClassifiedData.getValues();
				java.lang.System.@out.print("Frame ");
				if (speechClassifiedData.isSpeech())
				{
					java.lang.System.@out.print('*');
				}
				else
				{
					java.lang.System.@out.print(' ');
				}
				java.lang.System.@out.print(new StringBuilder().append(" ").append(values.Length).toString());
				double[] array = values;
				int num = array.Length;
				for (int i = 0; i < num; i++)
				{
					double num2 = array[i];
					java.lang.System.@out.print(new StringBuilder().append(' ').append(this.formatter.format(num2)).toString());
				}
				java.lang.System.@out.println();
			}
			else if (data is FloatData)
			{
				FloatData floatData = (FloatData)data;
				float[] values2 = floatData.getValues();
				java.lang.System.@out.print(new StringBuilder().append("Frame ").append(values2.Length).toString());
				float[] array2 = values2;
				int num = array2.Length;
				for (int i = 0; i < num; i++)
				{
					float num3 = array2[i];
					java.lang.System.@out.print(new StringBuilder().append(' ').append(this.formatter.format((double)num3)).toString());
				}
				java.lang.System.@out.println();
			}
		}

		[LineNumberTable(new byte[]
		{
			159,
			133,
			162,
			104,
			102,
			127,
			1,
			103
		})]
		
		public DataDumper(string format, bool outputSignals)
		{
			this.initLogger();
			DecimalFormat.__<clinit>();
			DecimalFormatSymbols.__<clinit>();
			this.formatter = new DecimalFormat(format, new DecimalFormatSymbols(Locale.US));
			this.outputSignals = outputSignals;
		}

		[LineNumberTable(new byte[]
		{
			159,
			187,
			134
		})]
		
		public DataDumper()
		{
		}

		[Throws(new string[]
		{
			"edu.cmu.sphinx.util.props.PropertyException"
		})]
		[LineNumberTable(new byte[]
		{
			8,
			135,
			140,
			108,
			127,
			1,
			118
		})]
		
		public override void newProperties(PropertySheet ps)
		{
			base.newProperties(ps);
			this.logger = ps.getLogger();
			string @string = ps.getString("outputFormat");
			DecimalFormat.__<clinit>();
			string text = @string;
			DecimalFormatSymbols.__<clinit>();
			this.formatter = new DecimalFormat(text, new DecimalFormatSymbols(Locale.US));
			this.outputSignals = ps.getBoolean("outputSignals").booleanValue();
		}

		[LineNumberTable(new byte[]
		{
			20,
			102
		})]
		
		public override void initialize()
		{
			base.initialize();
		}

		[Throws(new string[]
		{
			"edu.cmu.sphinx.frontend.DataProcessingException"
		})]
		[LineNumberTable(new byte[]
		{
			33,
			108,
			135
		})]
		
		public override Data getData()
		{
			Data data = this.getPredecessor().getData();
			this.dumpData(data);
			return data;
		}

		[S4String(new object[]
		{
			64,
			"Ledu/cmu/sphinx/util/props/S4String;",
			"defaultValue",
			"0.00000E00;-0.00000E00"
		})]
		public const string PROP_OUTPUT_FORMAT = "outputFormat";

		[S4Boolean(new object[]
		{
			64,
			"Ledu/cmu/sphinx/util/props/S4Boolean;",
			"defaultValue",
			true
		})]
		public const string PROP_OUTPUT_SIGNALS = "outputSignals";

		private bool outputSignals;

		private DecimalFormat formatter;
	}
}
