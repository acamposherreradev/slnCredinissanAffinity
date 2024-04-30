using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net.Mail;
using System.Web;
using System.IO;
using System.Web.Script.Serialization;
using System.Net;

namespace Credinissan.Affinity.Web.Common
{
    public class Notificacion
    {

        public class Dato
        {
            public string Atributo { get; set; }
            public string Valor { get; set; }
        }

        public string Asunto { get; set; }

        public string Body { get; set; }
        public string From { get; set; }

        public string Mask { get; set; }

        public string To { get; set; }

        public string CC { get; set; }

        public string CCO { get; set; }

        //public string Plantilla { get; set; }

        public string Mensaje { get; set; }

        public List<Dato> Datos { get; set; }

        public int status { get; set; }


        public void SendMail()
        {

           
            var message = new MailMessage(From, To)
            {
                From = new MailAddress(From, Mask),
                Subject = Asunto,
                IsBodyHtml = true,
                Body = Body,
            };

            if (CC != null)
            {
                foreach (string copia in CC.Split(';'))
                {
                    message.CC.Add(copia);
                }
            }

            if (CCO != null)
            {
                foreach (string copia in CCO.Split(';'))
                {
                    message.Bcc.Add(copia);
                }
            }

            //if (Adjunto != null)
            //    message.Attachments.Add(Adjunto);

            var client = new SmtpClient();
            client.Send(message);


        }



        //public void Push(Cliente cliente, string title, string body)
        //{
        //    Context context = new Context();
        //    List<Usuario> usuarios = context.Usuarios.Where(u=> u.Cliente.Id== cliente.Id && u.Token != null).ToList();
        //    foreach (Usuario usuario in usuarios)
        //    {
        //        SendNotificationFromFirebaseCloud(usuario.Token,  body, title);
        //    }
        //}

        //public void NotoficarARod(string title, string body, int idCliente =0)
        //{
        //    var token = "f6B-KJic9O4:APA91bFGFps4BXhjpRaCJ5RKz7Sfxt8Y6prM2DErLAknZ7TsEr7BfGEBER9Cme89W9Z4mXpfBycvkOLHBdN9VX9QQJ-JZl0qjAA_ZMP7kgppviScPGHMd5c7yAHj-tjRswWCIXtLSdFU";
        //    //var title = "Buenos días Rod 😊";
        //    //var body = "Hoy será un gran día con Fitme App";
        //    SendNotificationFromFirebaseCloud(token, body, title);

        //    Context context = new Context();
        //    if (idCliente>0)
        //        context.Logs.Add(new Log { Texto = title + " - " + body, FechaHora= DateTime.Now, Cliente = context.Clientes.Find(idCliente)});
        //    else
        //        context.Logs.Add(new Log { Texto = title + " - " + body, FechaHora = DateTime.Now });

        //    context.SaveChanges();

        //}

        private string SendNotificationFromFirebaseCloud(string token, string body, string title)
        {
            var result = "-1";
            var webAddr = "https://fcm.googleapis.com/fcm/send";

            var httpWebRequest = (HttpWebRequest)WebRequest.Create(webAddr);
            httpWebRequest.ContentType = "application/json; charset=utf-8";

            /* Esta "Key" es de la cuenta zetta.fitme@gmail.com
             * Tú la puedes poner en tu web.config
             */
            httpWebRequest.Headers.Add("Authorization:key=" + "AAAATUJWO1U:APA91bFTb4qSygj5005uwJtUmIESvLA70yUk7SppEQDhrppoYEfWKTTUSbG2pOJ6Hoy9XGXCQL44DhUmjgB87V5QhP3IskmZNi_ZSK5i-hBKu6OlQLqrBGKVvIvpnfH88fFtay7vPzgf");
            httpWebRequest.Method = "POST";

            using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
            {
                var data = new
                {
                    to = token,
                    notification = new
                    {
                        body = body,
                        title = title
                    }
                };

                var json = new JavaScriptSerializer().Serialize(data);
                streamWriter.Write(json);
                streamWriter.Flush();
            }

            var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
            using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
            {
                result = streamReader.ReadToEnd();
            }

            return result;
        }



    }
}