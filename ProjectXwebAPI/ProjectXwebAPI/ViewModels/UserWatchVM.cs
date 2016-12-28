using ProjectXwebAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProjectXwebAPI.ViewModels
{
    public class UserWatchVM
    {
        public string UserId { get; set; }
        public int StoryId { get; set; }

        public UserWatchVM(UserWatch userWatch)
        {
            this.UserId = userWatch.UserId;
            this.StoryId = userWatch.StoryId;
        }
    }
}