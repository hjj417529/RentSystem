using System.ComponentModel.DataAnnotations;

namespace MyMvcApp.Models;

public class StockViewModel
{
    [Key]
    public Guid? id{get;set;} = Guid.NewGuid(); 
    public string? type {get;set;}
    public string? name {get;set;}
    public string? status {get;set;}
    public string? owner {get;set;}

}