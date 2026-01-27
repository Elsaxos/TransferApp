using TransferApp.Services;

namespace TransferApp.UnitTests.TestHelpers;

public class FakeEmailSender : IEmailSender
{
    public int SentCount { get; private set; }
    public string? LastTo { get; private set; }
    public string? LastSubject { get; private set; }
    public string? LastBody { get; private set; }

    public Task SendEmailAsync(string to, string subject, string htmlMessage)
    {
        SentCount++;
        LastTo = to;
        LastSubject = subject;
        LastBody = htmlMessage;
        return Task.CompletedTask;
    }
}

