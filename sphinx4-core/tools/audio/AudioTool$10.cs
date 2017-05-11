using java.awt.@event;
using java.util;

namespace edu.cmu.sphinx.tools.audio
{
	internal sealed class AudioTool_10 : java.lang.Object, ActionListener, EventListener
	{
		internal AudioTool_10()
		{
		}
	
		public void actionPerformed(ActionEvent actionEvent)
		{
			AudioTool.player.play(AudioTool.audioPanel.getSelectionStart(), AudioTool.audioPanel.getSelectionEnd());
		}
	}
}
