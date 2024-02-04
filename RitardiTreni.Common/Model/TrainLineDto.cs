namespace RitardiTreni.Common.Model
{
    public class TrainLineDto
    {
        public int Id { get; set; }
        public  required string Number { get; set; }
        public int LineId { get; set; } 
    }
}