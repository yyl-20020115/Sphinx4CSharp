using System;

using IKVM.Attributes;
using java.lang;
using java.net;

namespace edu.cmu.sphinx.util.props
{
	
	[EnclosingMethod("edu.cmu.sphinx.util.props.PropertyType", null, null)]
	[SourceFile("PropertyType.java")]
	
	[Serializable]
	internal sealed class PropertyType$5 : PropertyType
	{
		
		public new static void __<clinit>()
		{
		}

		
		
		internal PropertyType$5(string text, int num, string text2) : base(text, num, text2, null)
		{
			GC.KeepAlive(this);
		}

		[Throws(new string[]
		{
			"java.lang.Exception"
		})]
		[LineNumberTable(new byte[]
		{
			12,
			114,
			162,
			107,
			188,
			103
		})]
		
		public override bool validateString(string text)
		{
			if (java.lang.String.instancehelper_startsWith(java.lang.String.instancehelper_toLowerCase(text), "resource:/"))
			{
				return true;
			}
			if (java.lang.String.instancehelper_indexOf(text, 58) == -1)
			{
				text = new StringBuilder().append("file:").append(text).toString();
			}
			new URL(text);
			return true;
		}

		
		static PropertyType$5()
		{
			PropertyType.__<clinit>();
		}
	}
}
