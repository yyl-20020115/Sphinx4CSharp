﻿using System;
using System.Diagnostics;
using System.Reflection;

using IKVM.Attributes;

[assembly: AssemblyVersion("0.0.0.0")]
[assembly: Debuggable(true, false)]
[assembly: RuntimeCompatibility(WrapNonExceptionThrows = true)]
[module: SourceFile(null)]
[module: JavaModule(Jars = new string[]
{
	"sphinx4-core-5prealpha-SNAPSHOT.jar"
})]
[module: PackageList(new string[]
{
	"edu.cmu.sphinx.alignment",
	"edu.cmu.sphinx.alignment.tokenizer",
	"edu.cmu.sphinx.api",
	"edu.cmu.sphinx.decoder",
	"edu.cmu.sphinx.decoder.adaptation",
	"edu.cmu.sphinx.decoder.pruner",
	"edu.cmu.sphinx.decoder.scorer",
	"edu.cmu.sphinx.decoder.search",
	"edu.cmu.sphinx.decoder.search.stats",
	"edu.cmu.sphinx.frontend",
	"edu.cmu.sphinx.frontend.databranch",
	"edu.cmu.sphinx.frontend.denoise",
	"edu.cmu.sphinx.frontend.endpoint",
	"edu.cmu.sphinx.frontend.feature",
	"edu.cmu.sphinx.frontend.filter",
	"edu.cmu.sphinx.frontend.frequencywarp",
	"edu.cmu.sphinx.frontend.transform",
	"edu.cmu.sphinx.frontend.util",
	"edu.cmu.sphinx.frontend.window",
	"edu.cmu.sphinx.fst",
	"edu.cmu.sphinx.fst.operations",
	"edu.cmu.sphinx.fst.semiring",
	"edu.cmu.sphinx.fst.sequitur",
	"edu.cmu.sphinx.fst.utils",
	"edu.cmu.sphinx.instrumentation",
	"edu.cmu.sphinx.jsgf",
	"edu.cmu.sphinx.jsgf.parser",
	"edu.cmu.sphinx.jsgf.rule",
	"edu.cmu.sphinx.linguist",
	"edu.cmu.sphinx.linguist.acoustic",
	"edu.cmu.sphinx.linguist.acoustic.tiedstate",
	"edu.cmu.sphinx.linguist.acoustic.tiedstate.HTK",
	"edu.cmu.sphinx.linguist.acoustic.tiedstate.kaldi",
	"edu.cmu.sphinx.linguist.acoustic.tiedstate.tiedmixture",
	"edu.cmu.sphinx.linguist.acoustic.tiedstate.trainer",
	"edu.cmu.sphinx.linguist.acoustic.trivial",
	"edu.cmu.sphinx.linguist.aflat",
	"edu.cmu.sphinx.linguist.allphone",
	"edu.cmu.sphinx.linguist.dflat",
	"edu.cmu.sphinx.linguist.dictionary",
	"edu.cmu.sphinx.linguist.flat",
	"edu.cmu.sphinx.linguist.g2p",
	"edu.cmu.sphinx.linguist.language.classes",
	"edu.cmu.sphinx.linguist.language.grammar",
	"edu.cmu.sphinx.linguist.language.ngram",
	"edu.cmu.sphinx.linguist.language.ngram.large",
	"edu.cmu.sphinx.linguist.language.ngram.trie",
	"edu.cmu.sphinx.linguist.lextree",
	"edu.cmu.sphinx.linguist.util",
	"edu.cmu.sphinx.recognizer",
	"edu.cmu.sphinx.result",
	"edu.cmu.sphinx.speakerid",
	"edu.cmu.sphinx.tools.aligner",
	"edu.cmu.sphinx.tools.audio",
	"edu.cmu.sphinx.tools.bandwidth",
	"edu.cmu.sphinx.tools.batch",
	"edu.cmu.sphinx.tools.endpoint",
	"edu.cmu.sphinx.tools.feature",
	"edu.cmu.sphinx.tools.live",
	"edu.cmu.sphinx.tools.transcriber",
	"edu.cmu.sphinx.trainer",
	"edu.cmu.sphinx.util",
	"edu.cmu.sphinx.util.machlearn",
	"edu.cmu.sphinx.util.props",
	"edu.cmu.sphinx.util.props.tools"
})]
