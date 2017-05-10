using System;

namespace edu.cmu.sphinx.util
{
	public interface CommandInterface
	{
		string execute(CommandInterpreter ci, string[] strarr);

		string getHelp();
	}
}
