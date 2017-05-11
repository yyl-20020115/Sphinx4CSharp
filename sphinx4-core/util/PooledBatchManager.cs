using java.io;
using java.lang;
using java.net;
using java.nio.channels;
using java.util;

namespace edu.cmu.sphinx.util
{
	public class PooledBatchManager : java.lang.Object, BatchManager
	{	
		public PooledBatchManager(string filename, int skip)
		{
			this.testFileFilter = new TestFileFilter();
			this.batchFile = filename;
			this.skip = skip;
		}		
		private void @lock()
		{
			RandomAccessFile randomAccessFile = new RandomAccessFile(PooledBatchManager.lockFile, "rw");
			this._lock = randomAccessFile.getChannel().@lock();
			randomAccessFile.close();
		}
		
		private void createDirectories()
		{
			if (!PooledBatchManager.topDir.isDirectory())
			{
				PooledBatchManager.topDir.mkdir();
				PooledBatchManager.inProcessDir.mkdir();
				PooledBatchManager.completedDir.mkdir();
				PooledBatchManager.resultsDir.mkdir();
				this.createInputDirectory();
			}
		}
		
		private void redirectStdout()
		{
			string myName = this.getMyName();
			File file = File.createTempFile(myName, ".out", PooledBatchManager.resultsDir);
			FileOutputStream fileOutputStream = new FileOutputStream(file);
			PrintStream @out = new PrintStream(fileOutputStream);
			this.oldOut = java.lang.System.@out;
			java.lang.System.setOut(@out);
			java.lang.System.@out.println(new StringBuilder().append("# These results collected on ").append(this.getMyName()).toString());
		}
		
		private void unlock()
		{
			this._lock.release();
			this._lock = null;
		}

		private File getCompletedFile(File file)
		{
			return new File(PooledBatchManager.completedDir, file.getName());
		}
		
		private File getNextFile()
		{
			File[] array = PooledBatchManager.inputDir.listFiles(this.testFileFilter);
			if (array.Length > 0)
			{
				return array[0];
			}
			return null;
		}
		
		private File getProcessingFile(File file)
		{
			return new File(PooledBatchManager.inProcessDir, file.getName());
		}
		
		private BatchItem getBatchItem(File file)
		{
			List lines = BatchFile.getLines(file.getPath());
			if (lines.size() != 1)
			{
				string text = "Bad batch file size";
				
				throw new IOException(text);
			}
			string batchFileLine = (string)lines.get(0);
			return new BatchItem(BatchFile.getFilename(batchFileLine), BatchFile.getReference(batchFileLine));
		}
		
		private void closeStdout()
		{
			java.lang.System.@out.close();
			java.lang.System.setOut(this.oldOut);
		}
		
		private void createInputDirectory()
		{
			PooledBatchManager.inputDir.mkdir();
			List lines = BatchFile.getLines(this.batchFile, this.skip);
			for (int i = 0; i < lines.size(); i++)
			{
				string text = Integer.toString(i + 1);
				string text2 = (string)lines.get(i);
				this.createInputFile(PooledBatchManager.inputDir, text, text2);
			}
		}
		
		private void createInputFile(File file, string text, string text2)
		{
			File file2 = new File(file, text);
			FileOutputStream fileOutputStream = new FileOutputStream(file2);
			PrintStream printStream = new PrintStream(fileOutputStream);
			printStream.println(text2);
			printStream.close();
		}
		
		private string getMyName()
		{
			return InetAddress.getLocalHost().getHostName();
		}
		
		public virtual void start()
		{
			this.@lock();
			try
			{
				this.createDirectories();
				this.redirectStdout();
			}
			finally
			{
				this.unlock();
			}
		}
		
		public virtual BatchItem getNextItem()
		{
			this.@lock();
			BatchItem batchItem;
			try
			{
				File file;
				if (this.processingFile != null)
				{
					file = this.getCompletedFile(this.processingFile);
					this.processingFile.renameTo(file);
					this.processingFile = null;
				}
				file = this.getNextFile();
				if (file != null)
				{
					this.processingFile = this.getProcessingFile(file);
					file.renameTo(this.processingFile);
					java.lang.System.@out.println(new StringBuilder().append("Processing: ").append(this.processingFile).toString());
					batchItem = this.getBatchItem(this.processingFile);
					goto IL_98;
				}
			}
			catch
			{
				this.unlock();
				throw;
			}
			try
			{
			}
			finally
			{
				this.unlock();
			}
			return null;
			IL_98:
			this.unlock();
			return batchItem;
		}
		
		public virtual void stop()
		{
			this.@lock();
			try
			{
				this.closeStdout();
			}
			finally
			{
				this.unlock();
			}
		}

		public virtual string getFilename()
		{
			return this.batchFile;
		}

		static PooledBatchManager()
		{
			PooledBatchManager.inputDir = new File(PooledBatchManager.topDir, "ToRun");
			PooledBatchManager.inProcessDir = new File(PooledBatchManager.topDir, "InProcess");
			PooledBatchManager.completedDir = new File(PooledBatchManager.topDir, "Completed");
			PooledBatchManager.resultsDir = new File(PooledBatchManager.topDir, "Results");
			PooledBatchManager.lockFile = new File(".lock");
		}

		private string batchFile;

		private int skip;

		private File processingFile;
		
		private static File topDir = new File("tests");

		private static File inputDir;

		private static File inProcessDir;

		private static File completedDir;

		private static File resultsDir;

		private static File lockFile;

		private FileLock _lock;

		private PrintStream oldOut;
		
		private FileFilter testFileFilter;
	}
}
