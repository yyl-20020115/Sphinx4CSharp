﻿using edu.cmu.sphinx.fst.semiring;
using ikvm.@internal;
using java.io;
using java.lang;
using java.util;
using javax.xml.bind;
using javax.xml.bind.annotation;

namespace edu.cmu.sphinx.fst.sequitur
{
	public class SequiturImport : Object
	{
		public SequiturImport()
		{
		}
		
		public static void main(params string[] args)
		{
			JAXBContext jaxbcontext = JAXBContext.newInstance(new Class[]
			{
				ClassLiteral<SequiturImport.FSA>.Value
			});
			Unmarshaller unmarshaller = jaxbcontext.createUnmarshaller();
			Unmarshaller unmarshaller2 = unmarshaller;
			SequiturImport.FSA fsa = (SequiturImport.FSA)unmarshaller2.unmarshal(new File(args[0]));
			Fst fst = fsa.toFst();
			fst.saveModel(args[1]);
			java.lang.System.@out.println(new StringBuilder().append("The Sequitur G2P XML-formatted FST ").append(args[0]).append(" has been converted to Sphinx' OpenFst binary format in the file ").append(args[1]).toString());
		}

		public class Alphabet : Object
		{
			public Alphabet()
			{
			}
			
			public virtual void afterUnmarshal(Unmarshaller unmarshaller, object parent)
			{
				Iterator iterator = this.symbols.iterator();
				while (iterator.hasNext())
				{
					if (String.instancehelper_matches(((SequiturImport.Symbol)iterator.next()).content, "__\\d+__"))
					{
						iterator.remove();
					}
				}
				for (int i = 0; i < this.symbols.size(); i++)
				{
					if (!SequiturImport.Alphabet.assertionsDisabled && ((SequiturImport.Symbol)this.symbols.get(i)).index == null)
					{
						
						throw new AssertionError();
					}
					if (!SequiturImport.Alphabet.assertionsDisabled && ((SequiturImport.Symbol)this.symbols.get(i)).index.intValue() != i)
					{
						
						throw new AssertionError();
					}
					((SequiturImport.Symbol)this.symbols.get(i)).index = null;
				}
				SequiturImport.Symbol symbol = new SequiturImport.Symbol();
				symbol.content = "<s>";
				this.symbols.add(symbol);
			}
			
			internal virtual string[] toSymbols()
			{
				string[] array = new string[this.symbols.size()];
				for (int i = 0; i < array.Length; i++)
				{
					array[i] = ((SequiturImport.Symbol)this.symbols.get(i)).content;
				}
				return array;
			}

			
			[XmlElement(new object[]
			{
				64,
				"Ljavax/xml/bind/annotation/XmlElement;",
				"name",
				"symbol"
			})]
			internal List symbols;
			
			internal static bool assertionsDisabled = !ClassLiteral<SequiturImport>.Value.desiredAssertionStatus();
		}

		public class Arc : Object
		{
			public Arc()
			{
			}

			public virtual void afterUnmarshal(Unmarshaller unmarshaller, object parent)
			{
				this.target++;
			}
			
			public virtual edu.cmu.sphinx.fst.Arc toOpenFstArc(List openFstStates)
			{
				return new edu.cmu.sphinx.fst.Arc(this.@in, this.@out, this.weight, (edu.cmu.sphinx.fst.State)openFstStates.get(this.target));
			}

			[XmlAttribute(new object[]
			{
				64,
				"Ljavax/xml/bind/annotation/XmlAttribute;"
			})]
			internal int target;

			[XmlElement(new object[]
			{
				64,
				"Ljavax/xml/bind/annotation/XmlElement;"
			})]
			internal int @in;

			[XmlElement(new object[]
			{
				64,
				"Ljavax/xml/bind/annotation/XmlElement;"
			})]
			internal int @out;

			[XmlElement(new object[]
			{
				64,
				"Ljavax/xml/bind/annotation/XmlElement;"
			})]
			internal float weight;
		}

		[XmlRootElement(new object[]
		{
			64,
			"Ljavax/xml/bind/annotation/XmlRootElement;",
			"name",
			"fsa"
		})]
		public class FSA : Object
		{			
			public virtual Fst toFst()
			{
				Fst fst = new Fst(this.ring);
				fst.setIsyms(this.inputAlphabet.toSymbols());
				fst.setOsyms(this.outputAlphabet.toSymbols());
				this.openFstStates = new ArrayList(this.states.size());
				Iterator iterator = this.states.iterator();
				while (iterator.hasNext())
				{
					SequiturImport.State state = (SequiturImport.State)iterator.next();
					edu.cmu.sphinx.fst.State state2 = state.toUnconnectedOpenFstState();
					fst.addState(state2);
					if (!SequiturImport.FSA.assertionsDisabled && state2.getId() != state.id)
					{
						
						throw new AssertionError();
					}
					this.openFstStates.add(state2);
				}
				fst.setStart((edu.cmu.sphinx.fst.State)this.openFstStates.get(0));
				iterator = this.states.iterator();
				while (iterator.hasNext())
				{
					SequiturImport.State state = (SequiturImport.State)iterator.next();
					state.connectStates(this.openFstStates);
				}
				return fst;
			}
			
