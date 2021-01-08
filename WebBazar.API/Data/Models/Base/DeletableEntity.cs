using System;

namespace WebBazar.API.Data.Models.Base
{
    public class DeletableEntity : IDeletableEntity
    {
        public bool IsDeleted { get; set; }
        public DateTime? DeletedOn { get; set; }
        public string DeletedBy { get; set; }
    }
}