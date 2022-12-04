namespace QAF.DataObjects
{
    public class Student
    {
        public string Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int BirthdayMonth { get; set; }
        public int BirthdayDay { get; set; }
        public int BirthdayYear { get; set; }
        public string DateOfBirth { get => BirthdayMonth + "-" + BirthdayDay + "-" + BirthdayYear; set {; } }
        public string Gender { get; set; }
        public string GradeLevel { get; set; }
        public bool IsEnrolled { get; set; }
        public bool IsGraduated { get; set; }
    }
}
