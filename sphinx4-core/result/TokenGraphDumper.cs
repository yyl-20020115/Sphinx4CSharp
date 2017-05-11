using edu.cmu.sphinx.decoder.search;
using edu.cmu.sphinx.linguist;
using IKVM.Runtime;
using java.io;
using java.lang;
using java.util;

namespace edu.cmu.sphinx.result
{
	public class TokenGraphDumper : java.lang.Object
	{		
		public virtual string dumpGDL(string title)
		{
			StringBuilder stringBuilder = new StringBuilder("graph: {\n");
			stringBuilder.append("title: \"").append(title).append("\"\n");
			stringBuilder.append("display_edge_labels: yes\n");
			Iterator iterator = this.result.getResultTokens().iterator();
			while (iterator.hasNext())
			{
				Token token = (Token)iterator.next();
				stringBuilder.append(this.dumpTokenGDL(token));
			}
			stringBuilder.append("}\n");
			return stringBuilder.toString();
		}
		
		private string dumpTokenGDL(Token token)
		{
			if (this.dumpedTokens.contains(token))
			{
				return "";
			}
			string text = new StringBuilder().append("[").append(token.getAcousticScore()).append(token.getInsertionScore()).append(',').append(token.getLanguageScore()).append(']').toString();
			if (token.isWord())
			{
				text = new StringBuilder().append(token.getWord().getSpelling()).append(text).toString();
			}
			string text2 = null;
			if (token.getSearchState() != null)
			{
				text2 = this.getColor(token.getSearchState());
			}
			StringBuilder stringBuilder = new StringBuilder().append("node: { title: \"").append(this.getTokenID(token)).append("\" label: \"").append(text).append("\" color: ");
			if (text2 != null)
			{
				stringBuilder.append(text2).append(" }");
			}
			else
			{
				stringBuilder.append(" }");
			}
			stringBuilder.append('\n');
			this.dumpedTokens.add(token);
			if (token.getPredecessor() != null)
			{
				stringBuilder.append("edge: { sourcename: \"").append(this.getTokenID(token)).append("\" targetname: \"").append(this.getTokenID(token.getPredecessor())).append("\" }").append('\n').append(this.dumpTokenGDL(token.getPredecessor()));
			}
			if (this.loserManager != null)
			{
				List alternatePredecessors = this.loserManager.getAlternatePredecessors(token);
				if (alternatePredecessors != null)
				{
					Iterator iterator = alternatePredecessors.iterator();
					while (iterator.hasNext())
					{
						Token token2 = (Token)iterator.next();
						stringBuilder.append("edge: { sourcename: \"").append(this.getTokenID(token)).append("\" targetname: \"").append(this.getTokenID(token2)).append("\" }").append('\n').append(this.dumpTokenGDL(token2));
					}
				}
			}
			return stringBuilder.toString();
		}
		
		private string getColor(SearchState searchState)
		{
			string text = "lightred";
			if (searchState.isFinal())
			{
				text = "magenta";
			}
			else if (searchState is UnitSearchState)
			{
				text = "green";
			}
			else if (searchState is WordSearchState)
			{
				text = "lightblue";
			}
			else if (searchState is HMMSearchState)
			{
				text = "orange";
			}
			return text;
		}
		
		private Integer getTokenID(Token token)
		{
			Integer integer = (Integer)this.tokenIDMap.get(token);
			if (integer == null)
			{
				int id = this.ID;
				int num = id;
				this.ID = id + 1;
				integer = Integer.valueOf(num);
				this.tokenIDMap.put(token, integer);
			}
			return integer;
		}
		
		public TokenGraphDumper(Result result)
		{
			this.result = result;
			this.loserManager = result.getAlternateHypothesisManager();
			this.tokenIDMap = new HashMap();
			this.dumpedTokens = new HashSet();
		}
		
		public virtual void dumpGDL(string title, string fileName)
		{
			try
			{
				java.lang.System.err.println(new StringBuilder().append("Dumping ").append(title).append(" to ").append(fileName).toString());
				FileWriter fileWriter = new FileWriter(fileName);
				fileWriter.write(this.dumpGDL(title));
				fileWriter.close();
			}
			catch (IOException ex)
			{
				Throwable.instancehelper_printStackTrace(ex);
			}
		}
		
		private AlternateHypothesisManager loserManager;
		
		private Result result;
		
		private Map tokenIDMap;		
		
		private Set dumpedTokens;

		private int ID;
	}
}
