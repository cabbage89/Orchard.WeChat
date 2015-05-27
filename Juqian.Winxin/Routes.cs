using Orchard.Mvc.Routes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace Juqian.Winxin
{
    public class Routes : IRouteProvider
    {

        public void GetRoutes(ICollection<RouteDescriptor> routes)
        {
            foreach (var routeDescriptor in GetRoutes())
                routes.Add(routeDescriptor);
        }

        public IEnumerable<RouteDescriptor> GetRoutes()
        {
            return new[] {
                             new RouteDescriptor {
                                                     Route = new Route(
                                                         "wxSvc",
                                                         new RouteValueDictionary {
                                                                                      {"area", "Juqian.Winxin"},
                                                                                      {"controller", "WinXin"},
                                                                                      {"action", "Index"}
                                                                                  },
                                                         new RouteValueDictionary(),
                                                         new RouteValueDictionary {
                                                                                      {"area", "Juqian.Winxin"}
                                                                                  },
                                                         new MvcRouteHandler())
                                                 }
            };
        }
    }
}