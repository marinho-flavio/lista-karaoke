using System.ComponentModel.DataAnnotations;

namespace ListaKaraoke.Server.Models;

public class Musica
{
    [Key]
    public int Id { get; set; }
    
    [Required]
    [MaxLength(200)]
    public string Cantor { get; set; } = string.Empty;
    
    [Required]
    [MaxLength(50)]
    public string Codigo { get; set; } = string.Empty;
    
    [Required]
    [MaxLength(255)]
    public string Titulo { get; set; } = string.Empty;
    
    public string? InicioLetra { get; set; }
}
