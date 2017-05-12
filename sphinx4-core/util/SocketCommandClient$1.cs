using java.lang;

namespace edu.cmu.sphinx.util
{
	internal sealed class SocketCommandClient_1 : Object, CommandInterface
	{
		internal SocketCommandClient_1(SocketCommandClient socketCommandClient)
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
			this.val_sci.sendCommand(stringBuilder.toString());
			return "";
		}

		public string getHelp()
		{
			return "send a command";
		}
		
		internal SocketCommandClient val_sci;
	}
}
