﻿using IKVM.Attributes;
using java.lang.annotation;

namespace edu.cmu.sphinx.util.props
{
	[AnnotationAttribute("edu.cmu.sphinx.util.props.S4StringAttribute")]
	[Documented(new object[]
	{
		64,
		"Ljava/lang/annotation/Documented;"
	})]
	[Retention(new object[]
	{
		64,
		"Ljava/lang/annotation/Retention;",
		"value",
		new object[]
		{
			101,
			"Ljava/lang/annotation/RetentionPolicy, IKVM/OpenJDK/Core, Version=7/2/4630/5, Culture=neutral, PublicKeyToken=0738eb9f132ed756;",
			"RUNTIME"
		}
	})]
	[Target(new object[]
	{
		64,
		"Ljava/lang/annotation/Target;",
		"value",
		new object[]
		{
			91,
			new object[]
			{
				101,
				"Ljava/lang/annotation/ElementType, IKVM/OpenJDK/Core, Version=7/2/4630/5, Culture=neutral, PublicKeyToken=0738eb9f132ed756;",
				"FIELD"
			}
		}
	})]
	[S4Property(new object[]
	{
		64,
		"Ledu/cmu/sphinx/util/props/S4Property;"
	})]
	public interface S4String : Annotation
	{
		[AnnotationDefault("nullnullnull")]
		string defaultValue();

		[AnnotationDefault(true)]
		bool mandatory();

		[AnnotationDefault(new object[]
		{
			91
		})]
		string[] range();

		public const string NOT_DEFINED = "nullnullnull";

		
		public static class __Fields
		{
			public const string NOT_DEFINED = "nullnullnull";
		}
	}
}
