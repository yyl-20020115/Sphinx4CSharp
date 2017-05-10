using System;

using IKVM.Attributes;
using ikvm.@internal;
using java.awt;
using javax.swing;

namespace edu.cmu.sphinx.frontend.util
{
	public class VUMeterMonitor : BaseDataProcessor
	{
		[LineNumberTable(new byte[]
		{
			159,
			168,
			104,
			139,
			107,
			113,
			139,
			107,
			150,
			117,
			151,
			108
		})]
		
		public VUMeterMonitor()
		{
			this.vumeter = new VUMeter();
			this.vuMeterPanel = new VUMeterPanel();
			this.vuMeterPanel.setVu(this.vumeter);
			this.vuMeterPanel.start();
			this.vuMeterDialog = new JDialog();
			this.vuMeterDialog.setBounds(100, 100, 100, 400);
			this.vuMeterDialog.getContentPane().setLayout(new BorderLayout());
			this.vuMeterDialog.getContentPane().add(this.vuMeterPanel);
			this.vuMeterDialog.setVisible(true);
		}

		public virtual JDialog getVuMeterDialog()
		{
			return this.vuMeterDialog;
		}

		[Throws(new string[]
		{
			"edu.cmu.sphinx.frontend.DataProcessingException"
		})]
		[LineNumberTable(new byte[]
		{
			159,
			187,
			172,
			104,
			156,
			104,
			140
		})]
		
		public override Data getData()
		{
			Data data = this.getPredecessor().getData();
			if (data is DataStartSignal)
			{
				this.vuMeterPanel.setVisible(FrontEndUtils.getFrontEndProcessor(this, ClassLiteral<Microphone>.Value) != null);
			}
			if (data is DoubleData)
			{
				this.vumeter.calculateVULevels(data);
			}
			return data;
		}

		[Throws(new string[]
		{
			"edu.cmu.sphinx.frontend.DataProcessingException"
		})]
		[LineNumberTable(new byte[]
		{
			18,
			223,
			5,
			102,
			135,
			102,
			108,
			167
		})]
		
		public static void main(string[] args)
		{
			Microphone microphone = new Microphone(16000, 16, 1, true, true, true, 10, false, "selectChannel", 2, "default", 6400);
			microphone.initialize();
			microphone.startRecording();
			VUMeterMonitor vumeterMonitor = new VUMeterMonitor();
			vumeterMonitor.getVuMeterDialog().setModal(true);
			vumeterMonitor.setPredecessor(microphone);
			for (;;)
			{
				vumeterMonitor.getData();
			}
		}

		
		internal VUMeter vumeter;

		
		internal VUMeterPanel vuMeterPanel;

		
		internal JDialog vuMeterDialog;
	}
}
