using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using DBWeb;
namespace razor_ef.Pages;

public class IndexModel : PageModel
{
    private readonly ILogger<IndexModel> _logger;
    private readonly MyBlogContext _myBlog;
    public IndexModel(ILogger<IndexModel> logger, MyBlogContext myBlog)
    {
        _myBlog = myBlog;
        _logger = logger;
    }

    public void OnGet()
    {
        var ports = (from a in _myBlog.articles
                    orderby a.Created descending 
                    select a).ToList();
        ViewData["ports"] = ports;
    }
}
