using System;

using edu.cmu.sphinx.frontend;
using edu.cmu.sphinx.frontend.util;
using edu.cmu.sphinx.util.props;
using IKVM.Runtime;
using java.io;
using java.lang;
using java.net;
using java.util;
using org.apache.commons.math3.linear;
using org.apache.commons.math3.stat.correlation;

namespace edu.cmu.sphinx.speakerid
{
	public class SpeakerIdentification : java.lang.Object
	{
		public static double getBICValue(Array2DRowRealMatrix mat)
		{
			double num = (double)0f;
			EigenDecomposition eigenDecomposition = new EigenDecomposition(new Covariance(mat).getCovarianceMatrix());
			double[] realEigenvalues = eigenDecomposition.getRealEigenvalues();
			for (int i = 0; i < realEigenvalues.Length; i++)
			{
				num += java.lang.Math.log(realEigenvalues[i]);
			}
			return num * (double)(mat.getRowDimension() / 2);
		}
		
		internal virtual double getLikelihoodRatio(double num, int num2, Array2DRowRealMatrix array2DRowRealMatrix)
		{
			int num3 = 13;
			double num4 = 0.5 * ((double)num3 + 0.5 * (double)num3 * (double)(num3 + 1)) * java.lang.Math.log((double)array2DRowRealMatrix.getRowDimension()) * 2.0;
			int rowDimension = array2DRowRealMatrix.getRowDimension();
			int columnDimension = array2DRowRealMatrix.getColumnDimension();
			Array2DRowRealMatrix mat = (Array2DRowRealMatrix)array2DRowRealMatrix.getSubMatrix(0, num2 - 1, 0, columnDimension - 1);
			Array2DRowRealMatrix mat2 = (Array2DRowRealMatrix)array2DRowRealMatrix.getSubMatrix(num2, rowDimension - 1, 0, columnDimension - 1);
			double num5 = SpeakerIdentification.getBICValue(mat);
			double num6 = SpeakerIdentification.getBICValue(mat2);
			return num - num5 - num6 - num4;
		}
		
		private int getPoint(int num, int num2, int num3, Array2DRowRealMatrix array2DRowRealMatrix)
		{
			double num4 = double.NegativeInfinity;
			int columnDimension = array2DRowRealMatrix.getColumnDimension();
			int num5 = 0;
			Array2DRowRealMatrix array2DRowRealMatrix2 = (Array2DRowRealMatrix)array2DRowRealMatrix.getSubMatrix(num, num + num2 - 1, 0, columnDimension - 1);
			double num6 = SpeakerIdentification.getBICValue(array2DRowRealMatrix2);
			for (int i = 14; i < num2 - 13; i += num3)
			{
				double num7 = this.getLikelihoodRatio(num6, i, array2DRowRealMatrix2);
				if (num7 > num4)
				{
					num4 = num7;
					num5 = i;
				}
			}
			if (num4 < (double)0f)
			{
				num5 = int.MinValue;
			}
			return num5 + num;
		}
		
