using System;

using System.Runtime.Serialization;
using System.Security.Permissions;
using IKVM.Attributes;
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
		
		public static void __<clinit>()
		{
		}

		[LineNumberTable(new byte[]
		{
			159,
			163,
			232,
			126,
			104,
			104,
			104
		})]
		
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

		[LineNumberTable(new byte[]
		{
			159,
			176,
			103,
			108,
			107
		})]
		
		public virtual void start()
		{
			this.quit = false;
			this.thread = new VUMeterPanel.VUMeterPanelThread(this);
			this.thread.start();
		}

		[LineNumberTable(new byte[]
		{
			45,
			127,
			8,
			159,
			8,
			118,
			151,
			103,
			103,
			139,
			107,
			152,
			104,
			105,
			28,
			232,
			69,
			104,
			155
		})]
		
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

		[LineNumberTable(new byte[]
		{
			70,
			101,
			109,
			101,
			109,
			101,
			141,
			107
		})]
		
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

		[LineNumberTable(new byte[]
		{
			159,
			183,
			103,
			98,
			131,
			107,
			141,
			34,
			97,
			130
		})]
		
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

		[LineNumberTable(new byte[]
		{
			25,
			135,
			104,
			135
		})]
		
		public override void paintComponent(Graphics g)
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
			JPanel.__<clinit>();
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

		
		.
		
		internal sealed class VUMeterPanelThread : Thread
		{
			
			public static void __<clinit>()
			{
			}

			
			
			internal VUMeterPanelThread(VUMeterPanel vumeterPanel)
			{
			}

			[LineNumberTable(new byte[]
			{
				7,
				109,
				139,
				179,
				2,
				129,
				130
			})]
			
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

			
			static VUMeterPanelThread()
			{
				Thread.__<clinit>();
			}

			
			internal VUMeterPanel this_0 = vumeterPanel;
		}
	}
}
