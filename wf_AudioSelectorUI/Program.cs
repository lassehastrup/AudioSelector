using System;
using System.Windows.Forms;
using Gma.System.MouseKeyHook;

namespace wf_AudioSelectorUI
{
    internal static class Program
    {
        private static IKeyboardMouseEvents _globalHook;

        [STAThread]
        private static void Main()
        {
            ApplicationConfiguration.Initialize();
            _globalHook = Hook.GlobalEvents();
            _globalHook.KeyDown += GlobalHook_KeyDown;
            Application.Run(new Form1());
        }

        private static void GlobalHook_KeyDown(object sender, KeyEventArgs e)
        {
            Form1.Instance.HandleHotkey(e);
        }
    }
}