namespace GTT.Application.Response
{
    public class ListExerciseLibraryResponse
    {
        public List<ExerciseLibraryResponse> ExcerciseLibrary { get; set; } = new List<ExerciseLibraryResponse>();
        public int TotalRow { get; set; }
    }

    public class ExerciseLibraryResponse
    {
        public int ExerciseId { get; set; }

        public string Name { get; set; }

        public string Image { get; set; }
    }
}
