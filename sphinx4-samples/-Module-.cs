using System;
using edu.cmu.sphinx.demo;
using ikvm.runtime;
using java.lang;

internal class Program
{
	[STAThread]
	public static int main(string[] array)
	{
		int result = 0;
		try
		{
			Startup.enterMainThread();
			DemoRunner.main(Startup.glob(array, 0));
		}
		catch (System.Exception ex)
		{
			System.Exception ex2 = Util.mapException(ex);
			Thread thread = Thread.currentThread();
			thread.getThreadGroup().uncaughtException(thread, ex2);
			result = 1;
		}
		finally
		{
			Startup.exitMainThread();
		}
		return result;
	}
}
