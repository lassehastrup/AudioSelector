namespace wf_AudioSelectorUI
{
    partial class Form1
    {
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.ComboBox comboBoxAudioDevices;
        private System.Windows.Forms.Button buttonRefresh;
        private System.Windows.Forms.CheckBox checkBoxOnlyActiveDevices;
        private System.Windows.Forms.TextBox textBoxShortcut;

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
            comboBoxAudioDevices = new System.Windows.Forms.ComboBox();
            buttonRefresh = new System.Windows.Forms.Button();
            checkBoxOnlyActiveDevices = new System.Windows.Forms.CheckBox();
            textBoxShortcut = new System.Windows.Forms.TextBox();
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
            // textBoxShortcut
            //
            textBoxShortcut.Location = new System.Drawing.Point(14, 108);
            textBoxShortcut.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            textBoxShortcut.Name = "textBoxShortcut";
            textBoxShortcut.Size = new System.Drawing.Size(303, 23);
            textBoxShortcut.TabIndex = 3;
            textBoxShortcut.KeyDown += textBoxShortcut_KeyDown;
            //
            // Form1
            //
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            ClientSize = new System.Drawing.Size(331, 301);
            Controls.Add(textBoxShortcut);
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