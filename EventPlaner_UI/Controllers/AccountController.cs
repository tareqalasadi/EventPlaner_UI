using EventPlaner_UI.Helper;
using EventPlaner_UI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using System.Reflection;
using System.Text;

namespace EventPlaner_UI.Controllers
{
    public class AccountController : Controller
    {
        private readonly IConfiguration _IConfiguration;
        public AccountController(IConfiguration configuration)
        {
            _IConfiguration = configuration;    
        }
        #region Get 
        public IActionResult Index()
        {
            return View();
        }
        public async Task<IActionResult> GetAllService()
        {
            var GetServiceAPI = _IConfiguration["BaseWebAPIURL"] + "Services/All";
            var responseTask = await ManageAPI.GetURI(new Uri(GetServiceAPI));
            var services = JsonConvert.DeserializeObject<List<Service>>(responseTask);
            return View(services);
        }
        public IActionResult AddService()
        {
            return View();
        }
        public async Task<IActionResult> AddCategoryOfService()
        {
            var getServiceAPI = _IConfiguration["BaseWebAPIURL"] + "Services/All";
            var responseTask = await ManageAPI.GetURI(new Uri(getServiceAPI));
            var services = JsonConvert.DeserializeObject<List<Service>>(responseTask);

            var viewModel = new CategoryOfServiceViewModel
            {
                Services = services.Select(s => new SelectListItem
                {
                    Value = s.ID.ToString(),
                    Text = s.ServiceNameEn
                }).ToList()
            };

            return View(viewModel);
        }
        public async Task<IActionResult> AddDirectImage()
        {
            var getServiceAPI = _IConfiguration["BaseWebAPIURL"] + "Services/All";
            var responseTask = await ManageAPI.GetURI(new Uri(getServiceAPI));
            var services = JsonConvert.DeserializeObject<List<Service>>(responseTask);

            var viewModel = new DirectImageVeiwModel
            {
                Services = services.Select(s => new SelectListItem
                {
                    Value = s.ID.ToString(),
                    Text = s.ServiceNameEn
                }).ToList()
            };

            return View(viewModel);
        }

        public async Task<IActionResult> AddPartyServices()
        {
            var getServiceAPI = _IConfiguration["BaseWebAPIURL"] + "Services/All";
            var responseTask = await ManageAPI.GetURI(new Uri(getServiceAPI));
            var services = JsonConvert.DeserializeObject<List<Service>>(responseTask);

            var viewModel = new PartyServicesViewModel
            {
                Services = services.Select(s => new SelectListItem
                {
                    Value = s.ID.ToString(),
                    Text = s.ServiceNameEn
                }).ToList()
            };

            return View(viewModel);
        }

        public IActionResult UpdateService(long id)
        {
            var GetServiceAPI = _IConfiguration["BaseWebAPIURL"] + "Services/GetService/" + id;
            var responseTask = Task.Run(() => ManageAPI.GetURI(new Uri(GetServiceAPI)));
            responseTask.Wait();

            if (responseTask.IsCompleted && !string.IsNullOrEmpty(responseTask.Result))
            {
                var service = JsonConvert.DeserializeObject<Service>(responseTask.Result);
                return View(service);
            }
            return NotFound();
        }

        #endregion