			public FSA()
			{
				this.ring = new TropicalSemiring();
			}
			
			public virtual void afterUnmarshal(Unmarshaller unmarshaller, object parent)
			{
				if (!SequiturImport.FSA.assertionsDisabled && !String.instancehelper_equals("tropical", this.semiring))
				{
					
					throw new AssertionError();
				}
				SequiturImport.State state = new SequiturImport.State();
				state.id = 0;
				state.arcs = Collections.singletonList(new SequiturImport.Arc
				{
					@in = this.inputAlphabet.symbols.size() - 1,
					@out = this.outputAlphabet.symbols.size() - 1,
					target = this.initial + 1,
					weight = this.ring.one()
				});
				this.states.add(state);
				Collections.sort(this.states, new SequiturImport_FSA_1(this));
			}

			[XmlAttribute(new object[]
			{
				64,
				"Ljavax/xml/bind/annotation/XmlAttribute;"
			})]
			internal string semiring;

			[XmlAttribute(new object[]
			{
				64,
				"Ljavax/xml/bind/annotation/XmlAttribute;"
			})]
			internal int initial;

			[XmlElement(new object[]
			{
				64,
				"Ljavax/xml/bind/annotation/XmlElement;",
				"name",
				"input-alphabet"
			})]
			internal SequiturImport.Alphabet inputAlphabet;

			[XmlElement(new object[]
			{
				64,
				"Ljavax/xml/bind/annotation/XmlElement;",
				"name",
				"output-alphabet"
			})]
			internal SequiturImport.Alphabet outputAlphabet;
			
			[XmlElement(new object[]
			{
				64,
				"Ljavax/xml/bind/annotation/XmlElement;",
				"name",
				"state"
			})]
			internal List states;

			[System.NonSerialized]
			internal List openFstStates;

			[System.NonSerialized]
			internal Semiring ring;
			
			internal static bool assertionsDisabled = !ClassLiteral<SequiturImport>.Value.desiredAssertionStatus();
		}

		public class State : Object
		{
			public State()
			{
			}
			
			public virtual edu.cmu.sphinx.fst.State toUnconnectedOpenFstState()
			{
				return new edu.cmu.sphinx.fst.State((this.weight == null) ? 0f : this.weight.floatValue());
			}
			
			public virtual void connectStates(List openFstStates)
			{
				if (this.arcs != null)
				{
					Iterator iterator = this.arcs.iterator();
					while (iterator.hasNext())
					{
						SequiturImport.Arc arc = (SequiturImport.Arc)iterator.next();
						edu.cmu.sphinx.fst.Arc arc2 = arc.toOpenFstArc(openFstStates);
						((edu.cmu.sphinx.fst.State)openFstStates.get(this.id)).addArc(arc2);
					}
				}
			}

			public virtual void afterUnmarshal(Unmarshaller unmarshaller, object parent)
			{
				this.id++;
			}

			[XmlAttribute(new object[]
			{
				64,
				"Ljavax/xml/bind/annotation/XmlAttribute;"
			})]
			internal int id;

			[XmlElement(new object[]
			{
				64,
				"Ljavax/xml/bind/annotation/XmlElement;",
				"name",
				"final"
			})]
			internal object finalState;

			[XmlElement(new object[]
			{
				64,
				"Ljavax/xml/bind/annotation/XmlElement;"
			})]
			internal Float weight;
			
			[XmlElement(new object[]
			{
				64,
				"Ljavax/xml/bind/annotation/XmlElement;",
				"name",
				"arc"
			})]
			internal List arcs;
		}
		public class Symbol : Object
		{						
			public Symbol()
			{
			}
			
			public virtual void afterUnmarshal(Unmarshaller unmarshaller, object parent)
			{
				if (!SequiturImport.Symbol.assertionsDisabled && this.contentList == null)
				{
					object obj = new StringBuilder().append("Error with symbol ").append(this.index).toString();
					
					throw new AssertionError(obj);
				}
				if (!SequiturImport.Symbol.assertionsDisabled && this.contentList.size() != 1)
				{
					object obj2 = new StringBuilder().append("Error with symbol ").append(this.index).toString();
					
					throw new AssertionError(obj2);
				}
				this.content = (string)this.contentList.get(0);
				if (String.instancehelper_equals(this.content, "__term__"))
				{
					this.content = "</s>";
				}
				else if (String.instancehelper_matches(this.content, "__.+__"))
				{
					this.content = "<eps>";
				}
			}

			[XmlAttribute(new object[]
			{
				64,
				"Ljavax/xml/bind/annotation/XmlAttribute;"
			})]
			internal Integer index;
		
			[XmlMixed(new object[]
			{
				64,
				"Ljavax/xml/bind/annotation/XmlMixed;"
			})]
			internal List contentList;

			[System.NonSerialized]
			internal string content;
			
			internal static bool assertionsDisabled = !ClassLiteral<SequiturImport>.Value.desiredAssertionStatus();
		}
	}
}
