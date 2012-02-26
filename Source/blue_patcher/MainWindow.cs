using System;
using System.IO;
using Gtk;

namespace blue_patcher 
{

public partial class MainWindow: Gtk.Window
{	
	private blue_patcher.Patcher patcher = new blue_patcher.Patcher();
	
	public MainWindow (): base (Gtk.WindowType.Toplevel)
	{
		Build ();
		
		Gtk.FileFilter fileFilter = new Gtk.FileFilter();
		fileFilter.AddPattern("blue.dll");
		ChooseFile.Filter = fileFilter;
	}
	
	public void Log (string text) 
	{
		textView.Buffer.Text += "[+]" + text + "\r\n";
	}
	
	protected void OnDeleteEvent (object sender, DeleteEventArgs a)
	{
		Application.Quit ();
		a.RetVal = true;
	}

	protected void OnPatchClicked (object sender, System.EventArgs e)
	{
		if(!File.Exists(pathField.Text))
		{
			MessageDialog md = new MessageDialog(this, 
			                                     DialogFlags.Modal, MessageType.Info, 
			                                     ButtonsType.Ok,
			                                      "You didn't select a valid file!");
			return;
		}
		patcher.Patch(pathField.Text);
		
	}

	protected void OnChooseFileSelectionChanged (object sender, System.EventArgs e)
	{	
		pathField.Text = ChooseFile.Filename;	
	}
}
}
