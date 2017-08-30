using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MvcApplication1.Models;

namespace MvcApplication1.Controllers
{
    public class ItemController : Controller
    {
        private Model1Container db = new Model1Container();

        //
        // GET: /Item/Details/5

        public ActionResult Details(int id = 0)
        {
            Item item = db.ItemSet.Find(id);
            if (item == null)
            {
                return HttpNotFound();
            }
            return View(item);
        }

        //
        // GET: /Item/Create

        public ActionResult Create(int id=0)
        {
            Item it = new Item();
            it.ExpertSystemId = id;
            it.ExpertSystem = db.ExpertSystemSet.First(x => x.Id == id);
            return View(it);
        }

        //
        // POST: /Item/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Item item, List<string>props, int id=0)
        {
            if (ModelState.IsValid)
            {//установка ID системы
                item.ExpertSystemId = id;
                db.SaveChanges();
                ExpertSystem esys = db.ExpertSystemSet.First(es => es.Id == id);
                item.ExpertSystem =  esys;
                db.SaveChanges();
                for (int i=0;i<props.Count();i++)
                {   Note note = new Note{ValId=int.Parse(props[i])};
                    db.NoteSet.Add(note);
                    db.SaveChanges();
                    item.Note.Add(note);
                    db.SaveChanges();
                }
                db.ItemSet.Add(item);
                db.SaveChanges();
                return RedirectToAction("Menu", "ExpertSystem", new { id = id });
            }

            return View(item);
        }

        //
        // GET: /Item/Edit/5

        public ActionResult Edit(int id = 0)
        {
            Item item = db.ItemSet.Find(id);
            if (item == null)
            {
                return HttpNotFound();
            }
            return View(item);
        }

        //
        // POST: /Item/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Item item,List<string> props)
        {
            if (ModelState.IsValid)
            {  var it = db.ItemSet.Find(item.Id);
               it.Name = item.Name;
               var Allprops = it.ExpertSystem.Property;
               if (Allprops.Count() == it.Note.Count())
               {
                   for (int i = 0; i < props.Count(); i++)
                   {
                       it.Note.ElementAt(i).ValId = int.Parse(props[i]);
                   }
               }
               else
               {  
                    if (item.Note.Count() != 0)
            {
                var notes = item.Note.ToList();

                for (int i = 0; i < notes.Count(); i++)
                {
                    db.NoteSet.Remove(notes[i]);
                    db.SaveChanges();
                }
                    }
    
            
                   for (int i = 0; i < props.Count(); i++)
                   {
                       Note note = new Note { ValId = int.Parse(props[i]) };
                           db.NoteSet.Add(note);
                           db.SaveChanges();
                           it.Note.Add(note);
                           db.SaveChanges();
                       }
                   }
               
                db.Entry(it).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Menu", "ExpertSystem", new { id = (int)it.ExpertSystemId});
            }
            return View(item);
        }

        //
        // GET: /Item/Delete/5

        public ActionResult Delete(int id = 0)
        {
            Item item = db.ItemSet.Find(id);
            if (item == null)
            {
                return HttpNotFound();
            }
            return View(item);
        }

        //
        // POST: /Item/Delete/5

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Item item = db.ItemSet.Find(id);
            var curExpSystemId = item.ExpertSystemId;
            DeleteItem(item);
            return RedirectToAction("Menu", "ExpertSystem", new { id =(int)curExpSystemId });
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }

        public void DeleteItem(Item item)
        {
            if (item.Note.Count() != 0)
            {
                var notes = item.Note.ToList();

                for (int i = 0; i < notes.Count(); i++)
                {
                    db.NoteSet.Remove(notes[i]);
                    db.SaveChanges();
                }
    
            }

            db.ItemSet.Remove(item);
            db.SaveChanges();
        }
    }
}