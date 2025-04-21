using System.ComponentModel.DataAnnotations;

namespace schedule_api.Models


{
    public abstract class Person
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string? Name { get; set; }
        [Phone]
        public string? Phone { get; set; }
        //[RegularExpression()]
        [EmailAddress]
        public string? Email { get; set; }
    }
}
