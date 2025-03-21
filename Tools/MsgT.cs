namespace ClienteCadastroApplication.Tools
{
    public static class MsgT
    {
        #region DEFAULT MESSAGES

        public static string DefRequired = "Campo Obrigatório.";
        public static string RegNotFound = "Registro não Encontrado.";
        public static string RegAlreadyExists = "Já existe registro com Id.";

        #endregion DEFAULT MESSAGES

        //------------------------------------------------------------------------------------------------

        #region COLORS

        public enum DefColor
        {
            None,
            primary,// Blue
            secondary,// Light-Grey
            success,// Green
            danger,// Red
            warning,// Yellow
            info,// Light-Blue
            light,// White
            dark,// Dark-Grey
        }

        #endregion COLORS

        //------------------------------------------------------------------------------------------------

    }
}
