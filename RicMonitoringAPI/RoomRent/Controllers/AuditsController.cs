using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RicEntityFramework.Helpers;
using RicEntityFramework.Interfaces;
using RicEntityFramework.RoomRent.Interfaces.IAudits;
using RicEntityFramework.RoomRent.Interfaces.IPropertyMappings.IAudits;
using RicModel.RoomRent.Audits;
using RicModel.RoomRent.Dtos.Audits;
using RicMonitoringAPI.Common.Model;

namespace RicMonitoringAPI.RoomRent.Controllers
{
    //[AllowAnonymous]
    [Authorize(Policy = "SuperAndAdmin")]
    [Route("api/audits")]
    public class AuditsController : Controller
    {
        private readonly ITypeHelperService _typeHelperService;

        private readonly IAuditRenterRepository _auditRenterRepository;
        private readonly IAuditRenterPropertyMappingService _auditRenterPropertyMappingService;
        //
        private readonly IAuditRoomPropertyMappingService _auditRoomPropertyMappingService;
        private readonly IAuditRoomRepository _auditRoomRenterRepository;
        //
        private readonly IAuditRentTransactionPaymentRepository _auditRentTransactionPaymentRepository;
        private readonly IAuditRentTransactionPaymentPropertyMappingService _auditRentTransactionPaymentPropertyMappingService;
        //
        private readonly IAuditRentTransactionRepository _auditRentTransactionRepository;
        private readonly IAuditRentTransactionPropertyMappingService _auditRentTransactionPropertyMappingService;

        public AuditsController(
            ITypeHelperService typeHelperService,
            //
            IAuditRenterRepository auditRenterRepository,
            IAuditRoomPropertyMappingService auditRoomPropertyMappingService,
            //
            IAuditRoomRepository auditRoomRenterRepository,
            IAuditRentTransactionPaymentRepository auditRentTransactionPaymentRepository,
            //
            IAuditRenterPropertyMappingService auditRenterPropertyMappingService,
            IAuditRentTransactionPaymentPropertyMappingService auditRentTransactionPaymentPropertyMappingService,
            //
            IAuditRentTransactionRepository auditRentTransactionRepository,
            IAuditRentTransactionPropertyMappingService auditRentTransactionPropertyMappingService)
        {
            _typeHelperService = typeHelperService ?? throw new ArgumentNullException(nameof(typeHelperService));
            //
            _auditRenterRepository = auditRenterRepository ?? throw new ArgumentNullException(nameof(auditRenterRepository));
            _auditRenterPropertyMappingService = auditRenterPropertyMappingService ?? throw new ArgumentNullException(nameof(auditRenterPropertyMappingService));
            //
            _auditRoomPropertyMappingService = auditRoomPropertyMappingService ?? throw new ArgumentNullException(nameof(auditRoomRenterRepository));
            _auditRoomRenterRepository = auditRoomRenterRepository ?? throw new ArgumentNullException(nameof(auditRoomRenterRepository));
            //
            _auditRentTransactionPaymentRepository = auditRentTransactionPaymentRepository ?? throw new ArgumentNullException(nameof(auditRentTransactionPaymentRepository));
            _auditRentTransactionPaymentPropertyMappingService = auditRentTransactionPaymentPropertyMappingService ?? throw new ArgumentNullException(nameof(auditRentTransactionPaymentPropertyMappingService));
            //
            _auditRentTransactionRepository = auditRentTransactionRepository ?? throw new ArgumentNullException(nameof(auditRentTransactionRepository));
            _auditRentTransactionPropertyMappingService = auditRentTransactionPropertyMappingService ?? throw new ArgumentNullException(nameof(auditRentTransactionPropertyMappingService));
        }

