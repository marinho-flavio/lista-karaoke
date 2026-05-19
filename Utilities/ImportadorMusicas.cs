using System.Globalization;
using CsvHelper;
using CsvHelper.Configuration;
using ListaKaraoke.Server.Data;
using ListaKaraoke.Server.Models;
using Microsoft.EntityFrameworkCore;

namespace ListaKaraoke.Server.Utilities;

public static class ImportadorMusicas
{
    public static async Task ImportarAsync(AppDbContext context, string csvPath)
    {
        if (await context.Musicas.AnyAsync())
        {
            Console.WriteLine("O banco já contém dados. Importação abortada.");
            return;
        }

        var config = new CsvConfiguration(CultureInfo.InvariantCulture)
        {
            HasHeaderRecord = true,
            HeaderValidated = null,
            MissingFieldFound = null
        };

        using var reader = new StreamReader(csvPath);
        using var csv = new CsvReader(reader, config);
        
        var registros = csv.GetRecords<MusicaRecord>();
        
        var musicas = new List<Musica>();
        int contador = 0;

        foreach (var r in registros)
        {
            musicas.Add(new Musica
            {
                Cantor = r.Cantor,
                Codigo = r.Codigo,
                Titulo = r.Titulo,
                InicioLetra = r.InicioLetra
            });

            contador++;
            if (contador % 1000 == 0)
            {
                await context.Musicas.AddRangeAsync(musicas);
                await context.SaveChangesAsync();
                musicas.Clear();
                Console.WriteLine($"Importadas {contador} músicas...");
            }
        }

        if (musicas.Any())
        {
            await context.Musicas.AddRangeAsync(musicas);
            await context.SaveChangesAsync();
        }

        Console.WriteLine($"Sucesso! Total de {contador} músicas importadas.");
    }
}

public class MusicaRecord
{
    public string Cantor { get; set; } = string.Empty;
    public string Codigo { get; set; } = string.Empty;
    public string Titulo { get; set; } = string.Empty;
    public string InicioLetra { get; set; } = string.Empty;
}
