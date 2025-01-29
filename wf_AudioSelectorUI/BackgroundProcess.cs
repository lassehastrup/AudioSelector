﻿using System;
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
                Icon = SystemIcons.Application,
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
        }

        private void GlobalHook_KeyDown(object sender, KeyEventArgs e)
        {
            Console.WriteLine($"Key Pressed: {e.KeyCode}");
            HandleGlobalKeyDown(e);
        }

        public void HandleGlobalKeyDown(KeyEventArgs e)
        {
            foreach (var deviceShortcut in _deviceShortcuts)
            {
                if (deviceShortcut.Value.Contains(e.KeyCode))
                {
                    Console.WriteLine($"Key Combination Matched: {e.KeyCode} for device {deviceShortcut.Key}");
                    var device = _audioController.GetPlaybackDevices()
                        .FirstOrDefault(d => d.FullName == deviceShortcut.Key);

                    if (device == null)
                    {
                        Console.WriteLine($"Device not found: {deviceShortcut.Key}");
                        continue;
                    }

                    Console.WriteLine($"Found device: {device.FullName}, State: {device.State}");

                    // Set the device as default regardless of its current state
                    device.SetAsDefault();
                    Console.WriteLine($"Audio device switched to: {device.FullName}");
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
            const string SettingsFilePath = "AudioDeviceKeyMappingConfiguration.json";
            if (!File.Exists(SettingsFilePath))
            {
                Console.WriteLine("Settings file not found.");
                return;
            }

            var json = File.ReadAllText(SettingsFilePath);
            var settings = JsonSerializer.Deserialize<AudioSelectorSettings>(json);

            if (settings != null)
            {
                _deviceShortcuts.Clear();
                foreach (var kvp in settings.DeviceShortcuts)
                {
                    _deviceShortcuts[kvp.Key] = kvp.Value;
                    Console.WriteLine($"Loaded key combination for device {kvp.Key}: {string.Join(", ", kvp.Value)}");
                }
            }
            else
            {
                Console.WriteLine("Failed to deserialize settings.");
            }
        }
    }
}