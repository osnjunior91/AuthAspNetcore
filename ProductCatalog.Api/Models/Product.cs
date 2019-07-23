using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProductCatalog.Api.Models
{
    /*Classe que faz mapeamento de produtos */
    [Table("TB_PRODUCT")]
    public class Product : Base
    {
        [Column("DESCRIPTION")]
        public string Description { get; set; }
        [Column("PRICE")]
        public decimal Price { get; set; }
        [Column("QUANTITY")]
        public int Quantity { get; set; }
        [Column("IMAGE")]
        public string Image { get; set; }
        [Column("CREATE_DATE")]
        public DateTime CreateDate { get; set; }
        [Column("LAST_UPDATE_DATE")]
        public DateTime LastUpdateDate { get; set; }
        [Column("CATEGORY_ID")]
        [ForeignKey("CATEGORY_ID")]
        public Guid? CategoryId { get; set; }
        public Category Category { get; set; }
    }
}
