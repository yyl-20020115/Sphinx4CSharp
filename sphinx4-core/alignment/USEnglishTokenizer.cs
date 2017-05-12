using edu.cmu.sphinx.alignment.tokenizer;
using java.io;
using java.lang;
using java.util;
using java.util.regex;

namespace edu.cmu.sphinx.alignment
{
	public class USEnglishTokenizer : Object, TextTokenizer
	{
		private string simplifyChars(string text)
		{
			text = String.instancehelper_replace(text, '’', '\'');
			text = String.instancehelper_replace(text, '‘', '\'');
			text = String.instancehelper_replace(text, '”', '"');
			text = String.instancehelper_replace(text, '“', '"');
			text = String.instancehelper_replace(text, '»', '"');
			text = String.instancehelper_replace(text, '«', '"');
			text = String.instancehelper_replace(text, '–', '-');
			text = String.instancehelper_replace(text, '—', ' ');
			text = String.instancehelper_replace(text, '…', ' ');
			text = String.instancehelper_replace(text, '\f', ' ');
			return text;
		}

		private void tokenToWords(string text)
		{
			FeatureSet features = this.tokenItem.getFeatures();
			string @string = features.getString("name");
			int num = String.instancehelper_length(text);
			if (features.isPresent("phones"))
			{
				this.wordRelation.addWord(text);
			}
			else if ((String.instancehelper_equals(text, "a") || String.instancehelper_equals(text, "A")) && (this.tokenItem.getNext() == null || !String.instancehelper_equals(text, @string) || !String.instancehelper_equals((string)this.tokenItem.findFeature("punc"), "")))
			{
				this.wordRelation.addWord("_a");
			}
			else if (USEnglishTokenizer.matches(USEnglishTokenizer.alphabetPattern, text))
			{
				if (USEnglishTokenizer.matches(USEnglishTokenizer.romanNumbersPattern, text))
				{
					this.romanToWords(text);
				}
				else if (USEnglishTokenizer.matches(USEnglishTokenizer.illionPattern, text) && USEnglishTokenizer.matches(USEnglishTokenizer.usMoneyPattern, (string)this.tokenItem.findFeature("p.name")))
				{
					this.wordRelation.addWord(text);
					this.wordRelation.addWord("dollars");
				}
				else if (USEnglishTokenizer.matches(USEnglishTokenizer.drStPattern, text))
				{
					this.drStToWords(text);
				}
				else if (String.instancehelper_equals(text, "Mr"))
				{
					this.tokenItem.getFeatures().setString("punc", "");
					this.wordRelation.addWord("mister");
				}
				else if (String.instancehelper_equals(text, "Mrs"))
				{
					this.tokenItem.getFeatures().setString("punc", "");
					this.wordRelation.addWord("missus");
				}
				else if (num == 1 && Character.isUpperCase(String.instancehelper_charAt(text, 0)) && String.instancehelper_equals((string)this.tokenItem.findFeature("n.whitespace"), " ") && Character.isUpperCase(String.instancehelper_charAt((string)this.tokenItem.findFeature("n.name"), 0)))
				{
					features.setString("punc", "");
					string text2 = String.instancehelper_toLowerCase(text);
					if (String.instancehelper_equals(text2, "a"))
					{
						this.wordRelation.addWord("_a");
					}
					else
					{
						this.wordRelation.addWord(text2);
					}
				}
				else if (!this.isStateName(text))
				{
					if (num > 1 && !this.isPronounceable(text))
					{
						NumberExpander.expandLetters(text, this.wordRelation);
					}
					else
					{
						this.wordRelation.addWord(String.instancehelper_toLowerCase(text));
					}
				}
			}
			else if (USEnglishTokenizer.matches(USEnglishTokenizer.dottedAbbrevPattern, text))
			{
				object obj = ".";
				object obj2 = "";

				CharSequence charSequence = CharSequence.Cast(obj);

				CharSequence charSequence2 = CharSequence.Cast(obj2);

				NumberExpander.expandLetters(String.instancehelper_replace(text, charSequence2, charSequence), this.wordRelation);
			}
			else if (USEnglishTokenizer.matches(USEnglishTokenizer.commaIntPattern, text))
			{
				object obj4 = ",";
				object obj3 = "";
				object obj2 = obj4;
				CharSequence charSequence = CharSequence.Cast(obj4);
				CharSequence charSequence3 = CharSequence.Cast(obj3);
				string text3 = String.instancehelper_replace(text, charSequence3, charSequence);
				object obj5 = "'";
				obj2 = "";
				charSequence = CharSequence.Cast(obj5);
				CharSequence charSequence4 = charSequence;
				charSequence = CharSequence.Cast(obj2);
				NumberExpander.expandReal(String.instancehelper_replace(text3, charSequence4, charSequence), this.wordRelation);
			}
			else if (USEnglishTokenizer.matches(USEnglishTokenizer.sevenPhoneNumberPattern, text))
			{
				int num2 = String.instancehelper_indexOf(text, 45);
				string numberString = String.instancehelper_substring(text, 0, num2);
				string text4 = String.instancehelper_substring(text, num2 + 1);
				NumberExpander.expandDigits(numberString, this.wordRelation);
				this.wordRelation.addBreak();
				NumberExpander.expandDigits(text4, this.wordRelation);
			}
			else if (this.matchesPartPhoneNumber(text))
			{
				string text2 = (string)this.tokenItem.findFeature("punc");
				if (String.instancehelper_equals(text2, ""))
				{
					this.tokenItem.getFeatures().setString("punc", ",");
				}
				NumberExpander.expandDigits(text, this.wordRelation);
				this.wordRelation.addBreak();
			}
			else if (USEnglishTokenizer.matches(USEnglishTokenizer.numberTimePattern, text))
			{
				int num2 = String.instancehelper_indexOf(text, 58);
				string numberString = String.instancehelper_substring(text, 0, num2);
				string text4 = String.instancehelper_substring(text, num2 + 1);
				NumberExpander.expandNumber(numberString, this.wordRelation);
				if (!String.instancehelper_equals(text4, "00"))
				{
					NumberExpander.expandID(text4, this.wordRelation);
				}
			}
			else if (USEnglishTokenizer.matches(USEnglishTokenizer.digits2DashPattern, text))
			{
				this.digitsDashToWords(text);
			}
			else if (USEnglishTokenizer.matches(USEnglishTokenizer.digitsPattern, text))
			{
				this.digitsToWords(text);
			}
			else if (num == 1 && Character.isUpperCase(String.instancehelper_charAt(text, 0)) && String.instancehelper_equals((string)this.tokenItem.findFeature("n.whitespace"), " ") && Character.isUpperCase(String.instancehelper_charAt((string)this.tokenItem.findFeature("n.name"), 0)))
			{
				features.setString("punc", "");
				string text2 = String.instancehelper_toLowerCase(text);
				if (String.instancehelper_equals(text2, "a"))
				{
					this.wordRelation.addWord("_a");
				}
				else
				{
					this.wordRelation.addWord(text2);
				}
			}
			else if (USEnglishTokenizer.matches(USEnglishTokenizer.doublePattern, text))
			{
				NumberExpander.expandReal(text, this.wordRelation);
			}
			else if (USEnglishTokenizer.matches(USEnglishTokenizer.ordinalPattern, text))
			{
				string text2 = String.instancehelper_substring(text, 0, num - 2);
				NumberExpander.expandOrdinal(text2, this.wordRelation);
			}
			else if (USEnglishTokenizer.matches(USEnglishTokenizer.usMoneyPattern, text))
			{
				this.usMoneyToWords(text);
			}
			else if (num > 0 && String.instancehelper_charAt(text, num - 1) == '%')
			{
				this.tokenToWords(String.instancehelper_substring(text, 0, num - 1));
				this.wordRelation.addWord("percent");
			}
			else if (USEnglishTokenizer.matches(USEnglishTokenizer.numessPattern, text))
			{
				NumberExpander.expandNumess(String.instancehelper_substring(text, 0, num - 1), this.wordRelation);
			}
			else if (USEnglishTokenizer.matches(USEnglishTokenizer.digitsSlashDigitsPattern, text) && String.instancehelper_equals(text, @string))
			{
				this.digitsSlashDigitsToWords(text);
			}
			else if (String.instancehelper_indexOf(text, 45) != -1)
			{
				this.dashToWords(text);
			}
			else if (num > 1 && !USEnglishTokenizer.matches(USEnglishTokenizer.alphabetPattern, text))
			{
				this.notJustAlphasToWords(text);
			}
			else if (String.instancehelper_equals(text, "&"))
			{
				this.wordRelation.addWord("and");
			}
			else if (!String.instancehelper_equals(text, "-"))
			{
				this.wordRelation.addWord(String.instancehelper_toLowerCase(text));
			}
		}

