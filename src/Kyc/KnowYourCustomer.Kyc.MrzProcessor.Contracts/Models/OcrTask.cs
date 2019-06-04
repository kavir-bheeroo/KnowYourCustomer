using System;
using System.Collections.Generic;

namespace KnowYourCustomer.Kyc.MrzProcessor.Contracts.Models
{
    public class OcrTask
    {
        public TaskId TaskId;
        public TaskStatus Status;

        /// <summary>
        /// When task was created. Can be null if no information
        /// </summary>
        public DateTime RegistrationTime;

        /// <summary>
        /// Last activity time. Can be null if no information
        /// </summary>
        public DateTime StatusChangeTime;

        /// <summary>
        /// Number of files in the task
        /// </summary>
        public int FilesCount = 1;

        /// <summary>
        /// Task cost in credits
        /// </summary>
        public int Credits = 0;

        /// <summary>
        /// Task description provided by user
        /// </summary>
        public string Description = null;

        /// <summary>
        /// Url to download processed tasks
        /// </summary>
        public List<string> DownloadUrls = null;

        /// <summary>
        /// Error description when task processing failed
        /// </summary>
        public string Error = null;

        public OcrTask()
        {
            Status = TaskStatus.Unknown;
            TaskId = new TaskId("<unknown>");
        }

        public OcrTask(TaskId id, TaskStatus status)
        {
            TaskId = id;
            Status = status;
        }

        public bool IsTaskActive()
        {
            return IsTaskActive(Status);
        }

        // Task is submitted or is processing
        public static bool IsTaskActive(TaskStatus status)
        {
            switch (status)
            {
                case TaskStatus.Submitted:
                case TaskStatus.Queued:
                case TaskStatus.InProgress:
                    return true;
                default:
                    return false;
            }
        }
    }
}