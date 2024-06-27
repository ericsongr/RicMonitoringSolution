using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using RicEntityFramework.Helpers;
using RicEntityFramework.Interfaces;
using RicEntityFramework.RicXplorer.Interfaces;
using RicEntityFramework.RicXplorer.Repositories;
using RicModel.RicXplorer;
using RicModel.RoomRent.Dtos;
using RicMonitoringAPI.Common.Model;

namespace RicMonitoringAPI.RicXplorer.Controllers
{
    [Route("api/guest-check-list")]
    [ApiController]
    public class CheckListForCheckInOutGuestController : ControllerBase
    {
        private readonly ICheckListForCheckInOutGuestRepository _checkListForCheckInOutGuestRepository;
        private readonly ILookupTypeItemRepository _lookupTypeItemRepository;
        private readonly ITypeHelperService _typeHelperService;
        private readonly IMapper _mapper;


        public CheckListForCheckInOutGuestController(
            ICheckListForCheckInOutGuestRepository checkListForCheckInOutGuestRepository,
            ILookupTypeItemRepository lookupTypeItemRepository,
            ITypeHelperService typeHelperService,
            IMapper mapper)
        {
            _checkListForCheckInOutGuestRepository = checkListForCheckInOutGuestRepository ?? throw new ArgumentNullException(nameof(checkListForCheckInOutGuestRepository));
            _lookupTypeItemRepository = lookupTypeItemRepository ?? throw new ArgumentNullException(nameof(lookupTypeItemRepository));
            _typeHelperService = typeHelperService ?? throw new ArgumentNullException(nameof(typeHelperService));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        [HttpGet("{id}", Name = "GetGuestCheckList")]
        public async Task<IActionResult> GetGuestCheckList(int id, int lookupId, [FromQuery] string fields)
        {

            if (!_typeHelperService.TypeHasProperties<LookupTypeItemDto>(fields))
            {
                return BadRequest();
            }

            var saveLookupItemRepo = _checkListForCheckInOutGuestRepository.FindBy(o => o.LookupId == lookupId && o.IsIncluded);

            var lookupTypeItemRepo = _lookupTypeItemRepository.FindBy(o => o.LookupTypeId == id && !o.IsDeleted).OrderBy(o => o.Description).ToList();
            if (!lookupTypeItemRepo.Any())
            {
                return NotFound();
            }

            lookupTypeItemRepo.ForEach(item =>
            {
                item.IsActive = saveLookupItemRepo.Any(o => o.LookupTypeItemId == item.Id);
            });

            var lookupTypeItems = _mapper.Map<IEnumerable<LookupTypeItemDto>>(lookupTypeItemRepo);
           
            return Ok(new BaseRestApiModel
            {
                Payload = lookupTypeItems.ShapeData(fields),
                Errors = new List<BaseError>(),
                StatusCode = (int)HttpStatusCode.OK
            });

        }

        [HttpPost("save")]
        public IActionResult Save(GuestCheckListDto model)
        {
            var repo = _checkListForCheckInOutGuestRepository
                .GetSingleAsync(o => o.LookupId == model.LookupId && o.LookupTypeItemId == model.LookupTypeItemId)
                .GetAwaiter().GetResult();
            if (repo != null)
            {
                repo.IsIncluded = model.IsIncluded;
            }
            else
            {
                var entity = new CheckListForCheckInOutGuest
                {
                    LookupId = model.LookupId,
                    LookupTypeItemId = model.LookupTypeItemId,
                    IsIncluded = model.IsIncluded,
                };

                _checkListForCheckInOutGuestRepository.Add(entity);
            }

            _checkListForCheckInOutGuestRepository.Commit();

            return Ok(new BaseRestApiModel
            {
                Payload = "success",
                Errors = new List<BaseError>(),
                StatusCode = (int)HttpStatusCode.OK
            });
        }

        [HttpPost("save-all")]
        public IActionResult SaveAll(GuestAllCheckListDto model)
        {

            var lookupTypeItemRepo = _lookupTypeItemRepository.FindBy(o => o.LookupTypeId == model.Id && !o.IsDeleted).ToList();
            if (!lookupTypeItemRepo.Any())
            {
                return NotFound();
            }

            lookupTypeItemRepo.ForEach(item =>
            {
                //item.IsActive = saveLookupItemRepo.Any(o => o.LookupTypeItemId == item.Id);
                var repo = _checkListForCheckInOutGuestRepository
                    .GetSingleAsync(o => o.LookupId == model.LookupId && o.LookupTypeItemId == item.Id)
                    .GetAwaiter().GetResult();
                if (repo != null)
                {
                    repo.IsIncluded = model.IsAllIncluded;
                }
                else
                {
                    var entity = new CheckListForCheckInOutGuest
                    {
                        LookupId = model.LookupId,
                        LookupTypeItemId = item.Id,
                        IsIncluded = model.IsAllIncluded,
                    };

                    _checkListForCheckInOutGuestRepository.Add(entity);
                }
            });


            

            _checkListForCheckInOutGuestRepository.Commit();

            return Ok(new BaseRestApiModel
            {
                Payload = "success",
                Errors = new List<BaseError>(),
                StatusCode = (int)HttpStatusCode.OK
            });
        }
    }
}