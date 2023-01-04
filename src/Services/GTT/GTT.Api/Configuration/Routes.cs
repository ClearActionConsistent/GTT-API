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
    public const string CreateChallenge = "v1/insert-challenge";
    #endregion

    /// <summary>
    /// Exercise route 
    /// </summary>
    #region ExerciseManagement
    public const string ExerciseLibrary = "v1/excercise-library";
    public const string GetExGroup = "v1/excercise-group";
    #endregion
}
