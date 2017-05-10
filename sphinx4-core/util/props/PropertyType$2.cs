using System;

using IKVM.Attributes;
using java.lang;

namespace edu.cmu.sphinx.util.props
{
	
	[EnclosingMethod("edu.cmu.sphinx.util.props.PropertyType", null, null)]
	[SourceFile("PropertyType.java")]
	
	[Serializable]
	internal sealed class PropertyType_2 : PropertyType
	{
		
		public new static void __<clinit>()
		{
		}

		
		
		internal PropertyType_2(string text, int num, string text2) : base(text, num, text2, null)
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
			171,
			104
		})]
		
		protected internal override bool validateString(string text)
		{
			text = java.lang.String.instancehelper_toLowerCase(text);
			return java.lang.String.instancehelper_equals("true", text) || java.lang.String.instancehelper_equals("false", text);
		}

		
		static PropertyType_2()
		{
			PropertyType.__<clinit>();
		}
	}
}
