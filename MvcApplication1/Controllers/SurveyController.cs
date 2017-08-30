using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MvcApplication1.Models;
using System.Text;

namespace MvcApplication1.Controllers
{
    public class SurveyController : Controller
    {
        private Model1Container db = new Model1Container();
        //
        //
        // GET: /Survey/Create

        public ActionResult Create(int userid=0,int esysid=0)
        {
            try
            {
                var curExpSystem = db.ExpertSystemSet.Find(esysid);
                Survey surv = new Survey();
                surv.ExpertSystem = curExpSystem;
                surv.ExpertSystemId = curExpSystem.Id;
                surv.UserId = userid;
                return View(surv);
            }
            catch (Exception)
            { return HttpNotFound(); }
        }

        //
        // POST: /Survey/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Survey survey, int esysid=0,int userid =0, string Type="usual")
        {
            if (ModelState.IsValid)
            {   
                var curExpSystem = db.ExpertSystemSet.Find(esysid);
                survey.ExpertSystem = curExpSystem;
                survey.ExpertSystemId = curExpSystem.Id;
                survey.UserId = userid;
                survey.IsPropbability = false;
                if (Type != "usual")
                survey.IsPropbability = true;
              
                try
                {
                    //Add survey
                    db.SurveySet.Add(survey);
                    db.SaveChanges();

                    //Are there any properties in Expert System?
                    if (curExpSystem.Property.Count() == 0)
                    {@ViewBag.Err="Expert system has no properties(empty)";
                    return View(survey);
                    }
                    //Are there any items in Expert System?
                    if (curExpSystem.Item.Count() == 0)
                    {
                        @ViewBag.Err = "Expert system has no items(empty)";
                        return View(survey);
                    }

                    //Add first lof(log for the first Property)
                    var firstProp = curExpSystem.Property.Where(p=> p.NumberInList==1).First();
                    var firstQ = firstProp.Question.Where(q => q.Deep == 1).First();
                    var firstA = firstQ.ChildAnswer.First();
                    //logId=0 - empty log
                    db.LogSet.Add(new Log 
                    {  SurveyId= survey.Id,
                        PropId = firstProp.Id,
                        QId = firstQ.Id,
                        AnswId = firstA.Id
                    });

                    db.SaveChanges();

                    //Area=all expert system objects
                    //Initial probability is same for each variant
                    double count = curExpSystem.Item.Count();
                    double StartProbability = 1 / count;
                    foreach (var item in curExpSystem.Item)
                    {
                        db.SurveyItemSet.Add(new SurveyItem 
                        { 
                            SurveyId=survey.Id,
                            ItemId=item.Id,
                            Probability = StartProbability
                        });
                        db.SaveChanges();
                    }

                    return RedirectToAction("CurLog",new {logid= curExpSystem.Survey.First().Log.First().Id});
                }
                // Error
                catch (Exception)
                {      
                        DeleteSurvey(survey);
                        db.SaveChanges();
                        @ViewBag.Err = "System is empty or contain logical errors";
                        return View(survey);
                }
            }

            ViewBag.ExpertSystemId = survey.ExpertSystemId;
            return View(survey);
        }


        //Current survey step
         // GET: /Survey/CurLog

        public ActionResult CurLog(int logid = 0)
        {
           var curLog= db.LogSet.Find(logid);
           var curQ = db.QuestionSet.Find(curLog.QId);
           ViewBag.Question = curQ.QuestionTxt;
           ViewBag.Answers = new SelectList(curQ.ChildAnswer, "Id", "Val", curQ.ChildAnswer.First().Val);
           return View(curLog);
        }

