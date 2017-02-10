namespace dojodachi.Models
{
    public class Dojodachi
    {
        public Dojodachi()
        {
            fullness = 20;
            happiness = 20;
            energy = 50;
            meals = 3;
        }
        public int fullness {get; set;}
        public int happiness {get; set;}
        public int energy {get; set;}
        public int meals {get; set;}
    }
}