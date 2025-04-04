using System.ComponentModel.DataAnnotations;

namespace L02P02_2022JH650_2022RS650.Models
{
    public class libros
    {
        [Key]
        public int id { get; set; }
        public string nombre { get; set; }
        public string descripcion { get; set; }
        public string url_imagen { get; set; }
        public int id_autor { get; set; }
        public int id_categoria { get; set; }
        public decimal precio { get; set; }
        public char estado { get; set; }
    }
}
