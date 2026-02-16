using AutoMapper;
using DTO;
using Entities;
using Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Services
{
    public class BasicSitesServise :IBasicSitesServise
    {
        private readonly ISiteTypesRepository _siteTypesRepository;
        private readonly IBasicSitesReposetory _basicSitesReposetory;
        private readonly IMapper _mapper;
        private readonly IPlatformsReposetory _platformsReposetory;
        public BasicSitesServise(IMapper mapper, IBasicSitesReposetory basicSitesReposetory, ISiteTypesRepository siteTypesRepository
            ,IPlatformsReposetory platformsReposetory)
        {
            this._mapper = mapper;
            this._basicSitesReposetory = basicSitesReposetory;
            this._siteTypesRepository = siteTypesRepository;
            this._platformsReposetory = platformsReposetory;
        }


        async public Task<BasicSiteDTO> GetByIDbasicSiteServise(long id)
        {
            BasicSite? basicSiteFromReposetory = await _basicSitesReposetory.GetByIDBasicSiteReposetory(id);
            return _mapper.Map<BasicSiteDTO>(basicSiteFromReposetory);

        }

        async public Task<Resulte<BasicSiteDTO?>> UpdateBasicSiteServise(long id, UpdateBasicSiteDTO basicSiteToUpdate)
        {
            if(id!= basicSiteToUpdate.BasicSiteID)
            {
                return Resulte<BasicSiteDTO?>.Failure("The IDs are diffrent");
            }
            BasicSite? checkIfBasicSiteInsist = await _basicSitesReposetory.GetByIDBasicSiteReposetory(id);
            Platform ? platformToCheckId = await _platformsReposetory.GetByIDPlatformsReposetory(basicSiteToUpdate.PlatformID);
            SiteType? checkIfIsEmptySiteType = await _siteTypesRepository.GetSiteTypeByIdReposetory(basicSiteToUpdate.SiteTypeID);
            if (checkIfBasicSiteInsist == null)
            {
                return Resulte<BasicSiteDTO?>.Failure("The Basic Site ID isn't insist");
            }
            if (checkIfIsEmptySiteType == null)
            {
                return Resulte<BasicSiteDTO?>.Failure("The Type syte ID isn't insist");
            }
            else if (platformToCheckId == null)
            {
                return Resulte<BasicSiteDTO?>.Failure("The platforme ID isn't insist");
            }
            else if (checkIfIsEmptySiteType.SiteTypeName != "Empty" && basicSiteToUpdate.UserDescreption != null)
            {
                return Resulte<BasicSiteDTO?>.Failure("The Type syte ID isn't empty so you shudn't include user descreption");
            }
            else if (checkIfIsEmptySiteType.SiteTypeName == "Empty" && basicSiteToUpdate.UserDescreption == null)
            {
                return Resulte<BasicSiteDTO?>.Failure("The Type syte ID is empty so you must include user descreption");
            }

            BasicSite basicSiteToReposetory = _mapper.Map<BasicSite>(basicSiteToUpdate);

            await _basicSitesReposetory.UpdateBasicSiteReposetory(id, basicSiteToReposetory);
            return Resulte<BasicSiteDTO?>.Success(null);
        }


        async public Task<Resulte<BasicSiteDTO?>> AddBasicSiteServise(AddBasicSiteDTO BasicSiteToAdd)
        {
            Platform? platformToCheckId = await _platformsReposetory.GetByIDPlatformsReposetory(BasicSiteToAdd.PlatformID);
            SiteType? checkIfIsEmptySiteType = await _siteTypesRepository.GetSiteTypeByIdReposetory(BasicSiteToAdd.SiteTypeID);
            if (checkIfIsEmptySiteType == null)
            {
                return Resulte<BasicSiteDTO?>.Failure("The Type syte ID isn't insist");
            }
            else if (checkIfIsEmptySiteType == null)
            {
                return Resulte<BasicSiteDTO?>.Failure("The Type syte ID isn't insist");
            }
            else if (checkIfIsEmptySiteType.SiteTypeName != "Empty" && BasicSiteToAdd.UserDescreption != null)
            {
                return Resulte<BasicSiteDTO?>.Failure("The Type syte ID isn't empty so you shudn't include user descreption");
            }
            else if (checkIfIsEmptySiteType.SiteTypeName == "Empty" && BasicSiteToAdd.UserDescreption == null)
            {
                return Resulte<BasicSiteDTO?>.Failure("The Type syte ID is empty so you must include user descreption");
            }
            BasicSite basicSiteToReposetory = _mapper.Map<BasicSite>(BasicSiteToAdd);

            BasicSite basicSiteFromReposetory = await _basicSitesReposetory.AddBasicSiteReposetory(basicSiteToReposetory);

            BasicSiteDTO objectThatCreated = _mapper.Map<BasicSiteDTO>(basicSiteFromReposetory);

            return Resulte<BasicSiteDTO?>.Success(objectThatCreated);
        }
    }
}
