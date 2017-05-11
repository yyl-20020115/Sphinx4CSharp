using edu.cmu.sphinx.decoder.scorer;
using ikvm.@internal;
using java.lang;
using java.util;

namespace edu.cmu.sphinx.decoder.search
{
	public class AlternateHypothesisManager : java.lang.Object
	{
		public AlternateHypothesisManager(int maxEdges)
		{
			this.viterbiLoserMap = new HashMap();
			this.maxEdges = maxEdges;
		}
	
		public virtual void addAlternatePredecessor(Token token, Token predecessor)
		{
			if (!AlternateHypothesisManager.assertionsDisabled && predecessor == token.getPredecessor())
			{
				
				throw new AssertionError();
			}
			object obj = (List)this.viterbiLoserMap.get(token);
			if ((List)obj == null)
			{
				obj = new ArrayList();
				this.viterbiLoserMap.put(token, (ArrayList)obj);
			}
			object obj2 = obj;
			List list;
			if (obj2 != null)
			{
				if ((list = (obj2 as List)) == null)
				{
					throw new IncompatibleClassChangeError();
				}
			}
			else
			{
				list = null;
			}
			list.add(predecessor);
		}		
		
		public virtual List getAlternatePredecessors(Token token)
		{
			return (List)this.viterbiLoserMap.get(token);
		}
	
		public virtual void purge()
		{
			int num = this.maxEdges - 1;
			Iterator iterator = this.viterbiLoserMap.entrySet().iterator();
			while (iterator.hasNext())
			{
				Map.Entry entry = (Map.Entry)iterator.next();
				List list = (List)entry.getValue();
				Collections.sort(list, Scoreable.COMPARATOR);
				List list2 = list.subList(0, (list.size() <= num) ? list.size() : num);
				this.viterbiLoserMap.put(entry.getKey(), list2);
			}
		}

		public virtual bool hasAlternatePredecessors(Token token)
		{
			return this.viterbiLoserMap.containsKey(token);
		}

		private Map viterbiLoserMap;
		
		private int maxEdges;
		
		internal static bool assertionsDisabled = !ClassLiteral<AlternateHypothesisManager>.Value.desiredAssertionStatus();
	}
}
