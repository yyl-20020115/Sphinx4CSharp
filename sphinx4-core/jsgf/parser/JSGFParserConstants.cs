using System;

using IKVM.Attributes;

namespace edu.cmu.sphinx.jsgf.parser
{
	public interface JSGFParserConstants
	{
		
		public static void __<clinit>()
		{
		}

		
		static JSGFParserConstants()
		{
		}

		public const int EOF = 0;

		public const int SINGLE_LINE_COMMENT = 9;

		public const int FORMAL_COMMENT = 10;

		public const int MULTI_LINE_COMMENT = 11;

		public const int GRAMMAR = 13;

		public const int IMPORT = 14;

		public const int PUBLIC = 15;

		public const int INTEGER_LITERAL = 16;

		public const int DECIMAL_LITERAL = 17;

		public const int FLOATING_POINT_LITERAL = 18;

		public const int EXPONENT = 19;

		public const int CHARACTER_LITERAL = 20;

		public const int STRING_LITERAL = 21;

		public const int TAG = 22;

		public const int IDENTIFIER = 23;

		public const int LETTER = 24;

		public const int DIGIT = 25;

		public const int DEFAULT = 0;

		public const int IN_SINGLE_LINE_COMMENT = 1;

		public const int IN_FORMAL_COMMENT = 2;

		public const int IN_MULTI_LINE_COMMENT = 3;

		public static readonly string[] tokenImage = new string[]
		{
			"<EOF>",
			"\" \"",
			"\"\\t\"",
			"\"\\n\"",
			"\"\\r\"",
			"\"\\f\"",
			"\"//\"",
			"<token of kind 7>",
			"\"/*\"",
			"<SINGLE_LINE_COMMENT>",
			"\"*/\"",
			"\"*/\"",
			"<token of kind 12>",
			"\"grammar\"",
			"\"import\"",
			"\"public\"",
			"<INTEGER_LITERAL>",
			"<DECIMAL_LITERAL>",
			"<FLOATING_POINT_LITERAL>",
			"<EXPONENT>",
			"<CHARACTER_LITERAL>",
			"<STRING_LITERAL>",
			"<TAG>",
			"<IDENTIFIER>",
			"<LETTER>",
			"<DIGIT>",
			"\";\"",
			"\"V1.0\"",
			"\"<\"",
			"\".\"",
			"\"*\"",
			"\">\"",
			"\"=\"",
			"\"|\"",
			"\"/\"",
			"\"+\"",
			"\"(\"",
			"\")\"",
			"\"[\"",
			"\"]\""
		};

		
		public static class __Fields
		{
			static __Fields()
			{
			}

			public const int EOF = 0;

			public const int SINGLE_LINE_COMMENT = 9;

			public const int FORMAL_COMMENT = 10;

			public const int MULTI_LINE_COMMENT = 11;

			public const int GRAMMAR = 13;

			public const int IMPORT = 14;

			public const int PUBLIC = 15;

			public const int INTEGER_LITERAL = 16;

			public const int DECIMAL_LITERAL = 17;

			public const int FLOATING_POINT_LITERAL = 18;

			public const int EXPONENT = 19;

			public const int CHARACTER_LITERAL = 20;

			public const int STRING_LITERAL = 21;

			public const int TAG = 22;

			public const int IDENTIFIER = 23;

			public const int LETTER = 24;

			public const int DIGIT = 25;

			public const int DEFAULT = 0;

			public const int IN_SINGLE_LINE_COMMENT = 1;

			public const int IN_FORMAL_COMMENT = 2;

			public const int IN_MULTI_LINE_COMMENT = 3;

			public static readonly string[] tokenImage = JSGFParserConstants.tokenImage;
		}
	}
}