		private ArrayList getFeatures()
		{
			ArrayList arrayList = new ArrayList();
			try
			{
				int num = -1;
				Data data = this.frontEnd.getData();
				while (!(data is DataEndSignal))
				{
					if (data is DoubleData)
					{
						double[] values = ((DoubleData)data).getValues();
						if (num < 0)
						{
							num = values.Length;
						}
						float[] array = new float[values.Length];
						for (int i = 0; i < values.Length; i++)
						{
							array[i] = (float)values[i];
						}
						arrayList.add(array);
					}
					else if (data is FloatData)
					{
						float[] values2 = ((FloatData)data).getValues();
						if (num < 0)
						{
							num = values2.Length;
						}
						arrayList.add(values2);
					}
					data = this.frontEnd.getData();
				}
			}
			catch (System.Exception ex)
			{
				Throwable.instancehelper_printStackTrace(ex);
			}
			return arrayList;
		}

		
		public virtual ArrayList cluster(ArrayList features)
		{
			ArrayList arrayList = new ArrayList();
			Array2DRowRealMatrix array2DRowRealMatrix = this.ArrayToRealMatrix(features, features.size());
			LinkedList allChangingPoints = this.getAllChangingPoints(array2DRowRealMatrix);
			Iterator iterator = allChangingPoints.iterator();
			int num = ((Integer)iterator.next()).intValue();
			Array2DRowRealMatrix array2DRowRealMatrix2;
			while (iterator.hasNext())
			{
				int num2 = ((Integer)iterator.next()).intValue();
				Segment s = new Segment(num * 10, (num2 - num) * 10);
				array2DRowRealMatrix2 = (Array2DRowRealMatrix)array2DRowRealMatrix.getSubMatrix(num, num2 - 1, 0, 12);
				arrayList.add(new SpeakerCluster(s, array2DRowRealMatrix2, SpeakerIdentification.getBICValue(array2DRowRealMatrix2)));
				num = num2;
			}
			int num3 = arrayList.size();
			new Array2DRowRealMatrix(num3, num3);
			array2DRowRealMatrix2 = this.updateDistances(arrayList);
			for (;;)
			{
				double num4 = (double)0f;
				int num5 = -1;
				int num6 = -1;
				for (int i = 0; i < num3; i++)
				{
					for (int j = 0; j < num3; j++)
					{
						if (i != j)
						{
							num4 += array2DRowRealMatrix2.getEntry(i, j);
						}
					}
				}
				num4 /= (double)(num3 * (num3 - 1) * 4);
				for (int i = 0; i < num3; i++)
				{
					for (int j = 0; j < num3; j++)
					{
						if (array2DRowRealMatrix2.getEntry(i, j) < num4 && i != j)
						{
							num4 = array2DRowRealMatrix2.getEntry(i, j);
							num5 = i;
							num6 = j;
						}
					}
				}
				if (num5 == -1)
				{
					break;
				}
				((SpeakerCluster)arrayList.get(num5)).mergeWith((SpeakerCluster)arrayList.get(num6));
				this.updateDistances(arrayList, num5, num6, array2DRowRealMatrix2);
				arrayList.remove(num6);
				num3 += -1;
			}
			return arrayList;
		}
		
		internal virtual Array2DRowRealMatrix ArrayToRealMatrix(ArrayList arrayList, int num)
		{
			int num2 = ((float[])arrayList.get(1)).Length;
			Array2DRowRealMatrix array2DRowRealMatrix = new Array2DRowRealMatrix(num, num2);
			for (int i = 0; i < num; i++)
			{
				double[] array = new double[num2];
				for (int j = 0; j < num2; j++)
				{
					array[j] = (double)((float[])arrayList.get(i))[j];
				}
				array2DRowRealMatrix.setRow(i, array);
			}
			return array2DRowRealMatrix;
		}
		
		private LinkedList getAllChangingPoints(Array2DRowRealMatrix array2DRowRealMatrix)
		{
			LinkedList linkedList = new LinkedList();
			linkedList.add(Integer.valueOf(0));
			int rowDimension = array2DRowRealMatrix.getRowDimension();
			int num = 500;
			int num2 = 0;
			int i = num;
			while (i < rowDimension)
			{
				int point = this.getPoint(num2, i - num2 + 1, num / 10, array2DRowRealMatrix);
				if (point > 0)
				{
					num2 = point;
					i = num2 + num;
					linkedList.add(Integer.valueOf(point));
				}
				else
				{
					i += num;
				}
			}
			linkedList.add(Integer.valueOf(rowDimension));
			return linkedList;
		}
		
		internal virtual Array2DRowRealMatrix updateDistances(ArrayList arrayList)
		{
			int num = arrayList.size();
			Array2DRowRealMatrix array2DRowRealMatrix = new Array2DRowRealMatrix(num, num);
			for (int i = 0; i < num; i++)
			{
				for (int j = 0; j <= i; j++)
				{
					array2DRowRealMatrix.setEntry(i, j, this.computeDistance((SpeakerCluster)arrayList.get(i), (SpeakerCluster)arrayList.get(j)));
					array2DRowRealMatrix.setEntry(j, i, array2DRowRealMatrix.getEntry(i, j));
				}
			}
			return array2DRowRealMatrix;
		}
		
