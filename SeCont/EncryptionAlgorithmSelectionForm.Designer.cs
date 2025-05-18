namespace SeCont
{
    partial class EncryptionAlgorithmSelectionForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            encryptionGroupBox = new GroupBox();
            encryptionComboBox = new ComboBox();
            okButton = new Button();
            cancelButton = new Button();
            encryptionGroupBox.SuspendLayout();
            SuspendLayout();
            // 
            // encryptionGroupBox
            // 
            encryptionGroupBox.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            encryptionGroupBox.Controls.Add(encryptionComboBox);
            encryptionGroupBox.Location = new Point(12, 12);
            encryptionGroupBox.Name = "encryptionGroupBox";
            encryptionGroupBox.Size = new Size(338, 51);
            encryptionGroupBox.TabIndex = 0;
            encryptionGroupBox.TabStop = false;
            encryptionGroupBox.Text = "Encryption Algorithm";
            // 
            // encryptionComboBox
            // 
            encryptionComboBox.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            encryptionComboBox.DropDownStyle = ComboBoxStyle.DropDownList;
            encryptionComboBox.FormattingEnabled = true;
            encryptionComboBox.Location = new Point(6, 22);
            encryptionComboBox.Name = "encryptionComboBox";
            encryptionComboBox.Size = new Size(326, 23);
            encryptionComboBox.TabIndex = 0;
            // 
            // okButton
            // 
            okButton.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            okButton.Location = new Point(194, 73);
            okButton.Name = "okButton";
            okButton.Size = new Size(75, 23);
            okButton.TabIndex = 1;
            okButton.Text = "OK";
            okButton.UseVisualStyleBackColor = true;
            okButton.Click += okButton_Click;
            // 
            // cancelButton
            // 
            cancelButton.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            cancelButton.Location = new Point(275, 73);
            cancelButton.Name = "cancelButton";
            cancelButton.Size = new Size(75, 23);
            cancelButton.TabIndex = 2;
            cancelButton.Text = "Cancel";
            cancelButton.UseVisualStyleBackColor = true;
            cancelButton.Click += cancelButton_Click;
            // 
            // EncryptionAlgorithmSelectionForm
            // 
            AcceptButton = okButton;
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            CancelButton = cancelButton;
            ClientSize = new Size(362, 108);
            Controls.Add(cancelButton);
            Controls.Add(okButton);
            Controls.Add(encryptionGroupBox);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            Name = "EncryptionAlgorithmSelectionForm";
            ShowIcon = false;
            StartPosition = FormStartPosition.CenterParent;
            Text = "Please select the Encryption Algorithm";
            encryptionGroupBox.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private GroupBox encryptionGroupBox;
        private ComboBox encryptionComboBox;
        private Button okButton;
        private Button cancelButton;
    }
}