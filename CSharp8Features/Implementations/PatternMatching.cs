using CSharp8Features.Interfaces;
using System;
using System.Threading.Tasks;

namespace CSharp8Features.Implementations
{
    public class PatternMatching : IExecutable
    {
        public Task Execute()
        {
            var employees = new[]
            {
                new Developer("Hamza", DateTime.Now.AddYears(-5), ProgrammingLangage.CSharp),
                new Developer("Hanselman", DateTime.Now.AddYears(-20), ProgrammingLangage.Python),
                new Employee("Y_Y", DateTime.Now)
            };

            CheckEmployees(employees);

            return Task.CompletedTask;
        }

        void CheckEmployees(Employee[] employees)
        {
            foreach (var employee in employees)
            {
                // using C# 7, we do it like bellow
                if (employee is Developer developer && developer.FavoriteLangage == ProgrammingLangage.CSharp)
                {
                    Console.WriteLine($"C# 7 - The employee : {developer.Name} is a {developer.GetType().Name}");
                }

                // In c# 8, we do it like follow
                if (employee is Developer { FavoriteLangage: ProgrammingLangage.CSharp, Name: string name })
                {
                    Console.WriteLine($"C# 8 - The employee : {name} is a Developer");
                }

                // switch statement in C# 8
                Console.WriteLine(employee switch
                {
                    Developer { FavoriteLangage: ProgrammingLangage.Python, Name: string employeeName } => $"{employeeName} is a developer",
                    _ => "Not interested by this one"
                });
            }
        }
    }

    class Employee
    {
        public Employee(string name, DateTime date) => (Name, StartDate) = (name, date);

        public string Name { get; set; }

        public DateTime StartDate { get; set; }
    }

    class Developer : Employee
    {
        public ProgrammingLangage FavoriteLangage { get; set; }

        public Developer(string name, DateTime date, ProgrammingLangage langage) : base(name, date)
        {
            FavoriteLangage = langage;
        }
    }

    enum ProgrammingLangage
    {
        CSharp,
        Python,
        Java
    };
}
