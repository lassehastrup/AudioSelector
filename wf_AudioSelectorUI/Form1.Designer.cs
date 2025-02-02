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
        private System.Windows.Forms.GroupBox groupBoxAudioDevices;
        private System.Windows.Forms.GroupBox groupBoxKeyBinding;
        private System.Windows.Forms.GroupBox groupBoxKeybindsList;
        private System.Windows.Forms.Button buttonClearKeyBindings;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }

            base.Dispose(disposing);
        }

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            comboBoxAudioDevices = new System.Windows.Forms.ComboBox();
            buttonRefresh = new System.Windows.Forms.Button();
            checkBoxOnlyActiveDevices = new System.Windows.Forms.CheckBox();
            labelShortcut = new System.Windows.Forms.Label();
            buttonAddKeyBindingToList = new System.Windows.Forms.Button();
            listViewKeybinds = new System.Windows.Forms.ListView();
            columnHeaderDevice = new System.Windows.Forms.ColumnHeader();
            columnHeaderKeybinds = new System.Windows.Forms.ColumnHeader();
            toolTip = new System.Windows.Forms.ToolTip(components);
            groupBoxAudioDevices = new System.Windows.Forms.GroupBox();
            groupBoxKeyBinding = new System.Windows.Forms.GroupBox();
            buttonClearKeyBindings = new System.Windows.Forms.Button();
            groupBoxKeybindsList = new System.Windows.Forms.GroupBox();
            groupBoxAudioDevices.SuspendLayout();
            groupBoxKeyBinding.SuspendLayout();
            groupBoxKeybindsList.SuspendLayout();
            SuspendLayout();
            // 
            // comboBoxAudioDevices
            // 
            comboBoxAudioDevices.FormattingEnabled = true;
            comboBoxAudioDevices.Location = new System.Drawing.Point(10, 22);
            comboBoxAudioDevices.Name = "comboBoxAudioDevices";
            comboBoxAudioDevices.Size = new System.Drawing.Size(300, 23);
            comboBoxAudioDevices.TabIndex = 0;
            comboBoxAudioDevices.SelectedIndexChanged += comboBoxAudioDevices_SelectedIndexChanged;
            // 
            // buttonRefresh
            // 
            buttonRefresh.Location = new System.Drawing.Point(10, 51);
            buttonRefresh.Name = "buttonRefresh";
            buttonRefresh.Size = new System.Drawing.Size(88, 27);
            buttonRefresh.TabIndex = 1;
            buttonRefresh.Text = "Refresh";
            buttonRefresh.UseVisualStyleBackColor = true;
            buttonRefresh.Click += buttonRefresh_Click;
            // 
            // checkBoxOnlyActiveDevices
            // 
            checkBoxOnlyActiveDevices.Location = new System.Drawing.Point(10, 84);
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
            labelShortcut.Location = new System.Drawing.Point(10, 22);
            labelShortcut.Name = "labelShortcut";
            labelShortcut.Size = new System.Drawing.Size(300, 23);
            labelShortcut.TabIndex = 0;
            labelShortcut.Text = "Press \'Record\' to start";
            labelShortcut.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // buttonAddKeyBindingToList
            // 
            buttonAddKeyBindingToList.Location = new System.Drawing.Point(10, 51);
            buttonAddKeyBindingToList.Name = "buttonAddKeyBindingToList";
            buttonAddKeyBindingToList.Size = new System.Drawing.Size(100, 23);
            buttonAddKeyBindingToList.TabIndex = 1;
            buttonAddKeyBindingToList.Text = "Add keybinding";
            toolTip.SetToolTip(buttonAddKeyBindingToList, "Click to add a new keybinding");
            buttonAddKeyBindingToList.UseVisualStyleBackColor = true;
            buttonAddKeyBindingToList.Click += ButtonaddKeyBindingToOverView;
            // 
            // listViewKeybinds
            // 
            listViewKeybinds.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] { columnHeaderDevice, columnHeaderKeybinds });
            listViewKeybinds.Location = new System.Drawing.Point(10, 22);
            listViewKeybinds.Name = "listViewKeybinds";
            listViewKeybinds.Size = new System.Drawing.Size(300, 120);
            listViewKeybinds.TabIndex = 0;
            listViewKeybinds.UseCompatibleStateImageBehavior = false;
            listViewKeybinds.View = System.Windows.Forms.View.Details;
            // 
            // columnHeaderDevice
            // 
            columnHeaderDevice.Name = "columnHeaderDevice";
            columnHeaderDevice.Text = "Device Name";
            columnHeaderDevice.Width = 150;
            // 
            // columnHeaderKeybinds
            // 
            columnHeaderKeybinds.Name = "columnHeaderKeybinds";
            columnHeaderKeybinds.Text = "Keybinding";
            columnHeaderKeybinds.Width = 150;
            // 
            // groupBoxAudioDevices
            // 
            groupBoxAudioDevices.Controls.Add(comboBoxAudioDevices);
            groupBoxAudioDevices.Controls.Add(buttonRefresh);
            groupBoxAudioDevices.Controls.Add(checkBoxOnlyActiveDevices);
            groupBoxAudioDevices.Location = new System.Drawing.Point(12, 12);
            groupBoxAudioDevices.Name = "groupBoxAudioDevices";
            groupBoxAudioDevices.Size = new System.Drawing.Size(320, 110);
            groupBoxAudioDevices.TabIndex = 0;
            groupBoxAudioDevices.TabStop = false;
            groupBoxAudioDevices.Text = "Audio Devices";
            // 
            // groupBoxKeyBinding
            // 
            groupBoxKeyBinding.Controls.Add(labelShortcut);
            groupBoxKeyBinding.Controls.Add(buttonAddKeyBindingToList);
            groupBoxKeyBinding.Controls.Add(buttonClearKeyBindings);
            groupBoxKeyBinding.Location = new System.Drawing.Point(12, 128);
            groupBoxKeyBinding.Name = "groupBoxKeyBinding";
            groupBoxKeyBinding.Size = new System.Drawing.Size(320, 80);
            groupBoxKeyBinding.TabIndex = 1;
            groupBoxKeyBinding.TabStop = false;
            groupBoxKeyBinding.Text = "Key Binding";
            // 
            // buttonClearKeyBindings
            // 
            buttonClearKeyBindings.Location = new System.Drawing.Point(120, 51);
            buttonClearKeyBindings.Name = "buttonClearKeyBindings";
            buttonClearKeyBindings.Size = new System.Drawing.Size(100, 23);
            buttonClearKeyBindings.TabIndex = 2;
            buttonClearKeyBindings.Text = "Clear";
            buttonClearKeyBindings.UseVisualStyleBackColor = true;
            buttonClearKeyBindings.Click += ButtonClearKeyBindings_Click;
            // 
            // groupBoxKeybindsList
            // 
            groupBoxKeybindsList.Controls.Add(listViewKeybinds);
            groupBoxKeybindsList.Location = new System.Drawing.Point(12, 214);
            groupBoxKeybindsList.Name = "groupBoxKeybindsList";
            groupBoxKeybindsList.Size = new System.Drawing.Size(320, 150);
            groupBoxKeybindsList.TabIndex = 2;
            groupBoxKeybindsList.TabStop = false;
            groupBoxKeybindsList.Text = "Keybinds List";
            // 
            // Form1
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            ClientSize = new System.Drawing.Size(344, 376);
            Controls.Add(groupBoxKeybindsList);
            Controls.Add(groupBoxKeyBinding);
            Controls.Add(groupBoxAudioDevices);
            Icon = ((System.Drawing.Icon)resources.GetObject("$this.Icon"));
            Opacity = 0.92D;
            Text = "Audio Selector";
            TopMost = true;
            Load += Form1_Load;
            groupBoxAudioDevices.ResumeLayout(false);
            groupBoxKeyBinding.ResumeLayout(false);
            groupBoxKeybindsList.ResumeLayout(false);
            ResumeLayout(false);
        }
    }
}