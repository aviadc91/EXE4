using server.BL;

namespace server.BL
{
    public class Flat
    {
        int id;
        string city;
        string address;
        double price;
        int numOfRooms;
        static List<Flat> flatsList = new List<Flat>();

        public Flat() { }

        public Flat(int id, string city, string address, double price, int numOfRooms)
        {
            Id = id;
            City = city;
            Address = address;
            Price = price;
            NumOfRooms = numOfRooms;
           

        }


        public int Id { get => id; set => id = value; }
        public string City { get => city; set => city = value; }
        public string Address { get => address; set => address = value; }
        public double Price { get => price; set { price = value; } }
        public int NumOfRooms { get => numOfRooms; set => numOfRooms = value; }
        public static List<Flat> FlatsList { get => flatsList; set => flatsList = value; }

        public void discount(Flat f)
        {
            if (this.numOfRooms > 1 && this.price > 100)
            {
                this.price *= 0.9;
            }
        }

        public int insert()
        {
            discount(this);

            //foreach (Flat ff in flatsList)
            //{
            //    if (ff.id == this.id) return false; 
            //}
			DBservices dBservices = new DBservices();
            return dBservices.InsertFlat(this);
        }        
        
        public  List<Flat> read()
        {
			DBservices dBservices = new DBservices();
			return flatsList;
        }

        public List<Flat> FlatsByMaxPrice(double MaxPrice) 
        {
			DBservices dBservices = new DBservices();
			List<Flat> tempList = new List<Flat>();
            foreach (Flat fl in flatsList)
            {
                if (fl.price<=MaxPrice)
                {
                    tempList.Add(fl);
                }
            }
            return tempList;
        }


    }
}