		internal virtual void updateDistances(ArrayList arrayList, int num, int num2, Array2DRowRealMatrix array2DRowRealMatrix)
		{
			int num3 = arrayList.size();
			for (int i = 0; i < num3; i++)
			{
				array2DRowRealMatrix.setEntry(i, num, this.computeDistance((SpeakerCluster)arrayList.get(i), (SpeakerCluster)arrayList.get(num)));
				array2DRowRealMatrix.setEntry(num, i, array2DRowRealMatrix.getEntry(i, num));
			}
			for (int i = num2; i < num3 - 1; i++)
			{
				for (int j = 0; j < num3; j++)
				{
					array2DRowRealMatrix.setEntry(i, j, array2DRowRealMatrix.getEntry(i + 1, j));
				}
			}
			for (int i = 0; i < num3; i++)
			{
				for (int j = num2; j < num3 - 1; j++)
				{
					array2DRowRealMatrix.setEntry(i, j, array2DRowRealMatrix.getEntry(i, j + 1));
				}
			}
		}
		
		internal virtual double computeDistance(SpeakerCluster speakerCluster, SpeakerCluster speakerCluster2)
		{
			int rowDimension = speakerCluster.getFeatureMatrix().getRowDimension() + speakerCluster2.getFeatureMatrix().getRowDimension();
			int columnDimension = speakerCluster.getFeatureMatrix().getColumnDimension();
			Array2DRowRealMatrix array2DRowRealMatrix = new Array2DRowRealMatrix(rowDimension, columnDimension);
			array2DRowRealMatrix.setSubMatrix(speakerCluster.getFeatureMatrix().getData(), 0, 0);
			array2DRowRealMatrix.setSubMatrix(speakerCluster2.getFeatureMatrix().getData(), speakerCluster.getFeatureMatrix().getRowDimension(), 0);
			double num = SpeakerIdentification.getBICValue(array2DRowRealMatrix);
			double num2 = 13.0;
			double num3 = 0.5 * (num2 + 0.5 * num2 * (num2 + (double)1f)) * java.lang.Math.log((double)array2DRowRealMatrix.getRowDimension()) * 2.0;
			return num - speakerCluster.getBicValue() - speakerCluster2.getBicValue() - num3;
		}
		
		public SpeakerIdentification()
		{
			this.__FRONTEND_NAME = "plpFrontEnd";
			URL resource = java.lang.Object.instancehelper_getClass(this).getResource("frontend.config.xml");
			this.cm = new ConfigurationManager(resource);
			this.audioSource = (StreamDataSource)this.cm.lookup("streamDataSource");
			this.frontEnd = (FrontEnd)this.cm.lookup("plpFrontEnd");
		}
				
		public virtual ArrayList cluster(InputStream stream)
		{
			this.audioSource.setInputStream(stream);
			ArrayList features = this.getFeatures();
			return this.cluster(features);
		}
		
		internal virtual void printMatrix(Array2DRowRealMatrix array2DRowRealMatrix)
		{
			for (int i = 0; i < array2DRowRealMatrix.getRowDimension(); i++)
			{
				for (int j = 0; j < array2DRowRealMatrix.getColumnDimension(); j++)
				{
					java.lang.System.@out.print(new StringBuilder().append(array2DRowRealMatrix.getEntry(i, j)).append(" ").toString());
				}
				java.lang.System.@out.println();
			}
		}
		
		public string FRONTEND_NAME
		{
			
			get
			{
				return this.__FRONTEND_NAME;
			}
			
			private set
			{
				this.__FRONTEND_NAME = value;
			}
		}

		internal string __FRONTEND_NAME;

		private FrontEnd frontEnd;

		private StreamDataSource audioSource;

		private ConfigurationManager cm;
	}
}
