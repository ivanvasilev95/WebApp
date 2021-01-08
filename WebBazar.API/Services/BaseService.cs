using AutoMapper;
using WebBazar.API.Data;

namespace WebBazar.API.Services
{
    public abstract class BaseService
    {
        protected readonly DataContext data;
        protected readonly IMapper mapper;
        protected BaseService(DataContext data, IMapper mapper)
        {
            this.data = data;
            this.mapper = mapper;
        }
    }
}