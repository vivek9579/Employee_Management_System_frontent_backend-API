namespace Domain.Entity
{
    public class RefreshToken
    {
        public int Id { get; set; }
        public string ReToken { get; set; }
        public DateTime ExpiryDate { get; set; }
        public bool IsRevers {  get; set; }
        public int UserId { get; set; }
        public User User { get; set; }
    }
}
