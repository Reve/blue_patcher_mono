using System;
using Gtk;

namespace blue_patcher
{
	class MainClass
	{
		public static MainWindow MainWindow {get; private set;}
		
		public static void Main (string[] args)
		{
			Application.Init ();
			MainWindow = new MainWindow();
			MainWindow.Show();
			Application.Run ();
		}
	}
}
