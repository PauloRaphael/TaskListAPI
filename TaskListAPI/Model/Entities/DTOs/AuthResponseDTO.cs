namespace TaskListAPI.Model.DTOs
{
    public class AuthResponseDTO
    {
        public string Token { get; set; }
        public int UserId { get; set; }
        public string UserEmail { get; set; }
        // You can add ExpirationDate if needed
    }
}