namespace Demo.API.Dto
{
    public class CreateEmployeeDto
    {
        public string fullname { get; init; } = string.Empty;
        public string address { get; init; } = string.Empty;
        public int age { get; init; }
        public DateTime birthday { get; init; }
    }
}