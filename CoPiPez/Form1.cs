using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace CoPiPez {
    public partial class Form1 : Form {
        public Form1() {
            InitializeComponent();
            dataGridView1.Rows.Clear();
            dataGridView1.Rows.Add("右クリックでペースト");
            dataGridView1.Rows.Add("ファイルドロップも可");
        }

        private void dataGridView1_DragDrop( object sender, DragEventArgs e ) {
            string[] fileName = (string[])e.Data.GetData(DataFormats.FileDrop, false);
            if ( 0 < fileName.Length ) {
                try {
                    System.IO.StreamReader sr = new System.IO.StreamReader(fileName[0], Encoding.GetEncoding("Shift_JIS"));
                    string buffer = sr.ReadToEnd();
                    sr.Close();
                    dataGridView1_RowsFill( buffer);
                } catch ( System.IO.IOException ex) {
                    MessageBox.Show(ex.Message);
                }
            }
        }
        public delegate string Normalizing( string buffer);

        private void contextMenuStrip1_Click( Normalizing n ) {
            IDataObject src = Clipboard.GetDataObject();
            if ( src != null ) {
                if ( src.GetDataPresent(DataFormats.Text) ) {
                    dataGridView1_RowsFill( n((String)src.GetData(DataFormats.Text)));
                }
            }
        }

        private void dataGridView1_RowsFill( string buffer) {
            dataGridView1.Rows.Clear();
            foreach ( var item in buffer.Replace("\r\n", "\n").Split(new char[] { '\n' }) ) {
                if ( 0 < item.Length) dataGridView1.Rows.Add(item);
            }
        }

        private void dataGridView1_DragEnter( object sender, DragEventArgs e ) {
            if ( e.Data.GetDataPresent(DataFormats.FileDrop) ) {
                e.Effect = DragDropEffects.Copy;
            }
        }

        private void button1_Click( object sender, EventArgs e ) {
            moveNext();
        }

        private void dataGridView1_CellMouseClick( object sender, DataGridViewCellMouseEventArgs e ) {
        }

        private void moveNext() {
            var i = dataGridView1.CurrentRow.Index;
            if ( i < dataGridView1.Rows.Count) {
                Clipboard.SetDataObject(dataGridView1.CurrentRow.Cells[0].Value, true);
                if ( ++i < dataGridView1.Rows.Count ) {
                    dataGridView1.Rows[i].Cells[0].Selected = true;
                } else System.Media.SystemSounds.Beep.Play();
            }
        }

        private void button1_MouseLeave( object sender, EventArgs e ) {
            if ( checkBox1.Checked == true ) moveNext();
        }

        private void MenuItemPaste_LF_Click( object sender, EventArgs e ) {
            contextMenuStrip1_Click( s => s);
        }

        private void MenuItemPaste_Comma_Click( object sender, EventArgs e ) {
            contextMenuStrip1_Click( s => s.Replace(",", "\n"));
        }
    }
}
