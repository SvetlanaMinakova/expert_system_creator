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
    public partial class SelectorSet
    {
        public int Id { get; set; }
        public Nullable<int> AnswerId { get; set; }
        public Nullable<int> ValId { get; set; }
        public double Probability { get; set; }
    
        public virtual Answer AnswerSet { get; set; }
        public virtual Val ValSet { get; set; }
    }
}