		private static bool matches(Pattern pattern, string _ref)
		{
			CharSequence charSequence = CharSequence.Cast(_ref);
			Matcher matcher = pattern.matcher(charSequence);
			return matcher.matches();
		}

		private void romanToWords(string text)
		{
			string text2 = (string)this.tokenItem.findFeature("p.punc");
			if (String.instancehelper_equals(text2, ""))
			{
				string text3 = String.valueOf(NumberExpander.expandRoman(text));
				if (USEnglishTokenizer.kingLike(this.tokenItem))
				{
					this.wordRelation.addWord("the");
					NumberExpander.expandOrdinal(text3, this.wordRelation);
				}
				else if (USEnglishTokenizer.sectionLike(this.tokenItem))
				{
					NumberExpander.expandNumber(text3, this.wordRelation);
				}
				else
				{
					NumberExpander.expandLetters(text, this.wordRelation);
				}
			}
			else
			{
				NumberExpander.expandLetters(text, this.wordRelation);
			}
		}

		private void drStToWords(string text)
		{
			int num = (int)String.instancehelper_charAt(text, 0);
			string word;
			string word2;
			if (num == 115 || num == 83)
			{
				word = "street";
				word2 = "saint";
			}
			else
			{
				word = "drive";
				word2 = "doctor";
			}
			FeatureSet features = this.tokenItem.getFeatures();
			string @string = features.getString("punc");
			string text2 = (string)this.tokenItem.findFeature("punc");
			if (this.tokenItem.getNext() == null || String.instancehelper_indexOf(@string, 44) != -1)
			{
				this.wordRelation.addWord(word);
			}
			else if (String.instancehelper_equals(text2, ","))
			{
				this.wordRelation.addWord(word2);
			}
			else
			{
				string text3 = (string)this.tokenItem.findFeature("p.name");
				string text4 = (string)this.tokenItem.findFeature("n.name");
				int num2 = (int)String.instancehelper_charAt(text3, 0);
				int num3 = (int)String.instancehelper_charAt(text4, 0);
				if (Character.isUpperCase((char)num2) && Character.isLowerCase((char)num3))
				{
					this.wordRelation.addWord(word);
				}
				else if (Character.isDigit((char)num2) && Character.isLowerCase((char)num3))
				{
					this.wordRelation.addWord(word);
				}
				else if (Character.isLowerCase((char)num2) && Character.isUpperCase((char)num3))
				{
					this.wordRelation.addWord(word2);
				}
				else
				{
					string text5 = (string)this.tokenItem.findFeature("n.whitespace");
					if (String.instancehelper_equals(text5, " "))
					{
						this.wordRelation.addWord(word2);
					}
					else
					{
						this.wordRelation.addWord(word);
					}
				}
			}
			if (@string != null && String.instancehelper_equals(@string, "."))
			{
				features.setString("punc", "");
			}
		}

