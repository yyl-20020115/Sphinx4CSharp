using java.awt.@event;
using java.util;
using java.lang;

namespace edu.cmu.sphinx.tools.audio
{
	internal sealed class AudioTool_11 : Object, ActionListener, EventListener
	{
		internal AudioTool_11()
		{
		}
	
		public void actionPerformed(ActionEvent actionEvent)
		{
			if (!AudioTool.recording)
			{
				AudioTool.recording = true;
				AudioTool.recorder.startRecording();
				AudioTool.access_400().setText("Stop");
				AudioTool.access_000().setEnabled(true);
			}
			else
			{
				AudioTool.recording = false;
				AudioTool.recorder.stopRecording();
				AudioTool.audio.setAudioData(AudioTool.access_500(AudioTool.recorder));
				AudioTool.access_400().setText("Record");
				AudioTool.player.play(AudioTool.audioPanel.getSelectionStart(), AudioTool.audioPanel.getSelectionEnd());
			}
		}
	}
}
