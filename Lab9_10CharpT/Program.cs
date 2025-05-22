using System;

class Horse
{
    public string Name { get; set; }
    public int AgeDays { get; set; } = 0;
    public int Energy { get; set; } = 100;
    public int Health { get; set; } = 100;
    public int Training { get; set; } = 0;
    public int Stress { get; set; } = 0;
    public bool IsSick { get; set; } = false;

    public int AgeYears => AgeDays / 365;

    public bool IsDead => Health <= 0 || AgeYears > 25;

    public void PrintStatus()
    {
        Console.WriteLine($"Вік: {AgeYears} років ({AgeDays} днів), Енергія: {Energy}, Здоров’я: {Health}, Тренованість: {Training}, Стрес: {Stress}, Стан: {(IsSick ? "Хворий" : "Здоровий")}");
    }
}

class Simulation
{
    private Horse horse;
    private Random rnd = new Random();
    private int daysSinceLastTreatment = 0;

    public Simulation(Horse horse)
    {
        this.horse = horse;
    }

    public void Run()
    {
        Console.WriteLine($"Кінь {horse.Name} народився.");
        while (!horse.IsDead)
        {
            horse.AgeDays++;
            daysSinceLastTreatment++;

            Console.WriteLine($"\n--- День {horse.AgeDays} ---");

            // Годування (щодня)
            horse.Energy = Math.Min(100, horse.Energy + 10);
            Console.WriteLine($"{horse.Name} поїв (+10 енергії)");

            // Тренування (щодня або за розкладом)
            if (horse.Energy >= 10)
            {
                horse.Training += 5;
                horse.Energy -= 10;
                Console.WriteLine($"{horse.Name} тренується (+5 тренованість, -10 енергії)");
            }

            // Змагання (енергія ≥ 50)
            if (horse.Energy >= 50 && rnd.NextDouble() < 0.3)
            {
                horse.Energy -= 20;
                horse.Stress += 10;
                Console.WriteLine($"{horse.Name} брав участь у змаганнях (-20 енергії, +10 стресу)");
            }

            // Хвороба (5% щодня)
            if (!horse.IsSick && rnd.NextDouble() < 0.05)
            {
                horse.IsSick = true;
                horse.Health -= 30;
                Console.WriteLine($"{horse.Name} захворів (-30 здоров’я)");
            }

            // Лікування (раз на 3 дні, якщо хворий)
            if (horse.IsSick && daysSinceLastTreatment >= 3)
            {
                horse.IsSick = false;
                horse.Health = Math.Min(100, horse.Health + 20);
                horse.Energy = Math.Max(0, horse.Energy - 10);
                daysSinceLastTreatment = 0;
                Console.WriteLine($"{horse.Name} отримав лікування (+20 здоров’я, -10 енергії)");
            }

            // Відпочинок (енергія < 30)
            if (horse.Energy < 30)
            {
                horse.Energy = Math.Min(100, horse.Energy + 20);
                horse.Stress = Math.Max(0, horse.Stress - 5);
                Console.WriteLine($"{horse.Name} відпочиває (+20 енергії, -5 стресу)");
            }

            // Старіння (щороку)
            if (horse.AgeDays % 365 == 0)
            {
                horse.Health -= 10;
                horse.Energy = Math.Max(0, horse.Energy - 5);
                Console.WriteLine($"{horse.Name} постарів (-10 здоров’я, -5 енергії)");
            }

            horse.PrintStatus();

            // Перевірка на смерть
            if (horse.IsDead)
            {
                Console.WriteLine($"\n{horse.Name} помер. Симуляція завершена.");
                break;
            }

            System.Threading.Thread.Sleep(200); // Для реалістичності симуляції
        }
    }
}

class Program
{
    static void Main()
    {
        Console.OutputEncoding = System.Text.Encoding.UTF8;
        Console.Write("Введіть ім’я коня: ");
        string name = Console.ReadLine();

        Horse horse = new Horse { Name = name };
        Simulation sim = new Simulation(horse);

        sim.Run();

        Console.WriteLine("Натисніть будь-яку клавішу для виходу...");
        Console.ReadKey();
    }
}
