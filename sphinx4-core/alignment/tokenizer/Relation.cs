using System;

using IKVM.Attributes;
using java.lang;

namespace edu.cmu.sphinx.alignment.tokenizer
{
	public class Relation : java.lang.Object
	{
		public virtual Item getHead()
		{
			return this.head;
		}

		public virtual string getName()
		{
			return this.name;
		}

		public virtual Utterance getUtterance()
		{
			return this.owner;
		}

		public virtual Item getTail()
		{
			return this.tail;
		}

		internal virtual void setTail(Item item)
		{
			this.tail = item;
		}

		internal virtual void setHead(Item item)
		{
			this.head = item;
		}

		[LineNumberTable(new byte[]
		{
			70,
			99,
			132,
			135,
			104,
			104,
			167,
			104,
			140,
			103
		})]
		
		public virtual Item appendItem(Item originalItem)
		{
			ItemContents sharedContents;
			if (originalItem == null)
			{
				sharedContents = null;
			}
			else
			{
				sharedContents = originalItem.getSharedContents();
			}
			Item item = new Item(this, sharedContents);
			if (this.head == null)
			{
				this.head = item;
			}
			if (this.tail != null)
			{
				this.tail.attach(item);
			}
			this.tail = item;
			return item;
		}

		[LineNumberTable(new byte[]
		{
			159,
			188,
			104,
			103,
			103,
			103,
			103
		})]
		
		internal Relation(string text, Utterance utterance)
		{
			this.name = text;
			this.owner = utterance;
			this.head = null;
			this.tail = null;
		}

		
		
		public virtual Item appendItem()
		{
			return this.appendItem(null);
		}

		private string name;

		private Utterance owner;

		private Item head;

		private Item tail;

		public const string TOKEN = "Token";

		public const string WORD = "Word";
	}
}
