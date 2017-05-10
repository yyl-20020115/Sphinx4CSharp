using System;

using IKVM.Attributes;
using java.lang;

namespace edu.cmu.sphinx.alignment.tokenizer
{
	public class ItemContents : java.lang.Object
	{
		[LineNumberTable(new byte[]
		{
			159,
			166,
			104,
			107,
			107
		})]
		
		public ItemContents()
		{
			this.features = new FeatureSet();
			this.relations = new FeatureSet();
		}

		[LineNumberTable(new byte[]
		{
			159,
			183,
			109
		})]
		
		public virtual void addItemRelation(string relationName, Item item)
		{
			this.relations.setObject(relationName, item);
		}

		
		
		public virtual Item getItemRelation(string relationName)
		{
			return (Item)this.relations.getObject(relationName);
		}

		public virtual FeatureSet getFeatures()
		{
			return this.features;
		}

		[LineNumberTable(new byte[]
		{
			0,
			108
		})]
		
		public virtual void removeItemRelation(string relationName)
		{
			this.relations.remove(relationName);
		}

		private FeatureSet features;

		private FeatureSet relations;
	}
}
