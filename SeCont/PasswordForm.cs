using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SeCont
{
    public partial class PasswordForm : Form
    {
        private HashAlgorithm _hashAlgorithm = SHA256.Create();
        public bool canceled = true;

        private string Password { get => passwordTextBox.Text; }
        public byte[] PasswordHash { get => GetPasswordHash(); }

        public PasswordForm()
        {
            InitializeComponent();
        }

        public PasswordForm(HashAlgorithm hashAlgorithm) : this()
        {
            _hashAlgorithm = hashAlgorithm;
        }

        private byte[] GetPasswordHash()
        {
            return _hashAlgorithm.ComputeHash(Encoding.UTF8.GetBytes(Password));
        }

        private void okButton_Click(object sender, EventArgs e)
        {
            canceled = false;
            Close();
        }

        private void cancelButton_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
