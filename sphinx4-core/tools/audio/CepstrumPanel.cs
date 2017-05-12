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
	[System.Serializable]
	public class CepstrumPanel : JPanel
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
		
		public CepstrumPanel(FrontEnd frontEnd, StreamDataSource dataSource, AudioData audioData)
		{
			this.zoom = 1f;
			this.audio = audioData;
			this.frontEnd = frontEnd;
			this.dataSource = dataSource;
			this.audio.addChangeListener(new CepstrumPanel_1(this));
		}
		
		protected internal virtual void computeCepstrum()
		{
			try
			{
				AudioDataInputStream audioDataInputStream = new AudioDataInputStream(this.audio);
				this.dataSource.setInputStream(audioDataInputStream);
				ArrayList arrayList = new ArrayList();
				float[] array = new float[100];
				Arrays.fill(array, float.Epsilon);
				Data data = this.frontEnd.getData();
				int i;
				while (!(data is DataEndSignal))
				{
					if (data is FloatData)
					{
						float[] values = ((FloatData)data).getValues();
						float[] array2 = new float[values.Length];
						for (i = 0; i < array2.Length; i++)
						{
							array2[i] = values[i];
							if (java.lang.Math.abs(array2[i]) > array[i])
							{
								array[i] = java.lang.Math.abs(array2[i]);
							}
						}
						arrayList.add(array2);
					}
					if (data is DoubleData)
					{
						double[] values2 = ((DoubleData)data).getValues();
						float[] array2 = new float[values2.Length];
						for (i = 0; i < array2.Length; i++)
						{
							array2[i] = (float)values2[i];
							if (java.lang.Math.abs(array2[i]) > array[i])
							{
								array[i] = java.lang.Math.abs(array2[i]);
							}
						}
						arrayList.add(array2);
					}
					data = this.frontEnd.getData();
				}
				audioDataInputStream.close();
				int num = arrayList.size();
				int num2 = ((float[])arrayList.get(0)).Length;
				i = num2 * 10;
				Dimension dimension = new Dimension(num, i);
				this.setMinimumSize(dimension);
				this.setMaximumSize(dimension);
				this.setPreferredSize(dimension);
				this.spectrogram = new BufferedImage(num, i, 1);
				for (int j = 0; j < num; j++)
				{
					float[] array3 = (float[])arrayList.get(j);
					for (int k = num2 - 1; k >= 0; k --)
					{
						int num3 = 127 - ByteCodeHelper.f2i(array3[k] / array[k] * 127f);
						int num4 = (num3 << 16 & 16711680) | (num3 << 8 & 65280) | (num3 & 255);
						for (int l = 0; l < 10; l++)
						{
							this.spectrogram.setRGB(j, i - 1 - k * 10 - l, num4);
						}
					}
				}
				ReplicateScaleFilter replicateScaleFilter = new ReplicateScaleFilter(ByteCodeHelper.f2i(this.zoom * (float)num), i);
				this.scaledSpectrogram = this.createImage(new FilteredImageSource(this.spectrogram.getSource(), replicateScaleFilter));
				Dimension size = this.getSize();
				this.repaint(0L, 0, 0, size.width - 1, size.height - 1);
			}
			catch (System.Exception ex)
			{
				Throwable.instancehelper_printStackTrace(ex);
			}
		}
		
		public CepstrumPanel()
		{
			this.zoom = 1f;
		}
		
		public virtual void setOffsetFactor(double offsetFactor)
		{
			this.offsetFactor = offsetFactor;
			this.computeCepstrum();
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
		
		[System.Security.Permissions.PermissionSet(System.Security.Permissions.SecurityAction.Demand, XML = "<PermissionSet class=\"System.Security.PermissionSet\"\nversion=\"1\">\n<IPermission class=\"System.Security.Permissions.SecurityPermission, mscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089\"\nversion=\"1\"\nFlags=\"SerializationFormatter\"/>\n</PermissionSet>\n")]
		protected CepstrumPanel(System.Runtime.Serialization.SerializationInfo serializationInfo, System.Runtime.Serialization.StreamingContext streamingContext) : base(serializationInfo, streamingContext)
		{
		}

		protected internal BufferedImage spectrogram;

		protected internal Image scaledSpectrogram;

		protected internal float zoom;

		protected internal double offsetFactor;

		protected internal AudioData audio;

		protected internal FrontEnd frontEnd;

		protected internal StreamDataSource dataSource;

		internal const int HSCALE = 10;
	}
}
