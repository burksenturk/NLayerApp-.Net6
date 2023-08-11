﻿namespace NLayer.Service.Exceptions
{
    public class ClientSideException : Exception //client tarafından oluşan hatalarda kullanacagımız sınıf Middleware için (UseCustomExceptionHandler)
    {
        public ClientSideException(string message) : base(message)  //Exceptiona mesaj göndereyim.
        {

        }

        //string message = exception mesajı alayım    :base(message) basedeki exceptiona göndereyim.. Base = Exception constructor ına gider.
    }
}
