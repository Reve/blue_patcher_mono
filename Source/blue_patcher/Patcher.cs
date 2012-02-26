using System;
using System.IO;
using Gtk;

namespace blue_patcher
{
	public class Patcher
	{	
		public Patcher ()
		{
		}
		
		// 8b 46 ?? 2b c1 8b 54 ?? ?? 8b 6c ?? ?? 6a 00 6a 00 52 50 e8 ?? ?? ?? ?? 50 55 ff 15 ?? ?? ?? ?? 8b f0 8b 44 ?? ?? 50 ff 15 ?? ?? ?? ?? 85 f6 0f 85
        // FF 6A 00 6A 00 52 2B C1 50 8B 85 C8 FA FF FF 51 50 C6 85 D3 FA FF FF 01 FF 15 00 10 15 10 8B 8D C8 FA FF FF 51 8B F0 FF 15 74 10 15 10 85 F6 0F 85
		// FF 6A 00 6A 00 52 2B C1 50 8B 85 C8 FA FF FF 51 50 C6 85 CF FA FF FF 01 FF 15 54 D0 14 10 8B 8D C8 FA FF FF 51 8B F0 FF 15 74 D0 14 10 8B F6 0F 85
        private static readonly byte[] Bytes = new byte[]
                                           {
                                               0x6a, 0x00, 0x6a, 0x00, 0x52, 0x2b, 0xc1, 0x50, 0x8b, 0x85, 0xc8,
                                               0xfa,
                                               0xff, 0xff, 0x51, 0x50, 0xc6, 0x85, 0xcf, 0xfa, 0xff, 0xff, 0x01, 0xff,
                                               0x15,
                                               0x54, 0xd0, 0x14, 0x10, 0x8b, 0x8d, 0xc8, 0xfa, 0xff, 0xff, 0x51, 0x8b,
                                               0xf0,
                                               0xff, 0x15, 0x74, 0xd0, 0x14, 0x10, 0x85, 0xf6, 0x0f, 0x85
                                           };
        private static readonly bool[] Mask = new bool[]
                                          {
                                              true, true, true, false, true, true, false, false, true, true, false
                                              ,
                                              false, true, true, true, true, true, true, true, false, false, false,
                                              false,
                                              true, true, true, true, false, false, false, false, true, true, true, true
                                              ,
                                              false, false, true, true, true, false, false, false, false, true, true,
                                              true, true
                                          };
		//nop, jmp
		private static readonly byte[] PatchBytes = new byte[]{0x90, 0xE9};
		
		public delegate void ProgressHandler(int percentage);
		private static int Match(byte[] data, ProgressHandler handler)
		{
			int matched = 0;
			int ret = -1;
			int pctstep = data.Length/100;
			for (int i = 0; i < data.Length; i++)
			{
				if ((i % pctstep) == 0)
					handler(i/pctstep);
				
				if (!Mask[matched])
				{
					matched++;
					continue;
				}
				
				if (data[i] == Bytes[matched])
					matched++;
				else
					matched = 0;
				
				if (matched == Bytes.Length)
				{
					ret = i - 1;
					break;
				}
			}
			handler(100);
			return ret;
		}
		
		public void Patch(string path)
		{
			var bakExt = ".bak";
			if(File.Exists(path + bakExt))
			{
				MessageDialog md = new MessageDialog(null, 
				                                     DialogFlags.Modal, 
				                                     MessageType.Info,
				                                     ButtonsType.YesNo,
			                                      "Backup file already exists! Overwrite?");
				if((ResponseType) md.Run() == ResponseType.No)
				{
					bakExt += DateTime.Now.ToFileTimeUtc();
				}
			}	
			var data = File.ReadAllBytes(path);
			MainClass.MainWindow.Log("writing backup");
			File.WriteAllBytes(path + bakExt, data);
			MainClass.MainWindow.Log("success: " + Path.GetFileName(path) + bakExt);
			MainClass.MainWindow.Log("finding patter in binary...");
			var offset = Match (data, HandleProgress);
				
			if(offset == -1)
			{
				MainClass.MainWindow.Log("failure: pattern not found!");
				return;
			}
			MainClass.MainWindow.Log("success: pattern found at 0x" + offset.ToString("X"));
				
			using (var fs = File.Open(path, FileMode.Open, FileAccess.ReadWrite))
			{
				fs.Seek(offset, SeekOrigin.Begin);
				fs.Write(PatchBytes, 0, PatchBytes.Length);
				fs.Flush();
			}
			MainClass.MainWindow.Log("successfully patched!");
		}
		
		private void HandleProgress(int percentage)
		{
			if (percentage % 10 == 0)
			{
				MainClass.MainWindow.Log(percentage + "% processed");
			}
		}
	}
}
