using System;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Xml;

namespace MunicpalTax.Data
{
    public class TaxRate
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        [MaxLength(120)]
        public string Name { get; set; }

        [Required]
        [MaxLength(120)]
        public string MunicipalityName { get; set; }

        [Required]
        public DateTime StartDate { get; set; }

        [Required]
        public DateTime EndDate { get; set; }

        [Required]
        public int RateType { get; set; }

        [Required]
        public double Rate { get; set; }
    }

    public class TaxRateContext : DbContext
    {
        public DbSet<TaxRate> TaxRates { get; set; }
    }

}
