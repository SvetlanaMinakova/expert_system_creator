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
    public partial class Property
    {
        public Property()
        {
            this.Val = new HashSet<Val>();
            this.Question = new HashSet<Question>();
        }
    
        public int Id { get; set; }
        public string Name { get; set; }
        public int RootQdeep { get; set; }
        public int NumberInList { get; set; }
        public Nullable<int> ExpertSystemId { get; set; }
    
        public virtual ICollection<Val> Val { get; set; }
        public virtual ICollection<Question> Question { get; set; }
        public virtual ExpertSystem ExpertSystem { get; set; }
    }
}