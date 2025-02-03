using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Windows.Forms;
using AudioSwitcher.AudioApi;
using AudioSwitcher.AudioApi.CoreAudio;
using Gma.System.MouseKeyHook;
using Microsoft.Win32;

namespace wf_AudioSelectorUI
{
    public class BackgroundProcess : ApplicationContext
    {
        private static BackgroundProcess _instance;
        public static BackgroundProcess Instance => _instance;
        private readonly CoreAudioController _audioController;
        private readonly Dictionary<string, List<Keys>> _deviceShortcuts = new Dictionary<string, List<Keys>>();
        private NotifyIcon _notifyIcon;
        private HashSet<Keys> _pressedKeys = new HashSet<Keys>();

        public BackgroundProcess()
        {
            _instance = this;
            _audioController = new CoreAudioController();
            LoadSettings();
            InitializeNotifyIcon();
            SetupGlobalKeyHandling();
            RegisterInStartup(true); // The application will start with Windows
        }

        private void InitializeNotifyIcon()
        {
            _notifyIcon = new NotifyIcon
            {
                Icon = new Icon("audioAppSwitcher.ico.ico"), // Ensure the path is correct
                Visible = true,
                Text = "Audio Selector UI"
            };

            var contextMenu = new ContextMenuStrip();
            contextMenu.Items.Add("Exit", null, ExitApplication);

            _notifyIcon.ContextMenuStrip = contextMenu;
        }

        private void ExitApplication(object sender, EventArgs e)
        {
            _notifyIcon.Visible = false;
            Application.Exit();
        }

        private void SetupGlobalKeyHandling()
        {
            var globalHook = Hook.GlobalEvents();
            globalHook.KeyDown += GlobalHook_KeyDown;
            globalHook.KeyUp += GlobalHook_KeyUp;
        }

        private void GlobalHook_KeyDown(object sender, KeyEventArgs e)
        {
            _pressedKeys.Add(e.KeyCode);
            Console.WriteLine($"Key Pressed: {e.KeyCode}");
            HandleGlobalKeyDown();
        }

        private void GlobalHook_KeyUp(object sender, KeyEventArgs e)
        {
            _pressedKeys.Remove(e.KeyCode);
        }

        public void HandleGlobalKeyDown()
        {
            foreach (var deviceShortcut in _deviceShortcuts)
            {
                var keys = deviceShortcut.Value;
                if (keys.Count == 0) continue;

                // Check if the key combination matches
                if (keys.All(k => _pressedKeys.Contains(k)))
                {
                    var device = _audioController.GetPlaybackDevices()
                        .FirstOrDefault(d => d.FullName == deviceShortcut.Key);

                    if (device != null)
                    {
                        device.SetAsDefault();
                        Console.WriteLine($"Audio device switched to: {device.FullName}");
                    }
                }
            }
        }

        private void RegisterInStartup(bool isChecked)
        {
            string runKey = @"SOFTWARE\Microsoft\Windows\CurrentVersion\Run";
            using (RegistryKey key = Registry.CurrentUser.OpenSubKey(runKey, true))
            {
                if (isChecked)
                {
                    key.SetValue("AudioSelectorUI", Application.ExecutablePath);
                }
                else
                {
                    key.DeleteValue("AudioSelectorUI", false);
                }
            }
        }

        private void LoadSettings()
        {
            string userProfilePath = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
            string settingsFilePath = Path.Combine(userProfilePath, "AudioDeviceKeyMappingConfiguration.json");

            if (!File.Exists(settingsFilePath))
            {
                Console.WriteLine("Settings file not found.");
                return;
            }

            var json = File.ReadAllText(settingsFilePath);
            var settings = JsonSerializer.Deserialize<AudioSelectorSettings>(json);

            if (settings != null)
            {
                _deviceShortcuts.Clear();
                foreach (var kvp in settings.DeviceShortcuts)
                {
                    var filteredKeys = new List<Keys>();
                    foreach (var key in kvp.Value)
                    {
                        filteredKeys.Add((Keys)Enum.Parse(typeof(Keys), key));
                    }

                    _deviceShortcuts[kvp.Key] = filteredKeys;
                    Console.WriteLine($"Loaded key combination for device {kvp.Key}: {string.Join(", ", filteredKeys)}");
                }
            }
            else
            {
                Console.WriteLine("Failed to deserialize settings.");
            }
        }
    }
}