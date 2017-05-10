using System;

using IKVM.Attributes;
using java.lang;

namespace edu.cmu.sphinx.util.props
{
	
	[EnclosingMethod("edu.cmu.sphinx.util.props.PropertyType", null, null)]
	[SourceFile("PropertyType.java")]
	
	[Serializable]
	internal sealed class PropertyType$4 : PropertyType
	{
		
		public new static void __<clinit>()
		{
		}

		
		
		internal PropertyType$4(string text, int num, string text2) : base(text, num, text2, null)
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
			183,
			103
		})]
		
		protected internal override bool validateString(string text)
		{
			Double.parseDouble(text);
			return true;
		}

		
		static PropertyType$4()
		{
			PropertyType.__<clinit>();
		}
	}
}
