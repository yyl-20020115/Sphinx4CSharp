using System;

using IKVM.Attributes;
using java.lang;

namespace edu.cmu.sphinx.util
{
	public class Complex : java.lang.Object
	{
		[LineNumberTable(new byte[]
		{
			159,
			168,
			104,
			102
		})]
		
		public Complex()
		{
			this.reset();
		}

		public virtual void set(double real, double imaginary)
		{
			this.real = real;
			this.imaginary = imaginary;
		}

		[LineNumberTable(new byte[]
		{
			44,
			116,
			116
		})]
		public virtual void addComplex(Complex a, Complex b)
		{
			this.real = a.real + b.real;
			this.imaginary = a.imaginary + b.imaginary;
		}

		public virtual void reset()
		{
			this.real = (double)0f;
			this.imaginary = (double)0f;
		}

		[LineNumberTable(new byte[]
		{
			159,
			185,
			104,
			106
		})]
		
		public Complex(double real, double imaginary)
		{
			this.set(real, imaginary);
		}

		[LineNumberTable(new byte[]
		{
			94,
			112,
			112
		})]
		public virtual void scaleComplex(Complex a, double b)
		{
			this.real = a.real / b;
			this.imaginary = a.imaginary / b;
		}

		public virtual double squaredMagnitudeComplex()
		{
			return this.real * this.real + this.imaginary * this.imaginary;
		}

		[LineNumberTable(new byte[]
		{
			68,
			127,
			3,
			127,
			3
		})]
		public virtual void multiplyComplex(Complex a, Complex b)
		{
			this.real = a.real * b.real - a.imaginary * b.imaginary;
			this.imaginary = a.real * b.imaginary + a.imaginary * b.real;
		}

		[LineNumberTable(new byte[]
		{
			56,
			116,
			116
		})]
		public virtual void subtractComplex(Complex a, Complex b)
		{
			this.real = a.real - b.real;
			this.imaginary = a.imaginary - b.imaginary;
		}

		[LineNumberTable(new byte[]
		{
			159,
			176,
			104,
			109
		})]
		
		public Complex(double real)
		{
			this.set(real, (double)0f);
		}

		public virtual double getReal()
		{
			return this.real;
		}

		public virtual double getImaginary()
		{
			return this.imaginary;
		}

		[LineNumberTable(new byte[]
		{
			81,
			127,
			3,
			127,
			3,
			109
		})]
		
		public virtual void divideComplex(Complex a, Complex b)
		{
			this.real = a.real * b.real + a.imaginary * b.imaginary;
			this.imaginary = a.imaginary * b.real - a.real * b.imaginary;
			this.scaleComplex(this, b.squaredMagnitudeComplex());
		}

		
		
		public override string toString()
		{
			return new StringBuilder().append("(").append(this.real).append(", ").append(this.imaginary).append(')').toString();
		}

		private double real;

		private double imaginary;
	}
}
