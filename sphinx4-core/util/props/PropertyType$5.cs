using java.lang;
using java.net;

namespace edu.cmu.sphinx.util.props
{
	[System.Serializable]
	internal sealed class PropertyType_5 : PropertyType
	{
		internal PropertyType_5(string text, int num, string text2) : base(text, num, text2, null)
		{
			System.GC.KeepAlive(this);
		}
	
		protected internal override bool validateString(string text)
		{
			if (String.instancehelper_startsWith(String.instancehelper_toLowerCase(text), "resource:/"))
			{
				return true;
			}
			if (String.instancehelper_indexOf(text, 58) == -1)
			{
				text = new StringBuilder().append("file:").append(text).toString();
			}
			new URL(text);
			return true;
		}
	}
}
