namespace LMT.Application.DTOs
{
    public class T_EstablishmentRegistrationsDTO
    {
        public int Estd_Id { get; set; }
        public string Estd_Name { get; set; } = string.Empty;
        public string Estd_Owner_Name { get; set; } = string.Empty;
        public string Estd_Contact_No { get; set; } = string.Empty;
        public string Estd_Reg_No { get; set; } = string.Empty;
        public string Estd_TradeLicense_No { get; set; } = string.Empty;
        public int Estd_Reg_Act_Id { get; set; }
    }
}
