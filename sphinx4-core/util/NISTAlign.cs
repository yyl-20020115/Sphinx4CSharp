using IKVM.Runtime;
using java.io;
using java.lang;
using java.text;
using java.util;

namespace edu.cmu.sphinx.util
{
	public class NISTAlign : java.lang.Object
	{	
		public NISTAlign(bool showResults, bool showAlignedResults)
		{
			this.showResults = showResults;
			this.showAlignedResults = showAlignedResults;
			this.resetTotals();
		}

		public virtual void setShowResults(bool showResults)
		{
			this.showResults = showResults;
		}

		public virtual void setShowAlignedResults(bool showAlignedResults)
		{
			this.showAlignedResults = showAlignedResults;
		}

		public virtual void resetTotals()
		{
			this.totalSentences = 0;
			this.totalSentencesWithErrors = 0;
			this.totalSentencesWithSubtitutions = 0;
			this.totalSentencesWithInsertions = 0;
			this.totalSentencesWithDeletions = 0;
			this.totalReferenceWords = 0;
			this.totalHypothesisWords = 0;
			this.totalAlignedWords = 0;
			this.totalWordsCorrect = 0;
			this.totalSubstitutions = 0;
			this.totalInsertions = 0;
			this.totalDeletions = 0;
		}
		
		public virtual void printSentenceSummary()
		{
			if (this.showResults)
			{
				java.lang.System.@out.println(new StringBuilder().append("REF:       ").append(this.toString(this.referenceItems)).toString());
				java.lang.System.@out.println(new StringBuilder().append("HYP:       ").append(this.toString(this.hypothesisItems)).toString());
			}
			if (this.showAlignedResults)
			{
				java.lang.System.@out.println(new StringBuilder().append("ALIGN_REF: ").append(this.toString(this.alignedReferenceWords)).toString());
				java.lang.System.@out.println(new StringBuilder().append("ALIGN_HYP: ").append(this.toString(this.alignedHypothesisWords)).toString());
			}
		}
		
		public virtual void printTotalSummary()
		{
			if (this.totalSentences > 0)
			{
				java.lang.System.@out.print(new StringBuilder().append("   Accuracy: ").append(this.toPercentage("##0.000%", this.getTotalWordAccuracy())).toString());
				java.lang.System.@out.println(new StringBuilder().append("    Errors: ").append(this.getTotalWordErrors()).append("  (Sub: ").append(this.totalSubstitutions).append("  Ins: ").append(this.totalInsertions).append("  Del: ").append(this.totalDeletions).append(')').toString());
				java.lang.System.@out.println(new StringBuilder().append("   Words: ").append(this.totalReferenceWords).append("   Matches: ").append(this.totalWordsCorrect).append("    WER: ").append(this.toPercentage("##0.000%", this.getTotalWordErrorRate())).toString());
				java.lang.System.@out.println(new StringBuilder().append("   Sentences: ").append(this.totalSentences).append("   Matches: ").append(this.totalSentences - this.totalSentencesWithErrors).append("   SentenceAcc: ").append(this.toPercentage("##0.000%", this.getTotalSentenceAccuracy())).toString());
			}
		}
		
		public virtual bool align(string reference, string hypothesis)
		{
			this.rawReference = reference;
			this.rawHypothesis = hypothesis;
			int num = java.lang.String.instancehelper_indexOf(this.rawReference, 40);
			if (num != -1)
			{
				this.referenceAnnotation = java.lang.String.instancehelper_substring(this.rawReference, num);
				this.referenceItems = this.toList(java.lang.String.instancehelper_substring(this.rawReference, 0, num));
			}
			else
			{
				this.referenceAnnotation = null;
				this.referenceItems = this.toList(this.rawReference);
			}
			num = java.lang.String.instancehelper_indexOf(this.rawHypothesis, 40);
			if (num != -1)
			{
				this.hypothesisItems = this.toList(java.lang.String.instancehelper_substring(this.rawHypothesis, 0, num));
			}
			else
			{
				this.hypothesisItems = this.toList(this.rawHypothesis);
			}
			this.substitutions = 0;
			this.insertions = 0;
			this.deletions = 0;
			this.alignWords(this.backtrace(this.createBacktraceTable(this.referenceItems, this.hypothesisItems, new NISTAlign_1(this))), new NISTAlign_2(this));
			this.correct = this.alignedReferenceWords.size() - (this.insertions + this.deletions + this.substitutions);
			this.updateTotals();
			return this.insertions + this.deletions + this.substitutions == 0;
		}
		
