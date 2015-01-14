using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using TimeSeries.Domain.Abstract;
using TimeSeries.Domain.Entities;
using TimeSeries.WebUI.Infrastructure;
using TimeSeries.WebUI.Models;

namespace TimeSeries.WebUI.Controllers
{
    [Authorize]
    public class TimeSerieController : Controller
    {
        private ITimeSeriesRepository repository;

        public TimeSerieController(ITimeSeriesRepository timeSeriesRepository)
        {
            this.repository = timeSeriesRepository;
        }

        public ViewResult Create()
        {
            return View("Edit", new TimeSerie());
        }

        public ActionResult Edit(int timeSerieId)
        {
            TimeSerie timeSerie = repository.TimeSeries.FirstOrDefault(p => p.TimeSerieId == timeSerieId
                && p.User.Id == currentUser.Id);
            if (timeSerie == null)
            {
                return new NotFoundResult();
            }
            return View(timeSerie);
        }

        [HttpPost]
        public ActionResult DownloadFile(HttpPostedFileBase data)
        {
            if (data != null)
            {
                string vectorData = new StreamReader(data.InputStream).ReadToEnd();
                if (!String.IsNullOrWhiteSpace(vectorData))
                {
                    TimeSerie timeSerie = new TimeSerie();
                    timeSerie.VectorData = vectorData;//.Replace("\r\n", " ").Trim();
                    timeSerie.User_Id = currentUser.Id;
                    if (ModelState.IsValid)
                    {
                        repository.SaveTimeSerie(timeSerie);
                        TempData["success"] = String.Format("{0} файл обработан", DateTime.Now.ToLongTimeString());
                    }
                }
                else
                {
                    TempData["message"] = String.Format("{0} ошибка при обработке файла (пустой файл)", DateTime.Now.ToLongTimeString());
                }
            }
            else
            {
                TempData["warning"] = String.Format("{0} не выбран файл для загрузки", DateTime.Now.ToLongTimeString());
            }
            return RedirectToAction("List");
        }

        [HttpPost]
        public ActionResult Edit(TimeSerie timeSerie)
        {
            if (timeSerie.TimeSerieId == 0)
            {
                timeSerie.User_Id = currentUser.Id;
            }
            if (ModelState.IsValid)
            {
                repository.SaveTimeSerie(timeSerie);
                TempData["success"] = String.Format("{0} временной ряд сохранен", DateTime.Now.ToLongTimeString());
                return RedirectToAction("List");
            }
            else
            {
                return View(timeSerie);
            }

        }

        [HttpPost]
        public ActionResult Delete(int timeSerieId)
        {
            TimeSerie timeSerie = repository.DeleteTimeSerie(timeSerieId, currentUser.Id);
            if (timeSerie != null)
            {
                TempData["success"] = String.Format("{0} временной ряд удален", DateTime.Now.ToLongTimeString());
            }
            return RedirectToAction("List");

        }

        public ViewResult List()
        {
            return View(repository.TimeSeries.Where(p => p.User.Id == currentUser.Id));
        }

        public ViewResult Visualize()
        {
            return View(repository.TimeSeries.Where(p => p.User.Id == currentUser.Id));
        }

        [HttpPost]
        public ActionResult Visualize(int[] timeSeries)
        {
            if (Request.IsAjaxRequest())
            {
                Dictionary<string, ArrayList> timeSerieList = new Dictionary<string, ArrayList>();
                if (timeSeries != null && timeSeries.Length > 0)
                {
                    foreach (var id in timeSeries)
                    {
                        TimeSerie timeSerie = repository.TimeSeries.FirstOrDefault(p => p.TimeSerieId == id
                        && p.User.Id == currentUser.Id);
                        if (timeSerie != null)
                        {
                            string key = "VectorData" + timeSerie.TimeSerieId.ToString();
                            if (!timeSerieList.ContainsKey(key))
                            {
                                ArrayList arr = new ArrayList();
                                arr.Add(key);
                                arr.AddRange(timeSerie.VectorData.ToStringArray().ToDouble());
                                timeSerieList.Add(key, arr);
                                //.ToDoubleString();
                            }
                        }
                    }

                    return Json(timeSerieList.Values);
                }
                else
                {
                    return RedirectToAction("Visualize");
                }
            }
            return RedirectToAction("Visualize");
        }

        public ActionResult Phase(int timeSerieId)
        {
            TimeSerie timeSerie = repository.TimeSeries.FirstOrDefault(p => p.TimeSerieId == timeSerieId
                        && p.User.Id == currentUser.Id);
            if (timeSerie == null)
            {
                return new NotFoundResult();                
            }

            PhaseModel phase = new PhaseModel { TimeSerie = timeSerie, Tau = 2 };
            return View(phase);
        }


        [HttpPost]
        public ActionResult Phase(int timeSerieId, int Tau = 1)
        {
            TimeSerie timeSerie = repository.TimeSeries.FirstOrDefault(p => p.TimeSerieId == timeSerieId
                        && p.User.Id == currentUser.Id);
            PhaseModel phase = new PhaseModel { TimeSerie = timeSerie, Tau = Tau };
            if (Request.IsAjaxRequest())
            {
                var vectorData = timeSerie.VectorData.ToStringArray().Select(e => new { value = e.ToDouble() }).ToList();
                var phaseTimeSerie = new
                {
                    x = new List<double>(),
                    y = new List<double>(),
                    z = new List<double>()
                };
                for (int i = 0; i < vectorData.Count - Tau; i++)
                {
                    phaseTimeSerie.x.Add(vectorData[i].value);
                    phaseTimeSerie.y.Add(vectorData[i + Tau].value);
                    phaseTimeSerie.z.Add(Math.Pow(phaseTimeSerie.y.Last(), 2));
                }
                return Json(phaseTimeSerie);
            }
            return View(phase);
        }

        private AppUserManager UserManager
        {
            get
            {
                return HttpContext.GetOwinContext().GetUserManager<AppUserManager>();
            }
        }

        private AppUser currentUser
        {
            get
            {
                return UserManager.FindById(User.Identity.GetUserId());
            }
        }
    }
}