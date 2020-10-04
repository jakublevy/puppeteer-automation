using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Frontend.UserControls
{
    public partial class ThumbnailUserControl : UserControl
    {
        private Thumbnail thumbnail;
        public ThumbnailUserControl()
        {
            InitializeComponent();
        }

        public void BindThumbnail(Thumbnail t)
        {
            thumbnail = t;
            nameTextBox.Text = t.Name;
            websitesListBox.DataSource = t.Websites.ToList();
            createdLabel.Text = t.Created.ToString("dd.MM.yy HH:mm:ss");
            updatedLabel.Text = t.Updated.ToString("dd.MM.yy HH:mm:ss");
        }

        private void nameTextBox_TextChanged(object sender, EventArgs e)
        {
            thumbnail.Name = nameTextBox.Text;
            ThumbnailManager.SaveThumbnail(thumbnail);
            
        }

        private void editButton_Click(object sender, EventArgs e)
        {
            ((MainForm)Parent?.Parent).SwitchToEditMode(thumbnail);
        }

        private void deleteButton_Click(object sender, EventArgs e)
        {
            if (File.Exists($"recordings/{thumbnail.Id}.json"))
                File.Delete($"recordings/{thumbnail.Id}.json");
            
            ThumbnailManager.RemoveThumbnail(thumbnail);

            Parent.Controls.Remove(this);
        }
    }
}
