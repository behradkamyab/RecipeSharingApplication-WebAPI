namespace ServicesLibrary.Responses
{
    public class RecipeManagerResponse<T>
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public T? Data { get; set; }
    }
}
