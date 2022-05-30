using System.ComponentModel.DataAnnotations;
using TestWebAPI.SharedKernel;
using TestWebAPI.SharedKernel.Interfaces;

namespace TestWebAPI.Core.UserAggregate;
public class User : BaseEntity, IAggregateRoot {

    [RegularExpression(@"^[A-Za-z0-9]+$",
         ErrorMessage = "Only latin characters and numbers are allowed")]
    public string Login { get; set; } = string.Empty;

    [RegularExpression(@"^[A-Za-z0-9]+$",
         ErrorMessage = "Only latin characters and numbers are allowed")]
    public string Password { get; set; } = string.Empty;

    [RegularExpression(@"^[A-Za-zА-Яа-я]+$",
         ErrorMessage = "Only latin and Cyrillic  characters")]
    public string Name { get; set; } = string.Empty;
    public int Gender { get; set; }
    public DateTime? Birthday { get; set; }
    public bool Admin { get; set; }
    public DateTime CreatedOn { get; set; }
    public string CreatedBy { get; set; } = string.Empty;
    public DateTime? ModifiedOn { get; set; }
    public string ModifiedBy { get; set; } = string.Empty;
    public DateTime? RevokedOn { get; set; }
    public string RevokedBy { get; set; } = string.Empty;
}
