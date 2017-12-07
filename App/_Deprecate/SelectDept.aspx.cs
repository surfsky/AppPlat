using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.Entity;
using System.Linq;
using FineUI;
using System.Transactions;
using System.Text;
using App.DAL;

namespace App.Admin
{
    /// <summary>
    /// 部门选择窗口
    /// 输入参数：ids
    /// </summary>
    [ViewPower("CoreDeptView")]
    public partial class SelectDept : PageBase
    {
        private int _deptID;                // 当前部门ID
        private int _selectedRowIndex = -1; // 表格选中行索引


        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                btnClose.OnClientClick = ActiveWindow.GetHideReference();
                _deptID = Common.GetQueryIntValue("ids");
                BindGrid();
                if (_selectedRowIndex != -1)
                    Grid1.SelectedRowIndex = _selectedRowIndex;
            }
        }

        // 绑定表格
        private void BindGrid()
        {
            Grid1.DataSource = DbDept.Depts;
            Grid1.DataBind();
        }

        // 行绑定后，确定应该选择哪一行
        protected void Grid1_RowDataBound(object sender, FineUI.GridRowEventArgs e)
        {
            Dept dept = e.DataItem as Dept;
            if (dept != null && _deptID == dept.ID)
                _selectedRowIndex = e.RowIndex;
        }

        // 保存并关闭，将当前选择数据传递给父窗口
        protected void btnSaveClose_Click(object sender, EventArgs e)
        {
            int selectedRowIndex = Grid1.SelectedRowIndex;
            string deptID = Grid1.DataKeys[selectedRowIndex][0].ToString();
            string deptName = Grid1.DataKeys[selectedRowIndex][1].ToString();

            PageContext.RegisterStartupScript(
                  ActiveWindow.GetWriteBackValueReference(deptID, deptName)
                + ActiveWindow.GetHideReference()
                );
        }
    }
}