		internal virtual LinkedList toList(string text)
		{
			LinkedList linkedList = new LinkedList();
			StringTokenizer stringTokenizer = new StringTokenizer(java.lang.String.instancehelper_trim(text));
			while (stringTokenizer.hasMoreTokens())
			{
				string text2 = stringTokenizer.nextToken();
				linkedList.add(text2);
			}
			return linkedList;
		}
		
		internal virtual int[][] createBacktraceTable(LinkedList linkedList, LinkedList linkedList2, NISTAlign.Comparator comparator)
		{
			int num = linkedList.size() + 1;
			int num2 = linkedList2.size() + 1;
			int[] array = new int[2];
			int num3 = num2;
			array[1] = num3;
			num3 = num;
			array[0] = num3;
			int[][] array2 = (int[][])ByteCodeHelper.multianewarray(typeof(int[][]).TypeHandle, array);
			int num4 = linkedList.size() + 1;
			int num5 = linkedList2.size() + 1;
			array = new int[2];
			num3 = num5;
			array[1] = num3;
			num3 = num4;
			array[0] = num3;
			int[][] array3 = (int[][])ByteCodeHelper.multianewarray(typeof(int[][]).TypeHandle, array);
			array2[0][0] = 0;
			array3[0][0] = 0;
			for (int i = 1; i <= linkedList.size(); i++)
			{
				array2[i][0] = 75 * i;
				array3[i][0] = 3;
			}
			for (int i = 1; i <= linkedList2.size(); i++)
			{
				array2[0][i] = 75 * i;
				array3[0][i] = 2;
			}
			for (int i = 1; i <= linkedList.size(); i++)
			{
				for (int j = 1; j <= linkedList2.size(); j++)
				{
					int num6 = 1000000;
					int num7 = array2[i - 1][j] + 75;
					if (num7 < num6)
					{
						num6 = num7;
						array2[i][j] = num7;
						array3[i][j] = 3;
					}
					if (comparator.isSimilar(linkedList.get(i - 1), linkedList2.get(j - 1)))
					{
						num7 = array2[i - 1][j - 1];
						if (num7 < num6)
						{
							num6 = num7;
							array2[i][j] = num7;
							array3[i][j] = 0;
						}
					}
					else
					{
						num7 = array2[i - 1][j - 1] + 100;
						if (num7 < num6)
						{
							num6 = num7;
							array2[i][j] = num7;
							array3[i][j] = 1;
						}
					}
					num7 = array2[i][j - 1] + 75;
					if (num7 < num6)
					{
						array2[i][j] = num7;
						array3[i][j] = 2;
					}
				}
			}
			return array3;
		}
		
		internal virtual LinkedList backtrace(int[][] array)
		{
			LinkedList linkedList = new LinkedList();
			int num = this.referenceItems.size();
			int num2 = this.hypothesisItems.size();
			while (num >= 0 && num2 >= 0)
			{
				linkedList.add(Integer.valueOf(array[num][num2]));
				switch (array[num][num2])
				{
				case 0:
					num --;
					num2 --;
					break;
				case 1:
					num --;
					num2 --;
					this.substitutions++;
					break;
				case 2:
					num2 --;
					this.insertions++;
					break;
				case 3:
					num --;
					this.deletions++;
					break;
				}
			}
			return linkedList;
		}
		
