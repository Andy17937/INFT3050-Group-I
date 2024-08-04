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

        //The Apply method of the interface
        public void Apply(ApplicationModel application)
        {
            //Iterate over all Controllers
            foreach (var controller in application.Controllers)
            {
                // Controllers that have been tagged with RouteAttribute
                var matchedSelectors = controller.Selectors.Where(x => x.AttributeRouteModel != null).ToList();
                if (matchedSelectors.Any())
                {
                    foreach (var selectorModel in matchedSelectors)
                    {
                        // Add another route prefix to the current route.
                        selectorModel.AttributeRouteModel = AttributeRouteModel.CombineAttributeRouteModel(_centralPrefix,
                            selectorModel.AttributeRouteModel);

                        // Add no more route prefixes to the current route.
                        //selectorModel.AttributeRouteModel = selectorModel.AttributeRouteModel;
                    }
                }

                // Controllers not tagged with RouteAttribute
                var unmatchedSelectors = controller.Selectors.Where(x => x.AttributeRouteModel == null).ToList();
                if (unmatchedSelectors.Any())
                {
                    foreach (var selectorModel in unmatchedSelectors)
                    {
                        // Add a Routing Prefix
                        //selectorModel.AttributeRouteModel = _centralPrefix;

                        // No Add prefix (note: do not use global routes, refactor actions, implement custom, special action route Address)
                        selectorModel.AttributeRouteModel = selectorModel.AttributeRouteModel;
                    }
                }
            }
        }
    }
}
