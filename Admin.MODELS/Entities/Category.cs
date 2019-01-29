using Admin.MODELS.Abstracts;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Admin.MODELS.Entities
{
    public class Category:BaseEntity<int>
    {
        [StringLength(100,ErrorMessage ="Kategori Adı 3 ile 100 karakter arasında olabilir",MinimumLength =3)]
        [DisplayName("Kategori Adı")]
        public string CategoryName { get; set; }
        [Range(0, 1)]
        public decimal TaxRate { get; set; } = 0;
        public int? SupCategoryId { get; set; }
        [ForeignKey("SupCategoryId")]
        public virtual Category SupCategory { get; set; }
        public virtual ICollection<Category> Categories { get; set; } = new HashSet<Category>();
    }
}
