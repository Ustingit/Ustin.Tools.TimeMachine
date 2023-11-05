using Ustin.Tools.TimeMachine.Abstractions;
using Ustin.Tools.TimeMachine.Abstractions.Ticks;
using Ustin.Tools.TimeMachine.Models.Time;

namespace Ustin.Tools.TimeMachine.TimeIterator;

public class TimeMachineV1<TOperationContext, TTickContext, TTickResult> 
    where TTickResult : new()
    where TOperationContext : ITimerContext<TTickResult>
    where TTickContext : ITickContext
{   
    		private readonly FractionedPeriod _period;
		private readonly TOperationContext _operationalContext;

		private Func<TTickContext, TTickResult> _defaultOnTick = delegate(TTickContext context)
		{
			Console.WriteLine($"Empty iteration {context.TickDateTime.ToShortDateString()}.");
			return default;
		};

		public TimeMachineV1(FractionedPeriod period, TOperationContext context)
		{
			_period = period ?? throw new ArgumentNullException(nameof(period));
			_operationalContext = context ?? throw new ArgumentNullException(nameof(context));
		}

		public void Start()
		{
			_operationalContext.Before(_period);
			foreach (var tick in _period)
			{
				_operationalContext?.OnTick(_operationalContext.GetTickContext(tick));
			}
			_operationalContext.After(_period);
		}
}