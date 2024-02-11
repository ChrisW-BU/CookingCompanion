﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Data.Models.Interfaces;

namespace Data.Models.Models
{
    public class UserGlobal
    {
        public int Id { get; set; }
        public bool LoggedIn { get; set; }
        public string UserName { get; set; } = string.Empty;

        public bool IsAdmin { get; set; }

        public Guid UserToken { get; set; } = Guid.Empty;

        private bool TasksCompleted { get; set; }

        public void SetUserInfo(User user)
        {
            if (user != null)
            {
                Id = user.Id;
                UserName = user.Name;
                IsAdmin = user.IsAdmin;
                LoggedIn = true;
                UserToken = user.UserToken;
                TasksCompleted = user.TasksCompleted;
            }
        }

        public void ResetUserInfo()
        {
            Id = 0;
            UserName = string.Empty;
            IsAdmin = false;
            LoggedIn = false;
            UserToken = Guid.Empty;
            TasksCompleted = false;
        }

        public void SetTaskComplete(bool x)
        {
            TasksCompleted = x;
        }

        public void UpdateUserTaskStatus(User? u)
        {
            if (u != null)
            {
                u.TasksCompleted = TasksCompleted;
            }
        }

        public bool GetTaskComplete()
        {
            return TasksCompleted;
        }
    }
}
