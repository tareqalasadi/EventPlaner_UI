using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using System.Globalization;

namespace EventPlaner_UI.Controllers
{
    public class LangController : Controller
    {
        public ActionResult Index()
        {
            return RedirectToAction("SwitchLanguage", new { lang = "ar-Jo" });
        }

        public IActionResult SwitchLanguage(string cluture, string returnUrl)
        {

            Response.Cookies.Append(
             CookieRequestCultureProvider.DefaultCookieName,
             CookieRequestCultureProvider.MakeCookieValue(new RequestCulture(cluture)),
             new CookieOptions { Expires = DateTimeOffset.UtcNow.AddHours(1) }

             );
            var cultureInfo = new CultureInfo(cluture)
            {
                DateTimeFormat =
                {
                    Calendar = new GregorianCalendar(),
                    ShortDatePattern = "dd/MM/yyyy",
                    LongDatePattern = "dd/MM/yyyy",
                    FullDateTimePattern = "dd/MM/yyyy hh:mm:ss"
                }
            };
            HttpContext.Session.SetString("clutureLanguage", cluture);

            Thread.CurrentThread.CurrentCulture = cultureInfo;
            Thread.CurrentThread.CurrentUICulture = cultureInfo;
            return Redirect(Request.Headers["Referer"].ToString());
            //return LocalRedirect(returnUrl);

        }
    }
}
