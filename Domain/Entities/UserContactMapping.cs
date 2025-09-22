﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class UserContactMapping : BaseEntity
    {
        public Guid UserId { get; set; }
        public Guid ContactId { get; set; }
        public string ContactType { get; set; }
    }

}
