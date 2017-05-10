using System;
using IKVM.Attributes;

namespace edu.cmu.sphinx.util
{
	public interface BatchManager
	{
		[Throws(new string[]
		{
			"java.io.IOException"
		})]
		void start();

		string getFilename();

		[Throws(new string[]
		{
			"java.io.IOException"
		})]
		BatchItem getNextItem();

		[Throws(new string[]
		{
			"java.io.IOException"
		})]
		void stop();
	}
}
