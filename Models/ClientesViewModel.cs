using ClienteCadastroApplication.Controllers;
using ClienteCadastroApplication.Entities;

namespace ClienteCadastroApplication.Models
{
    public class ClientesViewModel
    {
        //------------------------------------------------------------------------------------------------
        // Generals

        public string Title = "Clientes";
        public string Origin { get; set; }

        #region INDEX

        public List<Clientes> PagedList { get; set; }

        //------------------------------------------------------------------------------------------------
        // Filters

        public string Nome { get; set; }
        public string TipoPessoa { get; set; }
        public string Documento {  get; set; }

        #endregion INDEX

        //------------------------------------------------------------------------------------------------

        public Clientes Clientes { get; set; }

        //------------------------------------------------------------------------------------------------

        #region HELPER

        public object RouteRegistry(Clientes reg) => new { reg.Id };

        //------------------------------------------------------------------------------------------------

        #endregion HELPER

    }
}
