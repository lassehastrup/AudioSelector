using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using AudioSwitcher.AudioApi;
using AudioSwitcher.AudioApi.CoreAudio;
using System.IO;
using System.Text.Json;

namespace wf_AudioSelectorUI
{
    public partial class Form1 : Form
    {
        private static Form1 _instance;
        public static Form1 Instance => _instance;
        private readonly CoreAudioController _audioController;
        private readonly Dictionary<string, List<Keys>> _deviceShortcuts = new Dictionary<string, List<Keys>>();
        private NotifyIcon _notifyIcon;
        private bool _isRecording = false;
        private ContextMenuStrip _contextMenu;

        public Form1()
        {
            InitializeComponent();
            _instance = this;
            _audioController = new CoreAudioController();
            LoadSettings();
            InitializeNotifyIcon();
        }

        private void InitializeNotifyIcon()
        {
            _notifyIcon = new NotifyIcon
            {
                Icon = SystemIcons.Application,
                Visible = true,
                Text = "Audio Selector UI"
            };

            _contextMenu = new ContextMenuStrip();
            _contextMenu.Items.Add("Show", null, ShowForm);
            _contextMenu.Items.Add("Exit", null, ExitApplication);

            _notifyIcon.ContextMenuStrip = _contextMenu;
            _notifyIcon.DoubleClick += ShowForm;
        }

        private void ShowForm(object sender, EventArgs e)
        {
            Show();
            WindowState = FormWindowState.Normal;
            ShowInTaskbar = true;
        }

        private void ExitApplication(object sender, EventArgs e)
        {
            _notifyIcon.Visible = false;
            Application.Exit();
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            if (WindowState == FormWindowState.Minimized)
            {
                Hide();
                ShowInTaskbar = false;
            }
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            SaveSettings();
            _notifyIcon.Visible = false;
            base.OnFormClosing(e);
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            LoadAudioDevices();
        }

        private void LoadAudioDevices()
        {
            comboBoxAudioDevices.Items.Clear();
            var devices = _audioController.GetPlaybackDevices();
            if (checkBoxOnlyActiveDevices.Checked)
            {
                devices = devices.Where(d => d.State == DeviceState.Active);
            }

            foreach (var device in devices)
            {
                comboBoxAudioDevices.Items.Add(device.FullName);
            }
        }

        private void checkBoxOnlyActiveDevices_CheckedChanged(object sender, EventArgs e)
        {
            LoadAudioDevices();
        }

        private void comboBoxAudioDevices_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBoxAudioDevices.SelectedItem == null) return;
            var selectedDeviceName = comboBoxAudioDevices.SelectedItem.ToString();
            if (_deviceShortcuts.TryGetValue(selectedDeviceName, out var keys))
            {
                var keyNamesMap = new Dictionary<Keys, string>
                {
                    { Keys.Control, "Ctrl" },
                    { Keys.ControlKey, "Ctrl" },
                    { Keys.Shift, "Shift" },
                    { Keys.ShiftKey, "Shift" },
                    { Keys.Alt, "Alt" },
                    { Keys.Menu, "Alt" }
                };
                var keyNames = keys.Select(k => keyNamesMap.ContainsKey(k) ? keyNamesMap[k] : k.ToString());
                labelShortcut.Text = string.Join(", ", keyNames);
            }
            else
            {
                labelShortcut.Text = "No shortcut assigned";
            }

