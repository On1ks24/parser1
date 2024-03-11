using DataAccessLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using Teacher;

namespace Logic
{
    public class Logic1
    {
        public DapperRepository repository=new DapperRepository();
        public List<string> Data2 = new List<string>() { "Место работы", "Преподаваемые дисциплины"};
        public List<teacher> Teachers=new List<teacher>();
        public string View(teacher Teacher)
        {
            string s = Teacher.Name + $"\ne-mail: {Teacher.Email}\nтелефон: {Teacher.Phone}\nадрес: {Teacher.Adress}\n\nМесто работы:\n{Teacher.Work}";
            return ProcessString(s);
        }
        public string ProcessString(string input)
        {
            string withoutTags = Regex.Replace(input, "<.*?>", string.Empty);
            string withAt = withoutTags.Replace(" [at] ", "@");
            string withDot = withAt.Replace(" [dot] ", ".");
            return withDot;
        }
        public void teacher()
        {
            string Name="";
            string Email = "";
            string Phone = "";
            string Address = "";
            string Work = "";
            for (int i = 0; i < 7000; i++)
            {
                HttpClient _client = new HttpClient();
                HttpRequestMessage request = new HttpRequestMessage();
                request.Method = HttpMethod.Get;
                request.Headers.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64; rv:120.0) Gecko/20100101 Firefox/120.0");
                request.Headers.Add("Accept", "text/html,application/xhtml+xml,application/xml;q=0.9,image/avif,image/webp,*/*;q=0.8");
                request.Headers.Add("Accept-Language", "ru-RU,ru;q=0.8,en-US;q=0.5,en;q=0.3");
                request.Headers.Add("Connection", "keep-alive");
                request.Headers.Add("Cookie", "SESS2e5bcdb0a3fb733bf72970e20c782619=949mbg74ofce3qbhl4006kcqo6; sfu-special-fonts=0; sfu-special-scheme=0; sfu-special-images=0; sputnik_session=1701780853880^|11; _ym_uid=1701780854482332604; _ym_d=1701780854; _ym_isad=2; _ym_visorc=w; has_js=1");
                request.Headers.Add("Upgrade-Insecure-Requests", "1");
                request.Headers.Add("Sec-Fetch-Dest", "document");
                request.Headers.Add("Sec-Fetch-Mode", "navigate");
                request.Headers.Add("Sec-Fetch-Site", "none");
                request.Headers.Add("Sec-Fetch-User", "?1");
                request.Headers.Add("TE", "trailers");
                request.RequestUri = new Uri($"https://structure.sfu-kras.ru/node/{i}");
                HttpResponseMessage response = _client.SendAsync(request).Result;
                try
                {
                    response.EnsureSuccessStatusCode();
                }
                catch (HttpRequestException)
                {
                    continue;
                }

                string responseBody = response.Content.ReadAsStringAsync().Result;
                if (responseBody.IndexOf("<a href=\"/people\" class='item-link'>") == -1)
                {
                    continue;
                }
                int strStart5 = responseBody.IndexOf("<section id=\"page-content\"");
                strStart5 = responseBody.IndexOf("<h2>", strStart5) + 4;

                int strEnd5 = responseBody.IndexOf("<", strStart5);
                string qualifications = "";

                int qualificationsStart = responseBody.IndexOf($"Место работы</h3>");
                if (qualificationsStart != -1)
                {
                    qualificationsStart = responseBody.IndexOf("<ul>", qualificationsStart) + 4;
                    int qualificationsEnd = responseBody.IndexOf("</ul>", qualificationsStart);
                    qualifications = responseBody.Substring(qualificationsStart, qualificationsEnd - qualificationsStart);

                    qualifications = qualifications.Replace("<li>", "").Replace("</li>", "\n");
                }
                Work= ProcessString(qualifications);
                int emailStart = responseBody.IndexOf("<strong class='key'>e-mail:</strong>");
                if (emailStart != -1)
                {
                    emailStart = responseBody.IndexOf("<span class='mail'>", emailStart) + "<span class='mail'>".Length;
                    int emailEnd = responseBody.IndexOf("</span>", emailStart);
                    Email = ProcessString(responseBody.Substring(emailStart, emailEnd - emailStart));
                }
                int phoneStartIndex = responseBody.IndexOf("<strong class='key'>телефон:</strong>");
                if (phoneStartIndex != -1)
                {
                    phoneStartIndex = responseBody.IndexOf("+", phoneStartIndex);
                    int phoneEndIndex = responseBody.IndexOf("</a>", phoneStartIndex);

                    if (phoneStartIndex != -1 && phoneEndIndex != -1)
                    {
                        Phone = ProcessString(responseBody.Substring(phoneStartIndex, phoneEndIndex - phoneStartIndex));
                    }
                }
                int addressStartIndex = responseBody.IndexOf("<strong class='key'>адрес:</strong>");
                if (addressStartIndex != -1)
                {
                    addressStartIndex = responseBody.IndexOf("<a href=", addressStartIndex);
                    int addressEndIndex = responseBody.IndexOf("</a>", addressStartIndex);

                    if (addressStartIndex != -1 && addressEndIndex != -1)
                    {
                        Address = ProcessString(responseBody.Substring(addressStartIndex, addressEndIndex - addressStartIndex));
                    }
                }
                int strStart1 = responseBody.IndexOf("<section id=\"page-content\"");
                strStart1 = responseBody.IndexOf("<h2>", strStart1) + 4;

                int strEnd1 = responseBody.IndexOf("<", strStart1);
                Name = ProcessString(responseBody.Substring(strStart1, strEnd1 - strStart1));
                Match phoneNumberMatch = Regex.Match(Phone, @"(\+7\s*\(\d{3}\)\s*\d{3}-\d{2}-\d{2})");
                Phone=phoneNumberMatch.Value;
                teacher Teacher = new teacher()
                {
                    Name = Name,
                    Email = Email,
                    Adress = Address,
                    Phone = Phone,
                    Work = Work,
                };
                Teachers.Add(Teacher);
            }
            foreach(teacher teacher1 in Teachers)
            {
                repository.Add(teacher1);
            }
        }
    }
}
