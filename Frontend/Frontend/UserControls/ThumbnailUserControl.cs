using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Frontend.UserControls
{
    public partial class ThumbnailUserControl : UserControl
    {
        public ThumbnailUserControl()
        {
            InitializeComponent();
        }

        public void BindThumbnail(Thumbnail t)
        {
            nameTextBox.Text = t.Name;
            websitesListBox.DataSource = t.Websites;
            createdLabel.Text = t.Created.ToString("dd.MM.yy HH:mm:ss");
            updatedLabel.Text = t.Updated.ToString("dd.MM.yy HH:mm:ss");
        }

        private void nameTextBox_TextChanged(object sender, EventArgs e)
        {
            //Save updated thumbnail
        }

        private void editButton_Click(object sender, EventArgs e)
        {

        }

        private void deleteButton_Click(object sender, EventArgs e)
        {

        }
    }
}
