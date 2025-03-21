using ClienteCadastroApplication.Entities;
using ClienteCadastroApplication.Models;
using ClienteCadastroApplication.Services;
using ClienteCadastroApplication.Tools;
using Microsoft.AspNetCore.Mvc;
using System.Linq.Expressions;

namespace ClienteCadastroApplication.Controllers
{
    public class ClientesController : Controller
    {
        private readonly AppDbContext _db;
        public ClientesController(AppDbContext context)
        {
            _db = context;
        }

        //------------------------------------------------------------------------------------------------

        #region INDEX

        public ActionResult Index(ClientesViewModel model)
        {
            //------------------------------------------------------------------------------------------------

            model.Documento = model.Documento.RemoveMask();

            model.PagedList = ClientesServ.GetList(_db, model);
            model.PagedList.ForEach(x =>
            {
                var RegTp = x.Tipo;
                var Tipo = RegTp == DocT.Tipo.Fisica.GetFlag() ? DocT.Tipo.Fisica : DocT.Tipo.Juridica;

                x.Documento = x.Documento.ApplyDocMask(Tipo);
            });

            return View(model);
        }

        #endregion INDEX

        //------------------------------------------------------------------------------------------------

        #region DELETE

        public ActionResult Delete(Clientes reg)
        {
            ActionResult RtnDef(string str = "")
            {
                return RedirectToAction(nameof(Index));
            };

            var temp = ClientesServ.Get(_db, reg.Id).FirstOrDefault();
            if (temp.Deleted == true)
            {
                var err = MsgT.RegNotFound;
                return RtnDef(err);
            }

            ClientesServ.SoftDelete(_db, temp);
            return RtnDef();
        }

        #endregion DELETE

        //------------------------------------------------------------------------------------------------

        #region CREATE/EDIT

        public ActionResult BaseGetCreateEdit(Clientes reg, string origin)
        {
            var isCreate = origin == nameof(Create);
            var isEdit = origin == nameof(Edit);

            var model = new ClientesViewModel()
            {
                Origin = origin,
            };

            if (isCreate)
            {
                model.Clientes = new Clientes()
                {
                    DataCadastro = DateTime.Today,
                };
            }
            else
            {
                var temp = ClientesServ.Get(_db, reg.Id).FirstOrDefault();
                var Exists = temp != null;
                if (!Exists) return RedirectToAction(nameof(Index));

                model.Clientes = temp;
            }

            return View(nameof(Create), model);
        }

        public ActionResult BasePostCreateEdit(ClientesViewModel model, string origin)
        {
            var isCreate = origin == nameof(Create);
            var isEdit = origin == nameof(Edit);

            //------------------------------------------------------------------------------------------

            ActionResult RtnDef(string msg = "")
            {
                return View(model);
            }

            // Clean-up ModelState
            foreach (var key in ModelState.Keys.Where(x => !x.StartsWith(nameof(model.Clientes))).ToList())
                ModelState.Remove(key);
            if (ModelState.IsValid == false) return RtnDef();

            //------------------------------------------------------------------------------------------

            void AddModelState(Expression<Func<ClientesViewModel, object>> property, string _error)
                => ModelState.AddModelError(SysT.GenName(property), _error);

            ActionResult RtnErrModelState(Expression<Func<ClientesViewModel, object>> property, string _error)
            {
                AddModelState(property, _error);
                return RtnDef();
            }

            ActionResult RtnErrRequired(Expression<Func<ClientesViewModel, object>> _property)
                => RtnErrModelState(_property, MsgT.DefRequired);

            if (!ModelState.IsValid) return RtnDef();

            //------------------------------------------------------------------------------------------------
            // Required

            if (string.IsNullOrEmpty(model.Clientes.Nome)) return RtnErrRequired(x => x.Clientes.Nome);
            if (string.IsNullOrEmpty(model.Clientes.Tipo)) return RtnErrRequired(x => x.Clientes.Tipo);
            if (string.IsNullOrEmpty(model.Clientes.Documento)) return RtnErrRequired(x => x.Clientes.Documento);
            if (string.IsNullOrEmpty(model.Clientes.Telefone)) return RtnErrRequired(x => x.Clientes.Telefone);

            //------------------------------------------------------------------------------------------------
            // Validations

            var Exists = ClientesServ.Get(_db, model.Clientes.Id).Any();
            if (Exists && isCreate) return RtnErrModelState(x => x.Clientes, MsgT.RegAlreadyExists);
            if (!Exists && isEdit) return RtnErrModelState(x => x.Clientes, MsgT.RegNotFound);

            var ExistsDoc = ClientesServ.GetByDocumento(_db, model.Clientes.Documento).Any();
            if (ExistsDoc && isCreate) return RtnErrModelState(x => x.Clientes.Documento, "Cliente com documento já cadastrado.");

            //------------------------------------------------------------------------------------------------
            // Default Values/Treatment

            model.Clientes.Documento = model.Clientes.Documento.RemoveMask();
            model.Clientes.Deleted = false;

            try
            {
                if (isCreate)
                {
                    ClientesServ.Create(_db, model.Clientes);
                }
                else
                {
                    ClientesServ.Edit(_db, model.Clientes);
                }

                _db.SaveChanges();

            }
            catch (Exception ex)
            {
                ex.ToString();
            }

            return RedirectToAction(nameof(Index));
        }

        //------------------------------------------------------------------------------------------------

        [HttpGet]
        public ActionResult Create() => BaseGetCreateEdit(null, nameof(Create));

        [HttpGet]
        public ActionResult Edit(Clientes reg) => BaseGetCreateEdit(reg, nameof(Edit));

        [HttpPost]
        public ActionResult Create(ClientesViewModel model) => BasePostCreateEdit(model, nameof(Create));

        [HttpPost]
        public ActionResult Edit(ClientesViewModel model) => BasePostCreateEdit(model, nameof(Edit));

        #endregion CREATE/EDIT
    }
}
