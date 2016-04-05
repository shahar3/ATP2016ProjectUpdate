namespace ATP2016Assignment0.Vehicles
{
    class Motorcycle : AVehicle
    {
        public Motorcycle() : base()
        {
            Weight = "0.5 Ton";
            NumberOfSeats = 2;
            FuelTankSize = 20;
            CurFuelQuantity = FuelTankSize;
        }

        public Motorcycle(string manufacturer, string yearOfManu, string weight, string color, int numOfSeats, double fuelTank, double curFuelTank) : base()
        {
            Manufacturer = manufacturer;
            YearOfManufacture = yearOfManu;
            Weight = weight;
            Color = color;
            NumberOfSeats = numOfSeats;
            FuelTankSize = fuelTank;
            CurFuelQuantity = curFuelTank;
        }

        public override void Move(int km)
        {
            CurFuelQuantity = CurFuelQuantity - (km / 20);
        }
    }
}
