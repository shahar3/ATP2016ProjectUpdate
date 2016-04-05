using System;

namespace ATP2016Assignment0.Vehicles
{

    abstract class AVehicle : IVehicle
    {
        private static int counterNumber = 100000;
        //fields 
        private int m_vehicleNumber;
        private string m_manufacturer;
        private string m_Model;
        private string m_yearOfManufacture;
        private string m_weight;
        private string m_Color;
        private int m_numberOfSeats;
        private double m_fuelTankSize;
        private double m_curFuelQuantity;

        //ctor
        public AVehicle()
        {
            m_vehicleNumber = counterNumber;
            m_manufacturer = "CarsService";
            m_Model = "1";
            m_yearOfManufacture = "2016";
            m_Model = "white";
        }



        public abstract void Move(int km);

        public void PrintDetails()
        {
            Console.WriteLine("------------------------------------");
            Console.WriteLine("Print car details");
            Console.WriteLine("VehicleNumber: " + m_vehicleNumber);
            Console.WriteLine("Manufacturer: " + m_manufacturer);
            Console.WriteLine("Model: " + m_Model);
            Console.WriteLine("Year of manufacture: " + m_yearOfManufacture);
            Console.WriteLine("Weight: " + m_weight);
            Console.WriteLine("Color: " + m_Color);
            Console.WriteLine("Number of seats: " + m_numberOfSeats);
            Console.WriteLine("Fuel tank size: " + m_fuelTankSize);
            Console.WriteLine("Current fuel quantity: " + m_curFuelQuantity);
            Console.WriteLine("-------------------------------------");
        }

        public void Refueling(double amount)
        {
            throw new NotImplementedException();
        }

        //getters and setters
        public int VehicleNumber
        {
            get { return m_vehicleNumber; }
            set { m_vehicleNumber = value; }
        }


        public string Manufacturer
        {
            get { return m_manufacturer; }
            set { m_manufacturer = value; }
        }


        public string Model
        {
            get { return m_Model; }
            set { m_Model = value; }
        }


        public string YearOfManufacture
        {
            get { return m_yearOfManufacture; }
            set { m_yearOfManufacture = value; }
        }


        public string Weight
        {
            get { return m_weight; }
            set { m_weight = value; }
        }


        public string Color
        {
            get { return m_Color; }
            set { m_Color = value; }
        }


        public int NumberOfSeats
        {
            get { return m_numberOfSeats; }
            set { m_numberOfSeats = value; }
        }


        public double FuelTankSize
        {
            get { return m_fuelTankSize; }
            set { m_fuelTankSize = value; }
        }


        public double CurFuelQuantity
        {
            get { return m_curFuelQuantity; }
            set { m_curFuelQuantity = value; }
        }





    }
}
