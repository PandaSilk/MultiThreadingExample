using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Diagnostics;

namespace MultiThreadingExample {
class Class1 {

	// CONFIG OF WORKER DETAILS
	public class ThreadDetails {
		public bool ISRUNNING { get; set; }
		public int ID { get; set; }
		public int RUNCOUNT { get; set; }
	}

	// CONFIG
	public static int MaxThreads = 10;
	public static bool RepeatProcess = true;
	public static List<ThreadDetails> TD = new List<ThreadDetails>();

// CHECK IF ANY WORK IS RUNNING
public static bool ThreadsAreRunning() {
	foreach (ThreadDetails T in TD) {
	if (T.ISRUNNING == true) { return true; }
	}
	return false;
}
	
// START WORK
public void Main() {

	// Create MaxThreads number of Thread managed details
	for (int i = 0; i < MaxThreads; i++) {
	ThreadDetails T = new ThreadDetails();
	T.ISRUNNING = false;
	T.ID = i;
	TD.Add(T);
	}

	// Start the Pool
	// ThreadPool.QueueUserWorkItem has better performance than Thread obj = new Thread(Process) method
	// Search internet for more info on this.
	ProcessPool();

	// Wait until all work is done before quiting
	// Check every 1 sec, if repeat is on, repeat the process for non-running works
	do {
	if (RepeatProcess == true) { ProcessPool(); }
	Thread.Sleep(1000);
	} while (ThreadsAreRunning() == true);

}

// SETUP WORK
private static void ProcessPool() {
	foreach (ThreadDetails T in TD) {
	if (T.ISRUNNING == false) {
	TD[T.ID].ISRUNNING = true;
	ThreadPool.QueueUserWorkItem(new WaitCallback(ProcessWork), T);
	}
	}
}

// DO WORK AND END
private static void ProcessWork(object callback) {
	// Pass details to Processing Function
	ThreadDetails T = (ThreadDetails)callback;

	// Get some random Sleeptime to simulate work beging done, like processing files, connecting to DBs and quering data, creating/copying files.
	Random RND = new Random();
	int SleepTime = (1000 * RND.Next(1, 20));

	// Show the work start and end
	//TD[T.ID].ISRUNNING = true;
	Console.WriteLine("ID:" + T.ID + ",["+TD[T.ID].RUNCOUNT+"] START, SLEEPTIME:" + SleepTime);
	Thread.Sleep(SleepTime);
	Console.WriteLine("ID:" + T.ID + ",["+TD[T.ID].RUNCOUNT+"] END");
	TD[T.ID].RUNCOUNT++;
	TD[T.ID].ISRUNNING = false;
}

}
}
