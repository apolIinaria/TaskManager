using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows.Input;
using TaskManager.Model;

namespace TaskManager.ViewModel
{
    public class TaskManagerViewModel : INotifyPropertyChanged
    {
        private string _newTaskDescription;
        private ObservableCollection<TaskItem> _tasks;
        private string _statusInfo;

        public event PropertyChangedEventHandler PropertyChanged;

        // новий опис завдання
        public string NewTaskDescription
        {
            get => _newTaskDescription;
            set
            {
                if (_newTaskDescription != value)
                {
                    _newTaskDescription = value;
                    OnPropertyChanged(nameof(NewTaskDescription));

                    (AddTaskCommand as RelayCommand)?.RaiseCanExecuteChanged();
                }
            }
        }

        // колекція завдань для відображення
        public ObservableCollection<TaskItem> Tasks
        {
            get => _tasks;
            set
            {
                if (_tasks != value)
                {
                    _tasks = value;
                    OnPropertyChanged(nameof(Tasks));
                    UpdateStatusInfo();
                }
            }
        }

        // інформація про статус завдань
        public string StatusInfo
        {
            get => _statusInfo;
            set
            {
                if (_statusInfo != value)
                {
                    _statusInfo = value;
                    OnPropertyChanged(nameof(StatusInfo));
                }
            }
        }

        // команда для додавання нового завдання
        public ICommand AddTaskCommand { get; private set; }

        public TaskManagerViewModel()
        {
            _tasks = new ObservableCollection<TaskItem>();
            _newTaskDescription = string.Empty;

            AddTaskCommand = new RelayCommand(ExecuteAddTask, CanAddTask);

            UpdateStatusInfo();
        }

        // метод для виконання команди додавання завдання
        private void ExecuteAddTask(object parameter)
        {
            if (!string.IsNullOrWhiteSpace(NewTaskDescription))
            {
                var newTask = new TaskItem(NewTaskDescription);

                // підписуємося на подію зміни статусу через лямбда-вираз
                newTask.StatusChanged += (sender, args) =>
                {
                    Console.WriteLine($"Завдання '{(sender as TaskItem)?.Description}' змінило статус з {args.OldStatus} на {args.NewStatus}");

                    UpdateStatusInfo();
                };

                Tasks.Add(newTask);
                NewTaskDescription = string.Empty;

                UpdateStatusInfo();
            }
        }

        private bool CanAddTask(object parameter)
        {
            return !string.IsNullOrWhiteSpace(NewTaskDescription);
        }

        // метод для оновлення інформації про статус
        private void UpdateStatusInfo()
        {
            int total = Tasks.Count;
            int completed = Tasks.Count(t => t.IsCompleted);

            if (total == 0)
            {
                StatusInfo = "Немає завдань. Додайте нове завдання, щоб розпочати.";
            }
            else
            {
                int percentage = total > 0 ? (completed * 100) / total : 0;
                StatusInfo = $"Виконано {completed} з {total} завдань ({percentage}%)";
            }
        }

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    // реалізація ICommand
    public class RelayCommand : ICommand
    {
        private readonly Action<object> _execute;
        private readonly Predicate<object> _canExecute;

        public event EventHandler CanExecuteChanged;

        public RelayCommand(Action<object> execute, Predicate<object> canExecute = null)
        {
            _execute = execute ?? throw new ArgumentNullException(nameof(execute));
            _canExecute = canExecute;
        }

        public bool CanExecute(object parameter)
        {
            return _canExecute == null || _canExecute(parameter);
        }

        public void Execute(object parameter)
        {
            _execute(parameter);
        }

        public void RaiseCanExecuteChanged()
        {
            CanExecuteChanged?.Invoke(this, EventArgs.Empty);
        }
    }
}