		private bool isStateName(string text)
		{
			string[] array = (string[])((string[])USEnglishTokenizer.usStatesMap.get(text));
			if (array != null)
			{
				int num4;
				if (String.instancehelper_equals(array[1], "ambiguous"))
				{
					string text2 = (string)this.tokenItem.findFeature("p.name");
					string text3 = (string)this.tokenItem.findFeature("n.name");
					int num = String.instancehelper_length(text3);
					FeatureSet features = this.tokenItem.getFeatures();
					int num2 = (!Character.isUpperCase(String.instancehelper_charAt(text2, 0)) || String.instancehelper_length(text2) <= 2 || !USEnglishTokenizer.matches(USEnglishTokenizer.alphabetPattern, text2) || !Object.instancehelper_equals(this.tokenItem.findFeature("p.punc"), ",")) ? 0 : 1;
					int num3 = (!Character.isLowerCase(String.instancehelper_charAt(text3, 0)) && this.tokenItem.getNext() != null && !String.instancehelper_equals(features.getString("punc"), ".") && ((num != 5 && num != 10) || !USEnglishTokenizer.matches(USEnglishTokenizer.digitsPattern, text3))) ? 0 : 1;
					if (num2 != 0 && num3 != 0)
					{
						num4 = 1;
					}
					else
					{
						num4 = 0;
					}
				}
				else
				{
					num4 = 1;
				}
				if (num4 != 0)
				{
					for (int i = 2; i < array.Length; i++)
					{
						if (array[i] != null)
						{
							this.wordRelation.addWord(array[i]);
						}
					}
					return true;
				}
			}
			return false;
		}

		public virtual bool isPronounceable(string word)
		{
			string inputString = String.instancehelper_toLowerCase(word);
			return this.prefixFSM.accept(inputString) && this.suffixFSM.accept(inputString);
		}
		private bool matchesPartPhoneNumber(string text)
		{
			string text2 = (string)this.tokenItem.findFeature("n.name");
			string text3 = (string)this.tokenItem.findFeature("n.n.name");
			string text4 = (string)this.tokenItem.findFeature("p.name");
			string text5 = (string)this.tokenItem.findFeature("p.p.name");
			int num = USEnglishTokenizer.matches(USEnglishTokenizer.threeDigitsPattern, text4) ? 1 : 0;
			return (USEnglishTokenizer.matches(USEnglishTokenizer.threeDigitsPattern, text) && ((!USEnglishTokenizer.matches(USEnglishTokenizer.digitsPattern, text4) && USEnglishTokenizer.matches(USEnglishTokenizer.threeDigitsPattern, text2) && USEnglishTokenizer.matches(USEnglishTokenizer.fourDigitsPattern, text3)) || USEnglishTokenizer.matches(USEnglishTokenizer.sevenPhoneNumberPattern, text2) || (!USEnglishTokenizer.matches(USEnglishTokenizer.digitsPattern, text5) && num != 0 && USEnglishTokenizer.matches(USEnglishTokenizer.fourDigitsPattern, text2)))) || (USEnglishTokenizer.matches(USEnglishTokenizer.fourDigitsPattern, text) && !USEnglishTokenizer.matches(USEnglishTokenizer.digitsPattern, text2) && num != 0 && USEnglishTokenizer.matches(USEnglishTokenizer.threeDigitsPattern, text5));
		}

