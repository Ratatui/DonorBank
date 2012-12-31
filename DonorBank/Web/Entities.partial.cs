using System.Runtime.Serialization;
using System.ServiceModel.DomainServices.Client;
using System.Text;

namespace DonorBank.Web {

    /// <summary>
    /// The 'Blood' entity class.
    /// </summary>
    public sealed partial class Blood : Entity {

        public override string ToString() {
            StringBuilder sb = new StringBuilder();
            sb.Append(Id)
                .Append(". Донор: ").Append(Donor != null ? Donor.FirstName : "")
                .Append(".  ").Append(Purpose);

            return sb.ToString();
        }
    }

    /// <summary>
    /// The 'Clinic' entity class.
    /// </summary>
    public sealed partial class Clinic : Entity {

        public override string ToString() {
            return Id +
                ". " + Title +
                ". Главврач: " + Director;
        }
    }

    /// <summary>
    /// The 'Donor' entity class.
    /// </summary>
    public sealed partial class Donor : Entity {

        public override string ToString() {
            return Id +
                ". " + FirstName +
                " " + MiddleName +
                " " + LastName;
        }
    }

    /// <summary>
    /// The 'Transplantant' entity class.
    /// </summary>
    public sealed partial class Transplantant : Entity {

        public override string ToString() {
            return Id +
                ". " + Type;
        }
    }

    /// <summary>
    /// The 'RespondBlood' entity class.
    /// </summary>
    public sealed partial class RespondBlood : Entity {

        public override string ToString() {
            return Id +
                ". Кровь группы " + Blood +
                " объем " + Purpose;
        }
    }

    /// <summary>
    /// The 'RespondTransplantant' entity class.
    /// </summary>
    public sealed partial class RespondTransplantant : Entity {

        public override string ToString() {
            StringBuilder sb = new StringBuilder();
            sb.Append(Id)
                .Append(". Больница ").Append(Clinic != null ? (Clinic.Title + "(" + Clinic.Id + ")") : "")
                .Append(", запрос на  ").Append(Type);

            return sb.ToString();
        
        }
          
         
    }
}
