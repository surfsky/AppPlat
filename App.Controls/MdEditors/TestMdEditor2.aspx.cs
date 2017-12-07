using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using FineUI;

namespace App.Controls.MdEditors
{
    /// <summary>
    /// MdEditor嵌在FineUI中，需要用AjaxSetText方法设置文本。
    /// </summary>
    public partial class TestMdEditor2 : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
                edt1.Text = "###hello world!";
        }

        // 设置编辑器值
        protected void btnSet_Click(object sender, EventArgs e)
        {
            //edt.Text = "###hello world!" + DateTime.Now.ToString();      // 直接设置失败
            edt1.AjaxSetText("###hello world!" + DateTime.Now.ToString());  // 用ajax方式发送脚本到客户端执行，成功
            edt2.AjaxSetText("###hello world!" + DateTime.Now.ToString());  // 用ajax方式发送脚本到客户端执行，成功
        }

        // 获取编辑器值
        protected void btnGet_Click(object sender, EventArgs e)
        {
            Alert.ShowInTop(HttpUtility.HtmlEncode(edt1.Text));
        }
    }
}