		private void digitsDashToWords(string text)
		{
			int num = String.instancehelper_length(text);
			int num2 = 0;
			for (int i = 0; i <= num; i++)
			{
				if (i == num || String.instancehelper_charAt(text, i) == '-')
				{
					string numberString = String.instancehelper_substring(text, num2, i);
					NumberExpander.expandDigits(numberString, this.wordRelation);
					this.wordRelation.addBreak();
					num2 = i + 1;
				}
			}
		}

		private void digitsToWords(string text)
		{
			FeatureSet features = this.tokenItem.getFeatures();
			string text2 = "";
			if (features.isPresent("nsw"))
			{
				text2 = features.getString("nsw");
			}
			if (String.instancehelper_equals(text2, "nide"))
			{
				NumberExpander.expandID(text, this.wordRelation);
			}
			else
			{
				string @string = features.getString("name");
				string text3;
				if (String.instancehelper_equals(text, @string))
				{
					text3 = (string)this.cart.interpret(this.tokenItem);
				}
				else
				{
					features.setString("name", text);
					text3 = (string)this.cart.interpret(this.tokenItem);
					features.setString("name", @string);
				}
				if (String.instancehelper_equals(text3, "ordinal"))
				{
					NumberExpander.expandOrdinal(text, this.wordRelation);
				}
				else if (String.instancehelper_equals(text3, "digits"))
				{
					NumberExpander.expandDigits(text, this.wordRelation);
				}
				else if (String.instancehelper_equals(text3, "year"))
				{
					NumberExpander.expandID(text, this.wordRelation);
				}
				else
				{
					NumberExpander.expandNumber(text, this.wordRelation);
				}
			}
		}

		private void usMoneyToWords(string text)
		{
			int num = String.instancehelper_indexOf(text, 46);
			if (USEnglishTokenizer.matches(USEnglishTokenizer.illionPattern, (string)this.tokenItem.findFeature("n.name")))
			{
				NumberExpander.expandReal(String.instancehelper_substring(text, 1), this.wordRelation);
			}
			else if (num == -1)
			{
				string text2 = String.instancehelper_substring(text, 1);
				this.tokenToWords(text2);
				if (String.instancehelper_equals(text2, "1"))
				{
					this.wordRelation.addWord("dollar");
				}
				else
				{
					this.wordRelation.addWord("dollars");
				}
			}
			else if (num == String.instancehelper_length(text) - 1 || String.instancehelper_length(text) - num > 3)
			{
				NumberExpander.expandReal(String.instancehelper_substring(text, 1), this.wordRelation);
				this.wordRelation.addWord("dollars");
			}
			else
			{
				string text3 = String.instancehelper_substring(text, 1, num);
				object obj = ",";
				object obj2 = "";
				CharSequence charSequence = CharSequence.Cast(obj);
				CharSequence charSequence2 = CharSequence.Cast(obj2);
				string text2 = String.instancehelper_replace(text3, charSequence2, charSequence);
				string text4 = String.instancehelper_substring(text, num + 1);
				NumberExpander.expandNumber(text2, this.wordRelation);
				if (String.instancehelper_equals(text2, "1"))
				{
					this.wordRelation.addWord("dollar");
				}
				else
				{
					this.wordRelation.addWord("dollars");
				}
				if (!String.instancehelper_equals(text4, "00"))
				{
					NumberExpander.expandNumber(text4, this.wordRelation);
					if (String.instancehelper_equals(text4, "01"))
					{
						this.wordRelation.addWord("cent");
					}
					else
					{
						this.wordRelation.addWord("cents");
					}
				}
			}
		}

		private void digitsSlashDigitsToWords(string text)
		{
			int num = String.instancehelper_indexOf(text, 47);
			string text2 = String.instancehelper_substring(text, 0, num);
			string text3 = String.instancehelper_substring(text, num + 1);
			if (USEnglishTokenizer.matches(USEnglishTokenizer.digitsPattern, (string)this.tokenItem.findFeature("p.name")) && this.tokenItem.getPrevious() != null)
			{
				this.wordRelation.addWord("and");
			}
			int num2;
			if (String.instancehelper_equals(text2, "1") && String.instancehelper_equals(text3, "2"))
			{
				this.wordRelation.addWord("a");
				this.wordRelation.addWord("half");
			}
			else if ((num2 = Integer.parseInt(text2)) < Integer.parseInt(text3))
			{
				NumberExpander.expandNumber(text2, this.wordRelation);
				NumberExpander.expandOrdinal(text3, this.wordRelation);
				if (num2 > 1)
				{
					this.wordRelation.addWord("'s");
				}
			}
			else
			{
				NumberExpander.expandNumber(text2, this.wordRelation);
				this.wordRelation.addWord("slash");
				NumberExpander.expandNumber(text3, this.wordRelation);
			}
		}

