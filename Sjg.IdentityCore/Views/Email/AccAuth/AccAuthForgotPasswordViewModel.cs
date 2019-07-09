namespace Sjg.IdentityCore.Views.Email.AccAuth
{
    public class AccAuthForgotPasswordViewModel
    {
        public string CallBackUrl { get; set; }
        public string Code { get; set; }
        public string Email { get; set; }
        public string ApplicationName { get; set; }
    }
}