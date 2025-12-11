using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MyMvcApp.Models;

public class AccountController : Controller
{
    private readonly AppDbContext _db;

    public AccountController(AppDbContext db)
    {
        _db = db;
    }

    [HttpGet]
    public IActionResult Register()
    {
        return View();
    }

    [HttpPost]
    public IActionResult Register(
        string Name,
        string Password,
        string confirmPassword,
        string Email
    )
    {
        if (Password != confirmPassword)
        {
            ViewBag.Error = "密碼與確認密碼不一致";

            return View();
        }

        var existingUser = _db.account.FirstOrDefault(u => u.email == Email);
        if (existingUser != null)
        {
            ViewBag.Error = "這個 Email 已經註冊過了";
            return View();
        }
        var user = new AccountViewModel
        {
            name = Name,
            password = Password,
            email = Email,
        };
        var hasher = new PasswordHasher<AccountViewModel>();

        user.password = hasher.HashPassword(user, Password);
        _db.account.Add(user);
        _db.SaveChanges();

        return RedirectToAction("Login");
    }

    [HttpGet]
    public IActionResult Login(string message)
    {
        if (message == "PleaseLogin")
        {
            Console.WriteLine("請先登入才能使用此功能。");
            ViewBag.Message = "請先登入才能使用此功能。";
        }
        return View();
    }

    [HttpPost]
    public IActionResult Login(string Email, string Password)
    {
        var user = _db.account.FirstOrDefault(x => x.email == Email);
        if (user == null)
            return Unauthorized();

        var hasher = new PasswordHasher<AccountViewModel>();

        var result = hasher.VerifyHashedPassword(user, user.password, Password);

        if (result == PasswordVerificationResult.Success)
        {
            HttpContext.Session.SetString("UserId", user.email.ToString());
            HttpContext.Session.SetString("UserName", user.name);
            return RedirectToAction("", "Home");
        }

        if (result != PasswordVerificationResult.Success)
        {
            ViewBag.Error = "登入失敗";
            return View();
        }

        return Unauthorized();
    }

    [HttpGet]
    public IActionResult Logout()
    {
        // 清掉整個 Session
        HttpContext.Session.Clear();

        // 導回首頁或登入頁
        return RedirectToAction("Index", "Home");
    }
}
