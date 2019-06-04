using KnowYourCustomer.Kyc.MrzProcessor.Contracts.Models;
using System;
using System.Collections.Generic;
using System.Xml.Linq;

namespace KnowYourCustomer.Kyc.MrzProcessor.Abbyy.Parsers
{
    public static class XmlParser
    {
        public static TaskId GetTaskId(XDocument xml)
        {
            string id = xml.Root.Element("task").Attribute("id").Value;
            return new TaskId(id);
        }

        public static OcrTask GetTaskStatus(XDocument xml)
        {
            return GetTaskInfo(xml.Root.Element("task"));
        }

        private static TaskStatus StatusFromString(string status)
        {
            switch (status.ToLower())
            {
                case "submitted":
                    return TaskStatus.Submitted;
                case "queued":
                    return TaskStatus.Queued;
                case "inprogress":
                    return TaskStatus.InProgress;
                case "completed":
                    return TaskStatus.Completed;
                case "processingfailed":
                    return TaskStatus.ProcessingFailed;
                case "deleted":
                    return TaskStatus.Deleted;
                case "notenoughcredits":
                    return TaskStatus.NotEnoughCredits;
                default:
                    return TaskStatus.Unknown;
            }
        }

        public static OcrTask[] GetAllTasks(XDocument xml)
        {
            var result = new List<OcrTask>();
            XElement xResponse = xml.Root;

            foreach (XElement xTask in xResponse.Elements("task"))
            {
                OcrTask task = GetTaskInfo(xTask);
                result.Add(task);
            }

            return result.ToArray();
        }


        /// <summary>
        /// Get task data from xml node "task"
        /// </summary>
        private static OcrTask GetTaskInfo(XElement xTask)
        {
            TaskId id = new TaskId(xTask.Attribute("id").Value);
            TaskStatus status = StatusFromString(xTask.Attribute("status").Value);

            var task = new OcrTask
            {
                TaskId = id,
                Status = status
            };

            XAttribute xRegistrationTime = xTask.Attribute("registrationTime");
            if (xRegistrationTime != null)
            {
                DateTime time;
                if (DateTime.TryParse(xRegistrationTime.Value, out time))
                    task.RegistrationTime = time;
            }

            XAttribute xStatusChangeTime = xTask.Attribute("statusChangeTime");
            if (xStatusChangeTime != null)
            {
                DateTime time;
                if (DateTime.TryParse(xStatusChangeTime.Value, out time))
                    task.StatusChangeTime = time;
            }

            XAttribute xFilesCount = xTask.Attribute("filesCount");
            if (xFilesCount != null)
            {
                int filesCount;
                if (int.TryParse(xFilesCount.Value, out filesCount))
                    task.FilesCount = filesCount;
            }

            XAttribute xCredits = xTask.Attribute("credits");
            if (xCredits != null)
            {
                int credits;
                if (int.TryParse(xCredits.Value, out credits))
                    task.Credits = credits;
            }

            XAttribute xDescription = xTask.Attribute("description");
            if (xDescription != null)
                task.Description = xDescription.Value;

            XAttribute xResultUrl = xTask.Attribute("resultUrl");
            if (xResultUrl != null)
            {
                task.DownloadUrls = new List<string> { xResultUrl.Value };
                for (int i = 2; i < 10; i++)
                {
                    XAttribute xResultUrlI = xTask.Attribute("resultUrl" + i);
                    if (xResultUrlI != null)
                    {
                        task.DownloadUrls.Add(xResultUrlI.Value);
                    }
                    else
                    {
                        break;
                    }
                }
            }

            XAttribute xError = xTask.Attribute("error");
            if (xError != null)
            {
                task.Error = xError.Value;
            }

            return task;
        }
    }
}