using java.lang;

namespace edu.cmu.sphinx.util
{
	internal sealed class SocketCommandClient_2 : Object, CommandInterface
	{
		internal SocketCommandClient_2(SocketCommandClient socketCommandClient)
		{
			val_sci = socketCommandClient;
		}
	
		public string execute(CommandInterpreter commandInterpreter, string[] array)
		{
			while (this.val_sci.isResponse())
			{
				commandInterpreter.putResponse(this.val_sci.getResponse());
			}
			return "";
		}

		public string getHelp()
		{
			return "receive a response";
		}

		internal SocketCommandClient val_sci;
	}
}
