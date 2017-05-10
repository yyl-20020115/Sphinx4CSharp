using System;

using System.Runtime.Serialization;
using System.Security.Permissions;
using edu.cmu.sphinx.frontend;
using edu.cmu.sphinx.util;
using IKVM.Attributes;
using java.lang;
using java.util;

namespace edu.cmu.sphinx.linguist.acoustic.tiedstate.kaldi
{
	[Serializable]
	public class DiagGmm : ScoreCachingSenone
	{
		[LineNumberTable(new byte[]
		{
			22,
			108,
			152,
			101,
			102,
			127,
			1,
			172,
			116,
			109,
			108,
			106,
			127,
			4,
			255,
			17,
			61,
			235,
			70,
			244,
			57,
			235,
			74
		})]
		
		public override float[] calculateComponentScore(Data data)
		{
			float[] values = FloatData.toFloatData(data).getValues();
			int num = this.meansInvVars.Length;
			int num2 = this.gconsts.Length;
			int num3 = (num2 != -1) ? (num / num2) : (-num);
			if (values.Length != num3)
			{
				string text = "feature vector must be of length %d, got %d";
				string text2 = java.lang.String.format(text, new object[]
				{
					Integer.valueOf(num3),
					Integer.valueOf(values.Length)
				});
				string text3 = text2;
				
				throw new IllegalArgumentException(text3);
			}
			float[] array = Arrays.copyOf(this.gconsts, this.gconsts.Length);
			for (int i = 0; i < array.Length; i++)
			{
				for (int j = 0; j < values.Length; j++)
				{
					int num4 = i * values.Length + j;
					float[] array2 = array;
					int num5 = i;
					float[] array3 = array2;
					array3[num5] += this.meansInvVars[num4] * values[j];
					float[] array4 = array;
					num5 = i;
					array3 = array4;
					array3[num5] -= 0.5f * this.invVars[num4] * values[j] * values[j];
				}
				array[i] = LogMath.getLogMath().lnToLog(array[i]);
			}
			return array;
		}

		[LineNumberTable(new byte[]
		{
			159,
			172,
			104,
			135,
			107,
			107,
			140,
			139,
			135,
			107,
			140,
			107,
			108,
			107
		})]
		
		public DiagGmm(int id, KaldiTextParser parser)
		{
			this.id = id;
			parser.expectToken("<DiagGMM>");
			parser.expectToken("<GCONSTS>");
			this.gconsts = parser.getFloatArray();
			parser.expectToken("<WEIGHTS>");
			parser.getFloatArray();
			parser.expectToken("<MEANS_INVVARS>");
			this.meansInvVars = parser.getFloatArray();
			parser.expectToken("<INV_VARS>");
			this.invVars = parser.getFloatArray();
			parser.expectToken("</DiagGMM>");
		}

		public virtual int getId()
		{
			return this.id;
		}

		[LineNumberTable(new byte[]
		{
			13,
			102,
			102,
			126,
			47,
			168
		})]
		
		public override float calculateScore(Data data)
		{
			float num = float.MinValue;
			LogMath logMath = LogMath.getLogMath();
			float[] array = this.calculateComponentScore(data);
			int num2 = array.Length;
			for (int i = 0; i < num2; i++)
			{
				Float @float = Float.valueOf(array[i]);
				num = logMath.addAsLinear(num, @float.floatValue());
			}
			return num;
		}

		public override long getID()
		{
			return (long)this.id;
		}

		[LineNumberTable(new byte[]
		{
			50,
			127,
			9
		})]
		
		public override void dump(string msg)
		{
			java.lang.System.@out.format("%s DiagGmm: ID %d\n", new object[]
			{
				msg,
				Integer.valueOf(this.id)
			});
		}

		public override MixtureComponent[] getMixtureComponents()
		{
			return null;
		}

		public override float[] getLogMixtureWeights()
		{
			return null;
		}

		
		[PermissionSet(SecurityAction.Demand, XML = "<PermissionSet class=\"System.Security.PermissionSet\"\nversion=\"1\">\n<IPermission class=\"System.Security.Permissions.SecurityPermission, mscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089\"\nversion=\"1\"\nFlags=\"SerializationFormatter\"/>\n</PermissionSet>\n")]
		protected DiagGmm(SerializationInfo serializationInfo, StreamingContext streamingContext) : base(serializationInfo, streamingContext)
		{
		}

		private int id;

		private float[] gconsts;

		private float[] invVars;

		private float[] meansInvVars;
	}
}
