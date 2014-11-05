using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Collections;

using Hitops.Data;
using Hitops.exception;
using Hitops;
using HitopsCommon;

namespace com.hitops.PLN.RSC.ResourcePlan
{
    public partial class frmWorkerEntry : Form
    {
        string _mID = CommFunc.gloSystemPrefix + "12415";
        private frmWorkerMng _Form = null;

        ArrayList arWorker = new ArrayList();
        string m_sCompFull = "";

        public frmWorkerEntry(frmWorkerMng sForm, string WrkID, string WrkNm, string WTeam, string WPaWd, string WAvail, string WEmply, string WAuthd, string WChief)
        {
            InitializeComponent();
            _Form = sForm;
            Program.SetTeam(cmbTeam, false, false);
            SetMandatory();
            SetData(WrkID, WrkNm, WTeam, WPaWd, WAvail, WEmply, WAuthd, WChief, "", "");

            tbxPwd.Enabled = false;
            tbxWrker.Enabled = false;
            tbxName.Enabled = false;
            cmbTeam.Enabled = true ;
        }

        public frmWorkerEntry(frmWorkerMng sForm, string WrkID, string WrkNm, string WTeam, string WPaWd, string WAvail, string WEmply, string WAuthd, string WChief,
            string sCompCod, string sCompDesc, string sSort, string sAllocVisible)
        {
            InitializeComponent();
            _Form = sForm;
            Program.SetTeam(cmbTeam, false, false);
            SetMandatory();
            SetData(WrkID, WrkNm, WTeam, WPaWd, WAvail, WEmply, WAuthd, WChief, sSort, sAllocVisible);

            tbxPwd.Enabled = false;
            tbxWrker.Enabled = false;
            tbxName.Enabled = false;
            cmbTeam.Enabled = true;
            cmbVisible.Enabled = true;
            m_sCompFull = sCompCod + ":" + sCompDesc;
        }

        public frmWorkerEntry(string WrkID, string WrkNm, string WTeam, string WPaWd, string WAvail, string WEmply, string WAuthd, string WChief,
            string sCompCod, string sCompDesc, string sSort, string sAllocVisible)
        {
            InitializeComponent();
            Program.SetTeam(cmbTeam, false, false);
            SetMandatory();
            SetData(WrkID, WrkNm, WTeam, WPaWd, WAvail, WEmply, WAuthd, WChief, sSort, sAllocVisible);

            tbxPwd.Enabled = false;
            tbxWrker.Enabled = false;
            tbxName.Enabled = false;
            cmbTeam.Enabled = true;
            cmbVisible.Enabled = true;
            m_sCompFull = sCompCod + ":" + sCompDesc;
        }

        public frmWorkerEntry(frmWorkerMng sForm)
        {
            InitializeComponent();
            _Form = sForm;

            SetMandatory();
        }

        private void SetData(string WrkID, string WrkNm, string WTeam, string WPaWd, string WAvail, string WEmply, string WAuthd, string WChief, string sSort, string sAllocVisible)
        {
            tbxName.Text = WrkNm;
            tbxWrker.Text = WrkID;
            cmbTeam.Text = WTeam;
            tbxPwd.Text = WPaWd;
            cmbAut.Text = WAuthd;
            cmbEmp.Text = WEmply;
            cmbChief.Text = WChief;
            cmbAvail.Text = WAvail;
            txtSort.Text = sSort;
            cmbVisible.Text = sAllocVisible;
        }

        private void SetMandatory()
        {
            arWorker.Add(tbxWrker);
            arWorker.Add(tbxPwd);
            arWorker.Add(tbxName);
            arWorker.Add(cmbTeam);
            arWorker.Add(cmbAut);
            arWorker.Add(cmbAvail);
            arWorker.Add(cmbChief);
            arWorker.Add(cmbEmp);
            arWorker.Add(cmbCompany);
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            CreateWorker();
        }        