		internal virtual void alignWords(LinkedList linkedList, NISTAlign.StringRenderer stringRenderer)
		{
			ListIterator listIterator = this.referenceItems.listIterator();
			ListIterator listIterator2 = this.hypothesisItems.listIterator();
			object obj = null;
			object obj2 = null;
			this.alignedReferenceWords = new LinkedList();
			this.alignedHypothesisWords = new LinkedList();
			for (int i = linkedList.size() - 2; i >= 0; i --)
			{
				int num = ((Integer)linkedList.get(i)).intValue();
				string text;
				if (num != 2)
				{
					obj = listIterator.next();
					text = stringRenderer.getRef(obj, obj2);
				}
				else
				{
					text = null;
				}
				string text2;
				if (num != 3)
				{
					obj2 = listIterator2.next();
					text2 = stringRenderer.getHyp(obj, obj2);
				}
				else
				{
					text2 = null;
				}
				switch (num)
				{
				case 1:
					text = java.lang.String.instancehelper_toUpperCase(text);
					text2 = java.lang.String.instancehelper_toUpperCase(text2);
					break;
				case 2:
					text2 = java.lang.String.instancehelper_toUpperCase(text2);
					break;
				case 3:
					text = java.lang.String.instancehelper_toUpperCase(text);
					break;
				}
				if (text == null)
				{
					text = java.lang.String.instancehelper_substring("********************************************", 0, java.lang.String.instancehelper_length(text2));
				}
				if (text2 == null)
				{
					text2 = java.lang.String.instancehelper_substring("********************************************", 0, java.lang.String.instancehelper_length(text));
				}
				if (java.lang.String.instancehelper_length(text) > java.lang.String.instancehelper_length(text2))
				{
					text2 = java.lang.String.instancehelper_concat(text2, java.lang.String.instancehelper_substring("                                            ", 0, java.lang.String.instancehelper_length(text) - java.lang.String.instancehelper_length(text2)));
				}
				else if (java.lang.String.instancehelper_length(text) < java.lang.String.instancehelper_length(text2))
				{
					text = java.lang.String.instancehelper_concat(text, java.lang.String.instancehelper_substring("                                            ", 0, java.lang.String.instancehelper_length(text2) - java.lang.String.instancehelper_length(text)));
				}
				this.alignedReferenceWords.add(text);
				this.alignedHypothesisWords.add(text2);
			}
		}
		
		internal virtual void updateTotals()
		{
			this.totalSentences++;
			if (this.substitutions + this.insertions + this.deletions != 0)
			{
				this.totalSentencesWithErrors++;
			}
			if (this.substitutions != 0)
			{
				this.totalSentencesWithSubtitutions++;
			}
			if (this.insertions != 0)
			{
				this.totalSentencesWithInsertions++;
			}
			if (this.deletions != 0)
			{
				this.totalSentencesWithDeletions++;
			}
			this.totalReferenceWords += this.referenceItems.size();
			this.totalHypothesisWords += this.hypothesisItems.size();
			this.totalAlignedWords += this.alignedReferenceWords.size();
			this.totalWordsCorrect += this.correct;
			this.totalSubstitutions += this.substitutions;
			this.totalInsertions += this.insertions;
			this.totalDeletions += this.deletions;
		}
		
		private string toString(LinkedList linkedList)
		{
			if (linkedList == null || linkedList.isEmpty())
			{
				return "";
			}
			StringBuilder stringBuilder = new StringBuilder();
			ListIterator listIterator = linkedList.listIterator();
			while (listIterator.hasNext())
			{
				stringBuilder.append(listIterator.next()).append(' ');
			}
			stringBuilder.setLength(stringBuilder.length() - 1);
			return stringBuilder.toString();
		}

		public virtual int getTotalWordErrors()
		{
			return this.totalSubstitutions + this.totalInsertions + this.totalDeletions;
		}

		public virtual float getTotalWordAccuracy()
		{
			if (this.totalReferenceWords == 0)
			{
				return 0f;
			}
			return (float)this.totalWordsCorrect / (float)this.totalReferenceWords;
		}
		
