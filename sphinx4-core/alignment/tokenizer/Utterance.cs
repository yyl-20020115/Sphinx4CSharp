using System;

using IKVM.Attributes;
using java.lang;
using java.util;

namespace edu.cmu.sphinx.alignment.tokenizer
{
	public class Utterance : java.lang.Object
	{
		[LineNumberTable(new byte[]
		{
			159,
			178,
			104,
			107,
			107,
			103
		})]
		
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

		
		[LineNumberTable(new byte[]
		{
			160,
			91,
			108,
			107,
			108,
			135,
			114,
			135,
			104,
			109,
			114,
			104,
			37,
			133,
			114,
			104,
			42,
			133,
			104,
			42,
			197,
			101
		})]
		
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

		[LineNumberTable(new byte[]
		{
			107,
			199,
			103,
			110,
			98,
			105,
			137
		})]
		
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

		[LineNumberTable(new byte[]
		{
			160,
			79,
			103
		})]
		
		private static float getSegmentEnd(Item item)
		{
			FeatureSet featureSet = item.getFeatures();
			return featureSet.getFloat("end");
		}

		[LineNumberTable(new byte[]
		{
			160,
			71,
			103,
			109,
			137
		})]
		
		private static Item findFromFront(Relation relation, float num)
		{
			Item item = relation.getHead();
			while (item != null && num > Utterance.getSegmentEnd(item))
			{
				item = item.getNext();
			}
			return item;
		}

		[LineNumberTable(new byte[]
		{
			122,
			103,
			109,
			169,
			105,
			167
		})]
		
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

		[LineNumberTable(new byte[]
		{
			1,
			104,
			109
		})]
		
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

		[LineNumberTable(new byte[]
		{
			33,
			108
		})]
		
		public virtual void remove(string name)
		{
			this.features.remove(name);
		}

		[LineNumberTable(new byte[]
		{
			43,
			109
		})]
		
		public virtual void setInt(string name, int value)
		{
			this.features.setInt(name, value);
		}

		[LineNumberTable(new byte[]
		{
			53,
			110
		})]
		
		public virtual void setFloat(string name, float value)
		{
			this.features.setFloat(name, value);
		}

		[LineNumberTable(new byte[]
		{
			63,
			109
		})]
		
		public virtual void setString(string name, string value)
		{
			this.features.setString(name, value);
		}

		[LineNumberTable(new byte[]
		{
			73,
			109
		})]
		
		public virtual void setObject(string name, object value)
		{
			this.features.setObject(name, value);
		}

		[LineNumberTable(new byte[]
		{
			84,
			130,
			109,
			104,
			109,
			136,
			223,
			6,
			136,
			143,
			99,
			136
		})]
		
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