		private void dashToWords(string text)
		{
			int num = String.instancehelper_indexOf(text, 45);
			string text2 = String.instancehelper_substring(text, 0, num);
			string text3 = String.instancehelper_substring(text, num + 1, String.instancehelper_length(text));
			if (USEnglishTokenizer.matches(USEnglishTokenizer.digitsPattern, text2) && USEnglishTokenizer.matches(USEnglishTokenizer.digitsPattern, text3))
			{
				FeatureSet features = this.tokenItem.getFeatures();
				features.setString("name", text2);
				this.tokenToWords(text2);
				this.wordRelation.addWord("to");
				features.setString("name", text3);
				this.tokenToWords(text3);
				features.setString("name", "");
			}
			else
			{
				this.tokenToWords(text2);
				this.tokenToWords(text3);
			}
		}

		private void notJustAlphasToWords(string text)
		{
			int i = 0;
			int num = String.instancehelper_length(text);
			while (i < num - 1)
			{
				if (USEnglishTokenizer.isTextSplitable(text, i))
				{
					break;
				}
				i++;
			}
			if (i == num - 1)
			{
				this.wordRelation.addWord(String.instancehelper_toLowerCase(text));
				return;
			}
			string text2 = String.instancehelper_substring(text, 0, i + 1);
			string text3 = String.instancehelper_substring(text, i + 1, num);
			FeatureSet features = this.tokenItem.getFeatures();
			features.setString("nsw", "nide");
			this.tokenToWords(text2);
			this.tokenToWords(text3);
		}

		public static bool kingLike(Item tokenItem)
		{
			string text = String.instancehelper_toLowerCase((string)tokenItem.findFeature("p.name"));
			if (USEnglishTokenizer.inKingSectionLikeMap(text, "kingNames"))
			{
				return true;
			}
			string text2 = String.instancehelper_toLowerCase((string)tokenItem.findFeature("p.p.name"));
			return USEnglishTokenizer.inKingSectionLikeMap(text2, "kingTitles");
		}

		public static bool sectionLike(Item tokenItem)
		{
			string text = String.instancehelper_toLowerCase((string)tokenItem.findFeature("p.name"));
			return USEnglishTokenizer.inKingSectionLikeMap(text, "sectionTypes");
		}

		private static bool inKingSectionLikeMap(string text, string text2)
		{
			return USEnglishTokenizer.kingSectionLikeMap.containsKey(text) && String.instancehelper_equals((string)USEnglishTokenizer.kingSectionLikeMap.get(text), text2);
		}
		private static bool isTextSplitable(string text, int num)
		{
			int num2 = (int)String.instancehelper_charAt(text, num);
			int num3 = (int)String.instancehelper_charAt(text, num + 1);
			return (!Character.isLetter((char)num2) || !Character.isLetter((char)num3)) && (!Character.isDigit((char)num2) || !Character.isDigit((char)num3)) && num2 != 39 && !Character.isLetter((char)num3) && num3 != 39 && !Character.isLetter((char)num2);
		}

		public USEnglishTokenizer()
		{
			this.prefixFSM = null;
			this.suffixFSM = null;
			try
			{
				this.cart = new DecisionTree(Object.instancehelper_getClass(this).getResource("nums_cart.txt"));
				this.prefixFSM = new PrefixFSM(Object.instancehelper_getClass(this).getResource("prefix_fsm.txt"));
				this.suffixFSM = new SuffixFSM(Object.instancehelper_getClass(this).getResource("suffix_fsm.txt"));
			}
			catch (IOException ex)
			{
				throw new IllegalStateException("resources not found", ex);
			}
		}

		public virtual Item getTokenItem()
		{
			return this.tokenItem;
		}

