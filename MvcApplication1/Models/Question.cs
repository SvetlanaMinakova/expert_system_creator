//------------------------------------------------------------------------------
// <auto-generated>
//    Этот код был создан из шаблона.
//
//    Изменения, вносимые в этот файл вручную, могут привести к непредвиденной работе приложения.
//    Изменения, вносимые в этот файл вручную, будут перезаписаны при повторном создании кода.
// </auto-generated>
//------------------------------------------------------------------------------

namespace MvcApplication1.Models
{
    using System;
    using System.Collections.Generic;
    [Serializable]
    public partial class Question
    {
        public Question()
        {
            this.ChildAnswer = new HashSet<Answer>();
        }
    
        public int Id { get; set; }
        public string QuestionTxt { get; set; }
        public int Deep { get; set; }
        public Nullable<int> AnswerId { get; set; }
        public Nullable<int> PropertyId { get; set; }
    
        public virtual ICollection<Answer> ChildAnswer { get; set; }
        public virtual Answer ParentAnswer { get; set; }
        public virtual Property Property { get; set; }
    }
}
