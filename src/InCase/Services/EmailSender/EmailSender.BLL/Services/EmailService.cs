﻿using System.Text;
using EmailSender.BLL.Common;
using EmailSender.BLL.Interfaces;
using Infrastructure.MassTransit.Email;
using MailKit.Net.Smtp;
using Microsoft.Extensions.Configuration;
using MimeKit;

namespace EmailSender.BLL.Services;
public class EmailService(IConfiguration configuration) : IEmailService
{
    private static readonly string Env = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Development";

    private readonly string _host = configuration[$"EmailConfig:Host:{Env}"]!;
    private readonly int _port = int.Parse(configuration[$"EmailConfig:Port:{Env}"]!);
    private readonly string _smtpEmail = configuration[$"EmailConfig:Email:{Env}"]!;
    private readonly string _smtpPassword = configuration[$"EmailConfig:Password:{Env}"]!;
    private readonly string _addressCallback = configuration[$"EmailConfig:AddressCallback:{Env}"]!;

    public async Task SendToEmailAsync(EmailTemplate template, CancellationToken cancellationToken = default)
    {
        var msg = new MimeMessage();

        msg.From.Add(new MailboxAddress("Администрация сайта", _smtpEmail));
        msg.To.Add(new MailboxAddress(template.Email, template.Email));
        msg.Subject = template.Subject;
        msg.Body = new TextPart("html") { Text = CreateBodyLetter(template) };

        using var client = new SmtpClient();
        await client.ConnectAsync(_host, _port, true, cancellationToken);
        client.AuthenticationMechanisms.Remove("XOAUTH2");
        await client.AuthenticateAsync(_smtpEmail, _smtpPassword, cancellationToken);

        try
        {
            await client.SendAsync(msg, cancellationToken);
        }
        catch (SmtpCommandException)
        {
            // ignored
        }
        catch (Exception) {
            // ignored
        }
    }

    private string CreateBodyLetter(EmailTemplate template)
    {
        if (!string.IsNullOrEmpty(template.Body.ButtonLink))
            template.Body.ButtonLink = _addressCallback + template.Body.ButtonLink;

        if (!string.IsNullOrEmpty(template.Header.Title)) 
            return ConvertToBodyTemplate(template);

        var headerWords = template.Subject.Split(" ").ToList();

        template.Header.Title = headerWords[0];
        headerWords.Remove(headerWords[0]);

        if (headerWords.Count >= 2)
        {
            template.Header.Title += " " + headerWords[0];
            headerWords.Remove(headerWords[0]);
        }

        foreach (var word in headerWords) template.Header.Subtitle += word + " ";

        return ConvertToBodyTemplate(template);
    }

    private static string ConvertToBodyTemplate(EmailTemplate template)
    {
        template.Body.Description += "<br>С уважением команда InCase.";

        if (string.IsNullOrEmpty(template.Body.ButtonText)) template.Body.ButtonText = "Подтверждаю";

        var buttonTemplate = CreateButton(template.Body.ButtonLink, template.Body.ButtonText);
        var bannerTemplate = CreateBanner(template.BannerTemplates);

        return RenderBodyMessage(template, buttonTemplate, bannerTemplate).ToString();
    }

    private static StringBuilder CreateButton(string? link, string text)
    {
        if (string.IsNullOrEmpty(link)) return new StringBuilder();

        var button = new StringBuilder(EmailBodyConstants.ButtonPair1);

        button.Append(link);
        button.Append(EmailBodyConstants.ButtonPair2);
        button.Append(text);
        button.Append(EmailBodyConstants.ButtonPair3);

        return button;
    }

    private static StringBuilder CreateBanner(List<EmailBannerTemplate> bannerTemplates)
    {
        if (bannerTemplates.Count == 0) return new StringBuilder();

        var banners = new StringBuilder(EmailBodyConstants.BannerPair1);

        foreach (var b in bannerTemplates)
        {
            banners.Append(EmailBodyConstants.BannerPair2);
            banners.Append(b.Href);
            banners.Append(EmailBodyConstants.BannerPair3);
            banners.Append(b.ImageUri);
            banners.Append(EmailBodyConstants.BannerPair4);
        }

        banners.Append(EmailBodyConstants.BannerPair5);
        
        return banners;
    }

    private static StringBuilder RenderBodyMessage(EmailTemplate template, StringBuilder buttonTemplate, 
        StringBuilder bannerTemplate)
    {
        var body = new StringBuilder(EmailBodyConstants.BodyPair1);

        body.Append(template.Header.Title);
        body.Append(EmailBodyConstants.BodyPair2);
        body.Append(template.Header.Subtitle);
        body.Append(EmailBodyConstants.BodyPair3);
        body.Append(template.Body.Title);
        body.Append(EmailBodyConstants.BodyPair4);
        body.Append(template.Body.Description);
        body.Append(EmailBodyConstants.BodyPair5);
        body.Append(buttonTemplate);
        body.Append(bannerTemplate);
        body.Append(EmailBodyConstants.BodyPair6);

        return body;
    }
}