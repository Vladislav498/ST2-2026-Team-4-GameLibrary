using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;


namespace ST2_2026_Team_4_GameLibrary.Models;
public class User : IdentityUser
//polzvame IdentityUser za da polzvame vgradenite funkcii kato UserName, Email, PasswordHash, PhoneNumber i roli
//promenqm username-a samo zasega

{


    //Regexove ot chasa
    // Букви (латиница и кирилица), интервал, апостроф, точка и тире. Unicode escape-ите работят и в JS regex.
    // 2 до 40 символа*
    public const string NamePattern = "^[A-Za-z\u0400-\u04FF][A-Za-z\u0400-\u04FF '.-]{1,39}$";

 // Незадължителен "+" и 7–15 цифри (международен формат E.164, напр. +359888123456 или 0888123456).
 public const string PhonePattern = @"^\+?[0-9]{7,15}$";

 public const string EmailPattern = @"^[A-Za-z0-9._%+\-]+@[A-Za-z0-9\-]+(\.[A-Za-z0-9\-]+)*\.[A-Za-z]{2,}$";


	[Display(Name = "Име")]
	[Required(ErrorMessage = "Името е задължително.")]
	[StringLength(40, MinimumLength = 2, ErrorMessage = "Името може да съдържа от 2 до 40 символа.")]
	[RegularExpression(NamePattern, ErrorMessage = "Позволени са само букви, интервал, тире, апостроф и точка.")]
    
 
    // trqbva da napravim UserController + metod IsNameAvailable, koito da proverqva dali imeto e svobodno
    public override string? UserName
    {
        get => base.UserName;
        set => base.UserName = value;
    }

    
    public List<Library> Libraries { get; set; } = new();
}

