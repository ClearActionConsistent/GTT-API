namespace GTT.Domain.Enums
{
    public class Helper
    {
        public static string BuildErrorMessage(Exception ex)
        {
            return $"Error: {ex.Message} {(ex.InnerException != null ? ex.InnerException.Message : string.Empty)}";
        }
    }
}
