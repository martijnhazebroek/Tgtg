namespace Hazebroek.Tgtg.Infra
{
    public abstract class ExecutionContext
    {
        public abstract bool HasPrompt { get; }
    }

    public class CliExecutionContext: ExecutionContext
    {
        public override bool HasPrompt => true;
    }
    
    public class WorkerExecutionContext: ExecutionContext
    {
        public override bool HasPrompt => false;
    }
}