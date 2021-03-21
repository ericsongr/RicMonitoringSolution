using System;
using System.Linq;
using RicEntityFramework.BaseRepository;
using RicEntityFramework.Helpers;
using RicEntityFramework.Parameters;
using RicEntityFramework.RoomRent.Interfaces;
using RicEntityFramework.RoomRent.Interfaces.IPropertyMappings;
using RicModel.RoomRent;
using RicModel.RoomRent.Dtos;

namespace RicEntityFramework.RoomRent.Repositories
{
    public class AccountRepository : EntityBaseRepository<Account>, IAccountRepository
    {
        private readonly RicDbContext _context;
        private readonly IAccountPropertyMappingService _propertyMappingService;

        public AccountRepository(
            RicDbContext context,
            IAccountPropertyMappingService propertyMappingService) : base(context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _propertyMappingService = propertyMappingService ?? throw new ArgumentNullException(nameof(propertyMappingService));
        }

        public PagedList<Account> GetAccounts(AccountResourceParameters accountResourceParameters)
        {
            var collectionBeforPaging =
                _context.Accounts.ApplySort(
                    accountResourceParameters.OrderBy,
                    _propertyMappingService.GetPropertyMapping<AccountDto, Account>());


            if (!string.IsNullOrEmpty(accountResourceParameters.SearchQuery))
            {
                var searchQueryForWhereClause =
                    accountResourceParameters.SearchQuery.Trim().ToLowerInvariant();

                collectionBeforPaging = collectionBeforPaging
                    .Where(a => a.Name.ToLowerInvariant().Contains(searchQueryForWhereClause));
            }

            return PagedList<Account>.Create(collectionBeforPaging,
                accountResourceParameters.PageNumber,
                accountResourceParameters.PageSize);
        }
    }
}
