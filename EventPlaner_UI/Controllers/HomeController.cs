using EventPlaner_UI.Helper;
using EventPlaner_UI.Helper.Redis;
using EventPlaner_UI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Newtonsoft.Json;
using ServiceStack;
using System.Diagnostics;
using System.Text;
using System.Text.RegularExpressions;

namespace EventPlaner_UI.Controllers
{
    public class HomeController : Controller
    {
        #region Parameters
        private readonly IEmailHelper _IEmailHelper;
        private readonly IConfiguration _IConfiguration;
        private readonly IRedisConfig _IRedisConfig;
        private readonly IStringLocalizer<HomeController> _Localizer;
        #endregion
        #region Constructor
        public HomeController(IEmailHelper emailHelper, IConfiguration configuration, IRedisConfig redisConfig, IStringLocalizer<HomeController> stringLocalizer)
        {
            _IEmailHelper = emailHelper;
            _IConfiguration = configuration;
            _IRedisConfig = redisConfig;
            _Localizer= stringLocalizer;
        }
        #endregion
        #region Get
        /// <summary>
        /// </summary>
        /// <returns></returns>
        public IActionResult Index()
        {
            var GetAllServicsAPI = _IConfiguration["BaseWebAPIURL"] + "Services/All";
            var readTask = Task.Run(() => ManageAPI.GetURI(new Uri(GetAllServicsAPI)));

            readTask.Wait();
            if (readTask.IsCompleted && !string.IsNullOrEmpty(readTask.Result))
            {
                var response = JsonConvert.DeserializeObject<List<Service>>(readTask.Result, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
                response = response.Where(s => s.ID != 14).ToList();

                return View(response);
            }
            return View();
        }
        public IActionResult CategoryOfService(long Id)
        {
            var GetCategoryOfService = _IConfiguration["BaseWebAPIURL"] + "Services/GetCategoryOfService/" + Id;
            var readTask = Task.Run(() => ManageAPI.GetURI(new Uri(GetCategoryOfService)));

            readTask.Wait();
            if (readTask.IsCompleted && !string.IsNullOrEmpty(readTask.Result))
            {
                var response = JsonConvert.DeserializeObject<List<CategoryOfService>>(readTask.Result, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
                return View(response);
            }
            return View();
        }

        public IActionResult DirectRequest(long Id)
        {
            if (Id > 0)
            {
                var getCategoryOfService = _IConfiguration["BaseWebAPIURL"] + "Services/GetImgesByServiceId/" + Id;
                var readTask = Task.Run(() => ManageAPI.GetURI(new Uri(getCategoryOfService)));

                readTask.Wait();
                if (readTask.IsCompletedSuccessfully && !string.IsNullOrEmpty(readTask.Result))
                {
                    var response = JsonConvert.DeserializeObject<List<DirectImage>>(readTask.Result, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });

                    var uniqueTypeServices = response.Select(r => r.TypeService).Distinct().ToList();

                    ViewBag.TypeServices = uniqueTypeServices;
                    response.FirstOrDefault().ServiceId=Id;
                    return View(response);
                }
            }

            return View();
        }
        public async Task<IActionResult> DirectRequestPartial(long Id)
        {
            try
            {

                if (Id > 0)
                {
                    var response = new List<DirectImage>();
                    var Data = await _IRedisConfig.GetValueAsync("DirectRequestPartial" + Id);
                    if (string.IsNullOrEmpty(Data)|| Data=="[]")
                    {
                        var getCategoryOfService = _IConfiguration["BaseWebAPIURL"] + "Services/GetImgesByServiceId/" + Id;
                        var result = await ManageAPI.GetURI(new Uri(getCategoryOfService));

                        Console.WriteLine("Raw JSON Result: " + result);

                        if (!string.IsNullOrEmpty(result))
                        {
                            response = JsonConvert.DeserializeObject<List<DirectImage>>(result, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
                            await _IRedisConfig.SetValueAsync("DirectRequestPartial" + Id, result);

                        }
                    }
                    else
                    {
                        response = JsonConvert.DeserializeObject<List<DirectImage>>(Data, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });

                    }
                    ViewBag.Data = response;
                    return PartialView("~/Views/Shared/_DirectRequestPartial.cshtml", response);
                }


            }
            catch (Exception ex)
            {
                Console.Error.WriteLine("Error in DirectRequestPartial: " + ex.ToString());

            }
            return PartialView("~/Views/Shared/_DirectRequestPartial.cshtml", new List<DirectImage>());
        }






        public JsonResult DirectRequestImages(long Id)
        {
            var getCategoryOfService = _IConfiguration["BaseWebAPIURL"] + "Services/GetImgesByServiceId/" + Id;
            var readTask = Task.Run(() => ManageAPI.GetURI(new Uri(getCategoryOfService)));

            readTask.Wait();
            if (readTask.IsCompletedSuccessfully && !string.IsNullOrEmpty(readTask.Result))
            {
                var response = JsonConvert.DeserializeObject<List<DirectImage>>(readTask.Result, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
                return Json(new { success = true, images = response });
            }
            return Json(new { success = false });
        }



        public async Task<IActionResult> PartyService(long Id)
        {
            if (Id > 0)
            {
                var response = new List<PartyServices>();
                var Data = await _IRedisConfig.GetValueAsync("PartyService_" + Id);
                if (string.IsNullOrEmpty(Data))
                {
                    var GetPartyService = _IConfiguration["BaseWebAPIURL"] + "Services/GetPartyServiceById/" + Id;
                    var readTask = Task.Run(() => ManageAPI.GetURI(new Uri(GetPartyService)));

                    readTask.Wait();
                    if (readTask.IsCompletedSuccessfully && !string.IsNullOrEmpty(readTask.Result))
                    {
                        response = JsonConvert.DeserializeObject<List<PartyServices>>(readTask.Result, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });

                        await _IRedisConfig.SetValueAsync("PartyService_" + Id, readTask.Result);
                    }
                }
                else
                {
                    response = JsonConvert.DeserializeObject<List<PartyServices>>(Data, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
                }

                var uniqueTypeServices = response.Select(r => r.TypeService).Distinct().ToList();

                ViewBag.TypeServices = uniqueTypeServices;
                return View(response);
            }

            return View();
        }

        public IActionResult DirectServices()
        {
            var directServiecJson = HttpContext.Session.GetString("DirectServiec");
            if (!string.IsNullOrEmpty(directServiecJson))
            {
                var directServiec = JsonConvert.DeserializeObject<DirectServiec>(directServiecJson);
                return View(directServiec);
            }
            return BadRequest();
        }



                [HttpPost]
        public IActionResult GenericView([FromBody] DirectServiec directServiec)
        {
            if (directServiec != null)
            {
                var directServiecJson = JsonConvert.SerializeObject(directServiec);
                HttpContext.Session.SetString("DirectServiec", directServiecJson);
                return Json(true);
            }
            return BadRequest();
        }



        [HttpPost]
        public async Task<JsonResult> SubmitRequestAsync([FromBody] SubmitRequestVewModel CustomerName)
        {
            if (CustomerName != null)
            {
                var All_Item_Model = new List<DirectImage>();
                var All_Item_Cart = new List<CartItemViewModel>();
                if (!string.IsNullOrEmpty(CustomerName.All_items))
                {
                    All_Item_Model = JsonConvert.DeserializeObject<List<DirectImage>>(CustomerName.All_items, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
                    CustomerName.AllItemModel = All_Item_Model;
                    //CustomerName.All_items = String.Empty;
                }
                if (!string.IsNullOrEmpty(CustomerName.Cart_Items))
                {
                    All_Item_Cart = JsonConvert.DeserializeObject<List<CartItemViewModel>>(CustomerName.All_items, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
                    CustomerName.AllItemCartModel = All_Item_Cart;
                    //CustomerName.Cart_Items = string.Empty;
                }
                var addPartyServiceAPI = _IConfiguration["BaseWebAPIURL"] + "Services/InsertDirectServiceRequest";
                var jsonInsert = JsonConvert.SerializeObject(CustomerName);
                var content = new StringContent(jsonInsert, Encoding.UTF8, "application/json");

                var response = await ManageAPI.PostURI(new Uri(addPartyServiceAPI), content);
                if (response != null)
                    return Json(true);
            }
            return null;
        }

      

        #endregion
        #region Post Method
        public async Task<IActionResult> SubmitPartyForm([FromBody] RequstPartyModel model) 
        {
            if (ModelState.IsValid)
            {
                var InsertCustomerRequestAPI = _IConfiguration["BaseWebAPIURL"] + "Services/InsertCustomerRequest";
                var jsonInsert = JsonConvert.SerializeObject(model);
                var content = new StringContent(jsonInsert, Encoding.UTF8, "application/json");

               var response = await ManageAPI.PostURI(new Uri(InsertCustomerRequestAPI), content);





                var emailBody = BuildEmailBody(model);

                bool emailSent = _IEmailHelper.SendEmail(
                    toEmail: model.Email,
                    subject: "Party Booking Confirmation",
                    body: emailBody
                );

                if (emailSent)
                {
                    return Json(new { success = true, message = "Form submitted successfully. Confirmation email sent." });
                }
                else
                {
                    return Json(new { success = false, message = "Form submitted successfully, but email sending failed." });
                }
            }

            return Json(new { success = false, message = "Validation failed." });
        }
        [HttpPost]
        public IActionResult UploadDesign([FromForm] IFormFile file, [FromForm] string phoneNumber) 
        {
            // Validate phone number
            if (string.IsNullOrWhiteSpace(phoneNumber) || !Regex.IsMatch(phoneNumber, @"^(07\d{8}|7\d{8})$"))
            {
                return Json(new { success = false, message = "Invalid phone number" });
            }

            if (file != null && file.Length > 0)
            {
                var uploadPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/uploads");
                if (!Directory.Exists(uploadPath))
                {
                    Directory.CreateDirectory(uploadPath);
                }

                var filePath = Path.Combine(uploadPath, file.FileName);
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    file.CopyTo(stream);
                }

                return Json(new { success = true, message = "Upload successful" });
            }

            return Json(new { success = false, message = "No file received" });
        }

        [HttpPost]
        public async Task<IActionResult> BookingEmail([FromBody] SendBookingEmailModel model)
        {
            if (ModelState.IsValid)
            {
                var EmailBody = string.Format(_Localizer["BookingcutomEventEmailBody"], model.Name, model.Email, model.Phone, model.Date, model.Time, model.People, model.Message);

                bool emailSent = _IEmailHelper.SendEmail(
                   toEmail: "o.tashman@gig.com.jo",
                   subject: "spical Booking  ",
                   body: EmailBody
               );
            }
            return Json(new { success = false, message = "Validation failed." });
        }

        #endregion
        public string BuildEmailBody(RequstPartyModel model)
        {
            var sb = new StringBuilder();

            sb.Append("<h2>Party Booking Details</h2>");
            sb.Append("<hr>");

            sb.Append("<ul>");
            sb.Append($"<li><b>Service Name:</b> {model.serviceName ?? "N/A"}</li>");
            sb.Append($"<li><b>Booking ID:</b> {model.ID}</li>");
            sb.Append($"<li><b>Customer Name:</b> {model.CustomerName}</li>");
            sb.Append($"<li><b>Phone Number:</b> {model.PhoneNumber}</li>");
            sb.Append($"<li><b>Email:</b> {model.Email}</li>");
            sb.Append($"<li><b>City:</b> {model.CityId}</li>");
            sb.Append($"<li><b>Start Date:</b> {model.StartDate:dd-MM-yyyy}</li>");
            sb.Append($"<li><b>Theme Color:</b> {model.ThemeColor}</li>");
            sb.Append($"<li><b>Flower Types:</b> {string.Join(", ", model.FlowersType ?? new List<string>())}</li>");
            sb.Append($"<li><b>Notes:</b> {model.Note}</li>");
            if (!string.IsNullOrEmpty(model.MainImage))
            {
                sb.Append($"<li><b>Main Image:</b><br><img src='{model.MainImage}' style='max-width: 300px; max-height: 200px;' /></li>");
            }
            sb.Append("</ul>");

            // Add new fields
            sb.Append("<h3>Additional Options</h3>");
            sb.Append("<ul>");
            sb.Append($"<li><b>With Zafa:</b> {(model.WithZafa == true ? "Yes" : "No")}</li>");
            sb.Append($"<li><b>With Friends:</b> {(model.WithFreind == true ? "Yes" : "No")}</li>");
            if (model.WithFreind == true)
            {
                sb.Append($"<li><b>Gender of Friends:</b> {model.GenderFreind}</li>");
                sb.Append($"<li><b>Count of Friends:</b> {model.CountOfFreind}</li>");
            }
            sb.Append($"<li><b>With Photographer:</b> {(model.WithPhotographer == true ? "Yes" : "No")}</li>");
            if (model.WithPhotographer == true)
            {
                sb.Append($"<li><b>Gender of Photographer:</b> {model.GenderPhotographer}</li>");
            }
            sb.Append("</ul>");

            if (model.ServiceList != null && model.ServiceList.Any())
            {
                sb.Append("<h3>Additional Services</h3>");
                sb.Append("<ul>");
                foreach (var service in model.ServiceList)
                {
                    sb.Append("<li>");
                    sb.Append($"<b>Service Name:</b> {service.serviceNameEn} <br>");
                    if (!string.IsNullOrEmpty(service.MainImage))
                    {
                        sb.Append($"<b>Image:</b><br><img src='{service.MainImage}' style='max-width: 300px; max-height: 200px;' /> <br>");
                    }
                    sb.Append("</li>");
                }
                sb.Append("</ul>");
            }
            else
            {
                sb.Append("<h3>No additional services were added.</h3>");
            }

            return sb.ToString();
        }

       

        
    



    }
}