using java.awt.@event;
using java.io;
using java.lang;
using java.util;

namespace edu.cmu.sphinx.tools.audio
{

	internal sealed class AudioTool_2 : java.lang.Object, ActionListener, EventListener
	{
		internal AudioTool_2()
		{
		}
		
		public void actionPerformed(ActionEvent actionEvent)
		{
			if (AudioTool.filename != null && !java.lang.String.instancehelper_isEmpty(AudioTool.filename))
			{
				IOException ex2;
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
}
