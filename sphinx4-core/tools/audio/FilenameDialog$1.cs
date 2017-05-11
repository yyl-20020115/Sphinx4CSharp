using java.awt.@event;

namespace edu.cmu.sphinx.tools.audio
{
	internal sealed class FilenameDialog_1 : WindowAdapter
	{
		internal FilenameDialog_1(FilenameDialog filenameDialog)
		{
			this_0 = filenameDialog;
		}
		
		public override void windowClosing(WindowEvent windowEvent)
		{
			this.this_0.setVisible(false);
		}
		
		internal FilenameDialog this_0;
	}
}
