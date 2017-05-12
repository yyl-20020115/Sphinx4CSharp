using edu.cmu.sphinx.util.props;
using ikvm.@internal;
using java.lang;

namespace edu.cmu.sphinx.frontend.filter
{
	public class Dither : BaseDataProcessor
	{
		public override void initialize()
		{
			base.initialize();
			if (this.useRandSeed)
			{
				this.r = new java.util.Random();
			}
			else
			{
				this.r = new java.util.Random((long)((ulong)12345));
			}
		}
	
		private DoubleData process(Data data)
		{
			if (!Dither.assertionsDisabled && !(data is DoubleData))
			{
				
				throw new AssertionError();
			}
			DoubleData doubleData = (DoubleData)data;
			double[] values = doubleData.getValues();
			double[] array = new double[values.Length];
			for (int i = 0; i < values.Length; i++)
			{
				array[i] = (double)(this.r.nextFloat() * 2f) * this.ditherMax - this.ditherMax + values[i];
				array[i] = Math.max(Math.min(array[i], this.maxValue), this.minValue);
			}
			return new DoubleData(array, doubleData.getSampleRate(), doubleData.getFirstSampleNumber());
		}
		
		public Dither(double ditherMax, bool useRandSeed, double maxValue, double minValue)
		{
			this.initLogger();
			this.ditherMax = ditherMax;
			this.useRandSeed = useRandSeed;
			this.maxValue = maxValue;
			this.minValue = minValue;
			this.initialize();
		}
	
		public Dither()
		{
		}
		
		public override void newProperties(PropertySheet ps)
		{
			base.newProperties(ps);
			this.ditherMax = ps.getDouble("maxDither");
			this.useRandSeed = ps.getBoolean("useRandSeed").booleanValue();
			this.maxValue = ps.getDouble("upperValueBound");
			this.minValue = ps.getDouble("lowerValueBound");
		}
	
		public override Data getData()
		{
			object obj = this.getPredecessor().getData();
			if ((Data)obj != null && this.ditherMax != (double)0f && (((Data)obj) is DoubleData || ((Data)obj) is FloatData))
			{
				obj = this.process((Data)obj);
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

		[S4Double(new object[]
		{
			64,
			"Ledu/cmu/sphinx/util/props/S4Double;",
			"defaultValue",
			2.0
		})]
		public const string PROP_MAX_DITHER = "maxDither";

		private double ditherMax;

		[S4Double(new object[]
		{
			64,
			"Ledu/cmu/sphinx/util/props/S4Double;",
			"defaultValue",
			1.7976931348623157E+308
		})]
		public const string PROP_MAX_VAL = "upperValueBound";

		private double maxValue;

		[S4Double(new object[]
		{
			64,
			"Ledu/cmu/sphinx/util/props/S4Double;",
			"defaultValue",
			-1.7976931348623157E+308
		})]
		public const string PROP_MIN_VAL = "lowerValueBound";

		private double minValue;

		[S4Boolean(new object[]
		{
			64,
			"Ledu/cmu/sphinx/util/props/S4Boolean;",
			"defaultValue",
			false
		})]
		public const string PROP_USE_RANDSEED = "useRandSeed";

		private bool useRandSeed;

		internal java.util.Random r;

		internal static bool assertionsDisabled = !ClassLiteral<Dither>.Value.desiredAssertionStatus();
	}
}
