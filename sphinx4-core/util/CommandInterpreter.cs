﻿using java.io;
using java.lang;
using java.net;
using java.util;
using java.util.regex;

namespace edu.cmu.sphinx.util
{
	public class CommandInterpreter : Thread
	{
		public virtual void add(string name, CommandInterface command)
		{
			this.commandList.put(name, command);
		}

		public virtual void addAlias(string command, string alias)
		{
			this.commandList.put(alias, this.commandList.get(command));
		}

		public CommandInterpreter()
		{
			this.history = new CommandInterpreter.CommandHistory(this);
			BufferedReader bufferedReader = new BufferedReader(new InputStreamReader(java.lang.System.@in));
			PrintWriter printWriter = new PrintWriter(java.lang.System.@out);
			this.init(bufferedReader, printWriter);
		}

		public virtual void setPrompt(string prompt)
		{
			this.prompt = prompt;
		}

		public override void run()
		{
			while (!this.done)
			{
				string text;
				try
				{
					this.printPrompt();
					text = this.getInputLine();
					if (text == null)
					{
						break;
					}
				}
				catch (IOException)
				{
					java.lang.System.@out.println("Exception: CommandInterpreter.run()");
					break;
				}
				try
				{
					if (this.trace)
					{
						java.lang.System.@out.println("\n----------");
						java.lang.System.@out.println(new StringBuilder().append("In : ").append(text).toString());
					}
					text = String.instancehelper_trim(text);
					if (!String.instancehelper_isEmpty(text))
					{
						this.putResponse(this.execute(text));
					}
				}
				catch (IOException)
				{
					java.lang.System.@out.println("Exception: CommandInterpreter.run()");
					break;
				}
			}
			this.onExit();
		}

		public virtual void putResponse(string response)
		{
			if (response != null && !String.instancehelper_isEmpty(response))
			{
				this.@out.println(response);
				this.@out.flush();
				if (this.trace)
				{
					java.lang.System.@out.println(new StringBuilder().append("Out: ").append(response).toString());
				}
			}
		}

		internal static void access_000(CommandInterpreter commandInterpreter)
		{
			commandInterpreter.dumpCommands();
		}

		protected internal virtual string execute(string[] args)
		{
			string result = "";
			if (args.Length > 0)
			{
				CommandInterface commandInterface = (CommandInterface)this.commandList.get(args[0]);
				if (commandInterface != null)
				{
					result = commandInterface.execute(this, args);
				}
				else
				{
					result = "ERR  CMD_NOT_FOUND";
				}
				this.totalCommands++;
			}
			return result;
		}

		public virtual bool load(string filename)
		{
			int result;
			try
			{
				FileReader fileReader = new FileReader(filename);
				BufferedReader bufferedReader = new BufferedReader(fileReader);
				string cmdString;
				while ((cmdString = bufferedReader.readLine()) != null)
				{
					string text = this.execute(cmdString);
					if (!String.instancehelper_equals(text, "OK"))
					{
						this.putResponse(text);
					}
				}
				fileReader.close();
				result = 1;
			}
			catch (IOException)
			{
				return false;
			}
			return result != 0;
		}

		internal static CommandInterpreter.CommandHistory access_100(CommandInterpreter commandInterpreter)
		{
			return commandInterpreter.history;
		}

		internal static int access_200(CommandInterpreter commandInterpreter)
		{
			return commandInterpreter.totalCommands;
		}

		internal static bool access_302(CommandInterpreter commandInterpreter, bool result)
		{
			commandInterpreter.done = result;
			return result;
		}

		private void dumpCommands()
		{
			Iterator iterator = new TreeMap(this.commandList).entrySet().iterator();
			while (iterator.hasNext())
			{
				Map.Entry entry = (Map.Entry)iterator.next();
				this.putResponse(new StringBuilder().append((string)entry.getKey()).append(" - ").append(((CommandInterface)entry.getValue()).getHelp()).toString());
			}
		}

		private void init(BufferedReader bufferedReader, PrintWriter printWriter)
		{
			this.commandList = new HashMap();
			this.addStandardCommands();
			this.setStreams(bufferedReader, printWriter);
		}

