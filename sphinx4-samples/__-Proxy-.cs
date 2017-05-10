using System;
using System.ComponentModel;
using edu.cmu.sphinx.linguist.flat;
using edu.cmu.sphinx.util;
using IKVM.Attributes;
using org.apache.commons.math3.analysis.polynomials;
using org.apache.commons.math3.optim.nonlinear.scalar;
using org.apache.commons.math3.optimization.direct;

[HideFromJava]
[EditorBrowsable(EditorBrowsableState.Never)]
public interface __
{
	public interface edu_cmu_sphinx_linguist_flat_SentenceHMMStateVisitor : SentenceHMMStateVisitor
	{
	}

	public interface edu_cmu_sphinx_util_NISTAlign^1Comparator : NISTAlign.Comparator
	{
	}

	public interface org_apache_commons_math3_analysis_polynomials_PolynomialsUtils^1RecurrenceCoefficientsGenerator : PolynomialsUtils.RecurrenceCoefficientsGenerator
	{
	}

	public interface org_apache_commons_math3_optim_nonlinear_scalar_MultivariateFunctionMappingAdapter^1Mapper : org.apache.commons.math3.optim.nonlinear.scalar.MultivariateFunctionMappingAdapter.Mapper
	{
	}

	public interface org_apache_commons_math3_optimization_direct_MultivariateFunctionMappingAdapter^1Mapper : org.apache.commons.math3.optimization.direct.MultivariateFunctionMappingAdapter.Mapper
	{
	}
}
