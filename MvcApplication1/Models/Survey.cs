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
    public partial class Survey
    {
        public Survey()
        {
            this.Log = new HashSet<Log>();
            this.SurveyItem = new HashSet<SurveyItem>();
        }
    
        public int Id { get; set; }
        public Nullable<int> ExpertSystemId { get; set; }
        public string Type { get; set; }
        public bool IsPropbability { get; set; }
        public int UserId { get; set; }
    
        public virtual ICollection<Log> Log { get; set; }
        public virtual ExpertSystem ExpertSystem { get; set; }
        public virtual ICollection<SurveyItem> SurveyItem { get; set; }
    }
}
