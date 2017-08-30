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
    public class PropertyController : Controller
    {
        private Model1Container db = new Model1Container();

        //
        // GET: /Property/

        public ActionResult ToRoot(int id =0)
        {

            Property property = db.PropertySet.Find(id);
            if (property != null)
            {
                if (property.Question.Count() > 0)
                {
                    var quest = property.Question.Where(q => q.Deep == 1).ElementAt(0);
                    return RedirectToAction("Edit", "Question", new { id = quest.Id });
                }
                else
                { return RedirectToAction("Menu", "ExpertSystem", new { id = (int)property.ExpertSystemId }); }
            }
                return HttpNotFound();
        }

        //
        // GET: /Property/Details/5

        public ActionResult Details(int id = 0)
        {
            Property property = db.PropertySet.Find(id);
            if (property == null)
            {
                return HttpNotFound();
            }
            ViewBag.CurrentExpSystemId = property.ExpertSystemId;
            return View(property);
        }

        //
        // GET: /Property/Create

        public ActionResult Create(int id=0)
        {
            Property pr = new Property();
            pr.ExpertSystemId = id;
            return View(pr);
        }

        //
        // POST: /Property/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Property property, int id=0)
        {
            if (ModelState.IsValid)
            {//ID
                property.ExpertSystemId = id;
                property.ExpertSystem = db.ExpertSystemSet.Find(id);
             //number in list
                if (db.PropertySet.Count() == 0)
                    property.NumberInList = 1;
                else
                    property.NumberInList = db.PropertySet.ToList().Last().NumberInList+1;
            //root question deep
                property.RootQdeep = 0;
                db.PropertySet.Add(property);
                db.SaveChanges();
                return RedirectToAction("Menu", "ExpertSystem", new { id = id });
            }

            return View(property);
        }

        //
        // GET: /Property/Edit/5

        public ActionResult Edit(int id = 0)
        {
            Property property = db.PropertySet.Find(id);
            if (property == null)
            {
                return HttpNotFound();
            }
            return View(property);
        }

        //
        // POST: /Property/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Property property,string action="",string newvalname="")
        {
           Property newproperty = db.PropertySet.Find(property.Id);
            if (action == "AddValue")
            {
                if (newvalname != "")
                {
                    Val val = new Val { PropertyId = property.Id, Mean = newvalname };
                    db.ValSet.Add(val);
                    db.SaveChanges();               
                }

            }
            else
            {
                if (ModelState.IsValid)
                {
                    newproperty.Name = property.Name;
                    db.SaveChanges();
                }
            }
            
            return View(newproperty);
        }

        public ActionResult DeleteValue(int valueid)
        {  
            Val val = db.ValSet.Find(valueid);
            int propid = (int)val.PropertyId;
            DeleteVal(val);
            return RedirectToAction("Edit", "Property", new { id = propid });
        }

        //
        // GET: /Property/Delete/5

        public ActionResult Delete(int id = 0)
        {
            Property property = db.PropertySet.Find(id);
            if (property == null)
            {
                return HttpNotFound();
            }
            ViewBag.CurrentExpSystemId = property.ExpertSystemId;
            return View(property);
        }

        //
        // POST: /Property/Delete/5

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Property property = db.PropertySet.Find(id);
            var expsysId = property.ExpertSystemId;
            DeleteProperty(property);
            return RedirectToAction("Menu", "ExpertSystem", new { id = (int)expsysId });
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }


        #region DeleteFuncs
        public void DeleteProperty(Property property)
        {
            //если со свойством связано дерево вопросов
            if (property.Question.Count() != 0)
            {
                //удаляем связанное дерево вопросов
                var RootQ = db.QuestionSet.Where(q => q.Deep == 1).First(q => q.PropertyId == property.Id);
                DeleteBranchByQuestion(RootQ);
            }

            //удаляем связанные значения свойств и соответствующие, записи и селекторы
            DeleteAllVals(property);

            //удаляем само свойство
            db.PropertySet.Remove(property);
            db.SaveChanges();

        }

        public void DeleteAllVals(Property property)
        {
            if (property.Val.Count() != 0)
            {
                var vals = property.Val.ToList();
                for (int i = 0; i < vals.Count(); i++)
                    DeleteVal(vals[i]);
            }

        }

        public void DeleteVal(Val val)
        { //сначала удаляем все связанные со значением записи и селекторы

            if (val.SelectorSet.Count() != 0)
            {
                var selectors = val.SelectorSet.ToList();

                for (int i = 0; i < selectors.Count(); i++)
                {
                    db.SelectorSet.Remove(selectors[i]);
                    db.SaveChanges();
                }
            }
            if (val.Note.Count() != 0)
            {
                var notes = val.Note.ToList();

                for (int i = 0; i < notes.Count(); i++)
                {
                    db.NoteSet.Remove(notes[i]);
                    db.SaveChanges();
                }
            }

            db.ValSet.Remove(val);
            db.SaveChanges();
        }

        public void DeleteBranchByQuestion(Question question)
        {
            if (question.ChildAnswer.Count() != 0)
            {
                var CAnswers = question.ChildAnswer.ToList();

                for (int j = 0; j < CAnswers.Count(); j++)
                {   //все вложенные вопросы

                    if (CAnswers[j].ChildQuestion.Count() != 0)
                    {
                        var CQuestions = CAnswers[j].ChildQuestion.ToList();

                        for (int i = 0; i < CAnswers.Count(); i++)
                            DeleteBranchByQuestion(CQuestions[i]);
                    }
                    //все вложенные ответы
                    DeleteSingleAnswer(CAnswers[j]);
                }
            }
            db.QuestionSet.Remove(question);
            db.SaveChanges();
            return;
        }

        public void DeleteSingleAnswer(Answer answer)
        {  //для всех вложенных ответов удаляем все связанные с ними селекторы
            var selectors = answer.SelectorSet.ToList();
            for (int i = 0; i < selectors.Count(); i++)
            {
                db.SelectorSet.Remove(selectors[i]);
                db.SaveChanges();
            }
            db.AnswerSet.Remove(answer);
            db.SaveChanges();
        }
        #endregion
    }
}