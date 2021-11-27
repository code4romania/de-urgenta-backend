using System;
using System.Globalization;
using DeUrgenta.Domain.Api.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Routing;

namespace DeUrgenta.Backpack.Api.Constraints
{
    public class BackpackItemCategoryConstraint : IRouteConstraint
    {
        public bool Match(HttpContext httpContext, IRouter route,
            string routeKey, RouteValueDictionary values,
            RouteDirection routeDirection)
        {
            if (values.TryGetValue(routeKey, out var routeValue))
            {
                if (routeValue == null)
                {
                    return false;
                }

                var stringValue = Convert.ToString(routeValue, CultureInfo.InvariantCulture);

                if (int.TryParse(stringValue, out var value))
                {
                    return Enum.IsDefined(typeof(BackpackItemCategoryType), value);
                }
            }

            return false;
        }
    }
}