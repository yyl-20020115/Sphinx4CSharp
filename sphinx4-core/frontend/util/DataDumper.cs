using edu.cmu.sphinx.frontend.endpoint;
using edu.cmu.sphinx.util.props;
using java.lang;
using java.text;
using java.util;

namespace edu.cmu.sphinx.frontend.util
{
	public class DataDumper : BaseDataProcessor
	{		
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
		
		public DataDumper(string format, bool outputSignals)
		{
			this.initLogger();
			this.formatter = new DecimalFormat(format, new DecimalFormatSymbols(Locale.US));
			this.outputSignals = outputSignals;
		}
		
		public DataDumper()
		{
		}
		
		public override void newProperties(PropertySheet ps)
		{
			base.newProperties(ps);
			this.logger = ps.getLogger();
			string @string = ps.getString("outputFormat");
			string text = @string;
			this.formatter = new DecimalFormat(text, new DecimalFormatSymbols(Locale.US));
			this.outputSignals = ps.getBoolean("outputSignals").booleanValue();
		}
		
		public override void initialize()
		{
			base.initialize();
		}

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