		public virtual List expand(string text)
		{
			string inputText = this.simplifyChars(text);
			CharTokenizer charTokenizer = new CharTokenizer();
			charTokenizer.setWhitespaceSymbols(" \t\n\r");
			charTokenizer.setSingleCharSymbols("");
			charTokenizer.setPrepunctuationSymbols("\"'`({[");
			charTokenizer.setPostpunctuationSymbols("\"'`.,:;!?(){}[]");
			charTokenizer.setInputText(inputText);
			Utterance utterance = new Utterance(charTokenizer);
			Relation relation;
			if ((relation = utterance.getRelation("Token")) == null)
			{
				string text2 = "token relation does not exist";

				throw new IllegalStateException(text2);
			}
			this.wordRelation = WordRelation.createWordRelation(utterance, this);
			this.tokenItem = relation.getHead();
			while (this.tokenItem != null)
			{
				FeatureSet features = this.tokenItem.getFeatures();
				string @string = features.getString("name");
				this.tokenToWords(@string);
				this.tokenItem = this.tokenItem.getNext();
			}
			ArrayList arrayList = new ArrayList();
			for (Item item = utterance.getRelation("Word").getHead(); item != null; item = item.getNext())
			{
				if (!String.instancehelper_isEmpty(item.toString()))
				{
					string text3 = item.toString();
					object _ref = "#";
					CharSequence charSequence = CharSequence.Cast(_ref);
					if (!String.instancehelper_contains(text3, charSequence))
					{
						arrayList.add(item.toString());
					}
				}
			}
			return arrayList;
		}
		static USEnglishTokenizer()
		{
			Pattern.compile(".*[aeiouAEIOU].*");
			USEnglishTokenizer.illionPattern = Pattern.compile(".*illion");
			USEnglishTokenizer.numberTimePattern = Pattern.compile("((0[0-2])|(1[0-9])):([0-5][0-9])");
			USEnglishTokenizer.numessPattern = Pattern.compile("[0-9]+s");
			USEnglishTokenizer.ordinalPattern = Pattern.compile(UsEnglish.RX_ORDINAL_NUMBER);
			USEnglishTokenizer.romanNumbersPattern = Pattern.compile("(II?I?|IV|VI?I?I?|IX|X[VIX]*)");
			USEnglishTokenizer.sevenPhoneNumberPattern = Pattern.compile("[0-9][0-9][0-9]-[0-9][0-9][0-9][0-9]");
			USEnglishTokenizer.threeDigitsPattern = Pattern.compile("[0-9][0-9][0-9]");
			USEnglishTokenizer.usMoneyPattern = Pattern.compile("\\_[0-9,]+(\\.[0-9]+)?");
			USEnglishTokenizer.kingNames = new string[]
			{
				"louis",
				"henry",
				"charles",
				"philip",
				"george",
				"edward",
				"pius",
				"william",
				"richard",
				"ptolemy",
				"john",
				"paul",
				"peter",
				"nicholas",
				"frederick",
				"james",
				"alfonso",
				"ivan",
				"napoleon",
				"leo",
				"gregory",
				"catherine",
				"alexandria",
				"pierre",
				"elizabeth",
				"mary",
				"elmo",
				"erasmus"
			};
			USEnglishTokenizer.kingTitles = new string[]
			{
				"king",
				"queen",
				"pope",
				"duke",
				"tsar",
				"emperor",
				"shah",
				"caesar",
				"duchess",
				"tsarina",
				"empress",
				"baron",
				"baroness",
				"sultan",
				"count",
				"countess"
			};
			USEnglishTokenizer.sectionTypes = new string[]
			{
				"section",
				"chapter",
				"part",
				"phrase",
				"verse",
				"scene",
				"act",
				"book",
				"volume",
				"chap",
				"war",
				"apollo",
				"trek",
				"fortran"
			};
			USEnglishTokenizer.kingSectionLikeMap = new HashMap();
			for (int i = 0; i < USEnglishTokenizer.kingNames.Length; i++)
			{
				USEnglishTokenizer.kingSectionLikeMap.put(USEnglishTokenizer.kingNames[i], "kingNames");
			}
			for (int i = 0; i < USEnglishTokenizer.kingTitles.Length; i++)
			{
				USEnglishTokenizer.kingSectionLikeMap.put(USEnglishTokenizer.kingTitles[i], "kingTitles");
			}
			for (int i = 0; i < USEnglishTokenizer.sectionTypes.Length; i++)
			{
				USEnglishTokenizer.kingSectionLikeMap.put(USEnglishTokenizer.sectionTypes[i], "sectionTypes");
			}
			USEnglishTokenizer.usStates = new string[][]
			{
				new string[]
				{
					"AL",
					"ambiguous",
					"alabama"
				},
				new string[]
				{
					"Al",
					"ambiguous",
					"alabama"
				},
				new string[]
				{
					"Ala",
					"",
					"alabama"
				},
				new string[]
				{
					"AK",
					"",
					"alaska"
				},
				new string[]
				{
					"Ak",
					"",
					"alaska"
				},
				new string[]
				{
					"AZ",
					"",
					"arizona"
				},
				new string[]
				{
					"Az",
					"",
					"arizona"
				},
				new string[]
				{
					"CA",
					"",
					"california"
				},
				new string[]
				{
					"Ca",
					"",
					"california"
				},
				new string[]
				{
					"Cal",
					"ambiguous",
					"california"
				},
				new string[]
				{
					"Calif",
					"",
					"california"
				},
				new string[]
				{
					"CO",
					"ambiguous",
					"colorado"
				},
				new string[]
				{
					"Co",
					"ambiguous",
					"colorado"
				},
				new string[]
				{
					"Colo",
					"",
					"colorado"
				},
				new string[]
				{
					"DC",
					"",
					"d",
					"c"
				},
				new string[]
				{
					"DE",
					"",
					"delaware"
				},
				new string[]
				{
					"De",
					"ambiguous",
					"delaware"
				},
				new string[]
				{
					"Del",
					"ambiguous",
					"delaware"
				},
				new string[]
				{
					"FL",
					"",
					"florida"
				},
				new string[]
				{
					"Fl",
					"ambiguous",
					"florida"
				},
				new string[]
				{
					"Fla",
					"",
					"florida"
				},
				new string[]
				{
					"GA",
					"",
					"georgia"
				},
				new string[]
				{
					"Ga",
					"",
					"georgia"
				},
				new string[]
				{
					"HI",
					"ambiguous",
					"hawaii"
				},
				new string[]
				{
					"Hi",
					"ambiguous",
					"hawaii"
				},
				new string[]
				{
					"IA",
					"",
					"iowa"
				},
				new string[]
				{
					"Ia",
					"ambiguous",
					"iowa"
				},
				new string[]
				{
					"IN",
					"ambiguous",
					"indiana"
				},
				new string[]
				{
					"In",
					"ambiguous",
					"indiana"
				},
				new string[]
				{
					"Ind",
					"ambiguous",
					"indiana"
				},
				new string[]
				{
					"ID",
					"ambiguous",
					"idaho"
				},
				new string[]
				{
					"IL",
					"ambiguous",
					"illinois"
				},
				new string[]
				{
					"Il",
					"ambiguous",
					"illinois"
				},
				new string[]
				{
					"ILL",
					"ambiguous",
					"illinois"
				},
				new string[]
				{
					"KS",
					"",
					"kansas"
				},
				new string[]
				{
					"Ks",
					"",
					"kansas"
				},
				new string[]
				{
					"Kans",
					"",
					"kansas"
				},
				new string[]
				{
					"KY",
					"ambiguous",
					"kentucky"
				},
				new string[]
				{
					"Ky",
					"ambiguous",
					"kentucky"
				},
				new string[]
				{
					"LA",
					"ambiguous",
					"louisiana"
				},
				new string[]
				{
					"La",
					"ambiguous",
					"louisiana"
				},
				new string[]
				{
					"Lou",
					"ambiguous",
					"louisiana"
				},
				new string[]
				{
					"Lous",
					"ambiguous",
					"louisiana"
				},
				new string[]
				{
					"MA",
					"ambiguous",
					"massachusetts"
				},
				new string[]
				{
					"Mass",
					"ambiguous",
					"massachusetts"
				},
				new string[]
				{
					"Ma",
					"ambiguous",
					"massachusetts"
				},
				new string[]
				{
					"MD",
					"ambiguous",
					"maryland"
				},
				new string[]
				{
					"Md",
					"ambiguous",
					"maryland"
				},
				new string[]
				{
					"ME",
					"ambiguous",
					"maine"
				},
				new string[]
				{
					"Me",
					"ambiguous",
					"maine"
				},
				new string[]
				{
					"MI",
					"",
					"michigan"
				},
				new string[]
				{
					"Mi",
					"ambiguous",
					"michigan"
				},
				new string[]
				{
					"Mich",
					"ambiguous",
					"michigan"
				},
				new string[]
				{
					"MN",
					"ambiguous",
					"minnestota"
				},
				new string[]
				{
					"Minn",
					"ambiguous",
					"minnestota"
				},
				new string[]
				{
					"MS",
					"ambiguous",
					"mississippi"
				},
				new string[]
				{
					"Miss",
					"ambiguous",
					"mississippi"
				},
				new string[]
				{
					"MT",
					"ambiguous",
					"montanna"
				},
				new string[]
				{
					"Mt",
					"ambiguous",
					"montanna"
				},
				new string[]
				{
					"MO",
					"ambiguous",
					"missouri"
				},
				new string[]
				{
					"Mo",
					"ambiguous",
					"missouri"
				},
				new string[]
				{
					"NC",
					"ambiguous",
					"north",
					"carolina"
				},
				new string[]
				{
					"ND",
					"ambiguous",
					"north",
					"dakota"
				},
				new string[]
				{
					"NE",
					"ambiguous",
					"nebraska"
				},
				new string[]
				{
					"Ne",
					"ambiguous",
					"nebraska"
				},
				new string[]
				{
					"Neb",
					"ambiguous",
					"nebraska"
				},
				new string[]
				{
					"NH",
					"ambiguous",
					"new",
					"hampshire"
				},
				new string[]
				{
					"NV",
					"",
					"nevada"
				},
				new string[]
				{
					"Nev",
					"",
					"nevada"
				},
				new string[]
				{
					"NY",
					"",
					"new",
					"york"
				},
				new string[]
				{
					"OH",
					"ambiguous",
					"ohio"
				},
				new string[]
				{
					"OK",
					"ambiguous",
					"oklahoma"
				},
				new string[]
				{
					"Okla",
					"",
					"oklahoma"
				},
				new string[]
				{
					"OR",
					"ambiguous",
					"oregon"
				},
				new string[]
				{
					"Or",
					"ambiguous",
					"oregon"
				},
				new string[]
				{
					"Ore",
					"ambiguous",
					"oregon"
				},
				new string[]
				{
					"PA",
					"ambiguous",
					"pennsylvania"
				},
				new string[]
				{
					"Pa",
					"ambiguous",
					"pennsylvania"
				},
				new string[]
				{
					"Penn",
					"ambiguous",
					"pennsylvania"
				},
				new string[]
				{
					"RI",
					"ambiguous",
					"rhode",
					"island"
				},
				new string[]
				{
					"SC",
					"ambiguous",
					"south",
					"carlolina"
				},
				new string[]
				{
					"SD",
					"ambiguous",
					"south",
					"dakota"
				},
				new string[]
				{
					"TN",
					"ambiguous",
					"tennesee"
				},
				new string[]
				{
					"Tn",
					"ambiguous",
					"tennesee"
				},
				new string[]
				{
					"Tenn",
					"ambiguous",
					"tennesee"
				},
				new string[]
				{
					"TX",
					"ambiguous",
					"texas"
				},
				new string[]
				{
					"Tx",
					"ambiguous",
					"texas"
				},
				new string[]
				{
					"Tex",
					"ambiguous",
					"texas"
				},
				new string[]
				{
					"UT",
					"ambiguous",
					"utah"
				},
				new string[]
				{
					"VA",
					"ambiguous",
					"virginia"
				},
				new string[]
				{
					"WA",
					"ambiguous",
					"washington"
				},
				new string[]
				{
					"Wa",
					"ambiguous",
					"washington"
				},
				new string[]
				{
					"Wash",
					"ambiguous",
					"washington"
				},
				new string[]
				{
					"WI",
					"ambiguous",
					"wisconsin"
				},
				new string[]
				{
					"Wi",
					"ambiguous",
					"wisconsin"
				},
				new string[]
				{
					"WV",
					"ambiguous",
					"west",
					"virginia"
				},
				new string[]
				{
					"WY",
					"ambiguous",
					"wyoming"
				},
				new string[]
				{
					"Wy",
					"ambiguous",
					"wyoming"
				},
				new string[]
				{
					"Wyo",
					"",
					"wyoming"
				},
				new string[]
				{
					"PR",
					"ambiguous",
					"puerto",
					"rico"
				}
			};
			USEnglishTokenizer.usStatesMap = new HashMap();
			for (int i = 0; i < USEnglishTokenizer.usStates.Length; i++)
			{
				USEnglishTokenizer.usStatesMap.put(USEnglishTokenizer.usStates[i][0], USEnglishTokenizer.usStates[i]);
			}
		}


