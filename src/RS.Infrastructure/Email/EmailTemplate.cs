namespace RS.Infrastructure.Email;

public static class EmailTemplate
{
    public static string WelcomeEmail(string fullName, string loginUrl)
    {
        return $"""
<!DOCTYPE html>
<html lang="en">
<head>
    <meta charset="UTF-8">
    <meta name="viewport" content="width=device-width, initial-scale=1.0">
    <title>Welcome</title>
</head>
<body style="margin:0;padding:0;background-color:#f4f4f4;font-family:Arial,Helvetica,sans-serif;">

    <table width="100%" cellpadding="0" cellspacing="0" style="padding:40px 0;">
        <tr>
            <td align="center">

                <table width="600" cellpadding="0" cellspacing="0"
                       style="background:#ffffff;border-radius:8px;overflow:hidden;">

                    <tr>
                        <td style="background:#2563eb;padding:30px;text-align:center;">
                            <h1 style="color:white;margin:0;">
                                RealEstate System
                            </h1>
                        </td>
                    </tr>

                    <tr>
                        <td style="padding:40px;">

                            <h2 style="margin-top:0;color:#333;">
                                Welcome, {fullName} 👋
                            </h2>

                            <p style="font-size:16px;color:#555;line-height:1.6;">
                                Thank you for registering with the RealEstate System.
                                Your account has been created successfully.
                            </p>

                            <p style="font-size:16px;color:#555;line-height:1.6;">
                                You can now sign in and start browsing properties,
                                managing listings, and much more.
                            </p>

                            <div style="text-align:center;margin:40px 0;">
                                <a href="{loginUrl}"
                                   style="
                                       background:#2563eb;
                                       color:white;
                                       text-decoration:none;
                                       padding:14px 30px;
                                       border-radius:6px;
                                       display:inline-block;
                                       font-weight:bold;">
                                    Login Now
                                </a>
                            </div>

                            <hr style="border:none;border-top:1px solid #eee;">

                            <p style="font-size:14px;color:#888;">
                                If you did not create this account, please ignore this email.
                            </p>

                            <p style="font-size:14px;color:#888;">
                                Regards,<br>
                                <strong>RealEstate Team</strong>
                            </p>

                        </td>
                    </tr>

                </table>

            </td>
        </tr>
    </table>

</body>
</html>
""";
    }
}
