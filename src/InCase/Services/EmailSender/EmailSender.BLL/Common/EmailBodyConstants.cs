namespace EmailSender.BLL.Common;
public class EmailBodyConstants
{
    private static readonly string Env = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Development";

    private static readonly string YouTubeLinkImage = Env == "Production" ? 
        "https://fileserver.in-case.games:8080/mailbox/youtube.jpg" :
        "https://sun9-33.userapi.com/impg/pDHT-ZNEx2eALiBOCavVLivuxZoJcCBbyRukwA/pQiJz3xs1HQ.jpg?" +
        "size=50x29&quality=96&sign=b32c387df9355b766f711a031c36b443&type=album";
    private static readonly string TelegramLinkImage = Env == "Production" ?
        "https://fileserver.in-case.games:8080/mailbox/telegram.jpg" :
        "https://sun9-21.userapi.com/impg/A-6P4uCJQZfR66QrTrieXWApphA63QRebdH9hw/7DWAmoZcQY8.jpg?" +
        "size=37x37&quality=96&sign=464dd75123df66fee21ec449826783bb&type=album";
    private static readonly string VkLinkImage = Env == "Production" ?
        "https://fileserver.in-case.games:8080/mailbox/vk.jpg" :
        "https://sun9-18.userapi.com/impg/QHdJOSjvwnI5RF242kIkz8ANmj7e0sCOxL1MxA/gV-DPmqRvB0.jpg?" +
        "size=38x38&quality=96&sign=5d32284e755ed8a955a90c98751d168f&type=album";
    private static readonly string InCaseLogoLinkImage = Env == "Production" ?
        "https://fileserver.in-case.games:8080/mailbox/in-case-logo.jpg" :
        "https://sun9-4.userapi.com/impg/rzB56cbWicyBWzmj5u0f_BZ4-IIJFguSnFrPcw/mIziSoNmxvk.jpg?" +
        "size=175x220&quality=96&sign=46ea7dfbe656fa8ddeadd1896e6b93f4&type=album";

    public const string ButtonPair1 = "<div style=\"padding-bottom:30px;text-align:center\"><a href=\"";
    public const string ButtonPair2 = "\" style=\"text-decoration: none; margin: 30px 0; cursor: pointer; " +
                                      "background-color: transparent; font-family: 'Trebuchet MS',sans-serif; " +
                                      "font-weight: bold; padding: 8px 85px; font-size: 16px; color: #FD7E21; " +
                                      "border: 2px solid #FD7E21; border-radius: 8px;\" target=\"_blank\" " +
                                      "data-saferedirecturl=\"ya.ru\">";
    public const string ButtonPair3 = "</a></div>";
    public const string BannerPair1 = "<div style=\"text-align:left;\"><hr style=\"border: 1px solid #FD7E21; " +
                                      "margin: 10px 50px;\">";
    public const string BannerPair2 = "<table align=\"center\" style=\"padding:10px;\"><tbody><tr><td><a href=\"";
    public const string BannerPair3 = "\"><img align=\"center\" src=\"";
    public const string BannerPair4 = "\" width=\"350\" height=\"110\" alt=\"icon\" aria-hidden=\"true\" " +
                                      "alt=\"InCase\" data-bit=\"iit\"/></a></td></tr></tbody></table>";
    public const string BannerPair5 = "</div>";