        #region Post
        [HttpPost]
        public async Task<IActionResult> AddService(Service service)
        {
            if (service != null)
            {
                if (service.Images != null)
                {
                    using (var memoryStream = new MemoryStream())
                    {
                        await service.Images.CopyToAsync(memoryStream);
                        service.Image = memoryStream.ToArray();
                    }
                }

                var getAllServicesAPI = _IConfiguration["BaseWebAPIURL"] + "Services/AddService";
                var jsonInsert = JsonConvert.SerializeObject(service);
                var content = new StringContent(jsonInsert, Encoding.UTF8, "application/json");

                var addTask = Task.Run(() => ManageAPI.PostURI(new Uri(getAllServicesAPI), content));
                addTask.Wait();

                if (addTask.IsCompleted && !string.IsNullOrEmpty(addTask.Result))
                {
                    return RedirectToAction("GetAllService");
                }
            }

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddCategoryOfService(CategoryOfServiceViewModel CategoryOfService)
        {
            if (CategoryOfService != null)
            {
                try
                {
                    // Handle image uploading
                    if (CategoryOfService.Images != null && CategoryOfService.Images.Length > 0)
                    {
                        using (var memoryStream = new MemoryStream())
                        {
                            await CategoryOfService.Images.CopyToAsync(memoryStream);
                            CategoryOfService.Image = memoryStream.ToArray();
                        }
                    }

                    // Fetch the Service details using ServiceId
                    var getServiceAPI = _IConfiguration["BaseWebAPIURL"] + "Services/GetService/" + CategoryOfService.ServiceId;
                    var responseTask = await ManageAPI.GetURI(new Uri(getServiceAPI));
                    var service = JsonConvert.DeserializeObject<ServiceModel>(responseTask);

                    if (service != null)
                    {


                        // Construct CategoryOfService object
                        var categoryOfService = new CategoryOfService()
                        {
                            ID = 0,
                            CategoryNameAr = CategoryOfService.CategoryNameAr,
                            CategoryNameEn = CategoryOfService.CategoryNameEn,
                            DescriptionAr = CategoryOfService.DescriptionAr,
                            DescriptionEn = CategoryOfService.DescriptionEn,
                            ServiceId = CategoryOfService.ServiceId,
                            Image = CategoryOfService.Image,
                            //Service = new ServiceModel // Assign a new Service object or use the fetched one
                            //{
                            //    // Populate Service details
                            //    ServiceNameAr = service.ServiceNameAr,
                            //    ServiceNameEn = service.ServiceNameEn,
                            //    DescriptionAr = service.DescriptionAr,
                            //    DescriptionEn = service.DescriptionEn,
                            //    Image = service.Image,
                            //}
                        };


                        // Post the data to your API
                        var addCategoryOfServiceAPI = _IConfiguration["BaseWebAPIURL"] + "Services/AddCategoryOfService";
                        var jsonInsert = JsonConvert.SerializeObject(categoryOfService);
                        var content = new StringContent(jsonInsert, Encoding.UTF8, "application/json");

                        var response = await ManageAPI.PostURI(new Uri(addCategoryOfServiceAPI), content);

                        if (!string.IsNullOrEmpty(response))
                        {
                            return RedirectToAction("GetAllService");
                        }
                        else
                        {
                            ModelState.AddModelError("", "Failed to add Category of Service.");
                        }
                    }
                    else
                    {
                        ModelState.AddModelError("Service", "The specified Service does not exist or is invalid.");
                    }
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", $"Error: {ex.Message}");
                }
            }
            else
            {
                ModelState.AddModelError("", "Category of Service data is invalid.");
            }

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddDirectImage(DirectImageVeiwModel directImage)
        {
            if (directImage != null)
            {
                try
                {
                    // Handle image uploading
                    if (directImage.Images != null && directImage.Images.Length > 0)
                    {
                        using (var memoryStream = new MemoryStream())
                        {
                            await directImage.Images.CopyToAsync(memoryStream);
                            directImage.Image = memoryStream.ToArray();
                        }
                    }

                    var addCategoryOfServiceAPI = _IConfiguration["BaseWebAPIURL"] + "Services/AddDirectImage";
                    var jsonInsert = JsonConvert.SerializeObject(directImage);
                    var content = new StringContent(jsonInsert, Encoding.UTF8, "application/json");

                    var response = await ManageAPI.PostURI(new Uri(addCategoryOfServiceAPI), content);

                    if (!string.IsNullOrEmpty(response))
                    {
                        return RedirectToAction("GetAllService");
                    }


                    else
                    {
                        ModelState.AddModelError("Service", "The specified Service does not exist or is invalid.");
                    }
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", $"Error: {ex.Message}");
                }
            }
            else
            {
                ModelState.AddModelError("", "Category of Service data is invalid.");
            }

            return View();
        }
        [HttpPost]
        public async Task<IActionResult> AddPartyServices(PartyServicesViewModel partyServices)
        {
            if (partyServices != null)
            {
                try
                {
                    byte[] mainImageBytes = null;
                    if (partyServices.MainImage != null)
                    {
                        using (var memoryStream = new MemoryStream())
                        {
                            await partyServices.MainImage.CopyToAsync(memoryStream);
                            mainImageBytes = memoryStream.ToArray();
                        }
                    }

                    var imageListBytes = new List<byte[]>();
                    if (partyServices.ImageList != null && partyServices.ImageList.Count > 0)
                    {
                        foreach (var file in partyServices.ImageList)
                        {
                            using (var memoryStream = new MemoryStream())
                            {
                                await file.CopyToAsync(memoryStream);
                                imageListBytes.Add(memoryStream.ToArray());
                            }
                        }
                    }

                    // Serialize image list to byte array
                    byte[] imageListSerializedBytes = null;
                    if (imageListBytes.Count > 0)
                    {
                        var jsonString = JsonConvert.SerializeObject(imageListBytes);
                        imageListSerializedBytes = Encoding.UTF8.GetBytes(jsonString);
                    }

                    var partyService = new PartyServices
                    {
                        ServiceNameAr = partyServices.ServiceNameAr,
                        ServiceNameEn = partyServices.ServiceNameEn,
                        DescriptionEn = partyServices.DescriptionEn,
                        DescriptionAr = partyServices.DescriptionAr,
                        TypeService = partyServices.TypeService,
                        MainImage = mainImageBytes,
                        ImageListSerialized = imageListSerializedBytes,
                        ServiceId = partyServices.ServiceId,
                        IsPhotographerIncluded = partyServices.IsPhotographerIncluded,
                        IsZafaIncluded = partyServices.IsZafaIncluded,
                        IsSweetCollectionIncluded = partyServices.IsSweetCollectionIncluded,
                        IsFriendAssistanceNeeded = partyServices.IsFriendAssistanceNeeded,
                        IsCakeIncluded = partyServices.IsCakeIncluded,
                        IsFoodIncluded = partyServices.IsFoodIncluded,
                        IsBridalBouquetIncluded = partyServices.IsBridalBouquetIncluded,
                        IsFlowerArrangementIncluded = partyServices.IsFlowerArrangementIncluded,
                        Price = partyServices.Price
                    };

                    var addPartyServiceAPI = _IConfiguration["BaseWebAPIURL"] + "Services/InsertPartyService";
                    var jsonInsert = JsonConvert.SerializeObject(partyService);
                    var content = new StringContent(jsonInsert, Encoding.UTF8, "application/json");

                    var response = await ManageAPI.PostURI(new Uri(addPartyServiceAPI), content);

                    if (!string.IsNullOrEmpty(response))
                    {
                        return RedirectToAction("GetAllService");
                    }
                    else
                    {
                        ModelState.AddModelError("Service", "The specified Service does not exist or is invalid.");
                    }
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", $"Error: {ex.Message}");
                }
            }
            else
            {
                ModelState.AddModelError("", "PartyService data is invalid.");
            }

            return View();
        }





        [HttpPost]
        public async Task<IActionResult> UpdateService(Service service)
        {
            if (service != null)
            {
                if (service.Images != null)
                {
                    using (var memoryStream = new MemoryStream())
                    {
                        await service.Images.CopyToAsync(memoryStream);
                        service.Image = memoryStream.ToArray();
                    }
                }

                var updateServiceAPI = _IConfiguration["BaseWebAPIURL"] + "Services/UpdateService";
                var _jsonUpdate = JsonConvert.SerializeObject(service);
                var content = new StringContent(_jsonUpdate, Encoding.UTF8, "application/json");

                var updateTask = Task.Run(() => ManageAPI.PostURI(new Uri(updateServiceAPI), content));
                updateTask.Wait();

                if (updateTask.IsCompleted && !string.IsNullOrEmpty(updateTask.Result))
                {
                    return RedirectToAction("GetAllService"); // Assuming you have an Index action to list services
                }
            }
            return View(service);
        }
        [HttpPost]
        public async Task<IActionResult> DeleteService(long id)
        {
            var deleteServiceAPI = _IConfiguration["BaseWebAPIURL"] + "Services/DeleteService/" + id;
            var content = new StringContent("", Encoding.UTF8, "application/json"); // Providing an empty content
            var responseTask = await ManageAPI.PostURI(new Uri(deleteServiceAPI), content);

            if (!string.IsNullOrEmpty(responseTask))
            {
                return Json(new { success = true });
            }
            return Json(new { success = false });
        }

        #endregion

    }
}
