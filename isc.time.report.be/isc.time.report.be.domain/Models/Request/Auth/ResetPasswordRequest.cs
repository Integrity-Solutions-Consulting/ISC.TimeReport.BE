namespace isc.time.report.be.domain.Models.Request.Auth
{
    public class ResetPasswordRequest
    {
        public string NewPassword { get; set; }
        public string ConfirmPassword { get; set; }
    }

}
