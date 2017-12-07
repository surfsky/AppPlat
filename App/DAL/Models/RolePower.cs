using App.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace App.DAL
{
    /// <summary>
    /// 角色清单
    /// </summary>
    public enum RoleType
    {
        [UI("系统管理员")]             Admin = 0,
        [UI("会员", "Customer")]       Customer = 1,
        [UI("客户经理", "Employee")]   Salesman = 3,
        [UI("客服", "Employee")]       Receptionist = 7
    }

    /// <summary>
    /// 权限清单
    /// </summary>
    public enum PowerType : int
    {
        // 基础框架权限
        [UI("访问后台", "Core")]        Backend = -1,
        [UI("Admin专用", "Admin")]      Admin = 0,

        // 用户相关
        [UI("用户浏览",  "用户")]       UserView = 1,
        [UI("用户增改",  "用户")]       UserEdit = 2,
        [UI("用户删除",  "用户")]       UserDelete = 3,
        [UI("帮用户修改密码", "用户")]  UserChangePassword = 4,
        [UI("角色权限管理", "用户")]    RolePowerEdit = 5,

        // 配置
        [UI("部门查看", "配置")]        DeptView = 10,
        [UI("部门增改", "配置")]        DeptEdit = 11,
        [UI("部门删除", "配置")]        DeptDelete = 12,
        [UI("职务查看", "配置")]        TitleView = 13,
        [UI("职务增改", "配置")]        TitleEdit = 14,
        [UI("职务删除", "配置")]        TitleDelete = 15,
        [UI("区域查看", "配置")]        AreaView = 16,
        [UI("区域增改", "配置")]        AreaEdit = 17,
        [UI("区域删除", "配置")]        AreaDelete = 18,

        // 运维
        [UI("配置管理", "运维")]        ConfigEdit = 20,
        [UI("菜单管理", "运维")]        MenuEdit = 21,
        [UI("客户端管理", "运维")]      AppEdit = 22,
        [UI("日志管理", "运维")]        LogEdit = 23,
        [UI("在线用户管理", "运维")]    OnlineEdit = 24,
        [UI("资源管理", "运维")]        ResEdit = 25,
        [UI("消息管理", "运维")]        MessageEdit = 26,

        // 应用
        [UI("文章查看", "应用")]        ArticleView = 30,
        [UI("文章增改", "应用")]        ArticleEdit = 31,
        [UI("文章删除", "应用")]        ArticleDelete = 32,
        [UI("投诉查看", "应用")]        ComplainView = 33,
        [UI("投诉增改", "应用")]        ComplainEdit = 34,
        [UI("投诉删除", "应用")]        ComplainDelete = 35,

        // 报表
        [UI("报表查看", "报表")]        ReportView = 50,
        [UI("报表管理", "报表")]        ReportEdit = 51,

        // 商店
        [UI("门店查看", "商店")]        StoreView = 100,
        [UI("门店增改", "商店")]        StoreEdit = 101,
        [UI("门店删除", "商店")]        StoreDelete = 102,
        [UI("产品查看", "商店")]        ProductView = 103,
        [UI("产品增改", "商店")]        ProductEdit = 104,
        [UI("产品删除", "商店")]        ProductDelete = 105,
        [UI("订单查看", "商店")]        OrderView = 106,
        [UI("订单增改", "商店")]        OrderEdit = 107,
        [UI("订单删除", "商店")]        OrderDelete = 108,
        [UI("修改订单价格", "商店")]    OrderEditPrice = 109,
    }


    /// <summary>
    /// 角色拥有的权限
    /// TODO: 因为数据库冲突临时改名，以后再改为RolePower
    /// </summary>
    public class RolePower : EntityBase<RolePower>
    {
        public RoleType Role { get; set; }
        public PowerType Power { get; set; }
    }

}