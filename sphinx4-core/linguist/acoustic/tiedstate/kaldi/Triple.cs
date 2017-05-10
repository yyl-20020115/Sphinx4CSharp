﻿using System;

using IKVM.Attributes;
using java.lang;

namespace edu.cmu.sphinx.linguist.acoustic.tiedstate.kaldi
{
	[SourceFile("TransitionModel.java")]
	internal sealed class Triple : java.lang.Object
	{
		[LineNumberTable(new byte[]
		{
			3,
			104,
			103,
			103,
			103
		})]
		
		public Triple(int num, int num2, int num3)
		{
			this.phone = num;
			this.hmmState = num2;
			this.pdf = num3;
		}

		[LineNumberTable(new byte[]
		{
			11,
			104,
			130,
			135
		})]
		public override bool equals(object obj)
		{
			if (!(obj is Triple))
			{
				return false;
			}
			Triple triple = (Triple)obj;
			return this.phone == triple.phone && this.hmmState == triple.hmmState && this.pdf == triple.pdf;
		}

		public override int hashCode()
		{
			return 31 * (31 * this.phone + this.hmmState) + this.pdf;
		}

		
		
		public override string toString()
		{
			return java.lang.String.format("Triple {%d, %d, %d}", new object[]
			{
				Integer.valueOf(this.phone),
				Integer.valueOf(this.hmmState),
				Integer.valueOf(this.pdf)
			});
		}

		private int phone;

		private int hmmState;

		private int pdf;
	}
}
