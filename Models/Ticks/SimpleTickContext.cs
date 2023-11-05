using Ustin.Tools.TimeMachine.Abstractions.Ticks;

namespace Ustin.Tools.TimeMachine.Models.Ticks;

public record SimpleTickContext(DateTime TickDateTime) : ITickContext;