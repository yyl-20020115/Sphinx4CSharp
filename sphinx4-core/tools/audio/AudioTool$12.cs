using java.awt.@event;
using java.util;
using java.lang;

namespace edu.cmu.sphinx.tools.audio
{
	internal sealed class AudioTool_12 : Object, ActionListener, EventListener
	{
		internal AudioTool_12()
		{
		}
		
		public void actionPerformed(ActionEvent actionEvent)
		{
			AudioTool.player.play(AudioTool.audioPanel.getSelectionStart(), AudioTool.audioPanel.getSelectionEnd());
		}
	}
}
