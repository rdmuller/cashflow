namespace CashFlow.Communication.Responses;
public class ResponseErrorJson
{
    public /*required*/ List<string> ErrorMessages { get; set; }

    public ResponseErrorJson(List<string> errorMessages) // construtor
    {
        ErrorMessages = errorMessages;
    }

    public ResponseErrorJson(string errorMessage) // construtor
    {
        ErrorMessages = [errorMessage];
    }

}
