using System.ComponentModel.DataAnnotations;

namespace TransparencyCo.Models
{
    public class Product
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        public string Description { get; set; }

        public string ImagePath { get; set; }

        public string CertificateImagePath { get; set; }

        public string CertificateName { get; set; }
    }

}
