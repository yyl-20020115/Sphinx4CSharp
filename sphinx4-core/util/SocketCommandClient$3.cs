using java.lang;

namespace edu.cmu.sphinx.util
{
	internal sealed class SocketCommandClient_3 : Object, CommandInterface
	{
		internal SocketCommandClient_3(SocketCommandClient socketCommandClient)
		{
			val_sci = socketCommandClient;
		}
		
		public string execute(CommandInterpreter commandInterpreter, string[] array)
		{
			StringBuilder stringBuilder = new StringBuilder();
			for (int i = 1; i < array.Length; i++)
			{
				stringBuilder.append(array[i]).append(' ');
			}
			commandInterpreter.putResponse(this.val_sci.sendCommandGetResponse(stringBuilder.toString()));
			while (this.val_sci.isResponse())
			{
				commandInterpreter.putResponse(this.val_sci.getResponse());
			}
			return "";
		}

		public string getHelp()
		{
			return "send a command, receive a response";
		}
		
		internal SocketCommandClient val_sci;
	}
}
