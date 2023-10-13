using System.ComponentModel.DataAnnotations;

namespace LettersRegistration.WebAPI.Domain
{
    public class Letter
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public DateTime RegistrationTime { get; set; }
        [Required]
        public string Sender { get; set; }

        [Required]
        public string Addressee { get; set; }
        public string Content { get; set; } = string.Empty;
    }
}
