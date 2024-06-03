namespace WebApplication5
{
    public class Employee
    {
        public string Name { get; set; }
        public double Salary { get; set; }
        public int Id { get; set; }


        public Employee(string name, double salary, int id)
        {
            Name = name;
            Salary = salary;
            Id = id;
        }



    }
}
