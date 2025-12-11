using Microsoft.AspNetCore.Mvc;

namespace MyMvcApp.Controllers;

public class StockController : Controller
{
    private readonly AppDbContext _db;

    public StockController(AppDbContext db)
    {
        _db = db;
    }

    public IActionResult Index()
    {
        var list = _db.stock.ToList(); 
        return View(list); 
    }
}
