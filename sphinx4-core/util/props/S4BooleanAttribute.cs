﻿using System;
using System.ComponentModel;
using IKVM.Attributes;
using ikvm.@internal;

namespace edu.cmu.sphinx.util.props
{
	
	
	[Implements(new string[]
	{
		"edu.cmu.sphinx.util.props.S4Boolean"
	})]
	[AttributeUsage(AttributeTargets.Field)]
	public sealed class S4BooleanAttribute : AnnotationAttributeBase, S4Boolean
	{
		
		public S4BooleanAttribute(object[] definition) : this()
		{
			base.setDefinition(definition);
		}

		private S4BooleanAttribute() : base(ClassLiteral<S4Boolean>.Value)
		{
		}

		
		public S4BooleanAttribute(bool defaultValue) : this()
		{
			base.setValue("defaultValue", defaultValue);
		}

		bool S4Boolean.defaultValue()
		{
			return base.getBooleanValue("defaultValue");
		}
	}
}
