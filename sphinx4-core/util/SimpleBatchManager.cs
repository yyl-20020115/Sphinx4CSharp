using System;

using IKVM.Attributes;
using java.lang;
using java.util;

namespace edu.cmu.sphinx.util
{
	[Implements(new string[]
	{
		"edu.cmu.sphinx.util.BatchManager"
	})]
	public class SimpleBatchManager : java.lang.Object, BatchManager
	{
		[LineNumberTable(new byte[]
		{
			159,
			181,
			104,
			103,
			103,
			103,
			104
		})]
		
		public SimpleBatchManager(string filename, int skip, int whichBatch, int totalBatches)
		{
			this.batchFile = filename;
			this.skip = skip;
			this.whichBatch = whichBatch;
			this.totalBatches = totalBatches;
		}

		[Throws(new string[]
		{
			"java.io.IOException"
		})]
		
		[LineNumberTable(new byte[]
		{
			42,
			141,
			108,
			118,
			100,
			130,
			110,
			142,
			137,
			112,
			144,
			203
		})]
		
		private List getBatchItems(string text)
		{
			List list = BatchFile.getLines(text, this.skip);
			if (this.totalBatches > 1)
			{
				int num = list.size();
				int num2 = this.totalBatches;
				int num3 = (num2 != -1) ? (num / num2) : (-num);
				if (num3 < 1)
				{
					num3 = 1;
				}
				if (this.whichBatch >= this.totalBatches)
				{
					this.whichBatch = this.totalBatches - 1;
				}
				int num4 = this.whichBatch * num3;
				if (this.whichBatch == this.totalBatches - 1)
				{
					list = list.subList(num4, list.size());
				}
				else
				{
					list = list.subList(num4, num4 + num3);
				}
			}
			return list;
		}

		[Throws(new string[]
		{
			"java.io.IOException"
		})]
		[LineNumberTable(new byte[]
		{
			159,
			191,
			103,
			114
		})]
		
		public virtual void start()
		{
			this.curItem = 0;
			this.items = this.getBatchItems(this.batchFile);
		}

		[Throws(new string[]
		{
			"java.io.IOException"
		})]
		[LineNumberTable(new byte[]
		{
			11,
			115,
			130,
			127,
			5,
			103,
			42
		})]
		
		public virtual BatchItem getNextItem()
		{
			if (this.curItem >= this.items.size())
			{
				return null;
			}
			List list = this.items;
			int num = this.curItem;
			int num2 = num;
			this.curItem = num + 1;
			string batchFileLine = (string)list.get(num2);
			return new BatchItem(BatchFile.getFilename(batchFileLine), BatchFile.getReference(batchFileLine));
		}

		[Throws(new string[]
		{
			"java.io.IOException"
		})]
		public virtual void stop()
		{
		}

		public virtual string getFilename()
		{
			return this.batchFile;
		}

		
		private string batchFile;

		
		private int skip;

		private int whichBatch;

		
		private int totalBatches;

		private int curItem;

		
		private List items;
	}
}
