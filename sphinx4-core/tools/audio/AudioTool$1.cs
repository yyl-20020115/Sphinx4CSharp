using java.awt.@event;
using java.lang;
using java.util;

namespace edu.cmu.sphinx.tools.audio
{
	internal sealed class AudioTool_1 : java.lang.Object, ActionListener, EventListener
	{
		internal AudioTool_1()
		{
		}
		
		public void actionPerformed(ActionEvent actionEvent)
		{
			AudioTool.getFilename("Open...", 0);
			if (AudioTool.filename == null || java.lang.String.instancehelper_isEmpty(AudioTool.filename))
			{
				return;
			}
			try
			{
				AudioTool.getAudioFromFile(AudioTool.filename);
			}
			catch (System.Exception ex)
			{
				Throwable.instancehelper_printStackTrace(ex);
			}
		}
	}
}
