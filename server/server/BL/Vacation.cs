using server.BL;

namespace server.BL
{
    public class Vacation
    {
        int id;
        int userId;
        int flatId;
        DateTime startDate;
        DateTime endDate;
        static List<Vacation> vacationsList = new List<Vacation>();

        public Vacation() { }
        public Vacation(int id, int userId, int flatId, DateTime startDate, DateTime endDate)
        {
            Id = id;
            UserId = userId;
            FlatId = flatId;
            StartDate = startDate;
            EndDate = endDate;
        }

        public int Id { get => id; set => id = value; }
        public int UserId { get => userId; set => userId = value; }
        public int FlatId { get => flatId; set => flatId = value; }
        public DateTime StartDate { get => startDate; set => startDate = value; }
        public DateTime EndDate { get => endDate; set => endDate = value; }


    public bool insert()
        {
            foreach (Vacation v in vacationsList)
            {
                if (v.Id == this.Id) return false; //ask for unique vacation id
                if (v.FlatId == this.FlatId)
                {
                    if (this.StartDate >= v.StartDate && this.StartDate <= v.EndDate) return false;//start in range
                    if (this.EndDate >= v.StartDate && this.EndDate <= v.EndDate) return false;//finish in range
                    if (this.StartDate <= v.StartDate && this.EndDate >= v.EndDate) return false;//start before range and finish after it
                }
                
                   
            }
			DBservices dBservices = new DBservices();
			vacationsList.Add(this);
            return true;
        }


    public List<Vacation> read()
        {
			DBservices dBservices = new DBservices();
			return vacationsList;
        }
    public List<Vacation> readBySEDate(DateTime SD, DateTime ED)
        {
            List<Vacation> selectedList = new List<Vacation>(); 
            foreach(var v in vacationsList)
            {
                if(v.StartDate>= SD && v.EndDate<= ED) selectedList.Add(v);
            }

            return selectedList;
        }



    }


}
