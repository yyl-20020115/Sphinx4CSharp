using edu.cmu.sphinx.fst;
using edu.cmu.sphinx.fst.operations;
using edu.cmu.sphinx.fst.semiring;
using edu.cmu.sphinx.fst.utils;
using java.io;
using java.lang;
using java.net;
using java.util;
using java.util.regex;

namespace edu.cmu.sphinx.linguist.g2p
{
	public class G2PConverter : Object
	{		
		public G2PConverter(URL g2pModelUrl)
		{
			this.eps = "<eps>";
			this.se = "</s>";
			this.sb = "<s>";
			this.skip = "_";
			this.tie = "|";
			this.skipSeqs = new HashSet();
			this.clusters = null;
			try
			{
				this.g2pmodel = ImmutableFst.loadModel(g2pModelUrl.openStream());
			}
			catch (ClassNotFoundException ex)
			{
			throw new IOException(new StringBuilder().append("Failed to load the model from ").append(g2pModelUrl).toString(), ex);
			}
			this.init();
		}

		public virtual ArrayList phoneticize(string word, int nbest)
		{
			ArrayList arrayList = new ArrayList(String.instancehelper_length(word));
			for (int i = 0; i < String.instancehelper_length(word); i++)
			{
				string text = String.instancehelper_substring(word, i, i + 1);
				if (Utils.getIndex(this.g2pmodel.getIsyms(), text) >= 0)
				{
					arrayList.add(text);
				}
			}
			return this.phoneticize(arrayList, nbest);
		}
		
		private void init()
		{
			this.skipSeqs.add(this.eps);
			this.skipSeqs.add(this.sb);
			this.skipSeqs.add(this.se);
			this.skipSeqs.add(this.skip);
			this.skipSeqs.add("-");
			Compose.augment(0, this.g2pmodel, this.g2pmodel.getSemiring());
			ArcSort.apply(this.g2pmodel, new ILabelCompare());
			string[] isyms = this.g2pmodel.getIsyms();
			this.loadClusters(isyms);
			this.epsilonFilter = Compose.getFilter(this.g2pmodel.getIsyms(), this.g2pmodel.getSemiring());
			ArcSort.apply(this.epsilonFilter, new ILabelCompare());
		}
		
		private void loadClusters(string[] array)
		{
			this.clusters = new ArrayList[array.Length];
			for (int i = 0; i < array.Length; i++)
			{
				this.clusters[i] = null;
			}
			for (int i = 2; i < array.Length; i++)
			{
				string text = array[i];
				string text2 = text;
				object _ref = this.tie;
				CharSequence charSequence = CharSequence.Cast(_ref);
				if (String.instancehelper_contains(text2, charSequence))
				{
					string[] array2 = String.instancehelper_split(text, Pattern.quote(this.tie));
					ArrayList arrayList = new ArrayList(Arrays.asList(array2));
					this.clusters[i] = arrayList;
				}
			}
		}
		
		private Fst entryToFSA(ArrayList arrayList)
		{
			TropicalSemiring tropicalSemiring = new TropicalSemiring();
			Fst fst = new Fst(tropicalSemiring);
			State state = new State(tropicalSemiring.zero());
			fst.addState(state);
			fst.setStart(state);
			for (int i = 0; i < arrayList.size() + 1; i++)
			{
				state = new State(tropicalSemiring.zero());
				fst.addState(state);
				if (i >= 1)
				{
					int index = Utils.getIndex(this.g2pmodel.getIsyms(), (string)arrayList.get(i - 1));
					fst.getState(i).addArc(new Arc(index, index, 0f, state));
				}
				else if (i == 0)
				{
					int index = Utils.getIndex(this.g2pmodel.getIsyms(), this.sb);
					fst.getStart().addArc(new Arc(index, index, 0f, state));
				}
				if (i == arrayList.size())
				{
					State state2 = new State(tropicalSemiring.zero());
					fst.addState(state2);
					int num = Utils.getIndex(this.g2pmodel.getIsyms(), this.se);
					state.addArc(new Arc(num, num, 0f, state2));
					state2.setFinalWeight(0f);
				}
			}
			for (int i = 0; i < this.clusters.Length; i++)
			{
				ArrayList arrayList2 = this.clusters[i];
				if (arrayList2 != null)
				{
					int num = 0;
					int num2 = 0;
					while (num2 != -1)
					{
						num2 = Utils.search(arrayList, arrayList2, num);
						if (num2 != -1)
						{
							State state3 = fst.getState(num + num2 + 1);
							state3.addArc(new Arc(i, i, 0f, fst.getState(num + num2 + arrayList2.size() + 1)));
							num = num + num2 + arrayList2.size();
						}
					}
				}
			}
			fst.setIsyms(this.g2pmodel.getIsyms());
			fst.setOsyms(this.g2pmodel.getIsyms());
			return fst;
		}
		
