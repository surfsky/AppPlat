-------------------------------------
AppPlat功能
后台登录账户：admin/admin
注意：不要用LocalDb，部署时可能会有数据库版本兼容问题
-------------------------------------
主要功能
	- DbBase 数据模型统一和简化
	- PageBase/RoleType/PowerType 简化和统一鉴权逻辑
	- FormBase 统一新增、修改、编辑逻辑
	- GridPro 简化表格的增删查改排序分页逻辑
	- SimpleFormPro 根据UIAttribute自动生成查看、编辑、新增页面，可用于初期快速搭建页面
	- 任务调度引擎：App.Consoler.exe


权限体系
    (1)RBAC 体系(Role-Based Access Control)：用户--角色--权限，用户被赋予角色，角色是一组权限的集合，最终是根据权限来做访问判断。
    (2)菜单的可视性：在菜单管理中设置
    (3)页面可访问性：给PageBase子类设置 Auth 特性，即可限制非授权用户
    (4)控件可操作性：调用PageBase.CheckPower(...)方法来判断
    注：一般来说
	权限是固定的，是和应用程序紧密绑定的，即使对之进行硬编码，也没有关系
    用户和角色是可在部署后动态配置的
	但在实际项目调研后，角色也基本上是固定的，故本系统中角色和权限都定义为 Enum
	也便于避免角色名称是字符串带来的困扰，修改时很容易出错，难以重构

通用网格基类GridPro
    /新增、删除
    /将Window作为子控件
    /分页、排序、行删除
    /InitGrid参数过多，用属性替代
        EnableEdit
        EnableNew
        EditWindowID
        NewUrlTmpl
        EditUrlTmpl
    /UIAttribute：title、group、formatstring
    /或让类实现一个接口：IUI
    /小数类型默认格式化为 {0:0.00}
    /接收外来初始化，如List<UIAttribute>，已用 UISetting 实现
    /查看列
    /GridPro.Init(Toolbar bar...)
    /刷新数据后，保持当前选择项。用Hidden
	/GridPro 实现两种InitGrid方法
		/IQueryable<T> GetQuery() 方案：已实现。建议取消。
			好处：自动完成排序分页等方法
			坏处：用户要修改代码
		void BindGrid 方案：
			减少用户修改量
			但必须手工写上排序分页方法

/通用实体类编辑窗体（FormPage）
    /用户自定义页面（字段、控件、位置、控件类别）

/通用实体类编辑窗体（SimpleFormPro）
    /三态：detail、new、edit
    /根据类型自动展示字段控件
    /填充数据
    /收集数据
    /保存数据
    /新增数据
    /修正只读逻辑
	实现其它类型控件，如bool，text、datetime

通用数据表管理系统（未完成）
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


-------------------------------------
基于AppBox做的重构和修改
-------------------------------------
/将Models目录独立，剥离DAL层代码
/将数据操作类都放到Handlers目录下，因为webcall既可以做dll层，也可以做httphandlers，干脆都放这吧
/Migrations/Configuration.cs 这个类干什么用的? 数据库迁移用的，移动到bussiness/models目录下并改名AppBoxMigrationConfiguration
/将非数据库操作类库放到Components目录下
/目录ajax-》HttpHandlers
/login.aspx -> ashx, 并直接用json，或者直接用DataUser接口
/将 PageBase 移动到根目录下，所有页面都需要继承该类
/合并非ui类到lib目录下
/废除WebService接口
/ICustomTree-》ITree
/废除PageHelper，将里面的常量移到Common类中
/统一目录的大小写，看起来很难受。。。强迫症啊
/PageBase 缩身，管的事情太多了。只保留权限判断
    /剥离Db -> Common
    /剥离cookieticket -> FormAuthHelper
    /剥离log -> Common
    /剥离tree -> TreeHelper
    /剥离Grid -> GridHelper
    /剥离排序 -> GridHelper
    /剥离online -> OnlineHelper
/main.aspx/title 现在是写死的，以后改为从数据库取（main.js里面的DATA来源不知）
/美化标题栏：title、bg
/美化登录页面（需定制背景）
/logo（不用换了）
/帮助菜单隐藏掉（算了，右移）
/集成日志模块
/重构default.aspx - login.ashx直接用datauser.userlogin接口
/将Extjs和icon目录移动到Res目录下（算了）. icon目录在fineui6时已移到res目录下，extjs仍然位于根目录
/优化首页
/简化权限
    过于复杂
    针对每个页面权限为：浏览、管理；
    或者对于一些简单的应用，权限都可以不用
    /合并用户、角色、权限等管理界面。用分栏结构，减少菜单项。
	/老的不动了，新的如此操作
/移动main.aspx到admin目录（图片、窗口都有问题，以后再改）

