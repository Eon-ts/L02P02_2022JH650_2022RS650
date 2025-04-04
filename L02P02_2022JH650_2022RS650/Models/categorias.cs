using System.ComponentModel.DataAnnotations;

namespace L02P02_2022JH650_2022RS650.Models
{
    public class categorias
    {
        [Key]
        public int id { get; set; }
        public string categoria { get; set; }
    }
}
