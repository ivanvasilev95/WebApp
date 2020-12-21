using System;

namespace WebBazar.API.Data.Models.Base
{
    public interface IDeletableEntity
    {
        DateTime? DeletedOn { get; set; }
        string DeletedBy { get; set; }
        bool IsDeleted { get; set; }
    }
}