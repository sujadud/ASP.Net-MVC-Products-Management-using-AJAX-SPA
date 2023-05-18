using Master_Details.Models;
using Monthly_Final_Maser_Details.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Master_Details.Controllers
{
    public class MvcAjaxController : Controller
    {
        // GET: MvcAjax
        [HttpGet]
        public ActionResult Single(int? id)
        {
            var db = new DatabaseEntities();
            VmSale oSale = null;
            var oSM = db.SaleMasters.Where(x => x.SaleId == id).FirstOrDefault();
            if (oSM != null)
            {
                oSale = new VmSale();
                oSale.CreateDate = oSM.CreateDate;
                oSale.CustomerAddress = oSM.CustomerAddress;
                oSale.CustomerName = oSM.CustomerName;
                oSale.SaleId = oSM.SaleId;
                var listSaleDetail = new List<VmSale.VmSaleDetail>();
                var listSD = db.SaleDetails.Where(x => x.SaleId == id).ToList();
                foreach (var oSD in listSD)
                {
                    var oSaleDetail = new VmSale.VmSaleDetail();
                    oSaleDetail.Price = oSD.Price;
                    oSaleDetail.ProductName = oSD.ProductName;
                    oSaleDetail.Qty = oSD.Qty;
                    oSaleDetail.SaleDetailId = oSD.SaleDetailId;
                    oSaleDetail.SaleId = oSD.SaleId;
                    listSaleDetail.Add(oSaleDetail);
                }
                oSale.SaleDetails = listSaleDetail;
            }
            oSale = oSale == null ? new VmSale() : oSale;
            ViewData["List"] = db.SaleMasters.ToList();
            return View(oSale);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Single(VmSale model, HttpPostedFileBase img)
        {
            string fileName = "";
            if (img != null)
            {
                fileName = img.FileName;
                string picture = System.IO.Path.GetFileName(img.FileName);
                string path = System.IO.Path.Combine(Server.MapPath("~/Picture"), picture);
                img.SaveAs(path);
            }
            var db = new DatabaseEntities();
            var oSaleMaster = db.SaleMasters.Find(model.SaleId);
            if (oSaleMaster == null)
            {
                #region insert
                oSaleMaster = new SaleMaster();
                oSaleMaster.CreateDate = model.CreateDate;
                oSaleMaster.CustomerAddress = model.CustomerAddress;
                oSaleMaster.CustomerName = model.CustomerName;
                model.AttachmentURL = "/Picture/" + fileName;
                db.SaleMasters.Add(oSaleMaster);
                
                ViewBag.Message = "inserted successfully.";
                #endregion
            }
            else
            {
                #region update
                oSaleMaster.CreateDate = model.CreateDate;
                oSaleMaster.CustomerAddress = model.CustomerAddress;
                oSaleMaster.CustomerName = model.CustomerName;
                oSaleMaster.Photo = "/Picture/" + fileName;
                ViewBag.Message = "updated successfully.";
                var listSaleDetailRem = db.SaleDetails.Where(x => x.SaleId == oSaleMaster.SaleId).ToList();
                db.SaleDetails.RemoveRange(listSaleDetailRem);
                #endregion
            }
            db.SaveChanges();
            var listSaleDetail = new List<SaleDetail>();
            for (var i = 0; i < model.ProductName.Length; i++)
            {
                if (!string.IsNullOrEmpty(model.ProductName[i]))
                {
                    var oSaleDetail = new SaleDetail();
                    oSaleDetail.Price = model.Price[i];
                    oSaleDetail.ProductName = model.ProductName[i];
                    oSaleDetail.Qty = model.Qty[i];
                    oSaleDetail.SaleId = oSaleMaster.SaleId;
                    listSaleDetail.Add(oSaleDetail);
                }
                else
                {
                    ViewBag.Message = "Product name or attachment required.";
                }
            }
            db.SaleDetails.AddRange(listSaleDetail);
            db.SaveChanges();
            return RedirectToAction("Single");
        }

        [HttpGet]
        public ActionResult SingleDelete(int id)
        {
            var db = new DatabaseEntities();
            var oSaleMaster = (from o in db.SaleMasters where o.SaleId == id select o).FirstOrDefault();
            if (oSaleMaster != null)
            {
                db.SaleMasters.Remove(oSaleMaster);
                db.SaveChanges();
            }
            return RedirectToAction("Single");
        }
    }
}