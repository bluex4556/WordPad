using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Text;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WordPad
{
    public partial class Form1 : Form
    {
        string Path { get; set; }
        public Form1()
        {
            Path = "";
            InitializeComponent();
            LoadFonts();
            LoadSizes();
        }
        private void LoadFonts()
        {
            var fontsCollection = new InstalledFontCollection();
            var ff = fontsCollection.Families;
            foreach (var item in ff)
            {
                fontSelectorComboBox.Items.Add(item.Name);
            }
            fontSelectorComboBox.Items.Add("");
            fontSelectorComboBox.SelectedItem = richBody.Font.Name;
        }

        private void LoadSizes()
        {
            int[] defaultSizes = {8,9,10,11,12,14,16,18,20,22,24,26,28,36,48,72};
            foreach (int size in defaultSizes)
            {
                fontSizeComboBox.Items.Add(size);
            }
            fontSizeComboBox.SelectedIndex = 3;
        }

        private void changeFont()
        {
            if(!float.TryParse(fontSizeComboBox.Text,out float size))
            {
                size = 11;
                fontSizeComboBox.Text = "11";
            }
            Font f = new Font((String)fontSelectorComboBox.SelectedItem,size,GetFontStyle());
            richBody.SelectionFont = f;
            
        }

        private FontStyle GetFontStyle()
        {
            FontStyle fontStyle = FontStyle.Regular;
            if (boldButton.Checked)
                fontStyle = fontStyle | FontStyle.Bold;
            if (italicButton.Checked)
                fontStyle = fontStyle | FontStyle.Italic;
            if (underlineButton.Checked)
                fontStyle = fontStyle | FontStyle.Underline;
            return fontStyle;
        }

        private DialogResult UnsavedDocumentWarning()
        {
            if (richBody.Text.Trim().Length > 0)
            {
                return MessageBox.Show("Save document?", "Wordpad", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
            }
            return DialogResult.No;
        }

        private void newFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DialogResult dr = UnsavedDocumentWarning();
            if (dr == DialogResult.Yes)
            {
                Save();
            }
            if(dr!=DialogResult.Cancel)
            {
                Path = "";
                Name = "Document";
                richBody.Clear();
            }
        }


        private void Save()
        {
            if (Path.Length ==0)
            {
                SaveFileDialog sd = new SaveFileDialog();
                sd.Filter = "Rich Text Files|*.rtf";
                if (sd.ShowDialog() == DialogResult.OK)
                {
                    Path = sd.FileName;
                }
            }
            richBody.SaveFile(Path);
            Text = Path;
        }

        private void openFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog od = new OpenFileDialog();
            od.Filter = "Rich Text Field|*.rtf";
            if (od.ShowDialog() == DialogResult.OK)
            {
                Path = od.FileName;
                richBody.LoadFile(Path);
                Text = Path;
            }
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Save();
        }

        private void saveAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Path = "";
            Save();
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DialogResult dr = UnsavedDocumentWarning();
            if (dr == DialogResult.Yes)
                Save();
            if(dr != DialogResult.Cancel)
                Application.Exit();
        }

        private void toolStripPrintButton_Click(object sender, EventArgs e)
        {
            PrintDialog pd = new PrintDialog();
            pd.ShowDialog();
        }



        private void fontSelectorComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            richBody.Focus();
            changeFont();
        }

        private void fontSizeComboBox_TextChanged(object sender, EventArgs e)
        {
            if (float.TryParse(fontSizeComboBox.Text, out float size))
            { 
                changeFont();
            }
            else
            {
                fontSizeComboBox.Text = "11";
            }
        }

        private void richBody_SelectionChanged(object sender, EventArgs e)
        {
            var fontname = richBody.SelectionFont?.Name;
            if (fontname != null)
            {
                fontSelectorComboBox.SelectedItem = fontname;
                fontSizeComboBox.Text = richBody.SelectionFont.Size.ToString();
                boldButton.Checked = richBody.SelectionFont.Bold;
                italicButton.Checked = richBody.SelectionFont.Italic;
                underlineButton.Checked = richBody.SelectionFont.Underline;
                HorizontalAlignment alignment = richBody.SelectionAlignment;
                leftAlignButton.Checked = (alignment == HorizontalAlignment.Left);
                centerAlignButton.Checked = (alignment == HorizontalAlignment.Center);
                rightAlignButton.Checked = (alignment == HorizontalAlignment.Right);

            }
        }

        private void cutToolStripButton_Click(object sender, EventArgs e)
        {
            richBody.Cut();
        }

        private void copyToolStripButton_Click(object sender, EventArgs e)
        {
            richBody.Copy();
        }

        private void pasteToolStripButton_Click(object sender, EventArgs e)
        {
            richBody.Paste();
        }

        private void boldButton_Click(object sender, EventArgs e)
        {
            boldButton.Checked = !boldButton.Checked;
            changeFont();
        }

        private void italicButton_Click(object sender, EventArgs e)
        {
            italicButton.Checked = !italicButton.Checked;
            changeFont();
        }

        private void underlineButton_Click(object sender, EventArgs e)
        {
            underlineButton.Checked = !underlineButton.Checked;
            changeFont();
        }

        private void fontToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FontDialog fd = new FontDialog();
            if (fd.ShowDialog() == DialogResult.OK)
            {
                richBody.SelectionFont = fd.Font;
                fontSelectorComboBox.SelectedItem = fd.Font.Name;
                fontSizeComboBox.SelectedText = fd.Font.Size.ToString();
                boldButton.Checked = fd.Font.Bold;
                italicButton.Checked = fd.Font.Italic;
                underlineButton.Checked = fd.Font.Underline;

            }
        }

        private void colorToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ColorDialog cd = new ColorDialog();

            if (cd.ShowDialog() == DialogResult.OK)
            {
                richBody.SelectionColor =  cd.Color;
            }
        }

        private void leftAlignButton_Click(object sender, EventArgs e)
        {
            leftAlignButton.Checked = true;
            centerAlignButton.Checked = false;
            rightAlignButton.Checked = false;
            richBody.SelectionAlignment = HorizontalAlignment.Left;
        }

        private void centerAlignButton_Click(object sender, EventArgs e)
        {
            leftAlignButton.Checked = false;
            centerAlignButton.Checked = true;
            rightAlignButton.Checked = false;
            richBody.SelectionAlignment = HorizontalAlignment.Center;
        }

        private void rightAlignButton_Click(object sender, EventArgs e)
        {
            leftAlignButton.Checked = false;
            centerAlignButton.Checked = false;
            rightAlignButton.Checked = true;
            richBody.SelectionAlignment = HorizontalAlignment.Right;
        }
    }
}