    public const string BodyPair1 = "<!DOCTYPE html PUBLIC '-//W3C//DTD HTML 4.01 Transitional//EN'><html>" +
                                    "<head><meta http-equiv='Content-type' content='text/html; charset=utf-8'/></head>" +
                                    "<body><div style=\"margin:0;color: #FD7E21;padding:0\" bgcolor=\"#FFFFFF\">" +
                                    "<table width=\"100%\" height=\"100%\" style=\"min-width:480px\" border=\"0\" " +
                                    "cellspacing=\"0\" cellpadding=\"0\" lang=\"en\"><tbody><tr height=\"32\" " +
                                    "style=\"height:32px\"><td></td></tr><tr align=\"center\"><td><div><div></div></div>" +
                                    "<table border=\"0\" cellspacing=\"0\" cellpadding=\"0\" style=\"padding-bottom:10px;" +
                                    "max-width:520px;min-width:480px;color: #FD7E21;\" bgcolor=\"#1A1A1D\"><tbody><tr>" +
                                    "<td width=\"8\" style=\"width:8px\"></td><td><div style=\"padding:40px 10px 0px 10px\" " +
                                    "align=\"center\"><!--Header--><div style=\"font-family:'Trebuchet MS',Gadget," +
                                    "'Lucida Sans Unicode',sans-serif;line-height:32px;padding-bottom:60px;text-align:center;" +
                                    "word-break:break-word\"><table align=\"center\"><tbody><tr style=\"line-height:normal;\">" +
                                    "<td align=\"left\"><p style=\"font-family:'Trebuchet MS',Gadget,'Lucida Sans Unicode'," +
                                    "sans-serif;font-size:20px;line-height:20px;margin: 0;color: #FD7E21;\">";
    public const string BodyPair2 = "</p><p style=\"font-family:'Trebuchet MS',Gadget,'Lucida Sans Unicode',sans-serif;" +
                                    "font-size:20px;line-height:20px;margin: 0 0 0 50px;color: #FD7E21;\">";
    public const string BodyPair3 = "</p></td><td width=\"160px\"></td><td align=\"right\"><a href=\"https://in-case.games\" " +
                                    "style=\"text-decoration: none;font-family:'Trebuchet MS',Gadget,'Lucida Sans Unicode'," +
                                    "sans-serif;font-size:40px;line-height:20px;margin: 0;color: #FD7E21;cursor: pointer;\">" +
                                    "InCase</a></td></tr></tbody></table></div><!--Body--><div style=\"font-family:'Trebuchet MS'," +
                                    "Gadget,'Lucida Sans Unicode',sans-serif;line-height:32px;padding-bottom:18px;" +
                                    "text-align:center;word-break:break-word;font-size:20px;color:#fd7e21;\">";
    public const string BodyPair4 = "</div><div style=\"font-family:'Trebuchet MS',Gadget,'Lucida Sans Unicode',sans-serif;" +
                                    "font-size:15px;line-height:20px;text-align:center;color: #FD7E21;padding-bottom:30px;\">";
    public const string BodyPair5 = "</div>";
    public static string BodyPair6 = "<!--FOOTER--><div style=\"text-align:left;\"><hr style=\"border: 1px solid #FD7E21; " +
                                    "margin: 10px 50px;\"><table align=\"center\" style=\"padding-bottom:20px;\"><tbody>" +
                                    $"<tr><td><img align=\"center\" src=\"{InCaseLogoLinkImage}\" width=\"100\" alt=\"icon\" " +
                                    "aria-hidden=\"true\" alt=\"InCase\" data-bit=\"iit\"/></td></tr></tbody></table>" +
                                    "<div style=\"font-family:'Trebuchet MS',Gadget,'Lucida Sans Unicode',sans-serif;" +
                                    "font-size:14px;color: #FD7E21;line-height:18px;text-align:center\"><table " +
                                    "align=\"center\" style=\"padding-bottom:20px;\"><tbody><tr><td><a " +
                                    "href=\"https://in-case.games\"><img style=\"cursor: pointer;margin-right: 10px;\" " +
                                    $"src=\"{YouTubeLinkImage}\" width=\"45\" alt=\"icon\" aria-hidden=\"true\" alt=\"InCase\" " +
                                    "data-bit=\"iit\"/></a></td><td><a href=\"https://t.me/+IdWrGtInH9AwNWZi\">" +
                                    $"<img style=\"cursor: pointer;margin-right: 10px;\" src=\"{TelegramLinkImage}\" " +
                                    "href=\"yandex.ru\" width=\"32\" alt=\"icon\" aria-hidden=\"true\" alt=\"InCase\" " +
                                    "data-bit=\"iit\"/></a></td><td><a href=\"https://in-case.games\">" +
                                    $"<img style=\"cursor: pointer;margin-right: 10px;\" src=\"{VkLinkImage}\" " +
                                    "aria-hidden=\"true\" alt=\"InCase\" data-bit=\"iit\"/></a></td></tr></tbody></table></div>" +
                                    "</div></div></td><td width=\"8\" style=\"width:8px\"></td></tr></tbody></table></td></tr>" +
                                    "<tr height=\"32\" style=\"height:32px\"><td></td></tr></tbody></table></div></body></html>";
}