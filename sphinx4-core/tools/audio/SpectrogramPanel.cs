using System;
using System.Runtime.Serialization;
using System.Security.Permissions;
using edu.cmu.sphinx.frontend;
using edu.cmu.sphinx.frontend.util;
using IKVM.Runtime;
using java.awt;
using java.awt.image;
using java.lang;
using java.util;
using javax.swing;

namespace edu.cmu.sphinx.tools.audio
{
	[Serializable]
	public class SpectrogramPanel : JPanel
	{	
		protected internal virtual void zoomSet(float zoom)
		{
			this.zoom = zoom;
			if (this.spectrogram != null)
			{
				int width = this.spectrogram.getWidth();
				int height = this.spectrogram.getHeight();
				ReplicateScaleFilter replicateScaleFilter = new ReplicateScaleFilter(ByteCodeHelper.f2i(zoom * (float)width), height);
				this.scaledSpectrogram = this.createImage(new FilteredImageSource(this.spectrogram.getSource(), replicateScaleFilter));
				Dimension dimension = new Dimension(ByteCodeHelper.f2i((float)width * zoom), height);
				this.setMinimumSize(dimension);
				this.setMaximumSize(dimension);
				this.setPreferredSize(dimension);
				this.repaint();
			}
		}
		
		public SpectrogramPanel(FrontEnd frontEnd, StreamDataSource dataSource, AudioData audioData)
		{
			this.zoom = 1f;
			this.audio = audioData;
			this.frontEnd = frontEnd;
			this.dataSource = dataSource;
			this.audio.addChangeListener(new SpectrogramPanel_1(this));
		}
	
		protected internal virtual void computeSpectrogram()
		{
			try
			{
				AudioDataInputStream audioDataInputStream = new AudioDataInputStream(this.audio);
				this.dataSource.setInputStream(audioDataInputStream);
				ArrayList arrayList = new ArrayList();
				double num = double.Epsilon;
				Data data = this.frontEnd.getData();
				int i;
				while (!(data is DataEndSignal))
				{
					if (data is DoubleData)
					{
						double[] values = ((DoubleData)data).getValues();
						double[] array = new double[values.Length];
						for (i = 0; i < array.Length; i++)
						{
							array[i] = java.lang.Math.max(java.lang.Math.log(values[i]), (double)0f);
							if (array[i] > num)
							{
								num = array[i];
							}
						}
						arrayList.add(array);
					}
					data = this.frontEnd.getData();
				}
				audioDataInputStream.close();
				int num2 = arrayList.size();
				int num3 = ((double[])arrayList.get(0)).Length;
				i = num3 - 1;
				Dimension dimension = new Dimension(num2, num3);
				this.setMinimumSize(dimension);
				this.setMaximumSize(dimension);
				this.setPreferredSize(dimension);
				this.spectrogram = new BufferedImage(num2, num3, 1);
				double num4 = (255.0 + this.offsetFactor) / num;
				for (int j = 0; j < num2; j++)
				{
					double[] array2 = (double[])arrayList.get(j);
					for (int k = i; k >= 0; k += -1)
					{
						int num5 = ByteCodeHelper.d2i(array2[k] * num4 - this.offsetFactor);
						num5 = java.lang.Math.max(num5, 0);
						num5 = 255 - num5;
						int num6 = (num5 << 16 & 16711680) | (num5 << 8 & 65280) | (num5 & 255);
						this.spectrogram.setRGB(j, i - k, num6);
					}
				}
				ReplicateScaleFilter replicateScaleFilter = new ReplicateScaleFilter(ByteCodeHelper.f2i(this.zoom * (float)num2), num3);
				this.scaledSpectrogram = this.createImage(new FilteredImageSource(this.spectrogram.getSource(), replicateScaleFilter));
				Dimension size = this.getSize();
				this.repaint(0L, 0, 0, size.width - 1, size.height - 1);
			}
			catch (System.Exception ex)
			{
				Throwable.instancehelper_printStackTrace(ex);

			}
		}
		
		public SpectrogramPanel()
		{
			this.zoom = 1f;
		}
		
		public virtual void setOffsetFactor(double offsetFactor)
		{
			this.offsetFactor = offsetFactor;
			this.computeSpectrogram();
		}

		public override void paint(Graphics g)
		{
			Dimension size = this.getSize();
			g.setColor(Color.WHITE);
			g.fillRect(0, 0, size.width - 1, size.height - 1);
			if (this.spectrogram != null)
			{
				g.drawImage(this.scaledSpectrogram, 0, 0, null);
			}
		}

		[PermissionSet(SecurityAction.Demand, XML = "<PermissionSet class=\"System.Security.PermissionSet\"\nversion=\"1\">\n<IPermission class=\"System.Security.Permissions.SecurityPermission, mscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089\"\nversion=\"1\"\nFlags=\"SerializationFormatter\"/>\n</PermissionSet>\n")]
		protected SpectrogramPanel(SerializationInfo serializationInfo, StreamingContext streamingContext) : base(serializationInfo, streamingContext)
		{
		}

		protected internal BufferedImage spectrogram;

		protected internal Image scaledSpectrogram;

		protected internal float zoom;

		protected internal double offsetFactor;

		protected internal AudioData audio;

		protected internal FrontEnd frontEnd;

		protected internal StreamDataSource dataSource;
	}
}
