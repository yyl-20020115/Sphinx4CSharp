using java.util;
using javax.swing.@event;

namespace edu.cmu.sphinx.tools.audio
{
	internal sealed class CepstrumPanel_1 : java.lang.Object, ChangeListener, EventListener
	{	
		internal CepstrumPanel_1(CepstrumPanel cepstrumPanel)
		{
			this_0 = cepstrumPanel;
		}

		public void stateChanged(ChangeEvent changeEvent)
		{
			this.this_0.computeCepstrum();
		}

		internal CepstrumPanel this_0;
	}
}
