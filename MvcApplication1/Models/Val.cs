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
    public partial class Val
    {
        public Val()
        {
            this.Note = new HashSet<Note>();
            this.SelectorSet = new HashSet<SelectorSet>();
        }
    
        public int Id { get; set; }
        public Nullable<int> PropertyId { get; set; }
        public string Mean { get; set; }
    
        public virtual Property Property { get; set; }
        public virtual ICollection<Note> Note { get; set; }
        public virtual ICollection<SelectorSet> SelectorSet { get; set; }
    }
}
