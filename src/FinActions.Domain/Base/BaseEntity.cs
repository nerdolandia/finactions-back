using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FinActions.Domain.Base
{
    public abstract class BaseEntity
    {
        public Guid Id {get; set;}
        public Guid CreatedBy {get; set;}
        public DateTime CreationDate {get; set;}
        public Guid? EditedBy {get; set;}
        public DateTime? EditedDate {get; set;}
        public Guid? DeletedBy {get; set;}
        public DateTime? DeletedDate {get; set;}
        public bool IsDeleted {get; set;}
    }
}