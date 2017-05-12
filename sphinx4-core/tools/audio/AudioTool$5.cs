using java.awt.@event;
using java.util;
using java.lang;

namespace edu.cmu.sphinx.tools.audio
{
	internal sealed class AudioTool_5 : Object, ActionListener, EventListener
	{
		internal AudioTool_5()
		{
		}
		
		public void actionPerformed(ActionEvent actionEvent)
		{
			AudioTool.audioPanel.selectAll();
		}
	}
}
