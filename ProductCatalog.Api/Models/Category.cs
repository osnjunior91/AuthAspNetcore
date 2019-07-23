using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProductCatalog.Api.Models
{
    /*Criada classe que mapeia a tabela de categoria*/
    [Table("TB_CATEGORY")]
    public class Category : Base
    {
        public IEnumerable<Product> Products { get; set; }
    }
}
