/**
 * Ext.ux.ToastWindow
 *
 * @author  Edouard Fattal
 * @date	March 14, 2008
 *
 * @class Ext.ux.ToastWindow
 * @extends Ext.Window
 */

Ext.namespace("Ext.ux");

Ext.ux.NotificationMgr = {
	positions :new Ext.util.MixedCollection()
}

Ext.ux.Notification = Ext.extend(Ext.Window, {
	initComponent: function(){
		Ext.apply(this, {
			iconCls: this.iconCls || 'x-icon-information',
			cls: 'x-notification',
			width: 200,
			height: 100,
			autoHeight: true,
			plain: false,
			draggable: false,
			autoHide :  this.autoHide && true, //是否自动隐藏窗口
			hideDelay: this.hideDelay || 3000 ,//如果自动隐藏，n毫秒后隐藏窗口。autoHide为true，hideDelay起作用
			minimizable:true,
//			constrain:true,
			bodyStyle: 'text-align:left'
		});
		if(this.autoHide) {
			this.task = new Ext.util.DelayedTask(this.hideWin, this);
		}
		Ext.ux.Notification.superclass.initComponent.call(this);
	},
	hideWin:function(){
				this.hide();
				this.close();//关闭当前窗口
	},
	setMessage: function(msg){
		this.body.update(msg);
	},
	setTitle: function(title, iconCls){
		Ext.ux.Notification.superclass.setTitle.call(this, title, iconCls||this.iconCls);
	},
	onRender:function(ct, position) {
		Ext.ux.Notification.superclass.onRender.call(this, ct, position);
	},
	minimize:function(){
		if(this.minimizable){
			this.hideWin();
		}
	},
	onDestroy: function(){
		if(typeof(Ext.ux.NotificationMgr.positions.get(this.xpos)) != 'undefined')
			Ext.ux.NotificationMgr.positions.get(this.xpos).remove(this.ypos);
		Ext.ux.Notification.superclass.onDestroy.call(this);
	},
	cancelHiding: function(){
		this.addClass('fixed');
		if(this.autoHide) {
			this.task.cancel();
		}
	},
	afterShow: function(){
		Ext.ux.Notification.superclass.afterShow.call(this);
		Ext.fly(this.body.dom).on('click', this.cancelHiding, this);
		if(this.autoHide) {
			this.task.delay(this.hideDelay || 3000);
	   }
	},
	animShow: function(x,y){
		
		this.ypos = x||0;
		this.xpos = y||0;
		do{
			//获取当前x轴的定位数据
			if(Ext.ux.NotificationMgr.positions.get(this.xpos) == null){
				Ext.ux.NotificationMgr.positions.add(this.xpos,[]);
			}
			//获取y轴定位数据，如果定位数据为-1，说明当前定位数据可用
			while(Ext.ux.NotificationMgr.positions.get(this.xpos).indexOf(this.ypos)>-1)
				this.ypos++;
			
			
			//this.setSize(this.width,this.height);
		
			this.totalHeight = -20-((this.getSize().height+10)*this.ypos);
			//如果当前x轴上的y轴数据是第一次显示，不检测高度（如果检测高度，会导致死循环）
			if(this.ypos == 0 || Ext.getBody().getHeight() - this.height+this.totalHeight > 0){
				this.totalWidth = -20-((this.getSize().width+10)*this.xpos);
				Ext.ux.NotificationMgr.positions.get(this.xpos).push(this.ypos);
				
				this.el.alignTo(document, "br-br", [ this.totalWidth,  this.totalHeight]);
				this.el.slideIn('b', {
					duration: 1,
					callback: this.afterShow,
					scope: this	});
				break;
			}
			else{//如果y轴的高度大于当前窗口的高度，开始进入新的x轴定位数据
				this.xpos++;
	//			this.totalWidth = -20-((this.getSize().width+10)*this.xpos);
				this.ypos=0;
			}
			
		}while(true)
		
	},
	animHide: function(){
		if(typeof(Ext.ux.NotificationMgr.positions.get(this.xpos)) != 'undefined'){
		   Ext.ux.NotificationMgr.positions.get(this.xpos).remove(this.ypos);
		   if(Ext.ux.NotificationMgr.positions.get(this.xpos).length == 0){//当前x轴上的y轴定位数据为0
				Ext.ux.NotificationMgr.positions.removeKey(this.xpos);
			}
		}
		this.el.ghost("b", {
			duration: 1,
			remove: true
		});
	},
	
	/**
	 * 调用方法：操作成功，显示成功的信息
	 * @param {} title
	 * @param {} msg
	 */
	showSuccess:function(title,msg){
		this.iconCls=	'x-icon-information',
		this.title = title||'success';
		this.html = msg||'process successfully!';
		this.show(document);
	},
	/**
	 * 调用方法：操作失败，显示失败的信息
	 * @param {} title
	 * @param {} msg
	 */
	showFailure:function(title,msg){
		this.iconCls=	'x-icon-error',
		this.title = title||'success';
		this.html = msg||'process successfully!';
		this.show(document);	
	},
	/**
	 * 调用方法：显示操作结果的信息
	 * @param {} title
	 * @param {} msg
	 * @param {} success 操作是否成功
	 */
	showMessage:function(title,msg,success){
		if(success){
			this.iconCls=	'x-icon-information';
			this.autoHide=true;//自动隐藏窗口
			this.task = new Ext.util.DelayedTask(this.hideWin, this);
		}
		else{
			this.iconCls=	'x-icon-error';
			}
			this.title = title;
			this.html = msg;
			this.show(document);			
	},
	focus: Ext.emptyFn 

}); 
Ext.EventManager.onWindowResize(function(){
	Ext.ux.NotificationMgr.positions.clear();
});