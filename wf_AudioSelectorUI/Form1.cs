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
        private readonly CoreAudioController _audioController;
        private readonly Dictionary<string, List<Keys>> _deviceShortcuts = new Dictionary<string, List<Keys>>();
        private bool _isRecording = false;

        public Form1()
        {
            InitializeComponent();
            _audioController = new CoreAudioController();
            LoadSettings();
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            SaveSettings();
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
            labelShortcut.BackColor =
                _isRecording ? System.Drawing.Color.LightYellow : System.Drawing.Color.Transparent;

            // Update the ListView when done adding key bindings
            if (!_isRecording)
            {
                UpdateListView();
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
                var keyNames = device.Value.Select(k => keyNamesMap.ContainsKey(k) ? keyNamesMap[k] : k.ToString());
                item.SubItems.Add(string.Join(", ", keyNames));
                listViewKeybinds.Items.Add(item);
            }
        }

        private void buttonViewKeybinds_Click(object sender, EventArgs e)
        {
            UpdateListView();
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            // Log the keyData information
            Console.WriteLine($"KeyData: {keyData}");

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

            // List to store the key names
            var keyNames = new List<string>();

            // Check for specific key values and add their names to the list
            foreach (var key in keyNamesMap.Keys)
            {
                if (keyData.HasFlag(key))
                {
                    keyNames.Add(keyNamesMap[key]);
                }
            }

            // Remove the modifier keys from keyData to get the non-modifier key
            var nonModifierKey = keyData & ~Keys.Control & ~Keys.Shift & ~Keys.Alt;
            if (nonModifierKey != Keys.None && !keyNamesMap.ContainsKey(nonModifierKey))
            {
                keyNames.Add(nonModifierKey.ToString());
            }

            // Remove duplicates
            keyNames = keyNames.Distinct().ToList();

            // Join the key names with " + " and update the label
            var formattedKeys = string.Join(" + ", keyNames);

            if (!_isRecording || comboBoxAudioDevices.SelectedItem == null)
            {
                labelShortcut.Text = formattedKeys;
                return base.ProcessCmdKey(ref msg, keyData);
            }

            var selectedDeviceName = comboBoxAudioDevices.SelectedItem.ToString();
            if (!_deviceShortcuts.TryGetValue(selectedDeviceName, out _))
            {
                _deviceShortcuts[selectedDeviceName] = new List<Keys>();
            }

            // Update the keyData for the selected device
            _deviceShortcuts[selectedDeviceName] = new List<Keys> { keyData };

            // Update the label with the key combination
            labelShortcut.Text = string.Join(" + ",
                _deviceShortcuts[selectedDeviceName]
                    .Select(k => keyNamesMap.ContainsKey(k) ? keyNamesMap[k] : k.ToString()));
            UpdateListView(); // Update the ListView whenever a new keybinding is added
            return true; // Indicate that the key has been handled
        }

        private const string SettingsFilePath = "AudioDeviceKeyMappingConfiguration.json";

        private void SaveSettings()
        {
            var settings = new AudioSelectorSettings
            {
                SelectedDevice = comboBoxAudioDevices.SelectedItem?.ToString(),
                DeviceShortcuts = _deviceShortcuts
            };

            var json = JsonSerializer.Serialize(settings, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(SettingsFilePath, json);
        }

        private void LoadSettings()
        {
            if (!File.Exists(SettingsFilePath)) return;

            var json = File.ReadAllText(SettingsFilePath);
            var settings = JsonSerializer.Deserialize<AudioSelectorSettings>(json);

            if (settings != null)
            {
                _deviceShortcuts.Clear();
                foreach (var kvp in settings.DeviceShortcuts)
                {
                    _deviceShortcuts[kvp.Key] = kvp.Value;
                }

                comboBoxAudioDevices.SelectedItem = settings.SelectedDevice;
                UpdateListView();
            }
        }
    }

    public class AudioSelectorSettings
    {
        public string SelectedDevice { get; set; }
        public Dictionary<string, List<Keys>> DeviceShortcuts { get; set; }
    }
    
}