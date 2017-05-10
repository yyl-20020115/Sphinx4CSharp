using System;

using System.Runtime.Serialization;
using System.Security.Permissions;
using edu.cmu.sphinx.frontend;
using edu.cmu.sphinx.util;
using IKVM.Attributes;
using java.lang;

namespace edu.cmu.sphinx.linguist.acoustic.tiedstate
{
	[Serializable]
	public class GaussianMixture : ScoreCachingSenone
	{
		public override long getID()
		{
			return (long)this.id;
		}

		public override MixtureComponent[] getMixtureComponents()
		{
			return this.mixtureComponents;
		}

		[LineNumberTable(new byte[]
		{
			159,
			189,
			136,
			107,
			103,
			103,
			103
		})]
		
		public GaussianMixture(GaussianWeights mixtureWeights, MixtureComponent[] mixtureComponents, int id)
		{
			this.logMath = LogMath.getLogMath();
			this.mixtureComponents = mixtureComponents;
			this.mixtureWeights = mixtureWeights;
			this.id = id;
		}

		[LineNumberTable(new byte[]
		{
			12,
			127,
			16
		})]
		
		public override void dump(string msg)
		{
			java.lang.System.@out.println(new StringBuilder().append(msg).append(" GaussianMixture: ID ").append(this.getID()).toString());
		}

		[LineNumberTable(new byte[]
		{
			23,
			104,
			130,
			103
		})]
		
		public override bool equals(object o)
		{
			if (!(o is Senone))
			{
				return false;
			}
			Senone senone = (Senone)o;
			return this.getID() == senone.getID();
		}

		[LineNumberTable(new byte[]
		{
			38,
			103,
			102,
			99
		})]
		
		public override int hashCode()
		{
			long num = this.getID();
			int num2 = (int)(num >> 32);
			int num3 = (int)num;
			return num2 + num3;
		}

		
		
		public override string toString()
		{
			return new StringBuilder().append("senone id: ").append(this.getID()).toString();
		}

		[LineNumberTable(new byte[]
		{
			63,
			104,
			143,
			140,
			102,
			204,
			112,
			58,
			230,
			60,
			230,
			72
		})]
		
		public override float calculateScore(Data feature)
		{
			if (feature is DoubleData)
			{
				java.lang.System.err.println("DoubleData conversion required on mixture level!");
			}
			float[] values = FloatData.toFloatData(feature).getValues();
			float num = float.MinValue;
			for (int i = 0; i < this.mixtureComponents.Length; i++)
			{
				num = this.logMath.addAsLinear(num, this.mixtureComponents[i].getScore(values) + this.mixtureWeights.get(this.id, 0, i));
			}
			return num;
		}

		[LineNumberTable(new byte[]
		{
			88,
			104,
			143,
			140,
			109,
			204,
			255,
			7,
			60,
			230,
			71
		})]
		
		public override float[] calculateComponentScore(Data feature)
		{
			if (feature is DoubleData)
			{
				java.lang.System.err.println("DoubleData conversion required on mixture level!");
			}
			float[] values = FloatData.toFloatData(feature).getValues();
			float[] array = new float[this.mixtureComponents.Length];
			for (int i = 0; i < this.mixtureComponents.Length; i++)
			{
				array[i] = this.mixtureComponents[i].getScore(values) + this.mixtureWeights.get(this.id, 0, i);
			}
			return array;
		}

		
		
		public virtual int dimension()
		{
			return this.mixtureComponents[0].getMean().Length;
		}

		
		public virtual int numComponents()
		{
			return this.mixtureComponents.Length;
		}

		[LineNumberTable(new byte[]
		{
			123,
			109,
			103,
			54,
			134
		})]
		
		public override float[] getLogMixtureWeights()
		{
			float[] array = new float[this.getMixtureComponents().Length];
			for (int i = 0; i < array.Length; i++)
			{
				array[i] = this.mixtureWeights.get(this.id, 0, i);
			}
			return array;
		}

		[LineNumberTable(new byte[]
		{
			160,
			68,
			109,
			103,
			63,
			3,
			166
		})]
		
		public virtual float[] getComponentWeights()
		{
			float[] array = new float[this.getMixtureComponents().Length];
			for (int i = 0; i < array.Length; i++)
			{
				array[i] = (float)this.logMath.logToLinear(this.mixtureWeights.get(this.id, 0, i));
			}
			return array;
		}

		
		
		public virtual float getLogComponentWeight(int index)
		{
			return this.mixtureWeights.get(this.id, 0, index);
		}

		
		[PermissionSet(SecurityAction.Demand, XML = "<PermissionSet class=\"System.Security.PermissionSet\"\nversion=\"1\">\n<IPermission class=\"System.Security.Permissions.SecurityPermission, mscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089\"\nversion=\"1\"\nFlags=\"SerializationFormatter\"/>\n</PermissionSet>\n")]
		protected GaussianMixture(SerializationInfo serializationInfo, StreamingContext streamingContext) : base(serializationInfo, streamingContext)
		{
		}

		protected internal GaussianWeights mixtureWeights;

		private MixtureComponent[] mixtureComponents;

		protected internal int id;

		protected internal LogMath logMath;
	}
}
