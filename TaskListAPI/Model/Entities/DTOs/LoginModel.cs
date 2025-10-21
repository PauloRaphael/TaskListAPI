namespace TaskListAPI.Model.DTOs
{
    public class LoginModel
    {
        // Use validation attributes (e.g., [Required], [EmailAddress]) in a real app
        public string Email { get; set; }
        public string Password { get; set; }
    }
}