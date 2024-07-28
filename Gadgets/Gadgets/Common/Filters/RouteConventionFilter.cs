using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Microsoft.AspNetCore.Mvc.Routing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Gadgets.Common.Filters
{
    public class RouteConventionFilter : IApplicationModelConvention
    {
        private readonly AttributeRouteModel _centralPrefix;

        public RouteConventionFilter(IRouteTemplateProvider routeTemplateProvider)
        {
            _centralPrefix = new AttributeRouteModel(routeTemplateProvider);
        }

        //接口的Apply方法
        public void Apply(ApplicationModel application)
        {
            //遍历所有的 Controller
            foreach (var controller in application.Controllers)
            {
                // 已经标记了 RouteAttribute 的 Controller
                var matchedSelectors = controller.Selectors.Where(x => x.AttributeRouteModel != null).ToList();
                if (matchedSelectors.Any())
                {
                    foreach (var selectorModel in matchedSelectors)
                    {
                        // 在 当前路由上 再 Add一个 路由前缀
                        selectorModel.AttributeRouteModel = AttributeRouteModel.CombineAttributeRouteModel(_centralPrefix,
                            selectorModel.AttributeRouteModel);

                        // 在 当前路由上 不再 Add任何路由前缀
                        //selectorModel.AttributeRouteModel = selectorModel.AttributeRouteModel;
                    }
                }

                // 没有标记 RouteAttribute 的 Controller
                var unmatchedSelectors = controller.Selectors.Where(x => x.AttributeRouteModel == null).ToList();
                if (unmatchedSelectors.Any())
                {
                    foreach (var selectorModel in unmatchedSelectors)
                    {
                        // Add一个 路由前缀
                        //selectorModel.AttributeRouteModel = _centralPrefix;

                        // 不Add前缀(说明：不使用全局路由，重构action，实现自定义、特殊的action路由Address)
                        selectorModel.AttributeRouteModel = selectorModel.AttributeRouteModel;
                    }
                }
            }
        }
    }
}
