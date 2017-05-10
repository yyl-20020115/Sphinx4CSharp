using System;

using IKVM.Attributes;
using java.lang;

namespace edu.cmu.sphinx.util.props
{
	
	[EnclosingMethod("edu.cmu.sphinx.util.props.PropertyType", null, null)]
	[SourceFile("PropertyType.java")]
	
	[Serializable]
	internal sealed class PropertyType_1 : PropertyType
	{
		
		public new static void __<clinit>()
		{
		}

		
		
		internal PropertyType_1(string text, int num, string text2) : base(text, num, text2, null)
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
			165,
			103
		})]
		
		protected internal override bool validateString(string text)
		{
			Integer.parseInt(text);
			return true;
		}

		
		static PropertyType_1()
		{
			PropertyType.__<clinit>();
		}
	}
}
