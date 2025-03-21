using ClienteCadastroApplication.Entities;
using ClienteCadastroApplication.Models;
using ClienteCadastroApplication.Tools;
using Microsoft.EntityFrameworkCore;

namespace ClienteCadastroApplication.Services
{
    public static class ClientesServ
    {

        #region FILTER

        private static IQueryable<Clientes> BaseFilterDeleted(this IQueryable<Clientes> qry) => qry.Where(x => x.Deleted == false);

        //------------------------------------------------------------------------------------------------

        public static IQueryable<Clientes> FilterTipo(this IQueryable<Clientes> qry, string Tipo) => qry.Where(x => x.Tipo == Tipo);

        #endregion FILTER

        //------------------------------------------------------------------------------------------------

        #region BASE GET

        public static IQueryable<Clientes> GetAll(AppDbContext db) => db.Clientes;

        //------------------------------------------------------------------------------------------------

        public static IQueryable<Clientes> Get(AppDbContext db, int Id) => GetAll(db).Where(x => x.Id == Id);

        #endregion BASE GET

        //------------------------------------------------------------------------------------------------

        #region GET

        public static IQueryable<Clientes> GetByNome(AppDbContext db, string Nome) => GetAll(db).Where(x => x.Nome == Nome);

        //------------------------------------------------------------------------------------------------

        public static IQueryable<Clientes> GetByDocumento(AppDbContext db, string Documento) => GetAll(db).Where(x => x.Documento == Documento);

        //------------------------------------------------------------------------------------------------

        public static IQueryable<Clientes> GetPessoaFisica(AppDbContext db)
        {
            var Tipo = DocT.Tipo.Fisica.GetFlag();
            return GetAll(db).FilterTipo(Tipo);
        }

        public static IQueryable<Clientes> GetPessoaJuridica(AppDbContext db)
        {
            var Tipo = DocT.Tipo.Juridica.GetFlag();
            return GetAll(db).FilterTipo(Tipo);
        }

        #endregion GET

        //------------------------------------------------------------------------------------------------

        #region INDEX/SEARCH

        public static List<Clientes> GetList(AppDbContext db, ClientesViewModel model)
        {
            var qry = GetAll(db).BaseFilterDeleted();

            //------------------------------------------------------------------------------------------------

            if (!string.IsNullOrEmpty(model.TipoPessoa)) qry = qry.Where(x => x.Tipo == model.TipoPessoa);
            if (!string.IsNullOrEmpty(model.Nome)) foreach (var str in model.Nome.Split(" ").ToList()) qry = qry.Where(x => x.Nome.Contains(str));
            if (!string.IsNullOrEmpty(model.Documento)) qry = qry.Where(x => x.Documento.Contains(model.Documento));


            //------------------------------------------------------------------------------------------------

            return qry.ToList();
        }

        #endregion INDEX/SEARCH

        //------------------------------------------------------------------------------------------------

        #region MODIFY

        public static void Create(AppDbContext db, Clientes reg)
        {
            db.Add(reg);
            db.SaveChanges();
        }

        //------------------------------------------------------------------------------------------------

        public static void Edit(AppDbContext db, Clientes reg)
        {
            db.Update(reg);
            db.SaveChanges();
        }
        public static void SoftDelete(AppDbContext db, Clientes reg)
        {
            reg.Deleted = true;
            db.Update(reg);
            db.SaveChanges();
        }

        //------------------------------------------------------------------------------------------------

        public static void Delete(AppDbContext db, Clientes reg) => db.Remove(reg);

        #endregion MODIFY
    }
}
