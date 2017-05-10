using java.lang;
using java.util;

namespace edu.cmu.sphinx.alignment.tokenizer
{
	public class Item : java.lang.Object
	{
		public virtual FeatureSet getFeatures()
		{
			return this.getSharedContents().getFeatures();
		}

		public virtual Item getNext()
		{
			return this.next;
		}

		public override string toString()
		{
			string text = this.getFeatures().getString("name");
			if (text == null)
			{
				text = "";
			}
			return text;
		}

		public virtual object findFeature(string pathAndFeature)
		{
			object obj = null;
			int num = java.lang.String.instancehelper_lastIndexOf(pathAndFeature, ".");
			string name;
			string path;
			if (num == -1)
			{
				name = pathAndFeature;
				path = null;
			}
			else
			{
				name = java.lang.String.instancehelper_substring(pathAndFeature, num + 1);
				path = java.lang.String.instancehelper_substring(pathAndFeature, 0, num);
			}
			Item item = this.findItem(path);
			if (item != null)
			{
				obj = item.getFeatures().getObject(name);
			}
			return (obj != null) ? obj : "0";
		}

		public virtual Item getPrevious()
		{
			return this.prev;
		}

		public virtual ItemContents getSharedContents()
		{
			return this.contents;
		}

		public virtual Item getLastDaughter()
		{
			Item item = this.daughter;
			if (item == null)
			{
				return null;
			}
			while (item.next != null)
			{
				item = item.next;
			}
			return item;
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
			Item item = new Item(this.getOwnerRelation(), sharedContents);
			item.next = this.next;
			if (this.next != null)
			{
				this.next.prev = item;
			}
			this.attach(item);
			if (this.ownerRelation.getTail() == this)
			{
				this.ownerRelation.setTail(item);
			}
			return item;
		}

		public virtual Relation getOwnerRelation()
		{
			return this.ownerRelation;
		}
	
		public Item(Relation relation, ItemContents sharedContents)
		{
			this.ownerRelation = relation;
			if (sharedContents != null)
			{
				this.contents = sharedContents;
			}
			else
			{
				this.contents = new ItemContents();
			}
			this.parent = null;
			this.daughter = null;
			this.next = null;
			this.prev = null;
			this.getSharedContents().addItemRelation(relation.getName(), this);
		}
	
		public virtual Item addDaughter(Item item)
		{
			Item lastDaughter = this.getLastDaughter();
			Item item2;
			if (lastDaughter != null)
			{
				item2 = lastDaughter.appendItem(item);
			}
			else
			{
				ItemContents sharedContents;
				if (item == null)
				{
					sharedContents = new ItemContents();
				}
				else
				{
					sharedContents = item.getSharedContents();
				}
				item2 = new Item(this.getOwnerRelation(), sharedContents);
				item2.parent = this;
				this.daughter = item2;
			}
			return item2;
		}
		
		public virtual Item findItem(string path)
		{
			Item item = this;
			if (path == null)
			{
				return this;
			}
			StringTokenizer stringTokenizer = new StringTokenizer(path, ":.");
			while (item != null && stringTokenizer.hasMoreTokens())
			{
				string text = stringTokenizer.nextToken();
				if (java.lang.String.instancehelper_equals(text, "n"))
				{
					item = item.getNext();
				}
				else if (java.lang.String.instancehelper_equals(text, "p"))
				{
					item = item.getPrevious();
				}
				else if (java.lang.String.instancehelper_equals(text, "nn"))
				{
					item = item.getNext();
					if (item != null)
					{
						item = item.getNext();
					}
				}
				else if (java.lang.String.instancehelper_equals(text, "pp"))
				{
					item = item.getPrevious();
					if (item != null)
					{
						item = item.getPrevious();
					}
				}
				else if (java.lang.String.instancehelper_equals(text, "parent"))
				{
					item = item.getParent();
				}
				else if (java.lang.String.instancehelper_equals(text, "daughter") || java.lang.String.instancehelper_equals(text, "daughter1"))
				{
					item = item.getDaughter();
				}
				else if (java.lang.String.instancehelper_equals(text, "daughtern"))
				{
					item = item.getLastDaughter();
				}
				else if (java.lang.String.instancehelper_equals(text, "R"))
				{
					string relationName = stringTokenizer.nextToken();
					item = item.getSharedContents().getItemRelation(relationName);
				}
				else
				{
					java.lang.System.@out.println(new StringBuilder().append("findItem: bad feature ").append(text).append(" in ").append(path).toString());
				}
			}
			return item;
		}
		public virtual Item getParent()
		{
			Item item = this;
			while (item.prev != null)
			{
				item = item.prev;
			}
			return item.parent;
		}

		public virtual Item getDaughter()
		{
			return this.daughter;
		}

		internal virtual void attach(Item item)
		{
			this.next = item;
			item.prev = this;
		}
	
		public virtual Item getItemAs(string relationName)
		{
			return this.getSharedContents().getItemRelation(relationName);
		}

		public virtual bool hasDaughters()
		{
			return this.daughter != null;
		}

		public virtual Item getNthDaughter(int which)
		{
			Item item = this.daughter;
			int num = 0;
			for (;;)
			{
				int num2 = num;
				num++;
				if (num2 == which || item == null)
				{
					break;
				}
				item = item.next;
			}
			return item;
		}	
		public virtual Item createDaughter()
		{
			return this.addDaughter(null);
		}
		
		public virtual Utterance getUtterance()
		{
			return this.getOwnerRelation().getUtterance();
		}		
		public virtual Item prependItem(Item originalItem)
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
			Item item = new Item(this.getOwnerRelation(), sharedContents);
			item.prev = this.prev;
			if (this.prev != null)
			{
				this.prev.next = item;
			}
			item.next = this;
			this.prev = item;
			if (this.parent != null)
			{
				this.parent.daughter = item;
				item.parent = this.parent;
				this.parent = null;
			}
			if (this.ownerRelation.getHead() == this)
			{
				this.ownerRelation.setHead(item);
			}
			return item;
		}
		
		public virtual bool equalsShared(Item otherItem)
		{
			return otherItem != null && java.lang.Object.instancehelper_equals(this.getSharedContents(), otherItem.getSharedContents());
		}

		private Relation ownerRelation;

		private ItemContents contents;

		private Item parent;

		private Item daughter;

		private Item next;

		private Item prev;
	}
}