        [HttpGet("{id}/renters", Name = "GetAuditRenters")]
        public IActionResult Renters(int id, [FromQuery] string fields)
        {
            if (!_auditRenterPropertyMappingService.ValidMappingExistsFor<AuditRenterDto, AuditRenter>("AuditDateTimeString"))
            {
                return BadRequest();
            }

            if (!_typeHelperService.TypeHasProperties<AuditRenterDto>(fields))
            {
                return BadRequest();
            }

            var auditRenters = 
                    id > 0 ? _auditRenterRepository.FindBy(o => o.Id == id, o => o.Room) :
                        _auditRenterRepository.FindAll(o => o.Room);
            if (!auditRenters.Any())
            {
                return NotFound();
            }

            var auditRenterRepo = Mapper.Map<IEnumerable<AuditRenterDto>>(auditRenters.OrderByDescending(o => o.AuditDateTime));

            return Ok(new BaseRestApiModel
            {
                Payload = auditRenterRepo.ShapeData(fields)
            });
        }

        [HttpGet("{id}/transactions", Name = "GetAuditRentTransactions")]
        public IActionResult Transactions(int id, [FromQuery] string fields)
        {
            if (!_auditRentTransactionPropertyMappingService.ValidMappingExistsFor<AuditRentTransactionDto, AuditRentTransaction>("auditDateTimeString"))
            {
                return BadRequest();
            }

            if (!_typeHelperService.TypeHasProperties<AuditRentTransactionDto>(fields))
            {
                return BadRequest();
            }

            var auditRenters = (id > 0 ? 
                    _auditRentTransactionRepository
                        .FindBy(o => o.Id == id, o => o.Room, o => o.Renter) : 
                            _auditRentTransactionRepository.FindAll(o => o.Room, o => o.Renter))
                                .OrderByDescending(o => o.AuditDateTime);
            if (!auditRenters.Any())
            {
                return NotFound();
            }

            var auditRentTransactionRepo = Mapper.Map<IEnumerable<AuditRentTransactionDto>>(auditRenters);

            return Ok(new BaseRestApiModel
            {
                Payload = auditRentTransactionRepo.ShapeData(fields)
            });
        }

        [HttpGet("{id}/payments", Name = "GetAuditPayments")]
        public IActionResult Payments(int id, [FromQuery] string fields)
        {
            //id is the transactionId

            if (!_auditRentTransactionPaymentPropertyMappingService.ValidMappingExistsFor<AuditRentTransactionPaymentDto, AuditRentTransactionPayment>("AuditDateTimeString"))
            {
                return BadRequest();
            }

            if (!_typeHelperService.TypeHasProperties<AuditRentTransactionPaymentDto>(fields))
            {
                return BadRequest();
            }

            var auditPayments = _auditRentTransactionPaymentRepository
                .FindBy(o => o.RentTransactionPayment.RentTransactionId == id)
                .OrderByDescending(o => o.AuditDateTime);

            if (!auditPayments.Any())
            {
                return NotFound();
            }

            var auditPaymentRepo = Mapper.Map<IEnumerable<AuditRentTransactionPaymentDto>>(auditPayments);

            return Ok(new BaseRestApiModel
            {
                Payload = auditPaymentRepo.ShapeData(fields)
            });

        }

        [HttpGet("{id}/Rooms", Name = "GetAuditRooms")]
        public IActionResult Rooms(int id, [FromQuery] string fields)
        {
            if (!_auditRoomPropertyMappingService.ValidMappingExistsFor<AuditRoomDto, AuditRoom>("AuditDateTimeString"))
            {
                return BadRequest();
            }

            if (!_typeHelperService.TypeHasProperties<AuditRoomDto>(fields))
            {
                return BadRequest();
            }

            var auditRenters = _auditRoomRenterRepository
                .FindAll()
                .OrderByDescending(o => o.AuditDateTime);

            var auditRenterRepo = Mapper.Map<IEnumerable<AuditRoomDto>>(auditRenters);

            return Ok(new BaseRestApiModel
            {
                Payload = auditRenterRepo.ShapeData(fields)
            });
        }
    }
}