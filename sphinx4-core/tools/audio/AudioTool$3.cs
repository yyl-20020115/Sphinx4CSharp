using java.awt.@event;
using java.io;
using java.lang;
using java.util;

namespace edu.cmu.sphinx.tools.audio
{
	internal sealed class AudioTool_3 : java.lang.Object, ActionListener, EventListener
	{
		internal AudioTool_3()
		{
		}

		public void actionPerformed(ActionEvent actionEvent)
		{
			AudioTool.getFilename("Save As...", 1);
			if (AudioTool.filename == null || java.lang.String.instancehelper_isEmpty(AudioTool.filename))
			{
				return;
			}
			try
			{
				Utils.writeRawFile(AudioTool.audio, AudioTool.filename);
				AudioTool.access_000().setEnabled(false);
			}
			catch (IOException ex)
			{
				Throwable.instancehelper_printStackTrace(ex);
			}
		}
	}
}
