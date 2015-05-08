using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;

using DigitalPlatform.Xml;
using DigitalPlatform.GUI;

namespace dp2Circulation
{
    internal partial class BindingOptionDialog : Form
    {
        bool m_bCellContentsChanged = false;
        bool m_bGroupContentsChanged = false;

        public ApplicationInfo AppInfo = null;

        public string[] DefaultTextLineNames = null;
        public string[] DefaultGroupTextLineNames = null;

        public BindingOptionDialog()
        {
            InitializeComponent();
        }

        private void BindingOptionDialog_Load(object sender, EventArgs e)
        {
            Debug.Assert(this.AppInfo != null, "");

            // װ�����κ�
            this.textBox_general_bindingBatchNo.Text = this.AppInfo.GetString(
                "binding_form",
                "binding_batchno",
                "");

            // �������κ�
            this.textBox_general_acceptBatchNo.Text = this.AppInfo.GetString(
   "binding_form",
   "accept_batchno",
   "");

            // �༭�����ַ�ʽ
            this.comboBox_ui_splitterDirection.Text = this.AppInfo.GetString(
                "binding_form",
                "splitter_direction",
                "ˮƽ");

            // ��ʾ������Ϣ����ֵ
            this.checkBox_ui_displayOrderInfoXY.Checked = this.AppInfo.GetBoolean(
                "binding_form",
                "display_orderinfoxy",
                false);

            // ��ʾ�ֹ��ⶩ����
            this.checkBox_ui_displayLockedOrderGroup.Checked = this.AppInfo.GetBoolean(
                "binding_form",
                "display_lockedOrderGroup",
                true);

            // �����������
            {
                string strLinesCfg = this.AppInfo.GetString(
        "binding_form",
        "cell_lines_cfg",
        "");
                if (string.IsNullOrEmpty(strLinesCfg) == true
                    && this.DefaultTextLineNames != null)
                {
                    strLinesCfg = string.Join(",", this.DefaultTextLineNames);
                }

                FillCellContentsList(strLinesCfg);
            }

            // �����������
            {
                string strLinesCfg = this.AppInfo.GetString(
    "binding_form",
    "group_lines_cfg",
    "");
                if (string.IsNullOrEmpty(strLinesCfg) == true
                    && this.DefaultGroupTextLineNames != null)
                {
                    strLinesCfg = string.Join(",", this.DefaultGroupTextLineNames);
                }

                FillGroupContentsList(strLinesCfg);
            }
        }

