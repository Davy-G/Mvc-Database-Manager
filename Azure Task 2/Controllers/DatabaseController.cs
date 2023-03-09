using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Azure_Task_2.Models;
using System.IO;

namespace Azure_Task_2.Controllers
{
    public class DatabaseController : Controller
    {
        // GET: Database
        public ActionResult Index()
        {

            DataEntities _db = new DataEntities();

            return View(_db.Products.ToList());
        }

        public ActionResult OrderByName()
        {

            DataEntities _db = new DataEntities();

            return View("Index",_db.Products.OrderBy(e => e.Name));
        }

        public ActionResult OrderByPrice()
        {

            DataEntities _db = new DataEntities();

            return View("Index",_db.Products.OrderBy(e => e.Price));
        }

        public ActionResult OrderByDate()
        {

            DataEntities _db = new DataEntities();

            return View("Index", _db.Products.OrderBy(e => e.ModifiedDate));
        }

        public ActionResult OrderByColor()
        {

            DataEntities _db = new DataEntities();

            return View("Index", _db.Products.OrderBy(e => e.Color));
        }

        public ActionResult OrderByProductNum()
        {

            DataEntities _db = new DataEntities();

            return View("Index",_db.Products.OrderBy(e => e.ProductNumber));
        }

        public ActionResult OrderByWeight()
        {

            DataEntities _db = new DataEntities();

            return View("Index",_db.Products.OrderBy(e => e.Weight));
        }




        private void LogInAFile(string Action, string Values)

        {
            string StringToWrite = $"{DateTime.Now}, {Action}, {Values}" + "\n";
            if (!System.IO.File.Exists("ProductChangesLog.txt"))
            {
                StreamWriter fs = System.IO.File.CreateText("ProductChangesLog.txt");
                fs.Write(StringToWrite);
                fs.Dispose();
            }
            else
            {
                StreamWriter fs = System.IO.File.AppendText("ProductChangesLog.txt");
                fs.Write(StringToWrite);
                fs.Dispose();
            }
            


        }

        // GET: Database/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Database/Create
        [HttpPost]
        public ActionResult Create(Product product)
        {
                
                
                // TODO: Add insert logic here
                DataEntities _db = new DataEntities();
                _db.Products.Add(product);
                _db.UserChanges.Add(new UserChanx()
                {
                    OperationType = "Insert",
                    TableName = "Products",
                    Values = $"{product.Name},{product.Price},{product.Weight},{product.ProductNumber},{product.Color}"
                });
                _db.SaveChanges();
                LogInAFile("Create", $"{product.Name}," +
                    $"{product.Price}," +
                    $"{product.Weight}," +
                    $"{product.ProductNumber}," +
                    $"{product.Color}");
                return RedirectToAction("Index");
           
              
        }

        // GET: Database/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Database/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, Product data)
        {
            try
            {
                // TODO: Add update logic here

                DataEntities _db = new DataEntities();
                var Product = _db.Products.Find(id);
                Product.Color = data.Color;
                Product.ModifiedDate = DateTime.Now;
                Product.Name = data.Name;
                Product.Price = data.Price;
                Product.ProductNumber = data.ProductNumber;
                Product.Weight = data.Weight;

                _db.UserChanges.Add(new UserChanx()
                {
                    OperationType = "Update",
                    TableName = "Products",
                    Values = $"{data.Name},{data.Price},{data.Weight},{data.ProductNumber},{data.Color}",
                }); 
                _db.SaveChanges();
                LogInAFile("Edit", $"{Product.Name}," +
                   $"{Product.Price}," +
                   $"{Product.Weight}," +
                   $"{Product.ProductNumber}," +
                   $"{Product.Color}");

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Database/Delete/5
        public ActionResult Delete(int id)
        {
            using (DataEntities _db = new DataEntities())
            {

                var ThatProduct = _db.Products.Find(id);
                _db.Products.Remove(ThatProduct);

      

                var DeletedProduct = new DeletedProduct()
                {
                    DeletedDate = DateTime.Now,
                    Name = ThatProduct.Name,
                    Color = ThatProduct.Color,
                    Price = ThatProduct.Price,
                    ProductNumber = ThatProduct.ProductNumber,
                    Weight = ThatProduct.Weight
                };

                _db.DeletedProducts.Add(DeletedProduct);
                var Changed = new UserChanx()
                {
                    OperationType = "Delete",
                    TableName = "Products",
                    Values =
                     $"{ThatProduct.Name},{ThatProduct.Price},{ThatProduct.Weight},{ThatProduct.ProductNumber},{ThatProduct.Color}"
                };

                LogInAFile("Delete", $"{ThatProduct.Name}," +
                   $"{ThatProduct.Price}," +
                   $"{ThatProduct.Weight}," +
                   $"{ThatProduct.ProductNumber}," +
                   $"{ThatProduct.Color}");
                _db.UserChanges.Add(Changed);

                _db.SaveChanges();
                return RedirectToAction("index");
            }
           

        }

        public ActionResult UndeleteIndex()
        {

            DataEntities _db = new DataEntities();

            return View(_db.DeletedProducts.ToList());
        }


        public ActionResult Undelete(int id)
        {
            DataEntities _db = new DataEntities();

            var DeletedProduct = _db.DeletedProducts.Find(id);

            Product Restore = new Product() 
            {
                Color = DeletedProduct.Color,
                Name = DeletedProduct.Name,
                Price = DeletedProduct.Price,
                ProductNumber = DeletedProduct.ProductNumber,
                Weight = DeletedProduct.Weight,
            };


            _db.Products.Add(Restore);
            _db.DeletedProducts.Remove(DeletedProduct);
            _db.SaveChanges();
            LogInAFile("Restore", $"{Restore.Name}," +
                   $"{Restore.Price}," +
                   $"{Restore.Weight}," +
                   $"{Restore.ProductNumber}," +
                   $"{Restore.Color}");
            return RedirectToAction("UndeleteIndex");

        }

    }
}
