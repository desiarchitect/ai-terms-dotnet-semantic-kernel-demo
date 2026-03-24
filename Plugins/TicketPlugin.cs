using Microsoft.SemanticKernel;

sealed class TicketPlugin
{
    [KernelFunction]
    public string GetOpenTickets()
        => "The backend team has 7 open tickets. Top issues: login failure after password reset, MFA sync delay, and token expiry.";
}
