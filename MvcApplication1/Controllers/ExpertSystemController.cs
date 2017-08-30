using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;
using MvcApplication1.Models;
using Newtonsoft.Json;
using System.IO;
using System.Text;

namespace MvcApplication1.Controllers
{
    public class ExpertSystemController : Controller
    {
        private Model1Container db = new Model1Container();

        // GET: /ExpertSystem/PN

        public ActionResult PropertyNumeration(int id=0)
        {

            var expertsystem = db.ExpertSystemSet.Find(id);
            int[] numbers = new int[expertsystem.Property.Count()];
            for (int i = 0; i < numbers.Length; i++)
                numbers[i] = i + 1;
            @ViewBag.propsnumbers = new SelectList(numbers);
            return View(expertsystem);
        }

        // POST:

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult PropertyNumeration(ExpertSystem expertsystem,List<int>numeration)
        {
            
            if (ModelState.IsValid)
            {   
                var exsys = db.ExpertSystemSet.Find(expertsystem.Id);

                //link numbers to properties
                List<IdToNumber> curList = new List<IdToNumber>();
                
                int counter=0;
                foreach(var prop in exsys.Property)
                {
                    curList.Add(new IdToNumber(prop.Id, numeration[counter]));
                    counter++;
                }

                //if the numbers are same, previous numeration will be taken into account
                curList.Sort();

                for (int i = 0; i < curList.Count(); i++)
                {
                    db.PropertySet.Find(curList[i].Id).NumberInList = i + 1;
                    db.SaveChanges();
                }

                return RedirectToAction("PropertyNumeration",new {id= (int)expertsystem.Id});
            }
            return View(expertsystem);
        }


        //
        // GET: /ExpertSystem/

        public ActionResult Index()
        {
            return View(db.ExpertSystemSet.Where(es=> es.IsPublished).ToList());
        }

        public bool IsContainErrors(ExpertSystem esys)
        {
            TreeForJsInfo treeInfo = CheckTree(esys);
            if (treeInfo.Errors.Count()> 0)
                return true;
            else
                return false;
        }

        public ActionResult ChangePrivacy(int id = 0)
        {
           var curdB = db.ExpertSystemSet.Find(id);
           if (curdB.IsPublished)
               curdB.IsPublished = false;
           else
           {
               if (!IsContainErrors(curdB))
                   curdB.IsPublished = true;
           }

           db.SaveChanges();
           return RedirectToAction("Menu", "User", new { id =(int) curdB.UserId });
        }


       

        public ActionResult Menu(int id=0)
        {
            ExpertSystem esys=db.ExpertSystemSet.First(x => x.Id == id);
            return View(esys);
        }


        public ActionResult TreeView(int id = 0)
        {
            ExpertSystem esys = db.ExpertSystemSet.First(x => x.Id == id);
            return View(esys);
        }


        public JsonResult GetJsonTree(int id=0)
        {
            ExpertSystem esys =db.ExpertSystemSet.First(x => x.Id == id);
            JsonResult jsonresult = Json(BuildTreeForJs(esys), JsonRequestBehavior.AllowGet);
            return jsonresult;
        }

        public JsonResult CheckJsonTree(int id = 0)
        {
            ExpertSystem esys = db.ExpertSystemSet.First(x => x.Id == id);
            JsonResult jsonresult = Json(CheckTree(esys), JsonRequestBehavior.AllowGet);
            return jsonresult;
        }

        public ActionResult DownLoadXML(int id = 0)
        {
            
                ExpertSystem esys = db.ExpertSystemSet.Find(id);
                if (esys != null)
                {
                    if (CreateXML(esys) == true)
                    {
                    string appPath = Request.PhysicalApplicationPath + "XML" + Path.DirectorySeparatorChar;
                    string filepath = appPath + Path.DirectorySeparatorChar + esys.User.Login + Path.DirectorySeparatorChar + esys.Name + ".xml";
                    Response.ContentType = ".xml";
                    Response.AppendHeader("Content-Disposition", "attachment; filename=" + Path.GetFileName(filepath));
                    Response.WriteFile(filepath);
                    Response.End();
                    }
                    return RedirectToAction("Menu", "User", new { id = esys.UserId });
                }
                return HttpNotFound();
        }
        
