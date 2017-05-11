using edu.cmu.sphinx.fst.semiring;
using edu.cmu.sphinx.fst.utils;
using java.io;
using java.lang;
using java.util;

namespace edu.cmu.sphinx.fst
{
	public class Convert : java.lang.Object
	{		
		private static void exportSymbols(string[] array, string text)
		{
			if (array == null)
			{
				return;
			}
			FileWriter fileWriter = new FileWriter(text);
			PrintWriter printWriter = new PrintWriter(fileWriter);
			for (int i = 0; i < array.Length; i++)
			{
				string text2 = array[i];
				printWriter.println(new StringBuilder().append(text2).append("\t").append(i).toString());
			}
			printWriter.close();
		}
		
		private static void exportFst(Fst fst, string text)
		{
			FileWriter fileWriter = new FileWriter(text);
			PrintWriter printWriter = new PrintWriter(fileWriter);
			State start = fst.getStart();
			printWriter.println(new StringBuilder().append(start.getId()).append("\t").append(start.getFinalWeight()).toString());
			int numStates = fst.getNumStates();
			for (int i = 0; i < numStates; i++)
			{
				State state = fst.getState(i);
				if (state.getId() != fst.getStart().getId())
				{
					printWriter.println(new StringBuilder().append(state.getId()).append("\t").append(state.getFinalWeight()).toString());
				}
			}
			string[] isyms = fst.getIsyms();
			string[] osyms = fst.getOsyms();
			numStates = fst.getNumStates();
			for (int j = 0; j < numStates; j++)
			{
				State state2 = fst.getState(j);
				int numArcs = state2.getNumArcs();
				for (int k = 0; k < numArcs; k++)
				{
					Arc arc = state2.getArc(k);
					string text2 = (isyms == null) ? Integer.toString(arc.getIlabel()) : isyms[arc.getIlabel()];
					string text3 = (osyms == null) ? Integer.toString(arc.getOlabel()) : osyms[arc.getOlabel()];
					printWriter.println(new StringBuilder().append(state2.getId()).append("\t").append(arc.getNextState().getId()).append("\t").append(text2).append("\t").append(text3).append("\t").append(arc.getWeight()).toString());
				}
			}
			printWriter.close();
		}
		
		private static HashMap importSymbols(string text)
		{
			File file = new File(text);
			if (!file.exists() || !file.isFile())
			{
				return null;
			}
			FileInputStream fileInputStream = new FileInputStream(text);
			DataInputStream dataInputStream = new DataInputStream(fileInputStream);
			BufferedReader bufferedReader = new BufferedReader(new InputStreamReader(dataInputStream));
			HashMap hashMap = new HashMap();
			string text2;
			while ((text2 = bufferedReader.readLine()) != null)
			{
				string[] array = java.lang.String.instancehelper_split(text2, "\\t");
				string text3 = array[0];
				Integer integer = Integer.valueOf(Integer.parseInt(array[1]));
				hashMap.put(text3, integer);
			}
			bufferedReader.close();
			return hashMap;
		}
		
		private Convert()
		{
		}
		
		public static void export(Fst fst, string basename)
		{
			Convert.exportSymbols(fst.getIsyms(), new StringBuilder().append(basename).append(".input.syms").toString());
			Convert.exportSymbols(fst.getOsyms(), new StringBuilder().append(basename).append(".output.syms").toString());
			Convert.exportFst(fst, new StringBuilder().append(basename).append(".fst.txt").toString());
		}
		
		public static Fst importFst(string basename, Semiring semiring)
		{
			Fst fst = new Fst(semiring);
			HashMap hashMap = Convert.importSymbols(new StringBuilder().append(basename).append(".input.syms").toString());
			if (hashMap == null)
			{
				hashMap = new HashMap();
				hashMap.put("<eps>", Integer.valueOf(0));
			}
			HashMap hashMap2 = Convert.importSymbols(new StringBuilder().append(basename).append(".output.syms").toString());
			if (hashMap2 == null)
			{
				hashMap2 = new HashMap();
				hashMap2.put("<eps>", Integer.valueOf(0));
			}
			HashMap hashMap3 = Convert.importSymbols(new StringBuilder().append(basename).append(".states.syms").toString());
			FileInputStream fileInputStream = new FileInputStream(new StringBuilder().append(basename).append(".fst.txt").toString());
			DataInputStream dataInputStream = new DataInputStream(fileInputStream);
			BufferedReader bufferedReader = new BufferedReader(new InputStreamReader(dataInputStream, "UTF-8"));
			int num = 1;
			HashMap hashMap4 = new HashMap();
			string text;
			while ((text = bufferedReader.readLine()) != null)
			{
				string[] array = java.lang.String.instancehelper_split(text, "\\t");
				Integer integer;
				if (hashMap3 == null)
				{
					integer = Integer.valueOf(Integer.parseInt(array[0]));
				}
				else
				{
					integer = (Integer)hashMap3.get(array[0]);
				}
				State state = (State)hashMap4.get(integer);
				if (state == null)
				{
					state = new State(semiring.zero());
					fst.addState(state);
					hashMap4.put(integer, state);
				}
				if (num != 0)
				{
					num = 0;
					fst.setStart(state);
				}
				if (array.Length > 2)
				{
					Integer integer2;
					if (hashMap3 == null)
					{
						integer2 = Integer.valueOf(Integer.parseInt(array[1]));
					}
					else
					{
						integer2 = (Integer)hashMap3.get(array[1]);
					}
					State state2 = (State)hashMap4.get(integer2);
					if (state2 == null)
					{
						state2 = new State(semiring.zero());
						fst.addState(state2);
						hashMap4.put(integer2, state2);
					}
					if (hashMap.get(array[2]) == null)
					{
						hashMap.put(array[2], Integer.valueOf(hashMap.size()));
					}
					int iLabel = ((Integer)hashMap.get(array[2])).intValue();
					if (hashMap2.get(array[3]) == null)
					{
						hashMap2.put(array[3], Integer.valueOf(hashMap2.size()));
					}
					int oLabel = ((Integer)hashMap2.get(array[3])).intValue();
					float weight;
					if (array.Length > 4)
					{
						weight = Float.parseFloat(array[4]);
					}
					else
					{
						weight = 0f;
					}
					Arc arc = new Arc(iLabel, oLabel, weight, state2);
					state.addArc(arc);
				}
				else if (array.Length > 1)
				{
					float finalWeight = Float.parseFloat(array[1]);
					state.setFinalWeight(finalWeight);
				}
				else
				{
					state.setFinalWeight(0f);
				}
			}
			dataInputStream.close();
			fst.setIsyms(Utils.toStringArray(hashMap));
			fst.setOsyms(Utils.toStringArray(hashMap2));
			return fst;
		}
	}
}
