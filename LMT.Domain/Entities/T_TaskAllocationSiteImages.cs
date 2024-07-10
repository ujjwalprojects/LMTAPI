﻿using System.ComponentModel.DataAnnotations;

namespace LMT.Domain.Entities
{
    public class T_TaskAllocationSiteImages
    {
        [Key]
        public int Task_Site_Image_Id { get; set; }
        public string Task_Site_Image { get; set; } = string.Empty;
        public string Task_Id { get; set; }
    }
}
