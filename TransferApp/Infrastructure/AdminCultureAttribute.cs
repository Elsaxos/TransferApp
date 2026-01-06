using Microsoft.AspNetCore.Mvc.Filters;
using System.Globalization;

namespace TransferApp.Infrastructure
{
    public class AdminCultureAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var culture = new CultureInfo("bg-BG");
            CultureInfo.CurrentCulture = culture;
            CultureInfo.CurrentUICulture = culture;

            base.OnActionExecuting(context);
        }
    }
}

