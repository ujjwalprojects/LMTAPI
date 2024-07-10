using System.ComponentModel.DataAnnotations;

namespace LMT.Domain.Entities
{
    public class M_WorkerTypes
    {
        [Key]
        public int WorkerType_Id { get; set; }
        public string WorkerType_Name { get; set; } = string.Empty;

    }
}