		internal virtual string toPercentage(string text, float num)
		{
			NISTAlign.percentageFormat.applyPattern(text);
			return NISTAlign.percentageFormat.format((double)num);
		}
		
		public virtual float getTotalWordErrorRate()
		{
			if (this.totalReferenceWords == 0)
			{
				return 0f;
			}
			return (float)this.getTotalWordErrors() / (float)this.totalReferenceWords;
		}

		public virtual float getTotalSentenceAccuracy()
		{
			int num = this.totalSentences - this.totalSentencesWithErrors;
			if (this.totalSentences == 0)
			{
				return 0f;
			}
			return (float)num / (float)this.totalSentences;
		}
		
		internal virtual string toPercentage(string text, int num, int num2)
		{
			NISTAlign.percentageFormat.applyPattern(text);
			return this.padLeft(6, NISTAlign.percentageFormat.format((double)num / (double)num2));
		}
		
		internal virtual string padLeft(int num, int num2)
		{
			return this.padLeft(num, Integer.toString(num2));
		}
		
		internal virtual string padLeft(int num, string text)
		{
			int num2 = java.lang.String.instancehelper_length(text);
			if (num2 < num)
			{
				return java.lang.String.instancehelper_concat(java.lang.String.instancehelper_substring("                                            ", 0, num - num2), text);
			}
			return text;
		}
		
		public virtual void printNISTSentenceSummary()
		{
			int num = this.substitutions + this.insertions + this.deletions;
			java.lang.System.@out.println();
			java.lang.System.@out.print(new StringBuilder().append("REF: ").append(this.toString(this.alignedReferenceWords)).toString());
			if (this.referenceAnnotation != null)
			{
				java.lang.System.@out.print(new StringBuilder().append(' ').append(this.referenceAnnotation).toString());
			}
			java.lang.System.@out.println();
			java.lang.System.@out.print(new StringBuilder().append("HYP: ").append(this.toString(this.alignedHypothesisWords)).toString());
			if (this.referenceAnnotation != null)
			{
				java.lang.System.@out.print(new StringBuilder().append(' ').append(this.referenceAnnotation).toString());
			}
			java.lang.System.@out.println();
			java.lang.System.@out.println();
			if (this.referenceAnnotation != null)
			{
				java.lang.System.@out.println(new StringBuilder().append("SENTENCE ").append(this.totalSentences).append("  ").append(this.referenceAnnotation).toString());
			}
			else
			{
				java.lang.System.@out.println(new StringBuilder().append("SENTENCE ").append(this.totalSentences).toString());
			}
			java.lang.System.@out.println(new StringBuilder().append("Correct          = ").append(this.toPercentage("##0.0%", this.correct, this.referenceItems.size())).append(this.padLeft(5, this.correct)).append("   (").append(this.padLeft(6, this.totalWordsCorrect)).append(')').toString());
			java.lang.System.@out.println(new StringBuilder().append("Errors           = ").append(this.toPercentage("##0.0%", num, this.referenceItems.size())).append(this.padLeft(5, num)).append("   (").append(this.padLeft(6, this.totalSentencesWithErrors)).append(')').toString());
			java.lang.System.@out.println();
			java.lang.System.@out.println("============================================================================");
		}
		