        private void button_OK_Click(object sender, EventArgs e)
        {
            Debug.Assert(this.AppInfo != null, "");

            this.AppInfo.SetString(
                "binding_form",
                "binding_batchno",
                this.textBox_general_bindingBatchNo.Text);

            this.AppInfo.SetString(
               "binding_form",
               "accept_batchno",
               this.textBox_general_acceptBatchNo.Text);

            this.AppInfo.SetString(
                "binding_form",
                "splitter_direction",
                this.comboBox_ui_splitterDirection.Text);

            // ��ʾ������Ϣ����ֵ
            this.AppInfo.SetBoolean(
                "binding_form",
                "display_orderinfoxy",
                this.checkBox_ui_displayOrderInfoXY.Checked);

            // ��ʾ�ֹ��ⶩ����Ϣ
            this.AppInfo.SetBoolean(
                "binding_form",
                "display_lockedOrderGroup",
                this.checkBox_ui_displayLockedOrderGroup.Checked);

            // �����������
            if (this.m_bCellContentsChanged == true)
            {
                string strLinesCfg = GetCellContentList();
                string strDefault = string.Join(",", this.DefaultTextLineNames);
                if (strLinesCfg == strDefault)
                    strLinesCfg = "";

                this.AppInfo.SetString(
         "binding_form",
         "cell_lines_cfg",
         strLinesCfg);
            }

            // �����������
            if (this.m_bGroupContentsChanged == true)
            {
                string strLinesCfg = GetGroupContentList();
                string strDefault = string.Join(",", this.DefaultGroupTextLineNames);
                if (strLinesCfg == strDefault)
                    strLinesCfg = "";

                this.AppInfo.SetString(
         "binding_form",
         "group_lines_cfg",
         strLinesCfg);
            }

            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void button_Cancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        void FillCellContentsList(string strCfgText)
        {
            this.listView_cellContents_lines.Items.Clear();

            string[] parts = strCfgText.Split(new char[] {','});

            for (int i = 0; i < parts.Length / 2; i++)
            {
                string strName = parts[i * 2];
                string strCaption = parts[i * 2 + 1];

                ListViewItem item = new ListViewItem();
                item.Text = strName;
                ListViewUtil.ChangeItemText(item, 1, strCaption);

                this.listView_cellContents_lines.Items.Add(item);
            }
        }

        void FillGroupContentsList(string strCfgText)
        {
            this.listView_groupContents_lines.Items.Clear();

            string[] parts = strCfgText.Split(new char[] { ',' });

            for (int i = 0; i < parts.Length / 2; i++)
            {
                string strName = parts[i * 2];
                string strCaption = parts[i * 2 + 1];

                ListViewItem item = new ListViewItem();
                item.Text = strName;
                ListViewUtil.ChangeItemText(item, 1, strCaption);

                this.listView_groupContents_lines.Items.Add(item);
            }
        }

        string GetCellContentList()
        {
            string strResult = "";
            for (int i = 0; i < this.listView_cellContents_lines.Items.Count; i++)
            {
                ListViewItem item = this.listView_cellContents_lines.Items[i];

                if (i > 0)
                    strResult += ",";

                strResult += item.Text + ",";
                strResult += ListViewUtil.GetItemText(item, 1);
            }

            return strResult;
        }

        string GetGroupContentList()
        {
            string strResult = "";
            for (int i = 0; i < this.listView_groupContents_lines.Items.Count; i++)
            {
                ListViewItem item = this.listView_groupContents_lines.Items[i];

                if (i > 0)
                    strResult += ",";

                strResult += item.Text + ",";
                strResult += ListViewUtil.GetItemText(item, 1);
            }

            return strResult;
        }

        private void button_cellContents_new_Click(object sender, EventArgs e)
        {
            CellLineDialog dlg = new CellLineDialog();
            MainForm.SetControlFont(dlg, this.Font, false);

            dlg.StartPosition = FormStartPosition.CenterScreen;
            dlg.ShowDialog(this);

            if (dlg.DialogResult != DialogResult.OK)
                return;

            // ����?
            // ���Ʋ���
            ListViewItem dup = ListViewUtil.FindItem(this.listView_cellContents_lines, dlg.FieldName, 0);
            if (dup != null)
            {
                // �ò������ܿ����Ѿ����ڵ���
                ListViewUtil.SelectLine(dup, true);
                dup.EnsureVisible();

                DialogResult result = MessageBox.Show(this,
                    "��ǰ�Ѿ�������Ϊ '" + dlg.FieldName + "' �������С���������?",
                    "BindingOptionDialog",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question,
                    MessageBoxDefaultButton.Button2);
                if (result == DialogResult.No)
                    return;
            }



            ListViewItem item = new ListViewItem();
            item.Text = dlg.FieldName;
            ListViewUtil.ChangeItemText(item, 1, dlg.Caption);

            this.listView_cellContents_lines.Items.Add(item);
            ListViewUtil.SelectLine(item, true);
            item.EnsureVisible();

            listView_cellContents_lines_SelectedIndexChanged(sender, null);

            this.m_bCellContentsChanged = true;
        }

        private void listView_cellContents_lines_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.listView_cellContents_lines.SelectedIndices.Count == 0)
            {
                // û��ѡ������
                this.button_cellContents_delete.Enabled = false;
                this.button_cellContents_modify.Enabled = false;
                this.button_cellContents_moveDown.Enabled = false;
                this.button_cellContents_moveUp.Enabled = false;
                this.button_cellContents_new.Enabled = true;
            }
            else
            {
                // ��ѡ������
                this.button_cellContents_delete.Enabled = true;
                this.button_cellContents_modify.Enabled = true;
                if (this.listView_cellContents_lines.SelectedIndices[0] >= this.listView_cellContents_lines.Items.Count - 1)
                    this.button_cellContents_moveDown.Enabled = false;
                else
                    this.button_cellContents_moveDown.Enabled = true;

                if (this.listView_cellContents_lines.SelectedIndices[0] == 0)
                    this.button_cellContents_moveUp.Enabled = false;
                else
                    this.button_cellContents_moveUp.Enabled = true;

                this.button_cellContents_new.Enabled = true;

            }
        }

        private void button_cellContents_delete_Click(object sender, EventArgs e)
        {
            if (this.listView_cellContents_lines.SelectedItems.Count == 0)
            {
                MessageBox.Show(this, "��δѡ��Ҫɾ��������");
                return;
            }

            DialogResult result = MessageBox.Show(this,
                "ȷʵҪɾ��ѡ���� " + this.listView_cellContents_lines.SelectedItems.Count.ToString() + " ������? ",
                "BindingOptionDialog",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question,
                MessageBoxDefaultButton.Button2);
            if (result != DialogResult.Yes)
                return;


            while (this.listView_cellContents_lines.SelectedItems.Count > 0)
            {
                this.listView_cellContents_lines.Items.Remove(this.listView_cellContents_lines.SelectedItems[0]);
            }

            // ɾ������󣬵�ǰ��ѡ������������ƶ��Ŀ����Ի������ı�
            listView_cellContents_lines_SelectedIndexChanged(sender, null);

            this.m_bCellContentsChanged = true;
        }

