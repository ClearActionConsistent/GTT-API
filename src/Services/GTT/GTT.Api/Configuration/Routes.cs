namespace GTT.Api.Configuration;

public static class Routes
{
    /// <summary>
    /// Class route 
    /// </summary>
    #region ClassManagement
    public const string ClassV1 = "v1/class";
    #endregion

    /// <summary>
    /// Excercise route 
    /// </summary>
    #region ExerciseManagement
    public const string CreateExGroup = "v1/excercise-group";
    #endregion

    /// <summary>
    /// Challenge route 
    /// </summary>
    #region ChallengeManagement
    public const string CreateChallenge = "v1/add-challenge";
    public const string GetAllChallenge = "v1/challenge";
    #endregion

    /// <summary>
    /// Exercise route 
    /// </summary>
    #region ExerciseManagement
    public const string ExerciseLibrary = "v1/excercise-library";
    public const string GetExGroup = "v1/excercise-group";
    #endregion

    /// <summary>
    /// Group route 
    /// </summary>
    #region GroupManagement
    public const string Group = "v1/groups";
    #endregion

    /// <summary>
    /// Azure storage 
    /// </summary>
    #region AzureStorage
    public const string UploadImage = "v1/azure-storage/upload-image";
    #endregion

    ///<summary>
    /// Sports Route
    /// </summary>
    #region Sports Management
    public const string Sports = "v1/sports";
    #endregion
}