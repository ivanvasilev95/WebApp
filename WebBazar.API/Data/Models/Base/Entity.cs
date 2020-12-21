using System;

namespace WebBazar.API.Data.Models.Base
{
    public class Entity : DeletableEntity, IEntity
    {
        public DateTime CreatedOn { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public string ModifiedBy { get; set; }
    }
}