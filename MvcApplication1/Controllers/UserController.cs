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
    public class UserController : Controller
    {
        private Model1Container db = new Model1Container();
        //
        // GET: /User/Details/5

        public ActionResult Details(int id = 0)
        {
            User user = db.UserSet.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        //
        // GET: /User/Create

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /User/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(User user, string OneMorePassword)
        {
            if (ModelState.IsValid)
            {

                if (user.Password == null || user.Password == "")
                {   ViewBag.PassErr="Please, enter a password";
                    return View(user);
                }

                if(OneMorePassword!=user.Password)
                {
                    ViewBag.PassErr = "Passwords don't match";
                    return View(user);
                }

                try
                {
                    var Exist = db.UserSet.First(u => u.Login == user.Login);
                    ViewBag.LoginErr = "This login is already registred";
                    return View(user);
                }

                catch (Exception)
                {
                    user.LastVisited = DateTime.Now;
                    db.UserSet.Add(user);
                    db.SaveChanges();
                    return RedirectToAction("Menu",new {id=user.Id});
                }
            }

            ViewBag.LoginErr = "Unknown registration error";
            return View(user);
        }


        public ActionResult Login()
        {
            return View();
        }

        // POST: /User/Login

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(User user)
        {
            if (ModelState.IsValid)
            {
                User curUser = db.UserSet.FirstOrDefault(x => x.Login == user.Login);
                if (curUser != null)
                {

                    if (curUser.Password != user.Password)
                    {
                        ViewBag.PassErr = "Password is not correct";
                        return View(user);
                    }

                    else
                    {
                        curUser.LastVisited = DateTime.Now;
                        db.SaveChanges();
                        return RedirectToAction("Menu", new { id = curUser.Id });
                    }
                }
            }
                ViewBag.LoginErr = "Login is not correct";
                return View(user);
        }


        public ActionResult Menu(int id =(-1))
        {
            if (id == (-1))
                RedirectToAction("Login");
            //else
            User us = db.UserSet.Find(id);
                return View(us);
        }

        //
        // GET: /User/Edit/5

        public ActionResult Edit(int id = 0)
        {
            User user = db.UserSet.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        //
        // POST: /User/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(User user)
        {
            User us = db.UserSet.Find(user.Id);
            if (ModelState.IsValid)
            {
                us.LastVisited = DateTime.Now;
                us.Password = user.Password;
                db.SaveChanges();
                return RedirectToAction("Edit", new { id = user.Id });
            }
            return View(us);
        }

        //
        // GET: /User/Delete/5

        public ActionResult Delete(int id = 0)
        {
            User user = db.UserSet.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        //
        // POST: /User/Delete/5

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            User user = db.UserSet.Find(id);
            var curUserName = user.Login;
            DeleteUser(user);
            db.SaveChanges();
            return RedirectToAction("Login");
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
        
        #region DeleteFuncs
        public void DeleteUser(User user)
        {
            if (user.ExpertSystem.Count() != 0)
            {
                var esystems = user.ExpertSystem.ToList();
                for (int i=0;i<esystems.Count();i++)
                    DeleteExpertSystem(esystems[i]);
            }
            db.UserSet.Remove(user);
            db.SaveChanges();
        
        }

        public void DeleteExpertSystem(ExpertSystem expertsystem)
        {
            if (expertsystem.Property.Count() != 0)
            {
                var properties = expertsystem.Property.ToList();
                for (int i = 0; i < properties.Count(); i++)
                    DeleteProperty(properties[i]);
            }
            if (expertsystem.Item.Count() != 0)
            {
                var items = expertsystem.Item.ToList();
                for (int i = 0; i < items.Count(); i++)
                    DeleteItem(items[i]);
            }

            if (expertsystem.Survey.Count() != 0)
            {
                var surveys = expertsystem.Survey.ToList();
                for (int i = 0; i < surveys.Count(); i++)
                    DeleteSurvey(surveys[i]);
            }
            db.ExpertSystemSet.Remove(expertsystem);
            db.SaveChanges();
        }


        public void DeleteSurvey(Survey survey)
        {
            var loglist = survey.Log.ToList();
            for (int i = 0; i < loglist.Count(); i++)
                DeleteLog(loglist[i]);

            var itemlist = survey.SurveyItem.ToList();
            for (int i = 0; i < itemlist.Count(); i++)
            {
                db.SurveyItemSet.Remove(itemlist[i]);
                db.SaveChanges();
            }
            db.SurveySet.Remove(survey);
            db.SaveChanges();
        }

        public void DeleteLog(Log log)
        {
            var removelist = log.TempRemovedItem.ToList();
            for (int i = 0; i < log.TempRemovedItem.Count(); i++)
            {
                db.TempLogItemSet.Remove(removelist[i]);
                db.SaveChanges();
            }
            db.LogSet.Remove(log);
            db.SaveChanges();

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