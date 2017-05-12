namespace edu.cmu.sphinx.util.props
{
	[System.Serializable]
	internal sealed class PropertyType_4 : PropertyType
	{
		internal PropertyType_4(string text, int num, string text2) : base(text, num, text2, null)
		{
			System.GC.KeepAlive(this);
		}

		protected internal override bool validateString(string text)
		{
			java.lang.Double.parseDouble(text);
			return true;
		}
	}
}
