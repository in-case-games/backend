namespace CaseApplication.WebClient
{
    public class PostEntityModel<T>
    {
        public string PostUrl { get; set; } = null!;
        public T? PostContent { get; set; }

    }
}
