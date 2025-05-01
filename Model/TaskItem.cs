using System;
using System.ComponentModel;

namespace TaskManager.Model
{
    // клас для представлення завдання
    public class TaskItem : INotifyPropertyChanged
    {
        private string _description;
        private bool _isCompleted;

        public delegate void StatusChangedEventHandler(object sender, StatusChangedEventArgs e);

        public event StatusChangedEventHandler StatusChanged;
        public event PropertyChangedEventHandler PropertyChanged;

        public string Description
        {
            get => _description;
            set
            {
                if (_description != value)
                {
                    _description = value;
                    OnPropertyChanged(nameof(Description));
                }
            }
        }

        // статус виконання
        public bool IsCompleted
        {
            get => _isCompleted;
            set
            {
                if (_isCompleted != value)
                {
                    bool oldValue = _isCompleted;
                    _isCompleted = value;

                    OnStatusChanged(new StatusChangedEventArgs(oldValue, value));

                    OnPropertyChanged(nameof(IsCompleted));
                }
            }
        }

        public TaskItem(string description, bool isCompleted = false)
        {
            _description = description;
            _isCompleted = isCompleted;
        }

        protected virtual void OnStatusChanged(StatusChangedEventArgs e)
        {
            StatusChanged?.Invoke(this, e);
        }

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    // клас для передачі додаткової інформації
    public class StatusChangedEventArgs : EventArgs
    {
        public bool OldStatus { get; }
        public bool NewStatus { get; }

        public StatusChangedEventArgs(bool oldStatus, bool newStatus)
        {
            OldStatus = oldStatus;
            NewStatus = newStatus;
        }
    }
}