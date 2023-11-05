using System.Collections;

namespace Ustin.Tools.TimeMachine.Models.Time;

public record Period(DateTime From, DateTime To);

public record FractionedPeriod : Period, IEnumerable<DateTime>
{
	private readonly PeriodType _periodType;

	private DateTime[] _periodDates;
	private DateTime _currentIterationDate;

	public FractionedPeriod(DateTime from, DateTime to, PeriodType type = PeriodType.Month)
		: base(from, to)
	{
		_periodType = type;
		_currentIterationDate = from;
		_periodDates = null;
	}

	public DateTime[] PeriodDatesByType(PeriodType type = PeriodType.Month)
	{
		if (_periodDates != null)
		{
			return _periodDates;
		}

		var currentDate = From;
		var result = new List<DateTime>();
		result.Add(currentDate);

		while (currentDate < To)
		{
			DateTime newDate = GetNextDate(currentDate);

			result.Add(newDate);
			currentDate = newDate;
		}

		result.Add(To);

		_periodDates = result.ToArray();
		return _periodDates;
	}

	private DateTime GetNextDate(DateTime source)
	{
		switch (_periodType)
		{
			case PeriodType.Second:
				return source.AddSeconds(1);
			case PeriodType.Minute:
				return source.AddMinutes(1);
			case PeriodType.Hour:
				return source.AddHours(1);
			case PeriodType.HalfDay:
				return source.AddHours(12);
			case PeriodType.Day:
				return source.AddDays(1);
			case PeriodType.Week:
				return source.AddDays(7);
			case PeriodType.Month:
				return source.AddMonths(1);
			case PeriodType.Quarter:
				return source.AddMonths(3);
			case PeriodType.HalfYear:
				return source.AddMonths(6);
			case PeriodType.Year:
				return source.AddYears(1);
			default:
				throw new InvalidOperationException(nameof(_periodType));
		}
	}

	public IEnumerator<DateTime> GetEnumerator()
	{
		return new FractionedTimerEnumerator(this);
	}

	IEnumerator IEnumerable.GetEnumerator()
	{
		return GetEnumerator();
	}

	private class FractionedTimerEnumerator : IEnumerator<DateTime>
	{
		private readonly FractionedPeriod _period;

		public FractionedTimerEnumerator(FractionedPeriod period)
		{
			_period = period;
		}

		public bool MoveNext()
		{
			var hasNext = _period.GetNextDate(_period._currentIterationDate) <= _period.To;

			if (hasNext)
			{
				_period._currentIterationDate = _period.GetNextDate(_period._currentIterationDate);
			}

			return hasNext;
		}

		public void Reset()
		{
			_period._currentIterationDate = _period.From;
		}

		object IEnumerator.Current => Current;

		public DateTime Current => _period._currentIterationDate;

		public void Dispose()
		{
		}
	}
}