namespace ATP2016Assignment0.Vehicles
{
    interface IVehicle
    {
        void PrintDetails();
        void Move(int km);
        void Refueling(double amount);
    }
}
