namespace SeCont
{
    partial class PasswordForm
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
            passwordInputGroupBox = new GroupBox();
            passwordTextBox = new TextBox();
            okButton = new Button();
            cancelButton = new Button();
            passwordInputGroupBox.SuspendLayout();
            SuspendLayout();
            // 
            // passwordInputGroupBox
            // 
            passwordInputGroupBox.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            passwordInputGroupBox.Controls.Add(passwordTextBox);
            passwordInputGroupBox.Location = new Point(12, 12);
            passwordInputGroupBox.Name = "passwordInputGroupBox";
            passwordInputGroupBox.Size = new Size(332, 52);
            passwordInputGroupBox.TabIndex = 0;
            passwordInputGroupBox.TabStop = false;
            passwordInputGroupBox.Text = "Enter your password here:";
            // 
            // passwordTextBox
            // 
            passwordTextBox.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            passwordTextBox.Location = new Point(6, 22);
            passwordTextBox.Name = "passwordTextBox";
            passwordTextBox.PasswordChar = '*';
            passwordTextBox.Size = new Size(320, 23);
            passwordTextBox.TabIndex = 1;
            // 
            // okButton
            // 
            okButton.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            okButton.Location = new Point(188, 74);
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
            cancelButton.Location = new Point(269, 74);
            cancelButton.Name = "cancelButton";
            cancelButton.Size = new Size(75, 23);
            cancelButton.TabIndex = 2;
            cancelButton.Text = "Cancel";
            cancelButton.UseVisualStyleBackColor = true;
            cancelButton.Click += cancelButton_Click;
            // 
            // PasswordForm
            // 
            AcceptButton = okButton;
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            CancelButton = cancelButton;
            ClientSize = new Size(356, 109);
            Controls.Add(cancelButton);
            Controls.Add(okButton);
            Controls.Add(passwordInputGroupBox);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            Name = "PasswordForm";
            ShowIcon = false;
            StartPosition = FormStartPosition.CenterParent;
            Text = "Enter Password";
            passwordInputGroupBox.ResumeLayout(false);
            passwordInputGroupBox.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private GroupBox passwordInputGroupBox;
        private TextBox passwordTextBox;
        private Button okButton;
        private Button cancelButton;
    }
}