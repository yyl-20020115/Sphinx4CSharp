using System;

using edu.cmu.sphinx.jsgf.rule;
using edu.cmu.sphinx.linguist.language.grammar;
using IKVM.Attributes;
using IKVM.Runtime;
using java.io;
using java.lang;
using java.net;
using java.util;
using javax.xml.parsers;
using org.xml.sax;

namespace edu.cmu.sphinx.jsgf
{
	public class GrXMLGrammar : JSGFGrammar
	{
		protected internal virtual void loadXML()
		{
			SAXParseException ex2;
			SAXException ex4;
			ParserConfigurationException ex6;
			try
			{
				try
				{
					try
					{
						SAXParserFactory saxparserFactory = SAXParserFactory.newInstance();
						XMLReader xmlreader = saxparserFactory.newSAXParser().getXMLReader();
						this.rules = new HashMap();
						GrXMLHandler grXMLHandler = new GrXMLHandler(this.baseURL, this.rules, this.logger);
						xmlreader.setContentHandler(grXMLHandler);
						xmlreader.setErrorHandler(grXMLHandler);
						InputStream inputStream = this.baseURL.openStream();
						xmlreader.parse(new InputSource(inputStream));
						inputStream.close();
					}
					catch (SAXParseException ex)
					{
						ex2 = ByteCodeHelper.MapException<SAXParseException>(ex, 1);
						goto IL_85;
					}
				}
				catch (SAXException ex3)
				{
					ex4 = ByteCodeHelper.MapException<SAXException>(ex3, 1);
					goto IL_89;
				}
			}
			catch (ParserConfigurationException ex5)
			{
				ex6 = ByteCodeHelper.MapException<ParserConfigurationException>(ex5, 1);
				goto IL_8D;
			}
			return;
			IL_85:
			SAXParseException ex7 = ex2;
			string text = new StringBuilder().append("Error while parsing line ").append(ex7.getLineNumber()).append(" of ").append(this.baseURL).append(": ").append(ex7.getMessage()).toString();
			string text2 = text;
			
			throw new IOException(text2);
			IL_89:
			SAXException ex8 = ex4;
			string text3 = new StringBuilder().append("Problem with XML: ").append(ex8).toString();
			
			throw new IOException(text3);
			IL_8D:
			ParserConfigurationException ex9 = ex6;
			string text4 = Throwable.instancehelper_getMessage(ex9);
			
			throw new IOException(text4);
		}

		
		
		public GrXMLGrammar()
		{
		}
		
		public override void commitChanges()
		{
			MalformedURLException ex2;
			try
			{
				if (this.loadGrammar)
				{
					if (this.manager == null)
					{
						this.getGrammarManager();
					}
					this.loadXML();
					this.loadGrammar = false;
				}
				this.__ruleStack = new JSGFGrammar.RuleStack(this);
				this.newGrammar();
				this.firstNode = this.createGrammarNode("<sil>");
				GrammarNode grammarNode = this.createGrammarNode("<sil>");
				grammarNode.setFinalNode(true);
				Iterator iterator = this.rules.entrySet().iterator();
				while (iterator.hasNext())
				{
					Map.Entry entry = (Map.Entry)iterator.next();
					JSGFGrammar.GrammarGraph grammarGraph = new JSGFGrammar.GrammarGraph(this);
					this.__ruleStack.push((string)entry.getKey(), grammarGraph);
					JSGFGrammar.GrammarGraph grammarGraph2 = this.processRule((JSGFRule)entry.getValue());
					this.__ruleStack.pop();
					this.firstNode.add(grammarGraph.getStartNode(), 0f);
					grammarGraph.getEndNode().add(grammarNode, 0f);
					grammarGraph.getStartNode().add(grammarGraph2.getStartNode(), 0f);
					grammarGraph2.getEndNode().add(grammarGraph.getEndNode(), 0f);
				}
				this.postProcessGrammar();
			}
			catch (MalformedURLException ex)
			{
				ex2 = ByteCodeHelper.MapException<MalformedURLException>(ex, 1);
				goto IL_12E;
			}
			return;
			IL_12E:
			MalformedURLException ex3 = ex2;
			string text = new StringBuilder().append("bad base grammar URL ").append(this.baseURL).append(' ').append(ex3).toString();
			
			throw new IOException(text);
		}

		internal Map rules;
	}
}
