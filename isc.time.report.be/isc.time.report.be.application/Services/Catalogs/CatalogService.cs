using AutoMapper;
using isc.time.report.be.application.Interfaces.Repository.Catalogs;
using isc.time.report.be.application.Interfaces.Service.Catalogs;
using isc.time.report.be.domain.Models.Response.Catalogs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace isc.time.report.be.application.Services.Catalogs
{
    public class CatalogService : ICatalogService
    {
        private readonly ICatalogRepository _catalogRepository;
        private readonly IMapper _mapper;

        public CatalogService(ICatalogRepository catalogRepository, IMapper mapper)
        {
            _catalogRepository = catalogRepository;
            _mapper = mapper;
        }

        public async Task<List<GetActivityTypeResponse>> GetActivityTypesAsync()
        {
            var result = await _catalogRepository.GetActivityTypeActivosAsync();
            return _mapper.Map<List<GetActivityTypeResponse>>(result);
        }

        public async Task<List<GetApprovalStatusResponse>> GetApprovalStatusesAsync()
        {
            var result = await _catalogRepository.GetApprovalStatusActivosAsync();
            return _mapper.Map<List<GetApprovalStatusResponse>>(result);
        }

        public async Task<List<GetGenderResponse>> GetGendersAsync()
        {
            var result = await _catalogRepository.GetGenderActivosAsync();
            return _mapper.Map<List<GetGenderResponse>>(result);
        }

        public async Task<List<GetIdentificationTypeResponse>> GetIdentificationTypesAsync()
        {
            var result = await _catalogRepository.GetIdentificationTypeActivosAsync();
            return _mapper.Map<List<GetIdentificationTypeResponse>>(result);
        }

        public async Task<List<GetNationalityResponse>> GetNationalitiesAsync()
        {
            var result = await _catalogRepository.GetNationalityActivosAsync();
            return _mapper.Map<List<GetNationalityResponse>>(result);
        }

        public async Task<List<GetPermissionTypeResponse>> GetPermissionTypesAsync()
        {
            var result = await _catalogRepository.GetPermissionTypeActivosAsync();
            return _mapper.Map<List<GetPermissionTypeResponse>>(result);
        }

        public async Task<List<GetPositionResponse>> GetPositionsAsync()
        {
            var result = await _catalogRepository.GetPositionActivosAsync();
            return _mapper.Map<List<GetPositionResponse>>(result);
        }

        public async Task<List<GetProjectStatusResponse>> GetProjectStatusesAsync()
        {
            var result = await _catalogRepository.GetProjectStatusActivosAsync();
            return _mapper.Map<List<GetProjectStatusResponse>>(result);
        }
    }
}
