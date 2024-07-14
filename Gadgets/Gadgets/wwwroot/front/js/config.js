
var projectName = 'Gadgets';
var swiper = {
	width: '100%',
	height: '400px',
	arrow: 'none',
	anim: 'default',
	interval: 2000,
	indicator: 'outside'
}

var centerMenu = [{
	name: 'Personal Center',
	url: '../' + localStorage.getItem('userTable') + '/center.html'
}, 
{
	name: 'My Order',
	url: '../shop-order/list.html'
},

{
	name: 'My Address',
	url: '../shop-address/list.html'
},

{
	name: 'My Favorite',
	url: '../storeup/list.html'
}
]


var indexNav = [

{
	name: 'Item Information',
	url: './pages/shangpinxinxi/list.html'
}, 

]

var adminurl =  "http://localhost:5000/admin/dist/index.html";

var cartFlag = false

var chatFlag = false


cartFlag = true


var menu = [{"backMenu":[{"child":[{"buttons":["Add","View","Modify","Delete"],"menu":"Member","menuJump":"List","tableName":"huiyuan"}],"menu":"Member Management"},{"child":[{"buttons":["Add","View","Modify","Delete","ViewComment"],"menu":"Item Information","menuJump":"List","tableName":"shangpinxinxi"}],"menu":"Item Information Management"},{"child":[{"buttons":["Add","View","Modify","Delete"],"menu":"Staff","menuJump":"List","tableName":"yuangong"}],"menu":"Staff Management"},{"child":[{"buttons":["Add","View","Modify","Delete","Statements"],"menu":"Inventories","menuJump":"List","tableName":"kucun"}],"menu":"Inventories Management"},{"child":[{"buttons":["Add","View","Modify","Delete"],"menu":"Attendance","menuJump":"List","tableName":"kaoqin"}],"menu":"Attendance Management"},{"child":[{"buttons":["Add","View","Modify","Delete"],"menu":"Turnover statistics","menuJump":"List","tableName":"yingyeetongji"}],"menu":"Turnover statistics Management"},{"child":[{"buttons":["Add","View","Modify","Delete"],"menu":"Sales Management","menuJump":"List","tableName":"xiaoshouguanli"}],"menu":"Sales Management"},{"child":[{"buttons":["Add","View","Modify","Delete"],"menu":"Returns Management","menuJump":"List","tableName":"tuihuoguanli"}],"menu":"Returns Management"},{"child":[{"buttons":["Add","View","Modify","Delete","Statements"],"menu":"Sales","menuJump":"List","tableName":"xiaoshou"}],"menu":"Sales Management"},{"child":[{"buttons":["Add","View","Modify","Delete"],"menu":"My Favorite Management","tableName":"storeup"}],"menu":"My Favorite Management"},{"child":[{"buttons":["Add","View","Modify","Delete"],"menu":"Rotational Chart Management","tableName":"config"}],"menu":"System Management"},{"child":[{"buttons":["Add","View","Modify","Delete","Ship"],"menu":"Paid Order","tableName":"orders/Paid"},{"buttons":["Add","View","Modify","Delete"],"menu":"Refunded Order","tableName":"orders/Refunded"},{"buttons":["Add","View","Modify","Delete"],"menu":"Completed Order","tableName":"orders/Completed"},{"buttons":["Add","View","Modify","Delete"],"menu":"Shipped Order","tableName":"orders/Shipped"},{"buttons":["Add","View","Modify","Delete"],"menu":"Unpaid Order","tableName":"orders/Unpaid"},{"buttons":["Add","View","Modify","Delete"],"menu":"Cancelled订单","tableName":"orders/Cancelled"}],"menu":"Order Management"}],"frontMenu":[{"child":[{"buttons":["View","ViewComment"],"menu":"Item InformationList","menuJump":"List","tableName":"shangpinxinxi"}],"menu":"Item Information module"}],"hasBackLogin":"Yes","hasBackRegister":"No","hasFrontLogin":"No","hasFrontRegister":"No","roleName":"管理员","tableName":"users"},{"backMenu":[{"child":[{"buttons":["View","ViewComment"],"menu":"Item Information","menuJump":"List","tableName":"shangpinxinxi"}],"menu":"Item Information Management"},{"child":[{"buttons":["View","Delete"],"menu":"Returns Management","menuJump":"List","tableName":"tuihuoguanli"}],"menu":"Returns Management"},{"child":[{"buttons":["View"],"menu":"Completed Order","tableName":"orders/Completed"},{"buttons":["View","Confirm receipt of goods"],"menu":"Shipped Order","tableName":"orders/Shipped"},{"buttons":["View"],"menu":"Unpaid Order","tableName":"orders/Unpaid"},{"buttons":["View"],"menu":"Cancelled订单","tableName":"orders/Cancelled"},{"buttons":["View"],"menu":"Paid Order","tableName":"orders/Paid"},{"buttons":["View"],"menu":"Refunded Order","tableName":"orders/Refunded"}],"menu":"Order Management"}],"frontMenu":[{"child":[{"buttons":["View","ViewComment"],"menu":"Item InformationList","menuJump":"List","tableName":"shangpinxinxi"}],"menu":"Item Information module"}],"hasBackLogin":"Yes","hasBackRegister":"Yes","hasFrontLogin":"Yes","hasFrontRegister":"Yes","roleName":"Member","tableName":"huiyuan"},{"backMenu":[{"child":[{"buttons":["View"],"menu":"Item Information","menuJump":"List","tableName":"shangpinxinxi"}],"menu":"Item Information Management"},{"child":[{"buttons":["View","Statements"],"menu":"Inventories","menuJump":"List","tableName":"kucun"}],"menu":"Inventories Management"},{"child":[{"buttons":["View"],"menu":"Attendance","menuJump":"List","tableName":"kaoqin"}],"menu":"Attendance Management"},{"child":[{"buttons":["Statements","View"],"menu":"Sales","menuJump":"List","tableName":"xiaoshou"}],"menu":"Sales Management"},{"child":[{"buttons":["Ship"],"menu":"Paid Order","tableName":"orders/Paid"}],"menu":"Order Management"}],"frontMenu":[{"child":[{"buttons":["View","ViewComment"],"menu":"Item InformationList","menuJump":"List","tableName":"shangpinxinxi"}],"menu":"Item Information module"}],"hasBackLogin":"Yes","hasBackRegister":"Yes","hasFrontLogin":"No","hasFrontRegister":"No","roleName":"Staff","tableName":"yuangong"}]


var isAuth = function (tableName,key) {
    let role = localStorage.getItem("userTable");
    let menus = menu;
    for(let i=0;i<menus.length;i++){
        if(menus[i].tableName==role){
            for(let j=0;j<menus[i].backMenu.length;j++){
                for(let k=0;k<menus[i].backMenu[j].child.length;k++){
                    if(tableName==menus[i].backMenu[j].child[k].tableName){
                        let buttons = menus[i].backMenu[j].child[k].buttons.join(',');
                        return buttons.indexOf(key) !== -1 || false
                    }
                }
            }
        }
    }
    return false;
}

var isFrontAuth = function (tableName,key) {
    let role = localStorage.getItem("userTable");
    let menus = menu;
    for(let i=0;i<menus.length;i++){
        if(menus[i].tableName==role){
            for(let j=0;j<menus[i].frontMenu.length;j++){
                for(let k=0;k<menus[i].frontMenu[j].child.length;k++){
                    if(tableName==menus[i].frontMenu[j].child[k].tableName){
                        let buttons = menus[i].frontMenu[j].child[k].buttons.join(',');
                        return buttons.indexOf(key) !== -1 || false
                    }
                }
            }
        }
    }
    return false;
}
