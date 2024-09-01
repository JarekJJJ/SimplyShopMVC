namespace SimplyShopMVC.Web.Models;

public class ArticleBoxParams
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string ShortDescription { get; set; }
    public string Content { get; set; }
    public string CategoryTag { get; set; }
    public DateTime Create { get; set; }
}
