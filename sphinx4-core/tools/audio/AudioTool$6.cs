﻿using java.awt.@event;
using java.util;
using java.lang;

namespace edu.cmu.sphinx.tools.audio
{
	internal sealed class AudioTool_6 : Object, ActionListener, EventListener
	{	
		internal AudioTool_6()
		{
		}
		
		public void actionPerformed(ActionEvent actionEvent)
		{
			AudioTool.audioPanel.crop();
		}
	}
}
