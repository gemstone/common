//******************************************************************************************************
//  ScheduleManager.cs - Gbtc
//
//  Copyright © 2012, Grid Protection Alliance.  All Rights Reserved.
//
//  Licensed to the Grid Protection Alliance (GPA) under one or more contributor license agreements. See
//  the NOTICE file distributed with this work for additional information regarding copyright ownership.
//  The GPA licenses this file to you under the MIT License (MIT), the "License"; you may
//  not use this file except in compliance with the License. You may obtain a copy of the License at:
//
//      http://www.opensource.org/licenses/MIT
//
//  Unless agreed to in writing, the subject software distributed under the License is distributed on an
//  "AS-IS" BASIS, WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied. Refer to the
//  License for the specific language governing permissions and limitations.
//
//  Code Modification History:
//  ----------------------------------------------------------------------------------------------------
//  08/01/2006 - Pinal C. Patel
//       Original version of source code generated.
//  04/23/2007 - Pinal C. Patel
//       Made the schedules dictionary case-insensitive.
//  04/24/2007 - Pinal C. Patel
//       Implemented the IPersistSettings and ISupportInitialize interfaces.
//  05/02/2007 - Pinal C. Patel
//       Converted schedules to a list instead of a dictionary.
//  09/19/2008 - J. Ritchie Carroll
//       Convert to C#.
//  11/04/2008 - Pinal C. Patel
//       Edited code comments.
//  06/18/2009 - Pinal C. Patel
//       Fixed the implementation of Enabled property.
//  09/14/2009 - Stephen C. Wills
//       Added new header and license agreement.
//  06/21/2010 - Stephen C. Wills
//       Modified code to avoid calls to abort the m_startTimerThread.
//  07/02/2010 - J. Ritchie Carroll
//       Fixed an issue related to accessing a disposed timer.
//  08/04/2010 - Pinal C. Patel
//       Fixed an issue with NullReferenceException being encountered when ScheduleManager object is 
//       disposed before it successfully starts up (StartTimer() method).
//  12/14/2012 - Starlynn Danyelle Gilliam
//       Modified Header.
//
//******************************************************************************************************

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Timers;
using Gemstone.EventHandlerExtensions;
using Timer = System.Timers.Timer;

namespace Gemstone.Scheduling;

/// <summary>
/// Monitors multiple <see cref="Schedule"/> at an interval of one minute to check if they are due.
/// </summary>
/// <example>
/// This example shows how to use the <see cref="ScheduleManager"/> component:
/// <code>
/// using System;
/// using Gemstone;
/// using Gemstone.Scheduling;
/// 
/// class Program
/// {
///     static void Main(string[] args)
///     {
///         ScheduleManager scheduler = new ScheduleManager();
///
///         // Add event handlers.
///         scheduler.Starting += scheduler_Starting;
///         scheduler.Started += scheduler_Started;
///         scheduler.ScheduleDue += scheduler_ScheduleDue;
///
///         // Add test schedules.
///         scheduler.AddSchedule("Run.Notepad", "* * * * *");
///         scheduler.AddSchedule("Run.Explorer", "* * * * *");
///
///         // Start the scheduler.
///         scheduler.Start();
/// 
///         Console.ReadLine();
///     }
/// 
///     static void scheduler_Started(object? sender, EventArgs e)
///     {
///         Console.WriteLine("Scheduler has started successfully.");
///     }
/// 
///     static void scheduler_Starting(object? sender, EventArgs e)
///     {
///         Console.WriteLine("Scheduler is waiting to be started.");
///     }
/// 
///     static void scheduler_ScheduleDue(object? sender, EventArgs;lt;Schedule&gt; e)
///     {
///         Console.WriteLine(string.Format("{0} schedule is due for processing.", e.Argument.Name));
///     }
/// }
/// </code>
/// </example>
/// <seealso cref="Schedule"/>
public class ScheduleManager : IDisposable, IProvideStatus
{
    #region [ Members ]

    // Constants

    /// <summary>
    /// Number of milliseconds between timer ticks.
    /// </summary>
    private const int TimerInterval = 60000;

    // Events

    /// <summary>
    /// Occurs while the <see cref="ScheduleManager"/> is waiting to start at the top of the minute.
    /// </summary>
    public event EventHandler? Starting;

    /// <summary>
    /// Occurs when the <see cref="ScheduleManager"/> has started at the top of the minute.
    /// </summary>
    public event EventHandler? Started;

    /// <summary>
    /// Occurs asynchronously when a <see cref="Schedule"/> is due according to the rule specified for it.
    /// </summary>
    /// <remarks>
    /// <see cref="EventArgs{T}.Argument"/> is the <see cref="Schedule"/> that is due.
    /// </remarks>
    public event EventHandler<EventArgs<Schedule>>? ScheduleDue;

