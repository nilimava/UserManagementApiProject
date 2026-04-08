namespace UserManagementApi.Models
{
    //Actual entity represents table in the database
    public class User
    {
        public int Id { get; set; }  
        public string Username { get; set; } 
        public string Email { get; set; }
        public string PasswordHash { get; set; }
    }
}
