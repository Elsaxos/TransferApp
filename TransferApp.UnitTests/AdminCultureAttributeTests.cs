using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Routing;
using Microsoft.AspNetCore.Http;
using NUnit.Framework;
using System.Globalization;
using TransferApp.Infrastructure;

namespace TransferApp.UnitTests.Infrastructure;

[TestFixture]
public class AdminCultureAttributeTests
{
    [Test]
    public void OnActionExecuting_Sets_Bg_Culture()
    {
        var attr = new AdminCultureAttribute();
        var actionContext = new ActionContext(
            new DefaultHttpContext(),
            new RouteData(),
            new ActionDescriptor()
        );
        var context = new ActionExecutingContext(
            actionContext,
            new List<IFilterMetadata>(),
            new Dictionary<string, object?>(),
            controller: new object()
        );

        attr.OnActionExecuting(context);

        Assert.That(CultureInfo.CurrentCulture.Name, Is.EqualTo("bg-BG"));
        Assert.That(CultureInfo.CurrentUICulture.Name, Is.EqualTo("bg-BG"));
    }
}
