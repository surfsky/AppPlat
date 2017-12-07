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
    /// MdEditor.js 嵌入到FineUI中。示例成功。必须写客户端调用脚本
    /// </summary>
    public partial class TestMdEditor1 : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                // 编辑器初始值可如下设置
                edtArea.Value = "###hello world!";
            }
        }

        // 设置编辑器值
        protected void btnSet_Click(object sender, EventArgs e)
        {
            // 直接操作 textarea 失败
            //edtArea.Value = "## This is title";

            // 注册脚本到客户端成功
            string content = "## This is title";
            PageContext.RegisterStartupScript(String.Format("updateEditor({0});", JsHelper.Enquote(content)));
        }

        // 获取编辑器值
        protected void btnGet_Click(object sender, EventArgs e)
        {
            Alert.ShowInTop(HttpUtility.HtmlEncode(edtArea.Value));
        }
    }
}