		private ArrayList findAllPaths(Fst fst, int num, HashSet hashSet, string text)
		{
			Semiring semiring = fst.getSemiring();
			HashMap hashMap = new HashMap();
			HashMap hashMap2 = new HashMap();
			LinkedList linkedList = new LinkedList();
			Path path = new Path(fst.getSemiring());
			path.setCost(semiring.one());
			hashMap2.put(fst.getStart(), path);
			linkedList.add(fst.getStart());
			string[] osyms = fst.getOsyms();
			while (!linkedList.isEmpty())
			{
				State state = (State)linkedList.remove();
				Path path2 = (Path)hashMap2.get(state);
				if (state.getFinalWeight() != semiring.zero())
				{
					string text2 = path2.getPath().toString();
					if (hashMap.containsKey(text2))
					{
						Path path3 = (Path)hashMap.get(text2);
						if (path3.getCost() > path2.getCost())
						{
							hashMap.put(text2, path2);
						}
					}
					else
					{
						hashMap.put(text2, path2);
					}
				}
				int i = state.getNumArcs();
				for (int j = 0; j < i; j++)
				{
					Arc arc = state.getArc(j);
					path = new Path(fst.getSemiring());
					Path path4 = (Path)hashMap2.get(state);
					path.setCost(path4.getCost());
					path.setPath((ArrayList)path4.getPath().clone());
					string text3 = osyms[arc.getOlabel()];
					foreach (string text4 in String.instancehelper_split(text3, new StringBuilder().append("\\").append(text).toString()))
					{
						if (!hashSet.contains(text4))
						{
							path.getPath().add(text4);
						}
					}
					path.setCost(semiring.times(path.getCost(), arc.getWeight()));
					State nextState = arc.getNextState();
					hashMap2.put(nextState, path);
					if (!linkedList.contains(nextState))
					{
						linkedList.add(nextState);
					}
				}
			}
			ArrayList arrayList = new ArrayList();
			Iterator iterator = hashMap.values().iterator();
			while (iterator.hasNext())
			{
				Path path5 = (Path)iterator.next();
				arrayList.add(path5);
			}
			Collections.sort(arrayList, new PathComparator());
			int num2 = arrayList.size();
			for (int i = num; i < num2; i++)
			{
				arrayList.remove(arrayList.size() - 1);
			}
			return arrayList;
		}
		
		public virtual ArrayList phoneticize(ArrayList entry, int nbest)
		{
			Fst fst = this.entryToFSA(entry);
			Semiring semiring = fst.getSemiring();
			Compose.augment(1, fst, semiring);
			ArcSort.apply(fst, new OLabelCompare());
			Fst fst2 = Compose.compose(fst, this.epsilonFilter, semiring, true);
			ArcSort.apply(fst2, new OLabelCompare());
			fst2 = Compose.compose(fst2, this.g2pmodel, semiring, true);
			Project.apply(fst2, ProjectType.__OUTPUT);
			if (nbest == 1)
			{
				fst2 = NShortestPaths.get(fst2, 1, false);
			}
			else
			{
				fst2 = NShortestPaths.get(fst2, nbest * 10, false);
			}
			fst2 = RmEpsilon.get(fst2);
			return this.findAllPaths(fst2, nbest, this.skipSeqs, this.tie);
		}
		
		public G2PConverter(string g2pmodel_file)
		{
			this.eps = "<eps>";
			this.se = "</s>";
			this.sb = "<s>";
			this.skip = "_";
			this.tie = "|";
			this.skipSeqs = new HashSet();
			this.clusters = null;
			this.g2pmodel = ImmutableFst.loadModel(g2pmodel_file);
			this.init();
		}

		internal string eps;

		internal string se;

		internal string sb;

		internal string skip;

		internal string tie;
		
		internal HashSet skipSeqs;
		
		internal ArrayList[] clusters;

		internal ImmutableFst g2pmodel;

		internal Fst epsilonFilter;
	}
}
