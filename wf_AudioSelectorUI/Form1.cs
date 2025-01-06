using AudioSwitcher.AudioApi;
using AudioSwitcher.AudioApi.CoreAudio;

namespace wf_AudioSelectorUI;

public partial class Form1 : Form
{
    private readonly CoreAudioController _audioController;
    private readonly Dictionary<string, Keys> _deviceShortcuts = new Dictionary<string, Keys>();

    public Form1()
    {
        InitializeComponent();
        _audioController = new CoreAudioController();
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
            devices = devices.Where(d => d.State == AudioSwitcher.AudioApi.DeviceState.Active);
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
        var selectedDeviceName = comboBoxAudioDevices.SelectedItem?.ToString();
        if (selectedDeviceName != null && _deviceShortcuts.ContainsKey(selectedDeviceName))
        {
            textBoxShortcut.Text = _deviceShortcuts[selectedDeviceName].ToString();
        }
        else
        {
            textBoxShortcut.Clear();
        }
    }

    private void buttonRefresh_Click(object sender, EventArgs e)
    {
        LoadAudioDevices();
    }

    private void checkBox1_CheckedChanged(object sender, EventArgs e)
    {
        throw new NotImplementedException();
    }

    private void textBoxShortcut_KeyDown(object sender, KeyEventArgs e)
    {
        var selectedDeviceName = comboBoxAudioDevices.SelectedItem?.ToString();
        if (selectedDeviceName == null) return;
        _deviceShortcuts[selectedDeviceName] = e.KeyCode;
        textBoxShortcut.Text = e.KeyCode.ToString();
    }
}