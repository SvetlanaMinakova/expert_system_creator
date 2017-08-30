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
    public class QuestionController : Controller
    {
        private Model1Container db = new Model1Container();
        //
        // GET: /Question/Details/5

        public ActionResult Details(int id = 0)
        {
            Question question = db.QuestionSet.Find(id);
            if (question == null)
            {
                return HttpNotFound();
            }
            return View(question);
        }

        //
        // GET: /Question/Create

        public ActionResult Create(int id=0)
        {
            var ParentAnswer = db.AnswerSet.Find(id);
            Question q = new Question();
            q.ParentAnswer = ParentAnswer;
            return View(q);
        }

        //
        // POST: /Question/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Question question, int id)
        {
            if (ModelState.IsValid)
            {  
                var ParentAnswer = db.AnswerSet.Find(id);
                var curProperty = ParentAnswer.ParentQuestion.Property;

                if (ParentAnswer.ChildQuestion.Count() != 0)
                {
                    DeleteBranchByQuestion(ParentAnswer.ChildQuestion.First());
                    curProperty.RootQdeep = GetTreeDept(curProperty);
                    db.SaveChanges();
                }


                question.ParentAnswer = ParentAnswer;
                question.AnswerId = id;

                question.Property = curProperty;
                question.PropertyId = curProperty.Id;

                question.Deep = ParentAnswer.ParentQuestion.Deep + 1;
                db.QuestionSet.Add(question);
                db.SaveChanges();

                if (question.Property.RootQdeep < question.Deep)
                {
                    question.Property.RootQdeep = question.Deep;
                    db.SaveChanges();
                }
                return RedirectToAction("Menu","ExpertSystem", new { id = (int)curProperty.ExpertSystemId });
            }

            ViewBag.AnswerId = new SelectList(db.AnswerSet, "Id", "Val", question.AnswerId);
            ViewBag.PropertyId = new SelectList(db.PropertySet, "Id", "Name", question.PropertyId);
            return View(question);
        }
        // GET: /Question/Create

        public ActionResult CreateRoot(int id=0)
        {
            
            Question q = new Question();
            try
            {
                var curProp = db.PropertySet.Find(id);
                q.Property = curProp;
                q.PropertyId = curProp.Id;
            }
            catch (Exception) { ;}
            return View(q);
        }

        //
        // POST: /Question/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateRoot(Question question, int id=0)
        {
            if (ModelState.IsValid)
            {
                var curProperty = db.PropertySet.Find(id);
                //create root question or replace old root question by new branch
                try
                {
                    var OldRootQ = curProperty.Question.First();
                    DeleteBranchByQuestion(OldRootQ);
                }
                catch (Exception) { ;}


                question.Deep = 1;
                question.PropertyId = id;
                curProperty.RootQdeep = 1;
                db.SaveChanges();
                db.QuestionSet.Add(question);
                db.SaveChanges();
                return RedirectToAction("Menu", "ExpertSystem", new { id = (int)curProperty.ExpertSystemId });
            }
            return View(question);
        }
        //
        // GET: /Question/Edit/5

        public ActionResult Edit(int id = 0, int expsysid=0)
        {
            Question question = db.QuestionSet.Find(id);
            if (question == null)
            {
                return HttpNotFound();
            }
            ViewBag.AnswerId = new SelectList(db.AnswerSet, "Id", "Val", question.AnswerId);
            ViewBag.PropertyId = new SelectList(db.PropertySet, "Id", "Name", question.PropertyId);
            TempData["expsysid"] = expsysid;
            return View(question);
        }

        //
        // POST: /Question/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Question question)
        {
            Question curQuestion = new Question();
            if (ModelState.IsValid)
            {
                curQuestion=db.QuestionSet.Find(question.Id);
                curQuestion.QuestionTxt = question.QuestionTxt;
                var expsysid = TempData["expsysid"];
                return View(curQuestion);
            }
            ViewBag.AnswerId = new SelectList(db.AnswerSet, "Id", "Val", question.AnswerId);
            ViewBag.PropertyId = new SelectList(db.PropertySet, "Id", "Name", question.PropertyId);

            return View(curQuestion);
        }

        //
        // GET: /Question/Delete/5

        public ActionResult Delete(int id = 0)
        {
            Question question = db.QuestionSet.Find(id);
            if (question == null)
            {
                return HttpNotFound();
            }
            return View(question);
        }

        //
        // POST: /Question/Delete/5

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Question question = db.QuestionSet.Find(id);

            if (question.Property.RootQdeep == 1)
                question.Property.RootQdeep = 0;
            else
            {
                if (db.QuestionSet.Where(q => q.Deep == question.Deep).Count() == 1)
                {
                    question.Property.RootQdeep -= 1;
                    db.SaveChanges();
                }
            }
            
            DeleteBranchByQuestion(question);
            return RedirectToAction("Menu","ExpertSystem", new { id = (int)question.Property.ExpertSystemId });
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }


        public void DeleteQuestionSubTree(Question quest)
        {
            var curPropQuestions = db.QuestionSet.Where(q => q.PropertyId == quest.PropertyId);
        }


        #region HelpFuncs

        public int GetTreeDept(Property property)
        {
            int dept = 0;
            foreach(var quest in db.QuestionSet.Where(q=> q.PropertyId==property.Id))
            {
                if (dept < quest.Deep)
                    dept = quest.Deep;
            }
            return dept;
        
        }

        #endregion

        #region DeleteFuncs

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