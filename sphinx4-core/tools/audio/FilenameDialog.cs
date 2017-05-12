using System;
using System.Runtime.Serialization;
using System.Security.Permissions;
using java.awt;
using java.lang;
using javax.swing;

namespace edu.cmu.sphinx.tools.audio
{
	[Serializable]
	public class FilenameDialog : JDialog
	{
		internal virtual void createFilenamePanel()
		{
			Container contentPane = this.getContentPane();
			GridBagLayout gridBagLayout = new GridBagLayout();
			contentPane.setLayout(gridBagLayout);
			this.filename = new JTextField(12);
			JLabel jlabel = new JLabel("Filename:");
			jlabel.setLabelFor(this.filename);
			Insets insets = new Insets(12, 12, 0, 0);
			GridBagConstraints gridBagConstraints = new GridBagConstraints(0, 0, 1, 1, (double)0f, (double)0f, 17, 0, insets, 0, 0);
			gridBagLayout.setConstraints(jlabel, gridBagConstraints);
			contentPane.add(jlabel);
			insets = new Insets(12, 7, 0, 12);
			gridBagConstraints = new GridBagConstraints(1, 0, 1, 1, (double)1f, (double)1f, 17, 2, insets, 0, 0);
			gridBagLayout.setConstraints(this.filename, gridBagConstraints);
			contentPane.add(this.filename);
			JButton jbutton = new JButton("Save");
			jbutton.addActionListener(new FilenameDialog_2(this));
			insets = new Insets(0, 12, 12, 12);
			gridBagConstraints = new GridBagConstraints(0, 2, 2, 1, (double)1f, (double)1f, 13, 0, insets, 0, 0);
			gridBagLayout.setConstraints(jbutton, gridBagConstraints);
			contentPane.add(jbutton);
			this.getRootPane().setDefaultButton(jbutton);
		}
		
		public FilenameDialog(Frame parent, bool modal, string title) : base(parent, modal)
		{
			this.setTitle(title);
			this.addWindowListener(new FilenameDialog_1(this));
			this.createFilenamePanel();
			this.pack();
		}
		
		public virtual string getFilename()
		{
			return this.filename.getText();
		}
		
		public static void main(string[] args)
		{
			JFrame jframe = new JFrame();
			jframe.setTitle("Debug");
			jframe.setDefaultCloseOperation(3);
			jframe.pack();
			jframe.setVisible(false);
			FilenameDialog filenameDialog = new FilenameDialog(jframe, true, "Save as...");
			java.lang.System.@out.println("Showing dialog...");
			filenameDialog.setVisible(true);
			string text = filenameDialog.getFilename();
			java.lang.System.@out.println(new StringBuilder().append("Filename: ").append(text).append(" (length = ").append(java.lang.String.instancehelper_length(text)).append(')').toString());
			java.lang.System.exit(0);
		}

		[PermissionSet(SecurityAction.Demand, XML = "<PermissionSet class=\"System.Security.PermissionSet\"\nversion=\"1\">\n<IPermission class=\"System.Security.Permissions.SecurityPermission, mscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089\"\nversion=\"1\"\nFlags=\"SerializationFormatter\"/>\n</PermissionSet>\n")]
		protected FilenameDialog(SerializationInfo serializationInfo, StreamingContext streamingContext) : base(serializationInfo, streamingContext)
		{
		}

		internal JTextField filename;
	}
}
