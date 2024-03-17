﻿namespace Hotboil.Mailer;

public class SendResponse
{
    public string MessageId { get; set; }
    public IList<string> ErrorMessages { get; set; }
    public bool Successful => !ErrorMessages.Any();

    public SendResponse()
    {
        ErrorMessages = new List<string>();
    }
}

public class SendResponse<T> : SendResponse
{
    public T Data { get; set; }
}