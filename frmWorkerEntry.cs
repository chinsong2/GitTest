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

        public frmWorkerEntry(
            frmWorkerMng sForm, string WrkID, string WrkNm, string WTeam, string WPaWd, string WEmply, string WAuthd, 
            string MainEQP, string SUB1_EQP, string SUB2_EQP, string MainEQPClass, string SUB1_EQP_Class, string SUB2_EQP_CLASS)
        {
            InitializeComponent();
            _Form = sForm;
            Program.SetTeam(cmbTeam, false, false);
            SetMandatory();
            SetData(WrkID, WrkNm, WTeam, WPaWd, WEmply, WAuthd, MainEQP, SUB1_EQP, SUB2_EQP, MainEQPClass, SUB1_EQP_Class, SUB2_EQP_CLASS);

            tbxWrker.Enabled = false;
            tbxName.Enabled = false;
            cmbTeam.Enabled = true ;
        }

        public frmWorkerEntry(frmWorkerMng sForm)
        {
            InitializeComponent();
            _Form = sForm;

            SetMandatory();
        }

        private void SetData(string WrkID, string WrkNm, string WTeam, string WPaWd, string WEmply, string WAuthd,
            string MainEQP, string SUB1_EQP, string SUB2_EQP, string MainEQPClass, string SUB1_EQP_Class, string SUB2_EQP_CLASS)
        {
            tbxName.Text = WrkNm;
            tbxWrker.Text = WrkID;
            cmbTeam.Text = WTeam;
            tbxPwd.Text = WPaWd;
            cmbAut.SelectedIndex = int.Parse(WAuthd);

            for (int idxItm = 0; idxItm < cmbEmpType.Items.Count; idxItm++)
            {
                String sItem = (String)cmbEmpType.Items[idxItm];
                if (sItem.Length > 2 && sItem.Substring(0, 2).Trim() == WEmply)
                {
                    cmbEmpType.SelectedIndex = idxItm;
                }
            }

            cmbEqp_Main.Text = MainEQP;
            cmbEqp_Sub1.Text = SUB1_EQP;
            cmbEqp_Sub2.Text = SUB2_EQP;

            cmbCls_Main.Text = MainEQPClass;
            cmbCls_Sub1.Text = SUB1_EQP_Class;
            cmbCls_Sub2.Text = SUB2_EQP_CLASS;
        }

        private void SetMandatory()
        {
            arWorker.Add(tbxWrker);
            arWorker.Add(tbxPwd);
            arWorker.Add(tbxName);
            arWorker.Add(cmbTeam);
            arWorker.Add(cmbAut);
            arWorker.Add(cmbEmpType);
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
                hTable.Add("WORKER_ID", tbxWrker.Text);
                hTable.Add("WORKER_NAME", tbxName.Text);
                hTable.Add("TEAM", cmbTeam.Text);
                hTable.Add("WORKER_KND", "Y"); //앞으로 사용안함. 기본 "Y" 로 넣어줌. Available 의미로 사용되었음
                hTable.Add("PASWRD", tbxPwd.Text);
                hTable.Add("EMPLOY_TYP", cmbEmpType.Text.Substring(0,1));
                hTable.Add("CHIEF_TAG", "Y"); //앞으로 사용안함. 기본 "Y" 로 넣어줌.
                hTable.Add("AUTHORITY", cmbAut.SelectedIndex.ToString());
                hTable.Add("COMP_COD", "-");
                hTable.Add("SORT_SEQ", "0");  //앞으로 사용안함. 기본 "0" 로 넣어줌.
                hTable.Add("ALLOC_VISIBLE", "Y");  //앞으로 사용안함. 기본 "Y" 로 넣어줌.
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
            hTable.Add("WORKER_KND", "Y"); //앞으로 사용안함. 기본 "Y" 로 넣어줌. Available 의미로 사용되었음
            hTable.Add("PASWRD",     tbxPwd.Text);
            hTable.Add("EMPLOY_TYP", cmbEmpType.Text.Substring(0, 1));
            hTable.Add("CHIEF_TAG",  "Y");
            hTable.Add("AUTHORITY", cmbAut.SelectedIndex.ToString());
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
            Program.SetWorkerEqpTyp(cmbEqp_Main, false, false);
            Program.SetWorkerEqpTyp(cmbEqp_Sub1, false, false);
            Program.SetWorkerEqpTyp(cmbEqp_Sub2, false, false);

            this.WindowState = FormWindowState.Normal;
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

    }
}