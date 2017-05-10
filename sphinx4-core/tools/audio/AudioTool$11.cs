using System;

using IKVM.Attributes;
using java.awt.@event;
using java.lang;
using java.util;

namespace edu.cmu.sphinx.tools.audio
{
	
	[Implements(new string[]
	{
		"java.awt.event.ActionListener"
	})]
	[EnclosingMethod("edu.cmu.sphinx.tools.audio.AudioTool", "createMenuBar", "(Ljavax.swing.JFrame;)V")]
	[SourceFile("AudioTool.java")]
	internal sealed class AudioTool_11 : java.lang.Object, ActionListener, EventListener
	{
		
		
		internal AudioTool_11()
		{
		}

		[LineNumberTable(new byte[]
		{
			161,
			52,
			103,
			102,
			107,
			111,
			141,
			102,
			106,
			116,
			111,
			116,
			37,
			165
		})]
		
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
