using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace xPlanner.Domain.Entities;

public class UserSettings
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public int PomodoroBreakInterval { get; set; }
    public int PomodoroIntervalsCount { get; set; }
    public int PomodoroWorkInterval { get; set; }
}