using System;

using IKVM.Attributes;
using java.io;
using java.lang;
using java.net;
using java.nio.channels;
using java.util;

namespace edu.cmu.sphinx.util
{
	[Implements(new string[]
	{
		"edu.cmu.sphinx.util.BatchManager"
	})]
	public class PooledBatchManager : java.lang.Object, BatchManager
	{
		
		public static void __<clinit>()
		{
		}

		[LineNumberTable(new byte[]
		{
			159,
			187,
			232,
			54,
			235,
			75,
			103,
			103
		})]
		
		public PooledBatchManager(string filename, int skip)
		{
			this.testFileFilter = new TestFileFilter();
			this.batchFile = filename;
			this.skip = skip;
		}

		[Throws(new string[]
		{
			"java.io.IOException"
		})]
		[LineNumberTable(new byte[]
		{
			160,
			81,
			112,
			113,
			102
		})]
		
		private void @lock()
		{
			RandomAccessFile randomAccessFile = new RandomAccessFile(PooledBatchManager.lockFile, "rw");
			this.@lock = randomAccessFile.getChannel().@lock();
			randomAccessFile.close();
		}

		[Throws(new string[]
		{
			"java.io.IOException"
		})]
		[LineNumberTable(new byte[]
		{
			70,
			108,
			107,
			107,
			107,
			107,
			134
		})]
		
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

		[Throws(new string[]
		{
			"java.io.IOException"
		})]
		[LineNumberTable(new byte[]
		{
			114,
			103,
			145,
			103,
			103,
			107,
			134,
			127,
			10
		})]
		
		private void redirectStdout()
		{
			string myName = this.getMyName();
			File file = File.createTempFile(myName, ".out", PooledBatchManager.resultsDir);
			FileOutputStream fileOutputStream = new FileOutputStream(file);
			PrintStream @out = new PrintStream(fileOutputStream);
			this.oldOut = java.lang.System.@out;
			System.setOut(@out);
			java.lang.System.@out.println(new StringBuilder().append("# These results collected on ").append(this.getMyName()).toString());
		}

		[Throws(new string[]
		{
			"java.io.IOException"
		})]
		[LineNumberTable(new byte[]
		{
			160,
			89,
			107,
			103
		})]
		
		private void unlock()
		{
			this.@lock.release();
			this.@lock = null;
		}

		
		
		private File getCompletedFile(File file)
		{
			File.__<clinit>();
			return new File(PooledBatchManager.completedDir, file.getName());
		}

		[Throws(new string[]
		{
			"java.io.IOException"
		})]
		[LineNumberTable(new byte[]
		{
			160,
			118,
			145,
			101,
			132
		})]
		
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
			File.__<clinit>();
			return new File(PooledBatchManager.inProcessDir, file.getName());
		}

		[Throws(new string[]
		{
			"java.io.IOException"
		})]
		[LineNumberTable(new byte[]
		{
			160,
			134,
			108,
			105,
			144,
			109,
			103,
			42
		})]
		
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

		[Throws(new string[]
		{
			"java.io.IOException"
		})]
		[LineNumberTable(new byte[]
		{
			160,
			74,
			106,
			107
		})]
		
		private void closeStdout()
		{
			java.lang.System.@out.close();
			System.setOut(this.oldOut);
		}

		[Throws(new string[]
		{
			"java.io.IOException"
		})]
		[LineNumberTable(new byte[]
		{
			82,
			139,
			146,
			107,
			105,
			109,
			237,
			61,
			230,
			69
		})]
		
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

		[Throws(new string[]
		{
			"java.io.IOException"
		})]
		[LineNumberTable(new byte[]
		{
			104,
			104,
			103,
			103,
			103,
			102
		})]
		
		private void createInputFile(File file, string text, string text2)
		{
			File file2 = new File(file, text);
			FileOutputStream fileOutputStream = new FileOutputStream(file2);
			PrintStream printStream = new PrintStream(fileOutputStream);
			printStream.println(text2);
			printStream.close();
		}

		[Throws(new string[]
		{
			"java.io.IOException"
		})]
		
		
		private string getMyName()
		{
			return InetAddress.getLocalHost().getHostName();
		}

		[Throws(new string[]
		{
			"java.io.IOException"
		})]
		[LineNumberTable(new byte[]
		{
			5,
			134,
			102,
			138,
			73,
			98,
			98
		})]
		
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

		[Throws(new string[]
		{
			"java.io.IOException"
		})]
		[LineNumberTable(new byte[]
		{
			22,
			230,
			69,
			104,
			109,
			109,
			135,
			103,
			99,
			109,
			109,
			127,
			10,
			243,
			69,
			75,
			230,
			59,
			130,
			166,
			73,
			226,
			61
		})]
		
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

		[Throws(new string[]
		{
			"java.io.IOException"
		})]
		[LineNumberTable(new byte[]
		{
			49,
			134,
			138,
			73,
			98,
			98
		})]
		
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

		[LineNumberTable(new byte[]
		{
			159,
			169,
			111,
			121,
			121,
			121,
			121
		})]
		static PooledBatchManager()
		{
			File.__<clinit>();
			PooledBatchManager.inputDir = new File(PooledBatchManager.topDir, "ToRun");
			File.__<clinit>();
			PooledBatchManager.inProcessDir = new File(PooledBatchManager.topDir, "InProcess");
			File.__<clinit>();
			PooledBatchManager.completedDir = new File(PooledBatchManager.topDir, "Completed");
			File.__<clinit>();
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

		private FileLock @lock;

		private PrintStream oldOut;

		
		private FileFilter testFileFilter;
	}
}
