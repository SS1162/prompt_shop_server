using AutoMapper;
using DTO;
using Entities;
using Humanizer;
using Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public class SiteTypesService :ISiteTypesService
    {

        private readonly ISiteTypesRepository _siteTypesRepository;
        private readonly IMapper _mapper;

        public SiteTypesService(ISiteTypesRepository siteTypesRepository, IMapper mapper)
        {
            this._siteTypesRepository = siteTypesRepository;
            this._mapper = mapper;
        }


        public async Task<IEnumerable<SiteTypeDTO>?> GetAllSiteTypesServise()
        {
            var siteTypes = await _siteTypesRepository.GetAllSiteTypesReposetory();

            return _mapper.Map<IEnumerable<SiteTypeDTO>>(siteTypes);

        }
        public async Task<SiteTypeDTO?> GetSiteTypesByIdServise(long id)
        {
            SiteType? siteType = await _siteTypesRepository.GetSiteTypeByIdReposetory(id);
            return _mapper.Map<SiteTypeDTO>(siteType);
        }

        public async Task<Resulte<SiteTypeDTO>> UpdateSiteTypesByMngServise(long id, SiteTypeDTO dto)
        {
            if (id != dto.SiteTypeID)
            {
                return Resulte<SiteTypeDTO>.Failure("The ide's are diffrent");
            }
            SiteType siteType = _mapper.Map<SiteType>(dto);
            await _siteTypesRepository.UpdateSiteTypeByMngReposetory(id, siteType);

            return Resulte<SiteTypeDTO>.Success(null);
        }

        public async Task<Resulte<SiteTypeDTO>> DeleteSiteTypeServise(long id)
        {
           SiteType? siteType = await _siteTypesRepository.GetSiteTypeByIdReposetory(id);
            if (siteType==null)
            {
                return Resulte<SiteTypeDTO>.Failure("The site type isint insist ");
            }
            await _siteTypesRepository.DeleteSiteTypeByMngReposetory(id);

            return Resulte<SiteTypeDTO>.Success(null);
        }


        


    }
}
