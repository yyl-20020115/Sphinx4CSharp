using IKVM.Runtime;
using java.awt;
using java.awt.@event;
using java.util;
using javax.swing;

namespace edu.cmu.sphinx.tools.audio
{
	[System.Serializable]
	public class AudioPanel : JPanel, MouseMotionListener, EventListener, MouseListener
	{		
		internal static AudioData access_000(AudioPanel audioPanel)
		{
			return audioPanel.audio;
		}

		internal static float access_100(AudioPanel audioPanel)
		{
			return audioPanel.xScale;
		}
		
		internal static float access_200(AudioPanel audioPanel)
		{
			return audioPanel.yScale;
		}

		internal static float[] access_302(AudioPanel audioPanel, float[] result)
		{
			audioPanel.labelTimes = result;
			return result;
		}
		
		internal static string[] access_402(AudioPanel audioPanel, string[] result)
		{
			audioPanel.labels = result;
			return result;
		}

		public virtual void setSelectionStart(int newStart)
		{
			this.selectionStart = newStart;
			if (this.selectionEnd != -1 && this.selectionEnd < this.selectionStart)
			{
				this.selectionEnd = this.selectionStart;
			}
		}

		public virtual void setSelectionEnd(int newEnd)
		{
			this.selectionEnd = newEnd;
			if (this.selectionEnd != -1 && this.selectionStart > this.selectionEnd)
			{
				this.selectionStart = this.selectionEnd;
			}
		}
		
		private JViewport getViewport()
		{
			Container parent = this.getParent();
			if (parent is JViewport)
			{
				Container parent2 = parent.getParent();
				if (parent2 is JScrollPane)
				{
					JScrollPane jscrollPane = (JScrollPane)parent2;
					JViewport viewport = jscrollPane.getViewport();
					if (viewport == null || viewport.getView() != this)
					{
						return null;
					}
					return viewport;
				}
			}
			return null;
		}

		public virtual int getSelectionStart()
		{
			return this.selectionStart;
		}

		public virtual int getSelectionEnd()
		{
			return this.selectionEnd;
		}
		
		public AudioPanel(AudioData audioData, float scaleX, float scaleY)
		{
			this.selectionStart = -1;
			this.selectionEnd = -1;
			this.audio = audioData;
			this.labelTimes = new float[0];
			this.labels = new string[0];
			this.xScale = scaleX;
			this.yScale = scaleY;
			this.originalXScale = this.xScale;
			int num = ByteCodeHelper.f2i((float)this.audio.getAudioData().Length * this.xScale);
			int num2 = ByteCodeHelper.f2i(65536f * this.yScale);
			this.setPreferredSize(new Dimension(num, num2));
			this.setBackground(Color.white);
			this.audio.addChangeListener(new AudioPanel_1(this));
			this.addMouseMotionListener(this);
			this.addMouseListener(this);
			this.setFocusable(true);
			this.requestFocus();
		}
		
		public virtual void setLabels(float[] labelTimes, string[] labels)
		{
			this.labelTimes = labelTimes;
			this.labels = labels;
			this.repaint();
		}
		
		protected internal virtual void zoomSet(float zoom)
		{
			this.xScale = this.originalXScale * zoom;
			int num = ByteCodeHelper.f2i((float)this.audio.getAudioData().Length * this.xScale);
			int num2 = ByteCodeHelper.f2i(65536f * this.yScale);
			this.setPreferredSize(new Dimension(num, num2));
			this.revalidate();
			this.repaint();
		}
	