    /// <summary>
    /// Occurs when the a particular <see cref="Schedule"/> is being checked to see if it is due.
    /// </summary>
    /// <remarks>
    /// <see cref="EventArgs{T}.Argument"/> is the <see cref="Schedule"/> that is being checked to see if it is due.
    /// </remarks>
    public event EventHandler<EventArgs<Schedule>>? ScheduleDueCheck;

    // Fields
    private string m_name;
    private readonly List<Schedule> m_schedules;
    private Timer? m_timer;
    private Thread? m_startTimerThread;
    private bool m_disposed;

    #endregion

    #region [ Constructors ]

    /// <summary>
    /// Initializes a new instance of the <see cref="ScheduleManager"/> class.
    /// </summary>
    public ScheduleManager()
    {
        m_name = GetType().Name;
        m_schedules = new List<Schedule>();
    }

    #endregion

    #region [ Properties ]
        
    /// <summary>
    /// Gets or sets the name of the <see cref="ScheduleManager"/>.
    /// </summary>
    /// <exception cref="ArgumentNullException">The value being assigned is null or empty string.</exception>
    public string Name
    {
        get
        {
            return m_name;
        }
        set
        {
            if (string.IsNullOrEmpty(value))
                throw new ArgumentNullException(nameof(value));

            m_name = value;
        }
    }

    /// <summary>
    /// Gets a list of all <see cref="Schedule"/> monitored by the <see cref="ScheduleManager"/> object.
    /// </summary>
    public ReadOnlyCollection<Schedule> Schedules
    {
        get
        {
            lock (m_schedules)
                return m_schedules.AsReadOnly();
        }
    }

    /// <summary>
    /// Gets a boolean value that indicates whether the <see cref="ScheduleManager"/> is running.
    /// </summary>
    public bool IsRunning
    {
        get
        {
            return m_timer?.Enabled ?? false;
        }
    }

    /// <summary>
    /// Gets or sets a boolean value that indicates whether the <see cref="ScheduleManager"/> object is currently enabled.
    /// </summary>
    /// <remarks>
    /// <see cref="Enabled"/> property is not be set by user-code directly.
    /// </remarks>
    public bool Enabled
    {
        get
        {
            return IsRunning;
        }
        set
        {
            if (value && !Enabled)
                Start();
            else if (!value && Enabled)
                Stop();
        }
    }

    /// <summary>
    /// Gets the descriptive status of the <see cref="ScheduleManager"/>.
    /// </summary>
    public string Status
    {
        get
        {
            StringBuilder status = new();
            Schedule[] schedules;

            lock (m_schedules)
                schedules = m_schedules.ToArray();

            status.AppendLine($"       Number of schedules: {schedules.Length:N0}");

            for (int i = 0; i < schedules.Length; i++)
            {
                Schedule schedule = schedules[i];

                status.AppendLine();
                status.AppendLine($"Schedule {i + 1:N0}:");
                status.Append(schedule.Status);
            }

            return status.ToString();
        }
    }

    #endregion

    #region [ Methods ]

    /// <summary>
    /// Releases all the resources used by the <see cref="ScheduleManager"/> object.
    /// </summary>
    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    /// <summary>
    /// Releases the unmanaged resources used by the <see cref="ScheduleManager"/> object and optionally releases the managed resources.
    /// </summary>
    /// <param name="disposing">true to release both managed and unmanaged resources; false to release only unmanaged resources.</param>
    protected virtual void Dispose(bool disposing)
    {
        if (m_disposed)
            return;

        try
        {
            if (disposing)
                Stop();
        }
        finally
        {
            m_disposed = true;  // Prevent duplicate dispose.
        }
    }

    /// <summary>
    /// Starts the <see cref="ScheduleManager"/> asynchronously if not running.
    /// </summary>
    public void Start()
    {
        if (IsRunning || m_startTimerThread is not null)
            return;

        // Initialize timer that checks schedules.
        m_timer = new Timer(TimerInterval);
        m_timer.Elapsed += m_timer_Elapsed;

        // Spawn new thread to start timer at top of the minute.
        m_startTimerThread = new Thread(StartTimer) { IsBackground = true };
        m_startTimerThread.Start();
    }

    /// <summary>
    /// Stops the <see cref="ScheduleManager"/> if running.
    /// </summary>
    public void Stop()
    {
        if (m_timer is null)
            return;

        m_timer.Elapsed -= m_timer_Elapsed;
        m_timer.Dispose();

        m_timer = null;
        m_startTimerThread = null;
    }

