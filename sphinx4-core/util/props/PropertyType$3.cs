﻿using System;

using IKVM.Attributes;
using java.lang;

namespace edu.cmu.sphinx.util.props
{
	
	
	.
	
	[Serializable]
	internal sealed class PropertyType_3 : PropertyType
	{
		
		public new static void __<clinit>()
		{
		}

		
		
		internal PropertyType_3(string text, int num, string text2) : base(text, num, text2, null)
		{
			GC.KeepAlive(this);
		}

		[Throws(new string[]
		{
			"java.lang.Exception"
		})]
		[LineNumberTable(new byte[]
		{
			159,
			177,
			103
		})]
		
		protected internal override bool validateString(string text)
		{
			Float.parseFloat(text);
			return true;
		}

		
		static PropertyType_3()
		{
			PropertyType.__<clinit>();
		}
	}
}