		protected override void paintComponent(Graphics g)
		{
			base.paintComponent(g);
			Dimension size = this.getSize();
			int num = size.height / 2;
			short[] audioData = this.audio.getAudioData();
			JViewport viewport = this.getViewport();
			int num2;
			int num3;
			if (viewport != null)
			{
				Rectangle viewRect = viewport.getViewRect();
				num2 = ByteCodeHelper.d2i(viewRect.getX());
				num3 = ByteCodeHelper.d2i(viewRect.getWidth());
			}
			else
			{
				num2 = 0;
				num3 = ByteCodeHelper.f2i((float)audioData.Length * this.xScale);
			}
			g.setColor(Color.WHITE);
			g.fillRect(num2, 0, num3, size.height - 1);
			int num4 = java.lang.Math.max(0, this.getSelectionStart());
			int num5 = ByteCodeHelper.f2i((float)num4 * this.xScale);
			num4 = this.getSelectionEnd();
			if (num4 == -1)
			{
				num4 = audioData.Length - 1;
			}
			int num6 = ByteCodeHelper.f2i((float)num4 * this.xScale);
			g.setColor(Color.LIGHT_GRAY);
			g.fillRect(num5, 0, num6 - num5, size.height - 1);
			int[] array = new int[num3];
			int[] array2 = new int[num3];
			for (int i = 0; i < num3; i++)
			{
				array[i] = num2;
				num4 = ByteCodeHelper.f2i((float)num2 / this.xScale);
				if (num4 >= audioData.Length)
				{
					break;
				}
				array2[i] = num - ByteCodeHelper.f2i((float)audioData[num4] * this.yScale);
				num2++;
			}
			g.setColor(Color.RED);
			g.drawPolyline(array, array2, num3);
			for (int i = 0; i < this.labelTimes.Length; i++)
			{
				num2 = ByteCodeHelper.f2i(this.xScale * this.labelTimes[i] * this.audio.getAudioFormat().getSampleRate());
				g.drawLine(num2, 0, num2, size.height - 1);
				g.drawString(this.labels[i], num2 + 5, size.height - 5);
			}
		}
		
		public virtual void crop()
		{
			short[] audioData = this.audio.getAudioData();
			int num = java.lang.Math.max(0, this.getSelectionStart());
			int num2 = this.getSelectionEnd();
			if (num2 == -1)
			{
				num2 = audioData.Length;
			}
			this.audio.setAudioData(Arrays.copyOfRange(audioData, num, num2));
			this.setSelectionStart(-1);
			this.setSelectionEnd(-1);
		}
		
		public virtual void selectAll()
		{
			this.setSelectionStart(-1);
			this.setSelectionEnd(-1);
			this.repaint();
		}
		
		public virtual void mousePressed(MouseEvent evt)
		{
			this.xDragStart = java.lang.Math.max(0, evt.getX());
			this.setSelectionStart(ByteCodeHelper.f2i((float)this.xDragStart / this.xScale));
			this.setSelectionEnd(ByteCodeHelper.f2i((float)this.xDragStart / this.xScale));
			this.repaint();
		}
		
		public virtual void mouseDragged(MouseEvent evt)
		{
			this.xDragEnd = evt.getX();
			if (this.xDragEnd < ByteCodeHelper.f2i((float)this.getSelectionStart() * this.xScale))
			{
				this.setSelectionStart(ByteCodeHelper.f2i((float)this.xDragEnd / this.xScale));
			}
			else
			{
				this.setSelectionEnd(ByteCodeHelper.f2i((float)this.xDragEnd / this.xScale));
			}
			this.repaint();
		}

		public virtual void mouseReleased(MouseEvent evt)
		{
		}

		public virtual void mouseMoved(MouseEvent evt)
		{
		}

		public virtual void mouseEntered(MouseEvent evt)
		{
		}

		public virtual void mouseExited(MouseEvent evt)
		{
		}

		public virtual void mouseClicked(MouseEvent evt)
		{
		}

		[System.Security.Permissions.PermissionSet(System.Security.Permissions.SecurityAction.Demand, XML = "<PermissionSet class=\"System.Security.PermissionSet\"\nversion=\"1\">\n<IPermission class=\"System.Security.Permissions.SecurityPermission, mscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089\"\nversion=\"1\"\nFlags=\"SerializationFormatter\"/>\n</PermissionSet>\n")]
		protected AudioPanel(System.Runtime.Serialization.SerializationInfo serializationInfo, System.Runtime.Serialization.StreamingContext streamingContext) : base(serializationInfo, streamingContext)
		{
		}

		private AudioData audio;

		private float[] labelTimes;

		private string[] labels;

		private float xScale;

		private float yScale;
		
		private float originalXScale;

		private int xDragStart;

		private int xDragEnd;

		protected internal int selectionStart;

		protected internal int selectionEnd;
	}
}
