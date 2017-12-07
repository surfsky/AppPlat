using FineUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace App.Controls
{
    /*
    TwinTriggerBox 增强版。实现以下逻辑：
        <f:TwinTriggerBox ID = "ttbSearchMessage" runat="server" ShowLabel="false" EmptyText="查找用户名"
            Trigger1Icon="Clear" Trigger2Icon="Search" ShowTrigger1="false" 
            OnTrigger2Click="ttbSearchMessage_Trigger2Click"
            OnTrigger1Click="ttbSearchMessage_Trigger1Click"
        />
        protected void ttbSearchMessage_Trigger1Click(object sender, EventArgs e)
        {
            ttbSearchMessage.Text = String.Empty;
            ttbSearchMessage.ShowTrigger1 = false;
            BindGrid();
        }
        protected void ttbSearchMessage_Trigger2Click(object sender, EventArgs e)
        {
            ttbSearchMessage.ShowTrigger1 = true;
            BindGrid();
        }
    */
    public class TwinTriggerBoxPro : TwinTriggerBox
    {
        public event EventHandler<string> TriggerClick;

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            this.Trigger1Icon = TriggerIcon.Clear;
            this.Trigger2Icon = TriggerIcon.Search;
            this.ShowTrigger1 = false;
            this.Trigger1Click += (sender, args) =>
            {
                this.Text = "";
                this.ShowTrigger1 = false;
                if (TriggerClick != null)
                    TriggerClick(this, "Clear");
            };
            this.Trigger2Click += (sender, args) =>
            {
                this.ShowTrigger1 = true;
                if (TriggerClick != null)
                    TriggerClick(this, "Search");
            };
        }

    }
}