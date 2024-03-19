using System.ComponentModel.DataAnnotations;

namespace DotnetAssignment1.Entities
{
    public class IotData
    {
        [Key]
        public string Key { get; set; }
        public string Value { get; set; }
    }
}
