F.ready(function () {
    // 获取组件
    var treeMenu = Ext.getCmp(DATA.treeMenu);
    var regionPanel = Ext.getCmp(DATA.regionPanel);
    var regionTop = Ext.getCmp(DATA.regionTop);
    var mainTabStrip = Ext.getCmp(DATA.mainTabStrip);
    var txtUser = Ext.getCmp(DATA.txtUser);
    var txtOnlineUserCount = Ext.getCmp(DATA.txtOnlineUserCount);
    var btnRefresh = Ext.getCmp(DATA.btnRefresh);


    //-----------------------------------------------
    // 欢迎信息和在线用户数（直接改造为服务器端）
    //-----------------------------------------------
    txtUser.setText('<span class="label">欢迎 </span><span>' + DATA.userName + '</span>&nbsp;&nbsp;');
    txtOnlineUserCount.setText('<span class="label">在线 </span>' + DATA.onlineUserCount);



});