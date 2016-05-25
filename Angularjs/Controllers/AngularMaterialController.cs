﻿using Angularjs.DataModel;
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
    public class AngularMaterialController : Controller
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
            var response = context.Employees.Where(x => x.IsDeleted == null).ToList();
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
            Employee angularTable = new Employee();
            angularTable.FirstName = res.Name;
            angularTable.Age= res.RollNumber;
            angularTable.LastName= res.Trade;
            context.Employees.Add(angularTable);
            context.SaveChanges();

            return Content("Successfully Saved");
        }
        public ActionResult Edit(string str)
        {
            var id = Convert.ToInt16(str);
            WebApiPracticeEntities context = new WebApiPracticeEntities();
            var response = context.Employees.Where(x => x.ID == id).FirstOrDefault();
            return Json(response, JsonRequestBehavior.AllowGet);
        }
        public ActionResult Delete(string str)
        {
            var id = Convert.ToInt16(str);
            WebApiPracticeEntities context = new WebApiPracticeEntities();
            var response = context.Employees.Where(x => x.ID == id).FirstOrDefault();
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
            Employee angulartable = new Employee();
            var res = context.Employees.Where(x => x.ID == id).FirstOrDefault();
            res.FirstName = m.Name;
            res.LastName = m.RollNumber;
            res.Age = m.Trade;
            context.SaveChanges();


            return View();
        }

        public ActionResult AngularJsButtonLoader()
        {
            WebApiPracticeEntities context = new WebApiPracticeEntities();
            var result = context.Employees.Where(x => x.FirstName == "test").Select(x=> x.ID).ToList();

            return Json(result, JsonRequestBehavior.AllowGet);
        }


        public ActionResult Typeahead()
        {
            WebApiPracticeEntities context= new WebApiPracticeEntities();
            var model=context.Employees.ToList();
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

                    Employee employee = new Employee
                    {
                        FirstName = actualFileName,
                        LastName = fileName,
                        Age = description,
                         City =Convert.ToString(size)
                    };
                    using (WebApiPracticeEntities dc = new WebApiPracticeEntities())
                    {
                        dc.Employees.Add(employee);
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
