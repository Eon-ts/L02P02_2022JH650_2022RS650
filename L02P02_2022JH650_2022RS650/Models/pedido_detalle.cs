using System.ComponentModel.DataAnnotations;

namespace L02P02_2022JH650_2022RS650.Models
{
    public class pedido_detalle
    {
        [Key]
        public int id { get; set; }
        public int id_pedido { get; set; }
        public int id_libro { get; set; }
        public DateTime created_at { get; set; }
    }
}
