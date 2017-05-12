using java.io;
using java.lang;

namespace edu.cmu.sphinx.util
{
	internal sealed class TestFileFilter : Object, FileFilter
	{
		internal TestFileFilter()
		{
		}

		public bool accept(File file)
		{
			string name = file.getName();
			int result;
			try
			{
				Integer.parseInt(name);
				result = 1;
			}
			catch (NumberFormatException)
			{
				return false;
			}
			return result != 0;
		}
	}
}
