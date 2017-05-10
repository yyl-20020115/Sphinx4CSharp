using System;

using edu.cmu.sphinx.frontend;
using edu.cmu.sphinx.frontend.frequencywarp;
using edu.cmu.sphinx.frontend.transform;
using edu.cmu.sphinx.frontend.util;
using edu.cmu.sphinx.frontend.window;
using IKVM.Attributes;
using java.io;
using java.lang;
using java.util;

namespace edu.cmu.sphinx.tools.bandwidth
{
	public class BandDetector : java.lang.Object
	{		
		public BandDetector()
		{
			this.source = new AudioFileDataSource(320, null);
			RaisedCosineWindower raisedCosineWindower = new RaisedCosineWindower(0.97000002861022949, 25.625f, 10f);
			DiscreteFourierTransform discreteFourierTransform = new DiscreteFourierTransform(512, false);
			MelFrequencyFilterBank melFrequencyFilterBank = new MelFrequencyFilterBank(130.0, 6800.0, 40);
			ArrayList arrayList = new ArrayList();
			arrayList.add(this.source);
			arrayList.add(raisedCosineWindower);
			arrayList.add(discreteFourierTransform);
			arrayList.add(melFrequencyFilterBank);
			this.frontend = new FrontEnd(arrayList);
		}
		
		public virtual bool bandwidth(string file)
		{
			this.source.setAudioFile(new File(file), "");
			double[] array = new double[40];
			Data data;
			double num;
			while ((data = this.frontend.getData()) != null)
			{
				if (data is DoubleData)
				{
					num = 100000.0;
					double[] values = ((DoubleData)data).getValues();
					for (int i = 0; i < 40; i++)
					{
						num = java.lang.Math.max(num, values[i]);
					}
					if (num > 100000.0)
					{
						for (int i = 0; i < 40; i++)
						{
							array[i] = java.lang.Math.max(values[i] / num, array[i]);
						}
					}
				}
			}
			num = this.max(array, 23, 29);
			double num2 = this.max(array, 35, 39);
			return num2 < 0.02 && num > 0.5;
		}
		
		private double max(double[] array, int num, int num2)
		{
			double num3 = (double)0f;
			for (int i = num; i <= num2; i++)
			{
				num3 = java.lang.Math.max(num3, array[i]);
			}
			return num3;
		}
	
		public static void main(string[] args)
		{
			if (args.Length < 1)
			{
				java.lang.System.@out.println("Usage: Detector <filename.wav> or Detector <filelist>");
				return;
			}
			if (java.lang.String.instancehelper_endsWith(args[0], ".wav"))
			{
				BandDetector bandDetector = new BandDetector();
				java.lang.System.@out.println(new StringBuilder().append("Bandwidth for ").append(args[0]).append(" is ").append(bandDetector.bandwidth(args[0])).toString());
			}
			else
			{
				BandDetector bandDetector = new BandDetector();
				Scanner scanner = new Scanner(new File(args[0]));
				while (scanner.hasNextLine())
				{
					string text = java.lang.String.instancehelper_trim(scanner.nextLine());
					if (bandDetector.bandwidth(text))
					{
						java.lang.System.@out.println(new StringBuilder().append("Bandwidth for ").append(text).append(" is low").toString());
					}
				}
				scanner.close();
			}
		}

		internal const int bands = 40;

		internal const int highRangeStart = 35;

		internal const int highRangeEnd = 39;

		internal const int lowRangeStart = 23;

		internal const int lowRangeEnd = 29;

		internal const double noSignalLevel = 0.02;

		internal const double signalLevel = 0.5;

		internal const double lowIntensity = 100000.0;

		private FrontEnd frontend;

		private AudioFileDataSource source;
	}
}
