namespace ATP2016Assignment0.Vehicles
{
    class Airplane : AVehicle
    {
        public Airplane() : base()
        {
            Weight = "20 Ton";
            NumberOfSeats = 6;
            FuelTankSize = 1000;
            CurFuelQuantity = FuelTankSize;
        }

        public Airplane(string manufacturer, string yearOfManu, string weight, string color, int numOfSeats, double fuelTank, double curFuelTank) : base()
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
            CurFuelQuantity = CurFuelQuantity - (km / 10);
        }
    }
}
