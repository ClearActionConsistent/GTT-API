namespace GTT.Application.Extensions
{
    public class Helpers
    {
        public static string BuildErrorMessage(Exception ex)
        {
            return $"Error: {ex.Message} {(ex.InnerException != null ? ex.InnerException.Message : string.Empty)}";
        }
    }
}
