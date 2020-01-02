namespace DAL.Entities
{
    public class ArticleTeg
    {
        public int ArticleId { get; set; }
        public Article Article { get; set; }

        public int TegId { get; set; }
        public Teg Teg { get; set; }
    }
}
