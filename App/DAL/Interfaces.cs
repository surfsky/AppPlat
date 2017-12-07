﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace App.DAL
{
    /// <summary>
    /// 逻辑删除接口
    /// </summary>
    public interface ILogicDelete
    {
        bool InUsed { get; set; }
        //void DeleteLogic();
    }

    /// <summary>
    /// 树接口
    /// </summary>
    public interface ITree
    {
        /// <summary>名称</summary>
        string Name { get; set; }

        /// <summary>菜单在树形结构中的层级（从0开始）</summary>
        int TreeLevel { get; set; }

        /// <summary>是否可用（默认true）</summary>
        bool Enabled { get; set; }

        /// <summary>是否叶子节点（默认true）</summary>
        bool IsTreeLeaf { get; set; }
    }
}