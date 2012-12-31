
namespace DonorBank.Web
{
    using System;
    using System.Collections.Generic;
    using System.Net;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;
    using System.Data;
    using System.Linq;
    using System.ServiceModel.DomainServices.EntityFramework;
    using System.ServiceModel.DomainServices.Hosting;
    using System.ServiceModel.DomainServices.Server;


    // Реализует логику приложения с использованием контекста DonorBankDatabaseEntities.
    // TODO: добавьте свою прикладную логику в эти или другие методы.
    // TODO: включите проверку подлинности (Windows/ASP.NET Forms) и раскомментируйте следующие строки, чтобы запретить анонимный доступ
    // Кроме того, рассмотрите возможность добавления ролей для соответствующего ограничения доступа.
    // [RequiresAuthentication]
    [EnableClientAccess()]
    public class DonorBankDomainService : LinqToEntitiesDomainService<DonorBankDatabaseEntities>
    {

        // TODO:
        // рассмотрите возможность сокращения результатов метода запроса.  Если необходим дополнительный ввод,
        // то в этот метод можно добавить параметры или создать дополнительные методы выполнения запроса с другими именами.
        // Для поддержки разбиения на страницы добавьте упорядочение в запрос "Blood".
        [RequiresRole("Worker")]
        public IQueryable<Blood> GetBlood()
        {
            return this.ObjectContext.Blood;
        }

        [RequiresRole("Worker")]
        public void InsertBlood(Blood blood)
        {
            if ((blood.EntityState != EntityState.Detached))
            {
                this.ObjectContext.ObjectStateManager.ChangeObjectState(blood, EntityState.Added);
            }
            else
            {
                this.ObjectContext.Blood.AddObject(blood);
            }
        }

        [RequiresRole("Worker")]
        public void UpdateBlood(Blood currentBlood)
        {
            this.ObjectContext.Blood.AttachAsModified(currentBlood, this.ChangeSet.GetOriginal(currentBlood));
        }

        [RequiresRole("Worker")]
        public void DeleteBlood(Blood blood)
        {
            if ((blood.EntityState != EntityState.Detached))
            {
                this.ObjectContext.ObjectStateManager.ChangeObjectState(blood, EntityState.Deleted);
            }
            else
            {
                this.ObjectContext.Blood.Attach(blood);
                this.ObjectContext.Blood.DeleteObject(blood);
            }
        }

        // TODO:
        // рассмотрите возможность сокращения результатов метода запроса.  Если необходим дополнительный ввод,
        // то в этот метод можно добавить параметры или создать дополнительные методы выполнения запроса с другими именами.
        // Для поддержки разбиения на страницы добавьте упорядочение в запрос "Clinic".
        [RequiresRole("Worker")]
        public IQueryable<Clinic> GetClinic()
        {
            return this.ObjectContext.Clinic;
        }

        [Query(IsComposable = false)]
        public Clinic GetClinicByUserName(string UserName)
        {
            return this.ObjectContext.Clinic.SingleOrDefault(c => (c.Username == UserName));
        }

        [RequiresRole("Worker")]
        public void InsertClinic(Clinic clinic)
        {
            if ((clinic.EntityState != EntityState.Detached))
            {
                this.ObjectContext.ObjectStateManager.ChangeObjectState(clinic, EntityState.Added);
            }
            else
            {
                this.ObjectContext.Clinic.AddObject(clinic);
            }
        }

        [RequiresRole("Worker")]
        public void UpdateClinic(Clinic currentClinic)
        {
            this.ObjectContext.Clinic.AttachAsModified(currentClinic, this.ChangeSet.GetOriginal(currentClinic));
        }

        [RequiresRole("Worker")]
        public void DeleteClinic(Clinic clinic)
        {
            if ((clinic.EntityState != EntityState.Detached))
            {
                this.ObjectContext.ObjectStateManager.ChangeObjectState(clinic, EntityState.Deleted);
            }
            else
            {
                this.ObjectContext.Clinic.Attach(clinic);
                this.ObjectContext.Clinic.DeleteObject(clinic);
            }
        }

        // TODO:
        // рассмотрите возможность сокращения результатов метода запроса.  Если необходим дополнительный ввод,
        // то в этот метод можно добавить параметры или создать дополнительные методы выполнения запроса с другими именами.
        // Для поддержки разбиения на страницы добавьте упорядочение в запрос "Donor".
        [RequiresRole("Worker")]
        public IQueryable<Donor> GetDonor()
        {
            return this.ObjectContext.Donor;
        }

