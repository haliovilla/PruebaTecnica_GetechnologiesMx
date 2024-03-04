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
    public class SaleController : ControllerBase
    {
        private readonly IInvoiceRepository invoiceRepository;
        private readonly IPersonRepository personRepository;
        private readonly IMapper mapper;
        private readonly ILogger<SaleController> logger;

        public SaleController(IInvoiceRepository invoiceRepository,
            IPersonRepository personRepository,
            IMapper mapper,
            ILogger<SaleController> logger)
        {
            this.invoiceRepository = invoiceRepository;
            this.personRepository = personRepository;
            this.mapper = mapper;
            this.logger = logger;
        }

        [HttpPost("StoreInvoice")]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        public async Task<IActionResult> StoreInvoice([FromBody] CreateInvoiceDTO createDto)
        { 
            try
            {
                var person = await personRepository.GetByIdAsync(createDto.PersonId.Value);
                if (person == null)
                    return NotFound($"No se encontró la persona con el id: {createDto.PersonId}");
                
                    var entity = mapper.Map<Invoice>(createDto);
                invoiceRepository.Add(entity);
                await invoiceRepository.SaveChangesAsync();

                logger.LogInformation($"Factura agregada: {entity.Id}");

                return Ok(mapper.Map<InvoiceDTO>(entity));
            }
            catch (Exception ex)
            {
                logger.LogError(ex, ex.Message);
                return StatusCode(500, ex.Message);
            }
        }

        [HttpGet("FindInvoicesByPersonId/{personId}")]
        public async Task<IActionResult> FindInvoicesByPersonId(int personId)
        {
            try
            {
                var result = await invoiceRepository.GetByPersonIdAsync(personId);
                if (result.Count == 0)
                    return NotFound($"No se encontró ninguna factura con el id de persona: {personId}");

                return Ok(mapper.Map<List<InvoiceDTO>>(result));
            }
            catch (Exception ex)
            {
                logger.LogError(ex, ex.Message);
                return StatusCode(500, ex.Message);
            }
        }
    }
}