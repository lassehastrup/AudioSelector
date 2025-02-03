using System;
using System.Windows.Forms;

namespace wf_AudioSelectorUI
{
    internal static class Program
    {
        [STAThread]
        private static void Main(string[] args)
        {
            ApplicationConfiguration.Initialize();
            Application.Run(new BackgroundProcess());
            Application.Run(new Form1());
            
        }
    }
}