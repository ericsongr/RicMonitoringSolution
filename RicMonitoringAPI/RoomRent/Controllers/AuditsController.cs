using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RicEntityFramework.Helpers;
using RicEntityFramework.Interfaces;
using RicEntityFramework.RoomRent.Interfaces;
using RicEntityFramework.RoomRent.Interfaces.IPropertyMappings.IAudits;
using RicModel.RoomRent.Audits;
using RicModel.RoomRent.Dtos;
using RicModel.RoomRent.Dtos.Audits;

namespace RicMonitoringAPI.RoomRent.Controllers
{
    [AllowAnonymous]
    //[Authorize("Superuser")]
    [Route("api/audits")]
    public class AuditsController : Controller
    {
        private readonly ITypeHelperService _typeHelperService;
        private readonly IAuditRenterRepository _auditRenterRepository;
        private readonly IAuditRenterPropertyMappingService _auditRenterPropertyMappingService;

        public AuditsController(
            ITypeHelperService typeHelperService,
            IAuditRenterRepository auditRenterRepository,
                IAuditRenterPropertyMappingService auditRenterPropertyMappingService)
        {
            _typeHelperService = typeHelperService ?? throw new ArgumentNullException(nameof(typeHelperService));
            _auditRenterRepository = auditRenterRepository ?? throw new ArgumentNullException(nameof(auditRenterRepository));
            _auditRenterPropertyMappingService = auditRenterPropertyMappingService ?? throw new ArgumentNullException(nameof(auditRenterPropertyMappingService));
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

            var auditRenters = _auditRenterRepository
                .FindBy(o => o.Id == id, o => o.Room)
                .OrderByDescending(o => o.AuditDateTime);
            if (!auditRenters.Any())
            {
                return NotFound();
            }

            var auditRenterRepo = Mapper.Map<IEnumerable<AuditRenterDto>>(auditRenters);

            return Ok(auditRenterRepo.ShapeData(fields));

        }

        [HttpGet("{id}/transactions")]
        public IActionResult Transactions(int id)
        {
            var auditRenters = _auditRenterRepository.FindBy(o => o.Id == id);

            return Json(null);
        }

        [HttpGet("{id}/payments")]
        public IActionResult Payments(int id)
        {
            var auditRenters = _auditRenterRepository.FindBy(o => o.Id == id);

            return Json(null);
        }

        [HttpGet("Rooms")]
        public IActionResult Payments([FromQuery] string fields)
        {
            //if (!_auditRenterPropertyMappingService.ValidMappingExistsFor<AuditRenterDto, AuditRenter>("AuditDateTimeString"))
            //{
            //    return BadRequest();
            //}

            //if (!_typeHelperService.TypeHasProperties<AuditRenterDto>(fields))
            //{
            //    return BadRequest();
            //}

            //var auditRenters = _auditRenterRepository
            //    .FindBy(o => o.Id == id, o => o.Room)
            //    .OrderByDescending(o => o.AuditDateTime);
            //if (!auditRenters.Any())
            //{
            //    return NotFound();
            //}

            //var auditRenterRepo = Mapper.Map<IEnumerable<AuditRenterDto>>(auditRenters);

            //return Ok(auditRenterRepo.ShapeData(fields));
            return null;
        }
    }
}