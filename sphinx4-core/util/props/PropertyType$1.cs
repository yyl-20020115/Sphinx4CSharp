using java.lang;

namespace edu.cmu.sphinx.util.props
{
	[System.Serializable]
	internal sealed class PropertyType_1 : PropertyType
	{				
		internal PropertyType_1(string text, int num, string text2) : base(text, num, text2, null)
		{
			System.GC.KeepAlive(this);
		}
		
		protected internal override bool validateString(string text)
		{
			Integer.parseInt(text);
			return true;
		}
	}
}
