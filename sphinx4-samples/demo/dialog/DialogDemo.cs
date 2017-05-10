using System;
using System.Runtime.CompilerServices;
using edu.cmu.sphinx.api;
using IKVM.Attributes;
using java.lang;
using java.util;

namespace edu.cmu.sphinx.demo.dialog
{
	public class DialogDemo : java.lang.Object
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		public static void __<clinit>()
		{
		}

		[LineNumberTable(new byte[]
		{
			0,
			134,
			103,
			111,
			142,
			244,
			60,
			230,
			71
		})]
		[MethodImpl(MethodImplOptions.NoInlining)]
		private static double parseNumber(string[] array)
		{
			StringBuilder stringBuilder = new StringBuilder();
			for (int i = 1; i < array.Length; i++)
			{
				if (java.lang.String.instancehelper_equals(array[i], "point"))
				{
					stringBuilder.append(".");
				}
				else
				{
					stringBuilder.append(DialogDemo.DIGITS.get(array[i]));
				}
			}
			return java.lang.Double.parseDouble(stringBuilder.toString());
		}

		[LineNumberTable(new byte[]
		{
			12,
			111,
			111,
			111,
			111,
			143,
			135,
			108,
			115,
			103,
			130,
			107,
			98,
			102
		})]
		[MethodImpl(MethodImplOptions.NoInlining)]
		private static void recognizeDigits(LiveSpeechRecognizer liveSpeechRecognizer)
		{
			java.lang.System.@out.println("Digits recognition (using GrXML)");
			java.lang.System.@out.println("--------------------------------");
			java.lang.System.@out.println("Example: one two three");
			java.lang.System.@out.println("Say \"101\" to exit");
			java.lang.System.@out.println("--------------------------------");
			liveSpeechRecognizer.startRecognition(true);
			for (;;)
			{
				string hypothesis = liveSpeechRecognizer.getResult().getHypothesis();
				if (java.lang.String.instancehelper_equals(hypothesis, "one zero one"))
				{
					break;
				}
				if (java.lang.String.instancehelper_equals(hypothesis, "one oh one"))
				{
					break;
				}
				java.lang.System.@out.println(hypothesis);
			}
			liveSpeechRecognizer.stopRecognition();
		}

		[LineNumberTable(new byte[]
		{
			31,
			111,
			111,
			111,
			111,
			111,
			111,
			143,
			102,
			167,
			108,
			109,
			101,
			109,
			114,
			101,
			127,
			0,
			114,
			114,
			101,
			127,
			0,
			111,
			191,
			5,
			127,
			0,
			133,
			102
		})]
		[MethodImpl(MethodImplOptions.NoInlining)]
		private static void recognizerBankAccount(LiveSpeechRecognizer liveSpeechRecognizer)
		{
			java.lang.System.@out.println("This is bank account voice menu");
			java.lang.System.@out.println("-------------------------------");
			java.lang.System.@out.println("Example: balance");
			java.lang.System.@out.println("Example: withdraw zero point five");
			java.lang.System.@out.println("Example: deposit one two three");
			java.lang.System.@out.println("Example: back");
			java.lang.System.@out.println("-------------------------------");
			double num = (double)0f;
			liveSpeechRecognizer.startRecognition(true);
			for (;;)
			{
				string hypothesis = liveSpeechRecognizer.getResult().getHypothesis();
				if (java.lang.String.instancehelper_endsWith(hypothesis, "back"))
				{
					break;
				}
				if (java.lang.String.instancehelper_startsWith(hypothesis, "deposit"))
				{
					double num2 = DialogDemo.parseNumber(java.lang.String.instancehelper_split(hypothesis, "\\s"));
					num += num2;
					java.lang.System.@out.format("Deposited: $%.2f\n", new object[]
					{
						java.lang.Double.valueOf(num2)
					});
				}
				else if (java.lang.String.instancehelper_startsWith(hypothesis, "withdraw"))
				{
					double num2 = DialogDemo.parseNumber(java.lang.String.instancehelper_split(hypothesis, "\\s"));
					num -= num2;
					java.lang.System.@out.format("Withdrawn: $%.2f\n", new object[]
					{
						java.lang.Double.valueOf(num2)
					});
				}
				else if (!java.lang.String.instancehelper_endsWith(hypothesis, "balance"))
				{
					java.lang.System.@out.println(new StringBuilder().append("Unrecognized command: ").append(hypothesis).toString());
				}
				java.lang.System.@out.format("Your savings: $%.2f\n", new object[]
				{
					java.lang.Double.valueOf(num)
				});
			}
			liveSpeechRecognizer.stopRecognition();
		}

		[LineNumberTable(new byte[]
		{
			65,
			111,
			111,
			111,
			111,
			143,
			135,
			108,
			109,
			130,
			107,
			98,
			102
		})]
		[MethodImpl(MethodImplOptions.NoInlining)]
		private static void recognizeWeather(LiveSpeechRecognizer liveSpeechRecognizer)
		{
			java.lang.System.@out.println("Try some forecast. End with \"the end\"");
			java.lang.System.@out.println("-------------------------------------");
			java.lang.System.@out.println("Example: mostly dry some fog patches tonight");
			java.lang.System.@out.println("Example: sunny spells on wednesday");
			java.lang.System.@out.println("-------------------------------------");
			liveSpeechRecognizer.startRecognition(true);
			for (;;)
			{
				string hypothesis = liveSpeechRecognizer.getResult().getHypothesis();
				if (java.lang.String.instancehelper_equals(hypothesis, "the end"))
				{
					break;
				}
				java.lang.System.@out.println(hypothesis);
			}
			liveSpeechRecognizer.stopRecognition();
		}

