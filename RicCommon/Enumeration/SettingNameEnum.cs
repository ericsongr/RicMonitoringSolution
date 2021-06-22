namespace RicCommon.Enumeration
{
    public enum SettingNameEnum
    {
        TenantGracePeriod,
        AppDomain,
        AmazonS3BucketName,
        EnableAmazonS3,
        AmazonS3AccessKey,
        AmazonS3SecretKey,

        AppEmailRenterBeforeDueDateEnable,
        AppEmailRenterNoOfDaysBeforeDueDate,
        AppEmailMessageRenterBeforeDueDate,

        AppSMSRenterBeforeDueDateEnable,
        AppSMSRenterNoOfDaysBeforeDueDate,
        AppSMSMessageRenterBeforeDueDate,

        UseSystemDedicatedNumber,
        SMSGatewaySenderId,

        SMSFee,
    }
}