        //
        // GET: /ExpertSystem/Details/5

        public ActionResult Details(int id = 0)
        {
            ExpertSystem expertsystem = db.ExpertSystemSet.Find(id);
            if (expertsystem == null)
            {
                return HttpNotFound();
            }
            return View(expertsystem);
        }

        //
        // GET: /ExpertSystem/Create

        public ActionResult Create(int id=0)
        {
            User us = db.UserSet.Find(id);
            ExpertSystem es = new ExpertSystem();
            es.User=us;
            es.UserId=us.Id;
            return View(es);
        }

        //
        // POST: /ExpertSystem/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(ExpertSystem expertsystem,int id=0)
        {
            if (ModelState.IsValid)
            {
                User us = db.UserSet.Find(id);
                expertsystem.User = us;
                expertsystem.UserId = us.Id;
                expertsystem.IsPublished = false;
                db.ExpertSystemSet.Add(expertsystem);
                db.SaveChanges();
                return RedirectToAction("Menu","User",new{id= (int)expertsystem.UserId});
            }

            return View(expertsystem);
        }

        //
        // GET: /ExpertSystem/Edit/5

        public ActionResult Edit(int id = 0)
        {
            ExpertSystem expertsystem = db.ExpertSystemSet.Find(id);
            if (expertsystem == null)
            {
                return HttpNotFound();
            }
            return View(expertsystem);
        }

        //
        // POST: /ExpertSystem/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(ExpertSystem expertsystem)
        {
            var esys = db.ExpertSystemSet.Find(expertsystem.Id);
            if (ModelState.IsValid)
            {
                esys.Name = expertsystem.Name;
                db.SaveChanges();
            }
            return View(esys);
        }

        //
        // GET: /ExpertSystem/Delete/5

        public ActionResult Delete(int id = 0)
        {
            ExpertSystem expertsystem = db.ExpertSystemSet.Find(id);
            if (expertsystem == null)
            {
                return HttpNotFound();
            }
            return View(expertsystem);
        }

        //
        // POST: /ExpertSystem/Delete/5

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ExpertSystem expertsystem = db.ExpertSystemSet.Find(id);
            var curUserId = expertsystem.UserId;
            DeleteExpertSystem(expertsystem);
            return RedirectToAction("Menu","User",new {id = (int)curUserId });
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }

      #region XMLSerizlizationFuncs

        private bool CreateXML(ExpertSystem esys)
        {
            try
            {
                string appPath = Request.PhysicalApplicationPath + "XML" + Path.DirectorySeparatorChar;
                string userXMLpath = appPath + Path.DirectorySeparatorChar + esys.UserId;
                DirectoryInfo dir = new DirectoryInfo(@appPath);
                dir.CreateSubdirectory(esys.User.Login);
                string filepath = appPath + Path.DirectorySeparatorChar + esys.User.Login + Path.DirectorySeparatorChar + esys.Name + ".xml";
                FileInfo curpath = new FileInfo(filepath);
                if (curpath.Exists)
                { curpath.Delete(); }
               
                    StreamWriter sw = new StreamWriter(filepath, false, Encoding.UTF8);
                    using (sw)
                    {
                        sw.WriteLine("<?xml version='1.0'?>");
                        sw.WriteLine("<EXPERTSYSTEM Name= '" + esys.Name + "' Owner= '" + esys.User.Login + "'>");

                        foreach (var prop in esys.Property)
                        {
                            sw.WriteLine("<Property' Name= '" + prop.Name + "'>");

                            foreach (var val in prop.Val)
                            {
                                sw.WriteLine("<Value Mean= '" + val.Mean + "'>");
                                sw.WriteLine("</Value>");
                            }
                            foreach (var quest in prop.Question)
                            {
                                sw.WriteLine("<Question Text= '" + quest.QuestionTxt + "'>");
                                foreach (var answ in quest.ChildAnswer)
                                {
                                    sw.WriteLine("<Answer Val=" + answ.Val + "'>");
                                    foreach (var sel in answ.SelectorSet)
                                    {
                                        sw.WriteLine("<Selector Probability= '" + sel.Probability + "AnswerId='" + answ.Id + "' Value=" + sel.ValSet.Mean + "'>");
                                        sw.WriteLine("</Selector>");
                                    }
                                    sw.WriteLine("</Answer>");
                                }
                                sw.WriteLine("</Question>");
                            }

                            sw.WriteLine("</Property>");
                        }

                        foreach (var it in esys.Item)
                        {
                            sw.WriteLine("<Item Name= '" + it.Name + "'>");
                            foreach (var not in it.Note)
                            {
                                sw.WriteLine("<Property:='" + not.Val.Property.Name + "' Mean= '" + not.Val.Mean + "'>");
                            }
                            sw.WriteLine("</Item>");
                        }
                        sw.WriteLine("</EXPERTSYSTEM>");
                    }
                    sw.Close();
                    return true;
                }
            catch (Exception) { ;}
            return false; 
        }

