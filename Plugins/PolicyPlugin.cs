using Microsoft.SemanticKernel;

sealed class PolicyPlugin
{
    [KernelFunction]
    public string GetLeaveCarryForwardPolicy()
        => "Employees can carry forward up to 10 unused paid leaves to the next calendar year. Casual leaves do not carry forward.";
}
