using Angularjs.DataModel;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace Angularjs.Controllers
{
    public class AngularJSController : Controller
    {


        //
        // GET: /AngularJS/

        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Display()
        {
            WebApiPracticeEntities context = new WebApiPracticeEntities();
            var response = context.EmployeeDetails.Where(x => x.IsDeleted == null).ToList();
            return Json(response, JsonRequestBehavior.AllowGet);

        }
        public class StudentRecord
        {
            public string Name { get; set; }
            public string RollNumber { get; set; }
            public string Trade { get; set; }

        }
        public ActionResult Save(string stri)
        {
            var res = JsonConvert.DeserializeObject<StudentRecord>(stri);
            WebApiPracticeEntities context = new WebApiPracticeEntities();
            EmployeeDetail angularTable = new EmployeeDetail();
            angularTable.Name = res.Name;
            angularTable.Age= res.RollNumber;
            angularTable.City= res.Trade;
            context.EmployeeDetails.Add(angularTable);
            context.SaveChanges();

            return Content("Successfully Saved");
        }
        public ActionResult Edit(string str)
        {
            var id = Convert.ToInt16(str);
            WebApiPracticeEntities context = new WebApiPracticeEntities();
            var response = context.EmployeeDetails.Where(x => x.ID == id).FirstOrDefault();
            return Json(response, JsonRequestBehavior.AllowGet);
        }
        public ActionResult Delete(string str)
        {
            var id = Convert.ToInt16(str);
            WebApiPracticeEntities context = new WebApiPracticeEntities();
            var response = context.EmployeeDetails.Where(x => x.ID == id).FirstOrDefault();
            if (response != null)
            
                response.IsDeleted =true;
            context.SaveChanges();
            return RedirectToAction("Index");
            
            
        }
        public ActionResult Update(string str,string data)
        {
            var id = Convert.ToInt16(str);
            var m = JsonConvert.DeserializeObject<StudentRecord>(data);
            WebApiPracticeEntities context = new WebApiPracticeEntities();
            EmployeeDetail angulartable = new EmployeeDetail();
            var res = context.EmployeeDetails.Where(x => x.ID == id).FirstOrDefault();
            res.Name = m.Name;
            res.Age = m.RollNumber;
            res.Age = m.Trade;
            context.SaveChanges();


            return View();
        }

        public ActionResult AngularJsButtonLoader()
        {
            WebApiPracticeEntities context = new WebApiPracticeEntities();
            var result = context.EmployeeDetails.Where(x => x.Name == "test").Select(x=> x.ID).ToList();

            return Json(result, JsonRequestBehavior.AllowGet);
        }


        public ActionResult Typeahead()
        {
            WebApiPracticeEntities context= new WebApiPracticeEntities();
            var model=context.EmployeeDetails.ToList();
            return Json(model, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult SaveFiles(string description)
        {
            string Message, fileName, actualFileName;
            Message = fileName = actualFileName = string.Empty;
            bool flag = false;
            if (Request.Files != null)
            {
                var file = Request.Files[0];
                actualFileName = file.FileName;
                fileName = Guid.NewGuid() + Path.GetExtension(file.FileName);
                int size = file.ContentLength;

                try
                {

                    //uploadedFiles is the name of the folder
                    file.SaveAs(Path.Combine(Server.MapPath("~/UploadedFiles"), fileName));

                    EmployeeDetail employee = new EmployeeDetail
                    {
                      Name = actualFileName,
                        City = fileName,
                        Age = description,
                         Email =Convert.ToString(size)
                    };
                    using (WebApiPracticeEntities dc = new WebApiPracticeEntities())
                    {
                        dc.EmployeeDetails.Add(employee);
                        dc.SaveChanges();
                        Message = "File uploaded successfully";
                        flag = true;
                    }
                }
                catch (Exception)
                {
                    Message = "File upload failed! Please try again";
                }

            }
            return new JsonResult { Data = new { Message = Message, Status = flag } };
        }

        public ActionResult Part8() // Upload File with Data
        {
            return View();
        }

    }
}
