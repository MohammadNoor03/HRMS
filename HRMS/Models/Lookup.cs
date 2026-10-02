using System.ComponentModel.DataAnnotations;

namespace HRMS.Models
{
    public class Lookup
    {

        public long Id { get; set; }
        public int MajorCode { get; set; }
        public int MinorCode { get; set; }

        [MaxLength(50)]
        public string Name { get; set; }
    }

    /*
* 
* 1 => Developer // Position? Department Type?
* 2 => Manager // Position? Department Type?
* 3 => Finance // Position? Department Type?
* 4 => Technical // Position? Department Type?
*/
}
