﻿using Admin.MODELS.Abstracts;
using Admin.MODELS.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Admin.MODELS.Entities
{
    [Table("Orders")]
    public class Order:BaseEntity<long>
    {
        [DisplayName("Sipariş Tipi")]
        public OrderType OrderType { get; set; }
        public virtual ICollection<Invoice> Invoices { get; set; } = new HashSet<Invoice>();
    }
}