            // Reset the label when changing devices
            if (_isRecording)
            {
                labelShortcut.Text = "Recording...";
            }
        }

        private void buttonRefresh_Click(object sender, EventArgs e)
        {
            LoadAudioDevices();
        }

        private void ButtonaddKeyBindingToOverView(object sender, EventArgs e)
        {
            _isRecording = !_isRecording;
            buttonAddKeyBindingToList.Text = _isRecording ? "Done adding" : "Add keybinding";
            labelShortcut.Text = _isRecording ? "Press a key combination..." : "Press 'Add keybinding' to start";
            labelShortcut.BackColor = _isRecording ? System.Drawing.Color.LightCyan : System.Drawing.Color.Transparent;

            if (_isRecording && comboBoxAudioDevices.SelectedItem != null)
            {
                var selectedDeviceName = comboBoxAudioDevices.SelectedItem.ToString();
                if (_deviceShortcuts.ContainsKey(selectedDeviceName))
                {
                    _deviceShortcuts[selectedDeviceName].Clear();
                }
            }

            // Update the ListView when done adding key bindings
            if (!_isRecording)
            {
                UpdateListView();
                SaveSettings(); // Save settings after adding key bindings
            }
        }

        private void UpdateListView()
        {
            // Dictionary to map Keys values to their string representations
            var keyNamesMap = new Dictionary<Keys, string>
            {
                { Keys.Control, "Ctrl" },
                { Keys.ControlKey, "Ctrl" },
                { Keys.Shift, "Shift" },
                { Keys.ShiftKey, "Shift" },
                { Keys.Alt, "Alt" },
                { Keys.Menu, "Alt" }
            };

            listViewKeybinds.Items.Clear();
            foreach (var device in _deviceShortcuts)
            {
                var item = new ListViewItem(device.Key);
                var keyNames = device.Value.Select(k => keyNamesMap.ContainsKey(k) ? keyNamesMap[k] : k.ToString())
                    .Distinct();
                item.SubItems.Add(string.Join(", ", keyNames));
                listViewKeybinds.Items.Add(item);
            }
        }

        private void buttonViewKeybinds_Click(object sender, EventArgs e)
        {
            UpdateListView();
        }

        private void ButtonClearKeyBindings_Click(object sender, EventArgs e)
        {
            _deviceShortcuts.Clear();
            labelShortcut.Text = "No shortcut assigned";
            UpdateListView();
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (!_isRecording || comboBoxAudioDevices.SelectedItem == null)
            {
                return base.ProcessCmdKey(ref msg, keyData);
            }

            var selectedDeviceName = comboBoxAudioDevices.SelectedItem.ToString();
            if (!_deviceShortcuts.TryGetValue(selectedDeviceName, out var keys))
            {
                keys = new List<Keys>();
                _deviceShortcuts[selectedDeviceName] = keys;
            }

            // Split keyData into individual keys
            var individualKeys = keyData.ToString().Split(new[] { ", " }, StringSplitOptions.RemoveEmptyEntries)
                .Select(k => (Keys)Enum.Parse(typeof(Keys), k))
                .Where(key => key != Keys.None && key != Keys.LButton && key != Keys.RButton && key != Keys.XButton1)
                .ToList();

            // Add the individual keys to the list of keys for the selected device
            foreach (var key in individualKeys)
            {
                if (!keys.Contains(key))
                {
                    keys.Add(key);
                }
            }

            // Update the label with the key combination
            var keyNamesMap = new Dictionary<Keys, string>
            {
                { Keys.Control, "Ctrl" },
                { Keys.ControlKey, "Ctrl" },
                { Keys.Shift, "Shift" },
                { Keys.ShiftKey, "Shift" },
                { Keys.Alt, "Alt" },
                { Keys.Menu, "Alt" }
            };

            var keyNames = keys.Select(k => keyNamesMap.ContainsKey(k) ? keyNamesMap[k] : k.ToString()).Distinct();
            labelShortcut.Text = string.Join(" + ", keyNames);

            UpdateListView(); // Update the ListView whenever a new keybinding is added
            return true; // Indicate that the key has been handled
        }

        public void HandleGlobalKeyDown(KeyEventArgs e)
        {
            foreach (var deviceShortcut in _deviceShortcuts)
            {
                var keys = deviceShortcut.Value
                    .Where(key => key != Keys.None && key != Keys.LButton && key != Keys.RButton).ToList();
                if (keys.Count == 0) continue;

                // Check if the first key is a modifier key
                var firstKey = keys[0];
                if (e.Modifiers.HasFlag(firstKey) && keys.Skip(1).All(k => e.KeyCode == k))
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

        private static readonly string SettingsFilePath = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.UserProfile),
            "AudioDeviceKeyMappingConfiguration.json"
        );

        private void SaveSettings()
        {
            try
            {
                var keyNamesMap = new Dictionary<Keys, string>
                {
                    { Keys.Control, "Ctrl" },
                    { Keys.ControlKey, "Ctrl" },
                    { Keys.Shift, "Shift" },
                    { Keys.ShiftKey, "Shift" },
                    { Keys.Alt, "Alt" },
                    { Keys.Menu, "Alt" }
                };

                var mappedDeviceShortcuts = _deviceShortcuts.ToDictionary(
                    kvp => kvp.Key,
                    kvp => kvp.Value.Select(k => keyNamesMap.ContainsKey(k) ? keyNamesMap[k] : k.ToString()).ToList()
                );

                var settings = new AudioSelectorSettings
                {
                    SelectedDevice = comboBoxAudioDevices.SelectedItem?.ToString(),
                    DeviceShortcuts = mappedDeviceShortcuts
                };

                var json = JsonSerializer.Serialize(settings, new JsonSerializerOptions { WriteIndented = true });
                File.WriteAllText(SettingsFilePath, json);
                Console.WriteLine("Settings saved successfully.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to save settings: {ex.Message}");
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
                    var filteredKeys = new List<Keys>();
                    foreach (var key in kvp.Value)
                    {
                        if (Enum.TryParse(typeof(Keys), key.ToString(), out var parsedKey) && parsedKey is Keys validKey)
                        {
                            if (validKey != Keys.None && validKey != Keys.LButton && validKey != Keys.RButton && validKey != Keys.XButton1)
                            {
                                filteredKeys.Add(validKey);
                            }
                        }
                        else
                        {
                            Console.WriteLine($"Invalid key: {key}");
                        }
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


    public class AudioSelectorSettings
    {
        public string SelectedDevice { get; set; }
        public Dictionary<string, List<string>> DeviceShortcuts { get; set; }
    }
}