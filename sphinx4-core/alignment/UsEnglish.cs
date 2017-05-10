namespace edu.cmu.sphinx.alignment
{
	internal sealed class UsEnglish : java.lang.Object
	{
		private UsEnglish()
		{
		}

		static UsEnglish()
		{
		}

		public const string RX_DEFAULT_US_EN_WHITESPACE = "[ \n\t\r]+";

		public const string RX_DEFAULT_US_EN_ALPHABET = "[A-Za-z]+";

		public const string RX_DEFAULT_US_EN_UPPERCASE = "[A-Z]+";

		public const string RX_DEFAULT_US_EN_LOWERCASE = "[a-z]+";

		public const string RX_DEFAULT_US_EN_ALPHANUMERIC = "[0-9A-Za-z]+";

		public const string RX_DEFAULT_US_EN_IDENTIFIER = "[A-Za-z_][0-9A-Za-z_]+";

		public const string RX_DEFAULT_US_EN_INT = "-?[0-9]+";

		public const string RX_DEFAULT_US_EN_DOUBLE = "-?(([0-9]+\\.[0-9]*)|([0-9]+)|(\\.[0-9]+))([eE][---+]?[0-9]+)?";

		public const string RX_DEFAULT_US_EN_COMMAINT = "[0-9][0-9]?[0-9]?[,']([0-9][0-9][0-9][,'])*[0-9][0-9][0-9](\\.[0-9]+)?";

		public const string RX_DEFAULT_US_EN_DIGITS = "[0-9][0-9]*";

		public const string RX_DEFAULT_US_EN_DOTTED_ABBREV = "([A-Za-z]\\.)*[A-Za-z]";

		public const string RX_DEFAULT_US_EN_ORDINAL_NUMBER = "[0-9][0-9,]*(th|TH|st|ST|nd|ND|rd|RD)";

		public const string RX_DEFAULT_HAS_VOWEL = ".*[aeiouAEIOU].*";

		public const string RX_DEFAULT_US_MONEY = "\\_[0-9,]+(\\.[0-9]+)?";

		public const string RX_DEFAULT_ILLION = ".*illion";

		public const string RX_DEFAULT_DIGITS2DASH = "[0-9]+(-[0-9]+)(-[0-9]+)+";

		public const string RX_DEFAULT_DIGITSSLASHDIGITS = "[0-9]+/[0-9]+";

		public const string RX_DEFAULT_NUMBER_TIME = "((0[0-2])|(1[0-9])):([0-5][0-9])";

		public const string RX_DEFAULT_ROMAN_NUMBER = "(II?I?|IV|VI?I?I?|IX|X[VIX]*)";

		public const string RX_DEFAULT_DRST = "([dD][Rr]|[Ss][Tt])";

		public const string RX_DEFAULT_NUMESS = "[0-9]+s";

		public const string RX_DEFAULT_SEVEN_DIGIT_PHONE_NUMBER = "[0-9][0-9][0-9]-[0-9][0-9][0-9][0-9]";

		public const string RX_DEFAULT_FOUR_DIGIT = "[0-9][0-9][0-9][0-9]";

		public const string RX_DEFAULT_THREE_DIGIT = "[0-9][0-9][0-9]";

		public static string RX_WHITESPACE = "[ \n\t\r]+";

		public static string RX_ALPHABET = "[A-Za-z]+";

		public static string RX_UPPERCASE = "[A-Z]+";

		public static string RX_LOWERCASE = "[a-z]+";

		public static string RX_ALPHANUMERIC = "[0-9A-Za-z]+";

		public static string RX_IDENTIFIER = "[A-Za-z_][0-9A-Za-z_]+";

		public static string RX_INT = "-?[0-9]+";

		public static string RX_DOUBLE = "-?(([0-9]+\\.[0-9]*)|([0-9]+)|(\\.[0-9]+))([eE][---+]?[0-9]+)?";

		public static string RX_COMMAINT = "[0-9][0-9]?[0-9]?[,']([0-9][0-9][0-9][,'])*[0-9][0-9][0-9](\\.[0-9]+)?";

		public static string RX_DIGITS = "[0-9][0-9]*";

		public static string RX_DOTTED_ABBREV = "([A-Za-z]\\.)*[A-Za-z]";

		public static string RX_ORDINAL_NUMBER = "[0-9][0-9,]*(th|TH|st|ST|nd|ND|rd|RD)";

		public const string RX_HAS_VOWEL = ".*[aeiouAEIOU].*";

		public const string RX_US_MONEY = "\\_[0-9,]+(\\.[0-9]+)?";

		public const string RX_ILLION = ".*illion";

		public const string RX_DIGITS2DASH = "[0-9]+(-[0-9]+)(-[0-9]+)+";

		public const string RX_DIGITSSLASHDIGITS = "[0-9]+/[0-9]+";

		public const string RX_NUMBER_TIME = "((0[0-2])|(1[0-9])):([0-5][0-9])";

		public const string RX_ROMAN_NUMBER = "(II?I?|IV|VI?I?I?|IX|X[VIX]*)";

		public const string RX_DRST = "([dD][Rr]|[Ss][Tt])";

		public const string RX_NUMESS = "[0-9]+s";

		public const string RX_SEVEN_DIGIT_PHONE_NUMBER = "[0-9][0-9][0-9]-[0-9][0-9][0-9][0-9]";

		public const string RX_FOUR_DIGIT = "[0-9][0-9][0-9][0-9]";

		public const string RX_THREE_DIGIT = "[0-9][0-9][0-9]";

		public const string PUNCTUATION_SYMBOLS = "\"'`.,:;!?(){}[]";

		public const string PREPUNCTUATION_SYMBOLS = "\"'`({[";

		public const string SINGLE_CHAR_SYMBOLS = "";

		public const string WHITESPACE_SYMBOLS = " \t\n\r";
	}
}
