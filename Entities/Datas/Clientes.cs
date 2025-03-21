using ClienteCadastroApplication.Tools;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace ClienteCadastroApplication.Entities
{
    public class Clientes
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Campo Obrigatório")]
        [DisplayName("Nome")]
        public string Nome { get; set; }

        [Required(ErrorMessage = "Campo Obrigatório")]
        [DisplayName("Tipo")]
        public string Tipo { get; set; }

        [Required(ErrorMessage = "Campo Obrigatório")]
        [DisplayName("Documento")]
        public string Documento { get; set; }

        [Required(ErrorMessage = "Campo Obrigatório")]
        [DisplayName("Data de Cadastro")]
        public DateTime? DataCadastro { get; set; } = DateTime.Now;

        [Required(ErrorMessage = "Campo Obrigatório")]
        [DisplayName("Telefone")]
        public string Telefone { get; set; }
        public bool Deleted { get; set; }

    }
}