        [RequiresRole("Worker")]
        [Query(IsComposable = false)]
        public Donor GetDonorByPassport(string Series, int Number)
        {
            return this.ObjectContext.Donor.SingleOrDefault(c => (c.Number == Number) && (c.Series == Series));
        }

        [RequiresRole("Worker")]
        public void InsertDonor(Donor donor)
        {
            if ((donor.EntityState != EntityState.Detached))
            {
                this.ObjectContext.ObjectStateManager.ChangeObjectState(donor, EntityState.Added);
            }
            else
            {
                this.ObjectContext.Donor.AddObject(donor);
            }
        }

        [RequiresRole("Worker")]
        public void UpdateDonor(Donor currentDonor)
        {
            this.ObjectContext.Donor.AttachAsModified(currentDonor, this.ChangeSet.GetOriginal(currentDonor));
        }

        [RequiresRole("Worker")]
        public void DeleteDonor(Donor donor)
        {
            if ((donor.EntityState != EntityState.Detached))
            {
                this.ObjectContext.ObjectStateManager.ChangeObjectState(donor, EntityState.Deleted);
            }
            else
            {
                this.ObjectContext.Donor.Attach(donor);
                this.ObjectContext.Donor.DeleteObject(donor);
            }
        }

        // TODO:
        // рассмотрите возможность сокращения результатов метода запроса.  Если необходим дополнительный ввод,
        // то в этот метод можно добавить параметры или создать дополнительные методы выполнения запроса с другими именами.
        // Для поддержки разбиения на страницы добавьте упорядочение в запрос "RespondBlood".
        [RequiresRole("Worker","Clinic")]
        public IQueryable<RespondBlood> GetRespondBlood()
        {
            return this.ObjectContext.RespondBlood.OrderBy(e => e.Status);
        }

        [RequiresRole("Worker", "Clinic")]
        public IQueryable<RespondBlood> GetOwnRespondBlood(int ClinicId)
        {
            return this.ObjectContext.RespondBlood.Where(e => e.IdClinic == ClinicId).OrderBy(e => e.Status);
        }

        [RequiresRole("Worker", "Clinic")]
        public void InsertRespondBlood(RespondBlood respondBlood)
        {
            if ((respondBlood.EntityState != EntityState.Detached))
            {
                this.ObjectContext.ObjectStateManager.ChangeObjectState(respondBlood, EntityState.Added);
            }
            else
            {
                this.ObjectContext.RespondBlood.AddObject(respondBlood);
            }
        }

        [RequiresRole("Worker", "Clinic")]
        public void UpdateRespondBlood(RespondBlood currentRespondBlood)
        {
            this.ObjectContext.RespondBlood.AttachAsModified(currentRespondBlood, this.ChangeSet.GetOriginal(currentRespondBlood));
        }

        [RequiresRole("Worker", "Clinic")]
        public void DeleteRespondBlood(RespondBlood respondBlood)
        {
            if ((respondBlood.EntityState != EntityState.Detached))
            {
                this.ObjectContext.ObjectStateManager.ChangeObjectState(respondBlood, EntityState.Deleted);
            }
            else
            {
                this.ObjectContext.RespondBlood.Attach(respondBlood);
                this.ObjectContext.RespondBlood.DeleteObject(respondBlood);
            }
        }

        // TODO:
        // рассмотрите возможность сокращения результатов метода запроса.  Если необходим дополнительный ввод,
        // то в этот метод можно добавить параметры или создать дополнительные методы выполнения запроса с другими именами.
        // Для поддержки разбиения на страницы добавьте упорядочение в запрос "RespondTransplantant".
        [RequiresRole("Worker","Clinic")]
        public IQueryable<RespondTransplantant> GetRespondTransplantant()
        {
            return this.ObjectContext.RespondTransplantant.OrderBy(e => e.Status);
        }

        [RequiresRole("Clinic")]
        public IQueryable<RespondTransplantant> RespondOwnTransplantant(int ClinicId)
        {
            return this.ObjectContext.RespondTransplantant.Where(e => e.IdClinic == ClinicId).OrderBy(e => e.Status);
        }

        [RequiresRole("Worker")]
        public IQueryable<RespondTransplantant> RespondGivenTransplantant(DateTime from, DateTime to)
        {
            return this.ObjectContext.RespondTransplantant.Where(e => (e.Status == "Удовлетворен") && (e.CreateTime >= from) && (e.CreateTime <= to) ).OrderBy(e => e.CreateTime);
        }

