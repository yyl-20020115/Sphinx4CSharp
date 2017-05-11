namespace edu.cmu.sphinx.util
{
	public interface BatchManager
	{
		void start();

		string getFilename();

		BatchItem getNextItem();

		void stop();
	}
}
