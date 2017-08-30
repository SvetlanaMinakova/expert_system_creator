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
    public class AnswerController : Controller
    {
        private Model1Container db = new Model1Container();

     
        //
        // GET: /Answer/Details/5

        public ActionResult Details(int id = 0)
        {
            Answer answer = db.AnswerSet.Find(id);
            if (answer == null)
            {
                return HttpNotFound();
            }
            return View(answer);
        }

        //
        // GET: /Answer/Create

        public ActionResult Create(int id=0)
        {
            Question curquest = db.QuestionSet.Find(id);
            Answer answ = new Answer();
            answ.QuestionId = id;
            answ.ParentQuestion = curquest;
            ViewBag.CurrentQuest = curquest.QuestionTxt;
            TempData["parentqid"] = id;
            TempData["expsysid"] = curquest.Property.ExpertSystemId;
            return View(answ);
        }

        //
        // POST: /Answer/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Answer answer, int id=0)
        {
            if (ModelState.IsValid)
            {
                answer.ParentQuestion = db.QuestionSet.First(q => q.Id == id);
                answer.QuestionId = id;
                db.AnswerSet.Add(answer);
                db.SaveChanges();
                return RedirectToAction("Edit","Question",new{id=(int)id});
            }

            ViewBag.QuestionId = new SelectList(db.QuestionSet, "Id", "QuestionTxt", (int)answer.QuestionId);
            return View(answer);
        }

        //
        // GET: /Answer/Edit/5

        public ActionResult Edit(int id = 0)
        {
            Answer answer = db.AnswerSet.Find(id);
            if (answer == null)
            {
                return HttpNotFound();
            }
            return View(answer);
        }

        //
        // POST: /Answer/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Answer answer, string newselvalid, string action = "", double newprobability = 0)
        {
            Answer newansw = db.AnswerSet.Find(answer.Id);
            if (action == "AddSelector")
            {
                if (newselvalid!=null && newprobability!=0)
                {
                    int valid = int.Parse(newselvalid);
                    SelectorSet selector = new SelectorSet{AnswerId=newansw.Id,Probability = newprobability/100,ValId=valid};
                    db.SelectorSet.Add(selector);
                    db.SaveChanges();
                }
            }

            else
            {
                if (ModelState.IsValid)
                {
                    newansw.Val = answer.Val;
                    db.SaveChanges();
                }
            }
            
            return View(newansw);
        }


        public ActionResult DeleteSelector(int selectorid = (-1),int answid=(-1))
        {
            Answer answer = db.AnswerSet.Find(answid);
            if (answer == null)
            {
                return HttpNotFound();
            }

            if (selectorid != (-1))
            {
                db.SelectorSet.Remove(db.SelectorSet.Find(selectorid));
                db.SaveChanges();
            }

            return RedirectToAction("Edit", new { id = answer.Id });
        }

        //
        // GET: /Answer/Delete/5

        public ActionResult Delete(int id = 0)
        {
            Answer answer = db.AnswerSet.Find(id);
            if (answer == null)
            {
                return HttpNotFound();
            }
            return View(answer);
        }

        //
        // POST: /Answer/Delete/5

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {

            try
            {   Answer answer = db.AnswerSet.Find(id);
                int esysid =(int)answer.ParentQuestion.Property.ExpertSystemId;
                DeleteBranchByAnswer(answer);

                return RedirectToAction("Menu","ExpertSystem",new { id = esysid}); 
            }
            catch (Exception)
            {
                return RedirectToAction("Delete","Answer", new { id = id });
            }
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }

        #region DeleteFuncs
        public void DeleteBranchByAnswer(Answer answer)
        {
            if (answer.ChildQuestion.Count() != 0)
            {
                var CQuestions = answer.ChildQuestion.ToList();

                for (int j = 0; j < CQuestions.Count(); j++)
                {   //все вложенные ответы

                    if (CQuestions[j].ChildAnswer.Count() != 0)
                    {
                        var CAnswers = CQuestions[j].ChildAnswer.ToList();

                        for (int i = 0; i < CAnswers.Count(); i++)
                            DeleteBranchByAnswer(CAnswers[i]);
                    }
                    //все вложенные вопросы
                    db.QuestionSet.Remove(CQuestions[j]);
                    db.SaveChanges();
                }
            }
            DeleteSingleAnswer(answer);
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