        private void CreateWorker()
        {
            if (!CommFunc.ControlMandatoryItem(arWorker)) return;

            string sCompCod = "";
            ArrayList aList  = new ArrayList();
            Hashtable hTable = new Hashtable();

            try
            {
                if (cmbCompany.Text.Trim().Length > 0)
                {
                    string[] aComp = cmbCompany.Text.Split(':');
                    sCompCod = aComp[0];
                }
                hTable.Add("WORKER_ID", tbxWrker.Text);
                hTable.Add("WORKER_NAME", tbxName.Text);
                hTable.Add("TEAM", cmbTeam.Text);
                hTable.Add("WORKER_KND", cmbAvail.Text);
                hTable.Add("PASWRD", tbxPwd.Text);
                hTable.Add("EMPLOY_TYP", cmbEmp.Text);
                hTable.Add("CHIEF_TAG", cmbChief.Text);
                hTable.Add("AUTHORITY", cmbAut.Text);
                hTable.Add("COMP_COD", sCompCod);
                hTable.Add("SORT_SEQ", txtSort.Text);
                hTable.Add("ALLOC_VISIBLE", cmbVisible.Text);
                hTable.Add("INPUT_PSN", CommFunc.gloUserID);
                aList = (ArrayList)RequestHandler.Request(CommFunc.gloFrameworkServerName, "HITOPS3-PLN-RSC-S-GETWORKERINFO", _mID, hTable);

                if (((Hashtable)aList[0])["CNT"].ToString() == "0")
                {
                    //RequestHandler.Request(CommFunc.gloFrameworkServerName, "HITOPS3-PLN-RSC-S-CRTWORKERINFO", _mID, hTable);
                    RequestHandler.Request(CommFunc.gloFrameworkServerName, "HITOPS3-PLN-RSC-S-CRTWORKERINFO2", _mID, hTable);
                    if(_Form != null)
                        _Form.sLoadData();
                }
                else
                {
                    if (MessageBox.Show("동일한 기사정보가 존재합니다. 수정하시겠습니까?", "Update Confrim", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        //RequestHandler.Request(CommFunc.gloFrameworkServerName, "HITOPS3-PLN-RSC-S-UPDWORKERINFO", _mID, hTable);
                        RequestHandler.Request(CommFunc.gloFrameworkServerName, "HITOPS3-PLN-RSC-S-UPDWORKERINFO2", _mID, hTable);
                        if (_Form != null)
                            _Form.sLoadData();
                        this.Close();
                    }
                }
            }
            catch (HMMException ex)
            {
                MessageBox.Show(ex.Message1);
            }

        }        

        private void UpdateWorker()
        {
            ArrayList aList  = new ArrayList();
            Hashtable hTable = new Hashtable();

            hTable.Add("WORKER_ID",  tbxWrker.Text);
            hTable.Add("WORKER_NAME",tbxName.Text);
            hTable.Add("TEAM",       cmbTeam.Text);
            hTable.Add("WORKER_KND", cmbAvail.Text);
            hTable.Add("PASWRD",     tbxPwd.Text);
            hTable.Add("EMPLOY_TYP", cmbEmp.Text);
            hTable.Add("CHIEF_TAG",  cmbChief.Text);
            hTable.Add("AUTHORITY",  cmbAut.Text);
            hTable.Add("UPDATE_PSN", CommFunc.gloUserID);

            try
            {
                aList = (ArrayList)RequestHandler.Request(CommFunc.gloFrameworkServerName, "HITOPS3-PLN-RSC-S-GETWORKERINFO", _mID, hTable);

                if (((Hashtable)aList[0])["CNT"].ToString() != "0")
                {
                    RequestHandler.Request(CommFunc.gloFrameworkServerName, "HITOPS3-PLN-RSC-S-UPDWORKERINFO", _mID, hTable);
                }
                else
                {
                    MessageBox.Show("Update 가능한 항목이 없습니다", "Update Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (HMMException ex)
            {
                MessageBox.Show(ex.Message1);
            }
        }

        private void frmWorkerEntry_Load(object sender, EventArgs e)
        {
            this.Icon = CommFunc.GetMainIcon(CommFunc.MainIcon.Resource);
            this.AutoScaleMode = AutoScaleMode.None;
            Program.SetTeam(cmbTeam, false, false);
            SetCompany();
            this.WindowState = FormWindowState.Normal;
        }

        private void SetCompany()
        {
            try
            {
                ArrayList aList = (ArrayList)RequestHandler.Request(CommFunc.gloFrameworkServerName, "HITOPS3-PLN-RSC-S-LSTWORKCOMPANY", _mID);

                cmbCompany.Items.Clear();
                foreach (Hashtable hComp in aList)
                {
                    cmbCompany.Items.Add(hComp["COMP_COD"] + ":" + hComp["COMP_DESC"]);
                }

                for(int i=0; i<cmbCompany.Items.Count; i++)
                {
                    if (cmbCompany.Items[i].ToString() == m_sCompFull)
                    {
                        cmbCompany.SelectedIndex = i;
                        break;
                    }
                }
            }
            catch (HMMException ex)
            {
                MessageBox.Show(ex.Message1);
            }
        }

        private void tbbExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnView_Click(object sender, EventArgs e)
        {
            btnView.Visible = false;
            btnHide.Visible = true;
            CommFunc.TextViewTrueMode(tbrToolBar);
        }

        private void btnHide_Click(object sender, EventArgs e)
        {
            btnView.Visible = true;
            btnHide.Visible = false;
            CommFunc.TextViewFalseMode(tbrToolBar);
        }

        private void txtSort_KeyPress(object sender, KeyPressEventArgs e)
        {
            try
            {
                switch (e.KeyChar)
                {
                    case (char)Keys.Back:
                    case (char)Keys.Enter:
                    case (char)Keys.Delete:
                        break;
                    default:
                        if (e.KeyChar < 48 || e.KeyChar > 57)
                        {
                            MessageBox.Show("Insert into numeric data type.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                            e.Handled = true;
                            this.txtSort.Focus();
                        }
                        break;
                }
            }
            catch (HMMException ex)
            {
                CommFunc.ShowExceptionBox(ex);
            }
        }
    }
}