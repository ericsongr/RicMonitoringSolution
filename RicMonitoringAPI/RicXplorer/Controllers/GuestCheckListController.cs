using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using RicEntityFramework.RicXplorer.Interfaces;
using RicModel.RicXplorer;
using RicModel.RoomRent;
using RicMonitoringAPI.Common.Model;
using RicMonitoringAPI.Infrastructure.Helpers;
using RicMonitoringAPI.RicXplorer.ViewModels;
using GuestCheckListDto = RicMonitoringAPI.RicXplorer.ViewModels.GuestCheckListDto;

namespace RicMonitoringAPI.RicXplorer.Controllers
{
    [Route("api/guest-check-list-staff")]
    [ApiController]
    public class GuestCheckListController : ControllerBase
    {
        private readonly ICheckListForCheckInOutGuestRepository _checkListForCheckInOutGuestRepository;
        private readonly IGuestBookingDetailRepository _guestBookingDetailRepository;
        private readonly IGuestCheckListRepository _guestCheckListRepository;
        private readonly IMapper _mapper;

        public GuestCheckListController(
            ICheckListForCheckInOutGuestRepository checkListForCheckInOutGuestRepository,
            IGuestBookingDetailRepository guestBookingDetailRepository,
            IGuestCheckListRepository guestCheckListRepository,
            IMapper mapper)
        {
            _checkListForCheckInOutGuestRepository = checkListForCheckInOutGuestRepository ?? throw new ArgumentNullException(nameof(checkListForCheckInOutGuestRepository));
            _guestBookingDetailRepository = guestBookingDetailRepository ?? throw new ArgumentNullException(nameof(guestBookingDetailRepository));
            _guestCheckListRepository = guestCheckListRepository ?? throw new ArgumentNullException(nameof(guestCheckListRepository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }


        [HttpGet("{id:int}")]
        public IActionResult GetCheckList(int id, int lookupId, int guestId)
        {

            var checkLists = _checkListForCheckInOutGuestRepository
                .FindBy(o => o.LookupId == id && o.IsIncluded && o.LookupTypeItem.LookupTypes.Id == lookupId, 
                         o => o.LookupTypeItem);

            var guestSaveCheckLists = _guestCheckListRepository.FindBy(o => o.GuestBookingDetailId == guestId).ToList();

            var checkListViewModel = _mapper.Map<IEnumerable<GuestCheckListDetailDto>>(checkLists).ToList();
            checkListViewModel.ForEach(item =>
            {
                var itemModel = guestSaveCheckLists.FirstOrDefault(o => o.CheckListId == item.CheckListId);
                if (itemModel != null)
                {
                    item.IsChecked = itemModel.IsChecked;
                    item.Notes = itemModel.Notes;
                }
               
            });

            return Ok(new BaseRestApiModel
            {
                Payload = checkListViewModel,
                Errors = new List<BaseError>(),
                StatusCode = (int)HttpStatusCode.OK
            });
        }

        [HttpPost("save-guest-check-list", Name = "SaveGuestCheckList")]
        public IActionResult SaveGuestCheckList(GuestCheckListDto model)
        {
            try
            {
                model.GuestCheckListDetails.ForEach(item =>
                {
                    var itemModel = _guestCheckListRepository.GetSingleAsync(o =>
                        o.GuestBookingDetailId == model.GuestBookingDetailId && o.CheckListId == item.CheckListId)
                        .GetAwaiter().GetResult();
                    if (itemModel == null)
                    {
                        _guestCheckListRepository.Add(new GuestCheckList
                        {
                            GuestBookingDetailId = model.GuestBookingDetailId,
                            CheckListId = item.CheckListId,
                            IsChecked = item.IsChecked,
                            Notes = item.Notes
                        });
                    }
                    else
                    {
                        itemModel.IsChecked = item.IsChecked;
                        itemModel.Notes = item.Notes;
                        _guestCheckListRepository.Update(itemModel);
                    }

                });
                
                _guestCheckListRepository.Commit();

                return Ok(new BaseRestApiModel
                {
                    Payload = "Check-list saved successfully",
                    Errors = new List<BaseError>(),
                    StatusCode = (int)HttpStatusCode.OK
                });

            }
            catch (Exception ex)
            {
                return Ok(HandleApi.Exception(ex.InnerException.Message, HttpStatusCode.InternalServerError));
            }
        }
        
    }
}