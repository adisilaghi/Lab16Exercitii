using Lab16Exercitii.Models;
using Microsoft.EntityFrameworkCore;

using (var context = new DatabaseContext())
{
    Seed(context);
    context.Database.Migrate();

    DisplayAllStudents(context);
    DisplayYoungestStudentInConstructii(context);
    DisplayStudentsSummaryInInformatica(context);
    DisplayStudentsGroupedBySpecialization(context);

    DisplayAllCars(context);
    int newCarId = AddCar(context, "CarName", "XYZ123", 2021, "Toyota");
    Console.WriteLine($"New car added with ID: {newCarId}");

    var toyotaCars = GetCarsByProducator(context, "Toyota");
    foreach (var car in toyotaCars)
    {
        Console.WriteLine($"{car.Nume} - {car.Producator}");
    }

    DeleteCarById(context, newCarId);
    Console.WriteLine("Car deleted successfully.");


static void Seed(DatabaseContext context)
{
    if (!context.Students.Any())
    {
        context.Students.AddRange(
            new Student { Nume = "Popescu", Prenume = "Ion", Varsta = 22, Specializare = "Informatica" },
            new Student { Nume = "Ionescu", Prenume = "Maria", Varsta = 21, Specializare = "Litere" },
            new Student { Nume = "Georgescu", Prenume = "Vasile", Varsta = 23, Specializare = "Constructii" }
        );
        context.SaveChanges();
    }

    if (!context.Cars.Any())
    {
        context.Cars.AddRange(
            new Car { Nume = "Dacia", SerieDeSasiu = "ABC123", AnDeFabricatie = 2020, Producator = "Dacia" },
            new Car { Nume = "BMW", SerieDeSasiu = "XYZ456", AnDeFabricatie = 2019, Producator = "BMW" }
        );
        context.SaveChanges();
    }
}
}



static void DisplayAllStudents(DatabaseContext context)
{
    var students = context.Students.OrderBy(s => s.Nume).ThenBy(s => s.Prenume).ToList();
    foreach (var student in students)
    {
        Console.WriteLine($"{student.Nume} {student.Prenume}");
    }
}

static void DisplayYoungestStudentInConstructii(DatabaseContext context)
{
    var student = context.Students
        .Where(s => s.Specializare == "Constructii" && s.Varsta > 20)
        .OrderBy(s => s.Varsta)
        .FirstOrDefault();

    if (student != null)
    {
        Console.WriteLine($"{student.Nume} {student.Prenume} is the youngest student in Constructii over 20 years old.");
    }
}

static void DisplayStudentsSummaryInInformatica(DatabaseContext context)
{
    var students = context.Students
        .Where(s => s.Specializare == "Informatica")
        .Select(s => new { s.Id, s.Nume, s.Prenume })
        .ToList();

    foreach (var student in students)
    {
        Console.WriteLine($"ID: {student.Id}, Nume: {student.Nume}, Prenume: {student.Prenume}");
    }
}

static void DisplayStudentsGroupedBySpecialization(DatabaseContext context)
{
    var groupedStudents = context.Students
        .GroupBy(s => s.Specializare)
        .ToList();

    foreach (var group in groupedStudents)
    {
        Console.WriteLine($"Specialization: {group.Key}");
        foreach (var student in group)
        {
            Console.WriteLine($"{student.Nume} {student.Prenume}");
        }
    }
}

static void DisplayAllCars(DatabaseContext context)
{
    var cars = context.Cars.OrderByDescending(c => c.AnDeFabricatie).ToList();
    foreach (var car in cars)
    {
        Console.WriteLine($"{car.Nume} - {car.AnDeFabricatie}");
    }
}

static int AddCar(DatabaseContext context, string nume, string serieDeSasiu, int anDeFabricatie, string producator)
{
    var car = new Car
    {
        Nume = nume,
        SerieDeSasiu = serieDeSasiu,
        AnDeFabricatie = anDeFabricatie,
        Producator = producator
    };

    context.Cars.Add(car);
    context.SaveChanges();

    return car.Id;
}

static void DeleteCarById(DatabaseContext context, int id)
{
    var car = context.Cars.Find(id);
    if (car != null)
    {
        context.Cars.Remove(car);
        context.SaveChanges();
    }
}

static IQueryable<Car> GetCarsByProducator(DatabaseContext context, string producator)
{
    return context.Cars.Where(c => c.Producator == producator);
}