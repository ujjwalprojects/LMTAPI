using System.ComponentModel.DataAnnotations;

namespace LMT.Domain.Entities
{
    public class T_EstablishmentRegistrations
    {
        [Key]
        public int Estd_Id { get; set; }
        public string Estd_Name { get; set; } = string.Empty;
        public string Estd_Owner_Name { get; set; } = string.Empty;
        public string Estd_Contact_No { get; set; } = string.Empty;
        public string Estd_Reg_No { get; set; } = string.Empty;
        public string Estd_TradeLicense_No { get; set; } = string.Empty;
        public int Estd_Reg_Act_Id { get; set; }
        public DateTime Estd_Reg_Valid_From { get; set; }
        public DateTime Estd_Reg_Valid_Upto { get; set; }
        public DateTime Estd_Rec_Last_Updated { get; set; }
    }
}
