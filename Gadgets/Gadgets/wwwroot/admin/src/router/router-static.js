import Vue from 'vue';
//配置路由
import VueRouter from 'vue-router'
Vue.use(VueRouter);
//1.创建组件
import Index from '@/views/index'
import Home from '@/views/home'
import Login from '@/views/login'
import NotFound from '@/views/404'
import UpdatePassword from '@/views/update-password'
import pay from '@/views/pay'
import register from '@/views/register'
import center from '@/views/center'
    import yuangong from '@/views/modules/yuangong/list'
    import yingyeetongji from '@/views/modules/yingyeetongji/list'
    import shangpinxinxi from '@/views/modules/shangpinxinxi/list'
    import xiaoshouguanli from '@/views/modules/xiaoshouguanli/list'
    import kucun from '@/views/modules/kucun/list'
    import xiaoshou from '@/views/modules/xiaoshou/list'
    import huiyuan from '@/views/modules/huiyuan/list'
    import storeup from '@/views/modules/storeup/list'
    import discussshangpinxinxi from '@/views/modules/discussshangpinxinxi/list'
    import orders from '@/views/modules/orders/list'
    import kaoqin from '@/views/modules/kaoqin/list'
    import tuihuoguanli from '@/views/modules/tuihuoguanli/list'
    import config from '@/views/modules/config/list'


//2.配置路由   注意：名字
const routes = [{
    path: '/index',
    name: 'Home',
    component: Index,
    children: [{
      // 这里不设置Value，Yes把main作为DefaultPage面
      path: '/',
      name: 'Home',
      component: Home,
      meta: {icon:'', title:'center'}
    }, {
      path: '/updatePassword',
      name: 'ModifyPassword',
      component: UpdatePassword,
      meta: {icon:'', title:'updatePassword'}
    }, {
      path: '/pay',
      name: 'Pay',
      component: pay,
      meta: {icon:'', title:'pay'}
    }, {
      path: '/center',
      name: '个人Message',
      component: center,
      meta: {icon:'', title:'center'}
    }
          ,{
	path: '/yuangong',
        name: 'Staff',
        component: yuangong
      }
          ,{
	path: '/yingyeetongji',
        name: 'Turnover statistics',
        component: yingyeetongji
      }
          ,{
	path: '/shangpinxinxi',
        name: 'Item Information',
        component: shangpinxinxi
      }
          ,{
	path: '/xiaoshouguanli',
        name: 'Sales Management',
        component: xiaoshouguanli
      }
          ,{
	path: '/kucun',
        name: 'Inventories',
        component: kucun
      }
          ,{
	path: '/xiaoshou',
        name: 'Sales',
        component: xiaoshou
      }
          ,{
	path: '/huiyuan',
        name: 'Member',
        component: huiyuan
      }
          ,{
	path: '/storeup',
        name: 'My Favorite Management',
        component: storeup
      }
          ,{
	path: '/discussshangpinxinxi',
        name: 'Item Comment',
        component: discussshangpinxinxi
      }
          ,{
        path: '/orders/:status',
        name: 'Order Management',
        component: orders
      }
          ,{
	path: '/kaoqin',
        name: 'Attendance',
        component: kaoqin
      }
          ,{
	path: '/tuihuoguanli',
        name: 'Returns Management',
        component: tuihuoguanli
      }
          ,{
	path: '/config',
        name: 'Rotational Chart Management',
        component: config
      }
        ]
  },
  {
    path: '/login',
    name: 'login',
    component: Login,
    meta: {icon:'', title:'login'}
  },
  {
    path: '/register',
    name: 'register',
    component: register,
    meta: {icon:'', title:'register'}
  },
  {
    path: '/',
    name: 'Home',
    redirect: '/index'
  }, /*Default跳转路由*/
  {
    path: '*',
    component: NotFound
  }
]
//3.实例化VueRouter  注意：名字
const router = new VueRouter({
  mode: 'hash',
  /*hash模式改为history*/
  routes // （缩写）相当于 routes: routes
})

export default router;