        [RequiresRole("Clinic")]
        public void InsertRespondTransplantant(RespondTransplantant RespondTransplantant)
        {
            if ((RespondTransplantant.EntityState != EntityState.Detached))
            {
                this.ObjectContext.ObjectStateManager.ChangeObjectState(RespondTransplantant, EntityState.Added);
            }
            else
            {
                this.ObjectContext.RespondTransplantant.AddObject(RespondTransplantant);
            }
        }
        [RequiresRole("Worker", "Clinic")]
        public void UpdateRespondTransplantant(RespondTransplantant currentRespondTransplantant)
        {
            this.ObjectContext.RespondTransplantant.AttachAsModified(currentRespondTransplantant, this.ChangeSet.GetOriginal(currentRespondTransplantant));
        }

        [RequiresRole("Worker")]
        public void DeleteRespondTransplantant(RespondTransplantant RespondTransplantant)
        {
            if ((RespondTransplantant.EntityState != EntityState.Detached))
            {
                this.ObjectContext.ObjectStateManager.ChangeObjectState(RespondTransplantant, EntityState.Deleted);
            }
            else
            {
                this.ObjectContext.RespondTransplantant.Attach(RespondTransplantant);
                this.ObjectContext.RespondTransplantant.DeleteObject(RespondTransplantant);
            }
        }

        // TODO:
        // рассмотрите возможность сокращения результатов метода запроса.  Если необходим дополнительный ввод,
        // то в этот метод можно добавить параметры или создать дополнительные методы выполнения запроса с другими именами.
        // Для поддержки разбиения на страницы добавьте упорядочение в запрос "TransactionInfo".
        [RequiresRole("Worker","Clinic")]
        public IQueryable<TransactionInfo> GetTransactionInfo()
        {
            return this.ObjectContext.TransactionInfo;
        }



        [RequiresRole("Worker","Clinic")]
        public void InsertTransactionInfo(TransactionInfo transactionInfo)
        {
            if ((transactionInfo.EntityState != EntityState.Detached))
            {
                this.ObjectContext.ObjectStateManager.ChangeObjectState(transactionInfo, EntityState.Added);
            }
            else
            {
                this.ObjectContext.TransactionInfo.AddObject(transactionInfo);
            }
        }

        [RequiresRole("Worker")]
        public void UpdateTransactionInfo(TransactionInfo currentTransactionInfo)
        {
            this.ObjectContext.TransactionInfo.AttachAsModified(currentTransactionInfo, this.ChangeSet.GetOriginal(currentTransactionInfo));
        }

        [RequiresRole("Worker")]
        public void DeleteTransactionInfo(TransactionInfo transactionInfo)
        {
            if ((transactionInfo.EntityState != EntityState.Detached))
            {
                this.ObjectContext.ObjectStateManager.ChangeObjectState(transactionInfo, EntityState.Deleted);
            }
            else
            {
                this.ObjectContext.TransactionInfo.Attach(transactionInfo);
                this.ObjectContext.TransactionInfo.DeleteObject(transactionInfo);
            }
        }

        // TODO:
        // рассмотрите возможность сокращения результатов метода запроса.  Если необходим дополнительный ввод,
        // то в этот метод можно добавить параметры или создать дополнительные методы выполнения запроса с другими именами.
        // Для поддержки разбиения на страницы добавьте упорядочение в запрос "Transplantant".
        [RequiresRole("Worker")]
        public IQueryable<Transplantant> GetTransplantant()
        {
            return this.ObjectContext.Transplantant;
        }

        [RequiresRole("Worker")]
        public IQueryable<Transplantant> GetAvalibleTransplantants()
        {
            return this.ObjectContext.Transplantant.Where(e => e.StorageTime >= DateTime.Now);
        }

        [RequiresRole("Worker")]
        public void InsertTransplantant(Transplantant transplantant)
        {
            if ((transplantant.EntityState != EntityState.Detached))
            {
                this.ObjectContext.ObjectStateManager.ChangeObjectState(transplantant, EntityState.Added);
            }
            else
            {
                this.ObjectContext.Transplantant.AddObject(transplantant);
            }
        }

        [RequiresRole("Worker")]
        public void UpdateTransplantant(Transplantant currentTransplantant)
        {
            this.ObjectContext.Transplantant.AttachAsModified(currentTransplantant, this.ChangeSet.GetOriginal(currentTransplantant));
        }

        [RequiresRole("Worker")]
        public void DeleteTransplantant(Transplantant transplantant)
        {
            if ((transplantant.EntityState != EntityState.Detached))
            {
                this.ObjectContext.ObjectStateManager.ChangeObjectState(transplantant, EntityState.Deleted);
            }
            else
            {
                this.ObjectContext.Transplantant.Attach(transplantant);
                this.ObjectContext.Transplantant.DeleteObject(transplantant);
            }
        }
    }
}