        private void button_cellContents_modify_Click(object sender, EventArgs e)
        {
            if (this.listView_cellContents_lines.SelectedItems.Count == 0)
            {
                MessageBox.Show(this, "��δѡ��Ҫ�޸ĵ�����");
                return;
            }

            CellLineDialog dlg = new CellLineDialog();
            MainForm.SetControlFont(dlg, this.Font, false);

            dlg.FieldName = this.listView_cellContents_lines.SelectedItems[0].Text;
            dlg.Caption = this.listView_cellContents_lines.SelectedItems[0].SubItems[1].Text;
            dlg.StartPosition = FormStartPosition.CenterScreen;
            dlg.ShowDialog(this);

            if (dlg.DialogResult != DialogResult.OK)
                return;

            ListViewItem item = this.listView_cellContents_lines.SelectedItems[0];
            item.Text = dlg.FieldName;
            ListViewUtil.ChangeItemText(item, 1, dlg.Caption);

            this.m_bCellContentsChanged = true;
        }

        private void button_cellContents_moveUp_Click(object sender, EventArgs e)
        {
            if (this.listView_cellContents_lines.SelectedItems.Count == 0)
            {
                MessageBox.Show(this, "��δѡ��Ҫ�ƶ�������");
                return;
            }

            int nIndex = this.listView_cellContents_lines.SelectedIndices[0];

            if (nIndex == 0)
            {
                MessageBox.Show(this, "���ڶ���");
                return;
            }

            ListViewItem item = this.listView_cellContents_lines.SelectedItems[0];

            this.listView_cellContents_lines.Items.Remove(item);
            this.listView_cellContents_lines.Items.Insert(nIndex - 1, item);

            this.m_bCellContentsChanged = true;
        }

        private void button_cellContents_moveDown_Click(object sender, EventArgs e)
        {
            if (this.listView_cellContents_lines.SelectedItems.Count == 0)
            {
                MessageBox.Show(this, "��δѡ��Ҫ�ƶ�������");
                return;
            }

            int nIndex = this.listView_cellContents_lines.SelectedIndices[0];

            if (nIndex >= this.listView_cellContents_lines.Items.Count - 1)
            {
                MessageBox.Show(this, "���ڵײ�");
                return;
            }

            ListViewItem item = this.listView_cellContents_lines.SelectedItems[0];

            this.listView_cellContents_lines.Items.Remove(item);
            this.listView_cellContents_lines.Items.Insert(nIndex + 1, item);

            this.m_bCellContentsChanged = true;
        }

