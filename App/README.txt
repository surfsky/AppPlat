﻿通用数据表管理系统（未完成）
	数据表清单（从AppContext获取）
	展示
	新增
	编辑
	删除

实现和简化内容管理系统: 文本、图片、资源; 批量上传图片; 上传大文件
优化和简化
    用 GridPro & SimplyForm 快速完成表单管理页面
    简化权限列表, eg: CoreUserView, CoreUserEdit（包括增加、删除、修改、修改密码）
实现工作流
    处理历史
    指派处理
    实现投诉处理流程
    增加 LiteFlow 工作流引擎
        增加通用事件指派流程
        按金额报销流程

优化
    增加几个FineUI控件
        HUD: 如保存成功提醒。参照Loading div的样式改即可。
        颜色选取框
        下拉网格
        日期时间组合控件
    创建快速页面开发体系（EF + IID + MVC思路）
        优化 SimplyFormPro
            实现其它类型控件，如bool，text、datetime
            修改dict类型{Field, Control, ControlField}，便于实现各种控件值的读写
        创建页面体系
            /PageBase                 : 页面基类，负责访问权限控制、在线人数统计等统一逻辑
                /FormPage             : 实体表单编辑页面，支持展示、新增、修改逻辑
                    SimplyFormPage   : 用简单表单自动展示的页面（单列，含label），增加LabelPosition属性
                    HtmlFormPage     : 自定义html表单页面，用占位符表示字段控件，可用html编辑器做设计器，字段作为图标嵌入在工具栏中。
                    MobileFormPage   : 移动风格的表单页面，所有控件都用移动风格替代
                CollectionPage       : 展示数据集合，支持排序、分页、新增、删除、查看、修改、批量删除、导出
                    GridPage         : 以网格方式展示的页面，新增编辑页面用弹窗方式展示
                    GridDetailPage   : 以网格+详情方式展示的页面，新增编辑页面在详情页面中展示
                    MobileCardPage   : 以卡片方式展示的页面，手机上常用，点击后跳到详情页
        说明
            表单页、数据集合页面的数据源类型是确定且剥离的，仅页面表现形式不一样。
            其实和MVC很相似，就是页面用webform方式而已
            实现对应的控件，便于用户实现自己的复合布局页面，如左右，而非弹窗
    抽空再试试UrlRewrite，估计是IIS配置项问题
    尝试定制主题: 或者用浏览器开发者工具，抽取下css，修改一套出来。