		public virtual void printNISTTotalSummary()
		{
			int num = this.totalSentences - this.totalSentencesWithErrors;
			java.lang.System.@out.println();
			java.lang.System.@out.println("---------- SUMMARY ----------");
			java.lang.System.@out.println();
			java.lang.System.@out.println("SENTENCE RECOGNITION PERFORMANCE:");
			java.lang.System.@out.println(new StringBuilder().append("sentences                          ").append(this.totalSentences).toString());
			java.lang.System.@out.println(new StringBuilder().append("  correct                  ").append(this.toPercentage("##0.0%", num, this.totalSentences)).append(" (").append(this.padLeft(4, num)).append(')').toString());
			java.lang.System.@out.println(new StringBuilder().append("  with error(s)            ").append(this.toPercentage("##0.0%", this.totalSentencesWithErrors, this.totalSentences)).append(" (").append(this.padLeft(4, this.totalSentencesWithErrors)).append(')').toString());
			java.lang.System.@out.println(new StringBuilder().append("    with substitutions(s)  ").append(this.toPercentage("##0.0%", this.totalSentencesWithSubtitutions, this.totalSentences)).append(" (").append(this.padLeft(4, this.totalSentencesWithSubtitutions)).append(')').toString());
			java.lang.System.@out.println(new StringBuilder().append("    with insertion(s)      ").append(this.toPercentage("##0.0%", this.totalSentencesWithInsertions, this.totalSentences)).append(" (").append(this.padLeft(4, this.totalSentencesWithInsertions)).append(')').toString());
			java.lang.System.@out.println(new StringBuilder().append("    with deletions(s)      ").append(this.toPercentage("##0.0%", this.totalSentencesWithDeletions, this.totalSentences)).append(" (").append(this.padLeft(4, this.totalSentencesWithDeletions)).append(')').toString());
			java.lang.System.@out.println();
			java.lang.System.@out.println();
			java.lang.System.@out.println();
			java.lang.System.@out.println("WORD RECOGNITION PERFORMANCE:");
			java.lang.System.@out.println(new StringBuilder().append("Correct           = ").append(this.toPercentage("##0.0%", this.totalWordsCorrect, this.totalReferenceWords)).append(" (").append(this.padLeft(6, this.totalWordsCorrect)).append(')').toString());
			java.lang.System.@out.println(new StringBuilder().append("Substitutions     = ").append(this.toPercentage("##0.0%", this.totalSubstitutions, this.totalReferenceWords)).append(" (").append(this.padLeft(6, this.totalSubstitutions)).append(')').toString());
			java.lang.System.@out.println(new StringBuilder().append("Deletions         = ").append(this.toPercentage("##0.0%", this.totalDeletions, this.totalReferenceWords)).append(" (").append(this.padLeft(6, this.totalDeletions)).append(')').toString());
			java.lang.System.@out.println(new StringBuilder().append("Insertions        = ").append(this.toPercentage("##0.0%", this.totalInsertions, this.totalReferenceWords)).append(" (").append(this.padLeft(6, this.totalInsertions)).append(')').toString());
			java.lang.System.@out.println(new StringBuilder().append("Errors            = ").append(this.toPercentage("##0.0%", this.getTotalWordErrors(), this.totalReferenceWords)).append(" (").append(this.padLeft(6, this.getTotalWordErrors())).append(')').toString());
			java.lang.System.@out.println();
			java.lang.System.@out.println(new StringBuilder().append("Ref. words           = ").append(this.padLeft(6, this.totalReferenceWords)).toString());
			java.lang.System.@out.println(new StringBuilder().append("Hyp. words           = ").append(this.padLeft(6, this.totalHypothesisWords)).toString());
			java.lang.System.@out.println(new StringBuilder().append("Aligned words        = ").append(this.padLeft(6, this.totalAlignedWords)).toString());
			java.lang.System.@out.println();
			java.lang.System.@out.println(new StringBuilder().append("WORD ACCURACY=  ").append(this.toPercentage("##0.000%", this.totalWordsCorrect, this.totalReferenceWords)).append(" (").append(this.padLeft(5, this.totalWordsCorrect)).append('/').append(this.padLeft(5, this.totalReferenceWords)).append(")  ERRORS= ").append(this.toPercentage("##0.000%", this.getTotalWordErrors(), this.totalReferenceWords)).append(" (").append(this.padLeft(5, this.getTotalWordErrors())).append('/').append(this.padLeft(5, this.totalReferenceWords)).append(')').toString());
			java.lang.System.@out.println();
		}

		public virtual string getReference()
		{
			return this.toString(this.referenceItems);
		}
		public virtual string getHypothesis()
		{
			return this.toString(this.hypothesisItems);
		}
		
