using System;
using java.lang;

namespace edu.cmu.sphinx.speakerid
{
	public class Segment : java.lang.Object, Comparable
	{
		
		public virtual int compareTo(Segment @ref)
		{
			return this.startTime - @ref.startTime;
		}
		
		public Segment(Segment @ref)
		{
			this.startTime = @ref.startTime;
			this.length = @ref.length;
		}
		
		public Segment(int startTime, int length)
		{
			this.startTime = startTime;
			this.length = length;
		}
	
		public Segment(int startTime, int length, float[] features)
		{
			this.startTime = startTime;
			this.length = length;
		}
		
		public Segment()
		{
			int num = 0;
			int num2 = num;
			this.length = num;
			this.startTime = num2;
		}

		public virtual void setStartTime(int startTime)
		{
			this.startTime = startTime;
		}

		public virtual void setLength(int length)
		{
			this.length = length;
		}

		public virtual int getStartTime()
		{
			return this.startTime;
		}

		public virtual int getLength()
		{
			return this.length;
		}
		
		public virtual int equals(Segment @ref)
		{
			return (this.startTime == @ref.startTime) ? 1 : 0;
		}
		
		public override string toString()
		{
			return new StringBuilder().append(this.startTime).append(" ").append(this.length).append("\n").toString();
		}		
		
		public virtual int compareTo(object obj)
		{
			return this.compareTo((Segment)obj);
		}
		
		int IComparable.CompareTo(object obj)
		{
			return this.compareTo(obj);
		}

		public const int FEATURES_SIZE = 13;

		public const int FRAME_LENGTH = 10;

		private int startTime;

		private int length;
	}
}
