using java.lang;
using java.util;

namespace edu.cmu.sphinx.alignment.tokenizer
{
	public class Utterance : java.lang.Object
	{
		public Utterance(CharTokenizer tokenizer)
		{
			this.features = new FeatureSet();
			this.relations = new FeatureSet();
			this.setTokenList(tokenizer);
		}

		
		
		public virtual Relation getRelation(string name)
		{
			return (Relation)this.relations.getObject(name);
		}
	
		private void setTokenList(Iterator iterator)
		{
			Relation relation = this.createRelation("Token");
			while (iterator.hasNext())
			{
				Token token = (Token)iterator.next();
				string word = token.getWord();
				if (word != null && java.lang.String.instancehelper_length(word) > 0)
				{
					Item item = relation.appendItem();
					FeatureSet featureSet = item.getFeatures();
					featureSet.setString("name", word);
					featureSet.setString("whitespace", token.getWhitespace());
					featureSet.setString("prepunctuation", token.getPrepunctuation());
					featureSet.setString("punc", token.getPostpunctuation());
					featureSet.setString("file_pos", java.lang.String.valueOf(token.getPosition()));
					featureSet.setString("line_number", java.lang.String.valueOf(token.getLineNumber()));
				}
			}
		}
	
		private static Item getItem(Relation relation, float num)
		{
			Item tail = relation.getTail();
			float segmentEnd = Utterance.getSegmentEnd(tail);
			if (num < 0f || segmentEnd < num)
			{
				return null;
			}
			if (segmentEnd - num > num)
			{
				return Utterance.findFromFront(relation, num);
			}
			return Utterance.findFromEnd(relation, num);
		}
		
		private static float getSegmentEnd(Item item)
		{
			FeatureSet featureSet = item.getFeatures();
			return featureSet.getFloat("end");
		}
	
		private static Item findFromFront(Relation relation, float num)
		{
			Item item = relation.getHead();
			while (item != null && num > Utterance.getSegmentEnd(item))
			{
				item = item.getNext();
			}
			return item;
		}
		
		private static Item findFromEnd(Relation relation, float num)
		{
			Item item = relation.getTail();
			while (item != null && Utterance.getSegmentEnd(item) > num)
			{
				item = item.getPrevious();
			}
			if (item != relation.getTail())
			{
				item = item.getNext();
			}
			return item;
		}
		
		public virtual Relation createRelation(string name)
		{
			Relation relation = new Relation(name, this);
			this.relations.setObject(name, relation);
			return relation;
		}
	
		public virtual bool hasRelation(string name)
		{
			return this.relations.isPresent(name);
		}
		
		public virtual void remove(string name)
		{
			this.features.remove(name);
		}
		
		public virtual void setInt(string name, int value)
		{
			this.features.setInt(name, value);
		}
	
		public virtual void setFloat(string name, float value)
		{
			this.features.setFloat(name, value);
		}
		
		public virtual void setString(string name, string value)
		{
			this.features.setString(name, value);
		}
		
		public virtual void setObject(string name, object value)
		{
			this.features.setObject(name, value);
		}
		
		public virtual Item getItem(string relation, float time)
		{
			string pathAndFeature;
			if (java.lang.String.instancehelper_equals(relation, "Word"))
			{
				pathAndFeature = "R:SylStructure.parent.parent.R:Word";
			}
			else
			{
				if (!java.lang.String.instancehelper_equals(relation, "Token"))
				{
					string text = new StringBuilder().append("Utterance.getItem(): relation cannot be ").append(relation).toString();
					
					throw new IllegalArgumentException(text);
				}
				pathAndFeature = "R:SylStructure.parent.parent.R:Token.parent";
			}
			PathExtractor pathExtractor = new PathExtractor(pathAndFeature, false);
			Item item = Utterance.getItem(this.getRelation(relation), time);
			if (item != null)
			{
				return pathExtractor.findItem(item);
			}
			return null;
		}

		private FeatureSet features;

		private FeatureSet relations;
	}
}
