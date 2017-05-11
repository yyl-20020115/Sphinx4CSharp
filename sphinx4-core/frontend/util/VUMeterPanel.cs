using System;
using System.Runtime.Serialization;
using System.Security.Permissions;
using ikvm.@internal;
using IKVM.Runtime;
using java.awt;
using java.lang;
using javax.swing;

namespace edu.cmu.sphinx.frontend.util
{
	[Serializable]
	public class VUMeterPanel : JPanel
	{		
		public VUMeterPanel()
		{
			this.numberOfLights = 50;
			this.greenLevel = 15;
			this.yellowLevel = 35;
			this.redLevel = 45;
		}

		public virtual void setVu(VUMeter vu)
		{
			this.vu = vu;
		}

		public virtual void start()
		{
			this.quit = false;
			this.thread = new VUMeterPanel.VUMeterPanelThread(this);
			this.thread.start();
		}
		
		private void paintVUMeter(Graphics graphics)
		{
			int num = ByteCodeHelper.d2i(this.vu.getRmsDB() / this.vu.getMaxDB() * 50.0);
			int num2 = ByteCodeHelper.d2i(this.vu.getPeakDB() / this.vu.getMaxDB() * 50.0);
			if (!VUMeterPanel.assertionsDisabled && num < 0)
			{
				
				throw new AssertionError();
			}
			if (!VUMeterPanel.assertionsDisabled && num >= 50)
			{
				
				throw new AssertionError();
			}
			Dimension size = this.getSize();
			int width = size.width;
			int num3 = size.height / 50;
			graphics.setColor(Color.BLACK);
			graphics.fillRect(0, 0, size.width - 1, size.height - 1);
			for (int i = 0; i < num; i++)
			{
				this.setLevelColor(i, graphics);
				graphics.fillRect(1, size.height - i * num3 + 1, width - 2, num3 - 2);
			}
			this.setLevelColor(num2, graphics);
			graphics.fillRect(1, size.height - num2 * num3 + 1, width - 2, num3 - 2);
		}
		
		private void setLevelColor(int num, Graphics graphics)
		{
			if (num < 15)
			{
				graphics.setColor(Color.BLUE);
			}
			else if (num < 35)
			{
				graphics.setColor(Color.GREEN);
			}
			else if (num < 45)
			{
				graphics.setColor(Color.YELLOW);
			}
			else
			{
				graphics.setColor(Color.RED);
			}
		}
		
		public virtual void stop()
		{
			this.quit = true;
			int num = 0;
			while (num == 0)
			{
				try
				{
					this.thread.join();
					num = 1;
				}
				catch (InterruptedException ex)
				{
				}
			}
		}

		protected override void paintComponent(Graphics g)
		{
			base.paintComponent(g);
			if (this.vu != null)
			{
				this.paintVUMeter(g);
			}
		}

		public virtual VUMeter getVu()
		{
			return this.vu;
		}

		
		static VUMeterPanel()
		{
			VUMeterPanel.assertionsDisabled = !ClassLiteral<VUMeterPanel>.Value.desiredAssertionStatus();
		}

		[PermissionSet(SecurityAction.Demand, XML = "<PermissionSet class=\"System.Security.PermissionSet\"\nversion=\"1\">\n<IPermission class=\"System.Security.Permissions.SecurityPermission, mscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089\"\nversion=\"1\"\nFlags=\"SerializationFormatter\"/>\n</PermissionSet>\n")]
		protected VUMeterPanel(SerializationInfo serializationInfo, StreamingContext streamingContext) : base(serializationInfo, streamingContext)
		{
		}

		internal VUMeter vu;

		internal bool quit;

		internal Thread thread;

		
		internal int numberOfLights;

		
		internal int greenLevel;

		
		internal int yellowLevel;

		
		internal int redLevel;

		
		internal static bool assertionsDisabled;
		
		internal sealed class VUMeterPanelThread : Thread
		{			
			internal VUMeterPanelThread(VUMeterPanel vumeterPanel)
			{
				this.this_0 = vumeterPanel;
			}
			
			public override void run()
			{
				while (!this.this_0.quit)
				{
					this.this_0.repaint();
					try
					{
						Thread.sleep((long)((ulong)10));
					}
					catch (InterruptedException ex)
					{
					}
				}
			}

			internal VUMeterPanel this_0;
		}
	}
}
