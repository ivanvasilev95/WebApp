using AutoMapper;
using WebApp.API.Data;

namespace WebApp.API.Services
{
    public abstract class BaseService
    {
        protected readonly DataContext _context;
        protected readonly IMapper _mapper;
        protected BaseService(DataContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
    }
}