using Challenge_Clyvo_Vet_DotNet.Data;
using Challenge_Clyvo_Vet_DotNet.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Challenge_Clyvo_Vet_DotNet.Controllers;

[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class PetsController : ControllerBase
{
    private readonly AppDbContext _context;

    public PetsController(AppDbContext context)
    {
        _context = context;
    }

    /// <summary>
    /// Lista todos os pets cadastrados
    /// </summary>
    /// <remarks>
    /// Retorna todos os pets ordenados por nome.
    /// </remarks>
    /// <response code="200">Lista retornada com sucesso</response>
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var pets = await _context.Pets
            .OrderBy(p => p.NomePet)
            .ToListAsync();

        return Ok(pets);
    }

    /// <summary>
    /// Busca um pet pelo ID
    /// </summary>
    /// <param name="id">ID único do pet</param>
    /// <response code="200">Pet encontrado</response>
    /// <response code="404">Pet não encontrado</response>
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var pet = await _context.Pets.FindAsync(id);

        if (pet == null)
            return NotFound();

        return Ok(pet);
    }

    /// <summary>
    /// Lista todos os pets de um responsável
    /// </summary>
    /// <param name="idResponsavel">ID do responsável</param>
    /// <response code="200">Pets do responsável retornados com sucesso</response>
    /// <response code="404">Nenhum pet encontrado para este responsável</response>
    [HttpGet("responsavel/{idResponsavel}")]
    public async Task<IActionResult> GetByResponsavel(int idResponsavel)
    {
        var pets = await _context.Pets
            .Where(p => p.IdResponsavel == idResponsavel)
            .OrderBy(p => p.NomePet)
            .ToListAsync();

        if (!pets.Any())
            return NotFound("Nenhum pet encontrado para este responsável.");

        return Ok(pets);
    }

    /// <summary>
    /// Lista todos os pets de uma espécie
    /// </summary>
    /// <param name="especie">Nome da espécie (ex: Cão, Gato)</param>
    /// <response code="200">Pets da espécie retornados com sucesso</response>
    /// <response code="404">Nenhum pet encontrado para esta espécie</response>
    [HttpGet("especie/{especie}")]
    public async Task<IActionResult> GetByEspecie(string especie)
    {
        var pets = await _context.Pets
            .Where(p => p.EspeciePet.ToLower() == especie.ToLower())
            .OrderBy(p => p.NomePet)
            .ToListAsync();

        if (!pets.Any())
            return NotFound($"Nenhum pet da espécie '{especie}' encontrado.");

        return Ok(pets);
    }

    /// <summary>
    /// Cadastra um novo pet
    /// </summary>
    /// <remarks>
    /// Exemplo de requisição:
    ///
    ///     POST /api/pets
    ///     {
    ///         "idResponsavel": 1,
    ///         "nomePet": "Rex",
    ///         "especiePet": "Cão",
    ///         "racaPet": "Labrador",
    ///         "dataNascimentoPet": "2020-03-15",
    ///         "statusCastrado": "N"
    ///     }
    ///
    /// O ID do pet é gerado automaticamente pelo banco de dados.
    /// StatusCastrado aceita apenas "S", "N" ou nulo.
    /// </remarks>
    /// <param name="pet">Objeto com os dados do pet</param>
    /// <response code="201">Pet criado com sucesso</response>
    /// <response code="400">Dados inválidos ou campos obrigatórios ausentes</response>
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] Pet pet)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        if (pet.StatusCastrado != null &&
            pet.StatusCastrado != "S" &&
            pet.StatusCastrado != "N")
            return BadRequest("StatusCastrado deve ser 'S', 'N' ou nulo.");

        _context.Pets.Add(pet);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetById), new { id = pet.IdPet }, pet);
    }

    /// <summary>
    /// Atualiza os dados de um pet existente
    /// </summary>
    /// <remarks>
    /// Exemplo de requisição:
    ///
    ///     PUT /api/pets/1
    ///     {
    ///         "idPet": 1,
    ///         "idResponsavel": 1,
    ///         "nomePet": "Rex",
    ///         "especiePet": "Cão",
    ///         "racaPet": "Labrador",
    ///         "dataNascimentoPet": "2020-03-15",
    ///         "statusCastrado": "S"
    ///     }
    /// </remarks>
    /// <param name="id">ID único do pet</param>
    /// <param name="pet">Objeto com os dados atualizados</param>
    /// <response code="204">Pet atualizado com sucesso</response>
    /// <response code="400">ID da URL não corresponde ao IdPet do body</response>
    /// <response code="404">Pet não encontrado</response>
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] Pet pet)
    {
        if (id != pet.IdPet)
            return BadRequest("ID da URL não corresponde ao IdPet do body.");

        if (pet.StatusCastrado != null &&
            pet.StatusCastrado != "S" &&
            pet.StatusCastrado != "N")
            return BadRequest("StatusCastrado deve ser 'S', 'N' ou nulo.");

        var exists = await _context.Pets.AnyAsync(p => p.IdPet == id);
        if (!exists)
            return NotFound();

        _context.Entry(pet).State = EntityState.Modified;
        await _context.SaveChangesAsync();

        return NoContent();
    }

    /// <summary>
    /// Remove um pet pelo ID
    /// </summary>
    /// <param name="id">ID único do pet</param>
    /// <response code="204">Pet removido com sucesso</response>
    /// <response code="404">Pet não encontrado</response>
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var pet = await _context.Pets
            .FirstOrDefaultAsync(p => p.IdPet == id);

        if (pet == null)
            return NotFound();

        _context.Pets.Remove(pet);
        await _context.SaveChangesAsync();

        return NoContent();
    }
}