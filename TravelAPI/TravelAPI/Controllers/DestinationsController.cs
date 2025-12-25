/*
 * ===================================================================================
 * TRABALHO PRÁTICO: Integração de Sistemas de Informação (ISI)
 * -----------------------------------------------------------------------------------
 * Nome: Joel Oliveira Faria
 * Número: a28001
 * Curso: Engenharia de Sistemas Informáticos
 * Ano Letivo: 2025/2026
 * -----------------------------------------------------------------------------------
 * Ficheiro: DestinationsController.cs
 * Descrição: Controlador responsável pela gestão (CRUD) dos destinos turísticos.
 * ===================================================================================
 */
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TravelAPI.Data;
using TravelAPI.DTOs;
using TravelAPI.Models;


namespace TravelAPI.Controllers
{
    [Route("travel/destinations")]
    [ApiController]
    public class DestinationsController : ControllerBase
    {
        private readonly TravelPlannerContext _context;

        public DestinationsController(TravelPlannerContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Obtém uma lista de todos os destinos disponíveis.
        /// Permite filtrar por cidade ou país através de um termo de pesquisa.
        /// </summary>
        /// <param name="search">Texto opcional para filtrar por Cidade ou País.</param>
        /// <returns>Uma lista de objetos DestinationDto.</returns>
        /// GET: travel/destinations?search=Paris
        [HttpGet]
        public async Task<ActionResult<IEnumerable<DestinationDto>>> GetDestinations([FromQuery] string? search)
        {
            // Inicia a query base
            var query = _context.Destinations.AsQueryable();

            // Aplica o filtro se o utilizador escreveu algo na pesquisa
            if (!string.IsNullOrEmpty(search))
            {
                query = query.Where(d => d.City!.Contains(search) || d.Country!.Contains(search));
            }

            // Projeta os dados para o DTO (seleciona apenas o necessário)
            var destinations = await query
                .Select(d => new DestinationDto
                {
                    Id = d.Id,
                    City = d.City,
                    Country = d.Country,
                    Description = d.Description
                })
                .ToListAsync();

            return Ok(destinations);
        }

        /// <summary>
        /// Obtém os detalhes de um destino específico pelo seu ID único.
        /// </summary>
        /// <param name="id">O ID do destino a pesquisar.</param>
        /// <returns>O objeto DestinationDto se encontrado, ou NotFound.</returns>
        // GET: travel/destinations/5
        [HttpGet("{id}")]
        public async Task<ActionResult<DestinationDto>> GetDestination(int id)
        {
            var destination = await _context.Destinations.FindAsync(id);

            if (destination == null)
            {
                return NotFound();
            }

            // Converter Model -> DTO manualmente para resposta
            var destinationDto = new DestinationDto
            {
                Id = destination.Id,
                City = destination.City,
                Country = destination.Country,
                Description = destination.Description
            };

            return Ok(destinationDto);
        }

        /// <summary>
        /// Cria um novo destino na base de dados.
        /// </summary>
        /// <param name="createDto">Os dados do novo destino (Cidade, País, Descrição).</param>
        /// <returns>O destino criado com o código 201 Created.</returns>
        // POST: travel/destinations
        [HttpPost]
        public async Task<ActionResult<DestinationDto>> PostDestination(CreateDestinationDto createDto)
        {
            // Mapeamento DTO -> Entidade de Domínio
            var destination = new Destination
            {
                City = createDto.City,
                Country = createDto.Country,
                Description = createDto.Description
            };

            _context.Destinations.Add(destination);
            await _context.SaveChangesAsync();

            // Preparar o DTO de retorno com o ID gerado pela BD
            var resultDto = new DestinationDto
            {
                Id = destination.Id,
                City = destination.City,
                Country = destination.Country,
                Description = destination.Description
            };

            // Retorna 201 Created e o cabeçalho Location apontando para o GET
            return CreatedAtAction(nameof(GetDestination), new { id = destination.Id }, resultDto);
        }

        /// <summary>
        /// Atualiza os dados de um destino existente.
        /// </summary>
        /// <param name="id">O ID do destino a atualizar.</param>
        /// <param name="updateDto">Os novos dados do destino.</param>
        /// <returns>NoContent (204) se tiver sucesso, ou NotFound se o ID não existir.</returns>
        // PUT: travel/destinations/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutDestination(int id, CreateDestinationDto updateDto)
        {
            var destination = await _context.Destinations.FindAsync(id);
            if (destination == null)
            {
                return NotFound();
            }

            // Atualiza as propriedades da entidade existente
            destination.City = updateDto.City;
            destination.Country = updateDto.Country;
            destination.Description = updateDto.Description;

            await _context.SaveChangesAsync();

            return NoContent();
        }

        /// <summary>
        /// Remove um destino da base de dados permanentemente.
        /// </summary>
        /// <param name="id">O ID do destino a apagar.</param>
        /// <returns>NoContent (204) em caso de sucesso.</returns>
        // DELETE: travel/destinations/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDestination(int id)
        {
            var destination = await _context.Destinations.FindAsync(id);
            if (destination == null)
            {
                return NotFound();
            }

            _context.Destinations.Remove(destination);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}