namespace wf_AudioSelectorUI
{
    partial class Form1
    {
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.ComboBox comboBoxAudioDevices;
        private System.Windows.Forms.Button buttonRefresh;
        private System.Windows.Forms.CheckBox checkBoxOnlyActiveDevices;
        private System.Windows.Forms.Label labelShortcut;
        private System.Windows.Forms.Button buttonAddKeyBindingToList;
        private System.Windows.Forms.ListView listViewKeybinds;
        private System.Windows.Forms.ColumnHeader columnHeaderDevice;
        private System.Windows.Forms.ColumnHeader columnHeaderKeybinds;
        private System.Windows.Forms.ToolTip toolTip;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }

            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            comboBoxAudioDevices = new System.Windows.Forms.ComboBox();
            buttonRefresh = new System.Windows.Forms.Button();
            checkBoxOnlyActiveDevices = new System.Windows.Forms.CheckBox();
            labelShortcut = new System.Windows.Forms.Label();
            buttonAddKeyBindingToList = new System.Windows.Forms.Button();
            listViewKeybinds = new System.Windows.Forms.ListView();
            columnHeaderDevice = new System.Windows.Forms.ColumnHeader();
            columnHeaderKeybinds = new System.Windows.Forms.ColumnHeader();
            toolTip = new System.Windows.Forms.ToolTip(components);
            SuspendLayout();
            //
            // comboBoxAudioDevices
            //
            comboBoxAudioDevices.FormattingEnabled = true;
            comboBoxAudioDevices.Location = new System.Drawing.Point(14, 14);
            comboBoxAudioDevices.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            comboBoxAudioDevices.Name = "comboBoxAudioDevices";
            comboBoxAudioDevices.Size = new System.Drawing.Size(303, 23);
            comboBoxAudioDevices.TabIndex = 0;
            comboBoxAudioDevices.SelectedIndexChanged += comboBoxAudioDevices_SelectedIndexChanged;
            //
            // buttonRefresh
            //
            buttonRefresh.Location = new System.Drawing.Point(14, 45);
            buttonRefresh.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            buttonRefresh.Name = "buttonRefresh";
            buttonRefresh.Size = new System.Drawing.Size(88, 27);
            buttonRefresh.TabIndex = 1;
            buttonRefresh.Text = "Refresh";
            buttonRefresh.UseVisualStyleBackColor = true;
            buttonRefresh.Click += buttonRefresh_Click;
            //
            // checkBoxOnlyActiveDevices
            //
            checkBoxOnlyActiveDevices.Location = new System.Drawing.Point(14, 78);
            checkBoxOnlyActiveDevices.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            checkBoxOnlyActiveDevices.Name = "checkBoxOnlyActiveDevices";
            checkBoxOnlyActiveDevices.Size = new System.Drawing.Size(150, 24);
            checkBoxOnlyActiveDevices.TabIndex = 2;
            checkBoxOnlyActiveDevices.Text = "Only Active Devices";
            checkBoxOnlyActiveDevices.UseVisualStyleBackColor = true;
            checkBoxOnlyActiveDevices.CheckedChanged += checkBoxOnlyActiveDevices_CheckedChanged;
            //
            // labelShortcut
            //
            labelShortcut.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            labelShortcut.Location = new System.Drawing.Point(14, 108);
            labelShortcut.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            labelShortcut.Name = "labelShortcut";
            labelShortcut.Size = new System.Drawing.Size(303, 23);
            labelShortcut.TabIndex = 3;
            labelShortcut.Text = "Press 'Record' to start";
            labelShortcut.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            //
            // buttonAddKeyBindingToList
            //
            buttonAddKeyBindingToList.Location = new System.Drawing.Point(14, 140);
            buttonAddKeyBindingToList.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            buttonAddKeyBindingToList.Name = "buttonAddKeyBindingToList";
            buttonAddKeyBindingToList.Size = new System.Drawing.Size(100, 23);
            buttonAddKeyBindingToList.TabIndex = 4;
            buttonAddKeyBindingToList.Text = "Add keybinding";
            buttonAddKeyBindingToList.UseVisualStyleBackColor = true;
            buttonAddKeyBindingToList.Click += ButtonaddKeyBindingToOverView;
            //
            // listViewKeybinds
            //
            listViewKeybinds.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            columnHeaderDevice,
            columnHeaderKeybinds});
            listViewKeybinds.Location = new System.Drawing.Point(14, 170);
            listViewKeybinds.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            listViewKeybinds.Name = "listViewKeybinds";
            listViewKeybinds.Size = new System.Drawing.Size(303, 120);
            listViewKeybinds.TabIndex = 5;
            listViewKeybinds.UseCompatibleStateImageBehavior = false;
            listViewKeybinds.View = System.Windows.Forms.View.Details;
            //
            // columnHeaderDevice
            //
            columnHeaderDevice.Text = "Device Name";
            columnHeaderDevice.Width = 150;
            //
            // columnHeaderKeybinds
            //
            columnHeaderKeybinds.Text = "Keybinding";
            columnHeaderKeybinds.Width = 150;
            //
            // toolTip
            //
            toolTip.SetToolTip(buttonAddKeyBindingToList, "Click to add a new keybinding");
            //
            // Form1
            //
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            ClientSize = new System.Drawing.Size(331, 301);
            Controls.Add(listViewKeybinds);
            Controls.Add(buttonAddKeyBindingToList);
            Controls.Add(labelShortcut);
            Controls.Add(checkBoxOnlyActiveDevices);
            Controls.Add(buttonRefresh);
            Controls.Add(comboBoxAudioDevices);
            Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            Text = "Audio Selector";
            Load += Form1_Load;
            ResumeLayout(false);
            PerformLayout();
        }
    }
}