using API.Attributes;
using API.DTO;
using AutoMapper;
using EDM.Entities;
using Microsoft.AspNetCore.Mvc;
using REPO.Interfaces;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DirectoryController : ControllerBase
    {
        private readonly IPersonRepository personrepository;
        private readonly IMapper mapper;
        private readonly ILogger<DirectoryController> logger;

        public DirectoryController(IPersonRepository personrepository,
            IMapper mapper,
            ILogger<DirectoryController> logger)
        {
            this.personrepository = personrepository;
            this.mapper = mapper;
            this.logger = logger;
        }

        [HttpPost("StorePerson")]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        public async Task<IActionResult> StorePerson([FromBody] CreatePersonDTO createDto)
        {
            try
            {
                var identificationExists = await personrepository.GetByIdentificationAsync(createDto.Identification);
                if (identificationExists != null)
                    return BadRequest($"Ya existe una persona con la identificación: {createDto.Identification}");

                var entity = mapper.Map<Person>(createDto);
                personrepository.Add(entity);
                await personrepository.SaveChangesAsync();

                logger.LogInformation($"Persona agregada: {entity.FirstName} {entity.PaternalLastname} {entity.MaternalLastname}");

                return Ok(mapper.Map<PersonDTO>(entity));
            }
            catch (Exception ex)
            {
                logger.LogError(ex, ex.Message);
                return StatusCode(500, ex.Message);
            }
        }

        [HttpDelete("DeletePersonByIdentification/{identification}")]
        public async Task<IActionResult> DeletePersonByIdentification(string identification)
        {
            try
            {
                var entity = await personrepository.GetByIdentificationAsync(identification);
                if (entity == null)
                    return NotFound($"No se encontró la persona con la identificación: {identification}");

                personrepository.Remove(entity);
                await personrepository.SaveChangesAsync();

                logger.LogInformation($"Persona eliminada: {entity.FirstName} {entity.PaternalLastname} {entity.MaternalLastname}");

                return NoContent();
            }
            catch (Exception ex)
            {
                logger.LogError(ex, ex.Message);
                return StatusCode(500, ex.Message);
            }
        }

        [HttpGet("FindPeople")]
        public async Task<IActionResult> FindPeople()
        {
            try
            {
                var result = await personrepository.GetAllAsync();

                logger.LogInformation($"Cargadas {result.Count()} personas");

                return Ok(mapper.Map<IEnumerable<PersonDTO>>(result));
            }
            catch (Exception ex)
            {
                logger.LogError(ex, ex.Message);
                return StatusCode(500, ex.Message);
            }
        }

        [HttpGet("FindPersonByIdentification/{identification}")]
        public async Task<IActionResult> FindPersonByIdentification(string identification)
        {
            try
            {
                var result = await personrepository.GetByIdentificationAsync(identification);

                if (result == null)
                    return NotFound($"No se encontró la persona con la identificación: {identification}");

                logger.LogInformation($"Cargada persona con identificación: {identification}");
                var res = mapper.Map<PersonDTO>(result);
                return Ok(res);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, ex.Message);
                return StatusCode(500, ex.Message);
            }
        }
    }
}