    #endregion
    #region JSONSerizlizationFuncs
        private TreeForJs BuildTreeForJs(ExpertSystem esys)
        {
            TreeForJs resultTree = new TreeForJs();
            List<Property> PropList = esys.Property.ToList();
            resultTree.propertiesnumber = PropList.Count();

            for(int j=1;j<=resultTree.propertiesnumber;j++)
            {
                resultTree.properties.Add(BuildTreeForJsProperty(PropList[j-1],j));
            }
            return resultTree;
        }

        private TreeForJsProperty BuildTreeForJsProperty(Property prop, int lid)
        {
            TreeForJsProperty resultprop = (new TreeForJsProperty
                {
                    id = lid,
                    text = prop.Name,
                    link = "../../Property/Details/" + prop.Id.ToString(),
                    levelsnumber = prop.RootQdeep

                });
           //AddLevels
            for(int i=1; i<=resultprop.levelsnumber;i++)
            {   IEnumerable<Question> lQuestions = prop.Question.Where(q => q.Deep == i);

                int lQuestionsCount = 0;
                try{ lQuestionsCount = lQuestions.Count();}
                catch(Exception){;}

                if(lQuestionsCount>0)
                {
                    resultprop.levels.Add(new TreeForJsLevel{id=i,qnumberonlevel=lQuestionsCount});
                    TreeForJsLevel curLevel = resultprop.levels.Last();
                    int qid=1;
                    foreach (var question in lQuestions)
                    {
                        curLevel.questions.Add(BuildTreeForJsQuestion(question,qid));
                        qid++;
                    }

                }
            
            }

            return resultprop;
        }


        private TreeForJsQuestion BuildTreeForJsQuestion(Question quest, int qid)
        {
            TreeForJsQuestion resultquestion=(new TreeForJsQuestion
            {
                id = qid,
                text = quest.QuestionTxt,
                link = "../../Question/Details/" + quest.Id.ToString()
            });

            int answid = 1;
            foreach (var answ in quest.ChildAnswer)
            { 
                resultquestion.answers.Add(BuildTreeForJsAnswer(answ,answid));
                answid++;
            }

            return resultquestion;
        
        }

        private TreeForJsAnswer BuildTreeForJsAnswer(Answer answ, int answid)
        {

            TreeForJsAnswer resultanswer = (new TreeForJsAnswer
            {
                id = answid,
                text = answ.Val,
                link = "../../Answer/Details/" + answ.Id.ToString()
            });
            int qid = 1;
            int selid = 1;
            foreach (var quest in answ.ChildQuestion)
            {
                resultanswer.questions.Add(BuildTreeForJsQuestion(quest,qid));
                qid++;
            }

            foreach (var sel in answ.SelectorSet)
            { 
                resultanswer.selectors.Add(BuildTreeForJsSelector(sel,selid));
                selid++;
            }
            return resultanswer;

        }

