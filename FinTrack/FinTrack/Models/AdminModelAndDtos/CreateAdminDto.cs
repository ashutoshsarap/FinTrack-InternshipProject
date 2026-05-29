namespace FinTrack.Models.AdminModelAndDtos
{
    public class CreateAdminDto
    {
        public string FullName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public bool EmailConfirmed { get; set; } 
    }
}