		[LineNumberTable(21)]
		[MethodImpl(MethodImplOptions.NoInlining)]
		public DialogDemo()
		{
		}

		[Throws(new string[]
		{
			"java.lang.Exception"
		})]
		[LineNumberTable(new byte[]
		{
			83,
			102,
			107,
			107,
			107,
			135,
			107,
			167,
			107,
			167,
			103,
			107,
			167,
			135,
			111,
			111,
			111,
			111,
			143,
			141,
			110,
			133,
			110,
			102,
			102,
			167,
			110,
			102,
			102,
			167,
			110,
			102,
			102,
			135,
			133,
			102
		})]
		[MethodImpl(MethodImplOptions.NoInlining)]
		public static void main(string[] args)
		{
			Configuration configuration = new Configuration();
			configuration.setAcousticModelPath("resource:/edu/cmu/sphinx/models/en-us/en-us");
			configuration.setDictionaryPath("resource:/edu/cmu/sphinx/models/en-us/cmudict-en-us.dict");
			configuration.setGrammarPath("resource:/edu/cmu/sphinx/demo/dialog/");
			configuration.setUseGrammar(true);
			configuration.setGrammarName("dialog");
			LiveSpeechRecognizer liveSpeechRecognizer = new LiveSpeechRecognizer(configuration);
			configuration.setGrammarName("digits.grxml");
			LiveSpeechRecognizer liveSpeechRecognizer2 = new LiveSpeechRecognizer(configuration);
			configuration.setUseGrammar(false);
			configuration.setLanguageModelPath("resource:/edu/cmu/sphinx/demo/dialog/weather.lm");
			LiveSpeechRecognizer liveSpeechRecognizer3 = new LiveSpeechRecognizer(configuration);
			liveSpeechRecognizer.startRecognition(true);
			for (;;)
			{
				java.lang.System.@out.println("Choose menu item:");
				java.lang.System.@out.println("Example: go to the bank account");
				java.lang.System.@out.println("Example: exit the program");
				java.lang.System.@out.println("Example: weather forecast");
				java.lang.System.@out.println("Example: digits\n");
				string hypothesis = liveSpeechRecognizer.getResult().getHypothesis();
				if (java.lang.String.instancehelper_startsWith(hypothesis, "exit"))
				{
					break;
				}
				if (java.lang.String.instancehelper_equals(hypothesis, "digits"))
				{
					liveSpeechRecognizer.stopRecognition();
					DialogDemo.recognizeDigits(liveSpeechRecognizer2);
					liveSpeechRecognizer.startRecognition(true);
				}
				if (java.lang.String.instancehelper_equals(hypothesis, "bank account"))
				{
					liveSpeechRecognizer.stopRecognition();
					DialogDemo.recognizerBankAccount(liveSpeechRecognizer);
					liveSpeechRecognizer.startRecognition(true);
				}
				if (java.lang.String.instancehelper_endsWith(hypothesis, "weather forecast"))
				{
					liveSpeechRecognizer.stopRecognition();
					DialogDemo.recognizeWeather(liveSpeechRecognizer3);
					liveSpeechRecognizer.startRecognition(true);
				}
			}
			liveSpeechRecognizer.stopRecognition();
		}

		[LineNumberTable(new byte[]
		{
			159,
			174,
			202,
			118,
			118,
			118,
			118,
			118,
			118,
			118,
			118,
			118,
			118,
			119
		})]
		static DialogDemo()
		{
			DialogDemo.DIGITS.put("oh", Integer.valueOf(0));
			DialogDemo.DIGITS.put("zero", Integer.valueOf(0));
			DialogDemo.DIGITS.put("one", Integer.valueOf(1));
			DialogDemo.DIGITS.put("two", Integer.valueOf(2));
			DialogDemo.DIGITS.put("three", Integer.valueOf(3));
			DialogDemo.DIGITS.put("four", Integer.valueOf(4));
			DialogDemo.DIGITS.put("five", Integer.valueOf(5));
			DialogDemo.DIGITS.put("six", Integer.valueOf(6));
			DialogDemo.DIGITS.put("seven", Integer.valueOf(7));
			DialogDemo.DIGITS.put("eight", Integer.valueOf(8));
			DialogDemo.DIGITS.put("nine", Integer.valueOf(9));
		}

		private const string ACOUSTIC_MODEL = "resource:/edu/cmu/sphinx/models/en-us/en-us";

		private const string DICTIONARY_PATH = "resource:/edu/cmu/sphinx/models/en-us/cmudict-en-us.dict";

		private const string GRAMMAR_PATH = "resource:/edu/cmu/sphinx/demo/dialog/";

		private const string LANGUAGE_MODEL = "resource:/edu/cmu/sphinx/demo/dialog/weather.lm";

		[Modifiers((Modifiers)26)]
		[Signature("Ljava/util/Map<Ljava/lang/String;Ljava/lang/Integer;>;")]
		private static Map DIGITS = new HashMap();
	}
}
