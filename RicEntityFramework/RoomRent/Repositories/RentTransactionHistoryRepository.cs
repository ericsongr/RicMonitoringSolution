using System;
using System.Collections.Generic;
using System.Linq;
using RicEntityFramework.BaseRepository;
using RicEntityFramework.RoomRent.Interfaces;
using RicModel.RoomRent;
using RicModel.RoomRent.Dtos;

namespace RicEntityFramework.RoomRent.Repositories
{
    public class RentTransactionHistoryRepository : EntityBaseRepository<RentTransaction>, IRentTransactionHistoryRepository
    {
        private readonly RicDbContext _context;

        public RentTransactionHistoryRepository(RicDbContext context) : base(context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

    }
}
