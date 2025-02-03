using System;
using System.Windows.Forms;

namespace wf_AudioSelectorUI
{
    internal static class Program
    {
        [STAThread]
        private static void Main()
        {
            ApplicationConfiguration.Initialize();
            Application.Run(new Form1());
            Application.Run(new BackgroundProcess());
        }
    }
}