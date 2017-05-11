using java.util;
using javax.swing.@event;

namespace edu.cmu.sphinx.tools.audio
{
	internal sealed class SpectrogramPanel_1 : java.lang.Object, ChangeListener, EventListener
	{
		internal SpectrogramPanel_1(SpectrogramPanel spectrogramPanel)
		{
			this_0 = spectrogramPanel;
		}
		
		public void stateChanged(ChangeEvent changeEvent)
		{
			this.this_0.computeSpectrogram();
		}

		
		internal SpectrogramPanel this_0;
	}
}
