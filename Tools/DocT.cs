using ClienteCadastroApplication.Entities;
using LinqKit;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.ComponentModel.DataAnnotations;
using System.Drawing;
using System.Linq;
using System.Text;

namespace ClienteCadastroApplication.Tools
{
    public static class DocT
    {
        #region GENERAL HELPERS

        public const string FirstEmptyText = "Selecione";

        //------------------------------------------------------------------------------------------------

        public static List<T> ToList<T>(Type enumType, bool removeNone = false)
        {
            var list = Enum.GetValues(enumType).Cast<T>();
            if (removeNone == true) list = list.Where(x => x.ToString().ToUpper() != "NONE");
            return list.ToList();
        }

        public static List<T> ToList<T>(bool removeNone = false) where T : struct => ToList<T>(typeof(T), removeNone);

        public static Dictionary<TEnum, TVals> GetDict<TEnum, TVals>(Func<TEnum, TVals> funcGetVal, IEnumerable<TEnum> list = null) where TEnum : struct
            => (list ?? ToList<TEnum>()).ToDictionary(x => x, x => funcGetVal(x));

        //------------------------------------------------------------------------------------------------

        public static SelectListItem[] GetSelectList(
            List<CodName> list
            , string value = null
            , string textFirstEmpty = FirstEmptyText
            , bool descWithKey = true
            )
        {
            value = value ?? string.Empty;

            // Be careful: do not change the input list anyway.
            var finalList = list.AsEnumerable();
            if (string.IsNullOrEmpty(textFirstEmpty) == false)
                finalList = new[] {
                    new CodName() {
                        Codigo = "",
                        Descricao = textFirstEmpty,
                    }
                }
                .Union(list);

            return finalList
                .Select(x => new SelectListItem()
                {
                    Value = x.Codigo,
                    Text = string.IsNullOrEmpty(x.Codigo) ? x.Descricao : $"{x.Codigo} - {x.Descricao}",
                    Selected = x.Codigo == value,
                })
                .ToArray();
        }

        //------------------------------------------------------------------------------------------------
        // Mask

        public static string ApplyDocMask(this string str, Tipo Tp = Tipo.None)
        {
            var isFisica = Tp == Tipo.Fisica;
            var isJuridica = Tp == Tipo.Juridica;

            if (string.IsNullOrEmpty(str)) return string.Empty;
            if (!isFisica && !isJuridica) return str;

            var temp = str;
            str = new string(str.Where(char.IsDigit).ToArray());

            return isFisica 
                ? $"{str.Substring(0, 3)}.{str.Substring(3, 3)}.{str.Substring(6, 3)}-{str.Substring(9, 2)}" 
                : $"{str.Substring(0, 2)}.{str.Substring(2, 3)}.{str.Substring(5, 3)}/{str.Substring(8, 4)}-{str.Substring(12, 2)}"
                ;
        }

        public static string RemoveMask(this string str)
        {
            if (string.IsNullOrEmpty(str)) return string.Empty;

            char[] caract = str.ToCharArray();
            StringBuilder val = new StringBuilder();
            caract.ForEach(x =>
            {
                if (char.IsDigit(x)) val.Append(x);
            });

            return val.ToString();
        }

        #endregion GENERAL HELPERS


        //------------------------------------------------------------------------------------------------

        #region Tipo

        public static Tipo[] DefTipo = new[]
        {
            Tipo.Fisica,
            Tipo.Juridica,
        };

        public enum Tipo
        {
            None,
            Fisica,
            Juridica,
        }

        public static (string Flag, string Desc) GetVals(this Tipo arg)
        {
            switch (arg)
            {
                case Tipo.None: return ("", "");
                case Tipo.Fisica: return ("PF", "Pessoa Física");
                case Tipo.Juridica: return ("PJ", "Pessoa Jurídica");
                default:
                    return default;
            }
        }

        public static Dictionary<Tipo, (string Flag, string Desc)> GetTipoDict(IEnumerable<Tipo> list = null) => GetDict(GetVals, list);


        public static string GetFlag(this Tipo arg) => arg.GetVals().Flag;
        public static string GetDesc(this Tipo arg) => arg.GetVals().Desc;

        //------------------------------------------------------------------------------------------------

        public static List<CodName> GetDropdownTipo(IEnumerable<Tipo> list = null)
        {
            var _list = list.ToList();

            return GetTipoDict(_list.Where(x => x != Tipo.None))
                .Select(x => new CodName()
                {
                    Codigo = x.Value.Flag,
                    Descricao = x.Value.Desc,
                })
                .ToList();
        }

        public static SelectListItem[] GetSelListTipo(IEnumerable<Tipo> list = null, string textFirstEmpty = "Selecione", bool descWithKey = false)
            => GetSelectList(GetDropdownTipo(list), null, textFirstEmpty, descWithKey);

        //------------------------------------------------------------------------------------------------

        #endregion Tipo

        //------------------------------------------------------------------------------------------------
    }
}
