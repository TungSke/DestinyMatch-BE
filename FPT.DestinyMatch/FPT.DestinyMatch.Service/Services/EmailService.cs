using FluentEmail.Core;
using FPT.DestinyMatch.Service.Interfaces;
using Razor.Templating.Core;
using FPT.DestinyMatch.Repository.Models;
using FPT.DestinyMatch.Service.Models.BusinessObjects;

namespace FPT.DestinyMatch.Service.Services
{
    public class EmailService : IEmailService
    {
        private readonly IFluentEmailFactory _fluentEmailFactory;

        private readonly IRazorTemplateEngine _razorTemplateEngine;

        public EmailService(IFluentEmailFactory fluentEmailFactory, IRazorTemplateEngine razorTemplateEngine)
        {
            _fluentEmailFactory = fluentEmailFactory;
            _razorTemplateEngine = razorTemplateEngine;
        }

        public async Task ResetPassword(Account accountInfo, string confirmOtp)
        {
            var tempAcc = new Account
            {
                Email = accountInfo.Email,
                Password = confirmOtp,
                Status = accountInfo.Status,
            };

            var emailBody = await _razorTemplateEngine.RenderAsync("wwwroot/Views/ResetPassword.cshtml", tempAcc);

            await SendEmail(new Email_Info
            {
                ReceiverEmail = tempAcc.Email!,
                SubjectContent = "Mã Bảo Mật Mật Khẩu",
                BodyContent = emailBody
            });
        }

        public async Task MatchRequestEmail(string SenderName, Member receiverMember)
        {
            var tempMatching = new Matching
            {
                FirstName = SenderName,
                SecondName = receiverMember.Fullname,
                CreatedAt = DateTime.Now,

            };

            var emailBody = await _razorTemplateEngine.RenderAsync("wwwroot/Views/MatchingRequest.cshtml", tempMatching);

            await SendEmail(new Email_Info
            {
                ReceiverEmail = receiverMember.Account.Email!,
                SubjectContent = "Yêu Cầu Ghép Đôi Mới",
                BodyContent = emailBody
            });
        }

        private async Task SendEmail(Email_Info sendEmailDto, bool hasHtmlBody = true)
        {
            var email = _fluentEmailFactory.Create();

            email.To(sendEmailDto.ReceiverEmail);
            email.Subject(sendEmailDto.SubjectContent);
            email.Body(sendEmailDto.BodyContent, hasHtmlBody);
            await email.SendAsync();
        }
    }
}
