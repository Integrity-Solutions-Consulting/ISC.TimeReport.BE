using AutoMapper;
using isc.time.report.be.application.Interfaces.Repository.Holidays;
using isc.time.report.be.application.Interfaces.Service.Holidays;
using isc.time.report.be.domain.Entity.Holidays;
using isc.time.report.be.domain.Exceptions;
using isc.time.report.be.domain.Models.Request.Holidays;
using isc.time.report.be.domain.Models.Response.Holidays;

namespace isc.time.report.be.application.Services.Holidays
{
    public class HolidayServices : IHolidayService
    {

        private readonly IHolidayRepository holidayRepository;
        private readonly IMapper _mapper;

        public HolidayServices(IHolidayRepository repository, IMapper mapper)
        {
            holidayRepository = repository;
            _mapper = mapper;
        }


        public async Task<GetHolidayByIdResponse> GetHolidayByIdAsync(int id)
        {
            if (id <= 0)
                throw new ClientFaultException("El ID debe ser mayor a 0");
            var entity = await holidayRepository.GetHolidayByIdAsync(id);
            if (entity == null) throw new Exception("El feriado no existe");
            var holiday = _mapper.Map<GetHolidayByIdResponse>(entity);
            return holiday;
        }

        public async Task<List<GetAllHolidayResponse>> GetAllHolidayAsync()
        {
            var list = await holidayRepository.GetAllHolidayAsync();
            var holiday = _mapper.Map<List<GetAllHolidayResponse>>(list);
            return holiday;
        }


        public async Task<CreateHolidayResponse> CreateHolidayAsync(CreateHolidayRequest CreateHoliday)
        {
            //esto es un test 
            var entity = _mapper.Map<Holiday>(CreateHoliday);
            var response = await holidayRepository.CreateHolidayAsync(entity);
            var trueResponse = _mapper.Map<CreateHolidayResponse>(response);
            return trueResponse;
        }

        public async Task<UpdateHolidayResponse> UpdateHolidayAsync(UpdateHolidayRequest UpdateHoliday, int id)
        {
            var entity = _mapper.Map<Holiday>(UpdateHoliday);
            entity.Id = id;
            var response = await holidayRepository.UpdateHolidayAsync(entity);
            var TrueResponse = _mapper.Map<UpdateHolidayResponse>(response);
            return TrueResponse;

        }

        public async Task<ActiveInactiveHolidayResponse> ActivateHolidayAsync(int id)
        {
            var Response = await holidayRepository.ActivateHolidayAsync(id);
            var trueResponse = _mapper.Map<ActiveInactiveHolidayResponse>(Response);
            return trueResponse;
        }

        public async Task<ActiveInactiveHolidayResponse> InactiveHolidayAsync(int id)
        {
            var Response = await holidayRepository.InactivateHolidayAsync(id);
            var trueResponse = _mapper.Map<ActiveInactiveHolidayResponse>(Response);
            return trueResponse;
        }
    }
}
