using Ustin.Tools.TimeMachine.Abstractions.Ticks;
using Ustin.Tools.TimeMachine.Models.Time;

namespace Ustin.Tools.TimeMachine.Abstractions;

public interface ITimerContext<out TTickResult> 
    where TTickResult : new()
{
    TTickResult OnTick(ITickContext tickContext);
		
    ITickContext GetTickContext(DateTime tickTime);

    void Before(Period period);

    void After(Period period);
}