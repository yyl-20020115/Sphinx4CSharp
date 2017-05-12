using java.lang;

namespace edu.cmu.sphinx.util
{
	public class Complex : Object
	{		
		public Complex()
		{
			this.reset();
		}

		public virtual void set(double real, double imaginary)
		{
			this.real = real;
			this.imaginary = imaginary;
		}

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
		
		public Complex(double real, double imaginary)
		{
			this.set(real, imaginary);
		}

		public virtual void scaleComplex(Complex a, double b)
		{
			this.real = a.real / b;
			this.imaginary = a.imaginary / b;
		}

		public virtual double squaredMagnitudeComplex()
		{
			return this.real * this.real + this.imaginary * this.imaginary;
		}

		public virtual void multiplyComplex(Complex a, Complex b)
		{
			this.real = a.real * b.real - a.imaginary * b.imaginary;
			this.imaginary = a.real * b.imaginary + a.imaginary * b.real;
		}

		public virtual void subtractComplex(Complex a, Complex b)
		{
			this.real = a.real - b.real;
			this.imaginary = a.imaginary - b.imaginary;
		}
		
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
