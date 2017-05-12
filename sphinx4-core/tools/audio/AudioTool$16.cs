using java.awt.@event;
using java.util;
using java.lang;

namespace edu.cmu.sphinx.tools.audio
{
	internal sealed class AudioTool_16 : Object, ActionListener, EventListener
	{
		internal AudioTool_16()
		{
		}
	
		public void actionPerformed(ActionEvent actionEvent)
		{
			java.lang.System.exit(0);
		}
	}
}
