using Gma.System.MouseKeyHook;
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
        private IKeyboardMouseEvents _globalHook;

        public Form1()
        {
            InitializeComponent();
            _instance = this;
            _audioController = new CoreAudioController();
            LoadSettings();
            InitializeNotifyIcon();
            SetupGlobalKeyHandling();
        }

        private void SetupGlobalKeyHandling()
        {
            _globalHook = Hook.GlobalEvents();
            _globalHook.KeyDown += GlobalHook_KeyDown;
        }

        private void GlobalHook_KeyDown(object sender, KeyEventArgs e)
        {
            if (!_isRecording || comboBoxAudioDevices.SelectedItem == null)
            {
                return;
            }

            var selectedDeviceName = comboBoxAudioDevices.SelectedItem.ToString();
            if (!_deviceShortcuts.TryGetValue(selectedDeviceName, out var keys))
            {
                keys = new List<Keys>();
                _deviceShortcuts[selectedDeviceName] = keys;
            }

            if (e.KeyCode != Keys.None && !keys.Contains(e.KeyCode))
            {
                keys.Add(e.KeyCode);
            }

            // Ensure only a maximum of two keys are recorded
            if (keys.Count > 2)
            {
                keys = keys.Take(2).ToList();
                _deviceShortcuts[selectedDeviceName] = keys;
                _isRecording = false; // Stop recording after two keys
                buttonAddKeyBindingToList.Text = "Add keybinding";
                labelShortcut.BackColor = System.Drawing.Color.Transparent;
            }

            // Update the label with the key combination
            var keyNames = keys.Select(k => k.ToString());
            labelShortcut.Text = string.Join(" + ", keyNames);

            UpdateListView(); // Update the ListView whenever a new keybinding is added
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
            _globalHook.Dispose(); // Dispose the global hook
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
                var keyNames = keys.ToString();
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
            listViewKeybinds.Items.Clear();
            foreach (var deviceShortcut in _deviceShortcuts)
            {
                var keys = deviceShortcut.Value
                    .Where(key =>
                        key != Keys.None && key != Keys.LButton && key != Keys.RButton && key != Keys.XButton1)
                    .Take(2) // Ensure only a maximum of two keys are taken
                    .ToList();

                if (keys.Count > 0)
                {
                    var keyNames = keys.Select(k => k.ToString());
                    var combinedKeys = string.Join(" + ", keyNames);
                    var listViewItem = new ListViewItem(deviceShortcut.Key);
                    listViewItem.SubItems.Add(combinedKeys);
                    listViewKeybinds.Items.Add(listViewItem);
                }
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

        private static readonly string SettingsFilePath = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.UserProfile),
            "AudioDeviceKeyMappingConfiguration.json"
        );

        private void SaveSettings()
        {
            try
            {
                var mappedDeviceShortcuts = _deviceShortcuts.ToDictionary(
                    kvp => kvp.Key,
                    kvp => kvp.Value.Take(2).Select(k => k.ToString()).ToList() // Ensure only two keys are saved
                );

                var settings = new AudioSelectorSettings
                {
                    SelectedDevice = comboBoxAudioDevices.SelectedItem?.ToString(),
                    DeviceShortcuts = mappedDeviceShortcuts
                };

                var json = JsonSerializer.Serialize(settings, new JsonSerializerOptions { WriteIndented = true });
                File.WriteAllText(SettingsFilePath, json);
                Console.WriteLine(json);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to save settings: {ex.Message}");
            }
        }

        private void LoadSettings()
        {
            string userProfilePath = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
            string settingsFilePath = Path.Combine(userProfilePath, "AudioDeviceKeyMappingConfiguration.json");

            if (!File.Exists(settingsFilePath))
            {
                Console.WriteLine("Settings file not found.");
                Console.WriteLine("Creating the file: " + settingsFilePath);
                File.WriteAllText(settingsFilePath, JsonSerializer.Serialize(new AudioSelectorSettings()));
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
                    Console.WriteLine(
                        $"Loaded key combination for device {kvp.Key}: {string.Join(", ", filteredKeys)}");
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