        private void listView_groupContents_lines_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.listView_groupContents_lines.SelectedIndices.Count == 0)
            {
                // û��ѡ������
                this.button_groupContents_delete.Enabled = false;
                this.button_groupContents_modify.Enabled = false;
                this.button_groupContents_moveDown.Enabled = false;
                this.button_groupContents_moveUp.Enabled = false;
                this.button_groupContents_new.Enabled = true;
            }
            else
            {
                // ��ѡ������
                this.button_groupContents_delete.Enabled = true;
                this.button_groupContents_modify.Enabled = true;
                if (this.listView_groupContents_lines.SelectedIndices[0] >= this.listView_cellContents_lines.Items.Count - 1)
                    this.button_groupContents_moveDown.Enabled = false;
                else
                    this.button_groupContents_moveDown.Enabled = true;

                if (this.listView_groupContents_lines.SelectedIndices[0] == 0)
                    this.button_groupContents_moveUp.Enabled = false;
                else
                    this.button_groupContents_moveUp.Enabled = true;

                this.button_groupContents_new.Enabled = true;
            }
        }

        private void button_groupContents_moveUp_Click(object sender, EventArgs e)
        {
            if (this.listView_groupContents_lines.SelectedItems.Count == 0)
            {
                MessageBox.Show(this, "��δѡ��Ҫ�ƶ�������");
                return;
            }

            int nIndex = this.listView_groupContents_lines.SelectedIndices[0];

            if (nIndex == 0)
            {
                MessageBox.Show(this, "���ڶ���");
                return;
            }

            ListViewItem item = this.listView_groupContents_lines.SelectedItems[0];

            this.listView_groupContents_lines.Items.Remove(item);
            this.listView_groupContents_lines.Items.Insert(nIndex - 1, item);

            this.m_bGroupContentsChanged = true;
        }

        private void button_groupContents_moveDown_Click(object sender, EventArgs e)
        {
            if (this.listView_groupContents_lines.SelectedItems.Count == 0)
            {
                MessageBox.Show(this, "��δѡ��Ҫ�ƶ�������");
                return;
            }

            int nIndex = this.listView_groupContents_lines.SelectedIndices[0];

            if (nIndex >= this.listView_groupContents_lines.Items.Count - 1)
            {
                MessageBox.Show(this, "���ڵײ�");
                return;
            }

            ListViewItem item = this.listView_groupContents_lines.SelectedItems[0];

            this.listView_groupContents_lines.Items.Remove(item);
            this.listView_groupContents_lines.Items.Insert(nIndex + 1, item);

            this.m_bGroupContentsChanged = true;
        }

        private void button_groupContents_new_Click(object sender, EventArgs e)
        {
            CellLineDialog dlg = new CellLineDialog();
            MainForm.SetControlFont(dlg, this.Font, false);

            dlg.FillGroupFieldNameTable();
            dlg.StartPosition = FormStartPosition.CenterScreen;
            dlg.ShowDialog(this);

            if (dlg.DialogResult != DialogResult.OK)
                return;

            // ����?
            // ���Ʋ���
            ListViewItem dup = ListViewUtil.FindItem(this.listView_groupContents_lines, dlg.FieldName, 0);
            if (dup != null)
            {
                // �ò������ܿ����Ѿ����ڵ���
                ListViewUtil.SelectLine(dup, true);
                dup.EnsureVisible();

                DialogResult result = MessageBox.Show(this,
                    "��ǰ�Ѿ�������Ϊ '" + dlg.FieldName + "' �������С���������?",
                    "BindingOptionDialog",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question,
                    MessageBoxDefaultButton.Button2);
                if (result == DialogResult.No)
                    return;
            }



            ListViewItem item = new ListViewItem();
            item.Text = dlg.FieldName;
            ListViewUtil.ChangeItemText(item, 1, dlg.Caption);

            this.listView_groupContents_lines.Items.Add(item);
            ListViewUtil.SelectLine(item, true);
            item.EnsureVisible();

            listView_groupContents_lines_SelectedIndexChanged(sender, null);

            this.m_bGroupContentsChanged = true;
        }

        private void button_groupContents_modify_Click(object sender, EventArgs e)
        {
            if (this.listView_groupContents_lines.SelectedItems.Count == 0)
            {
                MessageBox.Show(this, "��δѡ��Ҫ�޸ĵ�����");
                return;
            }

            CellLineDialog dlg = new CellLineDialog();
            MainForm.SetControlFont(dlg, this.Font, false);

            dlg.FillGroupFieldNameTable();
            dlg.FieldName = this.listView_groupContents_lines.SelectedItems[0].Text;
            dlg.Caption = this.listView_groupContents_lines.SelectedItems[0].SubItems[1].Text;
            dlg.StartPosition = FormStartPosition.CenterScreen;
            dlg.ShowDialog(this);

            if (dlg.DialogResult != DialogResult.OK)
                return;

            ListViewItem item = this.listView_groupContents_lines.SelectedItems[0];
            item.Text = dlg.FieldName;
            ListViewUtil.ChangeItemText(item, 1, dlg.Caption);

            this.m_bGroupContentsChanged = true;
        }

        private void button_groupContents_delete_Click(object sender, EventArgs e)
        {
            if (this.listView_groupContents_lines.SelectedItems.Count == 0)
            {
                MessageBox.Show(this, "��δѡ��Ҫɾ��������");
                return;
            }

            DialogResult result = MessageBox.Show(this,
                "ȷʵҪɾ��ѡ���� " + this.listView_groupContents_lines.SelectedItems.Count.ToString() + " ������? ",
                "BindingOptionDialog",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question,
                MessageBoxDefaultButton.Button2);
            if (result != DialogResult.Yes)
                return;


            while (this.listView_groupContents_lines.SelectedItems.Count > 0)
            {
                this.listView_groupContents_lines.Items.Remove(this.listView_groupContents_lines.SelectedItems[0]);
            }

            // ɾ������󣬵�ǰ��ѡ������������ƶ��Ŀ����Ի������ı�
            listView_groupContents_lines_SelectedIndexChanged(sender, null);

            this.m_bGroupContentsChanged = true;
        }
    }
}