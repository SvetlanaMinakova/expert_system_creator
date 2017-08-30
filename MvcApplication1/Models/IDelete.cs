using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MvcApplication1.Models
{
    public class DeleteR
    { Model1Container db = new Model1Container();
    public void DeleteSelector(SelectorSet selector)
    {
        db.SelectorSet.Remove(selector);
        db.SaveChanges();
    }

    public void DeleteNote(Note note)
    { 
    db.NoteSet.Remove(note);
    db.SaveChanges();
    }

    public void DeleteItem(Item item)
    {//при удалении предмета удаляются все связанные с ним записи
        foreach (var note in item.Note)
            DeleteNote(note);
        //потом удаляется сам предмет
        db.ItemSet.Remove(item);
        db.SaveChanges();
    }
    public void DeleteVal(Val val)
    { //при удалении значения свойства также удаляются все связанные с ним записи
        foreach (var note in val.Note)
            DeleteNote(note);
        //потом удаляется само значение
        db.ValSet.Remove(val);
        db.SaveChanges();
    }

    public void DeleteSingleAnswer(Answer answer)
    {
        foreach (var selector in answer.SelectorSet)
            db.SelectorSet.Remove(selector);
        db.AnswerSet.Remove(answer);
        db.SaveChanges();
    }

        //при удалении ответа удаляется вся ветка вопросов, идущая от этого ответа
        //также удаляются все селекторы, зависящие от данного ответа
    public void DeleteBranchByAnswer(Answer answer)
    {
        Model1Container DB = new Model1Container();
        if (answer.ChildQuestion.Count() != 0)
        {
          
            foreach (var quest in answer.ChildQuestion)
            {   //все вложенные ответы
                foreach (var childA in quest.ChildAnswer)
                    DeleteBranchByAnswer(answer);
                //все вложенные вопросы
                DB.QuestionSet.Remove(quest);
                DB.SaveChanges();
            }
        }
        DB.AnswerSet.Remove(answer);
        DB.SaveChanges();
    }


    public void DeleteProperty(Property property)
    {//при удалении свойства удаляется его корневое дерево
        //также удаляются все связанные с ним значения
        foreach (var val in property.Val)
            DeleteVal(val);
        //потом удаляется само свойство
        db.PropertySet.Remove(property);
        db.SaveChanges();
    }

    }
}
