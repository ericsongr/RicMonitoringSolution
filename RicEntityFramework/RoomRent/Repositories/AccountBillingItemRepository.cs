using System;
using RicCommon.Enumeration;
using RicEntityFramework.BaseRepository;
using RicEntityFramework.RoomRent.Interfaces;
using RicModel.RoomRent;
using RicModel.RoomRent.Enumerations;

namespace RicEntityFramework.RoomRent.Repositories
{
    public class AccountBillingItemRepository : EntityBaseRepository<AccountBillingItem>, IAccountBillingItemRepository
    {
        private readonly ISettingRepository _settingRepository;

        public AccountBillingItemRepository(
            RicDbContext context,
            ISettingRepository settingRepository) : base(context)
        {
            _settingRepository = settingRepository ?? throw new ArgumentNullException(nameof(settingRepository));
        }

        public void AddSmsFee(int accountId, int number, int totalSmsBill)
        {
            var billingAmount = Math.Round(decimal.Parse(_settingRepository.Get(SettingNameEnum.SMSFee).Value) * totalSmsBill, 2);
            var billRef = "";

            if (totalSmsBill > 1) billRef = $" - x{totalSmsBill}";

            Add(new AccountBillingItem
            {
                AccountId = accountId,
                BillingAmount = billingAmount,
                BillingReason = (int)BillingItemReasonEnum.SMSFee,
                BillingReference = $"#{number} SMS fee {billRef}",
                CreatedUtcDateTime = DateTime.UtcNow,
                PaymentType = PaymentTypes.DirectDebit
            });
            Commit();
        }
    }
}