        private TreeForJsSelector BuildTreeForJsSelector(SelectorSet sel, int selid)
        {

            TreeForJsSelector resultselector = (new TreeForJsSelector
             {
                 id = selid,
                 text = sel.ValSet.Mean + " P:" + sel.Probability.ToString(),
                 link = "#"
             });
            return resultselector;
        }

        [Serializable]
        public class TreeForJs
        {
            public int propertiesnumber;
            public List<TreeForJsProperty> properties = new List<TreeForJsProperty>();
        }

        [Serializable]
        public class TreeForJsProperty
        {
            public int id;
            public string text;
            public string link;
            public int levelsnumber;
            public List<TreeForJsLevel> levels=new List<TreeForJsLevel>();
            
        }

        [Serializable]
        public class TreeForJsLevel
        {
            public int id;
            public int qnumberonlevel;
            public List<TreeForJsQuestion> questions = new List<TreeForJsQuestion>();
        }


        [Serializable]
        public class TreeForJsQuestion
        {
            public int id;
            public string text;
            public string link;
            public List<TreeForJsAnswer> answers = new List<TreeForJsAnswer>();
        }

         [Serializable]
        public class TreeForJsAnswer
        {
             public  int id;
             public string text;
             public string link;
             public List<TreeForJsQuestion> questions = new List<TreeForJsQuestion>();
             public List<TreeForJsSelector> selectors = new List<TreeForJsSelector>();
        }


        [Serializable]
        public class TreeForJsSelector
        {
            public int id;
            public string text;
            public string link;
        }


        public TreeForJsInfo CheckTree(ExpertSystem esys)
        {
            bool isEmpty=false;
            TreeForJsInfo resultTree = new TreeForJsInfo();
            if (esys.Property.Count() == 0)
            {
                resultTree.Errors.Add("Expert system " + esys.Name + " should have properties!");
                isEmpty = true;
            }
            if (esys.Item.Count() == 0)
            {
            resultTree.Errors.Add("Expert system " + esys.Name + " should have Items!");
            isEmpty = true;
            }

            if(isEmpty)
            return resultTree;

            int RootQDeep = 0;
            IEnumerable<Question> LastLevelQuests;
            foreach (var prop in esys.Property)
            {
                if (prop.Question.Count() == 0)
                    resultTree.Errors.Add("Property " + prop.Name + " should have branch!");
                else
                { 
                    RootQDeep = prop.RootQdeep;
                    LastLevelQuests = db.QuestionSet.Where(q => q.PropertyId == prop.Id && q.Deep == RootQDeep);
                    foreach (var quest in LastLevelQuests)
                    {
                        if (quest.ChildAnswer.Count() == 0)
                            resultTree.Errors.Add("Question " + quest.QuestionTxt + " ( property= )" + prop.Name + " should have answer!");
                        else
                        {  foreach (var answ in quest.ChildAnswer)
                            if(answ.SelectorSet.Count()==0)
                                resultTree.Warnings.Add("Answer " + answ.Val+ "( Question " + quest.QuestionTxt + " of property " + prop.Name +" ) should have selectors!");
                        }
                    
                    }

                }
            
            }
            if (resultTree.Errors.Count() == 0 && resultTree.Warnings.Count() == 0)
                resultTree.isTreeCorrect = true;
            return resultTree;
        
        }

        [Serializable]
        public class TreeForJsInfo
        {
            public bool isTreeCorrect=false;
            public List<string> Errors = new List<string>();
            public List<string> Warnings = new List<string>();

        }

        public class IdToNumber: IComparable<IdToNumber>
        {
            public int Id;
            public int NumberInList;

            public IdToNumber(int id, int numberInList)
            { this.Id = id; this.NumberInList = numberInList; }
            public int CompareTo(IdToNumber obj)
            {
                if (this.NumberInList > obj.NumberInList)
                    return 1;
                if (this.NumberInList < obj.NumberInList)
                    return -1;
                else
                    return 0;
            }        
        }
        #endregion

        #region DeleteFuncs

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