    /// <summary>
    /// Checks all of the <see cref="Schedules"/> to determine if they are due.
    /// </summary>
    public void CheckAllSchedules()
    {
        Schedule[] schedules;

        lock (m_schedules)
            schedules = m_schedules.ToArray();

        foreach (Schedule schedule in schedules)
        {
            OnScheduleDueCheck(new EventArgs<Schedule>(schedule));

            // Schedule is due so raise the event.
            if (schedule.IsDue())
                OnScheduleDue(schedule);
        }
    }

    /// <summary>
    /// Attempts to add a new <see cref="Schedule"/>.
    /// </summary>
    /// <param name="scheduleName">Name of the new <see cref="Schedule"/>.</param>
    /// <param name="scheduleRule">Rule of the new <see cref="Schedule"/>.</param>
    /// <param name="useLocalTime">Flag that determines whether to use local time for schedule.</param>
    /// <param name="updateExisting">Flag that determines whether to update existing <see cref="Schedule"/> with the specified <paramref name="scheduleRule"/>.</param>
    /// <returns><c>true</c> if a new <see cref="Schedule"/> was added or an existing one was updated; otherwise <c>false</c>.</returns>
    public bool AddSchedule(string scheduleName, string scheduleRule, bool useLocalTime = false, bool updateExisting = false)
    {
        Schedule? existingSchedule = FindSchedule(scheduleName);

        if (existingSchedule is null)
        {
            // Schedule doesn't exist, so we'll add it.
            lock (m_schedules)
                m_schedules.Add(new Schedule(scheduleName, scheduleRule, useLocalTime));

            return true;
        }

        if (!updateExisting)
            return false;

        // Update existing schedule.
        existingSchedule.Name = scheduleName;
        existingSchedule.Rule = scheduleRule;
        existingSchedule.UseLocalTime = useLocalTime;

        return true;
    }

    /// <summary>
    /// Attempts to remove a <see cref="Schedule"/> with the specified name if one exists.
    /// </summary>
    /// <param name="scheduleName">Name of the <see cref="Schedule"/> to be removed.</param>
    /// <returns><c>true</c> if the <see cref="Schedule"/> was removed; otherwise, <c>false</c>.</returns>
    public bool RemoveSchedule(string scheduleName)
    {
        Schedule? scheduleToRemove = FindSchedule(scheduleName);

        if (scheduleToRemove is null)
            return false;

        // Schedule exists, so remove it.
        lock (m_schedules)
            m_schedules.Remove(scheduleToRemove);

        return true;
    }

    /// <summary>
    /// Searches for the <see cref="Schedule"/> with the specified name.
    /// </summary>
    /// <param name="scheduleName">Name of the <see cref="Schedule"/> to be obtained.</param>
    /// <returns><see cref="Schedule"/> object if a match is found; otherwise <c>null</c>.</returns>
    public Schedule? FindSchedule(string scheduleName)
    {
        lock (m_schedules)
            return m_schedules.FirstOrDefault(schedule => schedule.Name.Equals(scheduleName, StringComparison.OrdinalIgnoreCase));
    }

    /// <summary>
    /// Raises the <see cref="Starting"/> event.
    /// </summary>
    protected virtual void OnStarting()
    {
        Starting?.SafeInvoke(this, EventArgs.Empty);
    }

    /// <summary>
    /// Raises the <see cref="Started"/> event.
    /// </summary>
    protected virtual void OnStarted()
    {
        Started?.SafeInvoke(this, EventArgs.Empty);
    }

    /// <summary>
    /// Raises the <see cref="ScheduleDue"/> event.
    /// </summary>
    /// <param name="schedule"><see cref="Schedule"/> to send to <see cref="ScheduleDue"/> event.</param>
    protected virtual void OnScheduleDue(Schedule schedule)
    {
        ScheduleDue?.SafeInvoke(this, new EventArgs<Schedule>(schedule));
    }

    /// <summary>
    /// Raises the <see cref="ScheduleDueCheck"/> event.
    /// </summary>
    /// <param name="e">Event data.</param>
    protected virtual void OnScheduleDueCheck(EventArgs<Schedule> e)
    {
        ScheduleDueCheck?.SafeInvoke(this, e);
    }

    private void StartTimer()
    {
        while (!IsRunning && m_timer is not null)
        {
            OnStarting();

            if (DateTime.UtcNow.Second == 0)
            {
                // We'll start the timer that will check the schedules at top of the minute.
                m_timer.Start();
                OnStarted();

                CheckAllSchedules();
                break;
            }

            Thread.Sleep(500);
        }

        m_startTimerThread = null;
    }

    private void m_timer_Elapsed(object? sender, ElapsedEventArgs e)
    {
        CheckAllSchedules();
    }

    #endregion
}
