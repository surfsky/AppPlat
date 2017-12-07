-------------------------------------
AppPlat功能
后台登录账户：admin/admin
注意：不要用LocalDb，部署时可能会有数据库版本兼容问题
-------------------------------------
主要功能
    - DbBase EntityFramework 数据模型统一和简化
    - PageBase/RoleType/PowerType 简化和统一鉴权逻辑
    - FormBase 统一新增、修改、编辑逻辑
    - GridPro 简化表格的增删查改排序分页逻辑
    - SimpleFormPro 根据UIAttribute自动生成查看、编辑、新增页面，可用于初期快速搭建页面
    - 任务调度引擎：App.Consoler.exe
    - 集成日志系统


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
