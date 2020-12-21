using System;

namespace WebBazar.API.Data.Models.Base
{
    public class DeletableEntity : IDeletableEntity
    {
        public DateTime? DeletedOn { get; set; }
        public string DeletedBy { get; set; }
        public bool IsDeleted { get; set; }
    }
}