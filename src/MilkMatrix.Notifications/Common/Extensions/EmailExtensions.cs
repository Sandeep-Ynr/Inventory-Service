using System.Net;
using System.Net.Mail;
using System.Text;
using MilkMatrix.Core.Abstractions.Logger;
using MilkMatrix.Core.Entities.Common;
using MilkMatrix.Notifications.Models;

namespace MilkMatrix.Notifications.Common.Extensions;

public static class EmailExtensions
{
    public static StatusCode SendMail(this EmailSettings model,
        bool isAllowedToSendEmail,
        ILogging logger,
        string pathToTemplate)
    {
        var response = new StatusCode();
        try
        {
            MailMessage Message = new();
            if (isAllowedToSendEmail)
            {
                StreamReader stream = File.OpenText(pathToTemplate + @model.BodyLink);
                Attachment attachment;
                string str = stream.ReadToEnd();
                stream.Close();
                if (!string.IsNullOrEmpty(model.Name))
                {
                    str = str.Replace("#NAME#", model.Name);
                }
                else
                    str = str.Replace("#NAME#", "");
                if (!string.IsNullOrEmpty(model.Content1))
                {
                    str = str.Replace("#CONTENT1#", model.Content1);
                }
                else
                    str = str.Replace("#CONTENT1#", "");

                if (!string.IsNullOrEmpty(model.Content2))
                {
                    str = str.Replace("#CONTENT2#", model.Content2);
                }
                else
                    str = str.Replace("#CONTENT2#", "");

                if (!string.IsNullOrEmpty(model.Content3))
                {
                    str = str.Replace("#CONTENT3#", model.Content3);
                }
                else
                    str = str.Replace("#CONTENT3#", "");

                if (!string.IsNullOrEmpty(model.Content4))
                {
                    str = str.Replace("#CONTENT4#", model.Content4);
                }
                else
                    str = str.Replace("#CONTENT4#", "");

                if (!string.IsNullOrEmpty(model.Content5))
                {
                    str = str.Replace("#CONTENT5#", model.Content5);
                }
                else
                    str = str.Replace("#CONTENT5#", "");

                if (!string.IsNullOrEmpty(model.Content6))
                {
                    str = str.Replace("#CONTENT6#", model.Content6);
                }
                else
                    str = str.Replace("#CONTENT6#", "");

                if (!string.IsNullOrEmpty(model.RedirectUrl))
                {
                    str = str.Replace("#REDIRECTURL#", model.RedirectUrl + Convert.ToBase64String(Encoding.UTF8.GetBytes(model.MailTo.ToString())));
                }
                else
                    str = str.Replace("#REDIRECTURL#", "");

                if (!string.IsNullOrEmpty(model.MailTo))
                {
                    if (model.MailTo.Contains(";"))
                    {
                        for (int countID = 0; countID < model.MailTo.Split(';').Length; countID++)
                        {
                            Message.To.Add(new MailAddress(model.MailTo.Split(';')[countID]));
                        }
                    }
                    else
                    {
                        Message.To.Add(new MailAddress(model.MailTo));
                    }
                }
                if (!string.IsNullOrEmpty(model.Cc))
                {
                    if (model.Cc.Contains(";"))
                    {
                        for (int countID = 0; countID < model.Cc.Split(';').Length; countID++)
                        {
                            Message.CC.Add(new MailAddress(model.Cc.Split(';')[countID]));
                        }
                    }
                    else
                    {
                        Message.CC.Add(new MailAddress(model.Cc));
                    }
                }
                if (!string.IsNullOrEmpty(model.Bcc))
                {
                    if (model.Bcc.Contains(";"))
                    {
                        for (int countID = 0; countID < model.Bcc.Split(';').Length; countID++)
                        {
                            Message.Bcc.Add(new MailAddress(model.Bcc.Split(';')[countID]));
                        }
                    }
                    else
                    {
                        Message.Bcc.Add(new MailAddress(model.Bcc));
                    }
                }
                if (!string.IsNullOrEmpty(model.Attachment))
                {
                    if (model.Attachment.Contains(";"))
                    {
                        for (int countID = 0; countID < model.Attachment.Split(';').Length; countID++)
                        {
                            attachment = new Attachment(model.Attachment.Split(';')[countID]);
                            Message.Attachments.Add(attachment);
                        }
                    }
                    else
                    {
                        attachment = new Attachment(model.Attachment);
                        Message.Attachments.Add(attachment);
                    }
                }
                Message.Subject = model.Subject;
                Message.From = new MailAddress(model.MailFrom, "");
                Message.Body = str;
                Message.IsBodyHtml = true;
                SmtpClient smtp = new SmtpClient(model.SmtpServer);
                smtp.Port = model.SmtpPort;
                smtp.UseDefaultCredentials = false;
                smtp.Credentials = new NetworkCredential(model.SmtpUserId, model.SmtpPassword); 
                smtp.EnableSsl = true;
                smtp.Send(Message);
            }
            response.Message = StatusCodeMessage.MailSuccessMessage;
            response.Code = (int)HttpStatusCode.OK;
            logger.LogInfo(StatusCodeMessage.MailSuccessMessage);
        }
        catch (Exception ex)
        {
            response.Message = StatusCodeMessage.MailFailedMessage;
            response.Code = (int)HttpStatusCode.InternalServerError;
            logger.LogError($"{StatusCodeMessage.MailFailedMessage} Exception {ex.Message} :: Stack trace {ex}");
        }
        return response;
    }
}
