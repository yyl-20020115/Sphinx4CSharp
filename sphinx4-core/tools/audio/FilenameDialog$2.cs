using java.awt.@event;
using java.util;

namespace edu.cmu.sphinx.tools.audio
{
	internal sealed class FilenameDialog_2 : java.lang.Object, ActionListener, EventListener
	{
		internal FilenameDialog_2(FilenameDialog filenameDialog)
		{
			this_0 = filenameDialog;
		}
		
		public void actionPerformed(ActionEvent actionEvent)
		{
			this.this_0.setVisible(false);
		}

		internal FilenameDialog this_0;
	}
}