		public virtual string getAlignedReference()
		{
			return this.toString(this.alignedReferenceWords);
		}
		
		public virtual string getAlignedHypothesis()
		{
			return this.toString(this.alignedHypothesisWords);
		}

		public virtual int getTotalWords()
		{
			return this.totalReferenceWords;
		}

		public virtual int getTotalSubstitutions()
		{
			return this.totalSubstitutions;
		}

		public virtual int getTotalInsertions()
		{
			return this.totalInsertions;
		}

		public virtual int getTotalDeletions()
		{
			return this.totalDeletions;
		}

		public virtual int getTotalSentences()
		{
			return this.totalSentences;
		}

		public virtual int getTotalSentencesWithErrors()
		{
			return this.totalSentencesWithDeletions;
		}

		
		public static void main(string[] args)
		{
			NISTAlign nistalign = new NISTAlign(true, true);
			System.Exception ex4 = null;
			try
			{
				BufferedReader bufferedReader = new BufferedReader(new InputStreamReader(new FileInputStream(args[0])));
				BufferedReader bufferedReader2 = new BufferedReader(new InputStreamReader(new FileInputStream(args[1])));
				try
				{
					for (;;)
					{
						string text = bufferedReader.readLine();
						string text2 = bufferedReader2.readLine();
						if (text == null)
						{
							break;
						}
						if (text2 == null)
						{
							break;
						}
						nistalign.align(text, text2);
						nistalign.printNISTSentenceSummary();
					}
				}
				catch (IOException)
				{
				}
			}
			catch (System.Exception ex2)
			{
				ex4 = ex2;
				goto IL_80;
			}
			goto IL_85;
			IL_80:
			System.Exception ex5 = ex4;
			goto IL_A6;
			IL_85:
			System.Exception ex8 = null;
			try
			{
				nistalign.printNISTTotalSummary();
			}
			catch (System.Exception ex6)
			{
				ex8 = ex6;
				goto IL_A0;
			}
			return;
			IL_A0:
			ex5 = ex8;
			IL_A6:
			java.lang.System.err.println(ex5);
			Throwable.instancehelper_printStackTrace(ex5);
			java.lang.System.@out.println();
			java.lang.System.@out.println("Usage: align <reference file> <hypothesis file>");
			java.lang.System.@out.println();
		}

		internal const int OK = 0;

		internal const int SUBSTITUTION = 1;

		internal const int INSERTION = 2;

		internal const int DELETION = 3;

		internal const int MAX_PENALTY = 1000000;

		internal const int SUBSTITUTION_PENALTY = 100;

		internal const int INSERTION_PENALTY = 75;

		internal const int DELETION_PENALTY = 75;

		internal const string STARS = "********************************************";

		internal const string SPACES = "                                            ";

		internal const string HRULE = "============================================================================";

		private int totalSentences;

		private int totalSentencesWithErrors;

		private int totalSentencesWithSubtitutions;

		private int totalSentencesWithInsertions;

		private int totalSentencesWithDeletions;

		private int totalReferenceWords;

		private int totalHypothesisWords;

		private int totalAlignedWords;

		private int totalWordsCorrect;

		private int totalSubstitutions;

		private int totalInsertions;

		private int totalDeletions;

		private int substitutions;

		private int insertions;

		private int deletions;

		private int correct;

		private string rawReference;

		private string referenceAnnotation;

		private LinkedList referenceItems;

		private LinkedList alignedReferenceWords;

		private string rawHypothesis;

		private LinkedList hypothesisItems;
		
		private LinkedList alignedHypothesisWords;

		internal static DecimalFormat percentageFormat = new DecimalFormat("##0.0%");

		private bool showResults;

		private bool showAlignedResults;

		internal interface Comparator
		{
			bool isSimilar(object a, object b);
		}

		public interface StringRenderer
		{
			string getRef(object obj1, object obj2);

			string getHyp(object obj1, object obj2);
		}
	}
}