		private static Pattern alphabetPattern = Pattern.compile(UsEnglish.RX_ALPHABET);


		private static Pattern commaIntPattern = Pattern.compile(UsEnglish.RX_COMMAINT);


		private static Pattern digits2DashPattern = Pattern.compile("[0-9]+(-[0-9]+)(-[0-9]+)+");


		private static Pattern digitsPattern = Pattern.compile(UsEnglish.RX_DIGITS);


		private static Pattern digitsSlashDigitsPattern = Pattern.compile("[0-9]+/[0-9]+");


		private static Pattern dottedAbbrevPattern = Pattern.compile(UsEnglish.RX_DOTTED_ABBREV);


		private static Pattern doublePattern = Pattern.compile(UsEnglish.RX_DOUBLE);


		private static Pattern drStPattern = Pattern.compile("([dD][Rr]|[Ss][Tt])");


		private static Pattern fourDigitsPattern = Pattern.compile("[0-9][0-9][0-9][0-9]");


		private static Pattern illionPattern;


		private static Pattern numberTimePattern;


		private static Pattern numessPattern;


		private static Pattern ordinalPattern;


		private static Pattern romanNumbersPattern;


		private static Pattern sevenPhoneNumberPattern;


		private static Pattern threeDigitsPattern;


		private static Pattern usMoneyPattern;


		private static string[] kingNames;


		private static string[] kingTitles;


		private static string[] sectionTypes;


		private static Map kingSectionLikeMap;

		private const string KING_NAMES = "kingNames";

		private const string KING_TITLES = "kingTitles";

		private const string SECTION_TYPES = "sectionTypes";

		private PronounceableFSM prefixFSM;

		private PronounceableFSM suffixFSM;


		private static string[][] usStates;


		private static Map usStatesMap;

		private WordRelation wordRelation;

		private Item tokenItem;

		private DecisionTree cart;
	}
}
