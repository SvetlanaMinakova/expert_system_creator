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
    public partial class TempLogItem
    {
        public int Id { get; set; }
        public Nullable<int> LogId { get; set; }
        public Nullable<int> ItemId { get; set; }
        public double Probability { get; set; }
    
        public virtual Log Log { get; set; }
        public virtual Item Item { get; set; }
    }
}