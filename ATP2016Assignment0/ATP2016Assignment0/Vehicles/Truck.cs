namespace ATP2016Assignment0.Vehicles
{
    class Truck : AVehicle
    {

        public Truck() : base()
        {
            Weight = "3 Ton";
            NumberOfSeats = 3;
            FuelTankSize = 100;
            CurFuelQuantity = FuelTankSize;
        }

        public Truck(string manufacturer, string yearOfManu, string weight, string color, int numOfSeats, double fuelTank, double curFuelTank) : base()
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
            CurFuelQuantity = CurFuelQuantity - (km / 12);
        }
    }
}