		private void addStandardCommands()
		{
			this.add("help", new CommandInterpreter_1(this));
			this.add("history", new CommandInterpreter_2(this));
			this.add("status", new CommandInterpreter_3(this));
			this.add("echo", new CommandInterpreter_4(this));
			this.add("quit", new CommandInterpreter_5(this));
			this.add("on_exit", new CommandInterpreter_6(this));
			this.add("version", new CommandInterpreter_7(this));
			this.add("gc", new CommandInterpreter_8(this));
			this.add("memory", new CommandInterpreter_9(this));
			this.add("delay", new CommandInterpreter_10(this));
			this.add("repeat", new CommandInterpreter_11(this));
			this.add("load", new CommandInterpreter_12(this));
			this.add("chain", new CommandInterpreter_13(this));
			this.add("time", new CommandInterpreter_14(this));
		}

		public virtual void setStreams(BufferedReader @in, PrintWriter @out)
		{
			this.@in = @in;
			this.@out = @out;
		}

		public virtual string execute(string cmdString)
		{
			if (this.trace)
			{
				java.lang.System.@out.println(new StringBuilder().append("Execute: ").append(cmdString).toString());
			}
			return this.execute(this.parseMessage(cmdString));
		}

		protected internal virtual string[] parseMessage(string message)
		{
			ArrayList arrayList = new ArrayList(20);
			StreamTokenizer streamTokenizer = new StreamTokenizer(new StringReader(message));
			streamTokenizer.resetSyntax();
			streamTokenizer.whitespaceChars(0, 32);
			streamTokenizer.wordChars(33, 255);
			streamTokenizer.quoteChar(34);
			streamTokenizer.quoteChar(34);
			streamTokenizer.commentChar(35);
			for (;;)
			{
				try
				{
					int num = streamTokenizer.nextToken();
					if (num == -3)
					{
						arrayList.add(streamTokenizer.sval);
					}
					else if (num == 39 || num == 34)
					{
						arrayList.add(streamTokenizer.sval);
					}
					else
					{
						if (num != -2)
						{
							break;
						}
						java.lang.System.@out.println("Unexpected numeric token!");
					}
				}
				catch (IOException)
				{
					break;
				}
			}
			return (string[])arrayList.toArray(new string[arrayList.size()]);
		}
		private void printPrompt()
		{
			if (this.prompt != null)
			{
				this.@out.print(this.prompt);
				this.@out.flush();
			}
		}

		private string getInputLine()
		{
			string text = this.@in.readLine();
			if (text == null)
			{
				return null;
			}
			int num = 0;
			int num2 = 0;
			int num3 = 0;
			Pattern pattern = CommandInterpreter.historyPush;
			object _ref = text;
			CharSequence charSequence = CharSequence.Cast(_ref);
			Matcher matcher = pattern.matcher(charSequence);
			if (matcher.matches())
			{
				num = 1;
				num2 = 1;
				text = matcher.group(1);
			}
			if (String.instancehelper_startsWith(text, "^"))
			{
				Pattern pattern2 = CommandInterpreter.editPattern;
				_ref = text;
				charSequence = CharSequence.Cast(_ref);
				matcher = pattern2.matcher(charSequence);
				if (matcher.matches())
				{
					string text2 = matcher.group(1);
					string text3 = matcher.group(2);
					PatternSyntaxException ex2;
					try
					{
						Pattern pattern3 = Pattern.compile(text2);
						Pattern pattern4 = pattern3;
						_ref = this.history.getLast(0);
						charSequence = CharSequence.Cast(_ref);
						Matcher matcher2 = pattern4.matcher(charSequence);
						if (matcher2.find())
						{
							text = matcher2.replaceFirst(text3);
							num2 = 1;
						}
						else
						{
							num3 = 1;
							this.putResponse(new StringBuilder().append(text).append(": substitution failed").toString());
						}
					}
					catch (PatternSyntaxException ex)
					{
						ex2 = ex;
						goto IL_114;
					}
					goto IL_144;
					IL_114:
					PatternSyntaxException ex3 = ex2;
					num3 = 1;
					this.putResponse(new StringBuilder().append("Bad regexp: ").append(ex3.getDescription()).toString());
					IL_144:;
				}
				else
				{
					num3 = 1;
					this.putResponse("bad substitution sytax, use ^old^new^");
				}
			}
			else
			{
				Pattern pattern5 = CommandInterpreter.bbPattern;
				_ref = text;
				charSequence = CharSequence.Cast(_ref);
				if ((matcher = pattern5.matcher(charSequence)).find())
				{
					text = matcher.replaceAll(this.history.getLast(0));
					num2 = 1;
				}
				else if (String.instancehelper_startsWith(text, "!"))
				{
					if (String.instancehelper_matches(text, "!\\d+"))
					{
						int num4 = Integer.parseInt(String.instancehelper_substring(text, 1));
						text = this.history.get(num4);
					}
					else if (String.instancehelper_matches(text, "!-\\d+"))
					{
						int num4 = Integer.parseInt(String.instancehelper_substring(text, 2));
						text = this.history.getLast(num4 - 1);
					}
					else
					{
						text = this.history.findLast(String.instancehelper_substring(text, 1));
					}
					num2 = 1;
				}
			}
			if (num3 != 0)
			{
				return "";
			}
			if (!String.instancehelper_isEmpty(text))
			{
				this.history.add(text);
			}
			if (num2 != 0)
			{
				this.putResponse(text);
			}
			return (num == 0) ? text : "";
		}

