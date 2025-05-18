using SeContCoreLib;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SeCont
{
    public partial class EncryptionAlgorithmSelectionForm : Form
    {
        private EncryptionAlgorithm? CurrentSelectedEncryptionAlgorithm
        {
            get
            {
                if (encryptionComboBox.SelectedItem == null)
                    return null;
                var encryptionItem = (EncryptionAlgorithmSelectionItem)encryptionComboBox.SelectedItem;
                return encryptionItem.encryptionAlgorithm;
            }
        }
        public EncryptionAlgorithm? SelectedEncryptionAlgorithm
        {
            get
            {
                if (canceled)
                    return null;
                return CurrentSelectedEncryptionAlgorithm;
            }
        }

        private bool canceled = true;

        public EncryptionAlgorithmSelectionForm()
        {
            InitializeComponent();
            encryptionComboBox.DataSource = GetEncryptionItems();
            encryptionComboBox.SelectedIndex = 0;
        }

        private EncryptionAlgorithmSelectionItem[] GetEncryptionItems()
        {
            List<EncryptionAlgorithmSelectionItem> encryptionItems = new();
            encryptionItems.Add(new EncryptionAlgorithmSelectionItem(null));
            foreach (var encryptionAlgorithm in EncryptionAlgorithmUtils.EncryptionAlgorithms)
            {
                encryptionItems.Add(new EncryptionAlgorithmSelectionItem(encryptionAlgorithm));
            }
            return encryptionItems.ToArray();
        }

        private class EncryptionAlgorithmSelectionItem
        {
            public readonly EncryptionAlgorithm? encryptionAlgorithm;

            public EncryptionAlgorithmSelectionItem(EncryptionAlgorithmBuilder? encryptionAlgorithmBuilder)
            {
                if (encryptionAlgorithmBuilder == null)
                {
                    encryptionAlgorithm = null;
                    return;
                }

                encryptionAlgorithm = encryptionAlgorithmBuilder.CreateEncryptionAlgorithm();
            }

            public override string ToString()
            {
                if (encryptionAlgorithm == null)
                    return "None";
                return encryptionAlgorithm.Name;
            }
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
