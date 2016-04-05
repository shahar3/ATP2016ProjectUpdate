using ATP2016Assignment0.Vehicles;
using System;
using System.Collections;
namespace ATP2016Assignment0

{
    class Program
    {
        static void Main(string[] args)
        {
            Boolean end = false;
            ArrayList vehicles = new ArrayList();
            while (!end)
            {
                System.Console.WriteLine("please enter which vehicle you want ro add");
                System.Console.WriteLine("1-Car");
                System.Console.WriteLine("2-Motorcycle");
                System.Console.WriteLine("3-Airplane");
                System.Console.WriteLine("4-Truck");
                Console.WriteLine("5-EXIT");
                string act = Console.ReadLine();

                switch (act)
                {
                    case "1":
                        Console.WriteLine("enter manufacture");
                        string manuf = Console.ReadLine();
                        Console.WriteLine("enter year of manufacture");
                        string year = Console.ReadLine();
                        Console.WriteLine("enter weight");
                        string weight = Console.ReadLine();
                        Console.WriteLine("enter color");
                        string color = Console.ReadLine();
                        Console.WriteLine("enter number of seats");
                        int number = Int32.Parse(Console.ReadLine());
                        Console.WriteLine("enter fuel tank size");
                        double tankSize = Double.Parse(Console.ReadLine());
                        Console.WriteLine("enter Current fuel quantity");
                        double quantityTank = Double.Parse(Console.ReadLine());
                        AVehicle newCar = new Car(manuf, year, weight, color, number, tankSize, quantityTank);
                        vehicles.Add(newCar);
                        break;
                    case "2":
                        Console.WriteLine("enter manufacture");
                        string Mmanuf = Console.ReadLine();
                        Console.WriteLine("enter year of manufacture");
                        string Myear = Console.ReadLine();
                        Console.WriteLine("enter weight");
                        string Mweight = Console.ReadLine();
                        Console.WriteLine("enter color");
                        string Mcolor = Console.ReadLine();
                        Console.WriteLine("enter number of seats");
                        int Mnumber = Int32.Parse(Console.ReadLine());
                        Console.WriteLine("enter fuel tank size");
                        double MtankSize = Double.Parse(Console.ReadLine());
                        Console.WriteLine("enter Current fuel quantity");
                        double MquantityTank = Double.Parse(Console.ReadLine());
                        AVehicle newMotorCycle = new Motorcycle(Mmanuf, Myear, Mweight, Mcolor, Mnumber, MtankSize, MquantityTank);
                        vehicles.Add(newMotorCycle);
                        break;
                    case "3":
                        Console.WriteLine("enter manufacture");
                        string Amanuf = Console.ReadLine();
                        Console.WriteLine("enter year of manufacture");
                        string Ayear = Console.ReadLine();
                        Console.WriteLine("enter weight");
                        string Aweight = Console.ReadLine();
                        Console.WriteLine("enter color");
                        string Acolor = Console.ReadLine();
                        Console.WriteLine("enter number of seats");
                        int Anumber = Int32.Parse(Console.ReadLine());
                        Console.WriteLine("enter fuel tank size");
                        double AtankSize = Double.Parse(Console.ReadLine());
                        Console.WriteLine("enter Current fuel quantity");
                        double AquantityTank = Double.Parse(Console.ReadLine());
                        AVehicle newAirPlane = new Airplane(Amanuf, Ayear, Aweight, Acolor, Anumber, AtankSize, AquantityTank);
                        vehicles.Add(newAirPlane);
                        break;
                    case "4":
                        Console.WriteLine("enter manufacture");
                        string Tmanuf = Console.ReadLine();
                        Console.WriteLine("enter year of manufacture");
                        string Tyear = Console.ReadLine();
                        Console.WriteLine("enter weight");
                        string Tweight = Console.ReadLine();
                        Console.WriteLine("enter color");
                        string Tcolor = Console.ReadLine();
                        Console.WriteLine("enter number of seats");
                        int Tnumber = Int32.Parse(Console.ReadLine());
                        Console.WriteLine("enter fuel tank size");
                        double TtankSize = Double.Parse(Console.ReadLine());
                        Console.WriteLine("enter Current fuel quantity");
                        double TquantityTank = Double.Parse(Console.ReadLine());
                        AVehicle newTruck = new Truck(Tmanuf, Tyear, Tweight, Tcolor, Tnumber, TtankSize, TquantityTank);
                        vehicles.Add(newTruck);
                        break;
                    case "5":
                        end = true;
                        break;
                    default:
                        break;
                }
            }

            foreach (AVehicle x in vehicles)
            {
                x.Move(1);
            }

            foreach (AVehicle x in vehicles)
            {
                x.PrintDetails();
            }
            Console.ReadLine();
        }
    }
}
