using java.lang;

namespace edu.cmu.sphinx.alignment.tokenizer
{
	public class Relation : Object
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
