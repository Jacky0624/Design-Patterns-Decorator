var list = new List<TaskItem>()
{
    new TaskItem("training module", 1, false),
    new TaskItem("reader SDK", 6, false),
    new TaskItem("cart project", 10, false),
};


ISchedule schedule = new Schedule(list);


schedule = new CostDownSchedule(schedule, 10);
var urgentTask = new TaskItem("transaction debug", 20, true);
schedule = new WithUrgentSchedule(schedule, urgentTask);

foreach (var item in schedule.Tasks)
{
    Console.WriteLine(item.ToString());
}
interface ITaskItem
{
    string Name { get; }
    double Score { get; }
    bool IsUrgent { get; }
}

class TaskItem : ITaskItem
{
    public TaskItem(string name, double score, bool isUrgent)
    {
        Name = name;
        Score = score;
        IsUrgent = isUrgent;
    }

    public string Name { get; }
    public double Score { get; }
    public bool IsUrgent { get; }

    public override string ToString()
    {
        string urgentDisplay = IsUrgent? "---Urgent Task--- " : string.Empty;
        return $"{urgentDisplay} Name : {Name}, Score : {Score}";
    }
}



class CostDownTaskItem : ITaskItem
{
    private readonly ITaskItem _item;
    private readonly double _costDownPercentage;

    public CostDownTaskItem(ITaskItem item, double costDownPercentage)
    {
        _item = item;
        _costDownPercentage = costDownPercentage;
    }

    public string Name => _item.Name;
    public double Score => _item.Score * (100 -_costDownPercentage) / 100;
    public bool IsUrgent => _item.IsUrgent;
    public override string ToString()
    {
        string urgentDisplay = IsUrgent ? "---Urgent Task--- " : string.Empty;
        return $"{urgentDisplay} Name : {Name}, Score : {Score}";
    }
}


interface ISchedule
{
    IEnumerable<ITaskItem> Tasks { get; }
}

class Schedule : ISchedule
{
    public IEnumerable<ITaskItem> Tasks { get; }
    public Schedule(IEnumerable<ITaskItem> tasks)
    {
        Tasks = tasks;
    } 

}
class CostDownSchedule : ISchedule
{
    private readonly ISchedule _schedule;
    private readonly double _costDownPercentage;

    public CostDownSchedule(ISchedule schedule, double costDownPercentage)
    {
        _schedule = schedule;
        _costDownPercentage = costDownPercentage;
    }

    public IEnumerable<ITaskItem> Tasks => _schedule.Tasks.Select(ToCostDwonTaskItem);


    private ITaskItem ToCostDwonTaskItem(ITaskItem taskItem)
    {
        return new CostDownTaskItem(taskItem, _costDownPercentage);
    }
}

class WithUrgentSchedule : ISchedule
{
    private readonly ISchedule _schedule;
    private readonly ITaskItem _urgentTaskItem;

    public WithUrgentSchedule(ISchedule schedule, ITaskItem urgentTaskItem)
    {
        _schedule = schedule;
        _urgentTaskItem = urgentTaskItem;
    }

    public IEnumerable<ITaskItem> Tasks => _schedule.Tasks.Append(_urgentTaskItem);
}







