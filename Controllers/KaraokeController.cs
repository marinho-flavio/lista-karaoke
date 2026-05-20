using ListaKaraoke.Server.Data;
using ListaKaraoke.Server.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ListaKaraoke.Server.Controllers;

[ApiController]
[Route("api/[controller]")]
public class KaraokeController : ControllerBase
{
    private readonly AppDbContext _context;

    public KaraokeController(AppDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<IActionResult> Get([FromQuery] string? filtro, [FromQuery] int pagina = 1, [FromQuery] int tamanhoPagina = 50)
    {
        var query = _context.Musicas.AsQueryable();

        if (!string.IsNullOrWhiteSpace(filtro))
        {
            filtro = filtro.ToLower();
            
            if (filtro.Length == 1 && char.IsLetter(filtro[0]))
            {
                // Se for apenas uma letra, busca artistas que COMEÇAM com ela
                query = query.Where(m => m.Cantor.ToLower().StartsWith(filtro));
            }
            else
            {
                // Busca geral para termos maiores
                query = query.Where(m => 
                    m.Cantor.ToLower().Contains(filtro) || 
                    m.Titulo.ToLower().Contains(filtro) ||
                    m.Codigo.Contains(filtro));
            }
        }

        var totalItens = await query.CountAsync();
        var musicas = await query
            .OrderBy(m => m.Cantor)
            .ThenBy(m => m.Titulo)
            .Skip((pagina - 1) * tamanhoPagina)
            .Take(tamanhoPagina)
            .ToListAsync();

        return Ok(new {
            Total = totalItens,
            Pagina = pagina,
            TamanhoPagina = tamanhoPagina,
            Itens = musicas
        });
    }
}
