using Microsoft.AspNetCore.Identity;

namespace MyMvcApp.Models;

public class AccountViewModel
{
    public Guid? id { get; set; } =Guid.NewGuid(); 

    public string? name { get; set; }

    public string? password { get; set; }

    public string? email{ get; set; }


}
