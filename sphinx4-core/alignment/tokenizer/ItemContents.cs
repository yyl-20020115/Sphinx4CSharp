using java.lang;

namespace edu.cmu.sphinx.alignment.tokenizer
{
	public class ItemContents : Object
	{
		public ItemContents()
		{
			this.features = new FeatureSet();
			this.relations = new FeatureSet();
		}
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

		public virtual void removeItemRelation(string relationName)
		{
			this.relations.remove(relationName);
		}

		private FeatureSet features;

		private FeatureSet relations;
	}
}
