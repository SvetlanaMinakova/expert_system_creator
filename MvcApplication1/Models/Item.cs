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
    using System.ComponentModel.DataAnnotations;
    [Serializable]
    public partial class Item
    {
        public Item()
        {
            this.Note = new HashSet<Note>();
            this.SurveyItem = new HashSet<SurveyItem>();
            this.TempRemovedItem = new HashSet<TempLogItem>();
        }
    
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        public Nullable<int> ExpertSystemId { get; set; }
    
        public virtual ICollection<Note> Note { get; set; }
        public virtual ExpertSystem ExpertSystem { get; set; }
        public virtual ICollection<SurveyItem> SurveyItem { get; set; }
        public virtual ICollection<TempLogItem> TempRemovedItem { get; set; }
    }
}