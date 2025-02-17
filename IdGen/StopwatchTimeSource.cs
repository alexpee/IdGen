﻿using System;
using System.Diagnostics;

namespace IdGen
{
    /// <summary>
    /// Provides time data to an <see cref="IdGenerator"/>. This timesource uses a <see cref="Stopwatch"/> for timekeeping.
    /// </summary>
    public abstract class StopwatchTimeSource : ITimeSource
    {
        private static readonly Stopwatch _sw = new();
        private static readonly DateTimeOffset _initialized = DateTimeOffset.UtcNow;
        private static string Maxdt = _initialized.Year + "-12-31T23:59:59.9999999Z";
        private static readonly DateTimeOffset _endOfTheYear = DateTimeOffset.Parse(Maxdt);
        /// <summary>
        /// Gets the epoch of the <see cref="ITimeSource"/>.
        /// </summary>
        public DateTimeOffset Epoch { get; private set; }



        /// <summary>
        /// Gets the elapsed time since this <see cref="ITimeSource"/> was initialized.
        /// </summary>
        protected static TimeSpan Elapsed => _sw.Elapsed;

        /// <summary>
        /// Gets the offset for this <see cref="ITimeSource"/> which is defined as the difference of it's creationdate
        /// and it's epoch which is specified in the object's constructor.
        /// </summary>
        protected TimeSpan Offset { get; private set; }

        /// <summary>
        /// Gets the maximum for this <see cref="ITimeSource"/> which is defined as the difference of it's creationdate
        /// and it's the end datetime of the year which is specified in the object's constructor.
        /// </summary>
        protected TimeSpan MaxTimeSpan { get; private set; }

        /// <summary>
        /// Initializes a new <see cref="StopwatchTimeSource"/> object.
        /// </summary>
        /// <param name="epoch">The epoch to use as an offset from now,</param>
        /// <param name="tickDuration">The duration of a single tick for this timesource.</param>
        public StopwatchTimeSource(DateTimeOffset epoch, TimeSpan tickDuration)
        {
            Epoch = epoch;
            Offset = (_initialized - Epoch);
            MaxTimeSpan = _endOfTheYear - Epoch;
            TickDuration = tickDuration;

            // Start (or resume) stopwatch
            _sw.Start();
        }

        /// <summary>
        /// Returns the duration of a single tick.
        /// </summary>
        public TimeSpan TickDuration { get; private set; }

        /// <summary>
        /// Returns the current number of ticks for the <see cref="DefaultTimeSource"/>.
        /// </summary>
        /// <returns>The current number of ticks to be used by an <see cref="IdGenerator"/> when creating an Id.</returns>
        public abstract long GetTicks();

        /// <summary>
        /// Returns the maximum number of ticks for the <see cref="DefaultTimeSource"/>.
        /// </summary>
        /// <returns>The maximum number of ticks to be used by an <see cref="IdGenerator"/> when creating an Id.</returns>
        public abstract long GetYearlyMaxTicks();

    }
}
