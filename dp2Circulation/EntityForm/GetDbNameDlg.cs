using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

using DigitalPlatform;

namespace dp2Circulation
{
    /// <summary>
    /// ѡ�����ݿ����ĶԻ���
    /// </summary>
    internal partial class GetDbNameDlg : Form
    {
        string m_strDbType = "biblio";    // biblio / reader

        const int WM_AUTO_CLOSE = API.WM_USER + 200;
        public bool AutoClose = false;  // �Ի���ڴ򿪺������ر�?

        /// <summary>
        /// ��ܴ���
        /// </summary>
        public MainForm MainForm = null;

        public bool EnableNotAsk = false;

        public GetDbNameDlg()
        {
            InitializeComponent();
        }

        public string DbType
        {
            get
            {
                return this.m_strDbType;
            }
            set
            {
                this.m_strDbType = value;

                this.Text = "ָ��" + this.GetTypeName() + "����";
                this.label_dbNameList.Text = this.GetTypeName() + "�����б�(&L)";
                this.label_dbName.Text = this.GetTypeName() + "����(&N)";
            }
        }

        private void GetDbNameDlg_Load(object sender, EventArgs e)
        {
            // ������ݿ����б�
            if (this.m_strDbType == "biblio")
            {
                if (this.MainForm != null
                    && this.MainForm.BiblioDbProperties != null)
                {
                    for (int i = 0; i < this.MainForm.BiblioDbProperties.Count; i++)
                    {
                        this.listBox_biblioDbNames.Items.Add(this.MainForm.BiblioDbProperties[i].DbName);
                    }
                }
            }
            else if (this.m_strDbType == "reader")
            {
                if (this.MainForm != null
                    && this.MainForm.ReaderDbNames != null)
                {
                    for (int i = 0; i < this.MainForm.ReaderDbNames.Length; i++)
                    {
                        this.listBox_biblioDbNames.Items.Add(this.MainForm.ReaderDbNames[i]);
                    }
                }
            }


            // ѡ����Ŀ
            if (String.IsNullOrEmpty(this.DbName) == false)
                this.listBox_biblioDbNames.SelectedItem = this.DbName;

            if (this.EnableNotAsk == true)
                this.checkBox_notAsk.Enabled = true;

            if (this.AutoClose == true)
                API.PostMessage(this.Handle, WM_AUTO_CLOSE, 0, 0);
        }

        public string DbName
        {
            get
            {
                return this.textBox_dbName.Text;
            }
            set
            {
                this.textBox_dbName.Text = value;
            }
        }

        /// <summary>
        /// ȱʡ���ڹ���
        /// </summary>
        /// <param name="m">��Ϣ</param>
        protected override void DefWndProc(ref Message m)
        {
            switch (m.Msg)
            {
                case WM_AUTO_CLOSE:
                    this.button_OK_Click(this, null);
                    return;
            }
            base.DefWndProc(ref m);
        }

        string GetTypeName()
        {
            if (this.m_strDbType == "biblio")
                return "��Ŀ";
            if (this.m_strDbType == "reader")
                return "����";

            return this.m_strDbType;
        }

        private void button_OK_Click(object sender, EventArgs e)
        {
            if (this.textBox_dbName.Text == "")
            {
                MessageBox.Show(this, "��ָ��"+this.GetTypeName()+"����");
                return;
            }
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void button_Cancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();

        }

        private void listBox_dbNames_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.textBox_dbName.Text = (string)this.listBox_biblioDbNames.SelectedItem;
        }

        private void listBox_dbNames_DoubleClick(object sender, EventArgs e)
        {
            button_OK_Click(null, null);
        }

        public bool NotAsk
        {
            get
            {
                return this.checkBox_notAsk.Checked;
            }
            set
            {
                this.checkBox_notAsk.Checked = value;
            }
        }
    }
}