using ActorModel.Actors;
using ActorModel.Mapping;
using ActorModel.Messages;
using ActorModel.Models;
using ActorModel.Reducers;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ActorModel.TaskManager
{
    internal class TaskHandler
    {
        public OriginalTaskHandler OriginalTasks;
        CurrentTaskHandler CurrentTasks;
        public ReduceTaskHandler ReduceTasks;
        FinishedTaskHandler FinishedTasks;
        ReduceIdentifier ReduceList;

        public TaskHandler()
        {
            this.OriginalTasks = new OriginalTaskHandler();
            this.CurrentTasks = new CurrentTaskHandler();
            this.ReduceTasks = new ReduceTaskHandler();
            this.FinishedTasks = new FinishedTaskHandler();
            this.ReduceList = new ReduceIdentifier();
        }

        public long AddNewTask(SetupWorkMessage message)
        {
            long ID = this.OriginalTasks.addNewTask(message);
            this.CurrentTasks.AddTask(ID, message.TheDLL);
            this.ReduceList.CreateNewIdentity(ID);
            this.ReduceTasks.Add(ID, message.TheDLL);
            return ID;
        }

        public bool SendMap(Routing Route)
        {
            bool sentMessage = false;
            if (OriginalTasks.Count() > 0)
            {
                TaskInfoOriginal taskToWork = OriginalTasks.GetPieceToWork();
                TaskIdentifier individualTask = taskToWork.GetPieceToWork(Route.IP);
                if (individualTask != null)
                {
                    individualTask.SentItem();
                    MapMessage mapMessage = new MapMessage();
                    mapMessage.MapDLL = taskToWork.WorkDLL;
                    mapMessage.Piece = taskToWork.position;
                    mapMessage.Port = ManagerActor.LocalPort;
                    mapMessage.TaskNumber = taskToWork.ID;
                    mapMessage.Data = individualTask.DataObject;
                    new NetworkResponseActor(mapMessage, Route.Port, Route.IP).Start();
                    Console.WriteLine("Sent a Map job out.");
                    sentMessage = true;
                }
            }
            return sentMessage;
        }

        public bool SendMapReduce(Routing Route)
        {
            bool sentMessage = false;
            if (CurrentTasks.Count() > 0)
            {
                //Console.WriteLine("Current Mapped Count : " + CurrentTasks.tasks.First().Value.task.Count());
                TaskInfoCurrentlyWorking ticw = CurrentTasks.GetTaskInfo();
                MappedAnswer work = ticw.GetPieceToWork();
                MappedAnswer work2 = ticw.GetPieceToWork();
                // i got rid of the check if work comes back and work2 doesn't........
                if (work != null)
                {
                    work.Sent();
                    work.isFromMapPart = true;
                    if (CheckReduceIDs(work, work2) && work != work2)
                    {
                        work2.isFromMapPart = true;
                    }
                    else
                    {
                        ReduceList.AddNew(ticw.ID, work);
                        work2 = ticw.MatchReduceKey(work.ReduceKey, work);
                        
                        if (work2 != null)
                        {
                            work2.isFromMapPart = true;
                        }
                    }
                    Console.WriteLine("Sent Reduce from Mapped.");
                    SendWork(new MappedAnswer[] { work, work2 }, ticw, Route);
                    sentMessage = true;
                }
                if (OriginalTasks.Count() > 0)
                {
                    OriginalTasks.CheckForDone(ticw.ID);
                }
            }
            return sentMessage;
        }

        public bool SendReduce(Routing Route)
        {
            bool sentMessage = false;
            if(ReduceTasks.Count() > 0)
            {
                //Console.WriteLine("Reduce Count --------------- : " + ReduceTasks.tasks.First().Value.task.Count());
                TaskInfoCurrentlyWorking ticw = ReduceTasks.GetTaskInfo();
                MappedAnswer work = ticw.GetPieceToWork();
                MappedAnswer work2 = ticw.GetPieceToWork();            

                if(work != null && work2 != null)
                {
                    work.Sent();
                
                    if (CheckReduceIDs(work, work2) && work != work2)
                    {
                        SendWork(new MappedAnswer[] { work, work2 }, ticw, Route);
                    }
                    else
                    {
                        work2 = ticw.MatchReduceKey(work.ReduceKey, work);

                        if (work2 != null)
                        {
                            SendWork(new MappedAnswer[] { work, work2 }, ticw, Route);
                        }   
                    }
                    Console.WriteLine("Sent Reduce Message.");
                    sentMessage = true;
                }
                CheckReduceTasksForFinished(ticw.ID);
            }
            return sentMessage;
        }

        private void SendWork(MappedAnswer[] ma, TaskInfoCurrentlyWorking ticw, Routing Route)
        {
            ReduceList.AddNew(ticw.ID, ma[0]);
            if (ma[1] != null)
            {
                ma[1].Sent();
                ma[1].ReduceKey = ma[0].ReduceKey;
            }
            CreateAndSendMessage(ma, ticw, Route);
        }

        private void CreateAndSendMessage(MappedAnswer[] ma, TaskInfoCurrentlyWorking ticw, Routing Route)
        {
            ReduceMessage rm = new ReduceMessage();
            rm.Port = ManagerActor.LocalPort;
            rm.ReduceDLL = ticw.WorkDLL;
            rm.TaskID = ticw.ID;
            rm.Data = ma;
            rm.ReduceID = ma[0].ReduceKey;
            new NetworkResponseActor(rm, Route.Port, Route.IP).Start();
        }

        public void CheckReduceTasksForFinished(long ID)
        {
            //if (this.ReduceTasks.tasks[ID].task.Count() > 0)
            //    Console.WriteLine(this.ReduceTasks.tasks[ID].task[0].ReduceKey);

            if (CurrentTasks.Finished(ID) && ReduceTasks.Finished(ID) && ReduceList.Complete(ID))
            {
                MappedAnswer answer = ReduceTasks.GetAnswer(ID);
                //Console.WriteLine(answer.Value);
                if (answer != null)
                {
                    FinishedTasks.AddFinishedWork(ID, new ReduceResult(answer.Key, answer.Value));
                    CurrentTasks.Remove(ID);
                    if(FinishedTasks.finishedWork.ContainsKey(ID))
                        ReduceTasks.Remove(ID);
                }
            }
        }

        private bool CheckReduceIDs(MappedAnswer work, MappedAnswer work2)
        {
            bool bothOK = false;
            
            if((work2 != null && work.ReduceKey == long.MaxValue && work2.ReduceKey == long.MaxValue) || (work2 != null && work.ReduceKey == work2.ReduceKey))
            {
                bothOK = true;
            }
            return bothOK;
        }

        public void AddSomeFinishedWork(long ID, int Piece, MappedAnswer[] results)
        {
            if (!this.OriginalTasks.PieceHasBeenWorked(ID, Piece))
            {
                this.CurrentTasks.AddCompletedWork(ID, results);
            }      
        }

        public void AddFinishedWork(long ID, long ReduceID, MappedAnswer[] work)
        {
            if (this.ReduceList.CheckAndSwitch(ID, ReduceID))
            {
                this.CurrentTasks.Remove(ID, ReduceID);
                this.ReduceTasks.Remove(ID, ReduceID);
                this.ReduceTasks.AddFinishedWork(ID, work);
            }            
        }

        public ReduceResult GetFinishedWork(long ID)
        {
            ReduceResult rr = this.FinishedTasks.GetFinishedWork(ID);
            return rr;
        }

        public void CheckThenDelete(long ID)
        {
            this.OriginalTasks.CheckForDone(ID);
        }
    }
}
