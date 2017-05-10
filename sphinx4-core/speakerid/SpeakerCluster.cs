using System;

using IKVM.Attributes;
using java.lang;
using java.util;
using org.apache.commons.math3.linear;

namespace edu.cmu.sphinx.speakerid
{
	public class SpeakerCluster : java.lang.Object
	{
		
		
		public virtual java.lang.Boolean addSegment(Segment s)
		{
			return java.lang.Boolean.valueOf(this.segmentSet.add(s));
		}

		public virtual Array2DRowRealMatrix getFeatureMatrix()
		{
			return this.featureMatrix;
		}

		public virtual double getBicValue()
		{
			return this.bicValue;
		}

		public virtual void setBicValue(double bicValue)
		{
			this.bicValue = bicValue;
		}

		[LineNumberTable(new byte[]
		{
			159,
			181,
			104,
			107
		})]
		
		public SpeakerCluster()
		{
			this.segmentSet = new TreeSet();
		}

		[LineNumberTable(new byte[]
		{
			159,
			185,
			104,
			107,
			118,
			105,
			104
		})]
		
		public SpeakerCluster(Segment s, Array2DRowRealMatrix featureMatrix, double bicValue)
		{
			this.segmentSet = new TreeSet();
			Array2DRowRealMatrix.__<clinit>();
			this.featureMatrix = new Array2DRowRealMatrix(featureMatrix.getData());
			this.bicValue = bicValue;
			this.addSegment(s);
		}

		[LineNumberTable(new byte[]
		{
			0,
			104,
			107,
			123,
			108,
			104,
			116
		})]
		
		public SpeakerCluster(SpeakerCluster c)
		{
			this.segmentSet = new TreeSet();
			Array2DRowRealMatrix.__<clinit>();
			this.featureMatrix = new Array2DRowRealMatrix(c.getFeatureMatrix().getData());
			Iterator iterator = c.segmentSet.iterator();
			while (iterator.hasNext())
			{
				this.addSegment((Segment)iterator.next());
			}
		}

		
		public virtual TreeSet getSegments()
		{
			return this.segmentSet;
		}

		
		[LineNumberTable(new byte[]
		{
			13,
			108,
			102,
			104,
			111
		})]
		
		public virtual ArrayList getArrayOfSegments()
		{
			Iterator iterator = this.segmentSet.iterator();
			ArrayList arrayList = new ArrayList();
			while (iterator.hasNext())
			{
				arrayList.add(iterator.next());
			}
			return arrayList;
		}

		
		
		public virtual java.lang.Boolean removeSegment(Segment s)
		{
			return java.lang.Boolean.valueOf(this.segmentSet.remove(s));
		}

		
		[LineNumberTable(new byte[]
		{
			40,
			108,
			116,
			103,
			103,
			98,
			102,
			104,
			107,
			109,
			115,
			115,
			110,
			155,
			100,
			137,
			136
		})]
		
		public virtual ArrayList getSpeakerIntervals()
		{
			Iterator iterator = this.segmentSet.iterator();
			new Segment(0, 0);
			Segment segment = (Segment)iterator.next();
			segment.getStartTime();
			segment.getLength();
			int num = 0;
			ArrayList arrayList = new ArrayList();
			arrayList.add(segment);
			while (iterator.hasNext())
			{
				Segment segment2 = (Segment)iterator.next();
				int startTime = ((Segment)arrayList.get(num)).getStartTime();
				int length = ((Segment)arrayList.get(num)).getLength();
				if (startTime + length == segment2.getStartTime())
				{
					arrayList.set(num, new Segment(startTime, length + segment2.getLength()));
				}
				else
				{
					num++;
					arrayList.add(segment2);
				}
			}
			return arrayList;
		}

		[Throws(new string[]
		{
			"java.lang.NullPointerException"
		})]
		[LineNumberTable(new byte[]
		{
			63,
			99,
			107,
			108,
			104,
			120,
			145,
			120,
			108,
			104,
			115,
			98,
			123,
			109,
			118
		})]
		
		public virtual void mergeWith(SpeakerCluster target)
		{
			if (target == null)
			{
				
				throw new NullPointerException();
			}
			Iterator iterator = target.segmentSet.iterator();
			while (iterator.hasNext())
			{
				if (!this.addSegment((Segment)iterator.next()).booleanValue())
				{
					java.lang.System.@out.println("Something doesn't work in mergeWith method, Cluster class");
				}
			}
			int rowDimension = this.featureMatrix.getRowDimension() + target.getFeatureMatrix().getRowDimension();
			int columnDimension = this.featureMatrix.getColumnDimension();
			Array2DRowRealMatrix array2DRowRealMatrix = new Array2DRowRealMatrix(rowDimension, columnDimension);
			array2DRowRealMatrix.setSubMatrix(this.featureMatrix.getData(), 0, 0);
			array2DRowRealMatrix.setSubMatrix(target.getFeatureMatrix().getData(), this.featureMatrix.getRowDimension(), 0);
			this.bicValue = SpeakerIdentification.getBICValue(array2DRowRealMatrix);
			Array2DRowRealMatrix.__<clinit>();
			this.featureMatrix = new Array2DRowRealMatrix(array2DRowRealMatrix.getData());
		}

		
		private TreeSet segmentSet;

		private double bicValue;

		protected internal Array2DRowRealMatrix featureMatrix;
	}
}
