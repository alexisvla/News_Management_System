using System;
using System.Collections.Generic;

namespace NewsAPI.Models;

public partial class PasswordReset
{
    public int PasswordResetId { get; set; }
    public int UserId { get; set; }
    public string token { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime ExpiredAt { get; set; }
}
