using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ProductCatalog.Api.Models
{
    /*Classe base, qualuqer classe que use o repositorio deve herdar dessa classe*/
    public class Base
    {
        [Key]
        [Column("ID")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }
        [Column("TITLE")]
        public string Title { get; set; }
    }
}
