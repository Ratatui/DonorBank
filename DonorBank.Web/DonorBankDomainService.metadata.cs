
namespace DonorBank.Web
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;
    using System.Data.Objects.DataClasses;
    using System.Linq;
    using System.ServiceModel.DomainServices.Hosting;
    using System.ServiceModel.DomainServices.Server;


    // Атрибут MetadataTypeAttribute идентифицирует BloodMetadata как класс,
    // который содержит дополнительные метаданные для класса Blood.
    [MetadataTypeAttribute(typeof(Blood.BloodMetadata))]
    public partial class Blood
    {

        // Этот класс позволяет добавлять настраиваемые атрибуты к свойствам
        // класса Blood.
        //
        // Например, далее свойство Xyz помечено как
        // обязательное и указан формат допустимых значений:
        //    [Required]
        //    [RegularExpression("[A-Z][A-Za-z0-9]*")]
        //    [StringLength(32)]
        //    public string Xyz { get; set; }
        internal sealed class BloodMetadata
        {

            // Классы метаданных не предназначены для создания экземпляров.
            private BloodMetadata()
            {
            }
            [Display(AutoGenerateField = false)]
            public Donor Donor { get; set; }

            [Key]
            [Display(AutoGenerateField = false)]
            public int Id { get; set; }

            [Display(AutoGenerateField = false)]
            public int IdDonor { get; set; }

            [Display(Name = "Цель", Order = 3)]
            public string Purpose { get; set; }

            [Display(Name = "Срок годности до:", Order = 2)]
            public DateTime StorageTime { get; set; }
        }
    }

    // Атрибут MetadataTypeAttribute идентифицирует ClinicMetadata как класс,
    // который содержит дополнительные метаданные для класса Clinic.
    [MetadataTypeAttribute(typeof(Clinic.ClinicMetadata))]
    public partial class Clinic
    {

        // Этот класс позволяет добавлять настраиваемые атрибуты к свойствам
        // класса Clinic.
        //
        // Например, далее свойство Xyz помечено как
        // обязательное и указан формат допустимых значений:
        //    [Required]
        //    [RegularExpression("[A-Z][A-Za-z0-9]*")]
        //    [StringLength(32)]
        //    public string Xyz { get; set; }
        internal sealed class ClinicMetadata
        {

            // Классы метаданных не предназначены для создания экземпляров.
            private ClinicMetadata()
            {
            }

            [Editable(false, AllowInitialValue = true)]
            [Display(Name = "Адрес", Order = 3)]
            public string Addres { get; set; }

            [Display(Name = "Главврач", Order = 1)]
            public string Director { get; set; }

            [Key]
            [Display(AutoGenerateField = false)]
            public int Id { get; set; }

            [Display(Name = "Телефон", Order = 2)]
            public string Phone { get; set; }

            [Display(AutoGenerateField = false)]
            public EntityCollection<RespondBlood> RespondBlood { get; set; }

            [Display(AutoGenerateField = false)]
            public EntityCollection<RespondTransplantant> RespondTransplantant { get; set; }

            [Display(Name = "Название", Order = 0)]
            public string Title { get; set; }

            [Editable(false, AllowInitialValue = true)]
            [Display(Name = "Пользователь", Order = 4)]
            public string Username { get; set; }
        }
    }

    // Атрибут MetadataTypeAttribute идентифицирует DonorMetadata как класс,
    // который содержит дополнительные метаданные для класса Donor.
    [MetadataTypeAttribute(typeof(Donor.DonorMetadata))]
    public partial class Donor
    {

        // Этот класс позволяет добавлять настраиваемые атрибуты к свойствам
        // класса Donor.
        //
        // Например, далее свойство Xyz помечено как
        // обязательное и указан формат допустимых значений:
        //    [Required]
        //    [RegularExpression("[A-Z][A-Za-z0-9]*")]
        //    [StringLength(32)]
        //    public string Xyz { get; set; }
        internal sealed class DonorMetadata
        {

            // Классы метаданных не предназначены для создания экземпляров.
            private DonorMetadata()
            {
            }

            [Display(Name = "Адрес", Order = 7)]
            public string Addres { get; set; }

            [Display(Name = "Группа крови", Order = 5)]
            public string Blood { get; set; }

            [Display(AutoGenerateField = false)]
            public EntityCollection<Blood> Blood1 { get; set; }

            [Display(Name = "Кол-во сдачь крови", Order = 9)]
            public Nullable<int> BloodDonor { get; set; }

            [Display(Name = "Время смерти", Order = 6)]
            public Nullable<DateTime> DeathTime { get; set; }

            [Display(Name = "Имя", Order = 2)]
            public string FirstName { get; set; }

            [Key]
            [Display(AutoGenerateField = false)]
            public int Id { get; set; }

            [Display(Name = "Фамилия", Order = 3)]
            public string LastName { get; set; }

            [Display(Name = "Отчество", Order = 4)]
            public string MiddleName { get; set; }

            [Display(Name = "Номер паспорта", Order = 1)]
            public int Number { get; set; }

            [Display(Name = "Телефон", Order = 8)]
            public string Phone { get; set; }

            [Display(Name = "Серия", Order = 0)]
            public string Series { get; set; }

            [Display(AutoGenerateField = false)]
            public EntityCollection<Transplantant> Transplantant { get; set; }
        }
    }

    // Атрибут MetadataTypeAttribute идентифицирует RespondBloodMetadata как класс,
    // который содержит дополнительные метаданные для класса RespondBlood.
    [MetadataTypeAttribute(typeof(RespondBlood.RespondBloodMetadata))]
    public partial class RespondBlood
    {

        // Этот класс позволяет добавлять настраиваемые атрибуты к свойствам
        // класса RespondBlood.
        //
        // Например, далее свойство Xyz помечено как
        // обязательное и указан формат допустимых значений:
        //    [Required]
        //    [RegularExpression("[A-Z][A-Za-z0-9]*")]
        //    [StringLength(32)]
        //    public string Xyz { get; set; }
        internal sealed class RespondBloodMetadata
        {

            // Классы метаданных не предназначены для создания экземпляров.
            private RespondBloodMetadata()
            {
            }

            [Display(Name = "Группа крови", Order = 2)]
            public string Blood { get; set; }

            [Display(AutoGenerateField = false)]
            public Clinic Clinic { get; set; }

            [Display(Name = "Кол-во",Order=0)]
            public int Count { get; set; }

            [Key]
            [Display(AutoGenerateField = false)]
            public int Id { get; set; }

            [Display(AutoGenerateField = false)]
            public int IdClinic { get; set; }

            [Display(Name = "Назначение",Order=2)]
            public string Purpose { get; set; }

            [Display(Name = "Статус",Order=1)]
            public string Status { get; set; }

            [Display(Name = "Время создания",Order=3)]
            public DateTime TimeCreate { get; set; }

            [Display(Name = "Время ожидания",Order=4)]
            public DateTime WaitTime { get; set; }
        }
    }

    // Атрибут MetadataTypeAttribute идентифицирует RespondTransplantantMetadata как класс,
    // который содержит дополнительные метаданные для класса RespondTransplantant.
    [MetadataTypeAttribute(typeof(RespondTransplantant.RespondTransplantantMetadata))]
    public partial class RespondTransplantant
    {

        // Этот класс позволяет добавлять настраиваемые атрибуты к свойствам
        // класса RespondTransplantant.
        //
        // Например, далее свойство Xyz помечено как
        // обязательное и указан формат допустимых значений:
        //    [Required]
        //    [RegularExpression("[A-Z][A-Za-z0-9]*")]
        //    [StringLength(32)]
        //    public string Xyz { get; set; }
        internal sealed class RespondTransplantantMetadata
        {

            // Классы метаданных не предназначены для создания экземпляров.
            private RespondTransplantantMetadata()
            {
            }
            [Display(AutoGenerateField = false)]
            public Clinic Clinic { get; set; }

            [Display(Name = "Дата создания",Order=9)]
            public DateTime CreateTime { get; set; }

            [Display(Name = "Фактор 1",Order=3)]
            public string Factor1 { get; set; }

            [Display(Name = "Фактор 2",Order=4)]
            public string Factor2 { get; set; }

            [Display(Name = "Фактор 3",Order=5)]
            public string Factor3 { get; set; }

            [Display(Name = "Фактор 4",Order=6)]
            public string Factor4 { get; set; }

            [Display(Name = "Фактор 5",Order=7)]
            public string Factor5 { get; set; }

            [Display(Name = "Фактор 6",Order=8)]
            public string Factor6 { get; set; }

            [Key]
            [Display(AutoGenerateField = false)]
            public int Id { get; set; }

            [Display(AutoGenerateField = false)]
            public int IdClinic { get; set; }

            [Display(Name = "Приоритет",Order=2)]
            public int Priority { get; set; }

            [Display(Name = "Статус",Order=1)]
            public string Status { get; set; }

            [Display(Name = "Тип",Order=0)]
            public int Type { get; set; }

            [Display(Name = "Время ожидания",Order=10)]
            public DateTime WaitTime { get; set; }
        }
    }

    // Атрибут MetadataTypeAttribute идентифицирует TransactionInfoMetadata как класс,
    // который содержит дополнительные метаданные для класса TransactionInfo.
    [MetadataTypeAttribute(typeof(TransactionInfo.TransactionInfoMetadata))]
    public partial class TransactionInfo
    {

        // Этот класс позволяет добавлять настраиваемые атрибуты к свойствам
        // класса TransactionInfo.
        //
        // Например, далее свойство Xyz помечено как
        // обязательное и указан формат допустимых значений:
        //    [Required]
        //    [RegularExpression("[A-Z][A-Za-z0-9]*")]
        //    [StringLength(32)]
        //    public string Xyz { get; set; }
        internal sealed class TransactionInfoMetadata
        {

            // Классы метаданных не предназначены для создания экземпляров.
            private TransactionInfoMetadata()
            {
            }

            [Display(Name = "Примечание", Order = 3)]
            public string About { get; set; }

            [Key]
            [Display(AutoGenerateField = false)]
            public int Id { get; set; }

            [Display(Name = "Дата/Время", Order = 0)]
            public DateTime TimeDate { get; set; }

            [Display(Name = "Тип", Order = 1)]
            public string Type { get; set; }

            [Display(Name = "Пользователь", Order = 2)]
            public string User { get; set; }
        }
    }

    // Атрибут MetadataTypeAttribute идентифицирует TransplantantMetadata как класс,
    // который содержит дополнительные метаданные для класса Transplantant.
    [MetadataTypeAttribute(typeof(Transplantant.TransplantantMetadata))]
    public partial class Transplantant
    {

        // Этот класс позволяет добавлять настраиваемые атрибуты к свойствам
        // класса Transplantant.
        //
        // Например, далее свойство Xyz помечено как
        // обязательное и указан формат допустимых значений:
        //    [Required]
        //    [RegularExpression("[A-Z][A-Za-z0-9]*")]
        //    [StringLength(32)]
        //    public string Xyz { get; set; }
        internal sealed class TransplantantMetadata
        {

            // Классы метаданных не предназначены для создания экземпляров.
            private TransplantantMetadata()
            {
            }
            [Display(AutoGenerateField = false)]
            public Donor Donor { get; set; }

            [Display(Name = "Фактор 1",Order=2)]
            public string Factor1 { get; set; }

            [Display(Name = "Фактор 2",Order=3)]
            public string Factor2 { get; set; }

            [Display(Name = "Фактор 3",Order=4)]
            public string Factor3 { get; set; }

            [Display(Name = "Фактор 4",Order=5)]
            public string Factor4 { get; set; }

            [Display(Name = "Фактор 5",Order=6)]
            public string Factor5 { get; set; }

            [Display(Name = "Фактор 6",Order=7)]
            public string Factor6 { get; set; }

            [Key]
            [Display(AutoGenerateField = false)]
            public int Id { get; set; }

            [Display(AutoGenerateField = false)]
            public int IdDonor { get; set; }

            [Display(Name = "Хранить до",Order=1)]
            public DateTime StorageTime { get; set; }

            [Display(Name = "Тип",Order=0)]
            public string Type { get; set; }
        }
    }
}