		protected internal virtual void onExit()
		{
			this.execute("on_exit");
			java.lang.System.@out.println("----------\n");
		}

		public CommandInterpreter(BufferedReader @in, PrintWriter @out)
		{
			this.history = new CommandInterpreter.CommandHistory(this);
			this.init(@in, @out);
		}

		public virtual void setTrace(bool trace)
		{
			this.trace = trace;
		}

		public virtual Socket getSocket()
		{
			return this.socket;
		}

		public virtual void setSocket(Socket skt)
		{
			this.socket = skt;
		}

		public virtual void add(Map newCommands)
		{
			this.commandList.putAll(newCommands);
		}

		public virtual void close()
		{
			this.done = true;
		}

		public virtual string getPrompt()
		{
			return this.prompt;
		}

		public virtual PrintWriter getPrintWriter()
		{
			return this.@out;
		}

		public static void main(string[] args)
		{
			CommandInterpreter commandInterpreter = new CommandInterpreter();
			try
			{
				java.lang.System.@out.println("Welcome to the Command interpreter test program");
				commandInterpreter.setPrompt("CI> ");
				commandInterpreter.run();
				java.lang.System.@out.println("Goodbye!");
			}
			catch (System.Exception ex)
			{
				java.lang.System.@out.println(ex);
			}
		}

		static CommandInterpreter()
		{
			CommandInterpreter.historyPush = Pattern.compile("(.+):p");
			CommandInterpreter.editPattern = Pattern.compile("\\^(.+?)\\^(.*?)\\^?");
			CommandInterpreter.bbPattern = Pattern.compile("(!!)");
		}

		private Map commandList;

		private int totalCommands;

		private BufferedReader @in;

		private PrintWriter @out;

		private string prompt;

		private bool done;

		private bool trace;

		private CommandInterpreter.CommandHistory history;

		private Socket socket;

		private static Pattern historyPush;

		private static Pattern editPattern;

		private static Pattern bbPattern;

		internal sealed class CommandHistory : Object
		{
			public void dump()
			{
				for (int i = 0; i < this.history.size(); i++)
				{
					string text = this.get(i);
					this.this_0.putResponse(new StringBuilder().append(i).append(" ").append(text).toString());
				}
			}

			public string get(int num)
			{
				if (this.history.size() > num)
				{
					return (string)this.history.get(num);
				}
				this.this_0.putResponse("command not found");
				return "";
			}

			internal CommandHistory(CommandInterpreter commandInterpreter)
			{
				this_0 = commandInterpreter;
				this.history = new ArrayList(100);
			}

			public void add(string text)
			{
				this.history.add(text);
			}

			public string getLast(int num)
			{
				if (this.history.size() > num)
				{
					return (string)this.history.get(this.history.size() - 1 - num);
				}
				this.this_0.putResponse("command not found");
				return "";
			}

			public string findLast(string text)
			{
				for (int i = this.history.size() - 1; i >= 0; i--)
				{
					string text2 = this.get(i);
					if (String.instancehelper_startsWith(text2, text))
					{
						return text2;
					}
				}
				this.this_0.putResponse("command not found");
				return "";
			}

			private List history;


			internal CommandInterpreter this_0;
		}
	}
}
