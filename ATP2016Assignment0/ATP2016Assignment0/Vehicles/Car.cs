namespace ATP2016Assignment0.Vehicles
{
    class Car : AVehicle
    {
        //ctor
        public Car() : base()
        {
            Weight = "1 Ton";
            NumberOfSeats = 5;
            FuelTankSize = 50;
            CurFuelQuantity = FuelTankSize;
        }

        public Car(string manufacturer, string yearOfManu, string weight, string color, int numOfSeats, double fuelTank, double curFuelTank) : base()
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
            CurFuelQuantity = CurFuelQuantity - (km / 15);
        }
    }
}
