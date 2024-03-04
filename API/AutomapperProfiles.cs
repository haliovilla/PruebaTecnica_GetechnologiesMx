using API.DTO;
using AutoMapper;
using EDM.Entities;

namespace API
{
    public class AutomapperProfiles : Profile
    {
        public AutomapperProfiles()
        {
            CreateMap<CreatePersonDTO, Person>();
            CreateMap<Person, PersonDTO>();

            CreateMap<CreateInvoiceDTO, Invoice>();
            CreateMap<Invoice, InvoiceDTO>();
        }
    }
}