        //
        // POST: /Survey/CurLog

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CurLog(Log log, string direction = "")
        {
            var curSurvay = db.SurveySet.Find(log.SurveyId);
            var curExpSystem = db.ExpertSystemSet.Find(curSurvay.ExpertSystemId);

            if (direction == "prev")
            {
                //if log upper is empty
                var parentlogId = db.LogSet.Find(log.Id).LogId;
                if (parentlogId == null)
                {
                    DeleteSurvey(curSurvay);
                    return RedirectToAction("Create", new { userid = log.Survey.UserId, esysid=curExpSystem.Id });
                }
                else
                {//clear memory
                    var parentLog = db.LogSet.Find(parentlogId);
                    var curLog = db.LogSet.Find(log.Id);

                    //usual survey type
                    if (!curLog.Survey.IsPropbability)
                    {
                        //return removed on the previous step items to choosing area
                        foreach (var removeditems in parentLog.TempRemovedItem)
                        {
                            db.SurveyItemSet.Add(new SurveyItem
                            {
                                SurveyId = curSurvay.Id,
                                ItemId = removeditems.ItemId
                            });
                            db.SaveChanges();
                        }
                    }

                    //probability survey
                    else
                    {//back to previous probabilities
                        foreach (var removeditems in parentLog.TempRemovedItem)
                        {
                            curSurvay.SurveyItem.First(si => si.ItemId == removeditems.ItemId).Probability = removeditems.Probability;
                        }
                    }

                    //delete current step
                    DeleteLog(curLog);
                    //back to previous step
                    return RedirectToAction("CurLog", new { logid = parentlogId });
                }
            }


            else
            {
                if (direction == "exit")
                {
                    var userId = curSurvay.UserId;
                    DeleteSurvey(curSurvay);
                    if (userId != (-1))
                        return RedirectToAction("Menu", "User", new { id = userId });
                    else
                        return RedirectToAction("Index", "ExpertSystem");
                }
                //next
                else
                {
                    if (ModelState.IsValid)
                    { //current selectors
                        var LocalSelectors = new List<SelectorSet>();
                        db.LogSet.Find(log.Id).AnswId = log.AnswId;
                        db.SaveChanges();

                        var curAnsw = db.AnswerSet.Find(log.AnswId);
                        var curProp = curAnsw.ParentQuestion.Property;

                        //result
                        string itemname = "";


                        if (curAnsw.SelectorSet.Count() > 0)
                        {
                           foreach (var sel in curAnsw.SelectorSet)
                           LocalSelectors.Add(sel);
                            var AllNotes = new List<Note>();
                               Item curItem;
                            //for each item in choosing area
                            foreach (var item in curSurvay.SurveyItem)
                            {
                                curItem = db.ItemSet.Find(item.ItemId);
                                //for each item's property connected with current property
                                foreach (var not in curItem.Note.Where(n => n.Val.PropertyId == curProp.Id))
                                AllNotes.Add(not);
                            }

                            if (curSurvay.IsPropbability)
                            {
                            SurveyItem curSurveyItem;

                            foreach (var sel in LocalSelectors)
                            {
                                foreach (var not in AllNotes)
                                {
                                    //if value is found, enrise the probability of Item
                                    if (sel.ValId == not.ValId)
                                    { curSurveyItem = curSurvay.SurveyItem.First(sit=> sit.ItemId==not.ItemId);
                                    curSurveyItem.Probability = (curSurveyItem.Probability * sel.Probability) / (curSurveyItem.Probability * sel.Probability +
                                    (1 - curSurveyItem.Probability) * (1 - sel.Probability));
                                    }
                                      
                                }
                            }

                                //save probabilities to temp (uses for "prev" direction)
                                foreach (var selit in curSurvay.SurveyItem)
                                {
                                    db.TempLogItemSet.Add(new TempLogItem
                                    {
                                        LogId = log.Id,
                                        ItemId = selit.ItemId,
                                        Probability = selit.Probability
                                    });
                                    db.SaveChanges();
                                } 
                            }

                            //usual survey
                            else
                            {

                                //Items, fittder by Selectors
                                var FiltredItemIds = new List<int>();

                                var ItemsForDelete = new List<int>();

                                foreach (var sel in LocalSelectors)
                                {
                                    foreach (var not in AllNotes)
                                    {
                                        if (sel.ValId == not.ValId)
                                            FiltredItemIds.Add((int)not.ItemId);
                                    }
                                }

                                FiltredItemIds = FiltredItemIds.Distinct().ToList();


                                //if choosing area has 1 Item
                                if (FiltredItemIds.Count() == 1)
                                {
                                    itemname = curExpSystem.Item.First(it => it.Id == FiltredItemIds.First()).Name;
                                    DeleteSurvey(curSurvay);
                                    return RedirectToAction("Finish", new { itemname = itemname, userid = curSurvay.UserId });
                                }
                                //if choosing area has 0 Items, back to previous step
                                if (FiltredItemIds.Count() == 0)
                                {
                                    itemname = curExpSystem.Item.First().Name;
                                    DeleteSurvey(curSurvay);
                                    return RedirectToAction("Finish", new { itemname=itemname, userid = curSurvay.UserId });
                                }

                                // //if choosing area has >1 Item
                                int curId;
                                foreach (var selit in curSurvay.SurveyItem)
                                {//Should we have this Item on the next step?
                                    try
                                    {
                                        curId = FiltredItemIds.First(it => it == selit.ItemId);
                                    }
                                    catch (Exception)
                                    {
                                        ItemsForDelete.Add((int)selit.ItemId);
                                    }
                                }

                                SurveyItem itemfordelete;

                                for (int i = 0; i < ItemsForDelete.Count(); i++)
                                {

                                    itemfordelete = db.SurveyItemSet.ToList().First(sit => sit.ItemId == ItemsForDelete[i]);

                                    db.TempLogItemSet.Add(new TempLogItem
                                    {
                                        LogId = log.Id,
                                        ItemId = ItemsForDelete[i],
                                        Probability = itemfordelete.Probability
                                    });
                                    db.SaveChanges();

                                    db.SurveyItemSet.Remove(itemfordelete);
                                    db.SaveChanges();
                                }
                            }

                        }

                        //next question of current property
                        if (curAnsw.ChildQuestion.Count() != 0)
                        {
                            db.LogSet.Add(new Log
                            {
                                SurveyId = log.SurveyId,
                                PropId = log.PropId,
                                QId = curAnsw.ChildQuestion.First().Id,
                                AnswId = curAnsw.ChildQuestion.First().ChildAnswer.First().Id,
                                PrevLog = db.LogSet.Find(log.Id),
                                LogId = db.LogSet.Find(log.Id).Id
                            });

                            db.SaveChanges();
                            int lastid = db.LogSet.ToList().Last().Id;

                            return RedirectToAction("CurLog", new { logid = lastid });

                        }

                            //next property
                        else
                        {
                            try
                            {
                                var NextProp = curExpSystem.Property.Where(p => p.NumberInList == curProp.NumberInList + 1).First();
                                var firstQ = NextProp.Question.First(q => q.Deep == 1);
                                var firstA = firstQ.ChildAnswer.First();
                                db.LogSet.Add(new Log
                                {
                                    SurveyId = log.SurveyId,
                                    Survey = db.SurveySet.Find(log.SurveyId),
                                    PropId = NextProp.Id,
                                    QId = firstQ.Id,
                                    AnswId = firstA.Id,
                                    PrevLog = db.LogSet.Find(log.Id),
                                    LogId = db.LogSet.Find(log.Id).Id
                                });
                                db.SaveChanges();

                                return RedirectToAction("CurLog", new { logid = curSurvay.Log.First(l => l.PropId == NextProp.Id).Id });
                            }
                            //no more properties
                            catch (Exception)
                            {
                                //probability survey
                                if (curSurvay.IsPropbability)
                                {
                                    var MaxPItem = curSurvay.SurveyItem.First();

                                    //Item with max probability
                                    foreach (var it in curSurvay.SurveyItem)
                                    {
                                        if (MaxPItem.Probability < it.Probability)
                                            MaxPItem = it;
                                    }

                                    itemname = curExpSystem.Item.First(it => it.Id == MaxPItem.ItemId).Name;   
                                }

                                //usual survey
                                else
                                itemname = curExpSystem.Item.First(it => it.Id == curSurvay.SurveyItem.First().ItemId).Name;

                                //delete survey
                                DeleteSurvey(curSurvay);
                                return RedirectToAction("Finish", new { itemname = itemname, userid = curSurvay.UserId });
                            }
                        }

                    }
                    return View(log);
                }
            }
        }

        // GET: /Survey/Finish

        public ActionResult Finish(string itemname = "", int userid = 0)
        {
            //model = Item
            ViewBag.UserId = userid;
            Item it= new Item();
            it.Name = itemname;
            return View(it);
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }

        #region DeleteHelpers

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

        #endregion
    }
}